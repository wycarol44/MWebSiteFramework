<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ContactDashboard.aspx.vb" Inherits="CRM_ContactDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
        <miles:MilesTabStrip ID="rdTabs" runat="server" ShowInvisible="true" XmlFileName="~/App_Data/Tabs/CustomerTabs.xml" />
    </asp:Panel>
    <asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail">
        <div class="detail-body">
            <div class="dashboard">
                <div class="row">
                    <div class="col-md-6">
                        <div class="panel panel-info widget">
                            <div class="panel-heading anchorheader">
                                <asp:HyperLink ID="lnkBasicInfoHeader" Text="Basic Info" runat="server">
                                    <asp:Image ID="lnkEdit" CssClass="pull-right" runat="server" ToolTip="Edit" ImageUrl="/Images/24x24/document-edit.png" />
                                </asp:HyperLink>
                            </div>
                            <div class="panel-body action-bottom">
                                <div class="contentarea">
                                    <asp:HyperLink ID="lnkName" runat="server" />
                                    <asp:Label ID="lblIsPrimary" runat="server" Text=" (Primary Contact)" /><br />
                                    <asp:Label ID="lblTitle" runat="server" /><br />
                                    <asp:HyperLink ID="lnkEmail" runat="server" /><br />
                                    <asp:Label ID="lblPhone" runat="server" />
                                    <asp:Label ID="lblAddress" runat="server" />
                                </div>
                                <div class="action">
                                    <telerik:RadToolBar ID="rdActions" runat="server">
                                        <Items>
                                            <telerik:RadToolBarDropDown DropDownWidth="175px" Text="Actions">
                                                <Buttons>
                                                    <telerik:RadToolBarButton Text="Locate Address" Target="_blank" Value="ViewMap" ImageUrl="/Images/16x16/place_blue.png" CommandName="ViewMap" runat="server" />
                                                </Buttons>
                                            </telerik:RadToolBarDropDown>
                                        </Items>
                                    </telerik:RadToolBar>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

