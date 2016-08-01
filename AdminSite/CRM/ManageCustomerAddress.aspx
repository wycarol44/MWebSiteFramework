<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageCustomerAddress.aspx.vb" Inherits="CRM_ManageCustomerAddress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       <script>
                function openEditDialog(id, id2, id3) {
                    openWindow('/CRM/DialogManageCustomerAddress.aspx?CustomerAddressID=' + id + '&CustomerID=' + id2 + '&AddressTypeID=' + id3, 'Edit Content', WINDOW_LARGE);
                    return false;
                }

<%--                function dialogClosed(sender, args) {
                    var arg = args.get_argument();

                    if (arg != null) {
                        $find('<%= rdAjaxManager.ClientID%>').ajaxRequest();
                    }
                }--%>

        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
    
     <asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
        <miles:MilesTabStrip ID="rdTabs" runat="server" ShowInvisible="true"   XmlFileName="~/App_Data/Tabs/CustomerTabs.xml"  />
    </asp:Panel>

<%--    Billing    --%>




     <%--    Search Panel--%>
    <miles:MilesSearchPanel ID="pnlSearch" runat="server" RadGridID="dgList">
        <ContentPanel>
        <div class="row">
            <div class="col-md-2">
            <br>
		            <asp:CheckBox ID="chkArchived" runat="server" Text="Include Archived" CssClass="chk-inline"  />
            </div>
        </div>
        </ContentPanel>
        </miles:MilesSearchPanel>




      <%--     Card List    --%>
      <telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />
        <telerik:RadAjaxManager ID="rdAjaxManager" runat="server" DefaultLoadingPanelID="rdLoadingPanel" EnableAJAX ="false" >
	        <AjaxSettings>
		        <telerik:AjaxSetting AjaxControlID="clAddress:CardListPanel">
			        <UpdatedControls>
				        <telerik:AjaxUpdatedControl ControlID="clAddress:CardListPanel" />
			        </UpdatedControls>
		        </telerik:AjaxSetting>
	        </AjaxSettings>
        </telerik:RadAjaxManager>

        <miles:MilesCardList ID="clAddress" runat="server" AutoBind="true" DataKeyNames="CustomerAddressID"  AddNewRecordText="Add New Address"  CardMinHeight="200px" >
        <SortExpressionList>
		        <miles:MilesCardListSortExpression DisplayName="Address1" FieldName="Address1" />
		        <miles:MilesCardListSortExpression DisplayName="Address2" FieldName="Address2" />
		        <miles:MilesCardListSortExpression DisplayName="City" FieldName="City" />
		        <miles:MilesCardListSortExpression DisplayName="State" FieldName="State" />
		        <miles:MilesCardListSortExpression DisplayName="PostalCode" FieldName="PostalCode" />
        </SortExpressionList>

	        <CardTemplate HeaderLinkType="HyperLink" HeaderNavigateUrlFormatString="javascript:openEditDialog({0},{1},{2});" HeaderNavigateUrlFields="CustomerAddressID,CustomerID,AddressTypeID">
		        
                <CardHeaderTemplate>
                    <h4>
                    <%# Eval("Address1")%>
                    <div class="pull-right">
                    <asp:Image ID="btnInfo" runat="server"  ImageUrl="/Images/24x24/nav_blue_right.png" />
                    </div>
                    </h4>
		        </CardHeaderTemplate>
		        <CardItemTemplate>
				          <div><strong>Address2:</strong> <%# Eval("Address2")%></div>
				          <div><strong>City:</strong> <%# Eval("City")%></div>
				          <div><strong>State:</strong> <%# Eval("State")%></div>
				          <div><strong>PostalCode:</strong> <%# Eval("PostalCode")%></div>
				          <div><strong>Archived:</strong> <%# Eval("Archived")%></div>
		        </CardItemTemplate>
		        <CardActionTemplate>
			        <miles:MilesToggleArchiveImageButton ID="btnArchiveRestore" runat="server" Archived='<%# CBool(Eval("Archived"))%>' />
		        </CardActionTemplate>
	         </CardTemplate>
        </miles:MilesCardList>


</asp:Content>

