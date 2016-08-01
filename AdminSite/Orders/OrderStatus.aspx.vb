Imports Telerik.Web.UI
Imports System.Web.Services
Imports System.Data


Partial Class Orders_OrderStatus
    Inherits BasePage



#Region "Methods"

    Private Sub BindGrid()
        clOrderStatus.DataSource = Nothing
        clOrderStatus.AutoBind = True
        clOrderStatus.Rebind()
    End Sub

#End Region

#Region "Events"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            PopulateDropDown()
            'Set Default to pending
            ddlOrderStatus.SelectedValue = BusinessLibrary.OrderStatus.Pending
        End If
    End Sub

    Private Sub PopulateDropDown()
        'populate status ddl
        Dim ddllist2 = MetaTypeItemManager.GetList(8)
        ControlBinding.BindListControl(ddlOrderStatus, ddllist2, "ItemName", "ItemID", True)
    End Sub

    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        clOrderStatus.CurrentPageIndex = 0
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

    Protected Sub clOrderStatus_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles clOrderStatus.ItemDataBound
        'Set Check Box
        Dim chkPending As CheckBox = e.Item.FindControl("ItemSelect")
        Dim keyID = CType(e.Item, RadListViewDataItem).GetDataKeyValue("OrderID")
        Dim osObj = OrderStatusManager.GetById(keyID)
        chkPending.Visible = osObj.OrderStatusID = BusinessLibrary.OrderStatus.Pending

        'Set Toggle Btn, Should not be available for Shipped
        If osObj.OrderStatusID = BusinessLibrary.OrderStatus.Shipped Then
            Dim btnToggle As MilesToggleArchiveImageButton = e.Item.FindControl("btnArchiveRestore")
            btnToggle.Visible = False
        End If
    End Sub

    'Set status to Cancelled if record is NOT Canceled
    'Set status to Pending if record is Canceled
    Protected Sub clOrderStatus_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clOrderStatus.ToggleArchive
        Dim chkPending As CheckBox = e.ListViewItem.FindControl("ItemSelect")
        Dim keyID = CType(e.ListViewItem, RadListViewDataItem).GetDataKeyValue("OrderID")
        Dim osObj = OrderStatusManager.GetById(keyID)
        If osObj.OrderStatusID = BusinessLibrary.OrderStatus.Cancelled Then
            osObj.OrderStatusID = BusinessLibrary.OrderStatus.Pending
            OrderStatusManager.Save(osObj)
        Else
            osObj.OrderStatusID = BusinessLibrary.OrderStatus.Cancelled
            OrderStatusManager.Save(osObj)
        End If
        clOrderStatus.Rebind()
    End Sub

    Protected Sub clOrderStatus_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clOrderStatus.NeedDataSource

        If ddlOrderStatus.Text <> "" Then
            Dim SName As String
            'test find the ID not the name
            Using ctx As New DataLibrary.ModelEntities
                Dim obj = (From mt In ctx.MetaTypeItems
                           Where mt.ItemID = CInt(ddlOrderStatus.Text)
                           Select mt).SingleOrDefault()
                SName = obj.ItemName
            End Using

            Dim objList = OrderStatusManager.GetList(PageIndex:=clOrderStatus.CurrentPageIndex,
                                                    PageSize:=clOrderStatus.PageSize,
                                                    CustomerName:=ddlCustomerName.Text,
                                                    OrderStatusName:=SName,
                                                    DateFrom:=txtDateFrom.DbSelectedDate,
                                                    DateTo:=txtDateTo.DbSelectedDate)
            clOrderStatus.DataSource = objList
            If objList.Any Then
                clOrderStatus.VirtualItemCount = objList.FirstOrDefault.TotalCount
            End If
        ElseIf ddlOrderStatus.Text = "" Then
            Dim objList = OrderStatusManager.GetList(PageIndex:=clOrderStatus.CurrentPageIndex,
                                                    PageSize:=clOrderStatus.PageSize,
                                                    CustomerName:=ddlCustomerName.Text,
                                                    DateFrom:=txtDateFrom.DbSelectedDate,
                                                    DateTo:=txtDateTo.DbSelectedDate)
            clOrderStatus.DataSource = objList
            If objList.Any Then
                clOrderStatus.VirtualItemCount = objList.FirstOrDefault.TotalCount
            End If
        End If

    End Sub

    Protected Sub pnlAjax_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles pnlAjax.AjaxRequest
        BindGrid()
    End Sub

    'Approve

    Protected Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click

        If clOrderStatus.SelectedCardValues.Count = 0 Then
            JGrowl.ShowMessage(JGrowlMessageType.Alert, objectName:="Record", message:="No order is selected")
        Else
            For Each i As SelectedCard In clOrderStatus.SelectedCardValues
                'Change to accepted
                Dim oId As Integer = i.Value
                Dim osObj = OrderStatusManager.GetById(oId)
                Dim cusObj = CustomerManager.GetById(osObj.CustomerID)

                osObj.OrderStatusID = BusinessLibrary.OrderStatus.Accepted
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
                Dim str2 As String = String.Format("Your Order #{0} has been accepted.  We are now in the process of fulfilling your order and will notify you again once it has been shipped.", osObj.OrderID)
                Dim str3 As String = String.Format("Sincerely, </br> {0}", cName)

                Dim str As String = str1 + "</br></br>" + str2 + "</br></br>" + str3
                Functions.SendEmail(emailFrom:="luquan@milestechnologies.com", emailTo:="luquan@milestechnologies.com", emailSubject:="Order Accepted", emailBody:=str)
            Next
        End If

        clOrderStatus.Rebind()
    End Sub


    Protected Sub btnDeny_Click(sender As Object, e As EventArgs) Handles btnDeny.Click
        If clOrderStatus.SelectedCardValues.Count = 0 Then
            JGrowl.ShowMessage(JGrowlMessageType.Alert, objectName:="Record", message:="No order is selected")
        Else
            For Each i As SelectedCard In clOrderStatus.SelectedCardValues
                'Change to accepted
                Dim oId As Integer = i.Value
                Dim osObj = OrderStatusManager.GetById(oId)
                Dim cusObj = CustomerManager.GetById(osObj.CustomerID)

                osObj.OrderStatusID = BusinessLibrary.OrderStatus.Accepted
                OrderStatusManager.Save(osObj)

                Dim cName As String
                Dim cPhone As String
                Using ctx As New DataLibrary.ModelEntities
                    Dim ComName = (From a In ctx.AppSettings
                                   Where a.AppSettingID = 1
                                   Select a).SingleOrDefault()
                    cName = ComName.CompanyName
                    cPhone = ComName.Phone
                End Using

                'Send email
                Dim str1 As String = String.Format("Dear {0}:", cusObj.CustomerName)
                Dim str2 As String = String.Format("We have cancelled your Order #{0}.  Please contact us at {1}.", osObj.OrderID, cPhone)
                Dim str3 As String = String.Format("Sincerely, </br> {0}", cName)

                Dim str As String = str1 + "</br></br>" + str2 + "</br></br>" + str3
                Functions.SendEmail(emailFrom:="luquan@milestechnologies.com", emailTo:="luquan@milestechnologies.com", emailSubject:="Order Accepted", emailBody:=str)
            Next
        End If

        clOrderStatus.Rebind()

    End Sub






#End Region

End Class
