Imports CommonLibrary
Imports System.Text

Public Class AuditLogManager
#Region "Methods"


    ''' <summary>
    '''Gets a Record By Id
    '''Author: Pash Shrestha
    '''Create Date: 11/19/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetByID(ByVal LogID As Integer) As DataLibrary.AuditLog
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.AuditLogs Where t.LogID = LogID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.AuditLog
            End If
            Return obj
        End Using
    End Function

    Public Shared Function GetList(Optional ByVal ObjectID As Integer? = Nothing, Optional ByVal KeyID As Integer? = Nothing, Optional ByVal CreatedBy As Integer? = Nothing, Optional ByVal DateCreatedFrom As DateTime? = Nothing, Optional ByVal DateCreatedTo As DateTime? = Nothing) As DataLibrary.AuditLog()
        Using ctx As New DataLibrary.ModelEntities

            Dim objs = (From a In ctx.AuditLogs.Include("User")
            Where
                (a.ObjectID = ObjectID And a.KeyID = KeyID And (CreatedBy Is Nothing OrElse a.CreatedBy = CreatedBy) _
                 And (DateCreatedFrom Is Nothing OrElse a.DateCreated >= DateCreatedFrom) _
                  And (DateCreatedTo Is Nothing OrElse a.DateCreated <= DateCreatedTo))
                 Select a).ToArray()

            Return objs
        End Using
    End Function

    Public Shared Function GetListForDashBoard(ObjectID As Integer, KeyID As Integer, NumberofItems As Integer) As DataLibrary.AuditLog_GetListForDashBoard_Result()
        Using ctx As New DataLibrary.ModelEntities()
            Return ctx.AuditLog_GetListForDashBoard(ObjectID, KeyID, NumberofItems).ToArray()
        End Using
    End Function

    Public Shared Function GetCount(ByVal ObjectID As Integer, ByVal KeyID As Integer) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            Return (From a In ctx.AuditLogs Where a.ObjectID = ObjectID And a.KeyID = KeyID).Count

        End Using
    End Function


    ''' <summary>
    '''Insert/Save Record
    '''Author: Pash Shrestha
    '''Create Date: 11/19/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.AuditLog) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.LogID = 0 Then
                'new record
                ctx.AuditLogs.Add(obj)
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID
            Else
                'edit existing record
                ctx.AuditLogs.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If
            'save the record
            ctx.SaveChanges()
            Return obj.LogID
        End Using
    End Function

#End Region

#Region "Audit Log Functions"

    ''' <summary>
    ''' Gets AuditLog Text to be saved in Audit Log Table
    ''' Author: Pashupati Shrestha
    ''' Date: 11/20/2013
    ''' </summary>
    ''' <param name="AuditLogType"></param>
    ''' <param name="AttributeID"></param>
    ''' <param name="PreviousValue"></param>
    ''' <param name="NewValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetAuditLogMergeText(ByVal AuditLogType As AuditLogType,
                                                 ByVal AttributeID As MilesMetaAuditLogAttributes,
                                                Optional ByVal NewValue As String = "",
                                                Optional ByVal PreviousValue As String = "") As String
        Dim Message As New StringBuilder

        Dim alText = MetaAuditLogAttributeManager.GetByID(AttributeID)
        If AuditLogType = AuditLogType.Insert Then
            Message.Append(alText.InsertLogText)
        ElseIf AuditLogType = AuditLogType.Update Then
            Message.Append(alText.UpdateLogText)
        End If

        Message = Message.Replace("[[NewValue]]", NewValue).Replace("[[PreviousValue]]", PreviousValue)

        Return Message.ToString

    End Function

    ''' <summary>
    ''' Author: Pashupati Shrestha
    ''' Saves Audit Logs to  log table
    ''' </summary>
    ''' <param name="AuditLogType"></param>
    ''' <param name="ObjectID"></param>
    ''' <param name="KeyID"></param>
    ''' <param name="AttributeKeyID"></param>
    ''' <param name="AttributeID"></param>
    ''' <param name="PreviousValue"></param>
    ''' <param name="NewValue"></param>
    ''' <param name="EffectiveDate"></param>
    ''' <remarks></remarks>
    Public Shared Sub SaveAuditLog(ByVal AuditLogType As AuditLogType, ByVal ObjectID As Integer,
                                    ByVal KeyID As Integer, ByVal AttributeKeyID As Integer,
                                    ByVal AttributeID As MilesMetaAuditLogAttributes,
                                    Optional ByVal NewValue As String = "", Optional ByVal PreviousValue As String = "", Optional ByVal EffectiveDate As DateTime? = Nothing, Optional CustomAuditLogText As String = "")

        Dim al As New DataLibrary.AuditLog
        al.ObjectID = ObjectID
        al.KeyID = KeyID
        al.AttributeKeyID = AttributeKeyID
        al.AttributeID = AttributeID
        al.EffectiveDate = EffectiveDate
        al.AuditLogText = If(CustomAuditLogText = String.Empty, GetAuditLogMergeText(AuditLogType, AttributeID, NewValue, PreviousValue), CustomAuditLogText)

        AuditLogManager.Save(al)

    End Sub

#End Region

End Class
