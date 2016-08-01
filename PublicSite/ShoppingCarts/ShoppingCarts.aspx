<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ShoppingCarts.aspx.vb" Inherits="ShoppingCarts_ShoppingCarts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">

    <h1>Shopping Carts</h1>

      <hr />

    <telerik:RadAjaxPanel runat="server" ID="pnlAjax" LoadingPanelID="rdLoadingPanel" EnableAJAX="false">
        <miles:MilesCardList  ID="clCarts" runat="server" AllowCustomPaging="true" DataKeyNames="CartID" CardMinHeight="40px" NumberOfColumns="One" AllowMultiItemSelection="true" AllowItemSelection="true">
            <SortExpressionList>
            </SortExpressionList>



            <CardInfoTemplate>
                <div class="row hidden-xs hidden-sm panel-heading">
                    <div class="col-md-3">Product Name</div>
                    <div class="col-md-3">SubCategory Name</div>
                    <div class="col-md-2">Qty</div>
                    <div class="col-md-2">Price</div>
                    <div class="col-md-2">Extended Price</div>
                </div>
            </CardInfoTemplate>

            <CardTemplate HeaderLinkType="HyperLink">
                <CardItemTemplate>
                    <div class="row panel-heading">

                        <div class="col-md-3">
                              <miles:CardListDataBoundProperty runat="server" DataField="ProductName" />
                        </div>
                        <div class="col-md-3">
                              <miles:CardListDataBoundProperty runat="server" DataField="SubCategoryName"/>
                        </div>
                        <div class="col-md-2">
                            <div class="col-md-4">
                            <telerik:RadNumericTextBox ID="txtQty" runat="server" Width ="100" MinValue ="0">
                                <NumberFormat GroupSeparator="" DecimalDigits="0" />
                            </telerik:RadNumericTextBox> 
                            </div>
                        </div>
                         <div class="col-md-2">
                              <miles:CardListDataBoundProperty runat="server" DataField="Price" DataFormatString="{0:C}"/>
                        </div>
                         <div class="col-md-2">
                             <asp:Label ID="lblExtendedPrice" runat="server" Text="Extended Price"></asp:Label>
                        </div>
                    </div>

                        <asp:Label ID="lblCartID" runat="server" Text= <%#Eval("CartID")%> Visible ="false" ></asp:Label>
                </CardItemTemplate>
            </CardTemplate>

        </miles:MilesCardList>
    </telerik:RadAjaxPanel>

    <hr />

<div class="detail-footer pull-right">
     <miles:MilesButton ID="btnUpdate" runat="server" Text="Update" ActionType="Secondary"/>
     <asp:Button ID="btnRemove" runat="server" Text="Remove" CausesValidation ="false" BackColor ="#e74c3c" ForeColor ="White"  SkinID ="Cancel"/>
     <asp:Button ID="btnCheck" runat="server" Text="Check Out" CausesValidation ="false" BackColor="#3498db"  ForeColor ="White"  SkinID ="Cancel"/>
</div>

</asp:Content>

