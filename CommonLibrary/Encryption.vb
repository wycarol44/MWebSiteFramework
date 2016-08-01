Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class Encryption
    Public Shared Salt As Byte() = {&H3, &H9, &H7, &HB, &HC, &H2, &HE, &HF}

    ''' <summary>
    ''' Encrypts the data using AES with a password
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="password"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Encrypt(ByVal data As String, ByVal password As String) As String
        Try


            Using aes As New RijndaelManaged()

                'derive the key from our password
                Dim keyGen = New Rfc2898DeriveBytes(password, Salt, 4897)
                'get some bytes
                aes.Key = keyGen.GetBytes(32)
                aes.BlockSize = 256
                'aes.Padding = PaddingMode.PKCS7

                'generate an IV
                aes.GenerateIV()

                'get the bytes for our message
                Dim plainBytes = Encoding.UTF8.GetBytes(data)

                'start up the encryption
                Using ms As New MemoryStream(),
                    cs = New CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write)
                    'write the bytes to the cryptostream
                    cs.Write(plainBytes, 0, plainBytes.Length)

                    cs.FlushFinalBlock()

                    'get message bytes
                    Dim msgBytes = ms.ToArray()

                    'create a new array big enough for the both of 'em
                    Dim cypherBytes((aes.IV.Length + msgBytes.Length) - 1) As Byte

                    'return the string with the iv as the first 32 bytes. will need this when decrypting
                    Buffer.BlockCopy(aes.IV, 0, cypherBytes, 0, aes.IV.Length)
                    Buffer.BlockCopy(msgBytes, 0, cypherBytes, aes.IV.Length, msgBytes.Length)

                    'now convert it to base64 string
                    Dim cypherText = Convert.ToBase64String(cypherBytes)

                    'return cypher text
                    Return cypherText
                End Using
            End Using

        Catch ex As Exception

            Return Nothing
        End Try


    End Function

    ''' <summary>
    ''' Decrypts data
    ''' </summary>
    ''' <param name="cypherData"></param>
    ''' <param name="password"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Decrypt(ByVal cypherData As String, password As String) As String

        Try


            Using aes As New RijndaelManaged()

                'derive the key from our password
                Dim keyGen = New Rfc2898DeriveBytes(password, Salt, 4897)
                'get some bytes
                aes.Key = keyGen.GetBytes(32)
                aes.BlockSize = 256
                'aes.Padding = PaddingMode.PKCS7

                'get the bytes for our message
                Dim cypherBytes = Convert.FromBase64String(cypherData)
                Dim iv(aes.IV.Length - 1) As Byte
                Dim msgBytes((cypherBytes.Length - iv.Length) - 1) As Byte

                'we use the first 32 bytes of the cypherdata for the IV
                Buffer.BlockCopy(cypherBytes, 0, iv, 0, iv.Length)
                Buffer.BlockCopy(cypherBytes, iv.Length, msgBytes, 0, msgBytes.Length)

                'set the IV for the instance
                aes.IV = iv

                'start up the decryption
                Using ms As New MemoryStream(),
                    cs = New CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write)
                    'write the bytes to the cryptostream
                    cs.Write(msgBytes, 0, msgBytes.Length)

                    cs.FlushFinalBlock()

                    'the plain text has been decrypted.
                    Dim plainText = System.Text.Encoding.UTF8.GetString(ms.ToArray())

                    'return plain text
                    Return plainText
                End Using
            End Using

        Catch ex As Exception

            Return Nothing
        End Try


    End Function
End Class
