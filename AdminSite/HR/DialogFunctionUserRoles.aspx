<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogFunctionUserRoles.aspx.vb" Inherits="HR_DialogFunctionUserRoles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" EnableAJAX="true">
        <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose" Style="padding-top: 10px">

            <asp:Label ID="label1" runat="server" Text="Select User Roles to give access to function "></asp:Label>
            <br />
            <br />

            <%--dialog content here--%>
            <div>
                <asp:DataList ID="dlUserRoles" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" RepeatLayout="Table">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkUserRole" runat="server" Style="padding-right: 30px" />
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <%--end dialog content--%>
            <%--buttons--%>
            <div class="dialog-actions">
                <div>
                    <miles:MilesButton ID="btnSaveClose" runat="server" Text="Save and Close" ActionType="Primary" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false" />
                </div>
            </div>

        </asp:Panel>

    </telerik:RadAjaxPanel>
</asp:Content>

