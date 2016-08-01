<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageCMSCategories.aspx.vb" Inherits="Meta_ManageCMSCategories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <telerik:RadCodeBlock ID="rdCodeBlock" runat="server">
        <script>
            function openEditDialog(id) {
                openWindow('DialogCMSCategoryInfo.aspx?CategoryID=' + id, 'CMS Category Info', WINDOW_MEDIUM, dialogClosed);
            }

            function dialogClosed(sender, args) {
                var arg = args.get_argument();

                if (arg != null) {
                    $find('<%= rdAjaxManager.ClientID%>').ajaxRequest();
                }
            }

        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />
    <telerik:RadAjaxManager ID="rdAjaxManager" runat="server" DefaultLoadingPanelID="rdLoadingPanel">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="clCMSCategories:CardListPanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clCMSCategories:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdAjaxManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clCMSCategories:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <miles:MilesSearchPanel ID="pnlSearch" runat="server" ListControlID="clCMSCategories">

        <ContentPanel>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="lblCategoryName" runat="server" Text="Category Name" AssociatedControlID="txtCategoryName" />
                    <asp:TextBox ID="txtCategoryName" runat="server" MaxLength="500" />
                </div>
                 <div class="col-md-2">
                    <asp:Label ID="Label2" runat="server" Text="Content Type" AssociatedControlID="ddlContentType" />
                    <miles:MilesTypeComboBox ID="ddlContentType" runat="server" StatusType="CMSContentType" IncludeDefaultItem="false"
                        CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="All" />
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblMergeFields" runat="server" Text="Merge Fields" AssociatedControlID="ddlMergeFields" />
                    <telerik:RadComboBox ID="ddlMergeFields" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                        EmptyMessage="All" />
                </div>

                <div class="col-md-2">
                    <br />
                    <asp:CheckBox ID="chkArchived" runat="server" Text="Include archived" CssClass="chk-inline" />
                </div>

            </div>
        </ContentPanel>

    </miles:MilesSearchPanel>

    <hr />
     <miles:MilesCardList ID="clCMSCategories" AutoBind="true" runat="server" DataKeyNames="CategoryID"
            AddNewRecordText="Add New Category" OnAddNewRecordClientClick="return openEditDialog(0); return false;">

        <SortExpressionList>
            <miles:MilesCardListSortExpression DisplayName="Category Name" FieldName="CategoryName" />
        </SortExpressionList>

        <CardTemplate HeaderLinkType="HyperLink" HeaderNavigateUrlFormatString="javascript:openEditDialog('{0}');" HeaderNavigateUrlFields="CategoryID">
            <CardHeaderTemplate>
                <h4>
                    <%# Eval("CategoryName")%>
                    <asp:Image ID="lnkEdit" CssClass="pull-right" runat="server" ToolTip="Edit" ImageUrl="/Images/24x24/document-edit.png" />
                </h4>
            </CardHeaderTemplate>

            <CardItemTemplate>
                <div><%# Eval("ContentType")%></div>
            </CardItemTemplate>

            <CardActionTemplate>
                <miles:MilesToggleArchiveImageButton ID="btnDelete" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
            </CardActionTemplate>
        </CardTemplate>
    </miles:MilesCardList>
</asp:Content>

