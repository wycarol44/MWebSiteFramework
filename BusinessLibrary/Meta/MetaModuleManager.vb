Imports CommonLibrary
Public Class MetaModuleManager
#Region "Methods"

    ''' <summary>
    '''Gets a Record By Id
    '''Author: Sanjog Sharma
    '''Create Date: 11/19/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetByID(ByVal ModuleID As Integer) As DataLibrary.MetaModule
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.MetaModules Where
                        t.ModuleID = ModuleID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.MetaModule
            End If
            Return obj
        End Using
    End Function


    ''' <summary>
    '''Gets a list and returns list of objects
    '''Author: Sanjog Sharma
    '''Create Date: 11/19/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetList(Optional ByVal ModuleName As String = Nothing, Optional ByVal ParentID As Integer = Nothing) As DataLibrary.MetaModule()
        Using ctx As New DataLibrary.ModelEntities
            Return (From t In ctx.MetaModules
                    Where (ModuleName Is Nothing Or t.ModuleName.Contains(ModuleName)) _
                    And (ParentID = Nothing Or t.ParentID = ParentID)).ToArray()

        End Using
    End Function


    ''' <summary>
    '''Insert/Save Record
    '''Author: Sanjog Sharma
    '''Create Date: 11/19/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.MetaModule) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.ModuleID = 0 Then
                'new record
                ctx.MetaModules.Add(obj)
            Else
                'edit existing record
                ctx.MetaModules.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If
            'save the record
            ctx.SaveChanges()
            Return obj.ModuleID
        End Using
    End Function
#End Region
End Class
