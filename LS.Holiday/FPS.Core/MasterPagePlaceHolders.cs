using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPS.Core
{
    public class MasterPagePlaceHolders
    {
        #region Fields

        /// <summary>
        ///  Used to add extra components such as JavaScript, Jscript, and Cascading Style Sheets in the head section of the page.
        /// </summary>
        public const string AdditionalPageHead = "PlaceHolderAdditionalPageHead";

        /// <summary>
        ///  The class of the body area. This  is no longer used in SharePoint 2010.
        /// </summary>
        public const string BodyAreaClass = "PlaceHolderBodyAreaClass";

        /// <summary>
        ///  This  does not appear as part of the interface but must be present for backward compatibility.
        /// </summary>
        public const string BodyLeftBorder = "PlaceHolderBodyLeftBorder";

        /// <summary>
        ///  This  does not appear as part of the interface but must be present for backward compatibility.
        /// </summary>
        public const string BodyRightMargin = "PlaceHolderBodyRightMargin";

        /// <summary>
        ///  The date picker used when a calendar is visible on the page.
        /// </summary>
        public const string CalendarNavigator = "PlaceHolderCalendarNavigator";

        /// <summary>
        ///  The container where the page form digest control is stored.
        /// </summary>
        public const string FormDigest = "PlaceHolderFormDigest";

        /// <summary>
        ///  The global navigation breadcrumb control on the page.
        /// </summary>
        public const string GlobalNavigation = "PlaceHolderGlobalNavigation";

        /// <summary>
        ///  The list of sub-sites and sibling sites in the global navigation on the page.
        /// </summary>
        public const string GlobalNavigationSiteMap = "PlaceHolderGlobalNavigationSiteMap";

        /// <summary>
        ///  The navigation menu that is inside the top navigation bar.
        /// </summary>
        public const string HorizontalNav = "PlaceHolderHorizontalNav";

        /// <summary>
        ///  The additional objects above the Quick Launch bar.
        /// </summary>
        public const string LeftActions = "PlaceHolderLeftActions";

        /// <summary>
        ///  The Quick Launch navigation bar.
        /// </summary>
        public const string LeftNavBar = "PlaceHolderLeftNavBar";

        /// <summary>
        ///  This  does not appear as part of the interface but must be present for backward compatibility.
        /// </summary>
        public const string LeftNavBarBorder = "PlaceHolderLeftNavBarBorder";

        /// <summary>
        ///  The placement of the data source used to populate the left navigation bar.
        /// </summary>
        public const string LeftNavBarDataSource = "PlaceHolderLeftNavBarDataSource";

        /// <summary>
        ///  The top section of the left navigation bar.
        /// </summary>
        public const string LeftNavBarTop = "PlaceHolderLeftNavBarTop";

        /// <summary>
        ///  The main content of the page.
        /// </summary>
        public const string Main = "PlaceHolderMain";

        /// <summary>
        ///  This  does not appear as part of the interface but must be present for backward compatibility.
        /// </summary>
        public const string MiniConsole = "PlaceHolderMiniConsole";

        /// <summary>
        ///  This  does not appear as part of the interface but must be present for backward compatibility.
        /// </summary>
        public const string NavSpacer = "PlaceHolderNavSpacer";

        /// <summary>
        ///  Description of the current page.
        /// </summary>
        public const string PageDescription = "PlaceHolderPageDescription";

        /// <summary>
        ///  This  does not appear as part of the interface but must be present for backward compatibility.
        /// </summary>
        public const string PageImage = "PlaceHolderPageImage";

        /// <summary>
        ///  The title of the site.
        /// </summary>
        public const string PageTitle = "PlaceHolderPageTitle";

        /// <summary>
        ///  The title of the page, which appears in the title area on the page.
        /// </summary>
        public const string PageTitleInTitleArea = "PlaceHolderPageTitleInTitleArea";

        /// <summary>
        ///  The top of the Quick Launch menu.
        /// </summary>
        public const string QuickLaunchTop = "PlaceHolderQuickLaunchTop";

        /// <summary>
        ///  The bottom of the Quick Launch menu.
        /// </summary>
        public const string QuickLaunchBottom = "PlaceHolderQuickLaunchBottom";

        /// <summary>
        ///  The section of the page for the search box and controls.
        /// </summary>
        public const string SearchArea = "PlaceHolderSearchArea";

        /// <summary>
        ///  The name of the site where the current page resides.
        /// </summary>
        public const string SiteName = "PlaceHolderSiteName";

        /// <summary>
        ///  Used for additional page editing controls.
        /// </summary>
        public const string SPNavigation = "SPNavigation";

        /// <summary>
        ///  The class for the title area. This control is now in the head tag and no longer used in SharePoint 2010.
        /// </summary>
        public const string TitleAreaClass = "PlaceHolderTitleAreaClass";

        /// <summary>
        ///  This  does not appear as part of the interface but must be present for backward compatibility.
        /// </summary>
        public const string TitleAreaSeparator = "PlaceHolderTitleAreaSeparator";

        /// <summary>
        ///  The breadcrumb text for the breadcrumb control.
        /// </summary>
        public const string TitleBreadcrumb = "PlaceHolderTitleBreadcrumb";

        /// <summary>
        ///  This  does not appear as part of the interface but must be present for backward compatibility.
        /// </summary>
        public const string TitleLeftBorder = "PlaceHolderTitleLeftBorder";

        /// <summary>
        ///  This  does not appear as part of the interface but must be present for backward compatibility.
        /// </summary>
        public const string TitleRightMargin = "PlaceHolderTitleRightMargin";

        /// <summary>
        ///  The container used to hold the top navigation bar.
        /// </summary>
        public const string TopNavBar = "PlaceHolderTopNavBar";

        /// <summary>
        ///  The additional content at the bottom of the page, outside the form tag.
        /// </summary>
        public const string UtilityContent = "PlaceHolderUtilityContent";

        #endregion
    }
}
