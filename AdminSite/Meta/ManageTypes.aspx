<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageTypes.aspx.vb" Inherits="Meta_ManageTypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadCodeBlock ID="rdCodeBlock" runat="server">
        <script>
            function openAddItemDialog(id) {
                openWindow('/Meta/DialogTypeInfo.aspx?TypeID=' + id, 'Add/Edit Items', WINDOW_MEDIUM);
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />
    <telerik:RadAjaxManager ID="rdAjaxManager" runat="server" DefaultLoadingPanelID="rdLoadingPanel">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="clTypes:CardListPanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clTypes:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="rdAjaxManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clTypes:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <miles:MilesSearchPanel ID="pnlSearch" runat="server" ListControlID="clTypes">

        <ContentPanel>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="lblTypeName" runat="server" Text="Type Name" AssociatedControlID="txtType" />
                    <asp:TextBox ID="txtType" runat="server" />
                </div>
            </div>
        </ContentPanel>

    </miles:MilesSearchPanel>
    <hr />
    <miles:MilesCardList ID="clTypes" runat="server" DataKeyNames="TypeID" AddNewRecordText="Add New Type" AutoBind="true" >
        <SortExpressionList>
            <miles:MilesCardListSortExpression DisplayName="Type Name" FieldName="TypeName" />
        </SortExpressionList>

        <CardTemplate ActionPosition="Right">
            <CardHeaderTemplate>
                <h4>
                    <%# Eval("TypeName")%>
                </h4>
            </CardHeaderTemplate>
            <CardItemTemplate></CardItemTemplate>
            <CardActionTemplate>
                <asp:ImageButton ID="btnEdit" runat="server" SkinID="CardListEdit" CommandName="Edit" />
                <asp:ImageButton ID="btnAddItem" runat="server" ImageUrl="/Images/24x24/list-add.png" ToolTip="Add/Edit Items" CommandName="AddItem" />
            </CardActionTemplate>
        </CardTemplate>

        <CardEditTemplate ActionPosition="Bottom" DefaultButton="lnkSave">
            <CardItemTemplate>
                <asp:Label ID="lblTypeName" Text="Type Name" runat="server" AssociatedControlID="txtTypeName" />
                <asp:Label ID="Label1" runat="server" SkinID="Required" />
                <asp:TextBox ID="txtTypeName" Text='<%# Eval("TypeName")%>' runat="server" MaxLength="100" />
                <asp:RequiredFieldValidator runat="server" ID="rfvTypeName" ControlToValidate="txtTypeName"
                    ErrorMessage="Type Name is required" Display="None"></asp:RequiredFieldValidator>
            </CardItemTemplate>

            <CardActionTemplate>
                <asp:LinkButton ID="lnkSave" runat="server" SkinID="CardListSave" CommandName="PerformInsert" />
                <asp:LinkButton ID="lnkCancel" runat="server" SkinID="CardListCancel" CausesValidation="false" CommandName="Cancel" />
            </CardActionTemplate>
        </CardEditTemplate>
    </miles:MilesCardList>
</asp:Content>
