
Partial Class CRM_CustomerNotes
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
#End Region

    Protected Sub CRM_CustomerNotes_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If CustomerID > 0 Then
                PopulateMilesNotesControl()
            Else
                Navigate("/CRM/ManageCustomers.aspx")
            End If
        End If
    End Sub

    Private Sub PopulateMilesNotesControl()
        'set page title
        PageHeader = CustomerManager.GetById(CustomerID).CustomerName + " - Notes"

        milesNotes.ObjectID = MilesMetaObjects.Customers
        milesNotes.KeyID = CustomerID
    End Sub

End Class
