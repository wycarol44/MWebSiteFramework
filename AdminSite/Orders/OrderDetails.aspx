<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="OrderDetails.aspx.vb" Inherits="Orders_OrderDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
    


<div class="detail-body">
    <div class="row">
        
        <div class="col-md-6">
             <div class="panel panel-info">
                 <div class="panel-heading">Order Detail</div>
                    <div class="panel-body">

                        <asp:Label ID="lblCate" runat="server" Text="Category"></asp:Label>
                        <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack ="true" ></asp:DropDownList>

                        <asp:Label ID="lbSubCate" runat="server" Text="SubCategory"></asp:Label>
                        <asp:DropDownList ID="ddlSubCategory" runat="server" AutoPostBack ="true" Enabled ="false" ></asp:DropDownList>

                        <asp:Label ID="lblProduct" runat="server" Text="Product:"></asp:Label>
                        <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack ="true" Enabled ="false" ></asp:DropDownList>

                        <asp:Label ID="lblQty" runat="server" Text="Qty:"></asp:Label>
                        <telerik:RadNumericTextBox ID="rtbQty" runat="server" MinValue ="0" MaxValue ="999999999">
                            <NumberFormat GroupSeparator ="" DecimalDigits ="0" />
                        </telerik:RadNumericTextBox>

                   </div>
             </div>
        </div>
    </div>
</div>


<div class="detail-footer">
     <miles:MilesButton ID="btnSaveCon" runat="server" Text="Save and Close" ActionType="Primary" />
     <asp:Button ID="btnClose" runat="server" Text="Cancel" SkinID="Cancel" CausesValidation="false" />
</div>

</asp:Content>

