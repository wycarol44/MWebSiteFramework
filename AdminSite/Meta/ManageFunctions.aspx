<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageFunctions.aspx.vb" Inherits="Meta_ManageFunctions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadCodeBlock ID="rdCodeBlock" runat="server">
        <script>
            function openEditDialog(id) {
                openWindow('/Meta/DialogFunctionInfo.aspx?FunctionID=' + id, 'Function Info', WINDOW_MEDIUM, dialogClosed);
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
            <telerik:AjaxSetting AjaxControlID="clFunctions:CardListPanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clFunctions:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="rdAjaxManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clFunctions:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <miles:MilesSearchPanel ID="pnlSearch" runat="server" ListControlID="clFunctions">

        <ContentPanel>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="lblFunctionName" runat="server" Text="Function Name" AssociatedControlID="txtFunctionName" />
                    <asp:TextBox ID="txtFunctionName" runat="server" />
                </div>

                <div class="col-md-2">
                    <asp:Label ID="Label4" runat="server" Text="Module" AssociatedControlID="txtModule" />
                    <asp:TextBox ID="txtModule" runat="server" />
                </div>

                <div class="col-md-2">
                    <asp:Label ID="Label1" runat="server" Text="Form" AssociatedControlID="txtForm" />
                    <asp:TextBox ID="txtForm" runat="server" />
                </div>

                <div class="col-md-2">
                    <br />
                    <asp:CheckBox ID="chkArchived" runat="server" Text="Include archived" CssClass="chk-inline" />
                </div>

            </div>
        </ContentPanel>

    </miles:MilesSearchPanel>
    <hr />
    <miles:MilesCardList ID="clFunctions" AutoBind="true" runat="server" DataKeyNames="FunctionID"
        AddNewRecordText="Add New Function" OnAddNewRecordClientClick="return openEditDialog(0); return false;" >
        <SortExpressionList>
            <miles:MilesCardListSortExpression DisplayName="Function Name" FieldName="FunctionName" />
            <miles:MilesCardListSortExpression DisplayName="Module Name" FieldName="MetaModule.ModuleName" />
        </SortExpressionList>

        <CardTemplate HeaderLinkType="HyperLink" HeaderNavigateUrlFormatString="javascript:openEditDialog('{0}');" HeaderNavigateUrlFields="FunctionID">
            <CardHeaderTemplate>
                <h4>
                    <%# Eval("FunctionName")%>
                    <asp:Image ID="lnkEdit" CssClass="pull-right" runat="server" ToolTip="Edit" ImageUrl="/Images/24x24/document-edit.png" />
                </h4>
            </CardHeaderTemplate>

            <CardItemTemplate>
                <div><%# Eval("MetaModule.ModuleName")%></div>
            </CardItemTemplate>

            <CardActionTemplate>
                <miles:MilesToggleArchiveImageButton ID="btnDelete" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
            </CardActionTemplate>
        </CardTemplate>
    </miles:MilesCardList>

   
</asp:Content>

