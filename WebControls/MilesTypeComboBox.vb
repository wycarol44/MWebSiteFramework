Imports Telerik.Web.UI
Imports BusinessLibrary
Imports CommonLibrary
Imports MilesControls

Public Class MilesTypeComboBox
    Inherits RadComboBox

#Region "Properties"


    ''' <summary>
    ''' Gets or sets the selected value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StatusID As MilesMetaTypeItem?
        Get
            Return ToNullableInteger(Me.SelectedValue)
        End Get
        Set(value As MilesMetaTypeItem?)
            If value.HasValue Then
                Me.SelectedValue = CInt(value).ToString()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the type id for the filter
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StatusType As MilesMetaType
        Get
            Return ViewState("StatusType")
        End Get
        Set(value As MilesMetaType)
            ViewState("StatusType") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether the default item is shown
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IncludeDefaultItem As Boolean
        Get
            Return ViewState("IncludeDefaultItem")
        End Get
        Set(value As Boolean)
            ViewState("IncludeDefaultItem") = value
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New()
        Me.MaxHeight = 300
        Me.IncludeDefaultItem = True
    End Sub
#End Region

#Region "Overrides"
    Protected Overrides Sub OnInit(e As EventArgs)

        If Not Page.IsPostBack Then
            Populate()
        End If

        MyBase.OnInit(e)
    End Sub

#End Region

#Region "Methods"
    Public Sub Populate()
        Dim typeList = MetaTypeItemManager.GetList(StatusType)

        ControlBinding.BindListControl(
            Me,
            typeList,
            "ItemName",
            "ItemID",
            IncludeDefaultItem)
    End Sub
#End Region

End Class
