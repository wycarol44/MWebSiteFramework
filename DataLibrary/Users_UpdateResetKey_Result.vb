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
Partial Public Class Users_UpdateResetKey_Result
    Public Property UserID As Integer
    Public Property Firstname As String
    Public Property Lastname As String
    Public Property Fullname As String
    Public Property Username As String
    Public Property UserKey As Byte()
    Public Property Password As Byte()
    Public Property JobTitleID As Nullable(Of Integer)
    Public Property AddressID As Nullable(Of Integer)
    Public Property Email As String
    Public Property WorkPhone As String
    Public Property WorkPhoneExt As String
    Public Property MobilePhone As String
    Public Property HomePhone As String
    Public Property Notes As String
    Public Property NotesText As String
    Public Property StatusID As Integer
    Public Property ResetKey As Byte()
    Public Property DateResetKeyIssued As Nullable(Of Date)
    Public Property Archived As Boolean
    Public Property DateCreated As Date
    Public Property CreatedBy As Integer
    Public Property DateModified As Nullable(Of Date)
    Public Property ModifiedBy As Nullable(Of Integer)

End Class
