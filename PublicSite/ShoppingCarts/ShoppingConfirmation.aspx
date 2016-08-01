<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ShoppingConfirmation.aspx.vb" Inherits="ShoppingCarts_ShoppingConfirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">

    <h3>Your Order Has Been Confirmed</h3>
    <br /> 
    <asp:Label ID="lblOrderID" runat="server" Text="OrderID:"></asp:Label>
    <asp:Label ID="lblOrderID2" runat="server" Text=" "></asp:Label>

    <h4>Customer Info</h4>
    <asp:Label ID="lblFName" runat="server" Text="First Name:"></asp:Label>
    <asp:Label ID="lblFName2" runat="server" Text=" "></asp:Label>

    <br /> 
    <asp:Label ID="lblLName" runat="server" Text="Last Name:"></asp:Label>
    <asp:Label ID="lblLName2" runat="server" Text=" "></asp:Label>

    <h4>Shipping Address</h4>

    <asp:Label ID="lblAddress1" runat="server" Text="Address1:"></asp:Label>
    <asp:Label ID="lblAddress12" runat="server" Text=" "></asp:Label>

    <br /> 
    <asp:Label ID="lblAddress2" runat="server" Text="Address2:"></asp:Label>
    <asp:Label ID="lblAddress22" runat="server" Text=" "></asp:Label>

    <br /> 
    <asp:Label ID="lblCity" runat="server" Text="City:"></asp:Label>
    <asp:Label ID="lblCity2" runat="server" Text=" "></asp:Label>

    <br /> 
    <asp:Label ID="lblState" runat="server" Text="State:"></asp:Label>
    <asp:Label ID="lblState2" runat="server" Text=" "></asp:Label>

     <br /> 
    <asp:Label ID="lblZip" runat="server" Text="Zip:"></asp:Label>
    <asp:Label ID="lblZip2" runat="server" Text=" "></asp:Label>
</asp:Content>

