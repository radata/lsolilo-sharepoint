using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using FPS.Core;
using Microsoft.SharePoint;

namespace FPS.Controls
{
    public static class SPViewExtensions
    {
        /// <summary>
        /// Returns the SPView data as HTML with the "aggr" tbody element filled with aggregation values.
        /// </summary>
        /// <param name="view">The SPView object.</param>
        /// <returns>HTML with data rendered in table.</returns>
        public static string RenderAsHtmlWithAggregations(this SPView view)
        {
            var outputHtml = view.RenderAsHtml();

            var queryXml = new XmlDocument();
            queryXml.LoadXml(view.GetViewXml());

            var query = new SPQuery() { Query = queryXml.SelectSingleNode("//Where").OuterXml, ViewFields = queryXml.SelectSingleNode("//ViewFields").InnerXml };
            var dataTable = view.ParentList.GetItems(query).GetDataTable();
            if (dataTable != null)
            {
                var match = Regex.Match(outputHtml, "<tbody id=\"aggr\">.*</tbody>");
                if (match.Success)
                {
                    var aggregationMarkup = match.Value;
                    var document = new XmlDocument();
                    document.LoadXml(aggregationMarkup);
                    var cells = document.GetElementsByTagName("td");

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        var expression = view.GetAggregationString(column.ColumnName);
                        if (!expression.IsNullOrEmpty())
                        {
                            var value = dataTable.Compute(string.Format("{0}({1})", expression, column.ColumnName), string.Empty);
                            cells[column.Ordinal].InnerText = string.Format("{0} {1}", expression, value);
                        }
                    }

                    outputHtml = outputHtml.Replace(aggregationMarkup, document.OuterXml);
                }
            }

            return outputHtml;
        }
    }
}
