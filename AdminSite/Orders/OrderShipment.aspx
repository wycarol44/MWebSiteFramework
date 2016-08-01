<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="OrderShipment.aspx.vb" Inherits="Orders_OrderShipment" %>

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
        </div>
        </ContentPanel>
   </miles:MilesSearchPanel>

    <hr />

        <telerik:RadAjaxPanel runat="server" ID="pnlAjax" LoadingPanelID="rdLoadingPanel" EnableAJAX="false">
        <miles:MilesCardList  ID="clOrderShipment" runat="server" AllowCustomPaging="true" DataKeyNames="OrderID" CardMinHeight="40px" NumberOfColumns="One" AllowMultiItemSelection="true" AllowItemSelection="true">
            <SortExpressionList>
            </SortExpressionList>



            <CardInfoTemplate>
                <div class="row hidden-xs hidden-sm panel-heading">
                    <div class="col-md-3">Tracking number</div>
                    <div class="col-md-1">OrderID</div>
                    <div class="col-md-2">Customer Name</div>
                    <div class="col-md-2">Date Ordered</div>
                    <div class="col-md-2">Payment Type</div>
                    <div class="col-md-2">Order Total</div>
                </div>
            </CardInfoTemplate>

            <CardTemplate HeaderLinkType="HyperLink">
                <CardItemTemplate>
                    <div class="row panel-heading">
                        <div class="col-md-3">
                                <asp:TextBox ID="txtTrackingNumber" runat="server" Width ="300" ></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                               <miles:CardListDataBoundProperty runat="server" DataField="OrderID" />
                        </div>
                        <div class="col-md-2">
                              <miles:CardListDataBoundProperty runat="server" DataField="CustomerName" />
                        </div>
                        <div class="col-md-2">
                              <miles:CardListDataBoundProperty runat="server" DataField="DateOrdered" DataFormatString="{0:d}"/>
                        </div>
                        <div class="col-md-2">
                              <miles:CardListDataBoundProperty runat="server" DataField="PaymentTypeName" />
                        </div>
                         <div class="col-md-2">
                              <miles:CardListDataBoundProperty runat="server" DataField="OrderTotal" DataFormatString="{0:C}"/>
                        </div>
                    </div>

                </CardItemTemplate>

            </CardTemplate>
        </miles:MilesCardList>
    </telerik:RadAjaxPanel>

    <hr />
<div class="detail-footer pull-right">
     <miles:MilesButton ID="btnApprove" runat="server" Text="Approve" ActionType="Secondary"/>
     <asp:Button ID="btnDeny" runat="server" Text="Deny" CausesValidation ="false" BackColor ="Red" ForeColor ="White"  SkinID ="Cancel"/>
</div>

</asp:Content>

