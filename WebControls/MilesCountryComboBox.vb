Imports Telerik.Web.UI
Imports BusinessLibrary
Imports CommonLibrary
Imports MilesControls

Public Class MilesCountryComboBox
    Inherits RadComboBox

#Region "Enums"
    Public Enum DisplayMode
        Name
        Code
    End Enum
#End Region

#Region "Properties"
    ''' <summary>
    ''' Gets or sets the country display mode. You must manually re-populate the countries
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CountryDisplayMode As DisplayMode
        Get
            Return ViewState("CountryDisplayMode")
        End Get
        Set(value As DisplayMode)
            ViewState("CountryDisplayMode") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the selected country id
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CountryID As Integer?
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
        CountryDisplayMode = DisplayMode.Name
        Me.MaxHeight = 300
    End Sub
#End Region

#Region "Overrides"
    Protected Overrides Sub OnInit(e As EventArgs)

        'populate the dropdown with countries
        If Not Page.IsPostBack Then
            PopulateCountries()
        End If

        MyBase.OnInit(e)
    End Sub

#End Region

#Region "Methods"
    Public Sub PopulateCountries()
        Dim countryList = MetaCountryManager.GetList()

        ControlBinding.BindListControl(
            Me,
            countryList,
            If(CountryDisplayMode = DisplayMode.Name, "CountryName", "CountryCode"),
            "CountryID",
            True)

    End Sub
#End Region

End Class
