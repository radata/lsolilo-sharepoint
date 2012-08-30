<%@ Control Language="C#" AutoEventWireup="false" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"
    Namespace="Microsoft.SharePoint.WebControls" %>
<%@ Register TagPrefix="ApplicationPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"
    Namespace="Microsoft.SharePoint.ApplicationPages.WebControls" %>
<%@ Register TagPrefix="SPHttpUtility" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"
    Namespace="Microsoft.SharePoint.Utilities" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>
<%@ Register TagPrefix="FPS" Assembly="$SharePoint.Project.AssemblyFullName$" Namespace="FPS.Controls" %>
<SharePoint:RenderingTemplate ID="FPSListForm" runat="server">
    <Template>
        <span id='part1'>
            <SharePoint:InformationBar runat="server" />
            <div id="listFormToolBarTop">
                <wssuc:ToolBar CssClass="ms-formtoolbar" ID="toolBarTbltop" RightButtonSeparator="&amp;#160;"
                    runat="server">
                    <Template_RightButtons>
                        <SharePoint:NextPageButton runat="server" />
                        <SharePoint:SaveButton runat="server" />
                        <SharePoint:GoBackButton runat="server" />
                    </Template_RightButtons>
                </wssuc:ToolBar>
            </div>
            <SharePoint:FormToolBar runat="server" />
            <SharePoint:ItemValidationFailedMessage runat="server" />
            <table class="ms-formtable" style="margin-top: 8px;" border="0" cellpadding="0" cellspacing="0"
                width="100%">
                <SharePoint:FolderFormFields runat="server" />
                <FPS:FPSListFieldIterator runat="server" TemplateName="FPSFieldsListFieldIterator"
                    ContentTypeTemplateName="FPSChangeContentType" />
                <SharePoint:ApprovalStatus runat="server" />
                <SharePoint:FormComponent TemplateName="AttachmentRows" runat="server" />
            </table>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="ms-formline">
                        <img src="/_layouts/images/blank.gif" width='1' height='1' alt="" />
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" width="100%" style="padding-top: 7px">
                <tr>
                    <td width="100%">
                        <SharePoint:ItemHiddenVersion runat="server" />
                        <SharePoint:ParentInformationField runat="server" />
                        <SharePoint:InitContentType runat="server" />
                        <wssuc:ToolBar CssClass="ms-formtoolbar" ID="toolBarTbl" RightButtonSeparator="&amp;#160;"
                            runat="server">
                            <Template_Buttons>
                                <SharePoint:CreatedModifiedInfo runat="server" />
                            </Template_Buttons>
                            <Template_RightButtons>
                                <FPS:FPSSaveButton runat="server" />
                                <SharePoint:GoBackButton runat="server" />
                            </Template_RightButtons>
                        </wssuc:ToolBar>
                    </td>
                </tr>
            </table>
        </span>
        <SharePoint:AttachmentUpload runat="server" />
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="FPSFieldsListFieldIterator" runat="server">
    <Template>
        <tr>
            <SharePoint:CompositeField runat="server" TemplateName="FPSCompositeField" />
        </tr>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="FPSCompositeField" runat="server">
    <Template>
        <td valign="top" width="250px" class="ms-formlabel">
            <SharePoint:FieldLabel runat="server" />
            <h3 class="ms-standardheader">
                <FPS:FPSFieldDescription runat="server" />
            </h3>
        </td>
        <td valign="top" class="ms-formbody">
            <FPS:FPSFormField runat="server" />
            <SharePoint:AppendOnlyHistory runat="server" />
        </td>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="FPSChangeContentType" runat="server">
    <Template>
        <tr>
            <td nowrap="true" valign="top" class="ms-formlabel">
                <asp:Label ID="DisplayName" runat="server" />
            </td>
            <td valign="top" class="ms-formbody">
                <asp:DropDownList ID="ContentTypeChoice" runat="server" />
                &nbsp;<br />
                <asp:Label ID="ContentTypeDescription" runat="server" />
            </td>
        </tr>
    </Template>
</SharePoint:RenderingTemplate>
