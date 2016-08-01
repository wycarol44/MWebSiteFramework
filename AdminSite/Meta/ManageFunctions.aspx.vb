
Partial Class Meta_ManageFunctions
    Inherits BasePage

    Protected Sub Meta_ManageFunctions_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Page.SetFocus(txtFunctionName)
        End If
    End Sub

#Region "Methods"

    Private Sub BindList()
        clFunctions.DataSource = Nothing
        clFunctions.AutoBind = True
        clFunctions.Rebind()
    End Sub
#End Region

#Region "Grid"
    Protected Sub clFunctions_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clFunctions.NeedDataSource
        Dim objlist = MetaFunctionManager.GetList(ToNull(txtFunctionName.Text), _
                                                      ToNull(txtModule.Text), _
                                                      ToNull(txtForm.Text), _
                                                      ToNegBool(chkArchived.Checked))
        clFunctions.DataSource = objlist
    End Sub

    Protected Sub clFunctions_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clFunctions.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem
        Dim keyId As Integer = item.GetDataKeyValue("FunctionID")
        MetaFunctionManager.ToggleArchived(keyId)
        BindList()
    End Sub

#End Region

#Region "Events"
    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindList()
    End Sub

    Protected Sub rdAjaxManager_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles rdAjaxManager.AjaxRequest
        BindList()
    End Sub
#End Region

  
End Class
