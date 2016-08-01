<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="DialogCategories.aspx.vb" Inherits="Admin_DialogCategories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">

<asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail">
	<asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
	</asp:Panel>
	<div class="detail-body">
		<div class="row">
			<div class="col-md-4">
			<div class="panel panel-info">
				<div class="panel-body">
				<asp:Label ID="lblCategoryName" runat="server" Text="Category Name" AssociatedControlID="txtCategoryName" />
				<asp:Label ID="lblrCategoryName" runat="server" SkinID="Required" />
				<asp:TextBox ID="txtCategoryName" runat="server" />
				<asp:RequiredFieldValidator ID="rfvCategoryName" runat="server" ControlToValidate="txtCategoryName" ErrorMessage="Category Name is required" />
				<asp:Label ID="lblDescription" runat="server" Text="Description" AssociatedControlID="txtDescription" />
                <miles:MilesEditor ID="txtDescription" runat="server"></miles:MilesEditor>

				</div>
			</div>
			</div>
			<div class="col-md-8">
			<div class="panel panel-default">
				<div class="panel-body">


        <miles:MilesSearchPanel ID="pnlSearch" runat="server" RadGridID="dgList">
        <ContentPanel>
        <div class="row">
        <div class="col-md-2">
		        <asp:Label ID="Label1" runat="server" Text="Category Name" AssociatedControlID="txtCategoryName2" />
		        <asp:TextBox ID="txtCategoryName2" runat="server" />
        </div>
        <div class="col-md-2">
		        <asp:Label ID="Label2" runat="server" Text="Description" AssociatedControlID="txtDescription2" />
		        <asp:TextBox ID="txtDescription2" runat="server" />
        </div>
        <div class="col-md-2">
		        
                <asp:Label ID="lblDateFrom" runat="server" Text="Date Created From" AssociatedControlID="txtFrom2" />
                <miles:MilesDateTimePicker ID="txtFrom2" runat="server" />
        </div>
        <div class="col-md-2">

                <asp:Label ID="lblDateTo" runat="server" Text="To" AssociatedControlID="txtTo2" />
                <miles:MilesDateTimePicker ID="txtTo2" runat="server" />

        </div>
        <div class="col-md-2">
        <br>
		        <asp:CheckBox ID="chkArchived" runat="server" Text="Include Archived " CssClass="chk-inline"  />
        </div>
        </div>
        </ContentPanel>
    </miles:MilesSearchPanel>

    <hr />



<telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />
        <telerik:RadAjaxManager ID="rdAjaxManager" runat="server" DefaultLoadingPanelID="rdLoadingPanel" EnableAJAX ="false" >
	        <AjaxSettings>
		        <telerik:AjaxSetting AjaxControlID="clCategories:CardListPanel">
			        <UpdatedControls>
				        <telerik:AjaxUpdatedControl ControlID="clCategories:CardListPanel" />
			        </UpdatedControls>
		        </telerik:AjaxSetting>
	        </AjaxSettings>
        </telerik:RadAjaxManager>

        <miles:MilesCardList ID="clCategories" runat="server" DataKeyNames="CategoryID"  AddNewRecordText="Add New Category"  CardMinHeight="200px"  >
        
            <SortExpressionList>
		        <miles:MilesCardListSortExpression DisplayName="CategoryName" FieldName="CategoryName" />
		        <miles:MilesCardListSortExpression DisplayName="Description" FieldName="Description" />
		        <miles:MilesCardListSortExpression DisplayName="Archived" FieldName="Archived" />
		        <miles:MilesCardListSortExpression DisplayName="DateCreated" FieldName="DateCreated" />
            </SortExpressionList>

	        <CardTemplate>
		        <CardHeaderTemplate>
        <h4>
        <%# Eval("CategoryName") %>
        <div class="pull-right">
        <asp:Image ID="btnInfo" runat="server"  ImageUrl="/Images/24x24/nav_blue_right.png" />
        </div>
        </h4>
		        </CardHeaderTemplate>
		        <CardItemTemplate>
				 
				          <div><strong>Description:</strong> <%# Eval("Description")%></div>
				          <div><strong>Archived:</strong> <%# Eval("Archived")%></div>
				          <div><strong>DateCreated:</strong> <%# Eval("DateCreated")%></div>

		        </CardItemTemplate>

		        <CardActionTemplate >
                       <asp:ImageButton ID="btnEdit" runat="server" SkinID="CardListEdit" CommandName="Edit" />                       
                </CardActionTemplate>

	         </CardTemplate>

            <CardEditTemplate ActionPosition="Bottom" DefaultButton="lnkSave">
                <CardItemTemplate>
                    <asp:Label ID="lblCategoryName3" Text="Category Name" runat="server" AssociatedControlID="txtCategoryName3" />
                    <asp:Label ID="Label2" runat="server" SkinID="Required" />
                    <asp:TextBox ID="txtCategoryName3" Text='<%# Eval("CategoryName")%>' runat="server" MaxLength="100" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvUserRole" ControlToValidate="txtCategoryName3"
                        ErrorMessage="Category Name is required" Display="None"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblDesc" Text="Description" runat="server" AssociatedControlID="txtDesc" />
                    <asp:TextBox ID="txtDesc" TextMode="MultiLine" Rows="3" Text='<%# Eval("Description")%>' runat="server" />
                </CardItemTemplate>

                <CardActionTemplate>
                    <asp:LinkButton ID="lnkSave" runat="server" SkinID="CardListSave" CommandName="PerformInsert" />
                    <asp:LinkButton ID="lnkCancel" runat="server" SkinID="CardListCancel" CausesValidation="false" CommandName="Cancel" />
                </CardActionTemplate>
            </CardEditTemplate>



     </miles:MilesCardList>










				</div>
			</div>
			</div>
		</div>
	</div>
	<div class="detail-footer">
<miles:MilesButton id="btnSaveClose" runat="server" Text="Save & Close" ActionType="Primary" />
<miles:MilesButton id="btnSave" runat="server" Text="Save" ActionType="Primary" />
<asp:Button id="btnCancel" runat="server" Text="Cancel" CausesValidation="false" SkinID="Cancel"  />
	</div>
</asp:Panel>












</asp:Content>

