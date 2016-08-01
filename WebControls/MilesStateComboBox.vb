Imports Telerik.Web.UI
Imports BusinessLibrary
Imports CommonLibrary
Imports MilesControls

Public Class MilesStateComboBox
    Inherits RadComboBox

#Region "Enums"
    Public Enum DisplayMode
        Name
        Code
    End Enum
#End Region

#Region "Properties"

    ''' <summary>
    ''' Gets or sets the display mode of the state
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StateDisplayMode As DisplayMode
        Get
            Return ViewState("StateDisplayMode")
        End Get
        Set(value As DisplayMode)
            ViewState("StateDisplayMode") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the country to use when filtering for states
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Country As MilesMetaCountry
        Get
            Return ViewState("Country")
        End Get
        Set(value As MilesMetaCountry)
            ViewState("Country") = value

            'rebind the list
            PopulateStates()
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the selected state
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StateID As Integer?
        Get
            Return ToNullableInteger(Me.SelectedValue)
        End Get
        Set(value As Integer?)
            If value.HasValue Then
                Me.SelectedValue = value.ToString()
            End If
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New()
        StateDisplayMode = DisplayMode.Name

        Me.MaxHeight = 300
    End Sub
#End Region

#Region "Overrides"
    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)
    End Sub

#End Region

#Region "Methods"
    Public Sub PopulateStates()

        Dim countryList = MetaStateManager.GetList(Country)

        ControlBinding.BindListControl(
            Me,
            countryList,
            If(StateDisplayMode = DisplayMode.Name, "StateName", "StateCode"),
            "StateID",
            True)
    End Sub

#End Region


End Class
