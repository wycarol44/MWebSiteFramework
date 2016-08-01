Imports System.Web.UI
Imports Telerik.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel

Public Class CardListHyperLinkProperty
    Inherits Control
    Implements ICardListDataPropery

#Region "Private Fields"
    Dim dataBoundPanel As Control
    Dim textLabel As New Label
    Dim valueLabel As New Label
    Dim valueLink As New HyperLink
#End Region

#Region "Properties"

    Public Property RenderMode As UpdatePanelRenderMode
        Get
            Return ViewState("RenderMode")
        End Get
        Set(value As UpdatePanelRenderMode)
            ViewState("RenderMode") = value
        End Set
    End Property

    Public Property LabelText As String
        Get
            Return ViewState("LabelText")
        End Get
        Set(value As String)
            ViewState("LabelText") = value
        End Set
    End Property

    Public Property DataTextField As String
        Get
            Return ViewState("DataTextField")
        End Get
        Set(value As String)
            ViewState("DataTextField") = value
        End Set
    End Property

    Public Property Text As String
        Get
            Return ViewState("Text")
        End Get
        Set(value As String)
            ViewState("Text") = value
        End Set
    End Property

    <TypeConverterAttribute(GetType(TargetConverter))>
    Public Property Target As String
        Get
            Return ViewState("Target")
        End Get
        Set(value As String)
            ViewState("Target") = value
        End Set
    End Property

    Public Property DataTextFormatString As String
        Get
            Return ViewState("DataTextFormatString")
        End Get
        Set(value As String)
            ViewState("DataTextFormatString") = value
        End Set
    End Property

    Public Property DataNavigateUrlFields As String
        Get
            Return ViewState("DataNavigateUrlFields")
        End Get
        Set(value As String)
            ViewState("DataNavigateUrlFields") = value
        End Set
    End Property

    Public Property DataNavigateUrlFormatString As String
        Get
            Return ViewState("DataNavigateUrlFormatString")
        End Get
        Set(value As String)
            ViewState("DataNavigateUrlFormatString") = value
        End Set
    End Property

    Public Property HideIfNoData As Boolean
        Get
            Return ViewState("HideIfNoData")
        End Get
        Set(value As Boolean)
            ViewState("HideIfNoData") = value
        End Set
    End Property

    Public Property AlwaysLink As Boolean
        Get
            Return ViewState("AlwaysLink")
        End Get
        Set(value As Boolean)
            ViewState("AlwaysLink") = value
        End Set
    End Property

    Public Property ShowLabel As Boolean
        Get
            Return ViewState("ShowLabel")
        End Get
        Set(value As Boolean)
            ViewState("ShowLabel") = value
        End Set
    End Property

    Public Property NotEnteredText As String
        Get
            Return ViewState("NotEnteredText")
        End Get
        Set(value As String)
            ViewState("NotEnteredText") = value
        End Set
    End Property

    Public Property LabelCssClass As String
        Get
            Return ViewState("LabelCssClass")
        End Get
        Set(value As String)
            ViewState("LabelCssClass") = value
        End Set
    End Property

    Public Property DataItem As Object Implements ICardListDataPropery.DataItem
#End Region

#Region "Constructor"
    Public Sub New()

        NotEnteredText = "not entered"
        ShowLabel = True
    End Sub
#End Region

#Region "Overrides"

    Protected Overrides Sub OnInit(e As EventArgs)

        If RenderMode = UpdatePanelRenderMode.Block Then
            dataBoundPanel = New Panel
        Else
            dataBoundPanel = New Label
        End If

        dataBoundPanel.Visible = False

        
        'add controls
        dataBoundPanel.Controls.Add(textLabel)
        dataBoundPanel.Controls.Add(valueLabel)
        dataBoundPanel.Controls.Add(valueLink)

        Me.Controls.Add(dataBoundPanel)

        MyBase.OnInit(e)
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)

        'get the data from the dataitem
        If DataItem IsNot Nothing Then
            'defaults
            Dim formattedValue = Nothing
            Dim hasData = True

            'get the value of the object's property
            Dim value = DataBinder.Eval(DataItem, DataTextField)
            If value IsNot Nothing AndAlso
                (Not TypeOf value Is Date OrElse (TypeOf value Is Date AndAlso Not value = Nothing)) AndAlso
                (Not TypeOf value Is String OrElse (TypeOf value Is String AndAlso Not String.IsNullOrWhiteSpace(value))) Then

                'format the string if we have a format string to use
                If Not String.IsNullOrWhiteSpace(DataTextFormatString) Then
                    formattedValue = String.Format(DataTextFormatString, value)
                Else
                    formattedValue = value.ToString()
                End If

                'control has data
                hasData = True
            Else
                hasData = False
            End If

            'set value label css
            valueLabel.CssClass = "text-muted"
            'set value label's value
            valueLabel.Text = NotEnteredText


            'show the value if we have a value
            If (HideIfNoData And hasData) Or Not HideIfNoData Then
                dataBoundPanel.Visible = True

                'set the link text
                valueLink.Text = If(formattedValue IsNot Nothing, formattedValue, NotEnteredText)
                ' set the link target window
                valueLink.Target = Target
                'build url

                If Not String.IsNullOrWhiteSpace(DataNavigateUrlFormatString) Then
                    valueLink.NavigateUrl = ResolveUrl(String.Format(DataNavigateUrlFormatString, GetUrlFieldData()))
                End If

                'show the label if we have show label as true
                textLabel.Visible = ShowLabel
                textLabel.CssClass = LabelCssClass
                textLabel.Font.Bold = True
                textLabel.Text = LabelText + " "

                'if we have data, show the value link, else, show the label
                valueLabel.Visible = (Not hasData And Not AlwaysLink)
                valueLink.Visible = (hasData Or AlwaysLink)

            End If

        End If

        MyBase.OnPreRender(e)
    End Sub




#End Region

#Region "Methods"
    ''' <summary>
    ''' Gets a list of objects from the url data fields
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetUrlFieldData() As Object()
        'build a list of objects
        Dim objList As New List(Of Object)

        'get the data from the data item
        If DataNavigateUrlFields IsNot Nothing Then
            'get the fields
            Dim fields = DataNavigateUrlFields.Split({","}, StringSplitOptions.RemoveEmptyEntries)

            'loop through each field and get the data, then add it to the list
            For Each field In fields
                Dim value = DataBinder.Eval(DataItem, field)
                objList.Add(value)
            Next

        End If

        'return the list as an array
        Return objList.ToArray()
    End Function
#End Region

End Class
