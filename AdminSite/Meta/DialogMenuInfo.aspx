<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogMenuInfo.aspx.vb" Inherits="Meta_DialogMenuInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">


        <asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnSaveClose">
            <asp:Label ID="Label1" runat="server" Text="Title" AssociatedControlID="txtTitle" />
            <asp:Label ID="Label2" runat="server" SkinID="Required" />
            <asp:TextBox ID="txtTitle" runat="server" MaxLength="50" placeholder="menu display title" />

            <asp:Panel ID="pnlForm" runat="server" >

                <asp:Label ID="Label3" runat="server" Text="Associated Form" AssociatedControlID="ddlForm" />
                <telerik:RadComboBox ID="ddlForm" runat="server" />

                <asp:Label ID="Label4" runat="server" Text="Navigate Url" AssociatedControlID="txtNavigateUrl" />
                <asp:TextBox ID="txtNavigateUrl" runat="server" placeholder="destination url (takes precedence)" />

            </asp:Panel>
            <p>
                <br />
                <asp:CheckBox ID="chkIsInternalOnly" runat="server" Text="Visible only to internal user" />
            </p>
            <div class="dialog-actions">
                <div>
                    <miles:MilesButton ID="btnSaveClose" runat="server" Text="Save and Close" ActionType="Primary" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false" />
                </div>
            </div>
        </asp:Panel>

</asp:Content>

