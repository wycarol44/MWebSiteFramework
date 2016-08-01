Imports CommonLibrary
Public Class EmailLogManager
#Region "Methods"


    ''' <summary>
    '''Gets a Record By Id
    '''Author: Eric Butler
    '''Create Date: 12/3/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetByID(ByVal EmailID As Integer) As DataLibrary.EmailLog
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.EmailLogs Where
            t.EmailID = EmailID
            ).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.EmailLog
            End If
            Return obj
        End Using
    End Function


    ''' <summary>
    '''Insert/Save Record
    '''Author: Eric Butler
    '''Create Date: 12/3/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.EmailLog) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.EmailID = 0 Then
                'new record
                ctx.EmailLogs.Add(obj)
                obj.DateCreated = Now

            Else
                'edit existing record
                ctx.EmailLogs.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If
            'save the record
            ctx.SaveChanges()
            Return obj.EmailID
        End Using
    End Function

#End Region

    ''' <summary>
    ''' Sends an email and logs it in the EmailLog
    ''' </summary>
    ''' <param name="emailFrom"></param>
    ''' <param name="emailTo"></param>
    ''' <param name="emailSubject"></param>
    ''' <param name="emailBody"></param>
    ''' <param name="emailCC"></param>
    ''' <param name="emailBCC"></param>
    ''' <param name="emailFromName"></param>
    ''' <param name="isHtml"></param>
    ''' <param name="attachments"></param>
    ''' <remarks></remarks>
    Public Shared Sub SendEmail(ByVal emailFrom As String, ByVal emailTo As String, ByVal emailSubject As String, ByVal emailBody As String, Optional emailCC As String = Nothing, Optional emailBCC As String = Nothing, Optional emailFromName As String = Nothing, Optional ByVal isHtml As Boolean = True, Optional ByVal attachments As ArrayList = Nothing)

        'log email
        Dim email As New DataLibrary.EmailLog
        email.FromAddress = emailFrom
        email.ToAddress = emailTo
        email.Subject = emailSubject
        email.Body = emailBody
        email.Sent = False
        email.CreatedBy = If(UserAuthentication.User IsNot Nothing, UserAuthentication.User.UserID, PositiveOrNull(0))

        Save(email)

        'send email
        Functions.SendEmail(emailFrom, emailTo, emailSubject, emailBody, emailCC, emailBCC, emailFromName, isHtml, attachments)

        'if we dont error out, assume sent
        email.Sent = True

        Save(email)

    End Sub


End Class
