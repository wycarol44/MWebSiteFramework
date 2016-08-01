
Partial Class SystemPages_SessionExpired
    Inherits BasePage

    Protected Sub SystemPages_SessionExpired_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session.Abandon()

        If Not IsPostBack Then
            'set the link to go back to the url we came from
            Dim returnUrl = Request("returnUrl")
            If Not String.IsNullOrWhiteSpace(returnUrl) Then
                lnkGoBack.NavigateUrl = returnUrl
            Else
                lnkGoBack.NavigateUrl = "~/Hello.aspx"
            End If
        End If

    End Sub
End Class
