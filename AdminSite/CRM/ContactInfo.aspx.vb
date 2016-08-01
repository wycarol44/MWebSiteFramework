
Partial Class CRM_ContactInfo
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

    Public Property ContactID As Integer
        Get
            Dim v As Object = ViewState("ContactID")
            If v Is Nothing Then
                v = ToInteger(Request("ContactID"))
                ViewState("ContactID") = v
            End If

            Return v
        End Get
        Set(value As Integer)
            ViewState("ContactID") = value
        End Set
    End Property
#End Region

#Region "Page Functions"

    Protected Sub CRM_ContactInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            SetHeader()
            If ContactID > 0 Then
                Populate()
            Else
                Page.SetFocus(txtFirstName)

                'if adding new contact and the contact is first unarchived contact for the customer, check is primary checkbox by default
                If CustomerContactManager.IsFirstContact(CustomerID) Then
                    chkPrimaryContact.Checked = True
                End If
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
        pnlHeader.Visible = (ContactID > 0)
    End Sub


    ''' <summary>
    ''' Populate the form with Customer Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Populate()

        Dim obj = CustomerContactManager.GetById(ContactID)

        'set page title
        PageHeader = CustomerManager.GetById(CustomerID).CustomerName & ": " & obj.Firstname + " " + obj.Lastname + " - Basic Info"

        'populate fields
        txtFirstName.Text = obj.Firstname
        txtLastName.Text = obj.Lastname
        txtTitle.Text = obj.Title
        txtNotes.Content = obj.Notes
        chkPrimaryContact.Checked = obj.IsPrimary

        txtEmail.Text = obj.Email
        txtHomePhone.Text = obj.HomePhone
        txtWorkPhone.Text = obj.WorkPhone
        txtWorkPhone.Extension = obj.WorkPhoneExt
        txtMobilePhone.Text = obj.MobilePhone

        milesAddr.AddressID = obj.AddressID.GetValueOrDefault()

        'set entity footer
        SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)
    End Sub

    ''' <summary>
    ''' Saves or updates the record
    ''' </summary>
    ''' <param name="close"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Save(Optional close As Boolean = False) As Boolean

        Try
            Dim obj = CustomerContactManager.GetById(ContactID)

            obj.CustomerID = CustomerID
            obj.Firstname = txtFirstName.Text
            obj.Lastname = txtLastName.Text
            obj.Title = txtTitle.Text
            obj.Notes = txtNotes.Content
            obj.NotesText = txtNotes.Text
            obj.IsPrimary = chkPrimaryContact.Checked

            obj.Email = txtEmail.Text
            obj.HomePhone = txtHomePhone.Text
            obj.MobilePhone = txtMobilePhone.Text
            obj.WorkPhone = txtWorkPhone.Text
            obj.WorkPhoneExt = txtWorkPhone.Extension

            'save address
            obj.AddressID = milesAddr.Save()

            'save customer
            ContactID = CustomerContactManager.Save(obj)

            'after saving the record, we need to update some stuff on the page

            'add params to tab
            ' rdTabs.AddQueryParameter("CustomerID", CustomerID)

            'show the header now
            ' SetHeader()

            'set entity footer
            SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)

            'success
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Contact", isDelayed:=close)

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

    Public Sub CloseForm()
        If ReferrerPageURL IsNot Nothing Then
            Navigate(ReferrerPageURL.ToString())
        Else
            Navigate("~/CRM/ManageContacts.aspx?CustomerID=" & CustomerID)
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
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
      
            CloseForm()
    End Sub
#End Region

    
End Class
