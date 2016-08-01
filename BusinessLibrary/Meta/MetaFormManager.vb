Imports System.Web
Imports System.IO

Public Class MetaFormManager
#Region "Standard"
    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="formId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(formId As Integer) As DataLibrary.MetaForm
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.MetaForms
                       Where t.FormID = formId).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.MetaForm
            End If

            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Gets all the forms
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetList() As DataLibrary.MetaForm()
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.MetaForms Select t Order By t.FormName).ToArray()
            
            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Gets all the forms
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetGridList(Optional ByVal FormName As String = Nothing, Optional ByVal FormPath As String = Nothing, Optional ByVal ModuleName As String = Nothing, Optional ByVal CanBeFavorite As Boolean? = Nothing) As DataLibrary.MetaForm()
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.MetaForms.Include("MetaModule")
                       Order By t.FormName
                       Where (FormName Is Nothing Or t.FormName.Contains(FormName)) _
                       And (FormPath Is Nothing Or t.FormPath.Contains(FormPath)) _
                        And (ModuleName Is Nothing Or t.MetaModule.ModuleName.Contains(ModuleName)) _
                        And (CanBeFavorite Is Nothing Or t.CanBeFavorite = CanBeFavorite)).ToArray()
            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Inserts or updates the record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.MetaForm) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.FormID = 0 Then
                ctx.MetaForms.Add(obj)

                'set default values
                obj.DateCreated = Now

            Else
                'its an update. attach the object to the context
                ctx.MetaForms.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified

            End If

            'save the record
            ctx.SaveChanges()

            Return obj.FormID
        End Using
    End Function

    ''' <summary>
    ''' Removes form records where the physical file no longer exists
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub CleanDeletedForms()
        If HttpContext.Current Is Nothing Then Return

        Using ctx As New DataLibrary.ModelEntities

            'get a list of all the forms and delete the ones that have no physical file
            Dim list = (From t In ctx.MetaForms
                        Select t)

            For Each form In list
                'check to see if the file actually exists
                Dim physicalPath = HttpContext.Current.Server.MapPath(form.FormPath)

                If Not File.Exists(physicalPath) Then
                    'delete the form entry
                    ctx.Entry(form).State = EntityState.Deleted
                End If
            Next

            ctx.SaveChanges()

        End Using
    End Sub

#End Region

#Region "Custom"

    ''' <summary>
    ''' Get form by path
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetByPath(path As String) As DataLibrary.MetaForm
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.MetaForms
                       Where t.FormPath = path).SingleOrDefault()

            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Adds a form to the database IF it does not already exist (checks path)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AddForm(path As String, name As String) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            Dim mf = GetByPath(path)

            If mf Is Nothing Then
                'create the form and save it
                mf = New DataLibrary.MetaForm()
                mf.FormName = name
                mf.FormPath = path

                Return Save(mf)

            Else
                'update the name if its different?
                If Not mf.FormName = name Then

                    'set new name
                    mf.FormName = name

                    'update
                    Return Save(mf)
                Else
                    Return mf.FormID
                End If
            End If
        End Using


    End Function


    ''' <summary>
    ''' Toggles CanBeFavorite
    ''' </summary>
    ''' <param name="formid"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToggleCanBeFavorite(formid As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.MetaForms
                       Where t.FormID = formid).SingleOrDefault()

            If obj IsNot Nothing Then
                obj.CanBeFavorite = Not obj.CanBeFavorite

                ctx.SaveChanges()
            End If
        End Using
    End Sub
#End Region

End Class
