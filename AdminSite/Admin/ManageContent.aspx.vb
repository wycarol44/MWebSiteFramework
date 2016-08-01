
Partial Class Admin_ManageContent
    Inherits BasePage

    Protected Sub Admin_ManageContent_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Page.SetFocus(txtCategory)
        End If
    End Sub


#Region "Methods"
    Private Sub BindGrid()
        clContent.DataSource = Nothing
        clContent.AutoBind = True
        clContent.Rebind()
    End Sub
#End Region

    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindGrid()
    End Sub

    Protected Sub clContent_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles clContent.ItemDataBound
        If TypeOf e.Item Is RadListViewDataItem Then
            Dim item As RadListViewDataItem = e.Item
            If item IsNot Nothing Then
                Dim cms As CMSCategory = item.DataItem
                Dim type As String = ""
                If cms.ContentTypeID = MilesMetaTypeItem.CMSContentTypeEmail Then
                    type = "Email"
                ElseIf cms.ContentTypeID = MilesMetaTypeItem.CMSContentTypeWebsite Then
                    type = "Website"
                ElseIf cms.ContentTypeID = MilesMetaTypeItem.CMSContentTypeHyperlink Then
                    type = "Hyperlink"
                End If
                Dim lblType As Label = e.Item.FindControl("lblType")
                If lblType IsNot Nothing Then lblType.Text = type
            End If
        End If
    End Sub

    Protected Sub clContent_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clContent.NeedDataSource
        Dim ContentTypeIDs As New List(Of Integer)
        For Each ci As RadComboBoxItem In ddlContentType.CheckedItems
            ContentTypeIDs.Add(ci.Value)
        Next

        clContent.DataSource = CMSCategoryManager.GetList(ToNull(txtCategory.Text), _
                                                      ContentTypeIDs, _
                                                      ToNegBool(chkArchived.Checked))
    End Sub

    Protected Sub clContent_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clContent.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem
        Dim keyId As Integer = item.GetDataKeyValue("CategoryID")

        CMSCategoryManager.ToggleArchived(keyId)
        BindGrid()
    End Sub
End Class
