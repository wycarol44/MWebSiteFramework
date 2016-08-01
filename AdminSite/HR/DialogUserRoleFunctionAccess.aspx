<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogUserRoleFunctionAccess.aspx.vb" Inherits="HR_DialogUserRoleFunctionAccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" EnableAJAX="true">
        <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose" Style="padding-top: 10px">
            <asp:RadioButtonList ID="rbtnCheckAll" AutoPostBack="true"  runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="CheckAll" Text="Check All"   ></asp:ListItem>
                <asp:ListItem Value="UnCheckAll" Text="Uncheck All" style="padding-left: 5px;"></asp:ListItem>
            </asp:RadioButtonList>
            <telerik:RadTreeView ID="tvFunctions" runat="server" CheckBoxes="true"
                TriStateCheckBoxes="true" DataTextField="Name" DataFieldID="ID"
                DataValueField="ID" DataFieldParentID="ParentID">
            </telerik:RadTreeView>
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

