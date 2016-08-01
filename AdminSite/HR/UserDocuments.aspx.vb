
Partial Class HR_UserDocuments
    Inherits BasePage


#Region "Properties"

    Public Property UserID As Integer
        Get
            Dim v As Object = ViewState("UserID")
            If v Is Nothing Then
                v = ToInteger(Request("UserID"))
                ViewState("UserID") = v
            End If

            Return v
        End Get
        Set(value As Integer)
            ViewState("UserID") = value
        End Set
    End Property

#End Region

#Region "Methods"

    Protected Sub CRM_CustomerDocuments_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            LoadDocuments()
        End If
    End Sub


    Private Sub LoadDocuments()
        milesDocuments.ObjectID = MilesMetaObjects.Users
        milesDocuments.KeyID = UserID
        PageHeader = UserManager.GetById(UserID).Fullname + " - Documents"
    End Sub
#End Region

End Class
