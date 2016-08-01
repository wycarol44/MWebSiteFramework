
Partial Class CRM_DialogManageCustomerAddress
    Inherits BasePage


#Region "Properties"
    Public Property CustomerAddressID As Integer
        Get
            If ViewState("CustomerAddressID") Is Nothing Then
                ViewState("CustomerAddressID") = ToInteger(Request("CustomerAddressID"))
            End If
            Return ViewState("CustomerAddressID")
        End Get
        Set(value As Integer)
            ViewState("CustomerAddressID") = value
        End Set
    End Property

    Public Property CustomerID As Integer
        Get
            Dim v As Object = ViewState("CustomerID")
            If v Is Nothing Then
                v = ToInteger(Request("CustomerID"))
                ViewState("CustomerID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("CustomerID") = value
        End Set
    End Property

    Public Property AddressTypeID As Integer
        Get
            Dim v As Object = ViewState("AddressTypeID")
            If v Is Nothing Then
                v = ToInteger(Request("AddressTypeID"))
                ViewState("AddressTypeID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("AddressTypeID") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If CustomerAddressID > 0 Then
                Populate()
            ElseIf CustomerID = 0 Or AddressTypeID = 0 Then
                JGrowl.ShowMessage(JGrowlMessageType.Alert, "There was no customer/type selected", isDelayed:=True)
                CloseDialogWindow()
            End If
        End If
    End Sub


    Private Sub Populate()
        Dim camobj = CustomerAddressManager.GetById(CustomerAddressID)
        Dim obj = AddressManager.GetById(camobj.AddressID)
        CustomerID = camobj.CustomerID
        AddressTypeID = camobj.AddressTypeID

        milesAdd.AddressID = camobj.AddressID

    End Sub


    Private Function Save() As Boolean


        If CustomerAddressID > 0 Then

            Dim camobj = CustomerAddressManager.GetById(CustomerAddressID)
            milesAdd.AddressID = camobj.AddressID
            milesAdd.Save()

            CustomerAddressID = CustomerAddressManager.Save(camobj)

        ElseIf CustomerAddressID = 0 Then
            'Add NEW Address
            Dim AddressID As Integer = milesAdd.Save()

            Dim camobj = New DataLibrary.CustomerAddress()
            camobj.CustomerID = CustomerID
            camobj.AddressTypeID = AddressTypeID
            camobj.AddressID = AddressID
            camobj.Archived = False
            CustomerAddressID = CustomerAddressManager.Save(camobj)

        End If
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Record")
        Return True
    End Function


    '#End Region

#Region "Events"
    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save() Then
            CloseDialogWindow()
        End If
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub
#End Region



End Class
