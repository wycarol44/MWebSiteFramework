
Partial Class HR_EditMyInfo
    Inherits BasePage



    Protected Sub HR_EditMyInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Populate()
        End If
    End Sub

#Region "Methods"
    ''' <summary>
    ''' Populates fields from the record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Populate()

        Dim obj = UserManager.GetById(UserAuthentication.User.UserID)

        'populate fields
        txtEmail.Text = obj.Email
        txtHomePhone.Text = obj.HomePhone
        txtMobilePhone.Text = obj.MobilePhone
        txtWorkPhone.Text = obj.WorkPhone
        txtWorkPhone.Extension = obj.WorkPhoneExt

        milesAddr.AddressID = obj.AddressID.GetValueOrDefault()

        'set picture
        ' milesPicUpload.PictureID = obj.PictureID
        milesPicUpload.ObjectID = MilesMetaObjects.Users
        milesPicUpload.KeyID = obj.UserID

        'set entity footer
        SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)
    End Sub

    ''' <summary>
    ''' Saves or updates the record
    ''' </summary>
    ''' <param name="close"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Save(Optional close As Boolean = False) As Boolean
        Try
            Dim obj = UserManager.GetById(UserAuthentication.User.UserID)

            obj.Email = txtEmail.Text
            obj.HomePhone = txtHomePhone.Text
            obj.MobilePhone = txtMobilePhone.Text
            obj.WorkPhone = txtWorkPhone.Text
            obj.WorkPhoneExt = txtWorkPhone.Extension

            'do some validation
            If UserManager.IsEmailDuplicate(obj) Then
                Throw New WarningException("Please enter a unique email address")
            End If
            'save address
            obj.AddressID = milesAddr.Save()

            'save user
            UserManager.Save(obj)

            'set entity footer
            SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)

            'success
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="User", isDelayed:=close)

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

    Public Sub CloseForm()
        If ReferrerPageURL IsNot Nothing Then
            Navigate(ReferrerPageURL.ToString())
        Else
            Navigate("~/HR/ManageUsers.aspx?RestoreSearch=1")
        End If
    End Sub
#End Region

#Region "Events"
    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then
            CloseForm()
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Save()
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseForm()
    End Sub
#End Region

End Class
