Imports CommonLibrary
Public Class UserLoginLogManager
#Region "Methods"


    ''' <summary>
    '''Gets a Record By Id
    '''Author: Sanjog Sharma
    '''Create Date: 12/16/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetByID(ByVal LogID As Integer) As DataLibrary.UserLoginLog
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.UserLoginLogs Where
                            t.LogID = LogID
                            ).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.UserLoginLog
            End If
            Return obj
        End Using
    End Function


    ''' <summary>
    '''Gets a list and returns list of objects
    '''Author: Sanjog Sharma
    '''Create Date: 12/16/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetList(Optional ByVal UserID As Integer? = Nothing, Optional ByVal DateCreatedFrom As DateTime? = Nothing, Optional ByVal DateCreatedTo As DateTime? = Nothing) As DataLibrary.UserLoginLog()
        Using ctx As New DataLibrary.ModelEntities

            'get list of objects
            Dim objs = (From t In ctx.UserLoginLogs
                        Where
                            (UserID Is Nothing OrElse t.UserID = UserID) _
                        And (DateCreatedFrom Is Nothing OrElse t.DateCreated >= DateCreatedFrom) _
                        And (DateCreatedTo Is Nothing OrElse t.DateCreated <= DateCreatedTo)
                        Select t).ToArray()
            Return objs
        End Using
    End Function


    ''' <summary>
    '''Insert/Save Record
    '''Author: Sanjog Sharma
    '''Create Date: 12/16/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.UserLoginLog) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.LogID = 0 Then
                'new record
                ctx.UserLoginLogs.Add(obj)
                obj.DateCreated = Now
            Else
                'edit existing record
                ctx.UserLoginLogs.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If
            'save the record
            ctx.SaveChanges()
            Return obj.LogID
        End Using
    End Function
#End Region
End Class
