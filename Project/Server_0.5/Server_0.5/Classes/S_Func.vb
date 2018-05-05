﻿Imports System.IO
Imports System.Security.Cryptography

Module S_Func
    Public pass As String = "|'x'|"

    Public Function AES_Encrypt(ByVal input As String)
        Dim AES As New RijndaelManaged
        Dim Hash_AES As New MD5CryptoServiceProvider
        Dim encrypted As String = ""
        Try
            Dim hash(31) As Byte
            Dim temp As Byte() = Hash_AES.ComputeHash(SB(pass))
            Array.Copy(temp, 0, hash, 0, 16)
            Array.Copy(temp, 0, hash, 15, 16)
            AES.Key = hash
            AES.Mode = CipherMode.ECB
            Dim DESEncrypter As ICryptoTransform = AES.CreateEncryptor
            Dim Buffer As Byte() = SB(input)
            encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            Return encrypted
        Catch ex As Exception
        End Try
    End Function
    Public Function AES_Decrypt(ByVal input As String)
        Dim AES As New RijndaelManaged
        Dim Hash_AES As New MD5CryptoServiceProvider
        Dim decrypted As String = ""
        Try
            Dim hash(31) As Byte
            Dim temp As Byte() = Hash_AES.ComputeHash(SB(pass))
            Array.Copy(temp, 0, hash, 0, 16)
            Array.Copy(temp, 0, hash, 15, 16)
            AES.Key = hash
            AES.Mode = CipherMode.ECB
            Dim DESDecrypter As ICryptoTransform = AES.CreateDecryptor
            Dim Buffer As Byte() = Convert.FromBase64String(input)
            decrypted = BS(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            Return decrypted
        Catch ex As Exception
        End Try
    End Function


    Public Function getMD5Hash(ByVal B As Byte()) As String
        B = New System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(B)
        Dim str2 As String = ""
        Dim num As Byte
        For Each num In B
            str2 = (str2 & num.ToString("x2"))
        Next
        Return str2
    End Function

    Public Function GetExternalAddress() As String
        Try
            If Form1.MYIP = String.Empty Then
                Dim response As Net.WebResponse = Net.WebRequest.Create("http://checkip.dyndns.org/").GetResponse()
                Dim reader As New StreamReader(response.GetResponseStream())
                Dim Str As String = reader.ReadToEnd()
                reader.Dispose()
                response.Close()
                Dim startIndex As Integer = Str.IndexOf("Address: ") + 9
                Dim num2 As Integer = Str.LastIndexOf("</body>")
                Form1.MYIP = Str.Substring(startIndex, num2 - startIndex)
                Return Form1.MYIP
            Else
                Return Form1.MYIP
            End If
        Catch ex As Exception
            Return "127.0.0.1"
        End Try
    End Function

    Public Function siz(ByVal Size As String) As String
        If (Size.ToString.Length < 4) Then
            Return (CInt(Size) & " Bytes")
        End If
        Dim str As String = String.Empty
        Dim num As Double = CDbl(Size) / 1024
        If (num < 1024) Then
            str = " KB"
        Else
            num = (num / 1024)
            If (num < 1024) Then
                str = " MB"
            Else
                num = (num / 1024)
                str = " GB"
            End If
        End If
        Return (num.ToString(".0") & str)
    End Function

    Function SB(ByVal s As String) As Byte() ' string to byte()
        Return System.Text.Encoding.Default.GetBytes(s)
    End Function

    Function BS(ByVal b As Byte()) As String ' byte() to string
        Return System.Text.Encoding.Default.GetString(b)
    End Function

    Function fx(ByVal b As Byte(), ByVal WRD As String) As Array ' split bytes by word
        Dim a As New List(Of Byte())
        Dim M As New IO.MemoryStream
        Dim MM As New IO.MemoryStream
        Dim T As String() = Split(BS(b), WRD)
        M.Write(b, 0, T(0).Length)
        MM.Write(b, T(0).Length + WRD.Length, b.Length - (T(0).Length + WRD.Length))
        a.Add(M.ToArray)
        a.Add(MM.ToArray)
        M.Dispose()
        MM.Dispose()
        Return a.ToArray
    End Function

    Function uFolder(ByVal BotID As String, ByVal file As String)
        If Not IO.Directory.Exists("Users" + "\" + BotID.ToString) Then
            IO.Directory.CreateDirectory("Users" + "\" + BotID.ToString)
        End If
        Return "Users" + "\" + BotID.ToString + "\" + file
    End Function

End Module