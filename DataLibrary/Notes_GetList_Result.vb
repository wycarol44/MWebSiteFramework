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
Partial Public Class Notes_GetList_Result
    Public Property NoteID As Integer
    Public Property ObjectID As Integer
    Public Property KeyID As Integer
    Public Property NoteTypeID As Integer
    Public Property Title As String
    Public Property Notes As String
    Public Property NotesText As String
    Public Property LinkURL As String
    Public Property Pinned As Boolean
    Public Property Archived As Boolean
    Public Property DateCreated As Nullable(Of Date)
    Public Property CreatedBy As Nullable(Of Integer)
    Public Property DateModified As Nullable(Of Date)
    Public Property ModifiedBy As Nullable(Of Integer)
    Public Property CreatedByName As String
    Public Property ModifiedByName As String
    Public Property TypeName As String

End Class
