Imports CommonLibrary
Public Class MetaTypeManager
#Region "Methods"


    ''' <summary>
    '''Gets a Record By Id
    '''Author: Sanjog Sharma
    '''Create Date: 12/23/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetByID(ByVal TypeID As Integer) As DataLibrary.MetaType
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.MetaTypes Where
                                            t.TypeID = TypeID
                                            ).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.MetaType
            End If
            Return obj
        End Using
    End Function


    ''' <summary>
    '''Gets a list and returns list of objects
    '''Author: Sanjog Sharma
    '''Create Date: 12/23/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetList(Optional ByVal TypeName As String = Nothing) As DataLibrary.MetaType()
        Using ctx As New DataLibrary.ModelEntities
            Return (From t In ctx.MetaTypes Where
                                            TypeName Is Nothing Or t.TypeName.Contains(TypeName)).ToArray()
        End Using
    End Function


    ''' <summary>
    '''Insert/Save Record
    '''Author: Sanjog Sharma
    '''Create Date: 12/23/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.MetaType) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.TypeID = 0 Then
                'new record
                ctx.MetaTypes.Add(obj)
            Else
                'edit existing record
                ctx.MetaTypes.Attach(obj)
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
