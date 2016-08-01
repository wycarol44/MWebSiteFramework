<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageOrders.aspx.vb" Inherits="Orders_ManageOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">

    <miles:MilesSearchPanel ID="pnlSearch" runat="server" RadGridID="dgList">
        <ContentPanel>
        <div class="row">
        <div class="col-md-2">
		        <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name" AssociatedControlID="ddlCustomerName" />
                <telerik:RadComboBox ID="ddlCustomerName" runat="server" EmptyMessage="Select an Customer" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="5"/> 
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
		        <asp:CheckBox ID="chkArchived" runat="server" Text="Archived" CssClass="chk-inline"  />
        </div>
        </div>
        </ContentPanel>
   </miles:MilesSearchPanel>

 
    <hr />
    <telerik:RadAjaxPanel runat="server" ID="pnlAjax" LoadingPanelID="rdLoadingPanel" EnableAJAX="false">
        <miles:MilesCardList ID="clOrders" runat="server" AllowCustomPaging="true" DataKeyNames="OrderID" AddNewRecordText="Add New Order" CardMinHeight="40px" NumberOfColumns="One">
            <SortExpressionList>
            </SortExpressionList>

            <CardInfoTemplate>
                <div class="row hidden-xs hidden-sm panel-heading">
                    <div class="col-md-1">Order #</div>
                    <div class="col-md-2">Customer Name</div>
                    <div class="col-md-2">Contact Phone</div>
                    <div class="col-md-2">Payment Type</div>
                    <div class="col-md-2">Order Total</div>
                    <div class="col-md-2">Order Date</div>
                </div>
            </CardInfoTemplate>

            <CardTemplate HeaderLinkType="HyperLink" HeaderNavigateUrlFormatString="~/Orders/OrderInfo.aspx?OrderID={0}" HeaderNavigateUrlFields="BatchID">
                <CardItemTemplate>
                    <div class="row panel-heading">
                        <div class="col-md-1">
                            <miles:CardListHyperLinkProperty runat="server" DataTextField="OrderID" DataNavigateUrlFormatString="../Orders/OrderInfo.aspx?OrderID={0}" DataNavigateUrlFields="OrderID" />
                        </div>
                        <div class="col-md-2">
                            <miles:CardListHyperLinkProperty runat="server" DataTextField ="CustomerName" DataNavigateUrlFormatString="../CRM/CustomerInfo.aspx?CustomerID={0}" DataNavigateUrlFields="CustomerID" Target ="_blank"/>
                        </div>
                        <div class="col-md-2">
                            <a href='<%# "tel:" + Eval("ContactPhone")%>'><%# FormatPhone(PhoneNumberType.Work, Eval("ContactPhone"))%></a>

                        </div>
                        <div class="col-md-2">
                            <miles:CardListDataBoundProperty runat="server" DataField="ItemName" />
                        </div>
                        <div class="col-md-2">
                            <miles:CardListDataBoundProperty runat="server" DataField="OrderTotal" DataFormatString="{0:C}"/>
                        </div>
                          <div class="col-md-2">
                            <miles:CardListDataBoundProperty runat="server" DataField="DateOrdered" DataFormatString="{0:d}"  />
                        </div>

                    </div>
                </CardItemTemplate>
                 <CardActionTemplate>
			        <miles:MilesToggleArchiveImageButton ID="btnArchiveRestore" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
		        </CardActionTemplate>


            </CardTemplate>
        </miles:MilesCardList>
    </telerik:RadAjaxPanel>


</asp:Content>

