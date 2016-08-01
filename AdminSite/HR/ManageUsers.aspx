<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageUsers.aspx.vb" Inherits="HR_ManageUsers" ClientIDMode="AutoID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />
    <telerik:RadAjaxManager ID="rdAjaxManagerProxy" runat="server" DefaultLoadingPanelID="rdLoadingPanel">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="clUsers:CardListPanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clUsers:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>


    <miles:MilesSearchPanel ID="pnlSearch" runat="server" ListControlID="clUsers">

        <ContentPanel>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="Label1" runat="server" Text="User Full Name" AssociatedControlID="txtFullName" />
                    <asp:TextBox ID="txtFullName" runat="server" />
                </div>

                <div class="col-md-2">
                    <asp:Label ID="Label2" runat="server" Text="Phone" AssociatedControlID="txtPhone:PhoneTextBox" />
                    <miles:MilesPhone ID="txtPhone" runat="server" ShowExt="false" />
                </div>

                <div class="col-md-2">
                    <asp:Label ID="Label4" runat="server" Text="Email" AssociatedControlID="txtEmail" />
                    <asp:TextBox ID="txtEmail" runat="server" />
                </div>

                <div class="col-md-2">
                    <asp:Label ID="Label3" runat="server" Text="Job Titles" AssociatedControlID="ddlJobTitles" />
                    <telerik:RadComboBox ID="ddlJobTitles" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                        EmptyMessage="All" />
                </div>
                <div class="col-md-2">
                    <asp:Label ID="Label5" runat="server" Text="Status" AssociatedControlID="ddlStatusTypes" />
                    <miles:MilesTypeComboBox ID="ddlStatusTypes" runat="server" StatusType="UserStatus" IncludeDefaultItem="false"
                        CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="All" />
                </div>

                <div class="col-md-2">
                    <br />
                    <asp:CheckBox ID="chkArchived" runat="server" Text="Include archived" CssClass="chk-inline" />
                </div>

            </div>
        </ContentPanel>

    </miles:MilesSearchPanel>


    <hr />

    <miles:MilesCardList ID="clUsers" runat="server" DataKeyNames="UserID" AddNewRecordText="Add New User">
        <SortExpressionList>

            <miles:MilesCardListSortExpression DisplayName="Full Name" FieldName="Fullname" />
            <miles:MilesCardListSortExpression DisplayName="Job Title" FieldName="JobTitle" />
            <miles:MilesCardListSortExpression DisplayName="Status" FieldName="Status" />

        </SortExpressionList>

        <CardTemplate HeaderLinkType="HyperLink" HeaderNavigateUrlFields="UserID" HeaderNavigateUrlFormatString="~/HR/UserDashboard.aspx?UserID={0}">

            <CardHeaderTemplate>

                <h4>

                    <miles:MilesPicture ID="mPicUser" runat="server" Width="48px"
                        ImageName='<%# Eval("ThumbnailPath")%>'
                        FullSizeImageName='<%# Eval("PicturePath")%>' />


                    <%# Eval("Fullname") %>

                    <div class="pull-right">
                        <asp:Image ID="btnEditCustomerInfo" runat="server" ImageUrl="/Images/24x24/nav_blue_right.png" />
                    </div>
                </h4>

            </CardHeaderTemplate>

            <CardItemTemplate>
                <a href='<%# "mailto:"+ Eval("Email")%>'><%# Eval("Email") %></a>
               

                <div><a href='<%# "tel:" + Eval("WorkPhone")%>'><%# FormatPhone(PhoneNumberType.Work, Eval("WorkPhone"), Eval("WorkPhoneExt"))%></a></div>
                <div><a href='<%# "tel:" + Eval("HomePhone")%>'><%# FormatPhone(PhoneNumberType.Home, Eval("HomePhone"))%></a></div>
                <div><a href='<%# "tel:" + Eval("MobilePhone")%>'><%# FormatPhone(PhoneNumberType.Mobile, Eval("MobilePhone"))%></a></div>

                <miles:CardListDataBoundProperty runat="server" DataField="JobTitle" LabelText="Job Title:" />
                <miles:CardListDataBoundProperty runat="server" DataField="Status" LabelText="Status:" />


            </CardItemTemplate>

            <CardActionTemplate>
                <miles:MilesToggleArchiveImageButton ID="btnDelete" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />

            </CardActionTemplate>
        </CardTemplate>
    </miles:MilesCardList>



</asp:Content>

