'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

<Serializable()>
Partial Public Class CMSCategory
    Public Property CategoryID As Integer
    Public Property CategoryName As String
    Public Property ContentTypeID As Nullable(Of Integer)
    Public Property EmailFromTypeID As Nullable(Of Integer)
    Public Property EmailFromID As Nullable(Of Integer)
    Public Property EmailFromName As String
    Public Property EmailFromEmail As String
    Public Property ContentSubject As String
    Public Property ContentBody As String
    Public Property Archived As Boolean
    Public Property DateCreated As Nullable(Of Date)

    Public Overridable Property MetaTypeItem As MetaTypeItem
    Public Overridable Property MetaTypeItem1 As MetaTypeItem
    Public Overridable Property CMSMergeFields As ICollection(Of CMSMergeField) = New HashSet(Of CMSMergeField)

End Class