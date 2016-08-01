Imports CommonLibrary

Public Class UserJobTitleManager
#Region "Standard"

    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="jobTitleId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(jobTitleId As Integer) As DataLibrary.UserJobTitle
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.UserJobTitles
                       Where t.JobTitleID = jobTitleId).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.UserJobTitle
            End If

            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Gets a simple list of records
    ''' </summary>
    ''' <param name="jobTitle"></param>
    ''' <param name="archived"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetList(Optional jobTitle As String = Nothing, Optional archived As Boolean? = False) As DataLibrary.UserJobTitle()
        Using ctx As New DataLibrary.ModelEntities

            'get list of objects
            Dim objs = (From t In ctx.UserJobTitles
                        Order By t.JobTitle
                        Where
                            (jobTitle Is Nothing OrElse t.JobTitle.Contains(jobTitle)) _
                        And (archived Is Nothing OrElse t.Archived = archived.Value)
                        Select t).ToArray()

            Return objs
        End Using
    End Function

    ''' <summary>
    ''' Inserts or updates the record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.UserJobTitle) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.JobTitleID = 0 Then
                ctx.UserJobTitles.Add(obj)

                'set default values
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID
            Else
                'set default values
                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID

                'its an update. attach the object to the context
                ctx.UserJobTitles.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If

            'save the record
            ctx.SaveChanges()

            Return obj.JobTitleID
        End Using
    End Function

    ''' <summary>
    ''' Archives or unarchives the specified record
    ''' </summary>
    ''' <param name="jobTitleId"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToggleArchived(jobTitleId As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.UserJobTitles
                       Where t.JobTitleID = jobTitleId).SingleOrDefault()

            If obj IsNot Nothing Then
                obj.Archived = Not obj.Archived

                ctx.SaveChanges()
            End If
        End Using
    End Sub

#End Region

#Region "Other"

    ''' <summary>
    ''' Checks to see if the job title is duplicate based on name
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsDuplicate(obj As DataLibrary.UserJobTitle) As Boolean
        Using ctx As New DataLibrary.ModelEntities
            Dim dupe = (From t In ctx.UserJobTitles
                        Where t.JobTitleID <> obj.JobTitleID _
                        And t.JobTitle = obj.JobTitle).SingleOrDefault()

            Return dupe IsNot Nothing


        End Using


    End Function

    ''' <summary>
    ''' Gets a list of records filtered by an id (including all unarchived records)
    ''' </summary>
    ''' <param name="jobTitleId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetIdFilteredList(Optional jobTitleId As Integer? = Nothing) As DataLibrary.UserJobTitle()

        Using ctx As New DataLibrary.ModelEntities
            'get list of objects
            Dim list = (From t In ctx.UserJobTitles
                        Order By t.JobTitle
                        Where (jobTitleId Is Nothing OrElse t.JobTitleID = jobTitleId) Or t.Archived = False
                        Select t).ToArray()

            Return list
        End Using
    End Function

#End Region

End Class
