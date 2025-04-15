Imports System.Net
Imports System.IO
Imports System.Data
Imports WinSCP
Imports System.Threading
Public Class Escapade
	Const RemoteDownladpath As String = "/var/www/vhosts/www.escapade.co.uk/integration/OrderExportCSVs/"
    Const remotepath As String = "/var/www/vhosts/www.escapade.co.uk/integration/inbound/"
    Const localpath As String = "C:\inetpub\wwwroot\EscapadeMIS_wip\CSVFiles\magento\"
    Const DownLoadFolder As String = "C:\inetpub\wwwroot\EscapadeMIS_wip\CSVFiles\magento\Download\"
    Const fingerprint As String = "ssh-rsa 2048 fd:50:1f:0b:9c:a9:0e:4e:95:71:f9:68:e5:9d:c2:84"
    Const hostname As String = "172.27.14.141"
    Const PortNumber As String = "2020"
    Const UserName As String = "wwwescapadecouk"
    Const Password As String = "fTZ3V0Q7"
    Const debugmode As Boolean = false
	 Const NPOS_GOODS_IN_FILE_RECIEPT_FOLDER = "\EscapadeNpos\Website1\FromIntegration\"
    Const NPOS_GOODS_IN_FILE_RECIEPT_ARCHIVE_FOLDER = "\EscapadeNpos\Website1\FromIntegration\archive\"
    Const NPOS_GOODS_IN_FILE_RECIEPT_PREFIX = "receipts_"
    Const NPOS_GOODS_IN_FILE_RECIEPT_EXTENTION = ".csv"
	 Const IntegrationFolder = "D:\Integration"
    Shared linenumber As Integer = 16	
	 Public Shared Function GetSessionOption() As SessionOptions

        Dim sessionOptions As New SessionOptions
        With sessionOptions
            .Protocol = Protocol.Sftp
            .HostName = hostname
            .PortNumber = PortNumber
            .UserName = UserName
            .Password = Password
            .SshHostKeyFingerprint = fingerprint
        End With
        Return sessionOptions
    End Function
	
	Public Shared Function Uploadfile(filename As String, LocalSourceFolder As String, Optional ftpfolder As String = "/var/www/vhosts/www.escapade.co.uk/integration/inbound/" ) As Integer

        Try
            ' Setup session options
            Dim SessionOptions As SessionOptions = GetSessionOption()
            Dim Localfilepath As String = LocalSourceFolder & filename
            Dim RemoteFolderPath = ftpfolder
            Using session As New Session
                ' Connect
                session.Open(SessionOptions)

                ' Upload files
                Dim transferOptions As New TransferOptions
                transferOptions.TransferMode = TransferMode.Binary

                Dim transferResult As TransferOperationResult
                transferResult = session.PutFiles(Localfilepath, RemoteFolderPath, False, transferOptions)

                ' Throw on any error
                transferResult.Check()

                ' Print results
                For Each transfer In transferResult.Transfers
                    'HttpContext.Current.Response.Write("Upload succeeded=" & transfer.FileName)
                    Dim sqlstr As String = "if (not exists (select * from [FTPUploadFileList] where File_Name ='" & filename & "'))" _
                        & "begin insert into FTPUploadFileList ([File_Name], [Upload_Destination_Folder], [Upload_File_Source]) " _
                        & "VALUES ('" & filename & "', '" & RemoteFolderPath  & "','" & LocalSourceFolder & "') end;"
                    SQLFunctions.RunSQLStringWOConstring(sqlstr)
                Next
            End Using

            Return 0
        Catch ew As WebException
            HttpContext.Current.Response.Write("WebException" & ew.Message.ToString)
            Return 1
        Catch e As Exception
            HttpContext.Current.Response.Write("Exception" & e.Message.ToString)
            Return 1
        End Try

    End Function
	 Public Shared Function RenameInventoryFileOnMagento(filename As String) As Integer

        Try
            ' Setup session options
            Dim SessionOptions As SessionOptions = GetSessionOption()
            Dim RemoteFolderPath = remotepath
            Using session As New Session
                ' Connect
                session.Open(SessionOptions)

                ' Rename files
                session.MoveFile(RemoteFolderPath & filename, RemoteFolderPath & "di.csv")

            End Using

            Return 0
        Catch ew As WebException
            HttpContext.Current.Response.Write("WebException" & ew.Message.ToString)
            Return 1
        Catch e As Exception
            HttpContext.Current.Response.Write("Exception" & e.Message.ToString)
            Return 1
        End Try

    End Function
     Shared Function AddDTtoSQLTable(dt As DataTable, Optional filename As String = "", Optional NewSQLlTable As String = "tbl_WH_DailyInventotyImport_tmp") As Integer
        Dim returnvalue As Integer = 0
        Dim NumberOfFields As Integer = 0


        '       Dim NewSQLlTable As String = "tbl_WH_DailyInventotyImport_tmp"
        dt = SQLFunctions.AddColumnToDataTable(dt, "File_Name", filename)
        If NewSQLlTable = "" Then
            NewSQLlTable = "tbl_" & Replace(filename, ".", "_") & "_tmp"
        End If

        Dim isProcessed As String = SQLFunctions.GetSearchDataSQLWithOutConStr("if ( EXISTS  (select * from tbl_WH_DailyInventotyImport where File_name = '" & filename & "' )) begin select 'True' end else begin select 'false' end ;")
        ' litResponse.Text = isProcessed

        If isProcessed = "false" Then
            Dim checkifSQLTableexists As String = SQLFunctions.IfSQLTableExists(NewSQLlTable)
            If checkifSQLTableexists = "False" Then
                Dim sqlstrCreateTable As String = SQLFunctions.CreateSQLTABLE(NewSQLlTable, dt)
                SQLFunctions.RunSQLStringWOConstring(sqlstrCreateTable)
            Else
                SQLFunctions.RunSQLStringWOConstring("truncate table " & NewSQLlTable & " ;")
            End If
            SQLFunctions.SaveDTtoSqlBulk(dt, NewSQLlTable)
            SQLFunctions.RunSQLStringWOConstring("truncate table [tbl_TransferData_tmp]")
            SQLFunctions.RunSQLStringWOConstring("insert into [tbl_TransferData_tmp] ([SKU],[Qty]) select StockCode,CurrentQty from " & NewSQLlTable & ";")
            SQLFunctions.RunSQLStringWOConstring("if ( NOT EXISTS  (select * from tbl_WH_DailyInventotyImport where File_name = '" & filename & "' )) begin insert  into tbl_WH_DailyInventotyImport  ([StockCode],[StockDescription],[CurrentQty],[File_Name]) select [StockCode],[StockDescription],[CurrentQty],[File_Name]  from tbl_WH_DailyInventotyImport_tmp end;")
            returnvalue = dt.Rows.Count.ToString
        End If
        Return returnvalue
    End Function
	Shared Function LoginCheck(ByVal uderid As Object, ByVal password As String) As String
        Dim retuenvalue As String = "Invalid credentials. Please try again."
        Dim msearchcriteria As String = " User_Name='" & uderid & "' and [Password]='" & password & "'" & " and [Enabled]= 1 "
        Dim pwcheck As String = SQLFunctions.GetSearchDataSQL("AdminUsers", msearchcriteria, "User_Name")
        'HttpContext.Current.Response.Write(" pwcheck=" & pwcheck)
        If (pwcheck <> "0") Then
            retuenvalue = pwcheck
        Else
            retuenvalue = "InvalidUser"
        End If
        Return retuenvalue
    End Function
	Shared Function UpdateProductPrices_Euro() As String
        Dim returnvalue As String = "0"
        returnvalue = SQLFunctions.RunSQLStringWOConstring("execute [UpdateProductPrices_Euro] ; ")
        If returnvalue = "SQLstring Executed" Then
            returnvalue = "1"
        Else
            returnvalue = "0"
        End If
        Return returnvalue
    End Function
	Shared Function DownloadFile(ftploginname As String, ftppassword As String, ftpsiteName As String, DownloadFolder As String, localfolder As String, DownloadfileName As String, Optional FileSizeAllowed As Integer = 12048) As String
        Dim returnvalue As String = ""
        Dim localPath As String = HttpContext.Current.Server.MapPath(localfolder)
        Dim fileName As String = DownloadfileName
        Dim URI As String = ftpsiteName & DownloadFolder & fileName
        Dim bytesRead As Integer = 0
        Dim responsestatuscode As String = "Nothing Received"
        Try
            Dim requestFileDownload As FtpWebRequest = DirectCast(WebRequest.Create(URI), FtpWebRequest)
            requestFileDownload.UsePassive = False
            requestFileDownload.Credentials = New System.Net.NetworkCredential(ftploginname, ftppassword)
            requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile

            Dim responseFileDownload As FtpWebResponse = DirectCast(requestFileDownload.GetResponse(), FtpWebResponse)

            Dim responseStream As Stream = responseFileDownload.GetResponseStream()
            Dim writeStream As New FileStream(localPath & fileName, FileMode.Create)

            Dim Length As Integer = FileSizeAllowed
            Dim buffer As [Byte]() = New [Byte](Length - 1) {}

            While bytesRead > 0
                writeStream.Write(buffer, 0, bytesRead)
                bytesRead = responseStream.Read(buffer, 0, Length)
            End While
            bytesRead = responseStream.Read(buffer, 0, Length)
            responsestatuscode = responseFileDownload.StatusCode.ToString
            'Response.Write("<br>bytesRead=" & bytesRead.tostring & "<br>")
            'Response.Write("<br>responseFileDownload.StatusCode=" & responsestatuscode & "<br>")
            responseStream.Close()
            writeStream.Close()

            requestFileDownload = Nothing
            responseFileDownload = Nothing
            returnvalue = True.ToString & bytesRead & responsestatuscode
        Catch ex As Exception
            HttpContext.Current.Response.Write("<br>bytesRead=" & bytesRead.ToString & "<br>")
            HttpContext.Current.Response.Write("<br>responseFileDownload.WebResponse.StatusCode.tostring" & responsestatuscode & "<br>")
            HttpContext.Current.Response.Write("ProcessCSVFile Error:" & ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write("Last Line Processed=" & linenumber & "<br>")
            HttpContext.Current.Response.Write("File Name submitted=" & fileName & "<br>")
            HttpContext.Current.Response.Write("URI=" & URI)
            returnvalue = False
        End Try
        Return returnvalue
    End Function
	Shared Function downloadDirectory(ftploginname As String, ftppassword As String, ftpsiteName As String, DownloadFromFolder As String, localfolder As String, Optional FileSizeAllowed As Integer = 12048) As String
        'Dim localPath As String = HttpContext.Current.Server.MapPath(localfolder)
        Dim returnvalue As String = ""
        Dim fileName As String = ""
        Dim Download_Source As String = ftpsiteName & DownloadFromFolder
        Dim ftpRequest As FtpWebRequest = DirectCast(WebRequest.Create(Download_Source), FtpWebRequest)
        ftpRequest.UsePassive = False
        ftpRequest.Credentials = New NetworkCredential(ftploginname, ftppassword)
        ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory

        Dim response As FtpWebResponse = DirectCast(ftpRequest.GetResponse(), FtpWebResponse)
        Dim streamReader As New StreamReader(response.GetResponseStream())
        Dim directories As New List(Of String)()
        Dim downloadtofolder As String = HttpContext.Current.Server.MapPath(localfolder)

        Dim line As String = streamReader.ReadLine()
        While Not String.IsNullOrEmpty(line)
            directories.Add(line)
            line = streamReader.ReadLine()
        End While
        streamReader.Close()


        Using ftpClient As New WebClient()
            ftpClient.Credentials = New System.Net.NetworkCredential(ftploginname, ftppassword)

            For i As Integer = 0 To directories.Count - 1
                If directories(i).Contains("DC") Then
                    fileName = (directories(i).ToString)
                    Dim path As String = Download_Source + directories(i).ToString()
                    Dim trnsfrpth As String = downloadtofolder + directories(i).ToString()
                    If Escapade.DownloadFile(ftploginname, ftppassword, ftpsiteName, DownloadFromFolder, localfolder, fileName,FileSizeAllowed).Contains("True") Then
                        returnvalue = returnvalue + "<font color='Green'>" + (i + 1).ToString + ": Downloaded , " + fileName & "</font><br>"
                    Else
                        returnvalue = returnvalue + "<font color='red'>" + (i + 1).ToString + ": Failed Downloading , " + fileName & "</font><br>"
                    End If
                End If
            Next
        End Using
        Return returnvalue
    End Function
	Shared Function downloadDirectoryV2(DownloadFromFolder As String, localfolder As String, Optional FileNameContains As String = "*", Optional ftpsiteName As String = "ftp://ftp2.pointbidplc.com/", Optional ftploginname As String = "ESCAPA01", Optional ftppassword As String = "f8458dLN", Optional FileSizeAllowed As Integer = 12048, Optional ConfirmExist As Boolean = False) As DataTable
        If debugmode Then HttpContext.Current.Response.Write("<br>DownloadFromFolder=" & DownloadFromFolder & "<br>localfolder=" & localfolder & "<br>FileNameContains=" & FileNameContains)
        'Dim localPath As String = HttpContext.Current.Server.MapPath(localfolder)
        Dim returnvalue As New DataTable
        returnvalue.Columns.Add("Name", GetType(String))
        returnvalue.Columns.Add("Status", GetType(String))
        'returnvalue.Columns.Add("banner", GetType(String))
        Dim fileName As String = ""
        Dim Download_Source As String = ftpsiteName & DownloadFromFolder
        Dim ftpRequest As FtpWebRequest = DirectCast(WebRequest.Create(Download_Source), FtpWebRequest)
        ftpRequest.UsePassive = False
        ftpRequest.Credentials = New NetworkCredential(ftploginname, ftppassword)
        ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory

        Dim response As FtpWebResponse = DirectCast(ftpRequest.GetResponse(), FtpWebResponse)
        Dim streamReader As New StreamReader(response.GetResponseStream())
        Dim directories As New List(Of String)()
        Dim downloadtofolder As String = HttpContext.Current.Server.MapPath(localfolder)

        Dim line As String = streamReader.ReadLine()
        While Not String.IsNullOrEmpty(line)
            directories.Add(line)
            line = streamReader.ReadLine()
        End While
        streamReader.Close()

        Using ftpClient As New WebClient()
            ftpClient.Credentials = New System.Net.NetworkCredential(ftploginname, ftppassword)

			For i As Integer = 0 To directories.Count - 1
                If (directories(i).Contains(FileNameContains) Or FileNameContains = "*") Then
                    fileName = (directories(i).ToString)
                    Dim path As String = Download_Source + fileName
                    Dim trnsfrpth As String = downloadtofolder + fileName
                    Dim ifexists As Boolean = False
                    If ConfirmExist = True Then
                        ifexists = FileObjectClass.IfFileExists(trnsfrpth)
                    End If
                    Dim STRSQL As String = "if ( EXISTS  (select * from DownLoadedFilesList where File_name = '" & fileName & "' )) begin select 'True' end else begin select 'false' end ;"
                    If debugmode Then HttpContext.Current.Response.Write("STRSQL=" & STRSQL)
                    Dim isProcessed As String = SQLFunctions.GetSearchDataSQLWithOutConStr(STRSQL)
                    If isProcessed = "false" And ifexists = False Then
                        If Escapade.DownloadFTP(DownloadFromFolder, fileName, localfolder).Contains("True") Then
                            returnvalue.Rows.Add(fileName, "true")
							 If FileNameContains = "gr" Then Escapade.ftpMoveOnServer(fileName, DownloadFromFolder) ' Moves only gr files and ignore others
                        Else
                            returnvalue.Rows.Add(fileName, "false")
                        End If
                    Else
                        If FileNameContains = "DC" Then Escapade.ftpMoveOnServer(fileName, DownloadFromFolder, "possible duplicates/")
						If FileNameContains = "gr" Then Escapade.ftpMoveOnServer(fileName, DownloadFromFolder, "duplicates/") ' Moves only gr files and ignore others
                    End If
                End If
            Next

			End Using
        Return returnvalue
    End Function
	Shared Function DownloadFTP(ftpfilePath As String, ftpfileName As String, localfolder As String, Optional ftpsite As String = "ftp://ftp2.pointbidplc.com/", Optional ftploginname As String = "ESCAPA01", Optional ftppassword As String = "f8458dLN") As String
        Dim returnvalue As String = "False"
        Dim reqFTP As FtpWebRequest
        Dim URIstr As String = ftpsite & ftpfilePath '& fileName
        Dim localPath As String = HttpContext.Current.Server.MapPath(localfolder)
        Dim newuri As String = (Convert.ToString(URIstr) & ftpfileName)
        Try
            Dim outputStream As New FileStream(localPath & ftpfileName, FileMode.Create)
            reqFTP = DirectCast(FtpWebRequest.Create(New Uri(Convert.ToString(URIstr) & ftpfileName)), FtpWebRequest)
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile
            reqFTP.UsePassive = False
            reqFTP.UseBinary = True
            reqFTP.Credentials = New NetworkCredential(ftploginname, ftppassword)
            Dim response As FtpWebResponse = DirectCast(reqFTP.GetResponse(), FtpWebResponse)
            Dim ftpStream As Stream = response.GetResponseStream()
            Dim cl As Long = response.ContentLength
            ' the range should be between 8-32Kb. Poor data transfer rates may result from settings outside this range. 
            ' In no case should the value of Send buffer or Receive buffer ever be set above 65535.
            Dim bufferSize As Integer = 2048
            Dim readCount As Integer
            Dim buffer As Byte() = New Byte(bufferSize - 1) {}
            readCount = ftpStream.Read(buffer, 0, bufferSize)
            While readCount > 0
                outputStream.Write(buffer, 0, readCount)
                readCount = ftpStream.Read(buffer, 0, bufferSize)
            End While
            ftpStream.Close()
            outputStream.Close()
            response.Close()
            returnvalue = "True"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message)
            HttpContext.Current.Response.Write("<br>filePath=" & ftpfilePath)
            HttpContext.Current.Response.Write("<br>fileName=" & ftpfileName)
            HttpContext.Current.Response.Write("<br>newuri=" & newuri)
            Return returnvalue = "False"
        End Try
        Return returnvalue
    End Function
	Shared Sub ftpMoveOnServer(filename As String, Optional ftpfolder As String = "Outbound/DC/", Optional ftpMovetoFolder As String = "Archive/")
        Try
            Dim currentFolderFilename As String = ftpfolder & filename
            Dim newFolderPathFilename As String = ftpMovetoFolder & filename
            RenameFileName(currentFolderFilename, newFolderPathFilename)
        Catch ex As Exception
            HttpContext.Current.Response.Write("<br>ftpMoveOnServer ln:179" & ex.Message.ToString())
        End Try
    End Sub
	Shared Sub ftpMoveOnServerToArchive(filename As String, Optional ftpfolder As String = "Outbound/DC/", Optional ftpMovetoFolder As String = "Archive/")
        Try
            Dim currentFolderFilename As String = ftpfolder & filename
            Dim newFolderPathFilename As String = ftpMovetoFolder & filename
            RenameFileName(currentFolderFilename, newFolderPathFilename)
        Catch ex As Exception
            HttpContext.Current.Response.Write("<br>Escapade.ftpMoveOnServer ln:281" & ex.Message.ToString())
        End Try
    End Sub

    Shared Function RenameFileName(ByVal currentFilename As String, ByVal newFilename As String, Optional ftpsite As String = "ftp://ftp2.pointbidplc.com/", Optional ftploginname As String = "ESCAPA01", Optional ftppassword As String = "f8458dLN") As String
        Dim returnvalue As String = "False"
        HttpContext.Current.Response.Write(currentFilename & "<br>")
        Dim reqFTP As FtpWebRequest = Nothing
        Dim ftpStream As Stream = Nothing
        Dim targetfile As String = ftpsite & currentFilename
        Try

            reqFTP = DirectCast(FtpWebRequest.Create(targetfile), FtpWebRequest)
            reqFTP.Method = WebRequestMethods.Ftp.Rename
            reqFTP.RenameTo = newFilename
            reqFTP.UseBinary = True
            reqFTP.UsePassive = False
            reqFTP.Credentials = New NetworkCredential(ftploginname, ftppassword)
            Dim response As FtpWebResponse = DirectCast(reqFTP.GetResponse(), FtpWebResponse)
            ftpStream = response.GetResponseStream()
            ftpStream.Close()
            response.Close()
            returnvalue = "True"
        Catch ex As Exception
            If ftpStream IsNot Nothing Then
                ftpStream.Close()
                ftpStream.Dispose()
            End If
            HttpContext.Current.Response.Write("<br>Escapade.RenameFileName ln:309" & ex.Message.ToString())
            HttpContext.Current.Response.Write("<br> Current/targetfile=" & targetfile)
            HttpContext.Current.Response.Write("<br>Rename  File  to = " & newFilename)
            HttpContext.Current.Response.Write("<br>currentFilename: " & currentFilename)
            HttpContext.Current.Response.Write("<br>newFilename" & newFilename)
            Return returnvalue = "False"
        End Try
        Return returnvalue
    End Function
    Shared Sub AddToTable(filename As String, DownloadToFolder As String, Download_Source As String)
        Dim sqlstr As String = "if (not exists (select * from DownLoadedFilesList where File_Name ='" & filename & "')) " _
            & " begin insert into DownLoadedFilesList ([File_Name], [Download_Folder], [Download_Source])" _
            & " VALUES ('" & filename & "', '" & DownloadToFolder & "','" & Download_Source & "') end;"
        If debugmode Then HttpContext.Current.Response.Write("<br>" & sqlstr)
        SQLFunctions.RunSQLStringWOConstring(sqlstr)
        'MoveToDuplicateFolder(filename)
    End Sub
	Shared Sub AddToFileDownloadLog(dt As DataTable, Optional LocalSourceFolder As String = "\CSVFiles\WH\", Optional ftpfolder As String = "Outbound/", Optional ftpsite As String = "ftp://ftp2.pointbidplc.com/")
        LocalSourceFolder = HttpContext.Current.Server.MapPath(LocalSourceFolder)
        For Each row In dt.Rows
            If debugmode Then HttpContext.Current.Response.Write("<br>" & row("Name") & ", LocalSourceFolder =" & LocalSourceFolder)
            Escapade.AddToTable(row("Name"), (LocalSourceFolder), (ftpsite & ftpfolder))
        Next
    End Sub
	Shared Function UploadToWH(filename As String, LocalSourceFolder As String, Optional ftpfolder As String = "Inbound/", Optional ftpsite As String = "ftp://ftp2.pointbidplc.com/", Optional ftploginname As String = "ESCAPA01", Optional ftppassword As String = "f8458dLN") As String
        'HttpContext.Current.Response.Write("filename:" & filename & ",LocalSourceFolder:" & LocalSourceFolder & ",ftpfolder:" & ftpfolder & "<br>") : HttpContext.Current.Response.End()
        Dim returnvalue As String = ""
        Dim myFtpWebRequest As FtpWebRequest
        Dim myFtpWebResponse As FtpWebResponse
        Dim myStreamWriter As StreamWriter
        Try
            myFtpWebRequest = WebRequest.Create(ftpsite & ftpfolder & filename)
            myFtpWebRequest.Credentials = New NetworkCredential(ftploginname, ftppassword)
            myFtpWebRequest.UsePassive = False
            myFtpWebRequest.Method = WebRequestMethods.Ftp.UploadFile : linenumber = 399
            'myFtpWebRequest.UseBinary = True : Response.Write("CLng:150")

            myStreamWriter = New StreamWriter(myFtpWebRequest.GetRequestStream()) : linenumber = 402
            myStreamWriter.Write(New StreamReader(HttpContext.Current.Server.MapPath(LocalSourceFolder & filename)).ReadToEnd)
            myStreamWriter.Close()
            myFtpWebResponse = myFtpWebRequest.GetResponse() : linenumber = 405
            returnvalue = "STATUS:" & myFtpWebResponse.StatusDescription ' Label1.Text = "dsdsdsdsdsdsdsdsdsd"

            'Dim sqlstr As String = "if (not exists (select * from [FTPUploadFileList] where File_Name ='" & filename & "'))" _
            '            & "begin insert into FTPUploadFileList ([File_Name], [Upload_Destination_Folder], [Upload_File_Source]) " _
            '            & "VALUES ('" & filename & "', '" & LocalSourceFolder & "','" & ftpfolder & "') end;" : linenumber = 409
            'SQLFunctions.RunSQLStringWOConstring(sqlstr) : linenumber = 410
            InsertIntoFTPUploadFileList(filename, LocalSourceFolder, ftpfolder)
            myFtpWebResponse.Close()
        Catch ex As Exception
            HttpContext.Current.Response.Write("Escapase.uploadtoWH Error:" & ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write("URI:" & ftpsite & ftpfolder & filename & "<br>")
            HttpContext.Current.Response.Write("File Name:" & filename & "<br>")
            HttpContext.Current.Response.Write("Lastlinenumber executed:" & linenumber & "<br>")
        End Try
        Return returnvalue
    End Function
    Shared Sub InsertIntoFTPUploadFileList(filename As String, Optional LocalSourceFolder As String = "\CSVFiles\magento\", Optional ftpfolder As String = "Inbound/ASN/")
        Dim SourceFolder As String = HttpContext.Current.Server.MapPath(LocalSourceFolder)
        Dim sqlstr As String = "if (not exists (select * from [FTPUploadFileList] where File_Name ='" & filename & "'))" _
            & "begin insert into FTPUploadFileList ([File_Name], [Upload_Destination_Folder], [Upload_File_Source]) " _
            & "VALUES ('" & filename & "', '" & ftpfolder  & "','" & SourceFolder  & "') end;"
        SQLFunctions.RunSQLStringWOConstring(sqlstr)
    End Sub
	 Shared Function MoveFile(filename As String, Optional sourcefolderPath As String = "", Optional destinationfolderPath As String = "") As String

        Dim sourcefilepath As String = ""
        Dim destinationfilepath As String = ""
        Try
            If sourcefolderPath = "" Then sourcefolderPath = HttpContext.Current.Server.MapPath(DownLoadFolder & filename) Else sourcefolderPath = (sourcefolderPath & filename)
            If destinationfolderPath = "" Then destinationfolderPath = HttpContext.Current.Server.MapPath(DownLoadFolder & "Archive\" & filename) Else destinationfolderPath = destinationfolderPath & filename
            'HttpContext.Current.Response.Write("destinationfolderPath:" & destinationfolderPath & ",  sourcefilepath:" & sourcefolderPath & "<br>")
            Dim repl As String = FileObjectClass.MovefileOnServer(sourcefolderPath, destinationfolderPath).ToString
            Return repl
        Catch ex As Exception
            HttpContext.Current.Response.Write("Error movefiles:" & ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write("sourcefilepath:" & sourcefilepath & "<br>")
            HttpContext.Current.Response.Write("destinationfolderPath:" & destinationfolderPath & "<br>")
            HttpContext.Current.Response.Write("File Name:" & filename & "<br>")
            Return "failed"
        End Try
    End Function
    Shared Function GetExchangeRateMonthlyAcverage() As String
        '  Dim dt As DataTable = SQLFunctions.GetDataTablefromSQLDatasource(SqlDataSource2_AvgMonthly)
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr("SELECT TOP (1) RateYear, RateMonth, AVG(EUROtoGBP) AS avg_EUROtoGBP, AVG(GBPtoEURO) AS avg_GBPtoEURO " _
        & " FROM (SELECT YEAR(Date_Added) AS RateYear, MONTH(Date_Added) AS RateMonth, GBP AS EUROtoGBP, 1 / GBP AS GBPtoEURO FROM exchange_rate_history) AS dd " _
        & " GROUP BY RateMonth, RateYear ORDER BY RateYear DESC, RateMonth DESC")
        Dim LatestMonthRate As String = dt.Rows(0)("RateMonth").ToString
        Dim myeartoday As Integer = Now.Year
        Dim mmonthtoday As Integer = Now.Month
        Dim GBPtoEURO As String = dt.Rows(0)("avg_GBPtoEURO").ToString
        Dim EurotoGBP As String = dt.Rows(0)("avg_EurotoGBP").ToString
        Dim enteredBy As String = "system"
        Dim Datetoday As String = MiscClass.GetBritishShortDate(Now)
        Dim DateRecorded As String = MiscClass.GetBritishShortDate(Now)
        Dim lastupdateYear = SQLFunctions.MinMaxValueSQLWithCriteria("ExchangeRateMonthlyAverage", "Year", "max", " 1=1 ")
        Dim lastupdatemonth = SQLFunctions.MinMaxValueSQLWithCriteria("ExchangeRateMonthlyAverage", "Month", "max", " year='" & lastupdateYear & "'")
        lastupdatemonth = MiscClass.ifnullget(lastupdatemonth, "0")

        If (lastupdatemonth < mmonthtoday And myeartoday = lastupdateYear) Or (myeartoday > lastupdateYear And lastupdatemonth > mmonthtoday) Then
            SQLFunctions.AddRecToTableSQL("ExchangeRateMonthlyAverage", "year,month,EurotoGBP,GBPtoEURO,EnterdBy,Date,DateRecorded", "'" & myeartoday & "','" & mmonthtoday & "','" & GBPtoEURO & "','" & EurotoGBP & "','" & enteredBy & "','" & Datetoday & "','" & DateRecorded & "'")
            ' Label3.text = ("New record Added ")
        Else
            ' Label3.text = (" Nothing added")
        End If
        Return GBPtoEURO.ToString

    End Function
	
	Shared Function SaveToExportLog(filename as string, ftpfolder as string, LocalSourceFolder as string )
		SQLFunctions.RunSQLStringWOConstring("insert into  [FTPUploadFileList] ([File_Name],[Upload_Destination_Folder],[Upload_File_Source],[Date_Processed]) VALUES ('" & filename & "','" & ftpfolder & "','" & LocalSourceFolder & "','" & MiscClass.DatetoSQLMonthName(Now.ToString) & "');")
		'Thread.Sleep(1000)
		'escapade.MoveFile(filename,LocalSourceFolder,(LocalSourceFolder & "Archive\"))
	End Function
	Shared Function CreateNposGoodsInFileName() As String
        Dim rv As String = ""
        rv = NPOS_GOODS_IN_FILE_RECIEPT_PREFIX & CreateNposUniqueFileNameIdentifier() & NPOS_GOODS_IN_FILE_RECIEPT_EXTENTION
        Return rv
    End Function
    Shared Function GetNposGoodsInFOlderpath() As String
        Dim rv As String = ""
        rv = IntegrationFolder & NPOS_GOODS_IN_FILE_RECIEPT_FOLDER
        Return rv
    End Function

    Shared Function CreateNposUniqueFileNameIdentifier() As String
        Dim dDate As String = Now()
        Dim rv As String = ""
        rv = Year(dDate) & Right("00" & CStr(Month(dDate)), 2) & Right("00" & CStr(Day(dDate)), 2) & "_" & Right("00" & CStr(Hour(dDate)), 2) & "." & Right("00" & CStr(Minute(dDate)), 2) & "." & Right("00" & CStr(Second(dDate)), 2)
        Return rv
    End Function
	 Shared Function LastDespatchConfirmationDateTime() As String
        Dim rv As String = ""
        rv = DateClass.GetTimePart(SQLFunctions.GetSearchDataSQLWithOutConStr("select top(1) Date_Processed from [DownLoadedFilesList] where Download_Folder = 'D:\IIS\IIS Content\MIS.escapade.co.uk\CSVFiles\WH\DC\' and isnull(date_processed,'')<> ''  order by recID desc").ToString)
        Return rv
    End Function
	 Shared Function updateExchageRate() As String
        Dim rv As String = ""
        Dim strsql As String = "SELECT TOP (1) RateYear, RateMonth, AVG(EUROtoGBP) AS avg_EUROtoGBP, AVG(GBPtoEURO) AS avg_GBPtoEURO " _
        & " FROM (Select YEAR(Date_Added) As RateYear, MONTH(Date_Added) As RateMonth, GBP As EUROtoGBP, 1 / GBP As GBPtoEURO FROM exchange_rate_history) As dd " _
        & " GROUP BY RateMonth, RateYear ORDER BY RateYear DESC, RateMonth DESC"
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql)
        Dim LatestMonthRate As String = dt.Rows(0)("RateMonth").ToString
        Dim myeartoday As Integer = Now.Year
        Dim mmonthtoday As Integer = Now.Month
        Dim GBPtoEURO As String = dt.Rows(0)("avg_GBPtoEURO").ToString
        Dim EurotoGBP As String = dt.Rows(0)("avg_EurotoGBP").ToString
        Dim enteredBy As String = "system"
        Dim Datetoday As String = MiscClass.GetBritishShortDate(Now)
        Dim DateRecorded As String = MiscClass.GetBritishShortDate(Now)
        Dim lastupdateYear = SQLFunctions.MinMaxValueSQLWithCriteria("ExchangeRateMonthlyAverage", "Year", "max", " 1=1 ")
        Dim lastupdatemonth = SQLFunctions.MinMaxValueSQLWithCriteria("ExchangeRateMonthlyAverage", "Month", "max", " year='" & lastupdateYear & "'")
        lastupdatemonth = MiscClass.ifnullget(lastupdatemonth, "0")
        'dt = Nothing
        dim strsql2_update as string = ""
		dim strsql2 as string = ""
        If (lastupdatemonth < mmonthtoday And myeartoday = lastupdateYear) Or (myeartoday > lastupdateYear And lastupdatemonth > mmonthtoday) Then
            SQLFunctions.AddRecToTableSQL("ExchangeRateMonthlyAverage", "year,month,EurotoGBP,GBPtoEURO,EnterdBy,Date,DateRecorded", "'" & myeartoday & "','" & mmonthtoday & "','" & GBPtoEURO & "','" & EurotoGBP & "','" & enteredBy & "','" & Datetoday & "','" & DateRecorded & "'")
        Else
		    strsql2_update ="update ExchangeRateMonthlyAverage set EurotoGBP='" & GBPtoEURO & "' ,GBPtoEURO= '" & EurotoGBP & "' ,EnterdBy= 'System' , DateRecorded= getdate() where  year = year(getdate()) and month = month(getdate()) ;  "
			strsql2 = "declare @lastdate as date ;set @lastdate = (select convert(date,DateRecorded,103)from ExchangeRateMonthlyAverage where year = year(getdate()) and month = month(getdate()) ) ;if @lastdate = convert(date,GETDATE(),103) begin  " & strsql2_update & "  end"
			
			'strsql2 = "declare @lastdate as date ;set @lastdate = (select convert(date,DateRecorded,103)from ExchangeRateMonthlyAverage where year = year(getdate()) and month = month(getdate()) ) ;if @lastdate <> convert(date,GETDATE(),103) begin  update ExchangeRateMonthlyAverage set EurotoGBP='1.14925837462902' ,GBPtoEURO= '0.87046275862069' ,EnterdBy= 'System' , DateRecorded= getdate() where year = year(getdate()) and month = month(getdate()) ;  end"
            SQLFunctions.RunSQLStringWOConstring(strsql2)
        End If
        'dt = SQLFunctions.GetDataTableFromSqlstr(strsql)
        ' HttpContext.Current.Response.Write(strsql2)
        ' GBPtoEURO = dt.Rows(0)("avg_GBPtoEURO").ToString
        dt = Nothing
        rv = GBPtoEURO
        Return rv & strsql2
    End Function
	Shared Function GetStockSummaryTable() As DataTable
        Dim rv As DataTable
        Dim strsql As String = "select COUNT(*)skus, COUNT(case when (isnull(Widd_Qty, 0)+isnull(amazon_qty,0))>0 then 1 end) As withstock, " _
            & "sum(isnull(Widd_Qty, 0) + isnull(amazon_qty, 0)) As stock , sum(Buy_Price* (isnull(Widd_Qty, 0) + isnull(amazon_qty, 0))) as StockValue from products with (NOLOCK) where 1= 1 And Product_SKU Not In ('0240000000017','0240000000031','duplicate') " _
            & "And products.Category_ID Not in(2136) And product_type='sale';"
        rv = SQLFunctions.GetDataTableFromSqlstr(strsql)
        Return rv

    End Function
End Class
