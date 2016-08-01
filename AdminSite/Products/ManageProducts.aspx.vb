
Partial Class Products_ManageProducts
    Inherits BasePage



    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindGrid()
    End Sub


#Region "CardList"
    Private Sub BindGrid()
        clProducts.DataSource = Nothing
        clProducts.AutoBind = True
        clProducts.Rebind()
    End Sub


    Private Sub clProducts_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clProducts.NeedDataSource
        Dim objList = ProductManager.GetList(PageIndex:=clProducts.CurrentPageIndex,
                                            PageSize:=clProducts.PageSize,
                                            ProductName:=txtProductName.Text,
                                            Description:=txtDescription.Text,
                                            DateCreatedFrom:=txtDateFrom.DbSelectedDate,
                                            DateCreatedTo:=txtDateTo.DbSelectedDate,
                                            Archived:=ToNegBool(chkArchived.Checked),
                                            SortExpression:=clProducts.CurrentSortExpression.FieldName,
                                            SortOrder:=clProducts.CurrentSortExpression.SortOrder)
        clProducts.DataSource = objList
    End Sub


    Protected Sub clProducts_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clProducts.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem
        Dim keyId As Integer = item.GetDataKeyValue("ProductID")
        ProductManager.ToggleArchived(keyId)
        clProducts.Rebind()
    End Sub


    Protected Sub clProducts_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clProducts.ItemCommand
        If e.CommandName = "InitInsert" Then
            Navigate("~/Products/ProductsInfo.aspx?ProductID=0")
        End If
    End Sub

#End Region










End Class
