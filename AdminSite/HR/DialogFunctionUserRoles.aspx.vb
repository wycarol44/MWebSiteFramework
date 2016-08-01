
Partial Class HR_DialogFunctionUserRoles
    Inherits BasePage

#Region "Properties"
    ReadOnly Property FunctionID As Integer
        Get
            Return Request("FunctionID").ToInteger()
        End Get
    End Property
    ReadOnly Property FunctionName As String
        Get
            Return Request("FunctionName")
        End Get
    End Property
#End Region

    Protected Sub HR_DialogFunctionUserRoles_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            label1.Text = label1.Text & "<strong>'" & FunctionName & "'</strong> "
            BindUserRoles()
        End If
    End Sub

#Region "Methods"
    Private Sub BindUserRoles()
        dlUserRoles.DataSource = Nothing
        dlUserRoles.DataSource = UserRoleManager.GetByFunctionID(FunctionID)
        dlUserRoles.DataBind()
    End Sub


    Protected Sub dlUserRoles_ItemDataBound(sender As Object, e As DataListItemEventArgs) Handles dlUserRoles.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim chkUserRole As CheckBox = e.Item.FindControl("chkUserRole")
            Dim ur As UserRoles_GetByFunctionID_Result = e.Item.DataItem
            chkUserRole.Text = ur.UserRoleName
            chkUserRole.Attributes("UserRoleID") = ur.UserRoleID
            chkUserRole.Checked = ur.HasAccess
        End If
    End Sub


    Private Function Save(Optional close As Boolean = False) As Boolean

        Dim SelecteduserroleIds As New List(Of Integer)

        For Each item As DataListItem In dlUserRoles.Items
            If item.ItemType = ListItemType.AlternatingItem Or item.ItemType = ListItemType.Item Then
                Dim chkUserRole As CheckBox = item.FindControl("chkUserRole")

                If chkUserRole.Checked Then
                    SelecteduserroleIds.Add(chkUserRole.Attributes("UserRoleID").ToInteger())
                End If
            End If
        Next

        MetaFunctionManager.SaveUserRoles(FunctionID, SelecteduserroleIds.ToArray())

        Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
        'show success
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Function Access", useParent:=close And Not delay, isDelayed:=close And delay)


        Return True
    End Function
#End Region

#Region "Events"

    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then
            UserManager.ClearAllFunctionAccessCache()
            CloseDialogWindow(FunctionID)
        End If

    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub

#End Region

End Class
