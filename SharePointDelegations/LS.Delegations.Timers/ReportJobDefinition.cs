using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using LS.Delegations.Generated;
using Microsoft.SharePoint.Utilities;
using System.Collections.ObjectModel;

namespace LS.Delegations.Timers
{
    class ReportJobDefinition : SPJobDefinition
    {
        public const string JobName = "ReportJobDefinition";

        #region Constructors
        public ReportJobDefinition()
            : base()
        {
        }

        public ReportJobDefinition(SPWebApplication webApp)
            : base(JobName, webApp, null, SPJobLockType.Job)
        {
            Title = "LS - Monthly Report";
        }
        #endregion

        public override void Execute(Guid targetInstanceId)
        {
            SPWebApplication webApp = this.Parent as SPWebApplication;
            SPSite site = webApp.Sites["sites/delegationsvs"];
            SPWeb web = site.RootWeb;
            SPList reportList = web.Lists[DelegationsLists.MonthlyReport.Name];
            SPList costList = web.Lists[DelegationsLists.Expenses.Name];
            SPList delegationsList = web.Lists[DelegationsLists.Delegations.Name];

            var items = costList.Items;

            UpdateReportsFromLastMonths(6, delegationsList, costList, reportList);
        }

        #region Private Methods
        private void UpdateReportsFromLastMonths(int months, SPList delegations, SPList costs, SPList report)
        {
            for (int i = 0; i < months; i++)
            {
                DateTime backDate = DateTime.Now.AddMonths(-i);
                UpdateDelegationReportForMonth(backDate.Month, backDate.Year, delegations, costs, report);
            }
        }

        private void UpdateDelegationReportForMonth(int month, int year, SPList delegations, SPList costs, SPList report)
        {
            DateTime reportStart = new DateTime(year, month, 1);
            DateTime reportEnd = new DateTime(year, month + 1, 1).AddDays(-1);

            SPListItemCollection delegationsForSelectedMonth = GetDelegationsFromMonth(delegations, reportStart, reportEnd);

            SPListItemCollection monthlyCosts = GetCostsFromMonth(costs, delegationsForSelectedMonth);

            UpdateReportForMonth(report, reportStart, monthlyCosts);
        }

        private static void UpdateReportForMonth(SPList report, DateTime reportStart, SPListItemCollection monthlyCosts)
        {
            double monthlySum = 0;
            foreach (SPListItem cost in monthlyCosts)
            {
                monthlySum += (double)cost["Cost"];
            }

            string reportsQueryString = string.Format(
                "<Where>" +
                    "<Eq>" +
                        "<FieldRef Name='{0}'></FieldRef>" +
                        "<Value Type='DateTime'>{1}</Value>" +
                    "</Eq>" +
                "</Where>",
                DelegationsFields.MonthIndicator.Name,
                SPUtility.CreateISO8601DateTimeFromSystemDateTime(reportStart));

            SPQuery monthlyReportQuery = new SPQuery();
            monthlyReportQuery.Query = reportsQueryString;

            SPListItemCollection monthlyReports = report.GetItems(monthlyReportQuery);

            if (monthlyReports.Count > 0)
            {
                var item = report.GetItemById((int)(monthlyReports[0]["ID"]));
                item[DelegationsFields.MonthlyCost] = monthlySum;
                item.Update();
            }
            else
            {
                var item = report.AddItem();
                item[DelegationsFields.MonthIndicator] = reportStart;
                item[DelegationsFields.MonthlyCost] = monthlySum;
                item.Update();
            }
        }

        private static SPListItemCollection GetCostsFromMonth(SPList costs, SPListItemCollection delegationsForSelectedMonth)
        {
            StringBuilder delegationIDBuilder = new StringBuilder();
            foreach (SPListItem delegationItem in delegationsForSelectedMonth)
            {
                delegationIDBuilder.AppendFormat("<Value Type='Lookup'>{0}</Value>", delegationItem["ID"]);
            }

            string costsForSelectedDelagationsQueryString = string.Format(
                "<Where>" +
                    "<In>" +
                        "<FieldRef Name='{0}' LookupId='TRUE'/>" +
                            "<Values>" +
                                "{1}" +
                            "</Values>" +
                    "</In>" +
                "</Where>",
                DelegationsFields.Delegation.Name,
                delegationIDBuilder);

            SPQuery costsForSelectedDelegationsQuery = new SPQuery();
            costsForSelectedDelegationsQuery.Query = costsForSelectedDelagationsQueryString;

            SPListItemCollection monthlyCosts = costs.GetItems(costsForSelectedDelegationsQuery);
            return monthlyCosts;
        }

        private static SPListItemCollection GetDelegationsFromMonth(SPList delegations, DateTime reportStart, DateTime reportEnd)
        {
            string delegationsForSelectedMonthQueryString = string.Format(
                "<Where>" +
                    "<And>" +
                        "<Geq>" +
                            "<FieldRef Name='{0}'/>" +
                            "<Value Type='DateTime'>{1}</Value>" +
                        "</Geq>" +
                        "<Leq>" +
                            "<FieldRef Name='{0}'/>" +
                            "<Value Type='DateTime'>{2}</Value>" +
                        "</Leq>" +
                    "</And>" +
                "</Where>",
                DelegationsFields.Start.Name,
                SPUtility.CreateISO8601DateTimeFromSystemDateTime(reportStart),
                SPUtility.CreateISO8601DateTimeFromSystemDateTime(reportEnd));

            SPQuery delegationsForSelectedMonthQuery = new SPQuery();
            delegationsForSelectedMonthQuery.Query = delegationsForSelectedMonthQueryString;
            SPListItemCollection delegationsForSelectedMonth = delegations.GetItems(delegationsForSelectedMonthQuery);
            return delegationsForSelectedMonth;
        }
        #endregion
    }
}
