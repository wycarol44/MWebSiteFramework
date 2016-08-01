Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports Telerik.Web.UI
Imports System.Text.RegularExpressions

<ParseChildren(True)>
Public Class MilesPhone
    Inherits Panel
    Implements INamingContainer

#Region "Properties"
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property PhoneTextBox As RadTextBox
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property ExtTextBox As RadTextBox
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property ExtLabel As Label

    Public Property Text As String
        Get
            Dim value As String = PhoneTextBox.Text

            'remove any non-number
            Dim rgx As New Regex("[^0-9]")
            value = rgx.Replace(value, "")

            'return value
            Return value
        End Get
        Set(value As String)
            PhoneTextBox.Text = value
        End Set
    End Property

    Public Property Extension As String
        Get
            Return ExtTextBox.Text
        End Get
        Set(value As String)
            ExtTextBox.Text = value
        End Set
    End Property

    Public Property Skin As String
        Get
            Return ViewState("Skin")
        End Get
        Set(value As String)
            ViewState("Skin") = value
        End Set
    End Property

    Public Property ShowExt As Boolean
        Get
            Return ExtTextBox.Visible
        End Get
        Set(value As Boolean)
            ExtTextBox.Visible = value
            ExtLabel.Visible = value
        End Set
    End Property

    'Public Overrides ReadOnly Property ClientID As String
    '    Get
    '        Return PhoneTextBox.ClientID
    '    End Get
    'End Property

#End Region

    Public Sub New()

        If PhoneTextBox Is Nothing Then
            PhoneTextBox = New RadTextBox()


        End If

        If ExtTextBox Is Nothing Then
            ExtTextBox = New RadTextBox()

        End If

        If ExtLabel Is Nothing Then
            ExtLabel = New Label()

        End If

        If String.IsNullOrWhiteSpace(ExtLabel.Text) Then
            ExtLabel.Text = "x&nbsp;"
        End If

    End Sub

#Region "Overrides"
    Protected Overrides Sub OnInit(e As EventArgs)

        PhoneTextBox.ID = "PhoneTextBox"
        PhoneTextBox.Skin = Skin
        PhoneTextBox.EnableTheming = False
        'PhoneTextBox.InputType = Html5InputType.Number
        'PhoneTextBox.Attributes.Add("type", "phone")

        Dim phoneWidth = 70
        If Not ShowExt Then
            phoneWidth = 100
        End If

        PhoneTextBox.Width = Unit.Percentage(phoneWidth)
        PhoneTextBox.ClientEvents.OnLoad = "MilesPhone_Load"
        PhoneTextBox.ClientEvents.OnBlur = "MilesPhone_Blur"


        ExtTextBox.ID = "ExtTextBox"
        ExtTextBox.Skin = Skin
        ExtTextBox.EnableTheming = False
        ExtTextBox.Width = Unit.Percentage(20)
        ExtTextBox.MaxLength = 5

        ExtLabel.Width = Unit.Percentage(10)
        ExtLabel.Style.Add("text-align", "right")
        ExtLabel.Style.Add("line-height", "34px")
        ExtLabel.Style.Add("vertical-align", "middle")

        'add controls to panel
        Me.Controls.Add(PhoneTextBox)
        Me.Controls.Add(ExtLabel)
        Me.Controls.Add(ExtTextBox)

        MyBase.OnInit(e)
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)

        'output some javascript
        Dim milesPhoneJs = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesPhone.js")

        'link javascript
        ScriptManager.RegisterClientScriptInclude(Page, Page.GetType(), "MilesPhone", milesPhoneJs)

        MyBase.OnPreRender(e)
    End Sub


#End Region

End Class
