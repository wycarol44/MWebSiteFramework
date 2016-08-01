Imports Telerik.Web.UI
Imports System.Web.UI

Public Class MilesButton
    Inherits RadButton

#Region "Enums"
    Public Enum ActionTypes
        Primary
        Secondary
        Neutral
        Negative
    End Enum
#End Region

#Region "Properties"

    ''' <summary>
    ''' Gets or sets buttons of similar action. All buttons will be disabled when this button is clicked
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RelatedButtons As String
        Get
            Return ViewState("RelatedButtons")
        End Get
        Set(value As String)
            ViewState("RelatedButtons") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the action type of the button. Skin will be automatically set if EnableAutoSkins is true
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ActionType As ActionTypes
        Get
            Return ViewState("ActionType")
        End Get
        Set(value As ActionTypes)
            ViewState("ActionType") = value
        End Set
    End Property


    ''' <summary>
    ''' Gets or sets whether other buttons of the same action are disabled
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutoDisableSameButtonType As Boolean
        Get
            Return ViewState("AutoDisableSimilarButtons")
        End Get
        Set(value As Boolean)
            ViewState("AutoDisableSimilarButtons") = value
        End Set
    End Property

#End Region


    Public Sub New()
        Me.OnClientLoad = "MilesButton_Init"
        Me.SingleClick = True


        'Me.EnableAutoSkins = True
        Me.AutoDisableSameButtonType = True
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        'output some javascript
        Dim milesButtonJs = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesButton.js")

        'link javascript
        ScriptManager.RegisterClientScriptInclude(Me.Page, Me.GetType(), "MilesButton", milesButtonJs)


    End Sub


    Protected Overrides Sub AddAttributesToRender(writer As System.Web.UI.HtmlTextWriter)
        MyBase.AddAttributesToRender(writer)

        'see if we have any buttons
        If RelatedButtons IsNot Nothing Then

            'create arrays
            Dim buttons = RelatedButtons.Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
            Dim clientIds As New List(Of String)

            'find the client IDs
            For Each btn In buttons
                Dim btnRelated As Control = Parent.FindControl(btn)
                If btnRelated IsNot Nothing Then
                    clientIds.Add(btnRelated.ClientID)
                End If
            Next

            'add the button ids as a string
            writer.AddAttribute("relatedbuttons", String.Join(",", clientIds))

        End If

        'add attributes
        writer.AddAttribute("actiontype", ActionType.ToString().ToLower())
        writer.AddAttribute("autodisablesamebuttontypes", AutoDisableSameButtonType.ToString.ToLower())
        writer.AddAttribute("causesvalidation", CausesValidation.ToString.ToLower())
    End Sub

End Class
