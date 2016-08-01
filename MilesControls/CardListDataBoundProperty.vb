Imports System.Web.UI
Imports Telerik.Web.UI
Imports System.Web.UI.WebControls

<Themeable(True)>
Public Class CardListDataBoundProperty
    Inherits Control
    Implements ICardListDataPropery

#Region "Private Fields"
    Dim dataBoundPanel As New Panel
    Dim valueLabel As New Label
    Dim textLabel As New Label
#End Region

#Region "Properties"
    Public Property LabelText As String
        Get
            Return ViewState("LabelText")
        End Get
        Set(value As String)
            ViewState("LabelText") = value
        End Set
    End Property

    Public Property DataField As String
        Get
            Return ViewState("DataField")
        End Get
        Set(value As String)
            ViewState("DataField") = value
        End Set
    End Property

    Public Property DataFormatString As String
        Get
            Return ViewState("DataFormatString")
        End Get
        Set(value As String)
            ViewState("DataFormatString") = value
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

    Public Property NotEnteredText As String
        Get
            Return ViewState("NotEnteredText")
        End Get
        Set(value As String)
            ViewState("NotEnteredText") = value
        End Set
    End Property

    <Themeable(True)>
    Public Property LabelCssClass As String
        Get
            Return ViewState("LabelCssClass")
        End Get
        Set(value As String)
            ViewState("LabelCssClass") = value
        End Set
    End Property

    Public Property LabelDisplay As String
        Get
            Return ViewState("LabelDisplay")
        End Get
        Set(value As String)
            ViewState("LabelDisplay") = value
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



        MyBase.OnInit(e)
    End Sub

    Protected Overrides Sub CreateChildControls()
        dataBoundPanel.Visible = False

        'add controls
        dataBoundPanel.Controls.Add(textLabel)
        dataBoundPanel.Controls.Add(valueLabel)

        Me.Controls.Add(dataBoundPanel)

        MyBase.CreateChildControls()
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)


        'get the data from the dataitem
        If DataItem IsNot Nothing Then
            'defaults
            Dim formattedValue = Nothing
            Dim formattedValueCssClass = ""
            Dim hasData = True

            'get the value of the object's property
            Dim value = DataBinder.Eval(DataItem, DataField)
            If value IsNot Nothing AndAlso
                (Not TypeOf value Is Date OrElse (TypeOf value Is Date AndAlso Not value = Nothing)) AndAlso
                (Not TypeOf value Is String OrElse (TypeOf value Is String AndAlso Not String.IsNullOrWhiteSpace(value))) Then

                'format the string if we have a format string to use
                If Not String.IsNullOrWhiteSpace(DataFormatString) Then
                    formattedValue = String.Format(DataFormatString, value)
                Else
                    formattedValue = value.ToString()
                End If

                'control has data
                hasData = True
            Else
                hasData = False
                'change class
                formattedValueCssClass = "text-muted"
                'not entered
                formattedValue = NotEnteredText
            End If

            'set text label
            textLabel.Text = LabelText + " "
            textLabel.Visible = ShowLabel
            textLabel.Font.Bold = True
            textLabel.CssClass = LabelCssClass

            'set value label css
            valueLabel.CssClass = formattedValueCssClass
            'set value label's value
            valueLabel.Text = formattedValue

            'show the value if we have a value
            If (HideIfNoData And hasData) Or Not HideIfNoData Then
                dataBoundPanel.Visible = True
            End If

        End If

        MyBase.OnPreRender(e)
    End Sub

#End Region


End Class
