<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogToolTipInfo.aspx.vb" Inherits="Meta_DialogToolTipinfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose" Style="padding-top: 10px">

        <%--dialog content here--%>
        <div class="detail-body">

            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="lblToolTipName" runat="server" Text="ToolTip Name" AssociatedControlID="txtToolTipName"></asp:Label>
                    <asp:Label ID="label1" SkinID="Required" runat="server"></asp:Label>
                    <br class="clear" />
                    <asp:TextBox ID="txtToolTipName" runat="server" MaxLength="200"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtToolTipName" runat="server" ControlToValidate="txtToolTipName" ErrorMessage="ToolTip Name is required" Display="None"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="lblToolTipDesc" runat="server" Text="Description" AssociatedControlID="txtToolTipDesc"></asp:Label><br />
                    <miles:MilesEditor ID="txtToolTipDesc" runat="server" ToolbarMode="Default" ToolsFile="~/App_Data/EditorToolFiles/BasicToolsFile.xml"
                        Height="300px" />
                    <br class="clear" />
                </div>
            </div>
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
</asp:Content>

