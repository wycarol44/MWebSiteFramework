Imports CommonLibrary
Public Class MetaToolTipManager
#Region "Methods"


    ''' <summary>
    '''Gets a Record By Id
    '''Author: Sanjog Sharma
    '''Create Date: 12/19/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetByID(ByVal ToolTipID As Integer) As DataLibrary.MetaToolTip
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.MetaToolTips Where
                            t.ToolTipID = ToolTipID
                            ).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.MetaToolTip
            End If
            Return obj
        End Using
    End Function


    ''' <summary>
    '''Gets a list and returns list of objects
    '''Author: Sanjog Sharma
    '''Create Date: 12/19/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetList(Optional ByVal ToolTip As String = Nothing) As DataLibrary.MetaToolTip()
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.MetaToolTips
                       Where (ToolTip Is Nothing Or t.ToolTipName.Contains(ToolTip) Or t.ToolTipDesc.Contains(ToolTip))
                     Select t Order By t.ToolTipName).ToArray()

            Return obj
        End Using
    End Function


    ''' <summary>
    '''Insert/Save Record
    '''Author: Sanjog Sharma
    '''Create Date: 12/19/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.MetaToolTip) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.ToolTipID = 0 Then
                'new record
                ctx.MetaToolTips.Add(obj)
                obj.DateCreated = Now
            Else
                'edit existing record
                ctx.MetaToolTips.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If
            'save the record
            ctx.SaveChanges()
            Return obj.ToolTipID
        End Using
    End Function
#End Region
End Class
