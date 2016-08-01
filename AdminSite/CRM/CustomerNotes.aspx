<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="CustomerNotes.aspx.vb" Inherits="CRM_CustomerNotes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail">
        <asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
            <miles:MilesTabStrip ID="rdTabs" runat="server" XmlFileName="~/App_Data/Tabs/CustomerTabs.xml" />
        </asp:Panel>
        <div class="detail-body">
            <miles:MilesNotes ID="milesNotes" runat="server" />
        </div>
    </asp:Panel>
</asp:Content>

