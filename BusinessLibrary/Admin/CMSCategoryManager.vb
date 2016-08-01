Imports CommonLibrary
Public Class CMSCategoryManager
#Region "Methods"


    ''' <summary>
    '''Gets a Record By Id
    '''Author: Sanjog Sharma
    '''Create Date: 11/21/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetByID(ByVal CategoryID As Integer) As DataLibrary.CMSCategory
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.CMSCategories Where
            t.CategoryID = CategoryID
            ).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.CMSCategory
            End If
            Return obj
        End Using
    End Function


    ''' <summary>
    '''Gets a list and returns list of objects
    '''Author: Sanjog Sharma
    '''Create Date: 11/21/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetList(Optional ByVal CategoryName As String = Nothing, Optional ByVal ContentTypeID As List(Of Integer) = Nothing,
                Optional ByVal Archived As Boolean? = Nothing) As DataLibrary.CMSCategory()
        Using ctx As New DataLibrary.ModelEntities
            If ContentTypeID Is Nothing Then
                ContentTypeID = New List(Of Integer)
            End If
            'get list of objects
            Dim objs = (From t In ctx.CMSCategories
                        Order By t.CategoryName
                        Where
                                (CategoryName Is Nothing OrElse t.CategoryName.Contains(CategoryName)) _
                                    And (Not ContentTypeID.Any OrElse ContentTypeID.Contains(t.ContentTypeID)) _
                                    And (Archived Is Nothing OrElse t.Archived = Archived.Value)
                                Select t).ToArray()

            Return objs

            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' Gets a list of CMS Categories by Merge Fields
    ''' </summary>
    ''' <param name="categoryName"></param>
    ''' <param name="mergeFieldIds"></param>
    ''' <param name="archived"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetListByMergeFields(Optional categoryName As String = Nothing, Optional contenttypeids As XElement = Nothing, Optional mergeFieldIds As XElement = Nothing, Optional archived As Boolean? = False) As DataLibrary.CMSCategories_GetListByMergeFields_Result()
        Using ctx As New DataLibrary.ModelEntities

            Return ctx.CMSCategories_GetListByMergeFields(
                categoryName,
                ToNull(contenttypeids),
                ToNull(mergeFieldIds),
                 archived
            ).ToArray()

        End Using
    End Function


    ''' <summary>
    '''Insert/Save Record
    '''Author: Sanjog Sharma
    '''Create Date: 11/21/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.CMSCategory) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.CategoryID = 0 Then
                'new record
                ctx.CMSCategories.Add(obj)
                obj.DateCreated = Now
            Else
                'edit existing record
                ctx.CMSCategories.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If
            'save the record
            ctx.SaveChanges()
            Return obj.CategoryID
        End Using
    End Function


    ''' <summary>
    ''' save multiple merge fields for a given cms category at once
    ''' used to save merge field for new category
    ''' </summary>
    ''' <param name="categoryId"></param>
    ''' <param name="mergeFields"></param>
    ''' <remarks></remarks>
    Public Shared Sub SaveMergeFields(categoryId As Integer, mergeFields As List(Of DataLibrary.CMSMergeField))

        Using ctx As New DataLibrary.ModelEntities

            Dim c = (From t In ctx.CMSCategories
                       Where t.CategoryID = categoryId).SingleOrDefault()
            For Each mf In mergeFields
                c.CMSMergeFields.Add((From x In ctx.CMSMergeFields Where x.MergeFieldID = mf.MergeFieldID).Single())
            Next

            ctx.SaveChanges()

        End Using
    End Sub


    ''' <summary>
    '''Toggles the active status of a record
    '''Author: Sanjog Sharma
    '''Create Date: 11/21/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub ToggleArchived(ByVal CategoryID As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.CMSCategories Where
                            t.CategoryID = CategoryID
                            ).SingleOrDefault()
            If obj IsNot Nothing Then
                obj.Archived = Not obj.Archived
                ctx.SaveChanges()
            End If
        End Using
    End Sub


    ''' <summary>
    ''' Get a record by its id
    ''' </summary>
    ''' <param name="categoryid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMergeFields(categoryid As Integer) As DataLibrary.CMSMergeField()
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = ctx.CMSCategories.Where(Function(x) x.CategoryID = categoryid).SingleOrDefault()

            Dim objlist() As DataLibrary.CMSMergeField
            If obj IsNot Nothing Then
                objlist = obj.CMSMergeFields.ToArray()
            Else
                objlist = Nothing
            End If

            Return objlist
        End Using
    End Function


    ''' <summary>
    ''' Get a record by its id
    ''' </summary>
    ''' <param name="categoryId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMergeField(categoryId As Integer) As DataLibrary.CMSMergeField()
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = ctx.CMSCategories.Where(Function(x) x.CategoryID = categoryId).SingleOrDefault()

            Dim objlist() As DataLibrary.CMSMergeField
            If obj IsNot Nothing Then
                objlist = obj.CMSMergeFields.ToArray()
            Else
                objlist = Nothing
            End If

            Return objlist
        End Using
    End Function

    Public Shared Sub SaveMergeField(categoryId As Integer, mergeFieldID As Integer)

        Using ctx As New DataLibrary.ModelEntities

            Dim mf = (From t In ctx.CMSCategories
                       Where t.CategoryID = categoryId).SingleOrDefault()

            mf.CMSMergeFields.Add((From c In ctx.CMSMergeFields Where c.MergeFieldID = mergeFieldID).Single())


            ctx.SaveChanges()

        End Using
    End Sub


    Public Shared Sub RemoveMergeField(categoryId As Integer, mergeFieldID As Integer)

        Using ctx As New DataLibrary.ModelEntities

            Dim mf = (From t In ctx.CMSCategories
                       Where t.CategoryID = categoryId).SingleOrDefault()

            mf.CMSMergeFields.Remove((From c In ctx.CMSMergeFields Where c.MergeFieldID = mergeFieldID).Single())

            ctx.SaveChanges()

        End Using
    End Sub

#End Region
End Class
