using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using FPS.Core;
using FPS.Core.Enums;
using FPS.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

namespace FPS.Controls
{
    [Guid("7e0e1a5f-27cd-474b-8f73-912260f941e9")]
    public class RelationFieldControl : TextField
    {
        #region Fields

        private const string PrimaryKeyColumnName = "PrimaryKeyColumnName";
        private const string LookupColumnName = "LookupColumnName";
        private const string RelatedListName = "RelatedListName";
        private const string ViewName = "ViewName";
        private const string EmptyViewIndicator = "ms-emptyView";
        private const string RequiredFieldErrorMessage = "You must specify a value for this required field.";
        private const string AddNewItemMessage = "Add new item";

        private int _numberOfChildren = 0;
        private string _primaryKeyValue = string.Empty;
        private LiteralControl _literalControl = new LiteralControl();
        private LiteralControl _totalsControl = new LiteralControl();
        private HyperLink _linkAddNew = new HyperLink();
        private SPList _relatedList;
        private SPField _lookupField;
        private string _viewName;
        private bool _isEmpty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        public string PrimaryKeyValue
        {
            get
            {
                if (_primaryKeyValue.IsNullOrEmpty())
                {
                    var primaryKeyColumnName = Field.GetCustomProperty(PrimaryKeyColumnName) as string;

                    if (!string.IsNullOrEmpty(primaryKeyColumnName))
                    {
                        if (Item.Fields.ContainsField(primaryKeyColumnName))
                        {
                            _primaryKeyValue = Convert.ToString(Item[primaryKeyColumnName]);

                            if (_primaryKeyValue.IsLookupField())
                                _primaryKeyValue = new SPFieldLookupValue(_primaryKeyValue).LookupId.ToString();
                        }
                    }
                    else
                    {
                        _primaryKeyValue = Item.ID.ToString();
                    }
                }

                return _primaryKeyValue;
            }
        }

        /// <summary>
        /// Gets the lookup field.
        /// </summary>
        public SPField LookupField
        {
            get
            {
                if (_lookupField == null)
                {
                    var lookupColumnName = Field.GetCustomProperty(LookupColumnName) as string;

                    if (lookupColumnName.IsNullOrEmpty())
                    {
                        foreach (SPField field in _relatedList.Fields)
                        {
                            if (field is SPFieldLookup && SPContext.Current.List.ID == new Guid((field as SPFieldLookup).LookupList))
                            {
                                _lookupField = field;
                                break;
                            }
                        }
                    }
                    else
                    {
                        _lookupField = RelatedList.Fields[lookupColumnName];
                    }
                }

                return _lookupField;
            }
        }

        /// <summary>
        /// Gets the related list.
        /// </summary>
        public SPList RelatedList
        {
            get
            {
                if (_relatedList == null)
                {
                    var relatedListName = Field.GetCustomProperty(RelatedListName) as string;
                    try
                    {
                        _relatedList = SPContext.Current.Web.Lists[relatedListName];
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException(@"Child List " + relatedListName + " doesn't exist at the site.", ex);
                    }
                }

                return _relatedList;
            }
        }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        public string ListViewName
        {
            get
            {
                if (_viewName.IsNullOrEmpty())
                {
                    _viewName = Field.GetCustomProperty(ViewName) as string;
                }

                return _viewName;
            }
        }

        #endregion

        #region Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                var outputHtml = string.Empty;
                try
                {
                    string customizationViewName = Field.GetCustomizationValue<string>(CustomPropertyType.ViewName);
                    var view = !customizationViewName.IsNullOrEmpty() ? RelatedList.Views[customizationViewName] : ListViewName.IsNullOrEmpty() ? RelatedList.DefaultView : RelatedList.Views[ListViewName];

                    var queryContent = string.Format(
                        @"<Eq>
                            <FieldRef Name='{0}' LookupId='true' />
                            <Value Type='{1}'>{2}</Value>
                        </Eq>",
                        LookupField.InternalName,
                        LookupField.TypeAsString,
                        PrimaryKeyValue);

                    view.Query = !view.Query.IsNullOrEmpty() && view.Query.Contains("<Where>") ?
                        view.Query.Replace("<Where>", "<Where><And>" + queryContent).Replace("</Where>", "</And></Where>") :
                        string.Format("<Where>{0}</Where>{1}", queryContent, view.Query);

                    outputHtml = view.RenderAsHtmlWithAggregations();

                    if (outputHtml.Contains(EmptyViewIndicator))
                    {
                        outputHtml = string.Empty;
                        _isEmpty = true;
                    }

                    IsValid = !(_isEmpty && Field.Required);
                }
                catch (Exception ex)
                {
                    outputHtml = string.Format("<p>There was an error loading the list information:<br />{0}</p>", ex.Message);
                    Logger.Instance.Log(ex);
                }

                _literalControl.Text = outputHtml;

                if (this.ControlMode != SPControlMode.Display && !this.Field.ReadOnlyField)
                {
                    string linkUrl = string.Format("{0}?{1}={2}", RelatedList.DefaultNewFormUrl, SPEncode.UrlEncode(LookupField.Title), PrimaryKeyValue);
                    _linkAddNew.Text = AddNewItemMessage;
                    _linkAddNew.NavigateUrl = linkUrl;
                    _linkAddNew.Attributes.Add(
                        "onclick",
                        string.Format("javascript:NewItem2(event, '{0}'); javascript:return false;", linkUrl));
                }

                ScriptLink.RegisterScriptAfterUI(Page, "FPS.Controls.Utility.js", false);
                ScriptLink.RegisterOnDemand(Page, "FPS.Controls.Utility.js", false);
            }
            catch (Exception ex)
            {
                _literalControl.Text = ex.Message;
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (!IsValid && Page.IsPostBack && ControlMode != SPControlMode.New)
            {
                ErrorMessage = RequiredFieldErrorMessage;
                RenderValidationMessage(output);
            }

            _literalControl.RenderControl(output);
            _totalsControl.RenderControl(output);

            ScriptLink.RegisterDelayedExecutionScript(Page, "FPS.Controls.Utility.js", "block", "FPS.Utility.moveAggregationRowToBottom", "document");

            if (this.ControlMode != SPControlMode.Display && !this.Field.ReadOnlyField)
                _linkAddNew.RenderControl(output);
        }

        public override void UpdateFieldValueInItem()
        {
            EnsureChildControls();

            try
            {
                Value = _numberOfChildren;
                ItemFieldValue = Value;
            }
            catch (Exception ex)
            {
                Value = ex.Message;
            }
        }

        #endregion
    }
}
