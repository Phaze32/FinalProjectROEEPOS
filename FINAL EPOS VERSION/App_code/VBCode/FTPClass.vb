Imports System.Collections.Generic
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security

Namespace FtpFramework
    Public Class FTP
        Public UserName As String = ""
        Public Password As String = ""
        Public KeepAlive As Boolean = False
        Public UseSSL As Boolean = True
        Private m_FTPSite As String = ""
        Public Property FTPSite() As String
            Get
                Return m_FTPSite
            End Get
            Set(value As String)
                m_FTPSite = value
                If Not m_FTPSite.EndsWith("/") Then
                    m_FTPSite += "/"
                End If
            End Set
        End Property
        Private m_CurDir As String = ""
        Public Property CurrentDirectory() As String
            Get
                Return m_CurDir
            End Get
            Set(value As String)
                m_CurDir = value
                If Not m_CurDir.EndsWith("/") AndAlso m_CurDir <> "" Then
                    m_CurDir += "/"
                End If
                m_CurDir = m_CurDir.TrimStart("/".ToCharArray())
            End Set
        End Property

        Public Sub New()
        End Sub
        Public Sub New(sFTPSite As String, sUserName As String, sPassword As String)
            UserName = sUserName
            Password = sPassword
            FTPSite = sFTPSite
        End Sub

        Public Shared Function ValidateServerCertificate(sender As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors__1 As SslPolicyErrors) As Boolean
            If sslPolicyErrors__1 = SslPolicyErrors.RemoteCertificateChainErrors Then
                Return False
            ElseIf sslPolicyErrors__1 = SslPolicyErrors.RemoteCertificateNameMismatch Then
                Dim z As System.Security.Policy.Zone = System.Security.Policy.Zone.CreateFromUrl(DirectCast(sender, HttpWebRequest).RequestUri.ToString())
                If z.SecurityZone = System.Security.SecurityZone.Intranet OrElse z.SecurityZone = System.Security.SecurityZone.MyComputer Then
                    Return True
                End If
                Return False
            End If
            Return True
        End Function

        Public Function GetFileList(CurDirectory As String, StartsWith As String, EndsWith As String) As List(Of String)
            CurrentDirectory = CurDirectory
            Return GetFileList(StartsWith, EndsWith)
        End Function
        Public Function GetFileList(StartsWith As String, EndsWith As String) As List(Of String)
            Dim oFTP As FtpWebRequest = DirectCast(FtpWebRequest.Create(FTPSite & CurrentDirectory), FtpWebRequest)
            'oFTP.EnableSsl = true;
            oFTP.Credentials = New NetworkCredential(UserName, Password)
            oFTP.KeepAlive = KeepAlive
            oFTP.EnableSsl = UseSSL
            ' Validate the server certificate with
            ' ServerCertificateValidationCallBack
            If UseSSL Then
                ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)
            End If
            'System.Security.Cryptography.X509Certificates.
            'X509Certificate oCert = new System.Security.Cryptography.
            'X509Certificates.X509Certificate();
            'oFTP.ClientCertificates.Add(oCert);

            oFTP.Method = WebRequestMethods.Ftp.ListDirectory
            Dim response As FtpWebResponse = DirectCast(oFTP.GetResponse(), FtpWebResponse)
            Dim sr As New StreamReader(response.GetResponseStream())
            Dim str As String = sr.ReadLine()
            Dim oList As New List(Of String)()
            While str IsNot Nothing
                If str.StartsWith(StartsWith) AndAlso str.EndsWith(EndsWith) Then
                    oList.Add(str)
                End If
                str = sr.ReadLine()
            End While
            sr.Close()
            response.Close()
            oFTP = Nothing

            Return oList
        End Function

        Public Function GetFile(Name As String, DestFile As String) As Boolean
            '1. Create a request: must be in ftp://hostname format,
            ' not just ftp.myhost.com
            Dim oFTP As FtpWebRequest = DirectCast(FtpWebRequest.Create(FTPSite & CurrentDirectory & Name), FtpWebRequest)
            'oFTP.EnableSsl = true;
            '2. Set credentials
            oFTP.Credentials = New NetworkCredential(UserName, Password)
            'Define the action required (in this case, download
            ' a file)
            oFTP.Method = WebRequestMethods.Ftp.DownloadFile

            '3. Settings
            oFTP.KeepAlive = KeepAlive
            oFTP.EnableSsl = UseSSL
            ' Validate the server certificate with
            ' ServerCertificateValidationCallBack
            If UseSSL Then
                ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)
            End If
            'we want a binary transfer, not textual data
            oFTP.UseBinary = True

            '4. If we were using a method that uploads data;
            ' for example, UploadFile, we would open the
            ' ftp.GetRequestStream here an send the data

            '5. Get the response to the Ftp request and the
            ' associated stream
            Dim response As FtpWebResponse = DirectCast(oFTP.GetResponse(), FtpWebResponse)
            Dim responseStream As Stream = response.GetResponseStream()
            'loop to read & write to file
            Dim fs As New FileStream(DestFile, FileMode.Create)
            Dim buffer As [Byte]() = New [Byte](2046) {}
            Dim read As Integer = 1
            While read <> 0
                read = responseStream.Read(buffer, 0, buffer.Length)
                fs.Write(buffer, 0, read)
            End While
            'see Note(1)
            responseStream.Close()
            fs.Flush()
            fs.Close()
            responseStream.Close()
            response.Close()
            oFTP = Nothing

            Return True
        End Function

        Public Function UploadFile(oFile As FileInfo) As Boolean
            Dim ftpRequest As FtpWebRequest
            Dim ftpResponse As FtpWebResponse

            Try
                'Settings required to establish a connection with
                'the server
                ftpRequest = DirectCast(FtpWebRequest.Create(FTPSite & CurrentDirectory & oFile.Name), FtpWebRequest)
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile
                ftpRequest.Proxy = Nothing
                ftpRequest.UseBinary = True
                ftpRequest.Credentials = New NetworkCredential(UserName, Password)
                ftpRequest.KeepAlive = KeepAlive
                ftpRequest.EnableSsl = UseSSL
                ' Validate the server certificate with
                ' ServerCertificateValidationCallBack
                If UseSSL Then
                    ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)
                End If

                'Selection of file to be uploaded
                Dim fileContents As Byte() = New Byte(oFile.Length - 1) {}

                'will destroy the object immediately after being used
                Using fr As FileStream = oFile.OpenRead()
                    fr.Read(fileContents, 0, Convert.ToInt32(oFile.Length))
                End Using
                Using writer As Stream = ftpRequest.GetRequestStream()
                    writer.Write(fileContents, 0, fileContents.Length)
                End Using
                'Gets the FtpWebResponse of the uploading operation
                ftpResponse = DirectCast(ftpRequest.GetResponse(), FtpWebResponse)
                'Display response
                'Response.Write(ftpResponse.StatusDescription);

                ftpResponse.Close()
                ftpRequest = Nothing

                Return True
            Catch webex As WebException
                'this.Message = webex.ToString();
                Return False
            End Try
        End Function

        Public Function DeleteFile(Name As String) As Boolean
            '1. Create a request: must be in ftp://hostname format,
            ' not just ftp.myhost.com
            Dim oFTP As FtpWebRequest = DirectCast(FtpWebRequest.Create(FTPSite & CurrentDirectory & Name), FtpWebRequest)
            'oFTP.EnableSsl = true;
            '2. Set credentials
            oFTP.Credentials = New NetworkCredential(UserName, Password)
            'Define the action required (in this case, download a file)
            oFTP.Method = WebRequestMethods.Ftp.DeleteFile

            '3. Settings
            oFTP.KeepAlive = KeepAlive
            oFTP.EnableSsl = UseSSL
            ' Validate the server certificate with
            ' ServerCertificateValidationCallBack
            If UseSSL Then
                ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)
            End If
            'we want a binary transfer, not textual data
            oFTP.UseBinary = True

            '4. If we were using a method that uploads data;
            ' for example, UploadFile, we would open the
            ' ftp.GetRequestStream here an send the data

            '5. Get the response to the Ftp request and the associated
            ' stream
            Dim response As FtpWebResponse = DirectCast(oFTP.GetResponse(), FtpWebResponse)
            Dim oStat As FtpStatusCode = response.StatusCode
            response.Close()
            oFTP = Nothing

            Return True
        End Function
        ' DeleteFile()

    End Class
End Namespace