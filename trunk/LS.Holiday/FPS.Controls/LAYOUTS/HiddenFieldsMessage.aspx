<%@ Page language="C#" MasterPageFile="~/_layouts/v4.master" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	Information
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server" />
<asp:Content ContentPlaceHolderId="PlaceHolderPageImage" runat="server">
	<img src="/_layouts/images/blank.gif" width='1' height='1' alt="" />
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderLeftNavBar" runat="server" />
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
    <div style="padding: 4px 4px 4px 4px;">
        <table>
            <tr>
                <td style="vertical-align: top;"><img src="/_layouts/images/TPDL.gif"></td>
                <td class="ms-dlgLoadingText"><b>All fields are hidden in this form.</b></td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server" />
<asp:Content ContentPlaceHolderId="PlaceHolderTitleLeftBorder" runat="server" />
<asp:Content ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server" />
<asp:Content ContentPlaceHolderId="PlaceHolderBodyAreaClass" runat="server" />
<asp:Content ContentPlaceHolderId="PlaceHolderBodyLeftBorder" runat="server" />
<asp:Content ContentPlaceHolderId="PlaceHolderTitleRightMargin" runat="server" />
<asp:Content ContentPlaceHolderId="PlaceHolderBodyRightMargin" runat="server" />
<asp:Content ContentPlaceHolderId="PlaceHolderTitleAreaSeparator" runat="server"/>