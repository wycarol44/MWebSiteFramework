
Partial Class Meta_ManageForms
    Inherits BasePage

    Protected Sub Meta_ManageForms_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Page.SetFocus(txtFormName)
        End If
    End Sub

#Region "Methods"
    Private Sub BindList()
        clForms.DataSource = Nothing
        clForms.AutoBind = True
        clForms.Rebind()
    End Sub
#End Region

#Region "ListView"
    Protected Sub clForms_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clForms.ItemCommand
        If e.CommandName = "ToggleFavorite" Then
            Dim item As RadListViewDataItem = e.ListViewItem
            Dim keyId As Integer = item.GetDataKeyValue("FormID")
            MetaFormManager.ToggleCanBeFavorite(keyId)
            BindList()
        End If
    End Sub

    Protected Sub clForms_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles clForms.ItemDataBound
        If TypeOf e.Item Is RadListViewDataItem Then
            Dim item As RadListViewDataItem = e.Item
            Dim obj As MetaForm = item.DataItem
            Dim btnFavorite As ImageButton = e.Item.FindControl("btnCanBeFavorite")
            If obj.CanBeFavorite Then
                btnFavorite.ImageUrl = "/Images/24x24/favorite.png"
                btnFavorite.ToolTip = "Click to disable favorite"
            Else
                btnFavorite.ImageUrl = "/Images/24x24/favorite-disabled.png"
                btnFavorite.ToolTip = "Click to enable favorite"

            End If
        End If
    End Sub

    Protected Sub clForms_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clForms.NeedDataSource
        clForms.DataSource = MetaFormManager.GetGridList(ToNull(txtFormName.Text), _
                                                        ToNull(txtFormPath.Text), _
                                                        ToNull(txtModule.Text), _
                                                        ToNegBool(chkCanBeFavourite.Checked))
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
