<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageToolTips.aspx.vb" Inherits="Meta_ManageToolTips" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
    <telerik:RadCodeBlock ID="rdCodeBlock" runat="server">
        <script>
            function openEditDialog(id) {
                openWindow('/Meta/DialogToolTipInfo.aspx?ToolTipID=' + id, 'ToolTip Info', WINDOW_LARGE, dialogClosed);
            }

            function dialogClosed(sender, args) {
                var arg = args.get_argument();

                if (arg != null) {
                    $find('<%= rdAjaxManager.ClientID%>').ajaxRequest();
                }
            }

        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />
    <telerik:RadAjaxManager ID="rdAjaxManager" runat="server" DefaultLoadingPanelID="rdLoadingPanel">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="clToolTips:CardListPanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clToolTips:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="rdAjaxManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clToolTips:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <miles:MilesSearchPanel ID="pnlSearch" runat="server" ListControlID="clToolTips">

        <ContentPanel>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="lblToolTip" runat="server" Text="ToolTip" AssociatedControlID="txtToolTip" />
                    <asp:TextBox ID="txtToolTip" runat="server" />
                </div>
            </div>
        </ContentPanel>

    </miles:MilesSearchPanel>
    <hr />
    <miles:MilesCardList ID="clToolTips" AutoBind="true" runat="server" DataKeyNames="ToolTipID"
        AddNewRecordText="Add New ToolTip" OnAddNewRecordClientClick="return openEditDialog(0); return false;" >
        <SortExpressionList>
            <miles:MilesCardListSortExpression DisplayName="ToolTip Name" FieldName="ToolTipName" />
        </SortExpressionList>

        <CardTemplate HeaderLinkType="HyperLink" HeaderNavigateUrlFormatString="javascript:openEditDialog('{0}');" HeaderNavigateUrlFields="ToolTipID">
            <CardHeaderTemplate>
                <h4>
                    <%# Eval("ToolTipName")%>
                    <asp:Image ID="lnkEdit" CssClass="pull-right" runat="server" ToolTip="Edit" ImageUrl="/Images/24x24/document-edit.png" />
                </h4>
            </CardHeaderTemplate>

            <CardItemTemplate>
                <div><%# if(Eval("ToolTipDesc").ToString().Length >200, Eval("ToolTipDesc").ToString.Substring(0,199) & "...",Eval("ToolTipDesc")) %></div>
            </CardItemTemplate>

            <CardActionTemplate>

            </CardActionTemplate>
        </CardTemplate>
    </miles:MilesCardList>
</asp:Content>

