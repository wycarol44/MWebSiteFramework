Public Class CMSMergeFieldManager

    ''' <summary>
    ''' Get a record by its id
    ''' </summary>
    ''' <param name="mergeFieldId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetByID(ByVal mergeFieldId As Integer) As DataLibrary.CMSMergeField
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.CMSMergeFields Where
            t.MergeFieldID = mergeFieldId
            ).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.CMSMergeField
            End If
            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Get the list of records
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetList() As DataLibrary.CMSMergeField()
        Using ctx As New DataLibrary.ModelEntities
            
            'get list of objects
            Dim objs = (From t In ctx.CMSMergeFields
                        Order By t.MergeField
                        Select t).ToArray()

            Return objs

            Return Nothing
        End Using
    End Function

    ''' <summary>
    '''Insert/Save Record
    ''' </summary>
    ''' <returns>MergeFieldID</returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.CMSMergeField) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.MergeFieldID = 0 Then
                'new record
                ctx.CMSMergeFields.Add(obj)
            Else
                'edit existing record
                ctx.CMSMergeFields.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If
            'save the record
            ctx.SaveChanges()
            Return obj.MergeFieldID
        End Using
    End Function

#Region "Merge Functions"



    Public Shared Function MergeCompanyFields(ByVal str As String, c As DataLibrary.AppSetting) As String

        str = str.Replace("[[CompanyName]]", c.CompanyName)
        Return str


    End Function

    Public Shared Function MergeUserFields(ByVal str As String, u As DataLibrary.User) As String

        str = str.Replace("[[UserName]]", u.Username)
        str = str.Replace("[[User-Full-Name]]", u.Fullname)
        str = str.Replace("[[User-First-Name]]", u.Firstname)
        str = str.Replace("[[User-Last-Name]]", u.Lastname)

        Return str

    End Function

#End Region

End Class
