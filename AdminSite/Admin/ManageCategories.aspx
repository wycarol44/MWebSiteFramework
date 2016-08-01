<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageCategories.aspx.vb" Inherits="Admin_ManageCategories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">

    <miles:MilesSearchPanel ID="pnlSearch" runat="server" RadGridID="dgList">
        <ContentPanel>
        <div class="row">
        <div class="col-md-2">
		        <asp:Label ID="lblCategoryName" runat="server" Text="Category Name" AssociatedControlID="txtCategoryName" />
		        <asp:TextBox ID="txtCategoryName" runat="server" />
        </div>
        <div class="col-md-2">
		        <asp:Label ID="lblDescription" runat="server" Text="Description" AssociatedControlID="txtDescription" />
		        <asp:TextBox ID="txtDescription" runat="server" />
        </div>
        <div class="col-md-2">
		        
                <asp:Label ID="lblDateFrom" runat="server" Text="Date Created From" AssociatedControlID="txtFrom" />
                <miles:MilesDateTimePicker ID="txtFrom" runat="server" />
        </div>
        <div class="col-md-2">

                <asp:Label ID="lblDateTo" runat="server" Text="To" AssociatedControlID="txtTo" />
                <miles:MilesDateTimePicker ID="txtTo" runat="server" />

        </div>
        <div class="col-md-2">
        <br>
		        <asp:CheckBox ID="chkArchived" runat="server" Text="Include Archived " CssClass="chk-inline"  />
        </div>
        </div>
        </ContentPanel>
    </miles:MilesSearchPanel>

    <hr />


    <telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />
        <telerik:RadAjaxManager ID="rdAjaxManager" runat="server" DefaultLoadingPanelID="rdLoadingPanel" EnableAJAX="false">
	        <AjaxSettings>
		        <telerik:AjaxSetting AjaxControlID="clCategories:CardListPanel">
			        <UpdatedControls>
				        <telerik:AjaxUpdatedControl ControlID="clCategories:CardListPanel" />
			        </UpdatedControls>
		        </telerik:AjaxSetting>
	        </AjaxSettings>
        </telerik:RadAjaxManager>

        <miles:MilesCardList ID="clCategories" runat="server" DataKeyNames="CategoryID"  AddNewRecordText="Add New Category"  CardMinHeight="200px"  >
        
            <SortExpressionList>
		        <miles:MilesCardListSortExpression DisplayName="CategoryName" FieldName="CategoryName" />
		        <miles:MilesCardListSortExpression DisplayName="Description" FieldName="Description" />
		        <miles:MilesCardListSortExpression DisplayName="Archived" FieldName="Archived" />
		        <miles:MilesCardListSortExpression DisplayName="DateCreated" FieldName="DateCreated" />
            </SortExpressionList>

	        <CardTemplate HeaderLinkType="HyperLink" HeaderNavigateUrlFormatString="~/Admin/DialogCategories.aspx?CategoryID={0}" HeaderNavigateUrlFields="CategoryID">
		        <CardHeaderTemplate>
        <h4>
        <%# Eval("CategoryName") %>
        <div class="pull-right">
        <asp:Image ID="btnInfo" runat="server"  ImageUrl="/Images/24x24/nav_blue_right.png" />
        </div>
        </h4>
		        </CardHeaderTemplate>
		        <CardItemTemplate>
				 
				          <div><strong>Description:</strong> <%# Eval("Description")%></div>
				          <div><strong>Archived:</strong> <%# Eval("Archived")%></div>
				          <div><strong>DateCreated:</strong> <%# Eval("DateCreated")%></div>

		        </CardItemTemplate>
		        <CardActionTemplate>
			        <miles:MilesToggleArchiveImageButton ID="btnArchiveRestore" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
		        </CardActionTemplate>
	         </CardTemplate>
     </miles:MilesCardList>


</asp:Content>

