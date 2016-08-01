<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ProductsInfo.aspx.vb" Inherits="Products_ProductsInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
   
 
<%--    Basic Info   --%> 
    
     <asp:Panel ID="Panel1" runat="server" CssClass="detail-header">
        <miles:MilesTabStrip ID="rdTabs" runat="server" ShowInvisible="true"   XmlFileName="~/App_Data/Tabs/ProductTab.xml" />
    </asp:Panel>




            <div class="col-md-6">
                            <asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail">
	                        <asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
	                        </asp:Panel>
	                        <div class="detail-body">
		                        <div class="row">
			                        <div class="col-md-12">
			                        <div class="panel panel-info">
				                        <div class="panel-body">

				                        <asp:Label ID="lblProductName" runat="server" Text="Product Name" AssociatedControlID="txtProductName" />
				                        <asp:Label ID="lblrProductName" runat="server" SkinID="Required" />
				                        <asp:TextBox ID="txtProductName" runat="server" />
				                        <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ControlToValidate="txtProductName" ErrorMessage="Product Name is required" />

				                        <asp:Label ID="lblShortDescription" runat="server" Text="Short Description" AssociatedControlID="txtShortDescription" />
				                        <asp:TextBox ID="txtShortDescription" runat="server" />

				                        <asp:Label ID="lblLongDescription" runat="server" Text="Long Description" AssociatedControlID="txtLongDescription" />
				                        <miles:MilesEditor ID="txtLongDescription" runat="server" />

				                        <asp:Label ID="lblCategoryName" runat="server" Text="Category" AssociatedControlID="ddlCategoryName" />
                                        <asp:DropDownList ID="ddlCategoryName" runat="server" AutoPostBack ="true" ></asp:DropDownList>

				                        <asp:Label ID="lblSubCategoryName" runat="server" Text="SubCategory" AssociatedControlID="ddlSubCategoryName"/>
				                        <asp:DropDownList ID="ddlSubCategoryName" runat="server" DataTextField="CategoryName" ></asp:DropDownList>
                                     

				                        <asp:Label ID="lblCost" runat="server" Text="Cost" AssociatedControlID="txtCost" />  
                                        <telerik:radnumerictextbox type="Currency" width="200px" id="txtCost" runat="server"></telerik:radnumerictextbox>

				                        <asp:Label ID="lblPrice" runat="server" Text="Price" AssociatedControlID="txtPrice" />
				                        <telerik:radnumerictextbox type="Currency" width="200px" id="txtPrice" runat="server"></telerik:radnumerictextbox>

                                        <asp:CheckBox ID="chkFeatured" runat="server" Text ="Is Featured" />
				                        </div>
			                        </div>
			                        </div>
		                        </div>
	                        </div>
	                        <div class="detail-footer">
                        <miles:MilesButton id="btnSaveClose" runat="server" Text="Save & Close" ActionType="Primary" />
                        <miles:MilesButton id="btnSave" runat="server" Text="Save" ActionType="Primary" />
                        <asp:Button id="btnCancel" runat="server" Text="Cancel" CausesValidation="false" SkinID="Cancel"  />
	                        </div>
                        </asp:Panel>

                     </div> 






</asp:Content>

