
Partial Class CRM_CustomerDocuments
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

    Protected Sub CRM_CustomerDocuments_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            LoadDocuments()
        End If
    End Sub

    Private Sub LoadDocuments()
        milesDocuments.ObjectID = MilesMetaObjects.Customers
        milesDocuments.KeyID = CustomerID
        PageHeader = CustomerManager.GetById(CustomerID).CustomerName + " - Documents"
    End Sub
End Class
