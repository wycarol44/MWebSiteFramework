
Partial Class CRM_CustomerInfo
    Inherits BasePage

#Region "Properties"

    Public Property CustomerID As Integer
        Get
            Dim v As Object = ViewState("CustomerID")
            If v Is Nothing Then
                v = ToInteger(Request("CustomerID"))
                ViewState("CustomerID") = v
            End If

            Return v
        End Get
        Set(value As Integer)
            ViewState("CustomerID") = value
        End Set
    End Property


    Public Property AuditLogCount As Integer
        Get
            Return ViewState("AuditLogCount")
        End Get
        Set(value As Integer)
            ViewState("AuditLogCount") = value
        End Set
    End Property



#End Region

#Region "Page Functions"

    Protected Sub CRM_CustomerInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            SetHeader()
            If CustomerID > 0 Then
                Populate()
            Else
                ShowStatusDropdown()

                Page.SetFocus(txtName)
            End If
        End If
    End Sub

#End Region

#Region "Methods"
    ''' <summary>
    ''' Sets the header visibility based on key ids
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetHeader()
        pnlHeader.Visible = (CustomerID > 0)
    End Sub

    Private Sub ShowStatusDropdown()
        ddlStatus.Visible = True
        lblStatusRequired.Visible = True
        lblStatus.Visible = False
    End Sub



    ''' <summary>
    ''' Populate the form with Customer Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Populate()

        Dim obj = CustomerManager.GetById(CustomerID)

        'set page title
        PageHeader = obj.CustomerName + " - Basic Info"

        'populate fields
        txtName.Text = obj.CustomerName
        lblStatusRequired.Visible = False
        ddlStatus.Visible = False
        lblStatus.Visible = True
        PopulateStatusLabel(obj.StatusID)

        'Audit Log Count
        PopulateAuditLog()



        txtWebSite.Text = obj.Website
        txtNotes.Content = obj.Notes

        txtPhone.Text = obj.Phone
        milesAddr.AddressID = obj.AddressID.GetValueOrDefault()


        'set entity footer
        SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)
    End Sub

    Private Sub PopulateAuditLog()
        Dim alCount = AuditLogManager.GetCount(MilesMetaObjects.Customers, CustomerID)
        Dim btnViewAuditLog As RadToolBarButton = rdTools.FindButtonByCommandName("ViewAuditLog")
        Dim alCounter = <span class="badge"><%= alCount %></span>

        btnViewAuditLog.Text = "Audit Log " & alCounter.ToString()
    End Sub

    Private Sub PopulateStatusLabel(statusid As Integer)
        lblStatus.Text = MetaTypeItemManager.GetById(statusid).ItemName
        'reconfigure the toolbar after populating status
        ConfigureActionsToolBar(statusid)
    End Sub

  
    Private Sub ConfigureActionsToolBar(statusid As MilesMetaTypeItem)
        Dim btnActive As RadToolBarButton = rdTools.FindButtonByCommandName("Active")
        Dim btnPending As RadToolBarButton = rdTools.FindButtonByCommandName("Pending")
        Dim btnLost As RadToolBarButton = rdTools.FindButtonByCommandName("Lost")
        Dim btnInActive As RadToolBarButton = rdTools.FindButtonByCommandName("InActive")

        btnActive.Visible = True
        btnPending.Visible = True
        btnLost.Visible = True
        btnInActive.Visible = True

        'assign id's to buttons
        btnActive.CommandArgument = MilesMetaTypeItem.CustomerStatusActive
        btnPending.CommandArgument = MilesMetaTypeItem.CustomerStatusPending
        btnLost.CommandArgument = MilesMetaTypeItem.CustomerStatusLost
        btnInActive.CommandArgument = MilesMetaTypeItem.CustomerStatusInactive

        'hide the selected item to visible 
        Select Case statusid
            Case MilesMetaTypeItem.CustomerStatusActive
                btnActive.Visible = False
            Case MilesMetaTypeItem.CustomerStatusInactive
                btnInActive.Visible = False
            Case MilesMetaTypeItem.CustomerStatusLost
                btnLost.Visible = False
            Case MilesMetaTypeItem.CustomerStatusPending
                btnPending.Visible = False
        End Select
    End Sub

    ''' <summary>
    ''' Saves or updates the record
    ''' </summary>
    ''' <param name="close"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Save(Optional close As Boolean = False) As Boolean


        Try

            Dim obj = CustomerManager.GetById(CustomerID)
            Dim newEntity As Boolean = (CustomerID = 0)


            obj.CustomerName = txtName.Text
            If CustomerID = 0 Then obj.StatusID = ddlStatus.StatusID 'status updated from ddl only for new records
            obj.Website = txtWebSite.Text
            obj.Notes = txtNotes.Content
            obj.NotesText = txtNotes.Text
            obj.Phone = txtPhone.Text

            'save address
            obj.AddressID = milesAddr.Save()

            'save customer
            CustomerID = CustomerManager.Save(obj)

            'after saving the record, we need to update some stuff on the page

            If newEntity Then
                ' save status chage log
                SaveAuditLogStatusChange(obj.StatusID)
            End If

            'add params to tab
            rdTabs.AddQueryParameter("CustomerID", CustomerID)

            'show the header now
            SetHeader()

            'set entity footer
            SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)

            'success
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Customer", isDelayed:=close)

            Return True

        Catch wex As WarningException

            'show a friendly message
            JGrowl.ShowMessage(JGrowlMessageType.Error, wex.Message)

            'return false
            Return False

        Catch ex As Exception
            'fail
            Throw
        End Try
    End Function

    Private Sub SaveAuditLogStatusChange(NewStatusID As Integer)

        Dim NewStatus As String = MetaTypeItemManager.GetById(NewStatusID).ItemName

        'Save Audit Log
        AuditLogManager.SaveAuditLog(AuditLogType.Insert, MilesMetaObjects.Customers, CustomerID, NewStatusID, MilesMetaAuditLogAttributes.Status, NewStatus)

    End Sub

    Public Sub CloseForm()
        If ReferrerPageURL IsNot Nothing Then
            Navigate(ReferrerPageURL.ToString())
        Else
            Navigate("~/CRM/ManageCustomers.aspx")
        End If
    End Sub


#End Region

#Region "Events"

    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then
            CloseForm()
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Save()
        PopulateAuditLog()
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseForm()
    End Sub

    Protected Sub rdTools_ButtonClick(sender As Object, e As RadToolBarEventArgs) Handles rdTools.ButtonClick
        If TypeOf e.Item Is RadToolBarButton Then
            Dim button As RadToolBarButton = e.Item
            If button.Value = "Active" Or _
                button.Value = "Pending" Or _
                button.Value = "Lost" Or _
                button.Value = "Inactive" Then

                Try
                    'update the status
                    CustomerManager.UpdateStatus(CustomerID, button.CommandArgument)

                    'change status shown in status label and reconfigure toolbar
                    PopulateStatusLabel(button.CommandArgument)
                    PopulateAuditLog()
                    JGrowl.ShowMessage(JGrowlMessageType.Success, "Customer Status Changed")
                Catch wex As WarningException
                    JGrowl.ShowMessage(JGrowlMessageType.Error, wex.Message)

                Catch ex As Exception
                    Throw
                End Try


            End If
        End If
    End Sub
#End Region

End Class
