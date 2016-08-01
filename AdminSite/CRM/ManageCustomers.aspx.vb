
Partial Class CRM_ManageCustomers
    Inherits BasePage

#Region "Page Functions"

    Protected Sub CRM_ManageCustomers_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            SearchActionToolBarSettings()
        End If
    End Sub

#End Region

#Region "Methods"

    Private Sub BindGrid()
        clCustomers.DataSource = Nothing
        clCustomers.AutoBind = True
        clCustomers.Rebind()
    End Sub

    Private Sub ExportCustomers()

        Dim cList = CustomerManager.GetList(0, 0,
                                      CustomerName:=txtCustomerName.Text,
                                      phone:=txtPhone.Text,
                                      StatusIDs:=ddlStatusTypes.ToXMLIdentifiers(),
                                      ContactName:=txtContactName.Text,
                                      ContactEmail:=txtEmail.Text,
                                      Archived:=ToNegBool(chkArchived.Checked),
                                      SortExpression:=clCustomers.CurrentSortExpression.FieldName,
                                      SortOrder:=clCustomers.CurrentSortExpression.SortOrder)

        Dim colsToInclude As String()
        Dim colHeaderNames As String()
        colsToInclude = {"CustomerName", "Phone", "PrimaryContact", "Status"}
        colHeaderNames = {"Customer Name", "Phone", "Primary Contact", "Status"}

        ExportToExcel(cList.ToList, colsToInclude, colHeaderNames, "CustomersList")
    End Sub

#End Region

#Region "Events"

    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        clCustomers.CurrentPageIndex = 0
        BindGrid()
    End Sub

#End Region

#Region "CardList"

    Protected Sub clCustomers_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clCustomers.ItemCommand
        If e.CommandName = "InitInsert" Then
            Navigate("~/CRM/CustomerInfo.aspx?CustomerID=0")
        End If
    End Sub

    Protected Sub clCustomers_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clCustomers.NeedDataSource

        Dim cList = CustomerManager.GetList(PageIndex:=clCustomers.CurrentPageIndex,
                                            PageSize:=clCustomers.PageSize,
                                            CustomerName:=txtCustomerName.Text,
                                            phone:=txtPhone.Text,
                                            StatusIDs:=ddlStatusTypes.ToXMLIdentifiers(),
                                            ContactName:=txtContactName.Text,
                                            ContactEmail:=txtEmail.Text,
                                            Archived:=ToNegBool(chkArchived.Checked),
                                            SortExpression:=clCustomers.CurrentSortExpression.FieldName,
                                            SortOrder:=clCustomers.CurrentSortExpression.SortOrder)
        clCustomers.DataSource = cList

        If cList.Any Then
            clCustomers.VirtualItemCount = cList.FirstOrDefault.TotalCount
        End If

        For Each p As System.Reflection.PropertyInfo In cList.GetType().GetProperties()
            If p.CanRead Then
                Response.Write("{0}: {1}", p.Name, "")
            End If
        Next

    End Sub

    Protected Sub clCustomers_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clCustomers.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem

        Dim keyId As Integer = item.GetDataKeyValue("CustomerID")

        'archive the Customer
        CustomerManager.ToggleArchived(keyId)

        clCustomers.Rebind()
    End Sub

#End Region

    Private Sub SearchActionToolBarSettings()
        Dim rtbButtonExportCustomers As RadToolBarButton = pnlSearch.CommandToolBar.FindButtonByCommandName("ExportCustomers")
        rtbButtonExportCustomers.Visible = UserManager.HasFunctionAccess(MilesMetaFunctions.CanExportCustomers)
    End Sub

    Protected Sub pnlSearch_ToolBarButtonClick(sender As Object, e As RadToolBarEventArgs) Handles pnlSearch.ToolBarButtonClick
        Dim btn As RadToolBarButton = e.Item
        Select Case btn.CommandName
            Case "ExportCustomers"

                ExportCustomers()

        End Select

    End Sub
End Class
