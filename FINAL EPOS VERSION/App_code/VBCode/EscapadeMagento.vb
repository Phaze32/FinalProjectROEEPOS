Imports Microsoft.VisualBasic
Imports System.Net
Imports System.IO
Imports System.Data
Imports WinSCP
Public Class EscapadeMagento
    '****************** FTP Credential Magento Server ******************

    '**************** FILE & FOLDER LOCATION CONSTANTS ****************
    Const WIDDISON_FROM_FOLDER = "\widdowson\from\"
    Const WIDDISON_TO_FOLDER = "\widdowson\sendorders\"
    Const WIDDISON_TO_PURCHASE_ORDER_FOLDER = "\widdowson\purchaseorders\"
    Const WIDDISON_TO_ARCHIVE_FOLDER = "\widdowson\sendorders\archived\"
    Const WIDDISON_FROM_ARCHIVE_FOLDER = "\widdowson\from\archived\"
    Const NPOS_FROM_FOLDER = "\EscapadeNpos\Website1\"

    Const NPOS_FROM_ARCHIVE_FOLDER = "\EscapadeNpos\Website1\ASNarchive\"
    Const WIDDOWSON_SENT_ORDERS_FILE_POSTFIX = "ord"
    Const WIDDOWSON_PURCHASE_ORDERS_FILE_POSTFIX = "po"
    Const WIDDOWSON_PRODUCT_CATALOG_FILE_POSTFIX = "gd"
    Const WIDDOWSON_SENT_ORDERS_FILE_EXTENTION = ".csv"
    Const WIDDOWSON_PURCHASE_ORDERS_FILE_EXTENTION = ".csv"
    Const WIDDOWSON_PRODUCT_CATALOG_FILE_EXTENTION = ".csv"
    Const WIDDISON_PRODUCT_CATALOG_FOLDER = "\widdowson\productcatalog\"
    'CONST WIDDISON_PRODUCT_CATALOG_FOLDER = "\widdowson\Temp_productcatalog\"
    Const WIDDISON_PRODUCT_CATALOG_ARCHIVE_FOLDER = "\widdowson\productcatalog\archived\"

    Const WIDDOWSON_GOODS_IN_FILE_PREFIX = "gin"
    Const WIDDOWSON_RECIEVED_FILE_PREFIX = "orc"
    'CONST WIDDOWSON_DISPATCHED_FILE_PREFIX 		 = "ack"
    Const WIDDOWSON_DISPATCHED_FILE_PREFIX = "ack"
    Const WIDDOWSON_INVENTORY_FILE_PREFIX = "di"
    Const WIDDOWSON_TRACKING_DETAILS_FILE_PREFIX = "tra"

    Const NPOS_ASN_FILE_PREFIX = "rmasn"

    Const NPOS_GOODS_IN_FILE_RECIEPT_FOLDER = "\EscapadeNpos\Website1\FromIntegration\"
    Const NPOS_GOODS_IN_FILE_RECIEPT_ARCHIVE_FOLDER = "\EscapadeNpos\Website1\FromIntegration\archive\"
    Const NPOS_GOODS_IN_FILE_RECIEPT_PREFIX = "receipts_"
    Const NPOS_GOODS_IN_FILE_RECIEPT_EXTENTION = ".csv"

    Const NPOS_INVENTORY_FILE_FOLDER = "\EscapadeNpos\Website1\FromIntegration\"
    Const NPOS_INVENTORY_FILE_ARCHIVE_FOLDER = "\EscapadeNpos\Website1\FromIntegration\archive\"
    Const NPOS_INVENTORY_FILE_PREFIX = "inventory_"
    Const NPOS_INVENTORY_FILE_EXTENTION = ".csv"

    Const DISPATCHED_EMAIL_TEMPLATE = "\Admin\Emails\RoyalMailManifoldProcessedEmail1.txt"
    Const SALES_CONFIRMATION_EMAIL_TEMPLATE = "\Emails\SalesConfirmation.txt"

    Const AMAZON_SHIPPING_UPDATE_1 = 1
    Const AMAZON_TRACKING_UPDATE_1 = 2
    '************FTP Related ************************
    Const RemoteDownladpath As String = "/var/www/vhosts/www.escapade.co.uk/integration/OrderExportCSVs/"
    Const remotepath As String = "/var/www/vhosts/www.escapade.co.uk/integration/inbound/"
    Const localpath As String = "C:\inetpub\wwwroot\EscapadeMIS_wip\CSVFiles\magento\"
    Const DownLoadFolder As String = "C:\inetpub\wwwroot\EscapadeMIS_wip\CSVFiles\magento\Download\"
    Const fingerprint As String = "ssh-rsa 2048 fd:50:1f:0b:9c:a9:0e:4e:95:71:f9:68:e5:9d:c2:84"
    Const hostname As String = "46.37.182.93"
    Const PortNumber As String = "2020"
    Const UserName As String = "wwwescapadecouk"
    Const Password As String = "fTZ3V0Q7"
    Const debugmode As Boolean = True

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
    Public Shared Function Uploadfile(filename As String, LocalSourceFolder As String) As Integer

        Try
            ' Setup session options
            Dim SessionOptions As SessionOptions = GetSessionOption()
            Dim Localfilepath As String = LocalSourceFolder & filename
            Dim RemoteFolderPath = remotepath
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
                        & "VALUES ('" & filename & "', '" & LocalSourceFolder & "','" & RemoteFolderPath & "') end;"
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
        Try
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
                SQLFunctions.RunSQLStringWOConstring("if ( NOT EXISTS  (select * from tbl_WH_DailyInventotyImport where File_name = '" & filename & "' )) begin insert  into tbl_WH_DailyInventotyImport  ([StockCode],[BarCode1],[StockDescription],[CurrentQty],[File_Name]) select [StockCode],[BarCode1],[StockDescription],[CurrentQty],[File_Name]  from tbl_WH_DailyInventotyImport_tmp end;")
                returnvalue = dt.Rows.Count.ToString
            End If
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message.ToString)
        End Try
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
    Shared Function ProcessMagentoSalesOrder(ByRef dt As DataTable) As Integer
        Dim Returnvalue As Integer = 0
        ' Common itmes
        Dim ItemsProcessed As Integer = 0

        ' repeate items
        Dim ItemId As String
        Dim PROD_NAME As String
        Dim ProductColour As String
        Dim ProductSize As String
        Dim QUANTITY As String
        Dim UnitPRICE As Double
        Dim TotalPrice As Double
        Dim dt22 As DataTable = dt
        ' HttpContext.Current.Response.Write("####")
        Dim neworder_id As Integer = insertcommaonitems(dt22)

        ' HttpContext.Current.Response.Write("####") : HttpContext.Current.Response.End()
        If neworder_id <> 0 Then

            For Each row In dt.Rows

                ItemId = row("ItemId")
                ItemId = GetProduct_ID(ItemId)
                PROD_NAME = row("PROD_NAME")
                ProductColour = row("ProductColour")
                ProductSize = row("ProductSize")
                QUANTITY = row("QUANTITY")
                UnitPRICE = row("UnitPRICE")
                TotalPrice = row("TotalPrice")
                ' HttpContext.Current.Response.Write("ItemId" & ItemId & "##")
                Dim strsql_Items As String = "insert into salesorder_items ([Order_ID],[Product_ID],[Size],[Quantity],[Price],[Amount])" _
                & "values ('" & neworder_id & "','" & ItemId & "','" & ProductSize & "','" & QUANTITY & "','" & UnitPRICE & "','" & TotalPrice & "')"
                'HttpContext.Current.Response.Write("####" & strsql_Items)
                SQLFunctions.RunSQLStringWOConstring(strsql_Items)
                ItemsProcessed = ItemsProcessed + 1
            Next
            dt22.Dispose()
            dt.Dispose()
            Returnvalue = ItemsProcessed
        End If
        Return Returnvalue
    End Function
    Shared Function GetProduct_ID(product_sku As String) As String
        Dim Product_ID As String
        Product_ID = SQLFunctions.GetSearchDataSQLWithOutConStr("select Product_ID from Products where product_sku ='" & product_sku & "'")
        Return Product_ID
    End Function

    Shared Function insertcommaonitems(dt As DataTable) As String
        Dim ORDER_ID As Integer
        Dim ORDER_DATE As DateTime
        Dim CUST_NAME As String
        Dim CUST_LNAME As String
        Dim HOUSENO As String
        Dim ROAD As String
        Dim CITY As String
        Dim COUNTY As String
        Dim POSTCODE As String
        Dim CountryName As String
        Dim DEL_HOUSENO As String
        Dim DEL_ROAD As String
        Dim DEL_CITY As String
        Dim DEL_COUNTY As String
        Dim DEL_POSTCODE As String
        Dim DEL_COUNTRY As String
        Dim PHONE As String
        Dim Email As String
        Dim GrandTotal As Double
        Dim DELIVERY_COST As Double
        Dim VAT_COST As Double
        Dim Noofitems As Integer

        Dim neworderid As Integer


        Dim OID As Integer = 0
        Dim strsql_OrdRef As String = ""
        For Each row In dt.Rows
            ORDER_ID = row("ORDER_ID")
            ORDER_DATE = row("ORDER_DATE")
            CUST_NAME = row("CUST_NAME")
            CUST_LNAME = row("CUST_LNAME")
            HOUSENO = row("HOUSENO")
            ROAD = row("ROAD")
            CITY = row("CITY")
            COUNTY = row("COUNTY")
            POSTCODE = row("POSTCODE")
            CountryName = row("CountryName")
            DEL_HOUSENO = row("DEL_HOUSENO")
            DEL_ROAD = row("DEL_ROAD")
            DEL_CITY = row("DEL_CITY")
            DEL_COUNTY = row("DEL_COUNTY")
            DEL_POSTCODE = row("DEL_POSTCODE")
            DEL_COUNTRY = row("DEL_COUNTRY")
            PHONE = row("PHONE")
            Email = row("Email")
            GrandTotal = row("GrandTotal")
            DELIVERY_COST = row("DELIVERY_COST")
            VAT_COST = row("VAT_COST")
            Noofitems = row("Noofitems")
            Exit For
        Next
        Dim ifexists As Integer = SQLFunctions.GetSearchDataSQLWithOutConStr("select order_id from salesorder where Magento_OrderID ='" & ORDER_ID & "'")
        If ifexists = "00" Then
            'Process 2.  saves data and creates new order
            strsql_OrdRef = "insert into salesorder " _
            & "([OrderDate],[BillingName],[BillingSurname],[BillingAddress],[BillingAddress1],[BillingTown],[BillingCounty],[BillingPostCode],[BillingCountry],[ShippingAddress],[ShippingAddress1],[ShippingTown],[ShippingCounty],[ShippingPostCode],[ShippingCountry],[ContactPhone],[ContactEmail],[OrderAmount],[Shipping_Charges],[VAT_Total],No_of_Items,[Magento_OrderID],[ShippingName],[ShippingSurname])" _
            & "values ('" & ORDER_DATE & "','" & CUST_NAME & "','" & CUST_LNAME & "','" & HOUSENO & "','" & ROAD & "','" & CITY & "','" & COUNTY & "','" & POSTCODE & "','" & CountryName & "','" & DEL_HOUSENO & "','" & DEL_ROAD & "','" & DEL_CITY & "','" & DEL_COUNTY & "','" & DEL_POSTCODE & "','" & DEL_COUNTRY & "','" & PHONE & "','" & Email & "','" & GrandTotal & "','" & DELIVERY_COST & "','" & VAT_COST & "','" & Noofitems & "','" & ORDER_ID & "','" & CUST_NAME & "','" & CUST_LNAME & "')"
            SQLFunctions.RunSQLStringWOConstring(strsql_OrdRef)

            'Process 3.  Gets new order_id for the newly created order
            neworderid = SQLFunctions.GetSearchDataSQLWithOutConStr("select order_id from salesorder where Magento_OrderID ='" & ORDER_ID & "'")
            Dim neworderref As String = GetNewOrderRef(neworderid)
            SQLFunctions.GetSearchDataSQLWithOutConStr("Update salesorder set orderref = '" & neworderref & "' where Magento_OrderID ='" & ORDER_ID & "'")
        Else
            neworderid = 0
        End If

        Return neworderid
    End Function

    Shared Function GetNewOrderRef(orderid As Integer) As String
        Dim StrRnd As String = "ESS-" & MiscClass.addpadding(Now.Day.ToString, 2) & MiscClass.addpadding(Now.Month.ToString, 2) & orderid.ToString
        Return StrRnd
    End Function
    Function GetTempOrderRef(orderid As Integer) As String
        Dim StrRnd As String = "ESS-" & MiscClass.addpadding(Now.Day.ToString, 2) & MiscClass.addpadding(Now.Month.ToString, 2) & orderid.ToString
        Return StrRnd
    End Function

    'Private Sub ProcessWiddowsonInventoryFile(file)
    '    Dim filename : filename = file.path
    '    If WeekdayName(Weekday(Of Date)) <> "Saturday" And WeekdayName(Weekday(Of Date)) <> "Sunday" Then
    '        Dim strFileContents : strFileContents = Replace(GetTextFileContents(filename), Chr(34), "")
    '        Dim arrFileRow : arrFileRow = Split(strFileContents, vbCrLf)
    '        Dim intCounter
    '        Dim blnProductExists : blnProductExists = False

    '        Dim inventory_date : inventory_date = convertWiddowsonInventoryFilenameToDate(file.name) 'ADDED by shaun.lewis on 18/jul/2014
    '        Dim strNposInventoryHeader : strNposInventoryHeader = "SKU,QUANTITY,TRANSACTION_TYPE,ADJUSTMENT_CODE,ADJUSTMENT_DESCRIPTION,TIMESTAMP" & vbCrLf
    '        Dim TRANSACTION_TYPE : TRANSACTION_TYPE = "ABSOLUTE"
    '        Dim ADJUSTMENT_CODE : ADJUSTMENT_CODE = "A001"
    '        Dim ADJUSTMENT_DESCRIPTION : ADJUSTMENT_DESCRIPTION = "ABSOLUTE STOCK LEVELS"
    '        Dim clsFastString :  Set clsFastString = New FastString 

    '    clsFastString.Append(strNposInventoryHeader)

    '        For intCounter = 0 To UBound(arrFileRow)
    '            Dim arrFileColumn : arrFileColumn = Split(arrFileRow(intCounter), ",")
    '            If UBound(arrFileColumn) >= 2 Then
    '                Dim Product_Sku : Product_Sku = Trim(arrFileColumn(0))
    '                Dim Widd_Qty : Widd_Qty = arrFileColumn(1)
    '                Dim Quarantine : Quarantine = UCase(arrFileColumn(2))
    '                Dim dtDateTimeNow : dtDateTimeNow = Now()
    '                Dim dateTimeStamp : dateTimeStamp = Year(dtDateTimeNow) & "-" & Right("00" & CStr(Day(dtDateTimeNow)), 2) & "-" & Right("00" & CStr(Month(dtDateTimeNow)), 2) & "T" & Hour(dtDateTimeNow) & ":" & Minute(dtDateTimeNow) & ":" & Right("00" & CStr(Second(dtDateTimeNow)), 2)

    '                'Response.write "'"&Product_Sku&",'<br>"
    '                If Len(Product_Sku) = 13 And IsNumeric(Widd_Qty) Then ' added this on 17/oct/2014 by shaun lewis as the warehouse sent a file with no quantity, so just another one of the MANY safety measures for those guys
    '                    blnProductExists = UpdateProductsFromWiddowsonInventoryFile(Widd_Qty, Quarantine, Product_Sku, inventory_date, file.name)
    '                Else
    '                    blnProductExists = False
    '                End If

    '                If blnProductExists Then
    '                    HttpContext.Current.Response.Write("This sku does exist (" & Product_Sku & ") quantity =" & Widd_Qty & "<br>")
    '                    If Product_Sku <> "0000000000000" And Product_Sku <> "BLACK WRAP PLT" And Product_Sku <> "UNKNOWN PLTS" And LCase(Product_Sku) <> "duplicate" Then
    '                        clsFastString.Append(Product_Sku & "," & Widd_Qty & "," & TRANSACTION_TYPE & "," & ADJUSTMENT_CODE & "," & ADJUSTMENT_DESCRIPTION & "," & dateTimeStamp & vbCrLf)
    '                    End If
    '                Else
    '                    HttpContext.Current.Response.Write("This sku does not exist (" & Product_Sku & ")<br>")
    '                End If

    '            End If
    '        Next
    '        'Nasty way of doing it, he he
    '        Dim s : s = clsFastString.ToString()
    '        s = Left(s, Len(s) - 1)

    '        CreateInventoryFileForNPosFromWiddowson(s)
    '        DestryObject(clsFastString)
    '    End If
    '    MoveProcessedFileToArchivedFolder(filename)

    'End Sub

    'Function convertWiddowsonInventoryFilenameToDate(dt)
    '    Dim RegExp :  Set RegExp = New RegExp
    '    With (RegExp)
    '        .Global = True
    '        .Pattern = "\D" 'matches all non-digits
    '        dt = .Replace(dt, "") 'all non-digits removed
    '    End With

    '    'response.write "day="& mid(dt,7,2)&"<br>"
    '    'response.write "month="&mid(dt,5,2)&"<br>"
    '    'response.write "year="&left(dt,4)  &"<br>"
    '    'response.write "dt="&dt  &"<br>"
    '    'response.end

    '    convertWiddowsonInventoryFilenameToDate = Mid(dt, 7, 2) & "/" & MonthName(Mid(dt, 5, 2), True) & "/" & Left(dt, 4)
    'End Function

    'Private Function GetTextFileContents(filename)
    '    HttpContext.Current.Response.Write("<b>Opening File " & filename & "</b><br>")
    '    'Dim oFile : Set oFile = objFSO.GetFile(filename)
    '    'If oFile.Size > 0 Then
    '    Dim objFile :   Set objFile= objFSO.OpenTextFile(filename)
    '    Dim strFileContents : strFileContents = objFile.ReadAll()
    '    DestryObject(objFile)
    '    GetTextFileContents = strFileContents
    'End Function
    'Private Function UpdateProductsFromWiddowsonInventoryFile(Widd_Qty, Quarantine, Product_Sku, inventory_date, filename)
    '    'Dim strSQL : strSQL = "UPDATE TOP(1) Products SET Widd_Qty = "& Widd_Qty &",Quarantine = " & Quarantine & " WHERE Product_Sku = '"& Product_Sku &"'"
    '    Dim strSQL : strSQL = "UpdateProductsFromWiddowsonInventoryFile"
    '    'Response.Write strSQL & "<br>"

    '    'Response.Write("Product_Sku="&Product_Sku&"<br>")
    '    'Response.Write("Widd_Qty="&Widd_Qty&"<br>")
    '    'Response.Write("Quarantine="&Quarantine&"<br>")

    '    Dim objCmd :   Set objCmd = Server.CreateObject("ADODB.Command")
    '        With objCmd
    '        .CommandTimeout = 5000
    '        .ActiveConnection = objConn
    '        .CommandType = 4
    '        .Commandtext = strSQL
    '        .Parameters.Append.CreateParameter("@Widd_Qty", 3, 1,, Widd_Qty)
    '        .Parameters.Append.CreateParameter("@Quarantine", 3, 1,, Quarantine)
    '        .Parameters.Append.CreateParameter("@Product_Sku", 200, 1, 50, Product_Sku)
    '        .Parameters.Append.CreateParameter("@inventory_date", 7, 1,, inventory_date)
    '        .Parameters.Append.CreateParameter("@filename", 200, 1, 100, filename)
    '        .Parameters.Append.CreateParameter("@Product_Sku_Exists", 11, 2)
    '        .Execute ,, 128
    '            UpdateProductsFromWiddowsonInventoryFile = CBool(.Parameters("@Product_Sku_Exists"))
    '    End With
    '    DestryObject(objCmd)
    'End Function
    'Private Sub CreateInventoryFileForNPosFromWiddowson(strOrderFileContents)
    '    Dim strCSVFileLocation As String : strCSVFileLocation = IntegrationFolder & NPOS_INVENTORY_FILE_FOLDER
    '    Dim strCSVFileName : strCSVFileName = CreateNPosInventoryFileName()
    '    Dim strCSVFullFilePath : strCSVFullFilePath = strCSVFileLocation & strCSVFileName
    '    Dim objFile :   Set objFile= objFSO.CreateTextFile(strCSVFullFilePath,true)

    '    objFile.Write(Left(strOrderFileContents, Len(strOrderFileContents) - 1))
    '    objFile.Close
    '    DestryObject(objFile)
    '    CopyInventoryFileToNPosFileToArchivedFolder(strCSVFullFilePath)
    '    HttpContext.Current.Response.Write("<P>The file named <b>'" & strCSVFileName & "'</b> was successfully created. and saved to the directory <b>" & strCSVFileLocation & "</b>. <br>The complete path to the file is <b>" & strCSVFullFilePath & "</b></P>")
    'End Sub
    'Private Sub CopyInventoryFileToNPosFileToArchivedFolder(filename As String)
    '    objFSO.CopyFile(filename, IntegrationFolder & NPOS_INVENTORY_FILE_ARCHIVE_FOLDER)
    'End Sub
    'Private Function CreateNPosInventoryFileName() As String
    '    CreateNPosInventoryFileName = NPOS_INVENTORY_FILE_PREFIX & CreateNposUniqueFileNameIdentifier() & NPOS_INVENTORY_FILE_EXTENTION
    '    Return CreateNPosInventoryFileName
    'End Function
    'Private Function CreateNposUniqueFileNameIdentifier()
    '    Dim dDate : dDate = Now()
    '    CreateNposUniqueFileNameIdentifier = Year(dDate) & Right("00" & CStr(Month(dDate)), 2) & Right("00" & CStr(Day(dDate)), 2) & "_" & Right("00" & CStr(Hour(dDate)), 2) & "." & Right("00" & CStr(Minute(dDate)), 2) & "." & Right("00" & CStr(Second(dDate)), 2)
    'End Function
    'Private Sub MoveProcessedFileToArchivedFolder(filename)
    '    objFSO.MoveFile(filename, IntegrationFolder & WIDDISON_FROM_ARCHIVE_FOLDER)
    'End Sub
    'Private Sub DestryObject(ByRef obj As Object)
    '    If Not obj Is Nothing Then
    '        obj = Nothing
    '    End If
    'End Sub
    Shared Function sendtoWH(ftpfolder As String, ftpfilename As String, LocalSourceFolder As String, Optional ftpsite As String = "ftp://ftp2.pointbidplc.com/", Optional ftploginname As String = "ESCAPA01", Optional ftppassword As String = "f8458dLN") As String
        Dim myFtpWebRequest As FtpWebRequest
        Dim myFtpWebResponse As FtpWebResponse
        Dim myStreamWriter As StreamWriter
        Dim mFtpWebResponse As String = ""
        Try
            myFtpWebRequest = WebRequest.Create(ftpsite & ftpfolder & ftpfilename)
            myFtpWebRequest.Credentials = New NetworkCredential(ftploginname, ftppassword)

            myFtpWebRequest.Method = WebRequestMethods.Ftp.UploadFile
            myFtpWebRequest.UseBinary = True ' Response.Write("CLng:150")

            myStreamWriter = New StreamWriter(myFtpWebRequest.GetRequestStream())
            myStreamWriter.Write(New StreamReader(HttpContext.Current.Server.MapPath(LocalSourceFolder & ftpfilename)).ReadToEnd)
            myStreamWriter.Close()
            myFtpWebResponse = myFtpWebRequest.GetResponse()
            'litResponse.Text = "STATUS:" & myFtpWebResponse.StatusDescription ' Label1.Text = "dsdsdsdsdsdsdsdsdsd"
            mFtpWebResponse = myFtpWebResponse.StatusDescription.ToString
            myFtpWebResponse.Close()
            'litResponse.Text = litResponse.Text & "... Complete"

        Catch ex As Exception
            HttpContext.Current.Response.Write("Error:" & ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write("URI:" & ftpsite & ftpfolder & ftpfilename & "<br>")
            HttpContext.Current.Response.Write("File Name:" & ftpfilename & "<br>")
            HttpContext.Current.Response.Write("FtpWebResponse:" & mFtpWebResponse & "<br>")
        End Try
        Return mFtpWebResponse

    End Function
End Class
