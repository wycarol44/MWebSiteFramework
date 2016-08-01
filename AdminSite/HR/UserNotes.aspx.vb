
Partial Class HR_UserNotes
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

    Protected Sub CRM_CustomerNotes_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If UserID > 0 Then
                PopulateMilesNotesControl()
            Else
                Navigate("/CRM/ManageUsers.aspx")
            End If
        End If
    End Sub

    Private Sub PopulateMilesNotesControl()
        'set page title
        PageHeader = UserManager.GetById(UserID).Fullname + " - Notes"

        milesNotes.ObjectID = MilesMetaObjects.Users
        milesNotes.KeyID = UserID
    End Sub

End Class
