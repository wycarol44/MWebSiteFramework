<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MilesNotes.ascx.vb" Inherits="UserControls_MilesNotes" %>

<telerik:RadScriptBlock ID="rdCodeBlock" runat="server">
    <script>
        function openEditDialog(id) {
            openWindow('/CommonDialogs/DialogAddEditNotes.aspx?NoteID=' + id + '&ObjectID=<%= ObjectID%>&KeyID=<%= KeyID%>', 'Notes', WINDOW_LARGE, dialogClosed);
        }

        function dialogClosed(sender, args) {
            var arg = args.get_argument();

            if (arg != null) {
                $find('<%= rdAjaxPanel.ClientID%>').ajaxRequest();
            }
        }

    </script>
</telerik:RadScriptBlock>


<miles:MilesSearchPanel ID="pnlSearch" runat="server" ListControlID="dgList">

    <ContentPanel>
        <div class="row">
            <div class="col-md-2">
                <asp:Label ID="lblNotes" runat="server" Text="Notes" AssociatedControlID="txtNotes" />
                <asp:TextBox ID="txtNotes" runat="server" />
            </div>

            <div class="col-md-2">
                <asp:Label ID="lblType" runat="server" Text="Type" AssociatedControlID="ddlTypes" />
                <miles:MilesTypeComboBox ID="ddlTypes" runat="server" StatusType="NoteType" IncludeDefaultItem="false"
                    CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="All" />
            </div>

            <div class="col-md-2">
                <asp:Label ID="lblCreatedBy" runat="server" Text="Created By" AssociatedControlID="ddlCreatedBy" />
                <miles:MilesUserComboBox ID="ddlCreatedBy" EmptyMessage="All" runat="server"></miles:MilesUserComboBox>
            </div>

            <div class="col-md-3">
                <asp:Label ID="lblDateRange" runat="server" Text="Date Created Range" AssociatedControlID="dtDateFrom:dateInput" />
                <div class="row">
                    <div class="col-sm-6">
                        <miles:MilesDateTimePicker ID="dtDateFrom" DateInput-EmptyMessage="from" runat="server"></miles:MilesDateTimePicker>
                    </div>
                    <div class="col-sm-6">
                        <miles:MilesDateTimePicker ID="dtDateTo" DateInput-EmptyMessage="to" runat="server"></miles:MilesDateTimePicker>
                    </div>
                </div>
            </div>

            <div class="col-md-2">
                <br />
                <asp:CheckBox ID="chkArchived" runat="server" Text="Include archived" CssClass="chk-inline" />
            </div>

        </div>
    </ContentPanel>

</miles:MilesSearchPanel>


<hr />
<telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" LoadingPanelID="rdAjaxLoadingPanel">

    <miles:MilesCardList ID="clNotes" AutoBind="true" OnAddNewRecordClientClick="openEditDialog(0);" runat="server" DataKeyNames="NoteID" 
        AddNewRecordText="Add New Note" CardMinHeight="225px">

        <SortExpressionList>

            <miles:MilesCardListSortExpression DisplayName="Title" FieldName="Title" />
            <miles:MilesCardListSortExpression DisplayName="Note Type" FieldName="NoteType" />
            <miles:MilesCardListSortExpression DisplayName="Date Created" FieldName="DateCreated" />
            <miles:MilesCardListSortExpression DisplayName="Created By" FieldName="CreatedBy" />
            <miles:MilesCardListSortExpression DisplayName="Date Modified" FieldName="DateModified" />
            <miles:MilesCardListSortExpression DisplayName="Modified By" FieldName="ModifiedBy" />

        </SortExpressionList>

        <CardTemplate HeaderLinkType="HyperLink" >

            <CardHeaderTemplate >
                <h4>
                    <asp:Label ID="lblPin" Visible='<%# If(Eval("Pinned") <> 0, False)%>' runat="server" CssClass="glyphicon glyphicon-pushpin"></asp:Label>
                    <asp:Label ID="lblLinkIcon" Visible='<%# If(Eval("NoteTypeID") = MilesMetaTypeItem.NotesTypeLink , False)%>' runat="server" CssClass="glyphicon glyphicon-link"></asp:Label>
                    <asp:Label ID="lblNoteIcon" Visible='<%# If(Eval("NoteTypeID") = MilesMetaTypeItem.NotesTypeNote , False)%>' runat="server" CssClass="glyphicon glyphicon-list-alt"></asp:Label>

                    <%# Eval("Title") %>
                </h4>

            </CardHeaderTemplate>

            <CardItemTemplate>
                <asp:HiddenField ID="hdNoteID" runat="server" Value='<%# Eval("NoteID")%>' />
                <asp:HiddenField ID="hdNoteType" runat="server" Value='<%# Eval("NoteTypeID")%>' />
                <asp:HiddenField ID="hdLinkURL" runat="server" Value='<%# Eval("LinkURL")%>' />
               
               <asp:Label ID="lblNotes" Text='<%# Eval("NotesText")%>' runat="server"></asp:Label>
            <asp:LinkButton ID="lnkViewNotes" Text="..." runat="server">

            </asp:LinkButton>

                <em>
                    <div class="text-muted">- Created By   <%# Eval("CreatedByName")%> On:<%# ToFormattedShortDateString(Eval("DateCreated"))%></div>
                </em>
                <asp:Panel ID="pnlModifiedBy" Visible='<%# If(Eval("ModifiedBy") <> 0, False)%>' runat="server">
                    <em>
                        <div class="text-muted">- Modified By   <%# Eval("ModifiedByName")%> On:<%# ToFormattedShortDateString(Eval("DateModified"))%></div>
                    </em>

                </asp:Panel>

              
            </CardItemTemplate>

            <CardActionTemplate>
                
                <asp:ImageButton ID="ibEdit" runat="server" ToolTip="Edit" ImageUrl="/Images/24x24/document-edit.png" CommandName="Edit" />
                <miles:MilesToggleArchiveImageButton ID="btnDelete" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />

            </CardActionTemplate>
        </CardTemplate>

   

    </miles:MilesCardList>

    
</telerik:RadAjaxPanel>
