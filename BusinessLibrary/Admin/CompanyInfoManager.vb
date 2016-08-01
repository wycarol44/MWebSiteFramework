Imports System.Web

Public Class CompanyInfoManager

    ''' <summary>
    ''' Gets or sets the application name in session.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property ApplicationName As String
        Get

            Dim appName As String = HttpContext.Current.Session("ApplicationName")
            If String.IsNullOrWhiteSpace(appName) Then
                Dim obj = GetInfo()
                appName = obj.ApplicationName

                'save Application Name to the session
                HttpContext.Current.Session("ApplicationName") = appName
            End If
            'return app name
            Return appName
        End Get
        Set(value As String)
            'save Application Name to the session
            HttpContext.Current.Session("ApplicationName") = value
        End Set
    End Property

    ''' <summary>
    ''' Get a record by its id
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetInfo() As DataLibrary.AppSetting
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.AppSettings).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.AppSetting

            End If

            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Saves a record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.AppSetting) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.AppSettingID = 0 Then
                ctx.AppSettings.Add(obj)

                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID

            Else

                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID

                'its an update. attach the object to the context
                ctx.AppSettings.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified

            End If

            'save the record
            ctx.SaveChanges()

            'save the application name in the session
            CompanyInfoManager.ApplicationName = obj.ApplicationName

            Return obj.AppSettingID
        End Using
    End Function

End Class
