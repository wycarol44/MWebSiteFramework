<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="TestSlider.aspx.vb" Inherits="Test_TestSlider" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
    <miles:MilesPercentageSlider ID="mpsPercentageDone" runat="server" AutoPostBack="true" />
    <br />
    <asp:label ID="lblValue" runat="server"></asp:label>
</asp:Content>

