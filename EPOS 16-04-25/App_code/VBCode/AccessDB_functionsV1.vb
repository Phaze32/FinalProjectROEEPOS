Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Web.HttpContext



Public Class AccessDB_functionsV1
    Public Shared Function CheckDuplications(ByVal fDBName As String, ByVal fTableName As String, ByVal fSearchField As String, ByRef fSearchValue As String, ByVal fSeekFields As String) As String

        Dim mDbFolder As String = "/App_Data/"
        Dim MDatabseName As String = Current.Application("accessDB_name")

        Dim fReturnData As String = ""



        Return fReturnData

    End Function ' GetSearchData
    Public Shared Function GetSearchDataString(ByVal fDBName As String, ByVal fTableName As String, ByVal fSearchField As String, ByRef fSearchValue As String, ByVal fSeekFields As String) As String

        Dim mDbFolder As String = "/App_Data/"
        Dim MDatabseName As String = Current.Application("accessDB_name")

        Dim fReturnData As String = ""
        Dim ds As New DataSet
        Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSearchField & " like '" & fSearchValue & "';"

        Dim DbFilePAth As String = "~" & mDbFolder & MDatabseName
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        fSearchField = Trim(Replace(Replace(fSearchField, "[[", "["), "]]", "]"))
        If Mid(fSearchField, 1, 1) = "[" Then
            SqlString = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSearchField & " like '" & fSearchValue & "' ;"
        Else
            SqlString = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE [" & fSearchField & "] like '" & fSearchValue & "' ;"
        End If
        '************************************
        'following lines are for testing
        'fReturnData = Mid(fSearchField, 1, 1) & "KKKK"
        fReturnData = SqlString
        'HttpContext.Current.Response.Write(fSearchValue)
        'HttpContext.Current.Response.Write(fSearchField)
        ' HttpContext.Current.Response.End()

        ''************************************

        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(fDBName))
        dbConnection.Open()
        Dim command As New OleDbCommand(SqlString, dbConnection)
        'HttpContext.Current.Response.Write(SqlString)
        'HttpContext.Current.Response.End()
        Dim da As New OleDbDataAdapter(command)
        da.Fill(ds)

        For Each dr As DataRow In ds.Tables(0).Rows
            fReturnData = dr(fSeekFields)
        Next

        If fReturnData = "" Or fReturnData Is Nothing Then
            fReturnData = "Not Found"
        End If

        dbConnection.Close()
        ds.Clear()
        da.Dispose()

        Return fReturnData

    End Function ' GetSearchData
    Public Shared Function GetSearchData(ByVal fDBName As String, ByVal fTableName As String, ByVal fSearchField As String, ByRef fSearchValue As Integer, ByVal fSeekFields As String) As String

        Dim mDbFolder As String = "/App_Data/"
        Dim MDatabseName As String = Current.Application("accessDB_name")

        Dim fReturnData As String = ""
        Dim ds As New DataSet

        Dim DbFilePAth As String = "~" & mDbFolder & MDatabseName
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        Dim SqlString As String = "SELECT * FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue

        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(fDBName))
        dbConnection.Open()
        Dim command As New OleDbCommand(SqlString, dbConnection)

        Dim da As New OleDbDataAdapter(command)
        da.Fill(ds)

        For Each dr As DataRow In ds.Tables(0).Rows
            fReturnData = dr(fSeekFields)
        Next

        If fReturnData = "" Then
            fReturnData = "Not Found"
        End If

        dbConnection.Close()
        ds.Clear()
        da.Dispose()

        Return fReturnData

    End Function ' GetSearchData

    Public Shared Function GetAccessDb(ByVal fTableName As String, ByVal fSearchCriteria As String, ByVal fSeekFields As String) As AccessDataSource

        Dim mDbFolder As String = "/App_Data/IMSStock.mdb"
        Dim MDatabseName As String = Current.Application("accessDB_name")

        Dim DbFilePAth As String = "~" & mDbFolder ' & MDatabseName
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'dim SqlString as string =  "SELECT "& mSeekFields & " FROM " & mTableName & " WHERE " & mSearchField & " = "  &mSearchValue

        Dim SqlString As String = "SELECT " & SafeSqlLiteral(fSeekFields, 2) & " FROM " & SafeSqlLiteral(fTableName, 2) & " WHERE " & SafeSqlLiteral(fSearchCriteria, 2)

        ' Create AccessDataSource
        Dim accessDS As New AccessDataSource(DbFilePAth, SqlString)

        Return accessDS
    End Function

    Public Shared Function SafeSqlLiteral(ByVal strValue As String, ByVal intLevel As Integer) As String

        ' intLevel represent how thorough the value will be checked for dangerous code
        ' intLevel (1) - Do just the basic. This level will already counter most of the SQL injection attacks
        ' intLevel (2) - &nbsp; (non breaking space) will be added to most words used in SQL queries to prevent unauthorized access to the database. Safe to be printed back into HTML code. Don't use for usernames or passwords

        If Not IsDBNull(strValue) Then
            If intLevel > 0 Then
                strValue = Replace(strValue, "'", "''") ' Most important one! This line alone can prevent most injection attacks
                strValue = Replace(strValue, "--", "")
                strValue = Replace(strValue, "[", "[[]")
                strValue = Replace(strValue, "%", "[%]")
            End If

            If intLevel > 1 Then
                Dim myArray As Array
                myArray = Split("xp_ ;update ;insert ;select ;drop ;alter ;create ;rename ;delete ;replace ", ";")
                Dim i, i2, intLenghtLeft As Integer
                For i = LBound(myArray) To UBound(myArray)
                    Dim rx As New Regex(myArray(i), RegexOptions.Compiled Or RegexOptions.IgnoreCase)
                    Dim matches As MatchCollection = rx.Matches(strValue)
                    i2 = 0
                    For Each match As Match In matches
                        Dim groups As GroupCollection = match.Groups
                        intLenghtLeft = groups.Item(0).Index + Len(myArray(i)) + i2
                        strValue = Left(strValue, intLenghtLeft - 1) & "&nbsp;" & Right(strValue, Len(strValue) - intLenghtLeft)
                        i2 += 5
                    Next
                Next
            End If

            'strValue = replace(strValue, ";", ";&nbsp;")
            'strValue = replace(strValue, "_", "[_]")

            Return strValue
        Else
            Return strValue
        End If

    End Function

    Public Shared Function importExcell(ByRef ImportintoDB As String, ByRef importintoTable As String, ByRef fileToImport As String) As String
        Dim mresult As String = "Data Imported"
        '        "SELECT * FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        Dim SqlString As String = "INSERT INTO" & importintoTable & " SELECT * FROM OPENROWSET('Microsoft.Jet.OLEDB.4.0', 'Excel 8.0;Database='" & fileToImport & "', 'SELECT * FROM [Sheet1$]')"
        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(ImportintoDB))
        Dim command As New OleDbCommand(SqlString, dbConnection)
        Return mresult
    End Function

    Public Shared Function CheckIfExists(ByVal fTableName As String, ByVal fSearchCriteria As String, ByVal fSeekFields As String) As String
        Dim MyDataset As DataSet
        '        Dim MyTable As DataTable

        Dim mDbFolder As String = "/App_Data/"
        Dim MDatabseName As String = Current.Application("accessDB_name")
        Dim DbFilePAth As String = "~" & mDbFolder & MDatabseName
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'dim SqlString as string =  "SELECT "& mSeekFields & " FROM " & mTableName & " WHERE " & mSearchField & " = "  &mSearchValue

        Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSeekFields & "=" & fSearchCriteria
        MyDataset = New DataSet
        ' Create AccessDataSource
        Dim MyAccessDataSource As New AccessDataSource(DbFilePAth, SqlString)

        Dim rdrSql As OleDbDataReader = DirectCast(MyAccessDataSource.Select(DataSourceSelectArguments.Empty), OleDbDataReader)

        Dim rdrSql1 As String = ""

        Return rdrSql1 IsNot Nothing

    End Function


    Public Shared Function NumberOfRows(ByVal fDBName As String, fTableName As String, ByVal fSearchCriteria As String, ByVal fSeekFields As String) As String


        Dim MyConnection As OleDbConnection
        Dim MyCommand As OleDbDataAdapter
        Dim MyDataset As DataSet
        Dim MyTable As DataTable
        Dim numrows As Integer
        '        Dim sqlstr As String

        Dim mDbFolder As String = "App_Data/"
        Dim MDatabseName As String = Current.Application("accessDB_name")
        Dim DbFilePAth As String = "~" & mDbFolder & MDatabseName
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'dim SqlString as string =  "SELECT "& mSeekFields & " FROM " & mTableName & " WHERE " & mSearchField & " = "  &mSearchValue

        Dim SqlString As String = "SELECT * FROM " & fTableName & " WHERE " & fSeekFields & "= '" & fSearchCriteria & "';"



        ' Create a connection to the data source. 
        MyConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(mDbFolder & fDBName))

        ' Create a Command object with the SQL statement.
        MyCommand = New OleDbDataAdapter(SqlString, MyConnection)

        ' Fill a DataSet with data returned from the database.
        MyDataset = New DataSet
        MyCommand.Fill(MyDataset)

        ' Create a new DataTable object and assign to it
        ' the new table in the Tables collection.
        MyTable = New DataTable
        MyTable = MyDataset.Tables(0)
        ' Find how many rows are in the Rows collection 
        ' of the new DataTable object.
        numrows = MyTable.Rows.Count

        Return numrows
    End Function

    Public Shared Function getTableFieldList(ByVal fDBName As String, fTableName As String) As String
        Dim strSQL As String
        Dim i As Integer
        Dim mDbFolder As String = "App_Data/"
        Dim TableFiledlist As String = ""
        ' Create a connection to the data source. 
        Dim MyConnection As OleDbConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(mDbFolder & fDBName))
        MyConnection.Open()
        strSQL = "SELECT * FROM " & fTableName
        ' Create a Command object with the SQL statement.

        Dim dtReader As OleDbDataReader
        Dim objCmd As New OleDbCommand(strSQL, MyConnection)
        dtReader = objCmd.ExecuteReader()

        TableFiledlist = "<b>Table customer have " & dtReader.FieldCount & " Fields</b>"

        For i = 0 To dtReader.FieldCount - 1
            TableFiledlist = TableFiledlist + "<br>" & dtReader.GetName(i)
        Next
        dtReader.Close()
        dtReader = Nothing
        Return TableFiledlist
    End Function
    Public Shared Function CheckTableFieldName(ByVal fDBName As String, fTableName As String, fFieldName As String) As String
        Dim strSQL As String
        Dim i As Integer
        Dim mDbFolder As String = "App_Data/"
        Dim FiledName As String = ""
        ' Create a connection to the data source. 
        Dim MyConnection As OleDbConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(mDbFolder & fDBName))
        MyConnection.Open()
        strSQL = "SELECT * FROM " & fTableName
        ' Create a Command object with the SQL statement.

        Dim dtReader As OleDbDataReader
        Dim objCmd As New OleDbCommand(strSQL, MyConnection)
        dtReader = objCmd.ExecuteReader()

        FiledName = "<b>Table customer have " & dtReader.FieldCount & " Fields</b>"

        For i = 0 To dtReader.FieldCount - 1
            'following line is for testing
            '  HttpContext.Current.Response.Write(Trim(dtReader.GetName(i)) & "<-->" & Trim(fFieldName) & "<br/>")
            If Trim(dtReader.GetName(i)) Like Trim(fFieldName) Then
                FiledName = dtReader.GetName(i)
                Exit For
            Else
                FiledName = fFieldName & " Not found"
            End If
        Next
        MyConnection.Close()
        dtReader.Close()
        dtReader = Nothing
        Return FiledName
    End Function
    Public Shared Function AddRecToTable(ByRef ImportintoDB As String, ByRef importintoTable As String, ByRef FieldsToInsert As String, ByRef FieldsValuesToInsert As String) As String
        ' make sure that field values are limited with these comas ->'<-
        'or the functions will return error that Error in the "insert into command"

        Dim mresult As String = "Data Imported"
        '        "SELECT * FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        Dim SqlString As String = "INSERT INTO " & importintoTable _
                                  & FieldsToInsert & " Values  " & FieldsValuesToInsert & " "
        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(ImportintoDB))
        Dim mycommand As New OleDbCommand(SqlString, dbConnection)

        'HttpContext.Current.Response.Write(SqlString)
        dbConnection.Open()
        mresult = mycommand.ExecuteNonQuery()
        dbConnection.Close()
        Return mresult

    End Function
    Public Shared Function UpdateTableRec(ByRef UpdateDB As String, ByRef UpdateTable As String, ByRef FieldsAndValuesToUpdate As String, ByRef mSeekRecordsWithValues As String) As String
        ' make sure that field values are limited with these comas ->'<-
        'or the functions will return error that "Error in  insert into command"

        Dim mresult As String = "Data Imported"

        Dim SqlString As String = "UPDATE " & UpdateTable & " SET " & FieldsAndValuesToUpdate & " WHERE " & mSeekRecordsWithValues & ";"

        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(UpdateDB))
        Dim mycommand As New OleDbCommand(SqlString, dbConnection)

        'HttpContext.Current.Response.Write(SqlString)
        dbConnection.Open()
        mycommand.ExecuteNonQuery()
        mresult = SqlString
        dbConnection.Close()
        Return mresult

    End Function
    Public Shared Function GetDBTablesListByConn(ByRef conn As System.Data.OleDb.OleDbConnection) As DataTable

        conn.Open()
        Dim schemaTable As DataTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, "TABLE"})
        conn.Close()
        Return schemaTable
    End Function
    Public Shared Function GetDBTablesListByDB(ByRef fDBName As String, ByRef fType As String) As String
        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(fDBName))
        dbConnection.Open()

        Dim userTables As DataTable = Nothing

        ' We only want user tables, not system tables
        Dim mType As String
        If fType <> "" Then
            mType = fType
        End If



        ' Get list of user tables
        userTables = dbConnection.GetSchema("Tables", New String() {Nothing, Nothing, Nothing, mType})


        ' Add list of table names to listBox
        Dim DBTableList As String = "test"
        Dim i As Integer
        For i = 0 To userTables.Rows.Count - 1 Step i + 1
            System.Console.WriteLine(userTables.Rows(i)(2).ToString())
            DBTableList = DBTableList + userTables.Rows(i)(2).ToString() & "<br>"
        Next
        Dim schemaTable As String = DBTableList
        dbConnection.Close()

        Return schemaTable
    End Function
    Public Shared Function GetDBViewsListByDB(ByRef fDBName As String, ByRef fType As String) As String
        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(fDBName))
        dbConnection.Open()

        Dim userTables As DataTable = Nothing

        ' We only want user tables, not system tables
        Dim mType As String
        If fType <> "" Then
            mType = fType
        End If
        Dim objArrRestrict = New Object() {Nothing, Nothing, Nothing, "TABLE"}
        '*****************
        ' Get list of user tables
        'userTables = dbConnection.GetOleDbSchemaTable(adSchemaViews, New String() {Nothing, Nothing, "Procedures"})
        '  Select MSysObjects.Name FROM MSysObjects 'WHERE type = 5
        userTables = dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, objArrRestrict)
        '*****************



        ' Add list of table names to listBox
        Dim DBTableList As String = "test"
        Dim i As Integer
        For i = 0 To userTables.Rows.Count - 1 Step i + 1
            System.Console.WriteLine(userTables.Rows(i)(2).ToString())
            DBTableList = DBTableList + userTables.Rows(i)(2).ToString() & "<br>"
        Next
        Dim schemaTable As String = DBTableList
        dbConnection.Close()

        Return schemaTable
    End Function
    Public Shared Function MyReplace(ByVal ValueIn As String, ByVal WhatToReplace As String, ByVal ReplaceValue As String) As String
        Dim Temp As String
        Dim P As Long
        Dim fReplace As String = ""

        Temp = ValueIn
        P = InStr(Temp, WhatToReplace)
        Do While P > 0
            Temp = Left(Temp, P - 1) & ReplaceValue & Mid(Temp, P + Len(WhatToReplace))
            P = InStr(P + Len(ReplaceValue), Temp, WhatToReplace, 1)
        Loop
        fReplace = Temp
        Return fReplace
    End Function

    Public Shared Function ReplaceRecords(ByVal fDBName As String, ByVal fTableName As String, ByVal fSearchField As String, ByRef fSearchValue As Integer, ByVal fSeekFields As String, ByRef fReplaceValue As String) As String

        Dim mDbFolder As String = "/App_Data/"
        Dim MDatabseName As String = Current.Application("accessDB_name")

        Dim fReturnData As String = ""
        Dim ds As New DataSet

        Dim DbFilePAth As String = "~" & mDbFolder & MDatabseName
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        Dim SqlString As String = "SELECT * FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue

        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(fDBName))
        dbConnection.Open()
        Dim command As New OleDbCommand(SqlString, dbConnection)

        Dim da As New OleDbDataAdapter(command)
        da.Fill(ds)

        For Each dr As DataRow In ds.Tables(0).Rows
            fReturnData = Left(dr(fSeekFields), 2)
            'UpdateTableRec()
        Next

        If fReturnData = "" Then
            fReturnData = "Not Found"
        End If

        dbConnection.Close()
        ds.Clear()
        da.Dispose()

        Return fReturnData

    End Function ' GetSearchData

    Public Shared Function SaveTotxtFile(ByRef fStoreFile As String, ByRef fDataTOSave As String) As String
        Dim DataSaved As String = ""

        Return DataSaved
    End Function
    'Public Shared Function RunSQLString(ByRef UpdateDB As String, ByRef UpdateTable As String, ByRef fSqlString As String) As String

    '    ' make sure that field values are limited with these comas ->'<-
    '    'or the functions will return error that "Error in  insert into command"

    '    Dim mresult As String = "Data Imported"
    '    '        "SELECT * FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue

    '    Dim SqlString As String = fSqlString

    '    Dim dbConnection As New OleDbConnection
    '    dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(UpdateDB))
    '    Dim mycommand As New OleDbCommand(SqlString, dbConnection)

    '    'HttpContext.Current.Response.Write(SqlString)
    '    dbConnection.Open()
    '    mresult = mycommand.ExecuteNonQuery()
    '    'mresult = SqlString
    '    dbConnection.Close()
    '    Return mresult
    'End Function

    'Sub UpdateRecordsByLine()

    '    Dim adpData As OleDbDataAdapter
    '    Dim dsLineItems As DataSet
    '    Dim MyTable As DataTable
    '    Dim strSQL As String
    '    'Dim loop2, fieldbumbers As Integer
    '    Dim loop1, numrows As Integer
    '    Dim connectionExcell As OleDbConnection
    '    Dim mPath As String = Session("DBPath")
    '    Dim filename As String = Session("sfilename")

    '    If filename = "" Then
    '        'Label3.Text = "PLease Upload a new file before Processing"
    '        Exit Sub
    '    End If

    '    connectionExcell = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + mPath + "';Extended Properties=Excel 12.0;")

    '    strSQL = "select * from [Sheet1$]"

    '    ' Create a Command object with the SQL statement.
    '    adpData = New OleDbDataAdapter(strSQL, connectionExcell)

    '    ' Create a new dataset
    '    dsLineItems = New DataSet

    '    ' Fill a DataSet with data returned from the Excel Sheet1.
    '    adpData.Fill(dsLineItems, "[Sheet1$]")

    '    ' Create a new DataTable object and assign to it, the new table in the Tables collection.
    '    MyTable = New DataTable
    '    MyTable = dsLineItems.Tables(0)

    '    ' Total Charges

    '    Dim ExcelSheetType = MyTable.Columns(0).ColumnName


    '    ' Find how many rows are in the Rows collection 
    '    ' of the new DataTable object.
    '    numrows = MyTable.Rows.Count
    '    If numrows = 0 Then
    '        'Label2.Text = "No records found"
    '    Else
    '        Dim recordcounted As Integer = CStr(numrows) - 1 & " records Procesed from sheet1 of " & filename

    '        '  Response.Write("<p>" & CStr(numrows) & " records found.</p>")
    '        For loop1 = 0 To numrows - 1

    '            ' checks if the line with totals is not added to the Database 
    '            If MyTable.Rows(loop1).Item("Companyid") = "" Or MyTable.Rows(loop1).Item("Companyid") Is Nothing Then
    '                Exit Sub
    '                'LocateName(
    '            End If
    '            '*******************
    '            If MyTable.Rows(loop1).Item("Companyid") <> "total" Or MyTable.Rows(loop1).Item("Companyid") <> "" Then

    '                Dim mCallsSell As Int32 = MyTable.Rows(loop1).Item("Calls Sell")
    '                Dim mCallsBuy As Int32 = MyTable.Rows(loop1).Item("Calls Buy")
    '                Dim mServicesSell As Int32 = MyTable.Rows(loop1).Item("Services Sell")
    '                Dim mServicesBuy As Int32 = MyTable.Rows(loop1).Item("Services Buy")
    '                Dim mInvAmount As Int32 = MyTable.Rows(loop1).Item("Inv Amount")
    '                Dim mfAMOUNTPAID As Int32 = MyTable.Rows(loop1).Item("AMOUNT PAID")
    '                Dim mProfitLossOnSale As Int32 = MyTable.Rows(loop1).Item("Profit Loss on Sale")
    '                Dim mComanayID As String = MyTable.Rows(loop1).Item("Companyid")
    '                Dim mCompanyName As String = MyTable.Rows(loop1).Item("Company Name")
    '                ' incase the customer code is missing the system looks up into the customer record to locate the matching customer name
    '                ' incase its not found then the "Not Found" code is assigned to fix the data issue manually

    '                If IsDBNull(MyTable.Rows(loop1).Item("Company Name")) Then
    '                    Exit For
    '                Else
    '                    mCompanyName = MyTable.Rows(loop1).Item("Company Name")
    '                    mCompanyName.Replace("'", "")
    '                End If

    '                Dim mPeriod As String = Session("sfilename")
    '                Dim mReferenceFile As String = Session("sfilename")
    '                Dim mAccountManagerRef As String = "Not Assigned"
    '                ' Every cclick on the button adds all records from excell file to the database.

    '                If mCompanyName Is Nothing Or mComanayID Is Nothing Or mPeriod Is Nothing Then ' to check if the blank data is not processed for upload

    '                Else
    '                    UpdateTableRec(mComanayID, mCompanyName, mCallsSell, mCallsBuy)

    '                    '***************
    '                End If
    '            End If
    '        Next loop1
    '    End If

    '    '***** Adds File name to the table CommsFilesToTable
    '    Dim mFieldsValuesToInsert As String = "('" & Session("sfilename") & "')"
    '    Dim mFieldsToInsert As String = "([DataFIleName])"

    '    AccessDB_functions.AddRecToTable("App_Data/IMSStock.mdb", "CommsFileToPeriod", mFieldsToInsert, mFieldsValuesToInsert)
    '    '**************************************
    '    connectionExcell.Close()
    'End Sub

    Public Shared Function RoundToNearest(Amount As Double, RoundTo As Double) As Double
        Dim ExcessAmount As Double = Amount Mod RoundTo
        If ExcessAmount < (RoundTo / 2) Then
            Amount -= ExcessAmount
        Else
            Amount += (RoundTo - ExcessAmount)
        End If

        Return Amount
    End Function

    Public Shared Function RoundToNext(Amount As Double, RoundTo As Double) As Double
        Dim ExcessAmount As Double = Amount Mod RoundTo
        Amount += (RoundTo - ExcessAmount)
        Return Amount
    End Function
End Class
