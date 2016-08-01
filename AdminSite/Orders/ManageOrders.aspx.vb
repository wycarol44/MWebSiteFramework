Imports Telerik.Web.UI
Imports System.Web.Services
Imports System.Data



Partial Class Orders_ManageOrders
    Inherits BasePage


#Region "Methods"

    Private Sub BindGrid()
        clOrders.DataSource = Nothing
        clOrders.AutoBind = True
        clOrders.Rebind()
    End Sub

#End Region

#Region "Events"

    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        clOrders.CurrentPageIndex = 0
        BindGrid()
    End Sub

#End Region

#Region "CardList"

    Protected Sub clOrders_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clOrders.NeedDataSource

        Dim objList = OrderManager.GetList(PageIndex:=clOrders.CurrentPageIndex,
                                                PageSize:=clOrders.PageSize,
                                                CustomerName:=ddlCustomerName.Text,
                                                DateFrom:=txtDateFrom.DbSelectedDate,
                                                DateTo:=txtDateTo.DbSelectedDate,
                                                Archived:=ToNegBool(chkArchived.Checked))

        clOrders.DataSource = objList

        If objList.Any Then
            clOrders.VirtualItemCount = objList.FirstOrDefault.TotalCount
        End If
    End Sub

    Protected Sub pnlAjax_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles pnlAjax.AjaxRequest
        BindGrid()
    End Sub
#End Region

    Protected Sub lblCustomerName_Load(sender As Object, e As EventArgs) Handles lblCustomerName.Load
        Dim ddlList = CustomerManager.GetList()
        ddlCustomerName.DataSource = ddlList
        ddlCustomerName.DataTextField = "CustomerName"
        ddlCustomerName.DataValueField = "CustomerID"
        ddlCustomerName.DataBind()
    End Sub

    Private Const ItemsPerRequest As Integer = 10

    Protected Sub ddlCustomerName_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        Dim data2 = CustomerManager.GetList()
        Dim data As New DataTable()
        data.LoadDataRow(data2, True) 'Change array to datatable

        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count)
        e.EndOfItems = endOffset = data.Rows.Count

        For i As Integer = itemOffset To endOffset - 1
            ddlCustomerName.Items.Add(New RadComboBoxItem(data.Rows(i)("CustomerName").ToString(), data.Rows(i)("CustomerName").ToString()))
        Next
    End Sub

    Protected Sub clOrders_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clOrders.ToggleArchive
        OrderManager.ToggleArchived(CType(e.ListViewItem, RadListViewDataItem).GetDataKeyValue("OrderID"))
        clOrders.Rebind()
    End Sub

    Protected Sub clOrders_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clOrders.ItemCommand
        If e.CommandName = "InitInsert" Then
            Navigate("~/Orders/OrderInfo.aspx?OrderID=0")
        End If
    End Sub

End Class
