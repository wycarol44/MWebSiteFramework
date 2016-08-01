<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="UserDocuments.aspx.vb" Inherits="HR_UserDocuments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail">

        <div class="detail-header">
            <miles:MilesTabStrip ID="rdTabs" runat="server" XmlFileName="~/App_Data/Tabs/UserTabs.xml" />
            
        </div>
         <div class="detail-body">
           
            <miles:MilesDocuments ID="milesDocuments" runat="server"></miles:MilesDocuments>

        </div>
    </asp:Panel>

</asp:Content>

