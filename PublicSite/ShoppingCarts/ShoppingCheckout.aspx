<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ShoppingCheckout.aspx.vb" Inherits="ShoppingCarts_ShoppingCheckout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">

    <h1>Check Out</h1>

    <div class="detail-body">
    <div class="row">
        
        <div class="col-md-4">
             <div class="panel panel-info">
                 <div class="panel-heading">Customer Info</div>
                    <div class="panel-body">

                        <asp:Label ID="lblFirstName" runat="server" Text="First Name"/>
                        <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                        <asp:Label ID="lblLastName" runat="server" Text="Last Name"/>
                        <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>

                        <asp:Label ID="lblEmail" runat="server" Text="Email Address" />
                        <asp:Label ID="lblemailRequired" runat="server" SkinID="Required" />
                        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="The Email Address is Required" />

                        <asp:Label ID="lblPhone" runat="server" Text="Contact Phone" />
                        <asp:Label ID="lblPhoneRequired" runat="server" SkinID="Required" />
                        <miles:MilesPhone ID="mpPhone" runat ="server" />
<%--                        <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="mpPhone" ErrorMessage="The Contact Phone is Required" />--%>
                   </div>
             </div>
        </div>

        <div class="col-md-4">
             <div class="panel panel-info">
                 <div class="panel-heading">Billing Address</div>
                     <div class="panel-body">

                            <%--Billing address Panel--%>
                            <asp:Panel ID="plBillAdd" runat ="server" >
                                <asp:Label ID="lblAddress1" runat="server" Text="Address 1" />
                                <asp:Label ID="lblAddress1Required" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtAddress1"  runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAddress1" runat="server" ControlToValidate="txtAddress1" ErrorMessage="The Address is Required" />

                                <asp:Label ID="lblAddress2" runat="server" Text="Address 2" />
                                <asp:TextBox ID="txtAddress2"  runat="server" ></asp:TextBox>

                                <asp:Label ID="lblCity" runat="server" Text="City" />
                                <asp:Label ID="lblCityRequired" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtCity"  runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" ErrorMessage="The City is Required" />

                                <asp:Label ID="lblState1" runat="server" Text="State" />
                                <asp:Label ID="lblstateRequired1" runat="server" SkinID="Required" />
                                <miles:MilesStateComboBox runat ="server" ID="cbState"></miles:MilesStateComboBox>
                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="cbState" ErrorMessage="The State is Required" />


                                 <asp:Label ID="lblZip" runat="server" Text="Zip" />
                                <asp:Label ID="lblZipRequired" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtZip"  runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvZip" runat="server" ControlToValidate="txtZip" ErrorMessage="The Zip is Required" />
                            </asp:Panel>
                     </div>
             </div>
        </div>

        <div class="col-md-4">
             <div class="panel panel-info">
                 <div class="panel-heading">Shipping Address</div>
                     <div class="panel-body">
                         <%--Shipping address Panel--%>

                             <asp:CheckBox ID="CheckBox" runat="server" Text="Same as Billing" AutoPostBack ="true" />
                             <br />

                            <asp:Panel ID="plShipAdd" runat ="server" >

                                <asp:Label ID="lblSAddress1" runat="server" Text="Address 1" />
                                <asp:Label ID="lblSAddress1Required" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtSAddress1"  runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvSAddress1" runat="server" ControlToValidate="txtSAddress1" ErrorMessage="The Address is Required" />

                                <asp:Label ID="lblSAddress2" runat="server" Text="Address 2" />
                                <asp:TextBox ID="txtSAddress2"  runat="server" ></asp:TextBox>

                                <asp:Label ID="lblSCity" runat="server" Text="City" />
                                <asp:Label ID="lblSCityRequired" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtSCity"  runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvSCity" runat="server" ControlToValidate="txtSCity" ErrorMessage="The City is Required" />

                                <asp:Label ID="lblState2" runat="server" Text="State" />
                                <asp:Label ID="lblStateRequired2" runat="server" SkinID="Required" />
                                <miles:MilesStateComboBox runat ="server" ID="cbState2"></miles:MilesStateComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cbState" ErrorMessage="The State is Required" />

                                 <asp:Label ID="lblSZip" runat="server" Text="Zip" />
                                <asp:Label ID="lblSZipRequired" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtSZip"  runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvSZip" runat="server" ControlToValidate="txtSZip" ErrorMessage="The Zip is Required" />

                            </asp:Panel>
                 </div>
             </div>
        </div>
    </div>
</div>
 
      <hr />

      <h1>Cart Contents</h1>

    <telerik:RadAjaxPanel runat="server" ID="pnlAjax" LoadingPanelID="rdLoadingPanel" EnableAJAX="false">
        <miles:MilesCardList  ID="clCarts" runat="server" AllowCustomPaging="true" DataKeyNames="CartID" CardMinHeight="40px" NumberOfColumns="One" ShowAddNewRecordButton="false" >
            <SortExpressionList>
            </SortExpressionList>



            <CardInfoTemplate>
                <div class="row hidden-xs hidden-sm panel-heading">
                    <div class="col-md-3">Product Name</div>
                    <div class="col-md-3">Qty</div>
                    <div class="col-md-3">Price</div>
                    <div class="col-md-3">Extended Price</div>
                </div>
            </CardInfoTemplate>

            <CardTemplate HeaderLinkType="HyperLink">
                <CardItemTemplate>
                    <div class="row panel-heading">

                        <div class="col-md-3">
                              <miles:CardListDataBoundProperty runat="server" DataField="ProductName" />
                        </div>
                        <div class="col-md-3">
                            <miles:CardListDataBoundProperty runat="server" DataField="Qty" />
                        </div>
                         <div class="col-md-3">
                              <miles:CardListDataBoundProperty runat="server" DataField="Price" DataFormatString="{0:C}"/>
                        </div>
                         <div class="col-md-3">
                             <asp:Label ID="lblExtendedPrice" runat="server" Text="Extended Price"></asp:Label>
                        </div>
                    </div>

                        <asp:Label ID="lblCartID" runat="server" Text= <%#Eval("CartID")%> Visible ="false" ></asp:Label>
                </CardItemTemplate>
            </CardTemplate>

        </miles:MilesCardList>
    </telerik:RadAjaxPanel>

<div class="detail-footer pull-right">
     <miles:MilesButton ID="btnUpdate" runat="server" Text="Update Cart" ActionType="Secondary" CausesValidation ="false" />
     <asp:Button ID="btnSubmit" runat="server" Text="Submit Order" CausesValidation ="false" BackColor="#3498db"  ForeColor ="White"  SkinID ="Cancel"/>
    
</div>
</asp:Content>

