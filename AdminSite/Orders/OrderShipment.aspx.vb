Imports Telerik.Web.UI
Imports System.Web.Services
Imports System.Data

Partial Class Orders_OrderShipment
    Inherits BasePage


#Region "Methods"

    Private Sub BindGrid()
        clOrderShipment.DataSource = Nothing
        clOrderShipment.AutoBind = True
        clOrderShipment.Rebind()
    End Sub

#End Region

#Region "Events"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        End If
    End Sub

    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        clOrderShipment.CurrentPageIndex = 0
        BindGrid()
    End Sub

    Protected Sub ddlCustomerName_Load(sender As Object, e As EventArgs) Handles ddlCustomerName.Load
        'Populate Name ddl
        Dim ddlList = CustomerManager.GetList()
        ControlBinding.BindListControl(ddlCustomerName, ddlList, "CustomerName", "CustomerID", , True)
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


#End Region

#Region "CardList"

    Protected Sub clOrderShipment_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clOrderShipment.NeedDataSource
        Dim objList = OrderShipmentManager.GetList(PageIndex:=clOrderShipment.CurrentPageIndex,
                                                    PageSize:=clOrderShipment.PageSize,
                                                    CustomerName:=ddlCustomerName.Text,
                                                    DateFrom:=txtDateFrom.DbSelectedDate,
                                                    DateTo:=txtDateTo.DbSelectedDate)
        clOrderShipment.DataSource = objList
        If objList.Any Then
            clOrderShipment.VirtualItemCount = objList.FirstOrDefault.TotalCount
        End If
    End Sub

    Protected Sub pnlAjax_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles pnlAjax.AjaxRequest
        BindGrid()
    End Sub

    'Approve

    Protected Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click

        If clOrderShipment.SelectedCardValues.Count = 0 Then
            JGrowl.ShowMessage(JGrowlMessageType.Alert, objectName:="Record", message:="No order is selected")
        Else
            Response.Write(clOrderShipment.SelectedItems.Count)

            For Each i As SelectedCard In clOrderShipment.SelectedCardValues
                'Change to accepted
                Dim oId As Integer = i.Value
                Dim osObj = OrderStatusManager.GetById(oId)
                Dim cusObj = CustomerManager.GetById(osObj.CustomerID)

                osObj.OrderStatusID = BusinessLibrary.OrderStatus.Shipped

                Dim txtTrackingNumber As TextBox = clOrderShipment.Items(i.Index).FindControl("txtTrackingNumber")
                osObj.TrackingNumber = txtTrackingNumber.Text
                OrderStatusManager.Save(osObj)

                'Send email
                Dim cName As String
                Using ctx As New DataLibrary.ModelEntities
                    Dim ComName = (From a In ctx.AppSettings
                                   Where a.AppSettingID = 1
                                   Select a).SingleOrDefault()
                    cName = ComName.CompanyName
                End Using

                Dim str1 As String = String.Format("Dear {0}:", cusObj.CustomerName)
                Dim str2 As String = String.Format("Your Order #{0} has been shipped. You can track your package using {1}", osObj.OrderID, osObj.TrackingNumber)
                Dim str3 As String = String.Format("Sincerely, </br> {0}", cName)

                Dim str As String = str1 + "</br></br>" + str2 + "</br></br>" + str3
                Functions.SendEmail(emailFrom:="luquan@milestechnologies.com", emailTo:="luquan@milestechnologies.com", emailSubject:="Order Accepted", emailBody:=str)
            Next
        End If
        clOrderShipment.Rebind()
    End Sub

    Protected Sub btnDeny_Click(sender As Object, e As EventArgs) Handles btnDeny.Click

        If clOrderShipment.SelectedCardValues.Count = 0 Then
            JGrowl.ShowMessage(JGrowlMessageType.Alert, objectName:="Record", message:="No order is selected")
        Else
            For Each i As SelectedCard In clOrderShipment.SelectedCardValues
                'Change to accepted
                Dim oId As Integer = i.Value
                Dim osObj = OrderStatusManager.GetById(oId)
                Dim cusObj = CustomerManager.GetById(osObj.CustomerID)

                osObj.OrderStatusID = BusinessLibrary.OrderStatus.Cancelled
                OrderStatusManager.Save(osObj)

                'Send email
                Dim cName As String
                Dim cPhone As String
                Using ctx As New DataLibrary.ModelEntities
                    Dim ComName = (From a In ctx.AppSettings
                                   Where a.AppSettingID = 1
                                   Select a).SingleOrDefault()
                    cName = ComName.CompanyName
                    cPhone = ComName.Phone
                End Using

                Dim str1 As String = String.Format("Dear {0}:", cusObj.CustomerName)
                Dim str2 As String = String.Format("We have cancelled your Order #{0}.  Please contact us at {1}.", osObj.OrderID, cPhone)
                Dim str3 As String = String.Format("Sincerely, </br> {0}", cName)

                Dim str As String = str1 + "</br></br>" + str2 + "</br></br>" + str3
                Functions.SendEmail(emailFrom:="luquan@milestechnologies.com", emailTo:="luquan@milestechnologies.com", emailSubject:="Order Accepted", emailBody:=str)
            Next
        End If
        clOrderShipment.Rebind()
    End Sub

#End Region


End Class
