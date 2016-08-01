<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogContentInfo.aspx.vb" Inherits="Admin_DialogContentInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function OnClientCommandExecuting(editor, args) {
            var name = args.get_name();
            var val = args.get_value();

            if (name == "MergeFields") {
                editor.pasteHtml("[[" + val + "]]");
                //Cancel the further execution of the command as such a command does not exist in the editor command list
                args.set_cancel(true);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" EnableAJAX="true">
        <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose" Style="padding-top: 10px">

            <%--dialog content here--%>
            <div class="detail-body">
                <asp:Panel ID="pnlEmailWebsite" runat="server">
                    <div class="row">

                        <div class="col-md-10">
                            <asp:Panel ID="pnlEmailFromUser" runat="server" Visible="false">
                                <div class="col-md-8" style="padding-left: 0px">
                                    <asp:Label ID="Label1" runat="server" Text="Email From User" AssociatedControlID="ucUserCombo"></asp:Label>
                                    <asp:Label ID="label5" SkinID="Required" runat="server"></asp:Label>
                                    <miles:MilesUserComboBox ID="ucUserCombo" runat="server" EmptyMessage=""></miles:MilesUserComboBox>
                                    <asp:RequiredFieldValidator ID="rfvucUserCombo" runat="server" ControlToValidate="ucUserCombo" 
                                         ErrorMessage="Email From User is required" InitialValue="< Choose >" Display="None"></asp:RequiredFieldValidator>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlEmailFromManual" runat="server" Visible="false">
                                <div >
                                    <asp:Label ID="Label2" runat="server" Text="Email From" AssociatedControlID="txtEmail"></asp:Label>
                                    <asp:Label ID="label6" SkinID="Required" runat="server"></asp:Label>
                                    <miles:MilesEmail ID="txtEmail" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvtxtEmail" runat="server" ControlToValidate="txtEmail" 
                                         ErrorMessage="Email From is required" Display="None"></asp:RequiredFieldValidator>
                                </div>
                                <div>
                                    <asp:Label ID="Label3" runat="server" Text="Email From Name" AssociatedControlID="txtEmailFromName"></asp:Label>
                                    <asp:TextBox ID="txtEmailFromName" runat="server" MaxLength="500"></asp:TextBox>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-10">
                            <asp:Panel ID="pnlSubject" runat="server">
                                <asp:Label ID="lblSubject" runat="server" Text="Subject" AssociatedControlID="txtSubject"></asp:Label>
                                <asp:Label ID="labelsubjectrequired" SkinID="Required" runat="server"></asp:Label>
                                <br class="clear" />
                                <asp:TextBox ID="txtSubject" runat="server" MaxLength="500"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" ErrorMessage="Subject is required" Display="None"></asp:RequiredFieldValidator>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Panel ID="pnlBody" runat="server">
                                <asp:Label ID="lblBody" runat="server" Text="Content" AssociatedControlID="txtBody"></asp:Label><br class="clear" />
                                <miles:MilesEditor ID="txtBody" runat="server" ToolbarMode="Default" OnClientCommandExecuting="OnClientCommandExecuting"
                                    Height="500px" />
                                <br class="clear" />
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlHyperLink" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-10">
                            <asp:Label ID="Label4" runat="server" Text="Content" AssociatedControlID="txtHyperlinkContent"></asp:Label>
                            <asp:TextBox ID="txtHyperlinkContent" runat="server" Width="95%" TextMode="MultiLine" Rows="6"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
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

