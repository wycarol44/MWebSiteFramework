<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="Test1.aspx.vb" Inherits="Test1" %>
<%@ Register Src="~/AddressUserControl.ascx" TagName="AddressUserControl" TagPrefix="ucl"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">

    <ucl:AddressUserControl ID="auc" runat="server" Address="Home" />  
</asp:Content>

