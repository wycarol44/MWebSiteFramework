
Partial Class CRM_ContactDashboard
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

    Protected Sub CRM_ContactDashboard_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If ContactID > 0 And CustomerID > 0 Then
                Dim contactdetails As DataLibrary.CustomerContacts_GetDetailsByID_Result = _
                            CustomerContactManager.GetDetailsById(ContactID)
                SetPageHeader(contactdetails)
                SetEntityFooter(contactdetails.CreatedBy, contactdetails.DateCreated, contactdetails.ModifiedBy, contactdetails.DateModified)
                PopulateBasicInfo(contactdetails)
            ElseIf CustomerID > 0 Then
                Navigate("/CRM/ManageContacts.aspx?CustomerID=" & CustomerID)
            Else
                Navigate("/CRM/ManageCustomers.aspx")
            End If
        End If
    End Sub

    Private Sub SetPageHeader(obj As DataLibrary.CustomerContacts_GetDetailsByID_Result)
        Dim customerName As String = CustomerManager.GetById(obj.CustomerID).CustomerName
        PageHeader = customerName & " : " & obj.Firstname & " " & obj.Lastname & " - Dashboard"
    End Sub

    Private Sub PopulateBasicInfo(obj As DataLibrary.CustomerContacts_GetDetailsByID_Result)
        lnkBasicInfoHeader.NavigateUrl = "/CRM/ContactInfo.aspx?CustomerID=" & CustomerID & "&ContactID=" & ContactID

        lblIsPrimary.Visible = obj.IsPrimary
        lnkName.Text = obj.Firstname & " " & obj.Lastname
        lnkName.NavigateUrl = "/CRM/ContactInfo.aspx?CustomerID=" & CustomerID & "&ContactID=" & ContactID
        lblTitle.Text = obj.Title
        lnkEmail.Text = obj.Email
        lnkEmail.NavigateUrl = "mailto:" & obj.Email

        lblPhone.Text = (<div><a href=<%= "tel:" & obj.WorkPhone %>><%= FormatPhone(PhoneNumberType.Work, obj.WorkPhone, obj.WorkPhoneExt) %></a></div>).ToString() &
                       (<div><a href=<%= "tel:" & obj.HomePhone %>><%= FormatPhone(PhoneNumberType.Home, obj.HomePhone) %></a></div>).ToString() &
                       (<div><a href=<%= "tel:" & obj.MobilePhone %>><%= FormatPhone(PhoneNumberType.Mobile, obj.MobilePhone) %></a></div>).ToString()

        lblAddress.Text = obj.FullAddress

        If Not String.IsNullOrEmpty(obj.FullAddress) Then
            Dim btnViewMap As RadToolBarButton = rdActions.FindButtonByCommandName("ViewMap")
            If btnViewMap IsNot Nothing Then
                btnViewMap.Attributes.Add("onclick", String.Format("javascript:window.open('http://maps.google.com/?q={0}');", Replace(obj.FullAddress, "<br />", " ")))
            End If
        End If
    End Sub
End Class
