using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FPS.Core.QueryBuilder.Interfaces;
using Microsoft.SharePoint;

namespace FPS.Core.QueryBuilder.QuerySchemaElements.Base
{
    /// <summary>
    /// CamlQuery class.
    /// </summary>
    public class CamlQuery : IQueryElement
    {
        #region Fields

        private const string ViewFieldsElementName = "ViewFields";
        private const string FieldRefElementName = "FieldRef";
        private const string NameAttributeName = "Name";
        private const string QueryElementName = "Query";
        private const string WhereElementName = "Where";
        private const string GroupByElementName = "GroupBy";
        private const string ViewElementName = "View";
        private const string RowLimitElementName = "RowLimit";
        private const string PagedAttributeName = "Paged";
        private const string CollapseAttributeName = "Collapse";
        private XDocument _schema = new XDocument();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public string View
        {
            get
            {
                if (_schema.Root == null || _schema.Root.Name != ViewElementName)
                    _schema = new XDocument(new XElement(ViewElementName, _schema.Elements()));

                return _schema.Element(ViewElementName).ToString();
            }

            set
            {
                var element = XElement.Parse(value);
                if (element.Name != ViewElementName)
                    element = new XElement(ViewElementName, element);

                _schema = new XDocument(element);
            }
        }

        /// <summary>
        /// Gets or sets the caml query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        public string Query
        {
            get
            {
                var queryElement = XElement.Parse(View).Element(QueryElementName);
                return queryElement == null ? new XElement(QueryElementName).ToString() : queryElement.ToString();
            }

            set
            {
                var quetyElement = XElement.Parse(value);
                if (quetyElement.Name != QueryElementName)
                    quetyElement = new XElement(QueryElementName, quetyElement);

                var currentViewElement = XElement.Parse(View);
                var currentQueryElement = currentViewElement.Element(QueryElementName);
                if (currentQueryElement == null)
                    currentViewElement.Add(quetyElement);
                else
                    currentViewElement.Element(QueryElementName).ReplaceWith(quetyElement);

                View = currentViewElement.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the where.
        /// </summary>
        /// <value>
        /// The where.
        /// </value>
        public string Where
        {
            get
            {
                var whereElement = XElement.Parse(Query).Element(WhereElementName);
                return whereElement == null ? new XElement(WhereElementName).ToString() : whereElement.ToString();
            }

            set
            {
                var whereElement = XElement.Parse(value);
                if (whereElement.Name != WhereElementName)
                    whereElement = new XElement(WhereElementName, whereElement);

                var currentQueryElement = XElement.Parse(Query);
                var currentWhereElement = currentQueryElement.Element(WhereElementName);
                if (currentWhereElement == null)
                    currentQueryElement.Add(whereElement);
                else
                    currentQueryElement.Element(WhereElementName).ReplaceWith(whereElement);

                Query = currentQueryElement.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the group by.
        /// </summary>
        /// <value>
        /// The group by.
        /// </value>
        public GroupBy GroupBy
        {
            get
            {
                return GroupBy.GetGroupBy(this);
            }

            set
            {
                if (value == null)
                    GroupBy.RemoveGroupBy(this);
                else
                    value.SetParent(this);
            }
        }

        /// <summary>
        /// Gets or sets the row limit.
        /// </summary>
        /// <value>
        /// The row limit.
        /// </value>
        public uint RowLimit
        {
            get
            {
                var element = XElement.Parse(View).Element(RowLimitElementName);
                return (element != null) ? (uint)Math.Abs(SafeParser.ToInt32(element.Value)) : 0;
            }

            set
            {
                var viewElement = XElement.Parse(View);
                var rowLimitElement = viewElement.Element(RowLimitElementName);
                if (rowLimitElement == null)
                {
                    var rowLimit = new XElement(RowLimitElementName);
                    rowLimit.Add(new XAttribute(PagedAttributeName, "TRUE"));
                    rowLimit.Value = value.ToString();
                    viewElement.Add(rowLimit);
                }
                else
                    rowLimitElement.Value = value.ToString();

                View = viewElement.ToString();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is recursive.
        /// </summary>
        /// <value>
        /// <c>True</c> if this instance is recursive; otherwise, <c>false</c>.
        /// </value>
        public bool IsRecursive { get; set; }

        /// <summary>
        /// Gets the valid fields.
        /// </summary>
        public List<string> ValidFields { get; private set; }

        /// <summary>
        /// Gets the invalid fields.
        /// </summary>
        public List<string> InvalidFields { get; private set; }

        /// <summary>
        /// Gets the view fields.
        /// </summary>
        public List<string> ViewFields
        {
            get
            {
                return XDocument.Parse(Query).Descendants(ViewFieldsElementName).Elements().Select(element => (string)element.Attribute(NameAttributeName)).ToList();
            }
        }

        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        /// <value>
        /// The list.
        /// </value>
        public SPList List { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CamlQuery class.
        /// </summary>
        public CamlQuery()
        {
            ValidFields = new List<string>();
            InvalidFields = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CamlQuery"/> class.
        /// </summary>
        /// <param name="schema">The schema.</param>
        public CamlQuery(string schema)
            : this()
        {
            var document = XDocument.Parse(schema);
            switch (document.Root.Name.ToString())
            {
                case ViewElementName:
                    _schema = document;
                    break;
                case QueryElementName:
                    Query = document.ToString();
                    break;
                case WhereElementName:
                    Where = document.ToString();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Query;
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>
        /// Returns query.
        /// </returns>
        public CamlQuery GetQuery(SPList list = null)
        {
            List = list;
            return new CamlQuery() { _schema = _schema, List = list };
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns>Returns SPListItemCollection.</returns>
        public SPListItemCollection GetItems()
        {
            if (List != null)
            {
                var newSPQuery = new SPQuery();
                if (IsRecursive)
                    newSPQuery.ViewAttributes = "Scope=\"Recursive\"";

                if (RowLimit != 0)
                    newSPQuery.RowLimit = RowLimit;

                newSPQuery.Query = Where;

                return List.GetItems(newSPQuery);
            }
            else
                return null;
        }

        #endregion
    }
}
