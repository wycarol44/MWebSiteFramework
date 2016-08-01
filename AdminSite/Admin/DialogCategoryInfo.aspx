<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="DialogCategoryInfo.aspx.vb" Inherits="Admin_DialogCategoryInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
      
      <asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnSaveClose">
            <asp:Label ID="Label1" runat="server" Text="Category Name" AssociatedControlID="txtCategory" />
            <asp:Label ID="Label2" runat="server" SkinID="Required" />
            <asp:TextBox ID="txtCategory" runat="server" MaxLength="50" />

            <asp:Panel ID="pnlForm" runat="server" >

                <asp:Label ID="Label3" runat="server" Text="Description" AssociatedControlID="txtDescription" />
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" row="6" Height ="200px"></asp:TextBox>

            </asp:Panel>
            
            <div class="dialog-actions">
                <div>
                    <miles:MilesButton ID="btnSaveClose" runat="server" Text="Save and Close" ActionType="Primary" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false" />
                </div>
            </div>
        </asp:Panel>


</asp:Content>

