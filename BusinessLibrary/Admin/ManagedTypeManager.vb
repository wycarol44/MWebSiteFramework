Imports CommonLibrary
Public Class ManagedTypeManager
#Region "Methods"


    ''' <summary>
    '''Gets a Record By Id
    '''Author: Sanjog Sharma
    '''Create Date: 11/29/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetByID(ByVal TypeID As Integer) As DataLibrary.ManagedType
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.ManagedTypes Where
            t.TypeID = TypeID
            ).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.ManagedType
            End If
            Return obj
        End Using
    End Function


    ''' <summary>
    '''Gets a list and returns list of objects
    '''Author: Sanjog Sharma
    '''Create Date: 11/29/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetList(Optional TypeName As String = Nothing) As DataLibrary.ManagedType()
            Using ctx As New DataLibrary.ModelEntities
            Return (From t In ctx.ManagedTypes Where
                                                TypeName Is Nothing _
                                                Or t.TypeName.Contains(TypeName) _
                                                Or t.FriendlyName.Contains(TypeName)).ToArray()
            End Using
    End Function


    ''' <summary>
    '''Insert/Save Record
    '''Author: Sanjog Sharma
    '''Create Date: 11/29/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.ManagedType) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.TypeID = 0 Then
                'new record
                ctx.ManagedTypes.Add(obj)
            Else
                'edit existing record
                ctx.ManagedTypes.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If
            'save the record
            ctx.SaveChanges()
            Return obj.TypeID
        End Using
    End Function
#End Region
End Class
