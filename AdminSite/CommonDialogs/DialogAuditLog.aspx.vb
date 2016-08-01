
Partial Class CommonDialogs_DialogAuditLog
    Inherits BasePage

#Region "Properties"

    Public Property ObjectID As MilesMetaObjects
        Get
            Dim v As Object = ViewState("ObjectID")
            If v Is Nothing Then
                v = ToInteger(Request("ObjectID"))
                ViewState("ObjectID") = v
            End If

            Return v
        End Get
        Set(value As MilesMetaObjects)
            ViewState("ObjectID") = value
        End Set
    End Property

    Public Property KeyID As Integer
        Get
            Dim v As Object = ViewState("KeyID")
            If v Is Nothing Then
                v = ToInteger(Request("KeyID"))
                ViewState("KeyID") = v
            End If

            Return v
        End Get
        Set(value As Integer)
            ViewState("KeyID") = value
        End Set
    End Property

#End Region

#Region "Page Functions"

    Protected Sub CommonDialogs_DialogAuditLog_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PopulateAuditLog()
            PageHeader = ObjectID.ToString & " Audit Log"
        End If
    End Sub

#End Region

#Region "Methods"

    Private Sub PopulateAuditLog()
        ucAuditLog.ObjectID = ObjectID
        ucAuditLog.KeyID = KeyID
        ucAuditLog.Populate()
    End Sub

#End Region

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub
End Class
