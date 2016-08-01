
Partial Class HR_ManageUserRoles
    Inherits BasePage

    Protected Sub HR_ManageUserRoles_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Page.SetFocus(txtUserRoleName)
        End If
    End Sub
#Region "Methods"
    Private Sub BindList()
        clUserRole.DataSource = Nothing
        clUserRole.AutoBind = True
        clUserRole.Rebind()
    End Sub
#End Region

#Region "ListView"

    Protected Sub clUserRole_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clUserRole.ItemCommand
        If e.CommandName = "PerformInsert" Then 'RadListView.PerformInsertCommandName
            If TypeOf e.ListViewItem Is RadListViewInsertItem Then
                InsertUpdate(e, 0)
            ElseIf TypeOf e.ListViewItem Is RadListViewEditableItem Then
                Dim item As RadListViewDataItem = e.ListViewItem
                Dim keyID = item.GetDataKeyValue("UserRoleID")
                InsertUpdate(e, keyID)
            End If

        End If
    End Sub

    Protected Sub clUserRole_ItemCreated(sender As Object, e As RadListViewItemEventArgs) Handles clUserRole.ItemCreated
        If e.Item.IsInEditMode Then
            Dim txtUserRole As TextBox = e.Item.FindControl("txtUserRole")
            If txtUserRole IsNot Nothing Then
                txtUserRole.Focus()
            End If
        End If
    End Sub

    Protected Sub clUserRole_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles clUserRole.ItemDataBound
        If TypeOf e.Item Is RadListViewDataItem And e.Item.IsInEditMode = False Then
            Dim btnManageAccess As ImageButton = e.Item.FindControl("btnManageAccess")
            Dim item As RadListViewDataItem = e.Item
            Dim keyid As Integer = item.GetDataKeyValue("UserRoleID")
            btnManageAccess.Attributes.Add("onclick", String.Format("return openAccessDialog({0})", keyid))
        End If
    End Sub
    Protected Sub clUserRole_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clUserRole.NeedDataSource
        clUserRole.DataSource = UserRoleManager.GetList(
                                                txtUserRoleName.Text,
                                                ToNegBool(chkArchived.Checked))
    End Sub

    Protected Sub clUserRole_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clUserRole.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem
        Dim keyId As Integer = item.GetDataKeyValue("UserRoleID")

        UserRoleManager.ToggleArchived(keyId)

        BindList()
    End Sub

    Protected Sub InsertUpdate(ByVal e As Telerik.Web.UI.RadListViewCommandEventArgs, ByVal KeyID As Integer)
        If TypeOf e.ListViewItem Is RadListViewEditableItem Or TypeOf e.ListViewItem Is RadListViewInsertItem Then
            'get the edit item
            Dim editedItem As Telerik.Web.UI.RadListViewEditableItem = CType(e.ListViewItem, Telerik.Web.UI.RadListViewEditableItem)
            'Get the edit fields
            Dim txtUserRole As TextBox
            txtUserRole = editedItem.FindControl("txtUserRole")
            Dim txtUserRoleDesc As TextBox
            txtUserRoleDesc = editedItem.FindControl("txtUserRoleDesc")

            Dim ur As DataLibrary.UserRole = UserRoleManager.GetById(KeyID)
            ur.UserRoleName = txtUserRole.Text
            ur.UserRoleDesc = txtUserRoleDesc.Text

            UserRoleManager.Save(ur)

            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="User Role")

            clUserRole.Rebind()
        End If
    End Sub


#End Region

#Region "Events"
    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindList()
    End Sub
  
#End Region

  
End Class
