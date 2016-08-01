
Partial Class HR_DialogChangeUserPassword
    Inherits BasePage

#Region "Properties"

    Public Property UserId As Integer
        Get
            Dim v As Object = ViewState("UserId")
            If v Is Nothing Then
                v = ToInteger(Request("UserId"))
                ViewState("UserId") = v
            End If

            Return v
        End Get
        Set(value As Integer)
            ViewState("UserId") = value
        End Set
    End Property

#End Region

#Region "Page Functions"

    Protected Sub HR_DialogChangeUserPassword_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.SetFocus(txtPassword1)
        End If
    End Sub

#End Region

#Region "Methods"

    Private Function Save(Optional close As Boolean = False) As Boolean

        UserManager.UpdatePassword(UserId, txtPassword1.Text, UserAuthentication.User.UserID)

        Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
        'show success
        JGrowl.ShowMessage(JGrowlMessageType.Success, "Password updated successfully", useParent:=close And Not delay, isDelayed:=close And delay)

        Return True
    End Function

#End Region

#Region "Events"

    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then
            CloseDialogWindow()
        End If
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        CloseDialogWindow()

    End Sub
#End Region

End Class
