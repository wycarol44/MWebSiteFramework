<%@ Control Language="VB" AutoEventWireup="false" CodeFile="orderheader.ascx.vb" Inherits="UserControls_orderheader" %>




<div class="detail-body">
    <div class="row">
        
        <div class="col-md-4">
             <div class="panel panel-info">
                 <div class="panel-heading">Order Info</div>
                    <div class="panel-body">
                        <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name"/>
                        <asp:Label ID="lblCustomerNameRequired" runat="server" SkinID="Required" />
                        <telerik:RadComboBox ID="ddlCustomerName" runat="server" EmptyMessage="Select an Customer" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="5" AutoPostBack ="true" ValidationGroup ="ValidCusName"/> 
                        <asp:RequiredFieldValidator ID="rfvCustomerName" runat="server" ControlToValidate="ddlCustomerName" ErrorMessage="The Customer Name is Required" />


                        <asp:Label ID="lblDateFrom" runat="server" Text="Order Date" />
                        <asp:Label ID="lblDateFromRequired" runat="server" SkinID="Required" />
                        <miles:MilesDateTimePicker ID="txtOrderDate" runat="server" ShowTime="false"  />
                        <asp:RequiredFieldValidator ID="rfvOrderDate" runat="server" ControlToValidate="txtOrderDate" ErrorMessage="The Order Date is Required" />


                        <asp:Label ID="lblPhone" runat="server" Text="Contact Phone" />
                        <asp:Label ID="lblPhoneRequired" runat="server" SkinID="Required" />
                        <miles:MilesPhone ID="mpPhone" runat ="server" />
                        <%--<asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="mpPhone" ErrorMessage="The Contact Phone is Required" />--%>

                        <asp:Label ID="lblTrackingNumber" runat="server" Text="Tracking Number" />
                        <asp:TextBox ID="txtTrackingNumber" runat="server"></asp:TextBox>

                        <asp:Label ID="lblDateShipped" runat="server" Text="Date Shipped" />
                        <miles:MilesDateTimePicker ID="txtDateShipped" runat="server" ShowTime="false"  />

                   </div>
             </div>
        </div>

        <div class="col-md-4">
             <div class="panel panel-info">
                 <div class="panel-heading">Billing Address</div>
                     <div class="panel-body">
                         <asp:Label ID="lblBillAdd" runat="server" Text="Billing Address" />
                           
                         
                          <asp:DropDownList ID="ddlBillAdd" runat="server" AutoPostBack ="true" ></asp:DropDownList> 

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

                                <asp:Label ID="lblState" runat="server" Text="State" />
                                <asp:Label ID="lblstateRequired" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtState"  runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="txtState" ErrorMessage="The State is Required" />

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

                            <asp:Label ID="lblShipAdd" runat="server" Text="Shipping Address " />
                            <asp:DropDownList ID="ddlShipAdd" runat="server" AutoPostBack ="true" ></asp:DropDownList> 

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

                                <asp:Label ID="lblSState" runat="server" Text="State" />
                                <asp:Label ID="lblSstateRequired" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtSState"  runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvSState" runat="server" ControlToValidate="txtSState" ErrorMessage="The State is Required" />

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
 








