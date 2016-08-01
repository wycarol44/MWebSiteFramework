
Partial Class AddressUserControl
    Inherits System.Web.UI.UserControl

    Public Property Address() As String
        Get
            Return TextBoxAddress.Text
        End Get
        Set(ByVal value As String)
            TextBoxAddress.Text = value
        End Set
    End Property
    Public Property City() As String
        Get
            Return TextBoxCity.Text
        End Get
        Set(ByVal value As String)
            TextBoxCity.Text = value
        End Set
    End Property
    Public Property StateProvince() As String
        Get
            Return TextBoxStateProv.Text
        End Get
        Set(ByVal value As String)
            TextBoxStateProv.Text = value
        End Set
    End Property
    Public Property PostalCode() As String
        Get
            Return TextBoxPostalCode.Text
        End Get
        Set(ByVal value As String)
            TextBoxPostalCode.Text = value
        End Set
    End Property

    Public Event SaveButtonClick(ByVal sender As Object, ByVal e As EventArgs)

    Protected Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        RaiseEvent SaveButtonClick(Me, New EventArgs())
    End Sub

End Class
