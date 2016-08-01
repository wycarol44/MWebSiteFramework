<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageJobTitles.aspx.vb" Inherits="HR_ManageJobTitles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <miles:MilesSearchPanel ID="pnlSearch" runat="server" SearchOnLoadSavedState="true">
        <ContentPanel>
            <div class="row">
                <div class="col-sm-2">
                    <asp:Label runat="server" Text="Job Title" AssociatedControlID="txtJobTitleName" />
                    <br />
                    <asp:TextBox ID="txtJobTitleName" runat="server" />
                </div>
                <div class="col-sm-3">

                    <br />
                    <asp:CheckBox ID="chkArchived" runat="server" Text="Include archived" CssClass="chk-inline" />

                </div>
            </div>
        </ContentPanel>
    </miles:MilesSearchPanel>

    <hr />

    <telerik:RadAjaxLoadingPanel ID="rdAjaxLoadingPanel" runat="server" />
    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" LoadingPanelID="rdAjaxLoadingPanel">

        <miles:MilesCardList ID="clJobTitle" runat="server" DataKeyNames="JobTitleID"
            AddNewRecordText="Add New Job Title" >
            <SortExpressionList>
                <miles:MilesCardListSortExpression DisplayName="Job Title" FieldName="JobTitle" />
            </SortExpressionList>

            <CardTemplate>
                <CardHeaderTemplate>
                    <h4>
                        <%# Eval("JobTitle") %>
                    </h4>
                </CardHeaderTemplate>

                <CardItemTemplate>
                    <miles:CardListDataBoundProperty runat="server" DataField="JobDescription" ShowLabel="false" />
                </CardItemTemplate>

                <CardActionTemplate>
                    <asp:ImageButton ID="btnEdit" runat="server" SkinID="CardListEdit" CommandName="Edit" />
                    <miles:MilesToggleArchiveImageButton ID="btnDelete" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
                </CardActionTemplate>
            </CardTemplate>

            <CardEditTemplate ActionPosition="Bottom" DefaultButton="lnkSave">
                <CardItemTemplate>

                    <asp:Label ID="lblJobTitle" Text="Job Title" runat="server" AssociatedControlID="txtJobTitle" />
                    <asp:Label runat="server" SkinID="Required" />
                    <asp:TextBox ID="txtJobTitle" Text='<%# Eval("JobTitle")%>' runat="server" MaxLength="100" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvJobTitle" ControlToValidate="txtJobTitle"
                        ErrorMessage="Job Title is required" Display="None" />
                    <asp:Label ID="lblDesc" Text="Description" runat="server" AssociatedControlID="txtJobDesc" />
                    <asp:TextBox ID="txtJobDesc" TextMode="MultiLine" Rows="4" Text='<%# Eval("JobDescription")%>' runat="server" />

                </CardItemTemplate>

                <CardActionTemplate>
                    <asp:LinkButton ID="lnkSave" runat="server" SkinID="CardListSave" CommandName="PerformInsert" />
                    <asp:LinkButton ID="lnkCancel" runat="server" SkinID="CardListCancel" CausesValidation="false" CommandName="Cancel" />
                </CardActionTemplate>
            </CardEditTemplate>
        </miles:MilesCardList>
    </telerik:RadAjaxPanel>

</asp:Content>

