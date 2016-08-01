
Partial Class HR_ManageAccess
    Inherits BasePage

    Protected Sub HR_ManageAccess_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Page.SetFocus(txtFunctionName)
            BindTreeList()
        End If
    End Sub

    Private Sub BindTreeList()
        tlFunctions.DataSource = Nothing
        tlFunctions.Rebind()
    End Sub

    Protected Sub tlFunctions_ItemDataBound(sender As Object, e As TreeListItemDataBoundEventArgs) Handles tlFunctions.ItemDataBound
        If TypeOf e.Item Is TreeListDataItem Then
            Dim item As TreeListDataItem = e.Item

            Dim fun As MetaFunctions_GetAllFunctionsWithModules_Result = item.DataItem
            Dim lnkManageAccess As HyperLink = e.Item.FindControl("lnkManageAccess")
            '  lnkManageAccess.Attributes.Add("onclick", String.Format("OpenModalWindow('/HR/UserRoles/DialogUserRoleList.aspx?FunctionID={0}&FunctionName={1}','{2}','{3}','{4}');", fun.FunctionID, fun.Name, "Select User Roles", 500, 325))

            'for parent node (modules) make the hyperlink invisible
            If fun.ID < 0 Then
                e.Item.Cells(2).Controls(0).Visible = False
            End If
        End If
    End Sub

    Protected Sub tlFunctions_NeedDataSource(sender As Object, e As TreeListNeedDataSourceEventArgs) Handles tlFunctions.NeedDataSource
        tlFunctions.DataSource = MetaFunctionManager.GetAllFunctionWithModules(FunctionName:=txtFunctionName.Text) '

    End Sub

    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindTreeList()
        If Trim(txtFunctionName.Text) <> "" Then
            tlFunctions.ExpandAllItems()
        Else
            tlFunctions.CollapseAllItems()
        End If
    End Sub
End Class
