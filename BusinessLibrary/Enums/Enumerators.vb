Public Enum AccessType
    FullAccess = 1
    ReadOnlyAccess = 2
    NoAccess = 3
End Enum

Public Enum ActionType
    Primary = 1
    Secondary = 2
    Neutral = 3
    Negative = 4
End Enum

Public Enum AuditLogType
    Insert = 1
    Update = 2
End Enum

Public Enum ManagedTypes
    CustomerCategory = 1
    ContactTitle = 2
    PaymentTypes = 3
End Enum



Public Enum MilesMetaFunctions
    CanExportCustomers = 15
End Enum

''' <summary>
''' Represents items in MetaAuditLogAttributes Table
''' </summary>
''' <remarks></remarks>
Public Enum MilesMetaAuditLogAttributes
    Status = 1
    JobTitle = 2
End Enum

''' <summary>
''' Represents items in the MetaObject table
''' </summary>
''' <remarks></remarks>
Public Enum MilesMetaObjects
    Users = 1
    Customers = 2
    Contacts = 3
End Enum

''' <summary>
''' Represents items in the MetaType table
''' </summary>
''' <remarks></remarks>
Public Enum MilesMetaType
    UserStatus = 1
    CustomerStatus = 2
    NoteType = 3
    CMSEmailFrom = 4
    CMSContentType = 5
    Attachment = 6
End Enum

''' <summary>
''' Represents items in the MetaTypeItems table
''' </summary>
''' <remarks></remarks>
Public Enum MilesMetaTypeItem
    UserStatusActive = 1
    UserStatusInactive = 2
    CustomerStatusPending = 3
    CustomerStatusActive = 4
    CustomerStatusLost = 5
    CustomerStatusInactive = 6
    NotesTypeNote = 9
    NotesTypeLink = 10
    CMSEmailFromLoggedInUser = 11
    CMSEmailFromSelectUser = 12
    CMSEmailFromManual = 13
    CMSEmailFromSetting = 14
    CMSContentTypeEmail = 15
    CMSContentTypeHyperlink = 16
    CMSContentTypeWebsite = 17
    RegularAttachment = 18
End Enum

''' <summary>
''' Represents items in the MetaCountries table
''' </summary>
''' <remarks></remarks>
Public Enum MilesMetaCountry As Integer
    UnitedStates = 230
    Canada = 39
End Enum

Public Enum CMSCategories As Integer
    ResetPassword = 1
    ResetPasswordLink = 2
End Enum

''' <summary>
''' Represents items in the MetaToolTips table
''' </summary>
''' <remarks></remarks>
Public Enum MilesMetaToolTips
    UserRoleDescription = 1
End Enum

Public Enum OrderStatus
    Pending = 25
    Accepted = 26
    Shipped = 27
    Cancelled = 28
End Enum

