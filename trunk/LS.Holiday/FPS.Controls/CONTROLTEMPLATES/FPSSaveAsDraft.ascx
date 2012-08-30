<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" %>

<SharePoint:RenderingTemplate ID="FPSSaveAsDraftButton" runat="server">
	<Template>
		<table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,multipages_direction_right_align_value%>' EncodeMethod='HtmlEncode'/>" width="100%" nowrap="nowrap">
			        <asp:Button UseSubmitBehavior="false" ID="diidIOSaveItem" CommandName="FPSSaveDraft" CommandArgument="SaveAsDraft" Text="Save as Draft" class="ms-ButtonHeightWidth2" accesskey="<%$Resources:wss,tb_saveasdraft_AK%>" target="_self" runat="server"/>
		        </td>
            </tr>
        </table>
	</Template>
</SharePoint:RenderingTemplate>