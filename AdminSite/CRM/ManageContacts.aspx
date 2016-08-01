<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageContacts.aspx.vb" Inherits="CRM_ManageContacts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />
    <telerik:RadAjaxManager ID="rdAjaxManager" runat="server" DefaultLoadingPanelID="rdLoadingPanel">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="clContacts:CardListPanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clContacts:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail">

        <asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
            <miles:MilesTabStrip ID="rdTabs" runat="server" XmlFileName="~/App_Data/Tabs/CustomerTabs.xml" />
        </asp:Panel>
        <div class="detail-body">
            <miles:MilesSearchPanel ID="pnlSearch" runat="server" ListControlID="clContacts">

                <ContentPanel>
                    <div class="row">
                        <div class="col-md-2">
                            <asp:Label ID="lblContactName" runat="server" Text="Contact Name" AssociatedControlID="txtContactName" />
                            <asp:TextBox ID="txtContactName" runat="server" />
                        </div>

                        <div class="col-md-2">
                            <asp:Label ID="Label4" runat="server" Text="Email" AssociatedControlID="txtEmail" />
                            <asp:TextBox ID="txtEmail" runat="server" />
                        </div>

                        <div class="col-md-2">
                            <asp:Label ID="lblPhone" runat="server" Text="Phone" AssociatedControlID="txtPhone:PhoneTextBox" />
                            <miles:MilesPhone ID="txtPhone" runat="server" ShowExt="false" />
                        </div>

                        <div class="col-md-2">
                            <br />
                            <asp:CheckBox ID="chkArchived" runat="server" Text="Include archived" CssClass="chk-inline" />
                        </div>

                    </div>
                </ContentPanel>

            </miles:MilesSearchPanel>
            <hr />
            <miles:MilesCardList ID="clContacts" runat="server" AutoBind="true"
                DataKeyNames="ContactID" AddNewRecordText="Add New Contact" >
                <SortExpressionList>
                    <miles:MilesCardListSortExpression DisplayName="Contact Name" FieldName="Fullname" />
                    <miles:MilesCardListSortExpression DisplayName="Title" FieldName="Title" />
                    <miles:MilesCardListSortExpression DisplayName="Email" FieldName="Email" />
                    <miles:MilesCardListSortExpression DisplayName="Phone" FieldName="WorkPhone" />
                </SortExpressionList>

                <CardTemplate HeaderLinkType="HyperLink" HeaderNavigateUrlFormatString="/CRM/ContactDashboard.aspx?CustomerID={0}&ContactID={1}" HeaderNavigateUrlFields="CustomerID, ContactID">
                    <CardHeaderTemplate>
                        <h4>
                            <img runat="server" src="../Images/24x24/tag_green.png" title="Primary Contact" visible='<%# Eval("IsPrimary")%>' />
                            <%# Eval("Fullname") %>
                            <div class="pull-right">
                                <asp:Image ID="imgEditContactInfo" runat="server"  ImageUrl="/Images/24x24/nav_blue_right.png" />
                            </div>
                        </h4>
                    </CardHeaderTemplate>

                    <CardItemTemplate>
                        <div><%# Eval("Title") %></div>
                        <a href='<%# "mailto:"+ Eval("Email")%>'><%# Eval("Email") %></a>

                        <div><a href='<%# "tel:" + Eval("WorkPhone")%>'><%# FormatPhone(PhoneNumberType.Work, Eval("WorkPhone"), Eval("WorkPhoneExt"))%></a></div>
                        <div><a href='<%# "tel:" + Eval("HomePhone")%>'><%# FormatPhone(PhoneNumberType.Home, Eval("HomePhone"))%></a></div>
                        <div><a href='<%# "tel:" + Eval("MobilePhone")%>'><%# FormatPhone(PhoneNumberType.Mobile, Eval("MobilePhone"))%></a></div>
                    </CardItemTemplate>

                    <CardActionTemplate>
                        <miles:MilesToggleArchiveImageButton ID="btnDelete" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
                    </CardActionTemplate>
                </CardTemplate>
            </miles:MilesCardList>
        </div>
    </asp:Panel>
</asp:Content>

