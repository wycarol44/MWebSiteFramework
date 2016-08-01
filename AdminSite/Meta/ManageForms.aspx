<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageForms.aspx.vb" Inherits="Meta_ManageForms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadCodeBlock ID="rdCodeBlock" runat="server">
        <script>
            function openEditDialog(id) {
                openWindow('/Meta/DialogFormInfo.aspx?MetaFormID=' + id, 'Form Info', WINDOW_MEDIUM, dialogClosed);
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
            <telerik:AjaxSetting AjaxControlID="clForms:CardListPanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clForms:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdAjaxManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clForms:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <miles:MilesSearchPanel ID="pnlSearch" runat="server" ListControlID="clForms">
        <ContentPanel>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="lblFormName" runat="server" Text="Form Name" AssociatedControlID="txtFormName" />
                    <asp:TextBox ID="txtFormName" runat="server" />
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblFormPath" runat="server" Text="Form Path" AssociatedControlID="txtFormPath" />
                    <asp:TextBox ID="txtFormPath" runat="server" />
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblModule" runat="server" Text="Module" AssociatedControlID="txtModule" />
                    <asp:TextBox ID="txtModule" runat="server" />
                </div>
                <div class="col-md-2">
                    <br>
                    <asp:CheckBox ID="chkCanBeFavourite" runat="server" Checked="true" Text="Include Can Be Favorite" CssClass="chk-inline" />
                </div>
            </div>
        </ContentPanel>
    </miles:MilesSearchPanel>
    <hr />

    <miles:MilesCardList ID="clForms" AutoBind="true" runat="server" DataKeyNames="FormID"
        ShowAddNewRecordButton="false" >
        <SortExpressionList>
            <miles:MilesCardListSortExpression DisplayName="Form Name" FieldName="FormName" />
            <miles:MilesCardListSortExpression DisplayName="Form Path" FieldName="FormPath" />
            <miles:MilesCardListSortExpression DisplayName="Module Name" FieldName="MetaModule.ModuleName" />
        </SortExpressionList>

        <CardTemplate HeaderLinkType="HyperLink" HeaderNavigateUrlFormatString="javascript:openEditDialog('{0}');" HeaderNavigateUrlFields="FormID">
            <CardHeaderTemplate>
                <h4>
                    <%# Eval("FormName")%>
                    <asp:Image ID="lnkEdit" CssClass="pull-right" runat="server" ToolTip="Edit" ImageUrl="/Images/24x24/document-edit.png" />
                </h4>
            </CardHeaderTemplate>

            <CardItemTemplate>
                <div><%# Eval("FormPath")%></div>
                <div><%# Eval("MetaModule.ModuleName")%></div>
            </CardItemTemplate>

            <CardActionTemplate>
                <asp:ImageButton ID="btnCanBeFavorite" runat="server" Font-Underline="false" CommandName="ToggleFavorite" ImageUrl="~/Images/24x24/favorite.png" ToolTip="Click to Toggle"></asp:ImageButton>
            </CardActionTemplate>
        </CardTemplate>
    </miles:MilesCardList>
</asp:Content>

