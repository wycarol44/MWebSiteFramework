Imports Telerik.Web.UI
Imports BusinessLibrary
Imports CommonLibrary
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports MilesControls

Public Class MilesUserComboBox
    Inherits RadComboBox

#Region "Properties"

    Public Property UserID As Integer?
        Get
            Return ToNullableInteger(Me.SelectedValue)
        End Get
        Set(value As Integer?)
            If value.HasValue Then
                Me.SelectedValue = value.ToString()
            End If
        End Set
    End Property

    Public Overrides Property SelectedValue As String
        Get
            Return MyBase.SelectedValue
        End Get
        Set(value As String)
            MyBase.SelectedValue = value

            If Not String.IsNullOrWhiteSpace(value) And EnableLoadOnDemand Then
                Dim args As New RadComboBoxItemsRequestedEventArgs()

                args.Value = value
                OnItemsRequested(args)
            End If
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New()
        'Me.MaxHeight = 300
        Me.ItemTemplate = New MilesUserComboBoxItemTemplate
        Me.EmptyMessage = "Select User"
    End Sub
#End Region

#Region "Overrides"
    Protected Overrides Sub OnInit(e As EventArgs)

        If Not Page.IsPostBack And Not EnableLoadOnDemand Then
            Populate()
        End If

        MyBase.OnInit(e)
    End Sub

    Protected Overrides Sub OnItemsRequested(args As RadComboBoxItemsRequestedEventArgs)

        Dim userId = ToNullableInteger(args.Value)

        Dim list = UserManager.GetComboBoxList(userId:=userId, fullname:=args.Text, skip:=args.NumberOfItems, take:=ItemsPerRequest)

        Me.DataSource = list
        Me.DataTextField = "Fullname"
        Me.DataValueField = "UserID"
        Me.DataBind()

        'set the selected value
        If userId IsNot Nothing Then
            'call mybase because we'll end up in an endless recursive loop otherwise
            MyBase.SelectedValue = args.Value
        End If

        Dim count = (From t In list Select t.TotalCount).FirstOrDefault()
        Dim endoffset = args.NumberOfItems + ItemsPerRequest
        If endoffset > count Then endoffset = count

        If list.Count > 0 Then
            args.Message = String.Format(
                "Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>",
                endoffset,
                count)

        Else
            args.Message = "No Matches"
        End If


        MyBase.OnItemsRequested(args)
    End Sub

    Protected Overrides Sub OnItemCreated(e As RadComboBoxItemEventArgs)

        If e.Item.DataItem Is Nothing Then
            Dim lblText As Label = e.Item.FindControl("lblText")

            lblText.Text = e.Item.Text
        End If

        MyBase.OnItemCreated(e)
    End Sub


#End Region

#Region "Methods"
    Public Sub Populate()
        Dim userList = UserManager.GetList()

        ControlBinding.BindListControl(
            Me,
            userList,
            "Fullname",
            "UserID",
            True)

    End Sub
#End Region

End Class


Class MilesUserComboBoxItemTemplate
    Implements ITemplate

    Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn
        'create a template for the user drop down

        Dim milesPic As New MilesPicture
        Dim lblText As New Label

        milesPic.ID = "milesPic"
        milesPic.Width = 32

        lblText.ID = "lblText"
        lblText.Style.Add("margin-left", "5px")

        'databinding
        AddHandler milesPic.DataBinding, AddressOf milesPic_DataBinding
        AddHandler lblText.DataBinding, AddressOf lblText_DataBinding

        container.Controls.Add(milesPic)
        container.Controls.Add(lblText)

    End Sub

    Protected Sub milesPic_DataBinding(sender As Object, e As EventArgs)
        Dim ctrl As MilesPicture = sender

        Dim container As RadComboBoxItem = ctrl.NamingContainer

        ctrl.ObjectID = MilesMetaObjects.Users
        ctrl.KeyID = DataBinder.Eval(container.DataItem, "UserID")
    End Sub

    Protected Sub lblText_DataBinding(sender As Object, e As EventArgs)
        Dim ctrl As Label = sender

        Dim container As RadComboBoxItem = ctrl.NamingContainer

        ctrl.Text = container.Text
    End Sub

End Class