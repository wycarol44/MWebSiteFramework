
Partial Class HR_ManageJobTitles
    Inherits BasePage

#Region "Properties"

#End Region

#Region "Page Functions"

    Protected Sub HR_JobTitles_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.SetFocus(txtJobTitleName)
        End If
    End Sub

#End Region

#Region "Methods"
    Private Sub BindList()
        clJobTitle.DataSource = Nothing
        clJobTitle.AutoBind = True
        clJobTitle.Rebind()
    End Sub
#End Region

#Region "ListView"
    Protected Sub clJobTitle_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clJobTitle.ItemCommand
        If e.CommandName = "PerformInsert" Then 'RadListView.PerformInsertCommandName
            If TypeOf e.ListViewItem Is RadListViewInsertItem Then
                InsertUpdate(e, 0)
            ElseIf TypeOf e.ListViewItem Is RadListViewEditableItem Then
                Dim item As RadListViewDataItem = e.ListViewItem
                Dim keyID = item.GetDataKeyValue("JobTitleID")
                InsertUpdate(e, keyID)
            End If

        End If
    End Sub

    Protected Sub clJobTitle_ItemCreated(sender As Object, e As RadListViewItemEventArgs) Handles clJobTitle.ItemCreated
        If e.Item.IsInEditMode Then
            Dim txtUserRole As TextBox = e.Item.FindControl("txtJobTitle")
            If txtUserRole IsNot Nothing Then
                txtUserRole.Focus()
            End If
        End If
    End Sub

    Protected Sub clJobTitle_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clJobTitle.NeedDataSource
        clJobTitle.DataSource = UserJobTitleManager.GetList(
            txtJobTitleName.Text,
            ToNegBool(chkArchived.Checked))
    End Sub

    Protected Sub clJobTitle_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clJobTitle.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem
        Dim keyId As Integer = item.GetDataKeyValue("JobTitleID")

        UserJobTitleManager.ToggleArchived(keyId)

        BindList()
    End Sub

    Protected Sub InsertUpdate(ByVal e As Telerik.Web.UI.RadListViewCommandEventArgs, ByVal KeyID As Integer)
        If TypeOf e.ListViewItem Is RadListViewEditableItem Or TypeOf e.ListViewItem Is RadListViewInsertItem Then
            'get the edit item
            Dim editedItem As Telerik.Web.UI.RadListViewEditableItem = CType(e.ListViewItem, Telerik.Web.UI.RadListViewEditableItem)
            'Get the edit fields
            Dim txtJobTitle As TextBox = editedItem.FindControl("txtJobTitle")
            Dim txtJobDesc As TextBox = editedItem.FindControl("txtJobDesc")

            Dim jt = UserJobTitleManager.GetById(KeyID)
            jt.JobTitle = txtJobTitle.Text
            jt.JobDescription = txtJobDesc.Text

            UserJobTitleManager.Save(jt)

            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Job Title")

            clJobTitle.Rebind()
        End If
    End Sub
#End Region

#Region "Events"
    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindList()
    End Sub

#End Region


End Class
