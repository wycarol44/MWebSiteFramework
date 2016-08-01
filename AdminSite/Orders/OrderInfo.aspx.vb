Imports Telerik.Web.UI
Imports System.Web.Services
Imports System.Data


Partial Class Orders_OrderInfo
    Inherits BasePage

#Region "Properties"
    Public Property OrderID As Integer
        Get
            Dim v As Object = ViewState("OrderID")
            If v Is Nothing Then
                v = ToInteger(Request("OrderID"))
                ViewState("OrderID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("OrderID") = value
        End Set
    End Property

#End Region


    Protected Sub btnSaveCon_Click(sender As Object, e As EventArgs) Handles btnSaveCon.Click
        orderheader.Save()
        clOrderDetails.Visible = True
        Label1.Visible = True
        lbloTotal.Visible = True
        BindGrid()
        'Page.ClientScript.RegisterStartupScript(Me.GetType, "OpenDialog", String.Format("function pageLoad() {{openEditDialog({0});}}", OrderID), True)
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Response.Redirect("~/Orders/ManageOrders.aspx")
    End Sub



#Region "Methods"

    Private Sub BindGrid()
        clOrderDetails.DataSource = Nothing
        clOrderDetails.AutoBind = True
        clOrderDetails.Rebind()
    End Sub

#End Region

#Region "CardList"

    Protected Sub clOrderDetails_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clOrderDetails.NeedDataSource
        Dim objList = OrderDetailManager.GetList(PageIndex:=clOrderDetails.CurrentPageIndex,
                                                 PageSize:=clOrderDetails.PageSize,
                                                 OrderID:=OrderID)
        clOrderDetails.DataSource = objList
        If objList.Any Then
            clOrderDetails.VirtualItemCount = objList.FirstOrDefault.TotalCount
        End If

        If objList.Count > 0 Then
            Dim totalMoney As String = String.Format("{0:c}", objList.FirstOrDefault().OrderTotal)
            lbloTotal.Text = totalMoney
        End If

    End Sub

    Protected Sub rdAjaxManager_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles rdAjaxManager.AjaxRequest
        BindGrid()
    End Sub
#End Region

    Protected Sub clOrderDetails_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clOrderDetails.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem
        Dim keyId As Integer = item.GetDataKeyValue("OrderDetailID")
        OrderDetailManager.ToggleArchived(keyId)

        Cal_OrderTotal(OrderID)
        clOrderDetails.Rebind()
        Dis_OrderTotal(OrderID)
    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            clOrderDetails.OnAddNewRecordClientClick = "return openEditDialog(0, " & OrderID & ");"
            BindGrid()
            If OrderID > 0 Then
                clOrderDetails.Visible = True
                Label1.Visible = True
                lbloTotal.Visible = True
                BindGrid()
            End If
        End If

        'Dis_OrderTotal(OrderID)
    End Sub

    Protected Sub Dis_OrderTotal(OrderID As Integer)
        Dim OrderTotal = OrderDetailManager.GetList(PageIndex:=clOrderDetails.CurrentPageIndex,
                                 PageSize:=clOrderDetails.PageSize,
                                 OrderID:=OrderID).FirstOrDefault()


    End Sub

    Protected Sub Cal_OrderTotal(OrderID As Integer)
        Dim oobj = OrderManager.GetById(OrderID)
        Using ctx As New DataLibrary.ModelEntities
            oobj.OrderTotal = ctx.Calculate_OrderTotal(OrderID).SingleOrDefault()
        End Using
        OrderID = OrderManager.Save(oobj)
    End Sub

End Class
