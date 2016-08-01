Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports Telerik.Web.UI

<ParseChildren(True)>
<ValidationProperty("Text")>
Public Class MilesEmail
    Inherits CompositeControl
    'Implements INamingContainer

#Region "Properties"
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property EmailTextBox As TextBox
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property EmailExpressionValidator As RegularExpressionValidator

    Public Property Text As String
        Get
            'return value
            Return EmailTextBox.Text
        End Get
        Set(value As String)
            EmailTextBox.Text = value
        End Set
    End Property


#End Region

    Public Sub New()

        If EmailTextBox Is Nothing Then EmailTextBox = New TextBox()
        If EmailExpressionValidator Is Nothing Then EmailExpressionValidator = New RegularExpressionValidator

        'set defaults
        If String.IsNullOrWhiteSpace(EmailExpressionValidator.ValidationExpression) Then
            EmailExpressionValidator.ValidationExpression = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"
        End If
        If String.IsNullOrWhiteSpace(EmailExpressionValidator.ErrorMessage) Then
            EmailExpressionValidator.ErrorMessage = "Please enter a valid email address"
        End If

    End Sub

#Region "Overrides"
    'Protected Overrides Sub OnInit(e As EventArgs)

    '    EmailTextBox.ID = "EmailTextBox"
    '    EmailTextBox.Attributes.Add("placeholder", "example@domain.com")
    '    EmailTextBox.TextMode = TextBoxMode.Email
    '    EmailTextBox.EnableTheming = False


    '    EmailExpressionValidator.ID = "rev" + EmailTextBox.ID
    '    EmailExpressionValidator.ControlToValidate = EmailTextBox.ID


    '    'add controls to panel
    '    Me.Controls.Add(EmailTextBox)
    '    Me.Controls.Add(EmailExpressionValidator)

    '    MyBase.OnInit(e)
    'End Sub

    Protected Overrides Sub CreateChildControls()
        EmailTextBox.ID = "EmailTextBox"
        EmailTextBox.Attributes.Add("placeholder", "example@domain.com")
        EmailTextBox.TextMode = TextBoxMode.Email
        EmailTextBox.EnableTheming = False


        EmailExpressionValidator.ID = "rev" + EmailTextBox.ID
        EmailExpressionValidator.ControlToValidate = EmailTextBox.ID


        'add controls to panel
        Me.Controls.Add(EmailTextBox)
        Me.Controls.Add(EmailExpressionValidator)
        MyBase.CreateChildControls()
    End Sub

#End Region
End Class
