<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="CustomerInfo.aspx.vb" Inherits="CRM_CustomerInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function OnClientButtonClicking(sender, args) {
            var button = args.get_item();
          
            if (button.get_commandName() == "ViewAuditLog") {
                args.set_cancel(true);
                openWindow("/CommonDialogs/DialogAuditLog.aspx?KeyID=<%= CustomerID%>&ObjectID=<%= MilesMetaObjects.Customers%>", "Customer Audit Log", WINDOW_MEDIUM)
            }

        }

        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail">
        <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server">

            <asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
                <miles:MilesTabStrip ID="rdTabs" runat="server" XmlFileName="~/App_Data/Tabs/CustomerTabs.xml" />
                <telerik:RadToolBar ID="rdTools" OnClientButtonClicking="OnClientButtonClicking" runat="server" Width="100%" >
                    <Items>
                        <telerik:RadToolBarDropDown DropDownWidth="175px" Text="Customer Actions">
                            <Buttons>
                                <telerik:RadToolBarButton Text="Mark As Active" Value="Active" CommandName="Active" runat="server"/>
                                <telerik:RadToolBarButton Text="Mark As Pending" Value="Pending" CommandName="Pending" runat="server"/>
                                <telerik:RadToolBarButton Text="Mark As Lost" Value="Lost" CommandName="Lost" runat="server" />
                                <telerik:RadToolBarButton Text="Mark As Inactive" Value="Inactive" CommandName="Inactive" runat="server" />
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

                                <asp:Label ID="Label1" runat="server" Text="Name" AssociatedControlID="txtName" />
                                <asp:Label ID="Label2" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtName" runat="server" MaxLength="500" />
                                <asp:RequiredFieldValidator ID="rfvtxtName" runat="server" ControlToValidate="txtName"
                                    ErrorMessage="Name is required" />

                                <asp:Label ID="Label14" runat="server" Text="Status" AssociatedControlID="ddlStatus" />
                                <asp:Label ID="lblStatusRequired" runat="server" SkinID="Required" />
                                <miles:MilesTypeComboBox ID="ddlStatus" runat="server" StatusType="CustomerStatus" Visible="false" />
                                <asp:RequiredFieldValidator ID="rfvddlStatus" runat="server" ControlToValidate="ddlStatus"
                                    ErrorMessage="Status is required" InitialValue="< Choose >" />
                                <asp:Label ID="lblStatus" runat="server" Text="" Visible="false"></asp:Label>
                                <br />
                                <asp:Label ID="Label11" runat="server" Text="WebSite" AssociatedControlID="txtWebSite" />
                                <asp:TextBox ID="txtWebSite" runat="server" MaxLength="200" />

                                <asp:Label ID="Label13" runat="server" Text="Notes" AssociatedControlID="txtNotes" />
                                <miles:MilesEditor ID="txtNotes" runat="server" />

                            </div>
                        </div>

                    </div>

                    <div class="col-md-4">
                        <div class="panel panel-default">
                            <div class="panel-heading">Contact Info</div>
                            <div class="panel-body">
                                <asp:Label ID="Label9" runat="server" Text="Phone" AssociatedControlID="txtPhone" />
                                <miles:MilesPhone ID="txtPhone" runat="server" ShowExt="false" />
                                <miles:MilesAddress ID="milesAddr" runat="server" />
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

