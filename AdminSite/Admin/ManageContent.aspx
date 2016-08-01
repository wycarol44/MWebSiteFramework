<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageContent.aspx.vb" Inherits="Admin_ManageContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <telerik:RadCodeBlock ID="rdCodeBlock" runat="server">
        <script>
            function openEditDialog(id) {
                openWindow('/Admin/DialogContentInfo.aspx?CategoryID=' + id, 'Edit Content', WINDOW_LARGE);
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />
    <telerik:RadAjaxManager ID="rdAjaxManagerProxy" runat="server" DefaultLoadingPanelID="rdLoadingPanel">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="clContent:cardListPanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clContent:cardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <miles:MilesSearchPanel ID="pnlSearch" runat="server" ListControlID="clContent" SearchOnLoadSavedState="true">
        <ContentPanel>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="Label1" runat="server" Text="Category Name" AssociatedControlID="txtCategory" />
                    <asp:TextBox ID="txtCategory" runat="server" />
                </div>
                <div class="col-md-2">
                    <asp:Label ID="Label2" runat="server" Text="Content Type" AssociatedControlID="ddlContentType" />
                    <miles:MilesTypeComboBox ID="ddlContentType" runat="server" StatusType="CMSContentType" IncludeDefaultItem="false"
                        CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="All" />
                </div>
                <div class="col-md-2">
                    <br />
                    <asp:CheckBox ID="chkArchived" runat="server" Text="Include archived" CssClass="chk-inline" />
                </div>
            </div>
        </ContentPanel>
    </miles:MilesSearchPanel>
    <hr />
        
    <miles:MilesCardList ID="clContent"   runat="server" DataKeyNames="CategoryID"
        ShowAddNewRecordButton="false" >
        <SortExpressionList>
            <miles:MilesCardListSortExpression DisplayName="Category Name" FieldName="CategoryName" />
            <miles:MilesCardListSortExpression DisplayName="Content Type" FieldName="ContentTypeID" />
        </SortExpressionList>

        <CardTemplate HeaderLinkType="HyperLink" HeaderNavigateUrlFormatString="javascript:openEditDialog('{0}');" HeaderNavigateUrlFields="CategoryID">
            <CardHeaderTemplate>
                <h4>
                    <%# Eval("CategoryName")%>
                    <asp:Image ID="lnkEdit" CssClass="pull-right" runat="server" ToolTip="Edit" ImageUrl="/Images/24x24/document-edit.png" />
                </h4>
            </CardHeaderTemplate>

            <CardItemTemplate>
                <div>
                    <asp:Label ID="lblType" runat="server" Text=""></asp:Label>
                </div>
            </CardItemTemplate>

            <CardActionTemplate>
                <miles:MilesToggleArchiveImageButton ID="btnDelete" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
            </CardActionTemplate>
        </CardTemplate>
    </miles:MilesCardList>
</asp:Content>

