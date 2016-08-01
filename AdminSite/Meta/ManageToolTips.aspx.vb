
Partial Class Meta_ManageToolTips
    Inherits BasePage

    Protected Sub Meta_ManageToolTips_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Page.SetFocus(txtToolTip)
        End If
    End Sub
#Region "Methods"

    Private Sub BindList()
        clToolTips.DataSource = Nothing
        clToolTips.AutoBind = True
        clToolTips.Rebind()
    End Sub
#End Region

#Region "Grid"
    Protected Sub clToolTips_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clToolTips.NeedDataSource
        Dim objlist = MetaToolTipManager.GetList(ToNull(txtToolTip.Text))
        clToolTips.DataSource = objlist
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
