<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageProducts.aspx.vb" Inherits="Products_ManageProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
   
     <miles:MilesSearchPanel ID="pnlSearch" runat="server" RadGridID="dgList">
        <ContentPanel>
        <div class="row">
        <div class="col-md-2">
		        <asp:Label ID="lblProductName" runat="server" Text="Product Name" AssociatedControlID="txtProductName" />
		        <asp:TextBox ID="txtProductName" runat="server" />
        </div>
        <div class="col-md-2">
		        <asp:Label ID="lblDescription" runat="server" Text="Description" AssociatedControlID="txtDescription" />
		        <asp:TextBox ID="txtDescription" runat="server" />
        </div>
        <div class="col-md-2">
		        <asp:Label ID="lblDateFrom" runat="server" Text="Date From" AssociatedControlID="txtDateFrom:dateInput" />
		        <miles:MilesDateTimePicker ID="txtDateFrom" runat="server" ShowTime="false"  />
        </div>
        <div class="col-md-2">
		        <asp:Label ID="lblDateTo" runat="server" Text="Date To" AssociatedControlID="txtDateTo:dateInput" />
		        <miles:MilesDateTimePicker ID="txtDateTo" runat="server" ShowTime="false"  />
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
        <telerik:RadAjaxManager ID="rdAjaxManager" runat="server" DefaultLoadingPanelID="rdLoadingPanel" EnableAJAX="false"  >
	        <AjaxSettings>
		        <telerik:AjaxSetting AjaxControlID="clProducts:CardListPanel">
			        <UpdatedControls>
				        <telerik:AjaxUpdatedControl ControlID="clProducts:CardListPanel" />
			        </UpdatedControls>
		        </telerik:AjaxSetting>
	        </AjaxSettings>
        </telerik:RadAjaxManager>
        <miles:MilesCardList ID="clProducts" runat="server" DataKeyNames="ProductID"  AddNewRecordText="Add New Product"  CardMinHeight="200px" NumberOfColumns ="three" >
        <SortExpressionList>
		        <miles:MilesCardListSortExpression DisplayName="ProductName" FieldName="ProductName" />
		        <miles:MilesCardListSortExpression DisplayName="ShortDescription" FieldName="ShortDescription" />
		        <miles:MilesCardListSortExpression DisplayName="CategoryID" FieldName="CategoryID" />
		        <miles:MilesCardListSortExpression DisplayName="SubCategoryID" FieldName="SubCategoryID" />
		        <miles:MilesCardListSortExpression DisplayName="Archived" FieldName="Archived" />
        </SortExpressionList>
	        <CardTemplate HeaderLinkType="HyperLink" HeaderNavigateUrlFormatString="~/Products/ProductsInfo.aspx?ProductID={0}" HeaderNavigateUrlFields="ProductID" >
		        <CardHeaderTemplate>
        <h4>
        <%# Eval("ProductName") %>
        <div class="pull-right">
        <asp:Image ID="btnInfo" runat="server"  ImageUrl="/Images/24x24/nav_blue_right.png" />
        </div>
        </h4>
		        </CardHeaderTemplate>
		        <CardItemTemplate>
				          <div><strong>Product Name:</strong> <%# Eval("ProductName")%></div>
				          <div><strong>Short Description:</strong> <%# Eval("ShortDescription")%></div>
				          <div><strong>Category:</strong> <%# Eval("CategoryName")%></div>
				          <div><strong>SubCategory:</strong> <%# Eval("SubCategoryName")%></div>
				          <div><strong>Archived:</strong> <%# Eval("Archived")%></div>
		        </CardItemTemplate>
		        <CardActionTemplate>
			        <miles:MilesToggleArchiveImageButton ID="btnArchiveRestore" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
		        </CardActionTemplate>
	         </CardTemplate>
        </miles:MilesCardList>















</asp:Content>

