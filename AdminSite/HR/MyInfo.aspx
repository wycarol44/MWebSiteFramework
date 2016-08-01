<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="MyInfo.aspx.vb" Inherits="HR_MyInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadCodeBlock ID="rdCodeBlock" runat="server">

        <script>
            function changePasswordDialog(id) {
                openWindow("/CommonDialogs/DialogChangeUserPassword.aspx?UserID=" + id, "Change Password", WINDOW_SMALL);
            }

            function changePictureDialog() {
                openWindow('/CommonDialogs/DialogUploadImage.aspx?ObjectID=<%= CInt(MilesMetaObjects.Users)%>&KeyID=<%= If (UserAuthentication.User isNot Nothing, UserAuthentication.User.UserID,0) %>', "Upload Image", WINDOW_LARGE, picturechanged);
            }

            function picturechanged(sender, args) {
                var arg = args.get_argument();

                if (arg != null) {
                    window.location.href = window.location.href;
                }
            }

        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>

    <div class="dashboard">
        <div class="row">
            <div class="col-md-6">
                <div class="panel panel-info widget">
                    <div class="panel-heading">
                        Basic Info
                    </div>
                    <div class="panel-body action-bottom">
                        <div class="contentarea">
                            <div class="row">
                                <div class="col-xs-4">
                                    <miles:MilesPicture ID="ucPicture" runat="server" Width="100%" Style="max-width: 100px;" />
                                </div>
                                <div class="col-xs-8">
                                    <asp:Label ID="lblFullName" CssClass="itemlabel" runat="server" />
                                    <br />
                                    <asp:Label ID="Label1" Text="Username:" CssClass="itemlabel" runat="server" />
                                    <asp:Label ID="lblUserName" runat="server" /><br />
                                    <asp:Label ID="Label2" Text="Status:" CssClass="itemlabel" runat="server" />
                                    <asp:Label ID="lblStatus" runat="server" /><br />
                                    <asp:Label ID="Label3" Text="Job Title:" CssClass="itemlabel" runat="server" />
                                    <asp:Label ID="lblJobTitle" runat="server" /><br />
                                    <asp:Label ID="Label4" Text="User Roles:" CssClass="itemlabel" runat="server" />
                                    <asp:Label ID="lblUserRoles" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="action">
                            <telerik:RadToolBar ID="rdActions" runat="server">
                                <Items>
                                    <telerik:RadToolBarDropDown DropDownWidth="175px" Text="Actions">
                                        <Buttons>
                                            <telerik:RadToolBarButton Text="Change Password" CommandName="UpdatePassword" CausesValidation="false"
                                                ImageUrl="~/Images/16x16/key.png" Value="ChangePassword" />
                                            <telerik:RadToolBarButton Text="Change Picture" CommandName="UpdatePicture" CausesValidation="false"
                                                ImageUrl="~/Images/16x16/picture-user.png" Value="ChangePicture" />
                                        </Buttons>
                                    </telerik:RadToolBarDropDown>
                                </Items>
                            </telerik:RadToolBar>
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-md-6">
                <div class="panel panel-default widget">
                    <div class="panel-heading anchorheader">
                        <asp:HyperLink ID="lnkContactInfoHeader" Text="Contact Info" runat="server">
                            <asp:Image ID="lnkEdit" CssClass="pull-right" runat="server" ToolTip="Edit" ImageUrl="/Images/24x24/document-edit.png" />
                        </asp:HyperLink>
                    </div>

                    <div class="panel-body">
                        <div class="contentarea">
                            <asp:HyperLink ID="lnkContactEmail" runat="server" /><br />
                            <asp:Label ID="lblPhone" runat="server" />
                            <asp:Label ID="lblAddress" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

