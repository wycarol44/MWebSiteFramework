<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="UserInfo.aspx.vb" Inherits="HR_UserInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script>
        function changePasswordDialog(id) {
            openWindow("/CommonDialogs/DialogChangeUserPassword.aspx?UserID=" + id, "Change Password", WINDOW_SMALL);
        }

        function rdTools_OnClientButtonClicking(sender, args) {
            var item = args.get_item();

            var command = item.get_commandName();
            var argument = item.get_commandArgument();
            if (command == "ChangePassword") {
                changePasswordDialog(argument);
                args.set_cancel(true);
            }
            if (command== "ViewAuditLog") {
                args.set_cancel(true);
                openWindow("/CommonDialogs/DialogAuditLog.aspx?KeyID=<%= UserId%>&ObjectID=<%= MilesMetaObjects.Users%>", "Audit Log", WINDOW_MEDIUM)
            }
        }



    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail">
        <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server">

            <asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
                <miles:MilesTabStrip ID="rdTabs" runat="server" XmlFileName="~/App_Data/Tabs/UserTabs.xml" />

                <telerik:RadToolBar ID="rdTools" runat="server" Width="100%" OnClientButtonClicking="rdTools_OnClientButtonClicking">
                    <Items>
                        <telerik:RadToolBarDropDown DropDownWidth="175px" Text="User Actions">
                            <Buttons>
                                <telerik:RadToolBarButton Text="Mark As Active" Value="Active" CommandName="Active" runat="server"/>
                                <telerik:RadToolBarButton Text="Mark As Inactive" Value="Inactive" CommandName="Inactive" runat="server" />
                                <telerik:RadToolBarButton IsSeparator="true"></telerik:RadToolBarButton>
                                <telerik:RadToolBarButton Text="Change Password"  ImageUrl="~/Images/16x16/key.png" CommandName="ChangePassword" CausesValidation="false"
                                Value="ChangePassword" />
                            </Buttons>
                        </telerik:RadToolBarDropDown>

                        
                      <telerik:RadToolBarButton  CommandName="ViewAuditLog"  
                            Value="ViewAuditLog" CausesValidation="false"  >
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
            </asp:Panel>


            <%--data-entry--%>
            <div class="detail-body">
                <div class="row">

                    <div class="col-md-4">

                        <div class="panel panel-info">
                            <div class="panel-heading">Basic Info</div>
                            <div class="panel-body">

                                <asp:Label ID="Label1" runat="server" Text="First Name" AssociatedControlID="txtFirstName" />
                                <asp:Label ID="Label2" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="rfvtxtFirstName" runat="server" ControlToValidate="txtFirstName"
                                    ErrorMessage="First Name is required" />

                                <asp:Label ID="Label3" runat="server" Text="Last Name" AssociatedControlID="txtLastName" />
                                <asp:Label ID="Label4" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="50"/>
                                <asp:RequiredFieldValidator ID="rfvtxtLastName" runat="server" ControlToValidate="txtLastName"
                                    ErrorMessage="Last Name is required" />


                                <asp:Label ID="Label5" runat="server" Text="Username" AssociatedControlID="txtUsername" />
                                <asp:Label ID="Label6" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtUsername" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvtxtUsername" runat="server" ControlToValidate="txtUsername"
                                    ErrorMessage="Username is required" />
                                <asp:RegularExpressionValidator ID="revtxtUsername" runat="server" ControlToValidate="txtUsername"
                                    ValidationExpression="<%$ AppSettings: Miles.UsernameValidationExpression %>"
                                    ErrorMessage="<%$ AppSettings: Miles.UsernameValidationErrorMessage %>" />


                                <asp:Label ID="Label14" runat="server" Text="Status" AssociatedControlID="ddlStatus" />
                                <asp:Label ID="lblStatusRequired" runat="server" SkinID="Required" />
                                <miles:MilesTypeComboBox ID="ddlStatus" runat="server" StatusType="UserStatus" Visible="false" />
                                <asp:RequiredFieldValidator ID="rfvddlStatus" runat="server" ControlToValidate="ddlStatus"
                                    ErrorMessage="Status is required" InitialValue="< Choose >" />
                                <asp:Label ID="lblStatus" runat="server" Text="" Visible="false"></asp:Label>
                                <br />
                                <asp:Label ID="Label11" runat="server" Text="Job Title" AssociatedControlID="ddlJobTitle" />
                                <telerik:RadComboBox ID="ddlJobTitle" runat="server" />

                                <miles:UserRoleList ID="milesUserRole" Required="true" runat="server"></miles:UserRoleList>


                            </div>
                        </div>

                    </div>

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
                                <br />
                               <asp:Label ID="Label13" runat="server" Text="Notes" AssociatedControlID="txtNotes" />
                                <miles:MilesEditor ID="txtNotes" runat="server" />
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
        </telerik:RadAjaxPanel>
    </asp:Panel>

</asp:Content>

