using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FPS.Core.QueryBuilder.Interfaces;

namespace FPS.Core.QueryBuilder.QuerySchemaElements.Base
{
    /// <summary>
    /// GroupBy class.
    /// </summary>
    public class GroupBy
    {
        #region Fields

        private const string GroupByElementName = "GroupBy";
        private const string FieldRefElementName = "FieldRef";
        private const string CollapseAttributeName = "Collapse";
        private const string NameAttributeName = "Name";
        private CamlQuery _parent = null;
        private string _fieldName = string.Empty;
        private bool _isCollapsed = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName
        {
            get { return _fieldName; }
            set
            {
                _fieldName = value;
                SetParentValues();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is collapse.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is collapse; otherwise, <c>false</c>.
        /// </value>
        public bool IsCollapsed
        {
            get { return _isCollapsed; }
            set
            {
                _isCollapsed = value;
                SetParentValues();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the parent values.
        /// </summary>
        internal void SetParentValues()
        {
            if (_parent == null)
                return;

            XElement groupByElement = null;

            if (!_fieldName.IsNullOrEmpty())
            {
                groupByElement = new XElement(GroupByElementName);
                groupByElement.Add(new XElement(FieldRefElementName, new XAttribute(NameAttributeName, _fieldName)));
                groupByElement.Add(new XAttribute(CollapseAttributeName, _isCollapsed));
            }

            var currentQueryElement = XElement.Parse(_parent.Query);
            var currentWhereElement = currentQueryElement.Element(GroupByElementName);

            if (currentWhereElement == null && groupByElement != null)
            {
                currentQueryElement.Add(groupByElement);
            }
            else if (groupByElement != null && currentQueryElement != null)
            {
                currentQueryElement.Element(GroupByElementName).ReplaceWith(groupByElement);
            }
            else if (groupByElement != null && currentQueryElement == null)
            {
                currentQueryElement.Element(GroupByElementName).Remove();
            }

            _parent.Query = currentQueryElement.ToString();
        }

        /// <summary>
        /// Sets the parent.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public void SetParent(CamlQuery parent)
        {
            _parent = parent;
            SetParentValues();
        }

        /// <summary>
        /// Gets the parent values.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns>Returns <c>true</c> if this element exists; otherwise, <c>false</c>.</returns>
        public static GroupBy GetGroupBy(CamlQuery parent)
        {
            if (parent == null)
                return null;

            var qroupByElement = XElement.Parse(parent.Query).Element(GroupByElementName);
            if (qroupByElement == null)
                return null;

            var isCollapsed = false;
            var fieldName = string.Empty;

            bool.TryParse(qroupByElement.Attribute(CollapseAttributeName).Value, out isCollapsed);

            var fieldRef = qroupByElement.Element(FieldRefElementName);
            if (fieldRef == null)
                return null;

            fieldName = fieldRef.Attribute(NameAttributeName).Value;

            var groupBy = new GroupBy();
            groupBy._parent = parent;
            groupBy._fieldName = fieldName;
            groupBy._isCollapsed = isCollapsed;

            return groupBy;
        }

        /// <summary>
        /// Removes the group by.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public static void RemoveGroupBy(CamlQuery parent)
        {
            var currentQueryElement = XElement.Parse(parent.Query);
            var groupByElement = currentQueryElement.Element(GroupByElementName);

            if (groupByElement != null)
                currentQueryElement.Element(GroupByElementName).Remove();
        }

        #endregion
    }
}
