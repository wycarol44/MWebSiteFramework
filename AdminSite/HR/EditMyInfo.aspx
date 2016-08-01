<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="EditMyInfo.aspx.vb" Inherits="HR_EditMyInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail">

        <%--data-entry--%>
        <div class="detail-body">
            <div class="row">

                <div class="col-md-4">

                    <div class="panel panel-default">
                        <div class="panel-heading">Contact Info</div>
                        <div class="panel-body">

                            <asp:Label ID="Label10" runat="server" Text="Email" AssociatedControlID="txtEmail:EmailTextBox" />
                            <asp:Label ID="Label12" runat="server" SkinID="Required" />
                            <miles:MilesEmail ID="txtEmail" runat="server" />

                            <asp:RequiredFieldValidator ID="rfvtxtEmail" runat="server" ControlToValidate="txtEmail:EmailTextBox"
                                ErrorMessage="Email is required" />

                            <asp:Label ID="Label7" runat="server" Text="Home Phone" AssociatedControlID="txtHomePhone" />
                            <miles:MilesPhone ID="txtHomePhone" runat="server" ShowExt="false" />

                            <asp:Label ID="Label8" runat="server" Text="Mobile Phone" AssociatedControlID="txtMobilePhone" />
                            <miles:MilesPhone ID="txtMobilePhone" runat="server" ShowExt="false" />

                            <asp:Label ID="Label9" runat="server" Text="Work Phone" AssociatedControlID="txtWorkPhone" />
                            <miles:MilesPhone ID="txtWorkPhone" runat="server" ShowExt="true" />

                            <miles:MilesAddress ID="milesAddr" runat="server" />
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="panel panel-default">
                        <div class="panel-heading">Other Info</div>
                        <div class="panel-body">
                            <miles:MilesPictureUpload ID="milesPicUpload" runat="server" OnClientDialogClosed="pictureUploadClosed"
                                Width="200px" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--footer--%>
        <div class="detail-footer">
            <miles:MilesButton ID="btnSaveClose" runat="server" Text="Save and Close" ActionType="Primary" />
            <miles:MilesButton ID="btnSave" runat="server" Text="Save" ActionType="Primary" />
            <asp:Button ID="btnClose" runat="server" Text="Close" SkinID="Cancel" CausesValidation="false" />
        </div>
    </asp:Panel>

</asp:Content>

