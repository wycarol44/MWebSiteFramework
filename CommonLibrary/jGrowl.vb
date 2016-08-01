Imports System.Web
Imports System.Web.UI


<Serializable()>
Public Class JGrowlMessage
    Public Property Type As JGrowlMessageType
    Public Property Message As String
    Public Property Sticky As Boolean
    Public Property Header As String
    Public Property Theme As String
    Public Property Position As String
    Public Property ObjectName As String
    Public Property Life As Integer


    Public Sub New(type As JGrowlMessageType, Optional message As String = Nothing, Optional sticky As Boolean = False, Optional header As String = Nothing, Optional theme As String = Nothing, Optional objectName As String = Nothing, Optional life As Integer = 10000)
        Me.Type = type
        Me.Message = message
        Me.Sticky = sticky
        Me.Header = header
        Me.Theme = theme
        Me.Position = Position
        Me.ObjectName = objectName
        Me.Life = life
    End Sub
End Class

Public Enum JGrowlMessageType
    [Success]
    [Error]
    [Notification]
    [Alert]
End Enum

Public Class JGrowl

#Region "Private"
    ''' <summary>
    ''' Gets a list of delayed messages to show on the next postback
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared ReadOnly Property DelayedMessages As List(Of JGrowlMessage)
        Get
            Try
                Dim list = HttpContext.Current.Session("DelayedMessages")
                If list Is Nothing Then
                    list = New List(Of JGrowlMessage)
                    HttpContext.Current.Session("DelayedMessages") = list
                End If

                Return list
            Catch ex As Exception
                Return New List(Of JGrowlMessage)
            End Try

        End Get

    End Property

    ''' <summary>
    ''' Shows a notification to the user
    ''' </summary>
    ''' <param name="msg"></param>
    ''' <remarks></remarks>
    Private Shared Sub ShowMessage(msg As JGrowlMessage)
        ShowMessage(msg.Type, msg.Message, msg.Sticky, msg.Header, msg.Theme, msg.ObjectName, msg.Life)
    End Sub
#End Region

#Region "Public"
    ''' <summary>
    ''' Dispatches delayed messages
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub DispatchMessages()
        While DelayedMessages.Count > 0
            Dim m = DelayedMessages(0)
            'show the messages when there is a postback
            ShowMessage(m)
            'remove it
            DelayedMessages.Remove(m)
        End While
    End Sub




    ''' <summary>
    ''' Immediately shows a notification to the user
    ''' </summary>
    ''' <param name="type"></param>
    ''' <param name="message"></param>
    ''' <param name="header"></param>
    ''' <param name="theme"></param>
    ''' <remarks></remarks>
    Public Shared Sub ShowMessage(type As JGrowlMessageType, Optional message As String = Nothing, Optional sticky As Boolean = False, Optional header As String = Nothing, Optional theme As String = Nothing, Optional objectName As String = Nothing, Optional life As Integer = 5000, Optional isDelayed As Boolean = False, Optional useParent As Boolean = False)

        Static msgId As Integer = 0

        If isDelayed Then

            DelayedMessages.Add(New JGrowlMessage(type, message, sticky, header, theme, objectName, life))
            Return
        End If

        'set header type based on message type
        If header Is Nothing Then
            Select Case type
                Case JGrowlMessageType.Success
                    header = "Success"
                Case JGrowlMessageType.Error
                    header = "Error"
                Case JGrowlMessageType.Notification
                    header = "Notice"
                Case JGrowlMessageType.Alert
                    header = "Alert"
            End Select
        End If

        'set theme based on message type
        If theme Is Nothing Then
            Select Case type
                Case JGrowlMessageType.Success
                    theme = "successMsg"
                Case JGrowlMessageType.Error
                    theme = "errorMsg"
                Case JGrowlMessageType.Notification
                    theme = "notifyMsg"
                Case JGrowlMessageType.Alert
                    theme = "alertMsg"
            End Select
        End If

        'Usese parent object to display message (Useful in closing dialog windows)
        Dim functionName = "showMsg"
        If useParent Then
            functionName = "parent.showMsg"
        End If
        'if we specify an object type, and our message type is success, auto set message
        If type = JGrowlMessageType.Success And objectName IsNot Nothing And message Is Nothing Then
            message = String.Format("{0} saved successfully", objectName)
        ElseIf type = JGrowlMessageType.Success And message Is Nothing And objectName Is Nothing Then
            message = "Record saved successfully"
        ElseIf message Is Nothing Then
            message = String.Empty
        End If

        'increase our msg id so we can call this multiple times in one postback
        msgId += 1

        Dim page As Page = HttpContext.Current.CurrentHandler
        'Dim sm = ScriptManager.GetCurrent(page)

        ScriptManager.RegisterStartupScript(page, GetType(Page), "jGrowlMsg" & msgId,
                                            String.Format("{6}('{0}', {1}, '{2}', '{3}', '{4}', {5});",
                                                          message.Replace("'", "\'").Replace(vbCrLf, ""),
                                                          sticky.ToString().ToLower(),
                                                          header,
                                                          theme,
                                                          "center",
                                                          life,
                                                          functionName),
                                                      True)
    End Sub
#End Region

End Class
