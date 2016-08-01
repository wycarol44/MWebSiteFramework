<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="CustomerDocuments.aspx.vb" Inherits="CRM_CustomerDocuments" %>


<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
    <asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail">
        <asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
            <miles:MilesTabStrip ID="rdTabs" runat="server" XmlFileName="~/App_Data/Tabs/CustomerTabs.xml" />
        </asp:Panel>
        <div class="detail-body">
           
            <miles:MilesDocuments ID="milesDocuments" runat="server"
                LoadDocumentOnPageLoad="true" />

        </div>
    </asp:Panel>
</asp:Content>

