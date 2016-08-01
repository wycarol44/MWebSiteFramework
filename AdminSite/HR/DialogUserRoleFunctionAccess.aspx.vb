
Partial Class HR_DialogUserRoleFunctionAccess
    Inherits BasePage

    Protected Property UserRoleID As Integer
        Get
            Dim v As Object = ViewState("UserRoleID")
            If v Is Nothing Then
                v = CInt(Val(Request("UserRoleID")))
                ViewState("UserRoleID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("UserRoleID") = value
        End Set
    End Property

    Protected Sub HR_DialogUserRoleFunctionAccess_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If UserRoleID > 0 Then
                BindTreeView()
            End If
        End If
    End Sub


#Region "Methods"
    Private Sub BindTreeView()
        tvFunctions.DataSource = MetaFunctionManager.GetAllFunctionWithModules(UserRoleID)
        tvFunctions.DataBind()
        CheckUnCheckAll()
    End Sub
    Private Sub CheckUnCheckAll()
        If tvFunctions.GetAllNodes.Count = tvFunctions.CheckedNodes.Count Then
            rbtnCheckAll.SelectedValue = "CheckAll"
        ElseIf tvFunctions.CheckedNodes.Count = 0 Then
            rbtnCheckAll.SelectedValue = "UnCheckAll"
        Else
            rbtnCheckAll.SelectedIndex = -1
        End If
    End Sub
    Private Function Save(Optional close As Boolean = False) As Boolean

        'selected functions are checked nodes whose value are >0 i.e id>0 modules has -ve id
        Dim SelectedfunctionIds = tvFunctions.CheckedNodes.Where(Function(x) _
                                                                     x.Value.ToInteger() > 0) _
                                                                 .Select(Function(x) x.Value.ToInteger()).ToArray()

        UserRoleManager.SaveFunctions(UserRoleID, SelectedfunctionIds)
        UserManager.ClearAllFunctionAccessCache()

        Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
        'show success
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Function Access", useParent:=close And Not delay, isDelayed:=close And delay)

        Return True
    End Function
#End Region

#Region "Events"

    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then


            CloseDialogWindow(UserRoleID)

        End If
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub

    Protected Sub tvFunctions_NodeCheck(sender As Object, e As RadTreeNodeEventArgs) Handles tvFunctions.NodeCheck
        CheckUnCheckAll()
    End Sub

    Protected Sub tvFunctions_NodeDataBound(sender As Object, e As RadTreeNodeEventArgs) Handles tvFunctions.NodeDataBound
        'check functions that were arleady assigned to userrole
        Dim f As MetaFunctions_GetAllFunctionsWithModules_Result = DirectCast(e.Node.DataItem, MetaFunctions_GetAllFunctionsWithModules_Result)

        'only check for functions, functions has id>0
        If f.ID > 0 Then
            e.Node.Checked = f.HasAccess
        Else
            e.Node.Font.Bold = True
        End If
    End Sub

    Protected Sub rbtnCheckAll_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles rbtnCheckAll.SelectedIndexChanged
        If rbtnCheckAll.SelectedValue = "CheckAll" Then
            tvFunctions.CheckAllNodes()
        ElseIf rbtnCheckAll.SelectedValue = "UnCheckAll" Then
            tvFunctions.UncheckAllNodes()
        End If
    End Sub
#End Region

End Class
