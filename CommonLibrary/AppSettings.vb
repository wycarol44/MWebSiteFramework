Imports Microsoft.VisualBasic
Imports System.Configuration

Public Class AppSettings
#Region "General Settings"
    Public Shared ReadOnly Property AdminSiteURL As String
        Get
            Return ConfigurationManager.AppSettings("Miles.AdminSiteURL")
        End Get
    End Property

    Public Shared ReadOnly Property ApplicationName As String
        Get
            Return ConfigurationManager.AppSettings("Miles.ApplicationName")
        End Get
    End Property

    ''' <summary>
    ''' Gets whether the remember me functionality is enabled
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property RememberMeEnabled As Boolean
        Get
            Return Convert.ToBoolean(ConfigurationManager.AppSettings("Miles.RememberMeEnabled"))
        End Get
    End Property

    Shared ReadOnly Property MilesActions As String
        Get
            Return ConfigurationManager.AppSettings("Miles.Actions")
        End Get
    End Property
#End Region

#Region "Validation Settings"
    ''' <summary>
    ''' Gets the validation expression for username fields
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property UsernameValidationExpression As String
        Get
            Return ConfigurationManager.AppSettings("Miles.UsernameValidationExpression")
        End Get
    End Property

    ''' <summary>
    ''' Gets the validation error message for username fields
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property UsernameValidationErrorMessage As String
        Get
            Return ConfigurationManager.AppSettings("Miles.UsernameValidationErrorMessage")
        End Get
    End Property

    ''' <summary>
    ''' Gets the validation expression for password fields
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property PasswordValidationExpression As String
        Get
            Return ConfigurationManager.AppSettings("Miles.PasswordValidationExpression")
        End Get
    End Property

    ''' <summary>
    ''' Gets the validation error message for password fields
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property PasswordValidationMessage As String
        Get
            Return ConfigurationManager.AppSettings("Miles.PasswordValidationMessage")
        End Get
    End Property

    ''' <summary>
    ''' Gets the validation expression for URL fields
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property URLValidationExpression As String
        Get
            Return ConfigurationManager.AppSettings("Miles.URLValidationExpression")
        End Get
    End Property

    ''' <summary>
    ''' Gets the validation error message for URL fields
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property URLValidationMessage As String
        Get
            Return ConfigurationManager.AppSettings("Miles.URLValidationMessage")
        End Get
    End Property
#End Region

#Region "Documents"

    Public Shared ReadOnly Property DocumentsMaxFileSize As String
        Get
            Return ConfigurationManager.AppSettings("Miles.DocumentsMaxFileSize")
        End Get
    End Property

    ''' <summary>
    ''' Gets the virtual path of the documents folder
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property DocumentsPath As String
        Get
            Return ConfigurationManager.AppSettings("Miles.DocumentsPath")
        End Get
    End Property

    ''' <summary>
    ''' Gets the physical path of the documents folder
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property DocumentsFolder As String
        Get
            Return ConfigurationManager.AppSettings("Miles.DocumentsFolder")
        End Get
    End Property


    Public Shared ReadOnly Property UploadedDocumentsPath As String
        Get
            Return ConfigurationManager.AppSettings("Miles.UploadedDocumentsPath")
        End Get
    End Property

    Public Shared ReadOnly Property UploadedDocumentsFolder As String
        Get
            Return ConfigurationManager.AppSettings("Miles.UploadedDocumentsFolder")
        End Get
    End Property

    ''' <summary>
    ''' Gets the virtual path of the temp documents folder. Only store files in here that are safe to delete
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property TempDocumentPath As String
        Get
            Return ConfigurationManager.AppSettings("Miles.TempDocumentPath")
        End Get
    End Property

    ''' <summary>
    ''' Gets the physical path of the temp documents folder. Only store files in here that are safe to delete
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property TempDocumentFolder As String
        Get
            Return ConfigurationManager.AppSettings("Miles.TempDocumentFolder")
        End Get
    End Property

    ''' <summary>
    ''' Gets the virtual path of the pictures folder. Only store files in here that are safe to delete
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property PicturesPath As String
        Get
            Return ConfigurationManager.AppSettings("Miles.PicturesPath")
        End Get
    End Property

    ''' <summary>
    ''' Gets the physical path of the pictures folder. Only store files in here that are safe to delete
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property PicturesFolder As String
        Get
            Return ConfigurationManager.AppSettings("Miles.PicturesFolder")
        End Get
    End Property


    'I add this

    Public Shared ReadOnly Property ProductImagePath As String
        Get
            Return ConfigurationManager.AppSettings("Miles.ProductImagePath")
        End Get
    End Property

    Public Shared ReadOnly Property ProductImageFolder As String
        Get
            Return ConfigurationManager.AppSettings("Miles.ProductImageFolder")
        End Get
    End Property




#End Region

#Region "Active Directory Settings"

    Public Shared ReadOnly Property ADEnabled As Boolean
        Get
            Return Convert.ToBoolean(ConfigurationManager.AppSettings("Miles.ADEnabled"))
        End Get
    End Property


    Public Shared ReadOnly Property ADPath As String
        Get
            Return ConfigurationManager.AppSettings("Miles.ADPath")
        End Get
    End Property

    Public Shared ReadOnly Property ADImpersonateUserID As Integer
        Get
            Return ToInteger(ConfigurationManager.AppSettings("Miles.ADImpersonateUserID"))
        End Get
    End Property
#End Region

#Region "caching"
    Shared ReadOnly Property BypassCaching As Boolean

        Get
            Return Convert.ToBoolean(ConfigurationManager.AppSettings("Miles.BypassCaching"))
        End Get

    End Property

    Shared ReadOnly Property FunctionAccessCacheKey As String
        Get
            Return ConfigurationManager.AppSettings("Miles.FunctionAccessCacheKey")
        End Get
    End Property

    Shared ReadOnly Property MetaFunctionsCacheKey As String
        Get
            Return ConfigurationManager.AppSettings("Miles.MetaFunctionsCacheKey")
        End Get
    End Property

    Shared ReadOnly Property MetaMenuCacheKey As String
        Get
            Return ConfigurationManager.AppSettings("Miles.MetaMenuCacheKey")
        End Get
    End Property

#End Region


#Region "Email Test Mode"
    Shared ReadOnly Property EmailTestMode As Boolean

        Get
            Return Convert.ToBoolean(ConfigurationManager.AppSettings("Miles.EmailTestMode"))
        End Get

    End Property

    Shared ReadOnly Property EmailTestModeSendTo As String

        Get
            Return ConfigurationManager.AppSettings("Miles.EmailTestModeSendTo")
        End Get

    End Property
#End Region
  

End Class
