
Partial Class Admin_ManageCategories
    Inherits BasePage

#Region "Page Functions"

    Protected Sub CRM_ManageCustomers_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            BindGrid()
        End If
    End Sub

#End Region


    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindGrid()
    End Sub

#Region "Grid"
    Protected Sub clCategories_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clCategories.ItemCommand
        If e.CommandName = "InitInsert" Then
            Navigate("~/Admin/DialogCategories.aspx?CategoryID=0")
        End If
    End Sub


#End Region

#Region "CardList"

    Private Sub BindGrid()
        clCategories.DataSource = Nothing
        clCategories.AutoBind = True
        clCategories.Rebind()
    End Sub

    Private Sub clCategories_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clCategories.NeedDataSource
        Dim objList = CategoryManager.GetList(PageIndex:=clCategories.CurrentPageIndex,
                                            PageSize:=clCategories.PageSize,
                                            CategoryName:=txtCategoryName.Text,
                                            Description:=txtDescription.Text,
                                            DateCreatedFrom:=txtFrom.DbSelectedDate,
                                            DateCreatedTo:=txtTo.DbSelectedDate,
                                            Archived:=ToNegBool(chkArchived.Checked),
                                            SortExpression:=clCategories.CurrentSortExpression.FieldName,
                                            SortOrder:=clCategories.CurrentSortExpression.SortOrder)
        clCategories.DataSource = objList
    End Sub

    Protected Sub clCategories_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clCategories.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem
        Dim keyId As Integer = item.GetDataKeyValue("CategoryID")

        Using ctx As New DataLibrary.ModelEntities
            ctx.Category_ToggleArchive(keyId)
        End Using

        BindGrid()
    End Sub



#End Region



End Class
