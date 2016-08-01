Public Class MetaAuditLogAttributeManager

    ''' <summary>
    '''Gets a Record By Id
    '''Author: Pash Shrestha
    '''Create Date: 11/19/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetByID(ByVal AttributeID As Integer) As DataLibrary.MetaAuditLogAttribute
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.MetaAuditLogAttributes Where t.AttributeID = AttributeID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.MetaAuditLogAttribute
            End If
            Return obj
        End Using
    End Function

End Class
