<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogCMSMergeFieldInfo.aspx.vb" Inherits="Meta_DialogCMSMergeFieldInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" RenderMode="Inline" EnableAJAX="true">
        <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose" Style="padding-top: 10px">


            <%--dialog content here--%>
            <div class="row">
                <div class="col-xs-8">

                    <asp:Label ID="lblMergeField" runat="server" Text="Merge Field" AssociatedControlID="txtMergeField" />
                    <asp:Label ID="lblMergeFieldRequired" runat="server" SkinID="Required" />
                    <asp:TextBox ID="txtMergeField" runat="server" MaxLength="500" />
                    <asp:RequiredFieldValidator ID="rfvMergeField" runat="server" ControlToValidate="txtMergeField"
                        ErrorMessage="Merge Field is required" Display="None" />

                </div>
            </div>
            <%--end dialog content--%>

            <%--buttons--%>
            <div class="dialog-actions">
                <div>
                    <miles:MilesButton ID="btnSaveClose" runat="server" Text="Save and Close" ActionType="Primary" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false"/>
                </div>
            </div>

        </asp:Panel>

    </telerik:RadAjaxPanel>

</asp:Content>

