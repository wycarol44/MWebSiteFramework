
Partial Class Admin_CompanyInfo
    Inherits BasePage

#Region "Page functions"

    Protected Sub Admin_CompanyInfo_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Populate()

            If appSettingId = 0 Then
                Page.SetFocus(txtCompanyName)
            End If
        End If

    End Sub

#End Region

#Region "Properties"

    Private Property appSettingId As Integer

        Get
            Return ToInteger(ViewState("appSettingId"))
        End Get
        Set(value As Integer)
            ViewState("appSettingId") = value
        End Set

    End Property

#End Region

#Region "Methods"

    ''' <summary>
    ''' Populates fields from the record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Populate()

        Dim obj = CompanyInfoManager.GetInfo()
        appSettingId = obj.AppSettingID

        If obj.AppSettingID > 0 Then

            'populate fields
            txtCompanyName.Text = obj.CompanyName
            txtApplicationName.Text = obj.ApplicationName
            txtEmail.Text = obj.Email
            txtPhone.Text = obj.Phone

            milesAddr.AddressID = obj.AddressID.GetValueOrDefault()

            'set entity footer
            SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)
        End If

    End Sub

    ''' <summary>
    ''' Saves or updates the record
    ''' </summary>
    ''' <param name="close"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Save(Optional close As Boolean = False) As Boolean

        Try

            Dim obj = CompanyInfoManager.GetInfo()

            obj.CompanyName = txtCompanyName.Text
            obj.ApplicationName = txtApplicationName.Text
            obj.Email = txtEmail.Text
            obj.Phone = txtPhone.Text

            'save address
            obj.AddressID = milesAddr.Save()

            'save company info
            appSettingId = CompanyInfoManager.Save(obj)

            Master.AppName = obj.ApplicationName

            'set entity footer
            SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)

            'success
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Company Info", isDelayed:=close)

            Return True

        Catch wex As WarningException

            'show a friendly message
            JGrowl.ShowMessage(JGrowlMessageType.Error, wex.Message)

            'return false
            Return False

        Catch ex As Exception
            'fail
            Throw

        End Try
    End Function

#End Region

#Region "Events"

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Save()
    End Sub

#End Region

End Class
