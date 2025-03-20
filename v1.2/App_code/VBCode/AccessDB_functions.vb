Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Web.HttpContext



Public Class AccessDB_functions
    Public Shared Function CheckDuplications(ByVal fDBName As String, ByVal fTableName As String, ByVal fSearchField As String, ByRef fSearchValue As String, ByVal fSeekFields As String) As String

        Dim mDbFolder As String = "/App_Data/"
        Dim MDatabseName As String = Current.Application("accessDB_name")

        Dim fReturnData As String = ""

        Return fReturnData

    End Function ' GetSearchData
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="fDBName">database Name with folder like "App_Data/ImsStock.mdb"</param>
    ''' <param name="fTableName">Table carrying the sought value field</param>
    ''' <param name="fSearchField">serach criteria field name</param>
    ''' <param name="fSearchValue">serach criteria field value</param>
    ''' <param name="fSeekFields">firld value to be sought can be like sum([Filed Name])</param>
    ''' <returns>returns value in </returns>
    ''' <remarks></remarks>
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
        If Mid(fSearchField, 1, 1) = "[" Or Mid(fSearchField, 4, 1) = "[" Then
            SqlString = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSearchField & " like '" & fSearchValue & "' ;"
        Else
            SqlString = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE [" & fSearchField & "] like '" & fSearchValue & "' ;"
        End If
        '************************************
        'following lines are for testing
        'fReturnData = Mid(fSearchField, 1, 1) & "KKKK"
        'fReturnData = SqlString
        'HttpContext.Current.Response.Write(fSearchValue)
        'HttpContext.Current.Response.Write(fSearchField)
        ' HttpContext.Current.Response.End()

        ''************************************

        Try
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
            'specuially for flags because data may return with "Not Found"
            If fReturnData = "Not Found" Then
                fReturnData = "NotFound"
            End If
            If fReturnData = "" Or fReturnData Is Nothing Then
                fReturnData = "Not Found"
            End If

            dbConnection.Close()
            ds.Clear()
            da.Dispose()

        Catch ex As Exception
            HttpContext.Current.Response.Write(SqlString & "<br/>")
            HttpContext.Current.Response.Write(ex.Message)
            HttpContext.Current.Response.End()
        End Try


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
        'checks if the required record exists of not

        Dim ReturnValue As String = ""
        Dim MyDataset As DataSet
        'Dim MyTable As DataTable

        Dim mDbFolder As String = "/App_Data/"
        Dim MDatabseName As String = Current.Application("accessDB_name")
        Dim DbFilePAth As String = "~" & mDbFolder & MDatabseName
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'dim SqlString as string =  "SELECT "& mSeekFields & " FROM " & mTableName & " WHERE " & mSearchField & " = "  &mSearchValue

        Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSeekFields & "=" & fSearchCriteria
        MyDataset = New DataSet
        ' Create AccessDataSource
        Dim MyAccessDataSource As New AccessDataSource(DbFilePAth, SqlString)

        Dim rdrSql As DataView = DirectCast(MyAccessDataSource.Select(DataSourceSelectArguments.Empty), DataView)

        For Each drsql As DataRowView In rdrSql

            ReturnValue = drsql(fSeekFields)

        Next

        If ReturnValue Is Nothing Then
            ReturnValue = "Not Found"
        End If

        Return ReturnValue
    End Function


    Public Shared Function NumberOfRows(ByVal fDBName As String, fTableName As String, ByVal fSearchCriteria As String, ByVal fSeekFields As String) As String

        Dim MyConnection As OleDbConnection
        Dim MyCommand As OleDbDataAdapter
        Dim MyDataset As DataSet
        Dim MyTable As DataTable
        Dim numrows As Integer
        '       Dim sqlstr As String

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
    ''' <summary>
    ''' This function updates data of selected records based on the criteria given, updates fields with the new  value
    ''' </summary>
    ''' <param name="UpdateDB">Name of the databse along with the folder path like App_data\IMSledger.mdb</param>
    ''' <param name="UpdateTable">Name of the table to update</param>
    ''' <param name="FieldsAndValuesToUpdate">Must give both field name and Field value to update ensure comas ' around value to avoid "Error in  insert into command"</param>
    ''' <param name="mSeekRecordsWithValues">Must give both field name and Field value for the criteria</param>
    ''' <returns>returns sql string in case of error or Updated if successful</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateTableRec(ByRef UpdateDB As String, ByRef UpdateTable As String, ByRef FieldsAndValuesToUpdate As String, ByRef mSeekRecordsWithValues As String) As String

        Dim mresult As String = "Data Imported"

        Dim SqlString As String = "UPDATE " & UpdateTable & " SET " & FieldsAndValuesToUpdate & " WHERE " & mSeekRecordsWithValues & ";"

        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(UpdateDB))
        Dim mycommand As New OleDbCommand(SqlString, dbConnection)

        'HttpContext.Current.Response.Write(SqlString)
        dbConnection.Open()
        Try
            mycommand.ExecuteNonQuery()
            mresult = SqlString
            dbConnection.Close()

        Catch ex As Exception
            HttpContext.Current.Response.Write(SqlString & "<br/>")
            HttpContext.Current.Response.Write(ex.Message & "<br/>")
            HttpContext.Current.Response.Write("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(UpdateDB))
            HttpContext.Current.Response.End()
        End Try
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
    Public Shared Function RunSQLString(ByRef UpdateDB As String, ByRef fSqlString As String) As String

        ' make sure that field values are limited with these comas ->'<-
        'or the functions will return error that "Error in  insert into command"

        Dim mresult As String = "Data Imported"
        '        "SELECT * FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue

        Dim SqlString As String = fSqlString

        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(UpdateDB))
        Dim mycommand As New OleDbCommand(SqlString, dbConnection)

        Try
            dbConnection.Open()
            mycommand.ExecuteNonQuery()
            mresult = "Data Updated"
            HttpContext.Current.Response.Write(SqlString)
        Catch ex As Exception
            HttpContext.Current.Response.Write(SqlString & "<br/>")
            HttpContext.Current.Response.Write(ex.Message & "<br/>")
            HttpContext.Current.Response.Write(UpdateDB)
            ' HttpContext.Current.Response.End()
            mresult = "Data Not Updated"
        End Try

        'mresult = SqlString
        dbConnection.Close()
        Return mresult
    End Function

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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ImportintoDB"></param>
    ''' <param name="DestinationTable"></param>
    ''' <param name="DestinationFields"></param>
    ''' <param name="SourceDB"></param>
    ''' <param name="SourceFields"></param>
    ''' <param name="SourceTable"></param>
    ''' <param name="SourceCriretia"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Shared Function ImportFromTableToTable(ByRef ImportintoDB As String, ByRef DestinationTable As String, ByRef DestinationFields As String, ByRef SourceDB As String, ByRef SourceFields As String, ByRef SourceTable As String, ByRef SourceCriretia As String) As String
        'to import bulk data from another MS Access datrabse
        'You could use the IN clause in a query that uses a DSN-less connection to quickly read data from the external table. For example:
        Dim mresult As String = "Data Imported"
        Dim SqlString As String = ""
        If SourceCriretia <> "" Then
            SqlString = "INSERT INTO " & DestinationTable & "  (" & DestinationFields & ")  SELECT " & SourceFields & " FROM " & SourceTable & " IN ''  [MS Access;DATABASE=" & Current.Server.MapPath(SourceDB) & "]" & " WHERE " & SourceCriretia & ";"
        Else
            SqlString = "INSERT INTO " & DestinationTable & "  (" & DestinationFields & ")  SELECT " & SourceFields & " FROM " & SourceTable & " IN ''  [MS Access;DATABASE=" & Current.Server.MapPath(SourceDB) & "];"
        End If
        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(ImportintoDB))
        Dim Mycommand As New OleDbCommand(SqlString, dbConnection)
        dbConnection.Open()
        ' HttpContext.Current.Response.Write(SqlString)
        ' HttpContext.Current.Response.End()
        Try
            Mycommand.ExecuteNonQuery()
            dbConnection.Close()

        Catch ex As Exception
            HttpContext.Current.Response.Write(SqlString & "<br/>")
            HttpContext.Current.Response.Write(ex.Message)
            HttpContext.Current.Response.End()
        End Try

        Return mresult
    End Function

    ''' <summary>
    ''' use words as Min, Max , Avg, count
    ''' </summary>
    ''' <param name="fDBName"></param>
    ''' <param name="fTableName"></param>
    ''' <param name="fSearchField"></param>
    ''' <param name="fSearchMinOrMax"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getMinMaxvalue(ByVal fDBName As String, ByVal fTableName As String, ByRef fSearchField As String, ByRef fSearchMinOrMax As String) As String
        Dim connect As [String] = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(fDBName))
        Dim dbConnection As New OleDbConnection(connect)
        Dim fReturnData As String = "nothing"
        Dim ds As New DataSet
        Dim Myvalue As String = "'" & fSearchMinOrMax & fSearchField & "'"
        dbConnection.Open()
        Dim SqlString As String = "SELECT " & fSearchMinOrMax & "(" & fSearchField & ") as " & Myvalue & "FROM " & fTableName
        Dim command As New OleDbCommand(SqlString, dbConnection)

        Dim da As New OleDbDataAdapter(command)
        'da.Fill(ds)
        '*******************
        Try
            da.Fill(ds)
            Console.WriteLine("Made the connection to the database")
            For Each dr As DataRow In ds.Tables(0).Rows
                fReturnData = dr(Myvalue)
            Next

        Catch ex As Exception
            HttpContext.Current.Response.Write(SqlString & "<br/>")
            HttpContext.Current.Response.Write(ex.Message)
            ' HttpContext.Current.Response.End()
        End Try

        'Console.WriteLine("Made the connection to the database")
        'For Each dr As DataRow In ds.Tables(0).Rows
        '    fReturnData = dr(Myvalue)
        'Next
        'Dim command As OleDbCommand = con.CreateCommand()

        ' fReturnData = (command.CommandText = "SELECT " & fSearchMinOrMax & "(" & fSearchField & ") FROM " & fTableName)
        Console.WriteLine("Max/min: {0:D}")
        dbConnection.Close()
        Return fReturnData
    End Function
    ''' <summary>
    ''' use words as Min, Max , Avg, count
    ''' </summary>
    ''' <param name="fDBName"></param>
    ''' <param name="fTableName"></param>
    ''' <param name="fSearchField"></param>
    ''' <param name="fSearchMinOrMax">use words as Min, Max , Avg, count</param>
    ''' <param name="fSearchFieldName">Search fIELD nAME</param>
    ''' <param name="fSearchFieldVale"> make sure the value is put into single comas like this 'SASHAS'</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMinMaxValueWithCriteria(ByVal fDBName As String, ByVal fTableName As String, ByRef fSearchField As String, ByRef fSearchMinOrMax As String, ByRef fSearchFieldName As String, ByRef fSearchFieldVale As String) As String
        Dim connect As [String] = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(fDBName))
        Dim dbConnection As New OleDbConnection(connect)
        Dim fReturnData As String = "nothing"
        Dim ds As New DataSet
        Dim Myvalue As String = "'" & fSearchMinOrMax & fSearchField & "'"
        dbConnection.Open()
        Dim SqlString As String = "SELECT " & fSearchMinOrMax & "(" & fSearchField & ") as " & Myvalue & " FROM " & fTableName & " Where " & fSearchFieldName & " like '" & fSearchFieldVale & "'"
        Dim command As New OleDbCommand(SqlString, dbConnection)

        Dim da As New OleDbDataAdapter(command)
        'da.Fill(ds)
        '*******************
        Try
            da.Fill(ds)
            Console.WriteLine("Made the connection to the database")
            For Each dr As DataRow In ds.Tables(0).Rows
                fReturnData = dr(Myvalue)
            Next

        Catch ex As Exception
            HttpContext.Current.Response.Write(SqlString & "<br/>")
            HttpContext.Current.Response.Write(ex.Message)
            ' HttpContext.Current.Response.End()
        End Try

        'Console.WriteLine("Made the connection to the database")
        'For Each dr As DataRow In ds.Tables(0).Rows
        '    fReturnData = dr(Myvalue)
        'Next
        'Dim command As OleDbCommand = con.CreateCommand()

        ' fReturnData = (command.CommandText = "SELECT " & fSearchMinOrMax & "(" & fSearchField & ") FROM " & fTableName)
        Console.WriteLine("Max/min: {0:D}")
        dbConnection.Close()
        Return fReturnData
    End Function
    Public Shared Function GetAutoIncrementValue(ByVal fDBName As String, ByVal fTableName As String, ByVal fkeyfield As String) As String
        Dim mDbFolder As String = "App_Data\"
        Dim MDatabseName As String = Current.Application("accessDB_name")
        Dim mdbwithpath As String = Current.Server.MapPath(mDbFolder & fDBName)
        Dim DbFilePAth As String = "~" & mDbFolder & MDatabseName

        'HttpContext.Current.Response.Write(mdbwithpath)
        'HttpContext.Current.Response.End()

        Dim SqlString As String = "Select user_id from " & fTableName
        ' Dim SqlString As String = "SELECT Max(ResponsesTemplates.response_Template_id) AS MaxOfresponse_Template_id FROM " & fTableName
        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & mdbwithpath)
        dbConnection.Open()
        Dim command As New OleDbCommand(SqlString, dbConnection)
        'HttpContext.Current.Response.Write(SqlString)
        'HttpContext.Current.Response.End()
        Dim newid As Integer = command.ExecuteNonQuery
        Return newid
    End Function ' GetSearchData

    Public Shared Sub DeleteRecord(ByVal fDBName As String, ByVal fTableName As String, ByVal fSearchField As String, ByVal fSearchValue As Integer)

        Dim mDbFolder As String = "/App_Data/"
        Dim MDatabseName As String = Current.Application("accessDB_name")

        Dim DbFilePAth As String = "~" & mDbFolder & MDatabseName
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        Dim SqlString As String = "DELETE  FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue

        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(fDBName))
        dbConnection.Open()
        Dim command As New OleDbCommand(SqlString, dbConnection)
        command.ExecuteNonQuery()
        'HttpContext.Current.Response.Write(SqlString)
        'HttpContext.Current.Response.End()
        dbConnection.Close()

    End Sub ' GetSearchData

    Public Shared Function ChangeToAccessDate(ByVal fDateToChange As String) As String
        Dim mDateReturn As String
        mDateReturn = "#" & fDateToChange & "#"
        Return mDateReturn
    End Function

    Shared Function GetSearchDataString(p1 As String, p2 As String, p3 As String, mNewPeriod As String) As String
        Throw New NotImplementedException
    End Function

End Class
