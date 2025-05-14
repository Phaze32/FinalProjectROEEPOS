Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Web.HttpContext
Imports System.Text
Imports System.Collections.Generic
Imports System.Linq
Imports System.Reflection
Imports System.Configuration
Public Class SQLFunctions
    Shared linenumber As Integer = 0
    Dim sharedConnectionString As String = "Escapade_NewConnectionString"
    'Public Sub BulkLoadContent(dt As DataTable, Optional SQLconstring As String = "Escapade_NewConnectionString")
    '    'OnMessage("Bulk loading records to temp table")
    '    'OnSubMessage("Bulk Load Started")
    '    Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString)
    '    Using bcp As New SqlBulkCopy(conn)
    '        bcp.DestinationTableName = "dbo.tempDispositions"
    '        bcp.BulkCopyTimeout = 0
    '        For Each dc As DataColumn In dt.Columns
    '            bcp.ColumnMappings.Add(dc.ColumnName, dc.ColumnName)
    '        Next
    '        bcp.NotifyAfter = 2000
    '        bcp.SqlRowsCopied += New SqlRowsCopiedEventHandler(bcp_SqlRowsCopied)
    '        bcp.WriteToServer(dt)
    '        bcp.Close()
    '    End Using
    'End Sub

    ''' <summary>
    ''' Tested ok 
    ''' </summary>
    ''' <param name="MyDataTable">Data Table use GetDataTablefromSQLDatasource() to generate Datatable</param>
    ''' <param name="tableToSaveTo">name of the table as string</param>
    ''' <remarks>copies bulk of data from one table to another</remarks>
    Public Shared Sub SaveDTtoSqlBulk(MyDataTable As DataTable, tableToSaveTo As String, Optional SQLconstring As String = "Escapade_NewConnectionString")
        ' HttpContext.Current.Response.Write("SaveDTtoSqlBulk Error: <hr>")
        Try
            Using cn As New SqlConnection(ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString)
                cn.Open()
                Using bulkcopy01 As New SqlBulkCopy(cn) : linenumber = 14
                    bulkcopy01.BatchSize = 2000
                    AddHandler bulkcopy01.SqlRowsCopied, AddressOf SQLFunctions.OnSqlRowsCopied
                    bulkcopy01.NotifyAfter = 2000
                    bulkcopy01.DestinationTableName = tableToSaveTo : linenumber = 15
                    bulkcopy01.WriteToServer(MyDataTable) : linenumber = 16
                    MyDataTable.Rows.Count.ToString() : linenumber = 17
                End Using
            End Using
        Catch se As SqlException
            HttpContext.Current.Response.Write("SaveDTtoSqlBulk.SqlException Error: <hr>")
            HttpContext.Current.Response.Write("DT rows =" & se.Message.ToString() & "<br>")
        Catch ex As Exception
            HttpContext.Current.Response.Write("SaveDTtoSqlBulk Error: <hr>")
            HttpContext.Current.Response.Write("DT rows =" & MyDataTable.Rows.Count.ToString() & "<br>")
            HttpContext.Current.Response.Write("DT Name=" & MyDataTable.TableName.ToString & "<br>")
            HttpContext.Current.Response.Write("Exception Message=" & ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write("tableToSaveTo=" & tableToSaveTo & "<br>")
            HttpContext.Current.Response.Write("Last linenumber executed =" & linenumber & "<br>")
        End Try
    End Sub
    Public Shared Sub SaveDTtoSqlBulk2(MyDataTable As DataTable, tableToSaveTo As String, Optional SQLconstring As String = "Escapade_NewConnectionString")
        HttpContext.Current.Response.Write("SaveDTtoSqlBulk Error: <hr>")
        Try
            Using cn As New SqlConnection(ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString)
                cn.Open()
                Using bulkcopy01 As New SqlBulkCopy(cn) : linenumber = 14
                    bulkcopy01.BatchSize = 2000
                    AddHandler bulkcopy01.SqlRowsCopied, AddressOf SQLFunctions.OnSqlRowsCopied
                    bulkcopy01.NotifyAfter = 2000
                    bulkcopy01.DestinationTableName = tableToSaveTo : linenumber = 15
                    bulkcopy01.WriteToServer(MyDataTable) : linenumber = 16
                    MyDataTable.Rows.Count.ToString() : linenumber = 17
                End Using
            End Using
        Catch ex As Exception
            HttpContext.Current.Response.Write("SaveDTtoSqlBulk Error: <hr>")
            HttpContext.Current.Response.Write("DT rows =" & MyDataTable.Rows.Count.ToString() & "<br>")
            HttpContext.Current.Response.Write("DT Name=" & MyDataTable.TableName.ToString & "<br>")
            HttpContext.Current.Response.Write("Exception Message=" & ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write("tableToSaveTo=" & tableToSaveTo & "<br>")
            HttpContext.Current.Response.Write("Last linenumber executed =" & linenumber & "<br>")
        End Try
    End Sub
    Public Shared Sub CopyBulkDataWithKeepIdentity(TableToSaveTo As String, SourceConnectionstrName As String, DestinationSQLconstringName As String, SourceSQLStr As String)
        'HttpContext.Current.Response.Write("SaveDTtoSqlBulk Error: <hr>")


        Dim sourceConString As String = ConfigurationManager.ConnectionStrings(SourceConnectionstrName).ConnectionString

        ' Open a connection to the Source database. 
        Using sourceConnection As SqlConnection = New SqlConnection(sourceConString)
            sourceConnection.Open()
            Dim destinationConstring As String = ConfigurationManager.ConnectionStrings(DestinationSQLconstringName).ConnectionString
            Dim dectinationConnection As New SqlConnection(destinationConstring)

            ' Perform an initial count on the destination table. 
            Dim commandRowCount As New SqlCommand("SELECT COUNT(*) FROM " & TableToSaveTo & ";", sourceConnection)
            Dim countStart As Long = System.Convert.ToInt32(commandRowCount.ExecuteScalar())
            'Console.WriteLine("Starting row count = {0}", countStart)

            ' Get data from the source table as a SqlDataReader.
            'Try
            Dim commandSourceData As SqlCommand = New SqlCommand(SourceSQLStr, sourceConnection)
            Dim reader As SqlDataReader = commandSourceData.ExecuteReader
            ' Create the SqlBulkCopy object using a connection string and the KeepIdentity option.  
            Using bulkCopy As SqlBulkCopy = New SqlBulkCopy(destinationConstring, SqlBulkCopyOptions.KeepIdentity)
                bulkCopy.DestinationTableName = TableToSaveTo

                Try
                    ' Write from the source to the destination.
                    bulkCopy.WriteToServer(reader)
                Catch ex1 As SqlException
                    HttpContext.Current.Response.Write("SqlException ex.Message=" & ex1.Message & "<br>")
                Catch ex As Exception
                    HttpContext.Current.Response.Write(" ex.Message=" & ex.Message & "<br>")
                    HttpContext.Current.Response.Write(" destinationConstring=" & destinationConstring & "<br>")
                    HttpContext.Current.Response.Write(" sourceConString=" & sourceConString & "<br>")
                    HttpContext.Current.Response.Write(" SourceSQLStr=" & SourceSQLStr & "<br>")
                    HttpContext.Current.Response.Write(" TableToSaveTo=" & TableToSaveTo & "<br>")
                Finally
                    ' Close the SqlDataReader. The SqlBulkCopy object is automatically closed at the end of the Using block.
                    reader.Close()
                End Try
            End Using
        End Using
    End Sub
    Public Shared Sub OnSqlRowsCopied(ByVal sender As Object, ByVal args As SqlRowsCopiedEventArgs)
        Console.WriteLine("Copied {0} so far...", args.RowsCopied)
    End Sub
    Public Shared Function AddRecToTableSQL(ByRef importintoTable As String, ByRef FieldsToInsert As String, ByRef FieldsValuesToInsert As String) As String
        ' make sure that field values are limited with these comas ->'<-
        'or the functions will return error that Error in the "insert into command"

        Dim mresult As String = "Data Imported"
        '        "SELECT * FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        Dim SqlString As String = "INSERT INTO " & importintoTable & " (" & FieldsToInsert & ") Values  (" & FieldsValuesToInsert & ")"
        Dim dbConnection As New OleDbConnection
        Dim SQLconstring As String = "Escapade_NewConnectionString"
        dbConnection.ConnectionString = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Dim mycommand As New OleDbCommand(SqlString, dbConnection)
        Try
            'HttpContext.Current.Response.Write(SqlString)
            dbConnection.Open()
            mresult = mycommand.ExecuteNonQuery()
            dbConnection.Close()
            Return mresult
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write(SqlString & "<br>")
            HttpContext.Current.Response.Write(dbConnection.ToString & "<br>")
        End Try
    End Function
    ''' <summary>
    ''' runs stored procedure with ot without parameters
    ''' </summary>
    ''' <param name="Sproc_name">name of the stored procedure</param>
    ''' <param name="Sproc_parameters"></param>
    ''' <returns>returns string value</returns>
    Public Shared Function executeSQL_Sproc(ByRef Sproc_name As String, Optional Sproc_parameters As String = "", Optional SQLconstring As String = "Escapade_NewConnectionString") As String
        ' make sure that field values are limited with these comas ->'<-
        'or the functions will return error that Error in the "insert into command"

        Dim mresult As String = "Data Imported"
        '        "SELECT * FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        Dim SqlString As String = ""
        If Sproc_parameters <> "" Then
            SqlString = "execute " & Sproc_name & " " & Sproc_parameters & ""
        Else
            SqlString = "execute " & Sproc_name & " ;"
        End If
        Dim dbConnection As New OleDbConnection
        'Dim SQLconstring As String = "Escapade_NewConnectionString"
        dbConnection.ConnectionString = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Dim mycommand As New OleDbCommand(SqlString, dbConnection)
        Try
            'HttpContext.Current.Response.Write(SqlString)
            dbConnection.Open()
            mresult = mycommand.ExecuteNonQuery()
            dbConnection.Close()
            Return mresult
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write(SqlString & "<br>")
            HttpContext.Current.Response.Write(dbConnection.ToString & "<br>")
        End Try
    End Function
    Public Shared Function UpdateTableSQL(ByRef importintoTable As String, ByRef FieldToUpdate As String, ByRef FieldValueToUpdate As String, ByRef mCondition As String) As String
        ' make sure that field values are limited with these comas ->'<-
        'or the functions will return error that Error in the "insert into command"
        'Dim doc As String = FieldsValueToUpdate.Linq.xdocument
        Dim mresult As String = "Data Imported"
        '        "SELECT * FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        Dim SqlString As String = "UPDATE " & importintoTable & " set " & FieldToUpdate & " =  " & FieldValueToUpdate & " where " & mCondition
        Dim dbConnection As New OleDbConnection
        Dim SQLconstring As String = "Escapade_NewConnectionString"
        dbConnection.ConnectionString = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        ' HttpContext.Current.Response.Write("SqlString=" & SqlString)

        Dim mycommand As New OleDbCommand(SqlString, dbConnection)
        If InStr(mCondition, "0110000001485") Or InStr(mCondition, "0110000002949") Then
            HttpContext.Current.Response.Write("FN_UpdateTableSQL: " & SqlString & "<br>")
            HttpContext.Current.Response.Write("********************************* <br>")
        End If
        Try
            'HttpContext.Current.Response.Write(SqlString)
            dbConnection.Open()
            mresult = mycommand.ExecuteNonQuery()
            dbConnection.Close()
            Return mresult
        Catch ex As Exception
            HttpContext.Current.Response.Write("ERROR :UpdateTableSQL: " & ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write(SqlString & "<br>")
            HttpContext.Current.Response.Write(dbConnection.ToString & "<br>")
            HttpContext.Current.Response.End()
        End Try
    End Function

    ' this is a shortcut for your connection string 
    'this value has to be taken from the web.config main db constring
    'Shared SharedDatabaseConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings(constring).ConnectionString

    ''' <summary>
    '''this is for just executing sql command with no value to return 
    ''' </summary>
    ''' <param name="SQLconstring"></param>
    ''' <param name="sqlstr">sql statement name fromweb.config</param>
    ''' <remarks>eexcutes sql string with connection without returning any result</remarks>

    Public Shared Sub SqlExecute(SQLconstring As String, sqlstr As String)
        Dim DatabaseConnectionString As String = ""
        Try
            DatabaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
            ' this is a shortcut for your connection string
            Using conn As New SqlConnection(DatabaseConnectionString)
                Dim cmd As New SqlCommand(sqlstr, conn)
                cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            HttpContext.Current.Response.Write("ex:" & ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write("sqlstr:" & sqlstr & "<br>")
            HttpContext.Current.Response.Write("DatabaseConnectionString:" & DatabaseConnectionString & "<br>")
            HttpContext.Current.Response.End()

        End Try

    End Sub
    ' this is a shortcut for your connection string 
    'this value has to be taken from the web.config main db constring
    'Shared SharedDatabaseConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings(constring).ConnectionString

    ''' <summary>
    '''this is for just executing sql command with no value to return 
    ''' </summary>
    ''' <param name="SQLconstring"></param>
    ''' <param name="sqlstr">sql statement name fromweb.config</param>
    ''' <remarks>eexcutes sql string with connection without returning any result</remarks>

    Public Shared Sub SqlExecuteWOConstring(sqlstr As String, Optional SQLconstring As String = "Escapade_NewConnectionString")
        Dim DatabaseConnectionString As String = ""
        Try
            DatabaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
            ' this is a shortcut for your connection string
            Using conn As New SqlConnection(DatabaseConnectionString)
                Dim cmd As New SqlCommand(sqlstr, conn)
                cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            HttpContext.Current.Response.Write("ex:" & ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write("SqlExecuteWOConstring.sqlstr:" & sqlstr & "<br>")
            HttpContext.Current.Response.Write("DatabaseConnectionString:" & DatabaseConnectionString & "<br>")
            HttpContext.Current.Response.End()

        End Try

    End Sub
    ''' <summary>
    '''   ' with this you will be able to return a value
    ''' </summary>
    ''' <param name="SQLconstring"> as connectionstring name from webconfig</param>
    ''' <param name="sql">sql statement name fromweb.config</param>
    ''' <returns>returns result as object</returns>
    ''' <remarks></remarks>
    Public Shared Function SqlReturn(SQLconstring As String, sql As String) As Object
        Dim DatabaseConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Try
            Using conn As New SqlConnection(DatabaseConnectionString)
                conn.Open()
                Dim cmd As New SqlCommand(sql, conn)
                Dim result As Object = DirectCast(cmd.ExecuteScalar(), Object)
                Return result
            End Using
        Catch e1 As SqlException
            HttpContext.Current.Response.Write("SqlReturn.SqlException.Message" & e1.Message)
            HttpContext.Current.Response.Write("<br>SqlReturn.strsql= " & sql)
            HttpContext.Current.Response.Write("<br>SqlReturn.DatabaseConnectionString= " & DatabaseConnectionString)
            'HttpContext.Current.Response.End()
        Catch e2 As Exception
            HttpContext.Current.Response.Write("SqlReturn.Exception.Message= " & e2.Message)
            HttpContext.Current.Response.Write("<br>SqlReturn.strsql= " & sql)
            HttpContext.Current.Response.Write("<br>SqlReturn.DatabaseConnectionString= " & DatabaseConnectionString)
            'HttpContext.Current.Response.End()
        End Try
    End Function


    Public Shared Function CheckDuplications(ByVal fDBName As String, ByVal fTableName As String, ByVal fSearchField As String, ByRef fSearchValue As String, ByVal fSeekFields As String) As String
        Dim mDbFolder As String = "/App_Data/"
        Dim MDatabseName As String = Current.Application("accessDB_name")
        Dim fReturnData As String = ""
        Return fReturnData
    End Function

    Public Shared Function MinMaxValueSQL(ByVal fTableName As String, ByRef fSearchField As String, ByRef fSearchMinOrMax As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As String
        'HttpContext.Current.Response.Write("fTableName: " & fTableName & "  fSearchField: " & fSearchField & "  fSearchMinOrMax: " & fSearchMinOrMax & "  SQLconstring: " & SQLconstring & "<br>")
        'HttpContext.Current.Response.End()
        'Dim connect As [String] = "Provider=SQLOLEDB;Data Source=ESCAPADEDEVELOP\SQLEXPRESS;Initial Catalog=Escapade_New;Persist Security Info=True;User ID=sa1;Password=password"
        Dim connect As String = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Dim dbConnection As New OleDbConnection(connect)
        Dim fReturnData As String = "nothing"
        Dim ds As New DataSet
        Dim Myvalue As String = fSearchMinOrMax & fSearchField ' & "'"
        dbConnection.Open()

        Dim SqlString As String = "SELECT " & fSearchMinOrMax & "(Convert(float," & fSearchField & ")) as " & Myvalue & " FROM [" & fTableName & "] WITH (NOLOCK) "
        Dim command As New OleDbCommand(SqlString, dbConnection)

        Dim da As New OleDbDataAdapter(command)
        'da.Fill(ds)
        '*******************
        Try
            da.Fill(ds)
            Console.WriteLine("Made connection to the database")
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If Not IsDBNull(dr(Myvalue)) Then
                        fReturnData = dr(Myvalue)
                    End If
                Next
            End If
        Catch ex1 As SqlException
            HttpContext.Current.Response.Write("SqlException=" & ex1.Message & "<br/>")
        Catch ex As Exception
            HttpContext.Current.Response.Write("FN MinMaxValueSQL error:" & SqlString & "<br/>")
            HttpContext.Current.Response.Write("FN MinMaxValueSQL ex.Message:" & ex.Message & "<br/>")
            ' HttpContext.Current.Response.End()
        End Try

        'Console.WriteLine("Made the connection to the database")
        'For Each dr As DataRow In ds.Tables(0).Rows
        '    fReturnData = dr(Myvalue)
        'Next
        'Dim command As OleDbCommand = con.CreateCommand()

        ' fReturnData = (command.CommandText = "SELECT " & fSearchMinOrMax & "(" & fSearchField & ") FROM " & fTableName)
        '        Console.WriteLine("Max/min: {0:D}")
        dbConnection.Close()
        fReturnData = MiscClass.ifnullget(fReturnData, 0)
        Return fReturnData
    End Function


    Public Shared Function MinMaxValueSQLWithCriteria(ByVal fTableName As String, ByRef fSearchField As String, ByRef fSearchMinOrMax As String, ByRef fSearchCriteria As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As String
        Dim connect As String = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Dim dbConnection As New OleDbConnection(connect)
        Dim fReturnData As String = "nothing"
        Dim ds As New DataSet
        Dim Myvalue As String = fSearchMinOrMax & fSearchField ' & "'"
        dbConnection.Open()

        Dim SqlString As String = "SELECT " & fSearchMinOrMax & "(Convert(float," & fSearchField & ")) as " & Myvalue & " FROM [" & fTableName & "] WITH (NOLOCK) where " & fSearchCriteria & ";"
        Dim command As New OleDbCommand(SqlString, dbConnection)

        Dim da As New OleDbDataAdapter(command)
        'da.Fill(ds)
        '*******************
        Try
            da.Fill(ds)
            ' Console.WriteLine("Made connection to the database")
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If Not IsDBNull(dr(Myvalue)) Then
                        fReturnData = dr(Myvalue)
                    End If
                Next
            End If
        Catch ex1 As SqlException
            HttpContext.Current.Response.Write("SqlException=" & ex1.Message & "<br/>")
        Catch ex As Exception
            HttpContext.Current.Response.Write("FN MinMaxValueSQL error:" & SqlString & "<br/>")
            HttpContext.Current.Response.Write("FN MinMaxValueSQL ex.Message:" & ex.Message & "<br/>")
            ' HttpContext.Current.Response.End()
        End Try
        dbConnection.Close()
        fReturnData = MiscClass.ifnullget(fReturnData, 0)
        Return fReturnData
    End Function
    Public Shared Function ifnullget(ByRef val As String, ByVal retval As String) As String
        Dim fReturnData As String = ""

        If IsNumeric(val) Then
            If IsDBNull(val) Or IsNothing(val) Or val = "" Then fReturnData = retval Else ifnullget = val
        Else
            If IsNothing(val) Or val = "" Then fReturnData = retval Else ifnullget = val
        End If
        Return fReturnData
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="fSqlString">sql string to run including stored procedures</param>
    ''' <param name="SQLconstring"> connection string name from web.config file</param>
    ''' <returns></returns>
    ''' <remarks>Runs Stored Procedures e.g SQLFunctions.RunSQLStringWOConstring("execute [AddSelectedItems] '663703'") </remarks>
    Public Shared Function RunSQLStringWOConstring(ByRef fSqlString As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As String

        ' make sure that field values are limited with these comas ->'<-
        'or the functions will return error that "Error in  insert into command"

        Dim mresult As String = "SQLstring Not Executed"
        '        "SELECT * FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue

        Dim SqlString As String = Replace(Replace(fSqlString, False, 0), True, 1)
        Dim conStr As String = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = conStr
        Dim mycommand As New OleDbCommand(SqlString, dbConnection)
        'HttpContext.Current.Response.Write("Error SqlString:" & SqlString & "<br>")


        Try
            dbConnection.Open()
            mycommand.ExecuteNonQuery()
            mresult = "SQLstring Executed"
            ' HttpContext.Current.Response.Write(SqlString & "<br>")
        Catch sqlex As SqlException
            HttpContext.Current.Response.Write("RunSQLStringWOConstring.SqlException.Message" & sqlex.Message)
            HttpContext.Current.Response.Write("<br>RunSQLStringWOConstring.strsql= " & SqlString)
            HttpContext.Current.Response.End()
        Catch ex As Exception
            HttpContext.Current.Response.Write("Error: RunSQLStringWOConstring ln:271" & "<br/>")
            HttpContext.Current.Response.Write("SqlString" & SqlString & "<br/>")
            HttpContext.Current.Response.Write("RunSQLString Func" & ex.Message & "<br/>")
            HttpContext.Current.Response.Write("constring:" & SQLconstring)
            HttpContext.Current.Response.End()
            mresult = "SQLstring Not Executed"
        End Try
        'HttpContext.Current.Response.Write("RunSQLString Func" & ex.Message & "<br/>")

        dbConnection.Close()
        'HttpContext.Current.Response.Write("mresult:" & mresult)
        'HttpContext.Current.Response.End()
        Return mresult
    End Function

    Public Shared Function RunSQLStringWOConstringWithCondition(ByRef fSqlString As String, ByRef sqlConditionString As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As String

        ' make sure that field values are limited with these comas ->'<-
        'or the functions will return error that "Error in  insert into command"

        Dim mresult As String = "SQLstring Not Executed"
        '        "SELECT * FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue

        Dim SqlString As String = Replace(Replace(fSqlString, False, 0), True, 1)
        Dim conStr As String = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = conStr
        Dim mycommand As New OleDbCommand(SqlString, dbConnection)
        Dim conditionalinsersql As String = " if (" & sqlConditionString & ") < 1 begin " & fSqlString & " end"
        HttpContext.Current.Response.Write("Error SqlString:" & conditionalinsersql & "<br>")

        Dim conditionmet As String = GetSearchDataSQLWithOutConStr(conditionalinsersql)
        Try
            dbConnection.Open()
            mycommand.ExecuteNonQuery()
            mresult = "SQLstring Executed"
            ' HttpContext.Current.Response.Write("FN sqlConditionString=" & sqlConditionString & "<br>")
        Catch sqlex As SqlException
            HttpContext.Current.Response.Write("RunSQLStringWOConstringWithCondition.SqlException.Message" & sqlex.Message)
            HttpContext.Current.Response.Write("<br>RunSQLStringWOConstringWithCondition.strsql= " & SqlString)
            HttpContext.Current.Response.End()
        Catch ex As Exception
            HttpContext.Current.Response.Write("Error: RunSQLStringWOConstring ln:271" & "<br/>")
            HttpContext.Current.Response.Write("SqlString" & SqlString & "<br/>")
            HttpContext.Current.Response.Write("RunSQLString Func" & ex.Message & "<br/>")
            HttpContext.Current.Response.Write("constring:" & SQLconstring)
            HttpContext.Current.Response.End()
            mresult = "SQLstring Not Executed"
        End Try
        'HttpContext.Current.Response.Write("RunSQLString Func" & ex.Message & "<br/>")
        dbConnection.Close()
        'HttpContext.Current.Response.Write("mresult:" & mresult)
        'HttpContext.Current.Response.End()
        Return mresult
    End Function

    Public Shared Function RunSQLString(ByRef constring As String, ByRef fSqlString As String) As String

        ' make sure that field values are limited with these comas ->'<-
        'or the functions will return error that "Error in  insert into command"

        Dim mresult As String = "SQLstring Not Executed"
        '        "SELECT * FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue

        Dim SqlString As String = Replace(Replace(fSqlString, False, 0), True, 1)

        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = constring
        Dim mycommand As New OleDbCommand(SqlString, dbConnection)
        'HttpContext.Current.Response.Write("Error SqlString:" & SqlString & "<br>")


        Try
            dbConnection.Open()
            mycommand.ExecuteNonQuery()
            mresult = "SQLstring Executed"
            ' HttpContext.Current.Response.Write(SqlString & "<br>")
        Catch sqlex As SqlException
            HttpContext.Current.Response.Write("ln: 459 RunSQLString.SqlException.Message" & sqlex.Message)
            HttpContext.Current.Response.Write("<br>ln: 460 RunSQLString.strsql= " & SqlString)
            HttpContext.Current.Response.End()
        Catch ex As Exception
            HttpContext.Current.Response.Write("Error:" & "<br/>")
            HttpContext.Current.Response.Write("SqlString" & SqlString & "<br/>")
            HttpContext.Current.Response.Write("RunSQLString Func" & ex.Message & "<br/>")
            HttpContext.Current.Response.Write("constring:" & constring)
            HttpContext.Current.Response.End()
            mresult = "SQLstring Not Executed"
        End Try
        'HttpContext.Current.Response.Write("RunSQLString Func" & ex.Message & "<br/>")

        dbConnection.Close()
        'HttpContext.Current.Response.Write("mresult:" & mresult)
        'HttpContext.Current.Response.End()
        Return mresult
    End Function

    ' GetSearchData
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
        Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & " WITH(NOLOCK)  WHERE " & fSearchField & " like '" & fSearchValue & "';"

        Dim DbFilePAth As String = "~" & mDbFolder & MDatabseName
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        fSearchField = Trim(Replace(Replace(fSearchField, "[[", "["), "]]", "]"))
        If Mid(fSearchField, 1, 1) = "[" Or Mid(fSearchField, 4, 1) = "[" Then
            SqlString = "SELECT " & fSeekFields & " FROM " & fTableName & " WITH(NOLOCK)  WHERE " & fSearchField & " like '" & fSearchValue & "' ;"
        Else
            SqlString = "SELECT " & fSeekFields & " FROM " & fTableName & " WITH(NOLOCK)  WHERE [" & fSearchField & "] like '" & fSearchValue & "' ;"
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



    ''' <summary>
    '''  Get Search Data SQ LWith Constr using datareader
    ''' </summary>
    ''' <param name="conStr">like :"Provider=SQLOLEDB;Data Source=ESCAPADEDEVELOP\SQLEXPRESS;Initial Catalog=Escapade_New;Persist Security Info=True;User ID=sa1;Password=password"</param>
    ''' <param name="SqlString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetSearchDataSQLWithConstr(ByVal conStr As String, ByVal SqlString As String) As String
        Dim fReturnData As String = "00"
        ' Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & "  WITH (NOLOCK)  WHERE " & fSearchCriteria
        'SqlString = Replace(SqlString, "'", "''")
        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = conStr
        dbConnection.Open()
        Using connection As New OleDbConnection(conStr)
            Dim command As New OleDbCommand(SqlString, connection)
            Try
                connection.Open()
                Dim dataReader As OleDbDataReader = command.ExecuteReader()
                Do While dataReader.Read()
                    If Not (IsNothing(dataReader(0)) Or DBNull.Value.Equals(dataReader(0))) Then : linenumber = 382
                        fReturnData = ifnullget(dataReader(0), "-")
                    End If
                Loop
                dataReader.Close()
            Catch sqlex As SqlException
                HttpContext.Current.Response.Write("GetSearchDataSQLWithConstr.SqlException.Message" & sqlex.Message)
                HttpContext.Current.Response.Write("<br>GetSearchDataSQLWithConstr.strsql= " & SqlString)
                HttpContext.Current.Response.End()
            Catch ex As Exception
                HttpContext.Current.Response.Write("Error Line=351 GetSearchDataSQLWithConstr : " & SqlString & "<br>")
                HttpContext.Current.Response.Write("ErrorToString GetSearchDataSQLWithConstr : " & conStr & "<br>")
                HttpContext.Current.Response.Write("ExceptionToString: " & ex.Message)
                Console.WriteLine(ex.Message)
                HttpContext.Current.Response.End()
            End Try
            Console.ReadLine()
        End Using
        'HttpContext.Current.Response.Write("SQLStringProcessed: " & SqlString & "<br>")
        'HttpContext.Current.Response.Write("ReturnValue: " & fReturnData & "<br>")
        Return fReturnData
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="SqlString"></param>
    ''' <param name="SQLconstring"></param>
    ''' <returns></returns>
    ''' <remarks>Function returns 00 if search data is not found</remarks>
    Public Shared Function GetSearchDataSQLWithOutConStr(ByVal SqlString As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As String
        Dim fReturnData As String = "00"
        'HttpContext.Current.Response.Write("SQLFUNCTIONS.GetSearchDataSQLWithOutConStr LN438: " & SqlString & "<br>")
        ' Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & "  WITH (NOLOCK)  WHERE " & fSearchCriteria
        'SqlString = Replace(SqlString, "'", "''")
        ' Dim SQLconstring As String = "Escapade_NewConnectionString"
        'Dim conStr As String = "Provider=SQLOLEDB;Data Source=ESCAPADEDEVELOP\SQLEXPRESS;Initial Catalog=Escapade_New;Persist Security Info=True;User ID=sa1;Password=password"
        Dim conStr As String = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        'Dim dbConnection As New OleDbConnection
        'dbConnection.ConnectionString = conStr
        'dbConnection.Open()

        Using connection As New OleDbConnection(conStr)
            Dim command As New OleDbCommand(SqlString, connection)
            Try
                connection.Open()
                'HttpContext.Current.Response.End()
                Dim dataReader As OleDbDataReader = command.ExecuteReader()
                Do While dataReader.Read()
                    If Not (IsNothing(dataReader(0)) Or DBNull.Value.Equals(dataReader(0))) Then : linenumber = 651
                        fReturnData = dataReader(0) : linenumber = 652
                    End If
                Loop
                dataReader.Close()
            Catch ex As Exception
                HttpContext.Current.Response.Write("Error Ln:632 GetSearchDataSQLWithConstr : " & SqlString & "<p>")
                HttpContext.Current.Response.Write("ExceptionToString: " & ex.Message & "<p>")
                HttpContext.Current.Response.Write("Last Line Executed: " & linenumber & "<p>")
                Console.WriteLine(ex.Message & "<p>")

                HttpContext.Current.Response.Write("SQLStringProcessed: " & SqlString & "<p>")
                HttpContext.Current.Response.Write("ReturnValue: " & fReturnData & "<p>")
                HttpContext.Current.Response.End()
            End Try
            Console.ReadLine()
        End Using
        'HttpContext.Current.Response.Write("SQLFUNCTIONS.GetSearchDataSQLWithOutConStr.fReturnData LN470:=" & fReturnData & "<br>")
        'HttpContext.Current.Response.Write("SQLFUNCTIONS.GetSearchDataSQLWithOutConStr.conStr LN470:=" & conStr & "<br>")
        Return fReturnData
    End Function

    Public Shared Function GetSearchDataSQLWithOutConstrRS(ByVal SqlString As String) As OleDbDataReader
        Dim fReturnData As OleDbDataReader
        ' Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & "  WITH (NOLOCK)  WHERE " & fSearchCriteria
        'SqlString = Replace(SqlString, "'", "''")
        Dim SQLconstring As String = "Escapade_NewConnectionString"
        'Dim conStr As String = "Provider=SQLOLEDB;Data Source=ESCAPADEDEVELOP\SQLEXPRESS;Initial Catalog=Escapade_New;Persist Security Info=True;User ID=sa1;Password=password"
        Dim conStr As String = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        'Dim dbConnection As New OleDbConnection
        'dbConnection.ConnectionString = conStr
        'dbConnection.Open()

        Using connection As New OleDbConnection(conStr)
            Dim command As New OleDbCommand(SqlString, connection)
            Try
                connection.Open()
                'HttpContext.Current.Response.End()
                Dim dataReader As OleDbDataReader = command.ExecuteReader()
                Do While dataReader.Read()
                    If Not IsNothing(dataReader(0)) Or DBNull.Value.Equals(dataReader(0)) Then
                        fReturnData = dataReader
                    End If
                Loop
                dataReader.Close()
            Catch ex As Exception
                HttpContext.Current.Response.Write("ErrorToString GetSearchDataSQLWithConstr : " & SqlString & "<p>")
                HttpContext.Current.Response.Write("ExceptionToString: " & ex.Message & "<p>")
                Console.WriteLine(ex.Message & "<p>")

                HttpContext.Current.Response.Write("SQLStringProcessed: " & SqlString & "<p>")
                'HttpContext.Current.Response.Write("ReturnValue: " & fReturnData & "<p>")
                HttpContext.Current.Response.End()
            End Try
            Console.ReadLine()
        End Using

        Return fReturnData
    End Function


    Public Shared Function GetSearchDataSQL(ByVal fTableName As String, ByVal fSearchCriteria As String, ByVal fSeekFields As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As String

        Dim fReturnData As String = "0"
        Dim ds As New DataSet

        ' Dim SQLconstring As String = "Escapade_NewConnectionString"
        'Dim conStr As String = "Provider=SQLOLEDB;Data Source=ESCAPADEDEVELOP\SQLEXPRESS;Initial Catalog=Escapade_New;Persist Security Info=True;User ID=sa1;Password=password"
        Dim conStr As String = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString

        conStr = "Provider=SQLOLEDB;" & conStr
        Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & "  WITH (NOLOCK)  WHERE " & fSearchCriteria
		'HttpContext.Current.Response.Write("<br>sqlfunctions.GetSearchDataSQL:SqlString : " & SqlString & "<br>")
        Using connection As New OleDbConnection(conStr)

            Dim command As New OleDbCommand(SqlString, connection)

            Try
                connection.Open()
                Dim dataReader As OleDbDataReader = command.ExecuteReader()
                Do While dataReader.Read()
                    If Not (IsNothing(dataReader(0)) Or DBNull.Value.Equals(dataReader(0))) Then
                        fReturnData = dataReader(0)
                        fReturnData = MiscClass.ifnullget(fReturnData, "00")
                    End If
                Loop
                dataReader.Close()

            Catch ex As Exception
                HttpContext.Current.Response.Write(ex.Message & "<br>")
                HttpContext.Current.Response.Write("sqlfunctions.GetSearchDataSQL:fSearchCriteria : " & fSearchCriteria & "fSeekFields=" & fSeekFields & "<br>")
                HttpContext.Current.Response.Write("<br>sqlfunctions.GetSearchDataSQL:SqlString : " & SqlString & "<br>")
                HttpContext.Current.Response.Write("<br>sqlfunctions.GetSearchDataSQL:connection : " & conStr & "<br>")
                HttpContext.Current.Response.End()
            End Try
            Console.ReadLine()
        End Using

        Return fReturnData
    End Function
    Public Shared Function GetSearchDataNoQuotes(ByVal ConnectionString As String, ByVal fTableName As String, ByVal fSearchField As String, ByRef fSearchValue As String, ByVal fSeekFields As String) As String


        Dim fReturnData As String = ""
        Dim ds As New DataSet
        'Dim conStr As String = "Provider=SQLOLEDB;Data Source=ESCAPADEDEVELOP\SQLEXPRESS;Initial Catalog=Escapade_New;Persist Security Info=True;User ID=sa1;Password=password"

        Dim SQLconstring As String = "Escapade_NewConnectionString"
        Dim conStr As String = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString

        conStr = "Provider=SQLOLEDB;" & conStr



        fSearchValue = Replace(Replace(Replace(Replace(fSearchValue, "'", ""), "\", ""), "/", ""), ",", "")
        fSearchField = "REPLACE(REPLACE(REPLACE(" & fSearchField & ", '''', ''), ',', ''), '\', '')"
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        Dim SqlString As String = "SELECT * FROM " & fTableName & " WITH(NOLOCK)  WHERE " & fSearchField & " = '" & fSearchValue & "'"

        ' HttpContext.Current.Response.Write(SqlString & "<br/>")
        'HttpContext.Current.Response.Write(ex.Message)
        'HttpContext.Current.Response.End()

        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = conStr
        dbConnection.Open()
        Dim command As New OleDbCommand(SqlString, dbConnection)
        Try
            'HttpContext.Current.Response.End()
            Dim da As New OleDbDataAdapter(command)
            da.Fill(ds)

            For Each dr As DataRow In ds.Tables(0).Rows
                If IsDBNull(dr(fSeekFields)) Or IsNothing(dr(fSeekFields)) Then
                    fReturnData = "0"
                Else
                    fReturnData = dr(fSeekFields)

                End If
            Next

            If fReturnData = "" Then
                fReturnData = "Not Found"
            End If
            dbConnection.Close()
            ds.Clear()
            da.Dispose()
        Catch ex As Exception
            HttpContext.Current.Response.Write("Error Message:  " & ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write("fSearchField: " & fSearchField & "<br>")
            HttpContext.Current.Response.Write("fSearchValue: " & fSearchValue & "<br>")
            HttpContext.Current.Response.Write("Criteria : " & fSearchField & "=" & fSearchValue & "<br>")
            HttpContext.Current.Response.Write("SqlString:  " & SqlString & "<br>")
            HttpContext.Current.Response.Write("ConnectionString:  " & ConnectionString.ToString & "<br>")
        End Try


        Return fReturnData
    End Function


    Public Shared Function GetSearchData(ByVal ConnectionString As String, ByVal fTableName As String, ByVal fSearchField As String, ByRef fSearchValue As String, ByVal fSeekFields As String) As String


        Dim fReturnData As String = ""
        Dim ds As New DataSet
        'Dim conStr As String = "Provider=SQLOLEDB;Data Source=ESCAPADEDEVELOP\SQLEXPRESS;Initial Catalog=Escapade_New;Persist Security Info=True;User ID=sa1;Password=password"
        Dim SQLconstring As String = "Escapade_NewConnectionString"
        Dim conStr As String = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        fSearchValue = Replace(fSearchValue, "'", "char(39)")
        Dim SqlString As String = "SELECT * FROM " & fTableName & " WITH(NOLOCK)  WHERE " & fSearchField & " = '" & fSearchValue & "'"

        ' HttpContext.Current.Response.Write(SqlString & "<br/>")
        'HttpContext.Current.Response.Write(ex.Message)
        'HttpContext.Current.Response.End()

        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = conStr
        dbConnection.Open()
        Dim command As New OleDbCommand(SqlString, dbConnection)
        Try
            Dim da As New OleDbDataAdapter(command)
            da.Fill(ds)

            For Each dr As DataRow In ds.Tables(0).Rows
                If IsDBNull(dr(fSeekFields)) Or IsNothing(dr(fSeekFields)) Then
                    fReturnData = "0"
                Else
                    fReturnData = dr(fSeekFields)

                End If
            Next

            If fReturnData = "" Then
                fReturnData = "Not Found"
            End If
            dbConnection.Close()
            ds.Clear()
            da.Dispose()
        Catch ex As Exception
            HttpContext.Current.Response.Write("Error Message:  " & ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write("SqlString:  " & SqlString & "<br>")
            HttpContext.Current.Response.Write("ConnectionString:  " & ConnectionString.ToString & "<br>")
        End Try


        Return fReturnData
    End Function


    Public Shared Function GetAccessDb(ByVal fTableName As String, ByVal fSearchCriteria As String, ByVal fSeekFields As String) As AccessDataSource

        Dim mDbFolder As String = "/App_Data/IMSStock.mdb"
        Dim MDatabseName As String = Current.Application("accessDB_name")

        Dim DbFilePAth As String = "~" & mDbFolder ' & MDatabseName
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'dim SqlString as string =  "SELECT "& mSeekFields & " FROM " & mTableName & " WHERE " & mSearchField & " = "  &mSearchValue

        Dim SqlString As String = "SELECT " & SafeSqlLiteral(fSeekFields, 2) & " FROM " & SafeSqlLiteral(fTableName, 2) & " WITH(NOLOCK)  WHERE " & SafeSqlLiteral(fSearchCriteria, 2)

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
    Public Shared Function getTableFieldListSQL(fTableName As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As String
        Dim strSQL As String
        Dim i As Integer
        Dim mDbFolder As String = "App_Data/"
        Dim TableFiledlist As String = ""
        ' Create a connection to the data source. 
        Dim MyConnection As OleDbConnection = New OleDbConnection("Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString)
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
    Public Shared Function getReaderTableFieldAttribs(dtreader As OleDbDataReader, fieldname As String, Optional atribneeded As String = "All") As String
        Dim returnvalue As String = ""
        Dim st = dtreader.GetSchemaTable
        Dim colname As String
        Dim colsize As String
        Dim coldatatype As String
        Dim coliskeycolumn As String = ""
        Dim colIsAutoIncrement As String
        For Each row In st.Rows
            colname = row("ColumnName")
            colsize = row("ColumnSize")
            coldatatype = row("DataType").ToString
            coliskeycolumn = row("IsKey").ToString
            colIsAutoIncrement = row("IsAutoIncrement").ToString
            If colname = fieldname Then
                Select Case atribneeded
                    Case "All"
                        returnvalue = colname & "^" & colsize & "^" & coldatatype & "^" & coliskeycolumn & "^" & colIsAutoIncrement
                    Case "ColumnName"
                        returnvalue = colname
                    Case "ColumnSize"
                        returnvalue = colsize
                    Case "DataType"
                        returnvalue = coldatatype
                    Case "IsKey"
                        returnvalue = coliskeycolumn
                    Case "IsAutoIncrement"
                        returnvalue = colIsAutoIncrement
                    Case Else
                        returnvalue = atribneeded & " not found"
                End Select
            End If

        Next
        Return returnvalue
    End Function

    Public Shared Function GetDataReader(strSQL As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As OleDbDataReader
        Dim returnvalue As OleDbDataReader
        'Dim strSQL As String
        'Dim i As Integer
        Dim FiledName As String = ""
        ' Create a connection to the data source. 
        Dim dbConnection As New OleDbConnection
        dbConnection.ConnectionString = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        dbConnection.Open()
        'strSQL = "SELECT * FROM " & fTableName
        ' Create a Command object with the SQL statement.

        Dim dtReader2 As OleDbDataReader
        Dim objCmd As New OleDbCommand(strSQL, dbConnection)
        dtReader2 = objCmd.ExecuteReader()

        returnvalue = dtReader2
        Return returnvalue
    End Function

    Public Shared Function CheckTableFieldNameExistsSQL(fTableName As String, fFieldName As String) As String
        Dim strSQL As String
        Dim i As Integer
        Dim FiledName As String = ""
        ' Create a connection to the data source. 
        Dim dbConnection As New OleDbConnection
        Dim SQLconstring As String = "Escapade_NewConnectionString"
        dbConnection.ConnectionString = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        dbConnection.Open()
        strSQL = "SELECT * FROM " & fTableName

        Dim dtReader As OleDbDataReader
        ' Create a Command object with the SQL statement.
        Dim objCmd As New OleDbCommand(strSQL, dbConnection)
        'dtReader = GetDataReader(strSQL)
        dtReader = objCmd.ExecuteReader()
        ' HttpContext.Current.Response.Write("##" & "<b>Table customer has " & dtReader.FieldCount & " Fields</b>" & "##")
        ' HttpContext.Current.Response.End()


        For i = 0 To dtReader.FieldCount - 1
            'following line is for testing
            '  HttpContext.Current.Response.Write(Trim(dtReader.GetName(i)) & "<-->" & Trim(fFieldName) & "<br/>")
            If Trim(dtReader.GetName(i)) Like Trim(fFieldName) Then
                FiledName = dtReader.GetName(i)
                HttpContext.Current.Response.Write("##" & SQLFunctions.getReaderTableFieldAttribs(dtReader, FiledName, "ColumnSize") & "##")
                Exit For
            Else
                FiledName = fFieldName & " Not found"
            End If
        Next
        ' dbConnection.Close()
        dtReader.Close()
        dtReader = Nothing
        Return FiledName
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
        Dim mType As String = ""
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
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Amount">amount to be rounded</param>
    ''' <param name="RoundTo"> give value like 1 , 10 , 100 or .01 or .1 for nearest integer or fraction</param>
    ''' <returns>returnsds teh value rounded to nearest unit, 1, tens 10, hundrads 100, or thousands</returns>
    ''' <remarks></remarks>
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
    ''' <param name="SQLconstring"></param>
    ''' <param name="fTableName"></param>
    ''' <param name="fSearchField"></param>
    ''' <param name="fSearchMinOrMaxOrSum">use words as Min, Max , Avg, count</param>
    ''' <param name="fSearchFieldName">Search fIELD nAME for date uses CONVERT(VARCHAR(11),orderdate,106) </param>
    ''' <param name="fSearchFieldVale"> make sure the value is put into single comas like this 'SASHAS' dor fdate uses "12 oct 2012"</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMinMaxSumValueWithCriteria(ByVal SQLconstring As String, ByVal fTableName As String, ByRef fSearchField As String, ByRef fSearchMinOrMaxOrSum As String, ByRef fSearchFieldName As String, ByRef fSearchFieldVale As String) As String
        Dim connect As [String] = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString

        Dim dbConnection As New OleDbConnection(connect)
        Dim fReturnData As String = "nothing"
        Dim ds As New DataSet
        Dim Myvalue As String = fSearchMinOrMaxOrSum & fSearchField
        dbConnection.Open()
        Dim SqlString As String = "SELECT " & fSearchMinOrMaxOrSum & "(" & fSearchField & ") as " & Myvalue & " FROM " & fTableName & " WITH(NOLOCK) Where " & fSearchFieldName & " = '" & fSearchFieldVale & "'"
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
    ''' <param name="SQLconstring"></param>
    ''' <param name="fTableName"></param>
    ''' <param name="fSearchField"></param>
    ''' <param name="fSearchMinOrMaxOrSum">use words as Min, Max , Avg, count</param>
    ''' <param name="fSearchFieldName">Search fIELD nAME for date uses CONVERT(VARCHAR(11),orderdate,106) </param>
    ''' <param name="fSearchFieldVale"> make sure the value is put into single comas like this 'SASHAS' dor fdate uses "12 oct 2012"</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMinMaxSumValueWithCriteriaWOConstr(ByVal fTableName As String, ByRef fSearchField As String, ByRef fSearchMinOrMaxOrSum As String, ByRef fSearchFieldName As String, ByRef fSearchFieldVale As String, Optional ByVal SQLconstring As String = "Escapade_NewConnectionString") As String
        Dim connect As [String] = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString

        Dim dbConnection As New OleDbConnection(connect)
        Dim fReturnData As String = "nothing"
        Dim ds As New DataSet
        Dim Myvalue As String = Replace(Replace(Replace(Replace(fSearchMinOrMaxOrSum & fSearchField, " ", ""), "(", ""), ")", ""), "'", "")
        dbConnection.Open()
        Dim SqlString As String = "SELECT isnull(" & fSearchMinOrMaxOrSum & "(" & fSearchField & "),0) as " & Myvalue & " FROM " & fTableName & " WITH(NOLOCK) Where " & fSearchFieldName & " = '" & fSearchFieldVale & "'"
        Dim command As New OleDbCommand(SqlString, dbConnection)
        'HttpContext.Current.Response.Write(SqlString & "<br/>")
        'HttpContext.Current.Response.end()
        Dim da As New OleDbDataAdapter(command)
        'da.Fill(ds)
        '*******************

        Try
            da.Fill(ds)
            Console.WriteLine("Made connection to database")
            For Each dr As DataRow In ds.Tables(0).Rows
                fReturnData = MiscClass.ifnullget(dr(Myvalue), "0")
            Next

        Catch ex As Exception
            HttpContext.Current.Response.Write("GetMinMaxSumValueWithCriteriaWOConstr Error: " & SqlString & "<br/>")
            HttpContext.Current.Response.Write(ex.Message)
            ' HttpContext.Current.Response.End()
        End Try

        'Console.WriteLine("Made the connection to the database")
        'For Each dr As DataRow In ds.Tables(0).Rows
        '    fReturnData = dr(Myvalue)
        'Next
        'Dim command As OleDbCommand = con.CreateCommand()

        ' fReturnData = (command.CommandText = "SELECT " & fSearchMinOrMax & "(" & fSearchField & ") FROM " & fTableName)
        'Console.WriteLine("Max/min: {0:D}")
        dbConnection.Close()
        fReturnData = MiscClass.ifnullget(fReturnData, "0")
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

    Public Shared Sub DeleteRecord(ByVal fTableName As String, ByVal fSearchField As String, ByVal fSearchValue As Integer)

        'Dim mDbFolder As String = "/App_Data/"
        'Dim MDatabseName As String = Current.Application("accessDB_name")
        'Dim DbFilePAth As String = "~" & mDbFolder & MDatabseName
        Dim SQLconstring As String = "Escapade_NewConnectionString"
        'for single item search function enable the following line and disabe the string with mSearchCriteria
        'Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue
        Dim SqlString As String = "DELETE  FROM " & fTableName & " WHERE " & fSearchField & " = " & fSearchValue

        Dim dbConnection As New OleDbConnection
        'dbConnection.ConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(fDBName))
        dbConnection.ConnectionString = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        dbConnection.Open()
        Dim command As New OleDbCommand(SqlString, dbConnection)
        command.ExecuteNonQuery()
        'HttpContext.Current.Response.Write(SqlString)
        'HttpContext.Current.Response.End()
        dbConnection.Close()

    End Sub ' GetSearchData
    Function CreateDataSource() As ICollection

        ' Create sample data for the DataGrid control.
        Dim dt As DataTable = New DataTable()
        Dim dr As DataRow

        ' Define the columns of the table.
        dt.Columns.Add(New DataColumn("IntegerValue", GetType(Int32)))
        dt.Columns.Add(New DataColumn("StringValue", GetType(String)))
        dt.Columns.Add(New DataColumn("CurrencyValue", GetType(Double)))

        ' Populate the table with sample values.
        Dim i As Integer

        For i = 0 To 10

            dr = dt.NewRow()

            dr(0) = i
            dr(1) = "Item " & i.ToString()
            dr(2) = 1.23 * (i + 1)

            dt.Rows.Add(dr)

        Next i

        Dim dv As DataView = New DataView(dt)

        Return dv

    End Function


    Public Shared Function ChangeToAccessDate(ByVal fDateToChange As String) As String
        Dim mDateReturn As String
        mDateReturn = "#" & fDateToChange & "#"
        Return mDateReturn
    End Function

    Shared Function GetSearchDataString(p1 As String, p2 As String, p3 As String, mNewPeriod As String) As String
        Throw New NotImplementedException
    End Function

    Private Function CsvToTable(ByVal filePathName As String, Optional ByVal hasHeader As Boolean = False) As DataTable
        ' Parses a csv into a datatable.
        Try
            Dim result As New DataTable
            If System.IO.File.Exists(filePathName) Then
                Dim parser As New Microsoft.VisualBasic.FileIO.TextFieldParser(filePathName)
                parser.Delimiters = New String() {","}
                parser.HasFieldsEnclosedInQuotes = True 'use if data may contain delimiters 
                parser.TextFieldType = FileIO.FieldType.Delimited
                parser.TrimWhiteSpace = True
                Dim HeaderFlag As Boolean
                If hasHeader Then HeaderFlag = True
                While Not parser.EndOfData
                    If AddValuesToTable(parser.ReadFields, result, HeaderFlag) Then
                        HeaderFlag = False
                    Else
                        parser.Close()
                        Return Nothing
                    End If
                End While
                parser.Close()
                Return result
            Else : Return Nothing
            End If
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Function TableToCSV(ByVal sourceTable As DataTable, ByVal filePathName As String, Optional ByVal HasHeader As Boolean = False) As Boolean
        'Writes a datatable back into a csv 
        Try
            Dim sb As New System.Text.StringBuilder
            If HasHeader Then
                Dim nameArray(200) As Object
                Dim i As Integer = 0
                For Each col As DataColumn In sourceTable.Columns
                    nameArray(i) = CType(col.ColumnName, Object)
                    i += 1
                Next col
                ReDim Preserve nameArray(i - 1)
                sb.AppendLine(String.Join(",", Array.ConvertAll(Of Object, String)(nameArray, _
                                Function(o As Object) If(o.ToString.Contains(","), _
                                ControlChars.Quote & o.ToString & ControlChars.Quote, o))))
            End If
            For Each dr As DataRow In sourceTable.Rows
                sb.AppendLine(String.Join(",", Array.ConvertAll(Of Object, String)(dr.ItemArray, _
                                Function(o As Object) If(o.ToString.Contains(","), _
                                ControlChars.Quote & o.ToString & ControlChars.Quote, o.ToString))))
            Next
            System.IO.File.WriteAllText(filePathName, sb.ToString)
            Return True
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
            Return False
        End Try
    End Function

    Private Function AddValuesToTable(ByRef source() As String, ByVal destination As DataTable, Optional ByVal HeaderFlag As Boolean = False) As Boolean
        'Ensures a datatable can hold an array of values and then adds a new row 
        Try
            Dim existing As Integer = destination.Columns.Count
            If HeaderFlag Then
                Resolve_Duplicate_Names(source)
                For i As Integer = 0 To source.Length - existing - 1
                    destination.Columns.Add(source(i).ToString, GetType(String))
                Next i
                Return True
            End If
            For i As Integer = 0 To source.Length - existing - 1
                destination.Columns.Add("Column" & (existing + 1 + i).ToString, GetType(String))
            Next
            destination.Rows.Add(source)
            Return True
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
            Return False
        End Try
    End Function

    Private Sub Resolve_Duplicate_Names(ByRef source() As String)
        ' Resolves the possibility of duplicated names by appending "Duplicate Name" and a number at the end of any duplicates
        Dim i, n, dnum As Integer
        dnum = 1
        For n = 0 To source.Length - 1
            For i = n + 1 To source.Length - 1
                If source(i) = source(n) Then
                    source(i) = source(i) & "Duplicate Name " & dnum
                    dnum += 1
                End If
            Next
        Next
        Return
    End Sub

    'Private Function GetDB_Details(ByRef dbname As String) As DataSet
    '    Dim SqlString As String = ""
    '    Dim mresult As DataTable
    '    Try
    '        SqlString = "SELECT " _
    '                                    & " t.NAME AS TableName, " _
    '                                    & "  s.Name AS SchemaName," _
    '                                    & "  p.rows AS RowCounts," _
    '                                    & "  SUM(a.total_pages) * 8 AS TotalSpaceKB, " _
    '                                    & "  SUM(a.used_pages) * 8 AS UsedSpaceKB, " _
    '                                    & "  (SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS UnusedSpaceKB" _
    '                                    & " FROM " _
    '                                    & " sys.tables t " _
    '                                    & " INNER Join" _
    '                                    & "         sys.indexes i ON t.OBJECT_ID = i.object_id  " _
    '                                    & " INNER Join " _
    '                                    & "         sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id " _
    '                                    & " INNER Join " _
    '                                    & "         sys.allocation_units a ON p.partition_id = a.container_id " _
    '                                    & "     LEFT OUTER JOIN  " _
    '                                    & "         sys.schemas s ON t.schema_id = s.schema_id" _
    '                                    & " WHERE " _
    '                                    & "         t.NAME NOT LIKE 'dt%'  " _
    '                                    & "         AND t.is_ms_shipped = 0 " _
    '                                    & "         AND i.OBJECT_ID > 255  " _
    '                                    & " Group BY " _
    '                                    & " t.Name, s.Name, p.Rows " _
    '                                    & " ORDER BY " _
    '                                    & " UsedSpaceKB(desc, t.Name)"

    '        Dim dbConnection As New OleDbConnection
    '        Dim SQLconstring As String = "Escapade_NewConnectionString"
    '        Dim DatabaseConnectionString As String = ""
    '        DatabaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
    '        ' this is a shortcut for your connection string
    '        Using conn As New SqlConnection(DatabaseConnectionString)
    '            Dim cmd As New SqlCommand(SqlString, conn)
    '            cmd.Connection.Open()
    '            cmd.ExecuteNonQuery()
    '        End Using

    '    Catch ex As Exception


    '    End Try
    '    Return mresult
    'End Function
    Public Shared Function SqlReturn2(SQLconstring As String, sql As String) As Object
        Dim DatabaseConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Using conn As New SqlConnection(DatabaseConnectionString)
            conn.Open()
            Dim cmd As New SqlCommand(sql, conn)
            Dim result As Object = DirectCast(cmd.ExecuteScalar(), Object)
            Return result
        End Using
    End Function
    Public Shared Function SqlReturnWOConstring(sql As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As String
        Dim DatabaseConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Using conn As New SqlConnection(DatabaseConnectionString)
            conn.Open()
            Dim cmd As New SqlCommand(sql, conn)
            Dim result As String = DirectCast(cmd.ExecuteScalar(), Object)
            Return result
        End Using
    End Function
    'Public Shared Function SqlReturn2(SQLconstring As String) As Object
    '    Dim connetionString As String
    '    Dim connection As SqlConnection
    '    Dim adapter As New SqlDataAdapter
    '    Dim sql As String = ""
    '    connetionString = "Data Source=ServerName;Initial Catalog=DatabaseName;User ID=UserName;Password=Password"
    '    connection = New SqlConnection(connetionString)
    '    sql = "update product set product_price = 1001 where Product_name ='Product7'"
    '    Try
    '        connection.Open()
    '        adapter.UpdateCommand = connection.CreateCommand
    '        adapter.UpdateCommand.CommandText = sql
    '        adapter.UpdateCommand.ExecuteNonQuery()
    '        MsgBox("Row updated  !! ")
    '    Catch ex As Exception
    '        MsgBox(ex.ToString)
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tablebname"></param>
    ''' <param name="fieldsList"></param>
    ''' <param name="criteria">example:Dim dt As DataTable=SQLFunctions.GetDataTable(???)</param>
    ''' <returns>example: dt.Rows(0).Item("Web_price").ToString</returns>
    ''' <remarks></remarks>
    Shared Function GetDataTable(ByRef tablebname As String, ByRef fieldsList As String, ByRef criteria As String, Optional constr As String = "Escapade_NewConnectionString") As DataTable

        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings(constr).ConnectionString
        Dim sql As String = "SELECT " & fieldsList & " FROM " & tablebname & " where " & criteria

        Try
            Using myConnection As New SqlConnection(connectionString)
                Using myCommand As New SqlCommand(sql, myConnection)
                    myConnection.Open()
                    Using myReader As SqlDataReader = myCommand.ExecuteReader()
                        Dim myTable As New DataTable()
                        myTable.Load(myReader)
                        myConnection.Close()
                        Return myTable
                    End Using
                End Using
            End Using

        Catch EX As Exception
            HttpContext.Current.Response.Write(EX.Message & "<br>")
            HttpContext.Current.Response.Write("sqlfunctions.GetSearchDataSQL:fSearchCriteria : " & criteria & "   fSeekFields=" & fieldsList & "<br>")
            HttpContext.Current.Response.Write("<br>sqlfunctions.GetSearchDataSQL:SqlString : " & sql & "<br>")
            HttpContext.Current.Response.Write("<br>sqlfunctions.GetSearchDataSQL:connection : " & connectionString & "<br>")
            HttpContext.Current.Response.End()
        End Try
    End Function

    Shared Function GetDataTableFromSqlstr(ByRef strsql As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As DataTable
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Dim sql As String = strsql
        Try
            Using myConnection As New SqlConnection(connectionString)
                Using myCommand As New SqlCommand(sql, myConnection)
                    myConnection.Open()
                    Using myReader As SqlDataReader = myCommand.ExecuteReader()
                        Dim myTable As New DataTable()
                        myTable.Load(myReader)
                        myConnection.Close()
                        Return myTable
                    End Using
                End Using
            End Using
        Catch EX As Exception
            HttpContext.Current.Response.Write(EX.Message & "<br>")
            HttpContext.Current.Response.Write("ln.2019 GetDataTableFromSqlstr.sql=" & sql)
            HttpContext.Current.Response.End()
        End Try

    End Function


    Public Shared Function GetDataLDataset(ByVal fTableName As String, ByVal fSearchCriteria As String, ByVal fSeekFields As String) As DataSet
        Dim fReturnData As DataSet
        'using

        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("Escapade_NewConnectionString").ConnectionString
        Dim sConnection As String = New SqlConnection(connectionString).ToString
        fSeekFields = " and " & fSeekFields
        Dim objDataAdapter As New SqlDataAdapter("Select " & fSeekFields & " From " & fTableName & " where 1=1 " & fSeekFields, sConnection)
        Dim dsResult As New DataSet("Result")
        Try
            If Not IsNothing(objDataAdapter) Then
                ' Fill data into dataset
                objDataAdapter.Fill(dsResult)

                objDataAdapter.Dispose()
            End If

            ' inserts dataset to the returnvalue to use it with datagrid use DataGridView1.DataSource = dsResult
            fReturnData = dsResult

        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message & "<br>")
            HttpContext.Current.Response.Write("sqlfunctions.GetSearchDataSQL:fSearchCriteria : " & fSearchCriteria & "fSeekFields=" & fSeekFields & "<br>")
            'HttpContext.Current.Response.Write("<br>sqlfunctions.GetSearchDataSQL:SqlString : " & sConnection & "<br>")
            HttpContext.Current.Response.Write("<br>sqlfunctions.GetSearchDataSQL:connection : " & sConnection & "<br>")
            HttpContext.Current.Response.End()
        End Try
        Console.ReadLine()
        'End Using

        Return fReturnData
    End Function
    ''' <summary>
    ''' dt1=SQLFunctions.GetDatatablefromSQLDatasource(SqlDataSource1)
    ''' to use results try this  technique Literal1.Text = dt1.Rows(0)("Product_Name").ToString() 	%26 "^" 	%26 dt1.Rows(3)("Product_Name").ToString()
    ''' </summary>
    ''' <param name="mySQLDataSource">submit sql datasource ID as object and not as string</param>
    ''' <returns>data table created from sqldatasource</returns>
    ''' <remarks></remarks>
    Shared Function GetDataTablefromSQLDatasource(mySQLDataSource As Object) As DataTable
        Try
            Dim returnvalue As New DataTable
            Dim dv = New DataView()
            Dim dt = New DataTable()
            ' Here mySQLDataSource is the ID of SQLDataSource
            dv = TryCast(mySQLDataSource.[Select](DataSourceSelectArguments.Empty), DataView)
            dt = dv.ToTable()
            returnvalue = dt
            Return dt
        Catch ex As Exception
            Current.Response.Write(" SQlFunctions.Error GetDataTablefromSQLDatasource :")
            Current.Response.Write("<br>" & ex.Message.ToString)
            Current.Response.Write("<br> mySQLDataSourceID = " & mySQLDataSource.ID.ToString)
			Current.Response.Write("<br> mySQLDataSource.SelectCommand = " & mySQLDataSource.SelectCommand.ToString)
			
        End Try

    End Function
    ''' <summary>
    ''' LITRAL1.TEXT=SQLFunctions.GetDatatablefromSQLDatasource(SqlDataSource1)
    ''' SqlDataSource1 SHOULD NOT BE IN QUOTES </summary>
    ''' <param name="mySQLDataSource"></param>
    ''' <returns>NUMBERS IN STRING </returns>
    ''' <remarks></remarks>
    Shared Function GetRecCountfromSQLDatasource(mySQLDataSource As Object) As String
        Try
            Dim returnvalue As String
            Dim dv = New DataView()
            ' Here mySQLDataSource is the ID of SQLDataSource
            dv = TryCast(mySQLDataSource.[Select](DataSourceSelectArguments.Empty), DataView)

            returnvalue = dv.Count.ToString
            Return returnvalue
        Catch ex As Exception
            Current.Response.Write(" Error GetRecCountfromSQLDatasource :")
            Current.Response.Write("<br>" & ex.Message.ToString)
            Current.Response.Write("<br> mySQLDataSource = " & mySQLDataSource.ToString)
        End Try

    End Function
    Shared Function GetRecCountfromSQLstr(sql As Object, Optional SQLconstring As String = "Escapade_NewConnectionString") As String
        Dim DatabaseConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Try
            Using conn As New SqlConnection(DatabaseConnectionString)
                conn.Open()
                Dim cmd As New SqlCommand(sql, conn)
                Dim result As Object = DirectCast(cmd.ExecuteScalar(), Object)
                Return result
            End Using
        Catch e1 As SqlException
            HttpContext.Current.Response.Write("GetRecCountfromSQLstr.SqlException.Message" & e1.Message)
            HttpContext.Current.Response.Write("<br>GetRecCountfromSQLstr.strsql= " & sql)
            HttpContext.Current.Response.Write("<br>GetRecCountfromSQLstr.DatabaseConnectionString= " & DatabaseConnectionString)
            'HttpContext.Current.Response.End()
        Catch e2 As Exception
            HttpContext.Current.Response.Write("GetRecCountfromSQLstr.Exception.Message= " & e2.Message)
            HttpContext.Current.Response.Write("<br>GetRecCountfromSQLstr.strsql= " & sql)
            HttpContext.Current.Response.Write("<br>GetRecCountfromSQLstr.DatabaseConnectionString= " & DatabaseConnectionString)
            'HttpContext.Current.Response.End()
        End Try
    End Function
    Shared Sub ClearTable(table As DataTable)
        Try
            table.Clear()
        Catch e As DataException
            ' Process exception and return.
            Console.WriteLine("Exception of type {0} occurred.", e.GetType().ToString())
        End Try
    End Sub
    ''' <summary>
    ''' Get Primary Key Filed Name for any Table
    ''' </summary>
    ''' <param name="SQLconstring"></param>
    ''' <param name="tablename"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetPrimaryKeyFiledName(ByRef tablename As String, Optional SQLconstring As String = "Escapade_NewConnectionString")
        Dim strsql As String = " Select column_name  FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 AND table_name = " & "'" & tablename & "'"
        Dim DatabaseConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Try
            Using conn As New SqlConnection(DatabaseConnectionString)
                conn.Open()
                Dim cmd As New SqlCommand(strsql, conn)
                Dim result As Object = DirectCast(cmd.ExecuteScalar(), Object)
                If IsDBNull(result) Or IsNothing(result) Then
                    result = "NA"
                End If
                Return result
            End Using

        Catch ex As Exception
            'HttpContext.Current.Response.Write("GetPrimaryKeyFiledName ERROR : & " & strsql)
            'HttpContext.Current.Response.Write("Exception : " & ex.ToString)
        End Try
    End Function
    Shared Function searchdt2(dt2a As DataTable, ByRef FieldName As String, ByRef FieldValue As String, Optional ReturnField As String = "") As String
        Dim returnvalue As String = "NOT FOUND"
        Dim mFieldName As String = FieldName
        If Not IsDBNull(dt2a) Then
            For n = 0 To dt2a.Rows.Count - 1
                For Each dRow As DataRow In dt2a.Rows
                    If Trim(dRow(mFieldName).ToString()) = Trim(FieldValue) Then
                        returnvalue = dRow(ReturnField).ToString()
                    End If
                Next
            Next
        End If
        Return returnvalue
    End Function
    Shared Function searchDT(ByRef mSqlDataSource As SqlDataSource, ByRef FieldName As String, ByRef FieldValue As String, Optional ReturnField As String = "") As String
        Try
            Dim mFieldName As String = FieldName '"TableName"
            Dim returnvalue As String = "NOT FOUND"
            If ReturnField = "" Then
                ReturnField = FieldName
            End If
            'HttpContext.Current.Response.Write(FieldName & "-" & FieldValue & "-" & ReturnField & "<br>")
            Dim dt2a As DataTable = SQLFunctions.GetDataTablefromSQLDatasource(mSqlDataSource)
            If Not IsDBNull(dt2a) Then
                For n = 0 To dt2a.Rows.Count - 1
                    For Each dRow As DataRow In dt2a.Rows
                        If Trim(dRow(mFieldName).ToString()) = Trim(FieldValue) Then
                            returnvalue = dRow(ReturnField).ToString()
                        End If
                    Next
                Next
            End If
            Return returnvalue
        Catch ex As Exception
            'HttpContext.Current.Response.Write("searchDT" & ex.Message & "<br>")
            'HttpContext.Current.Response.Write("searchDT: " & FieldName & "=" & FieldValue & "<br>")
        End Try
    End Function

    Shared Sub GetConstrainsList(ByRef tablebane As String)
        Dim strsql As String = " Select dc.name 'Constraint Name',  OBJECT_NAME(parent_object_id) 'Table Name',  c.name 'Column Name', definition" _
                & "FROM sys.default_constraints dc " _
                & "INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id " _
                & "where OBJECT_NAME(parent_object_id) = ' " & tablebane & "'  " _
                & "ORDER BY OBJECT_NAME(parent_object_id), c.name "

        Dim strsql2 As String = " SELECT 'ALTER TABLE dbo.' + OBJECT_NAME(parent_object_id) + ' ADD CONSTRAINT ' + dc.name + ' DEFAULT(' + definition + ') FOR ' + c.name " _
                 & " FROM sys.default_constraints dc " _
                 & " INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id " _
                 & " where OBJECT_NAME(parent_object_id) = ' " & tablebane & "' "
    End Sub


    'Shared Sub GetConstrainsList()


    '    Using sourceConnection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("local_AdvWorks_ConnStr").ConnectionString)
    '        Try
    '            Dim myCommand As New SqlCommand("SELECT * FROM HumanResources.Employee", sourceConnection)

    '            Dim QueryString As String = "SELECT * FROM HumanResources.Employee"

    '            Dim da As New SqlDataAdapter(QueryString, sourceConnection)
    '            Dim ds As New DataSet()
    '            da.Fill(ds, "Employee")
    '            Dim dt As DataTable = ds.Tables(0)
    '            dt.TableName = "Employee"
    '            sourceConnection.Close()
    '            Dim destinationConnection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("local_test2_ConnStr").ConnectionString)
    '            Using bulkCopy As New SqlBulkCopy(destinationConnection.ConnectionString)
    '                bulkCopy.BatchSize = 500
    '                bulkCopy.NotifyAfter = 1000
    '                ' bulkCopy.SqlRowsCopied +=  
    '                ' new SqlRowsCopiedEventHandler(bulkCopy_SqlRowsCopied);
    '                bulkCopy.DestinationTableName = "Employee"
    '                bulkCopy.WriteToServer(dt)

    '            End Using
    '        Catch ex As Exception
    '            Dim [error] As String = ex.Message
    '        End Try
    '    End Using

    'End Sub
    ''' <summary>
    ''' Returns Rows collection into DT
    ''' </summary>
    ''' <param name="fTableName">Table Name</param>
    ''' <param name="fSearchCriteria">Search Field and value Value</param>
    ''' <param name="fSeekFields">Search Field</param>
    ''' <param name="SQLconstring">Connection string optional</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SQLNumberOfRows(fTableName As String, Optional fSearchCriteria As String = " 1='1' ", Optional SQLconstring As String = "Escapade_NewConnectionString") As String

        Dim MyConnection As OleDbConnection
        Dim MyCommand As OleDbDataAdapter
        Dim MyDataset As DataSet
        Dim MyTable As DataTable
        Dim numrows As Integer


        If Trim(SQLconstring) = "" Then
            SQLconstring = "Escapade_NewConnectionString"
        End If


        If Trim(fSearchCriteria) = "" Then
            fSearchCriteria = " 1 = '1' "
        End If
        Dim SqlString As String = "SELECT * FROM " & fTableName & " WHERE " & fSearchCriteria & ";"

        ' Create a connection to the data source. 
        Dim connect As String = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        MyConnection = New OleDbConnection(connect)


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

    Public Shared Function SQLDBtoDT(fTableName As String, Optional fSearchCriteria As String = " 1='1' ", Optional SQLconstring As String = "Escapade_NewConnectionString") As DataTable
        Try
            Dim MyConnection As OleDbConnection
            Dim MyCommand As OleDbDataAdapter
            Dim MyDataset As DataSet
            Dim MyTable As DataTable
            Dim numrows As DataTable


            If Trim(SQLconstring) = "" Then
                SQLconstring = "Escapade_NewConnectionString"
            End If

            If Trim(fSearchCriteria) = "" Then
                fSearchCriteria = " 1 = '1' "
            End If
            Dim SqlString As String = "SELECT * FROM " & fTableName & " WHERE " & fSearchCriteria & ";"

            ' Create a connection to the data source. 
            Dim connect As String = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
            MyConnection = New OleDbConnection(connect)


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
            numrows = MyTable

            Return numrows
        Catch ex As Exception
            HttpContext.Current.Response.Write("SQLDBtoDT Error: " & ex.Message & "<br>")
            HttpContext.Current.Response.Write(" fTableName: " & fTableName & " fSearchCriteria: " & fSearchCriteria & "<br>")
            HttpContext.Current.Response.End()
        End Try

    End Function

    ''' <summary>
    ''' Sorts data table adn returns datatabe using sortfield 
    ''' </summary>
    ''' <param name="DataTableToSort">Data Table to sort</param>
    ''' <param name="SortField">Field Name to sort as string</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function sortDT(ByRef DataTableToSort As DataTable, ByRef SortField As String) As DataTable
        Dim DT As DataTable = DataTableToSort
        Dim dv As DataView = DT.DefaultView
        dv.Sort = SortField
        Dim dt2 As DataTable = dv.ToTable()

        Return DT
    End Function
    ''' <summary>
    ''' converts and Returns dataview from a Data Table
    ''' </summary>
    ''' <param name="DataTableToSort">Data Table to Convert</param>
    ''' <param name="SortField">optional data sort</param>
    ''' <returns>Dataview sorted to 1</returns>
    ''' <remarks></remarks>
    Public Shared Function DTtoDataView(ByRef DataTableToSort As DataTable, Optional SortField As String = "1", Optional strfilter As String = "1=1") As DataView
        Dim DT As DataTable = DataTableToSort
        Dim dv As DataView = DT.DefaultView
        dv.Sort = SortField
        dv.RowFilter = strfilter
        Return dv
    End Function
    ''' <summary>
    ''' sorts filters aby coverting dt into dataview
    ''' </summary>
    ''' <param name="DataTableToSort"></param>
    ''' <param name="SortField"></param>
    ''' <param name="strfilter"></param>
    ''' <param name="rowstatevalue">use "Deleted","Current","Unchanged","OriginalRows"</param>
    ''' <returns> and returns processed data table</returns>
    ''' <remarks></remarks>
    Public Shared Function DTtoDataViewSortWithFilter(ByRef DataTableToSort As DataTable, Optional SortField As String = "1", Optional strfilter As String = "1=1", Optional rowstatevalue As String = Nothing) As DataView
        Dim DT As DataTable = DataTableToSort
        Dim dv As DataView = DT.DefaultView
        dv.Sort = SortField
        dv.RowFilter = strfilter
        'dv.RowStateFilter = rowstatevalue
        Return dv
    End Function
    Public Shared Function DTtoDSMerge(ByRef DataTable As DataTable, ds As DataSet) As DataSet
        Dim DT As DataTable = DataTable
        ds.Merge(DT)
        Return ds
    End Function




    Shared Sub setupsqlserveruser()
        '        -- Create user for SQL Authentication
        '        CREATE Login 'JohnJacobs' WITH PASSWORD = 'JinGleHeimerSchmidt'
        ',DEFAULT_DATABASE = [YourDatabaseHere]
        '        GO()
        '-- Now add user to database
        'USE YourDatabaseHere;
        'CREATE USER JohnJacobs FOR LOGIN JohnJacobs;
        '        GO()
        '-- If adding to a second database, do so below:
        'USE YourSecondDatabaseHere;
        'CREATE USER JohnJacobs FOR LOGIN JohnJacobs;
        '******************************
        '    -- Create user windows Authentication
        'CREATE LOGIN [YourDomainNameJohnJacobs] FROM WINDOWS
        'WITH DEFAULT_DATABASE = [YourDatabaseHere];
        'GO
        '-- Now add user to database
        'USE YourDatabaseHere;
        'CREATE USER JohnJacobs FOR LOGIN [YourDomainNameJohnJacobs];
        '-- If adding to a second database, do so below:
        'USE YourSecondDatabaseHere;
        'CREATE USER JohnJacobs FOR LOGIN [YourDomainNameJohnJacobs];
    End Sub

    Shared Function sqlDBQueriesRunning(Optional SQLconstring As String = "Escapade_NewConnectionString") As DataTable
        Dim SqlString As String = ""
        Try
            Dim MyConnection As OleDbConnection
            Dim MyCommand As OleDbDataAdapter
            Dim MyDataset As DataSet
            Dim MyTable As DataTable
            Dim returntable As DataTable


            SqlString = " select  p.spid , right(convert(varchar, dateadd(ms, datediff(ms, P.last_batch, getdate()), '1900-01-01'), 121), 12) as 'batch_duration' " _
                        & ",   P.program_name,   P.hostname ,   P.loginame from master.dbo.sysprocesses P " _
                        & " where P.spid > 50" _
                        & " and      P.status not in ('background', 'sleeping')" _
                        & " and      P.cmd not in ('AWAITING COMMAND' " _
                        & " ,'MIRROR HANDLER' " _
                        & " ,'LAZY WRITER' " _
                        & " ,'CHECKPOINT SLEEP' " _
                        & " ,'RA MANAGER') " _
                        & " order by batch_duration desc  "


            Dim connect As String = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
            MyConnection = New OleDbConnection(connect)


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
            returntable = MyTable

            Return returntable
        Catch ex As Exception
            HttpContext.Current.Response.Write("SQLDBtoDT Error: " & ex.Message & "<br>")
            HttpContext.Current.Response.Write(" SqlString: " & SqlString & "<br>")
            HttpContext.Current.Response.End()
        End Try

        ' following codes to make a loop through to knwo the queries or query running

        '        declare
        '    @spid int
        ',   @stmt_start int
        ',   @stmt_end int
        ',   @sql_handle binary(20)

        'set @spid = 20000 -- Fill this in

        'select  top 1
        '    @sql_handle = sql_handle
        ',   @stmt_start = case stmt_start when 0 then 0 else stmt_start / 2 end
        ',   @stmt_end = case stmt_end when -1 then -1 else stmt_end / 2 end
        'from    master.dbo.sysprocesses
        'where   spid = 71
        'order by ecid

        'SELECT
        '    SUBSTRING(	text,
        '    		COALESCE(NULLIF(@stmt_start, 0), 1),
        '    		CASE @stmt_end
        '    			WHEN -1
        '    				THEN DATALENGTH(text)
        '    			ELSE
        '    				(@stmt_end - @stmt_start)
        '                    End
        '    	)
        'FROM ::fn_get_sql(@sql_handle)


    End Function
    ''' <summary>
    ''' Changes sequence of columns in a datatable
    ''' </summary>
    ''' <param name="table"></param>
    ''' <param name="columnNames"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SetColumnsOrder(table As DataTable, ParamArray columnNames As [String]()) As DataTable
        For columnIndex As Integer = 0 To columnNames.Length - 1
            table.Columns(columnNames(columnIndex)).SetOrdinal(columnIndex)
        Next
        Return table
    End Function
    Shared Function DBConnectionStatus(Optional SQLconstring As String = "Escapade_NewConnectionString") As Boolean
        Try
            Dim connect As String = System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
            Using sqlConn As New SqlConnection(connect)
                sqlConn.Open()
                Return (sqlConn.State = ConnectionState.Open)
            End Using
        Catch e1 As SqlException
            Return False
        Catch e2 As Exception
            Return False
        End Try
    End Function
    Shared Function GetSQLFieldType(ByRef fieldname As String, ByRef tablename As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As String
        Dim returnvalue As String = ""
        Dim strsql As String = "Select t.Name 'Data type' FROM sys.columns c" _
                & " INNER JOIN  sys.types t ON c.user_type_id = t.user_type_id" _
                & " LEFT OUTER JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id" _
                & " LEFT OUTER JOIN sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id" _
                & " WHERE c.object_id = OBJECT_ID('" & tablename & "') and  c.name = '" & fieldname & "' ;"
        Try
            returnvalue = SqlReturn(SQLconstring, strsql).ToString
        Catch e1 As SqlException
            HttpContext.Current.Response.Write("GetSQLFieldType.SqlException.Message" & e1.Message)
            HttpContext.Current.Response.Write("GetSQLFieldType.strsql" & strsql)
            Return False
        Catch e2 As Exception
            HttpContext.Current.Response.Write("GetSQLFieldType.Exception.Message= " & e2.Message)
            HttpContext.Current.Response.Write("<br>GetSQLFieldType.strsql= " & strsql)
            HttpContext.Current.Response.Write("<br>GetSQLFieldType.connect= " & SQLconstring)
            HttpContext.Current.Response.Write("<br>GetSQLFieldType.returnvalue= " & returnvalue)
        End Try
        Return returnvalue
    End Function

    Shared Sub TrimfieldData(tablename As String, Optional filedname As String = "*", Optional SQLconstring As String = "Escapade_NewConnectionString")

    End Sub
    Shared Function SPROCtoDT(StoredProcedureName As String, Optional strRowFiter As String = "1=1", Optional SQLconstring As String = "Escapade_NewConnectionString") As DataTable
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim dv As New DataView
        'Using conn As New SqlConnection(SQLconstring)
        Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString)
                Try
                    Using da As New SqlDataAdapter()
                        da.SelectCommand = New SqlCommand(StoredProcedureName, conn)
                        da.SelectCommand.CommandType = CommandType.StoredProcedure


                        da.Fill(ds, "result_name")

                        dt = ds.Tables("result_name")

                    dt = dtclass.filterDataTable(dt, strRowFiter)

                    'manipulate your data
                    'If strRowFiter <> "1=1" Then
                    '    For Each row As DataRow In dt.Rows
                    '    If Trim(row(mFieldName).ToString()) = Trim(FieldValue) Then
                    '        returnvalue = dRow(ReturnField).ToString()
                    '    End If
                    'Next
                    'End If
                End Using
                Catch ex As SqlException
                    HttpContext.Current.Response.Write("SQL Error: " + ex.Message)
                Catch e As Exception
                    HttpContext.Current.Response.Write("Error: " + e.Message)
                End Try
            End Using
            Return dt
    End Function

    Function joindatatables(table1 As DataTable, table2 As DataTable) As DataTable
        Dim dtResult As New DataTable()
        dtResult.Columns.Add("ID", GetType(String))
        dtResult.Columns.Add("name", GetType(String))
        dtResult.Columns.Add("stock", GetType(Integer))
        Dim result = From dataRows1 In table1.AsEnumerable()
                     Join dataRows2 In table2.AsEnumerable()
                        On dataRows1.Field(Of String)("ID") Equals dataRows2.Field(Of String)("ID")
        dtResult = result
        Return dtResult
    End Function
    Function MergerDataViews(dv1 As DataView, dv2 As DataView) As DataView
        dv1.Table.Merge(dv2.Table)
        Return dv1
    End Function
    Public Shared Function JoinTwoDataTablesOnOneColumn(dtblLeft As DataTable, dtblRight As DataTable, colToJoinOn As String, Optional joinType__1 As String = "right") As DataTable
        'Change column name to a temp name so the LINQ for getting row data will work properly.
        Dim strTempColName As String = colToJoinOn & Convert.ToString("_2")
        If dtblRight.Columns.Contains(colToJoinOn) Then
            dtblRight.Columns(colToJoinOn).ColumnName = strTempColName
        End If

        'Get columns from dtblLeft
        Dim dtblResult As DataTable = dtblLeft.Clone()

        'Get columns from dtblRight
        Dim dt2Columns = dtblRight.Columns.OfType(Of DataColumn)().[Select](Function(dc) New DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping))

        'Get columns from dtblRight that are not in dtblLeft
        Dim dt2FinalColumns = From dc In dt2Columns.AsEnumerable() Where Not dtblResult.Columns.Contains(dc.ColumnName) 'dc

        'Add the rest of the columns to dtblResult
        dtblResult.Columns.AddRange(dt2FinalColumns.ToArray())

        'No reason to continue if the colToJoinOn does not exist in both DataTables.
        If Not dtblLeft.Columns.Contains(colToJoinOn) OrElse (Not dtblRight.Columns.Contains(colToJoinOn) AndAlso Not dtblRight.Columns.Contains(strTempColName)) Then
            If Not dtblResult.Columns.Contains(colToJoinOn) Then
                dtblResult.Columns.Add(colToJoinOn)
            End If
            Return dtblResult
        End If

        Select Case joinType__1

            Case "Inner"
                '#Region "Inner"
                'get row data
                'To use the DataTable.AsEnumerable() extension method you need to add a reference to the System.Data.DataSetExtension assembly in your project. 
                Dim rowDataLeftInner As Array = From rowLeft In dtblLeft.AsEnumerable()
                                                Join rowRight In dtblRight.AsEnumerable() On rowLeft(colToJoinOn) Equals rowRight(strTempColName)
                                                Select rowLeft.ItemArray.Concat(rowRight.ItemArray).ToArray()


                'Add row data to dtblResult
                For Each values As Object() In rowDataLeftInner
                    dtblResult.Rows.Add(values)
                Next

                '#End Region
                Exit Select
            Case "Left"
                '#Region "Left"
                Dim rowDataLeftOuter As Array = From rowLeft In dtblLeft.AsEnumerable()
                                                Group Join rowRight In dtblRight.AsEnumerable() On rowLeft(colToJoinOn) Equals rowRight(strTempColName) Into gj = Group
                                                From subRight In gj.DefaultIfEmpty()
                                                Select rowLeft.ItemArray.Concat(If((subRight Is Nothing), (dtblRight.NewRow().ItemArray), subRight.ItemArray)).ToArray()


                'Add row data to dtblResult
                For Each values As Object() In rowDataLeftOuter
                    dtblResult.Rows.Add(values)
                Next

                '#End Region
                Exit Select

            Case Else
                '#Region "Inner"
                'get row data
                'To use the DataTable.AsEnumerable() extension method you need to add a reference to the System.Data.DataSetExtension assembly in your project. 
                Dim rowDataLeftInner As Array = From rowLeft In dtblLeft.AsEnumerable()
                                                Join rowRight In dtblRight.AsEnumerable() On rowLeft(colToJoinOn) Equals rowRight(strTempColName)
                                                Select rowLeft.ItemArray.Concat(rowRight.ItemArray).ToArray()


                'Add row data to dtblResult
                For Each values As Object() In rowDataLeftInner
                    dtblResult.Rows.Add(values)
                Next

                '#End Region
                Exit Select


        End Select

        'Change column name back to original
        dtblRight.Columns(strTempColName).ColumnName = colToJoinOn

        'Remove extra column from result
        dtblResult.Columns.Remove(strTempColName)

        Return dtblResult
    End Function

    Function createsqldatasource(ByRef SqlDatasourceName As String, ByRef SelectCommand As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As SqlDataSource
        Dim SqlDataSource1 As New SqlDataSource
        Dim connect As String = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        SqlDataSource1.ID = SqlDatasourceName
        SqlDataSource1.ConnectionString = connect
        SqlDataSource1.SelectCommand = SelectCommand
        Return SqlDataSource1
    End Function
    Shared Function filterDataTable(DataTable As DataTable, expression As String) As DataTable
        DataTable.DefaultView.RowFilter = expression
        Return DataTable
    End Function
    Private Function MakeDataRelation(parenttable As DataTable, childtable As DataTable, ds As String, parentcolumnName As String, ChildColumnName As String) As DataRelation
        ' DataRelation requires two DataColumn 
        ' (parent and child) and a name.
        Dim parentColumn As DataColumn = parenttable.Columns(parentcolumnName)
        Dim childColumn As DataColumn = childtable.Columns(ChildColumnName)
        Dim relation As DataRelation = New DataRelation(ds, parentColumn, childColumn)
        Return relation
    End Function

    Shared Function maketestdatatable(Optional DTName As String = "DTTest") As DataTable
        Dim returnvalue As New DataTable
        ' Create a new DataTable.
        Dim table As DataTable = New DataTable(DTName)
        Dim column As DataColumn
        Dim row As DataRow

        ' Create first column and add to the DataTable.
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.Int32")
        column.ColumnName = "ChildID"
        column.AutoIncrement = True
        column.Caption = "ID"
        column.ReadOnly = True
        column.Unique = True

        ' Add the column to the DataColumnCollection.
        table.Columns.Add(column)

        ' Create second column.
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.String")
        column.ColumnName = "ChildItem"
        column.AutoIncrement = False
        column.Caption = "ChildItem"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        ' Create third column.
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.Int32")
        column.ColumnName = "ParentID"
        column.AutoIncrement = False
        column.Caption = "ParentID"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        ' Create three sets of DataRow objects, five rows each, 
        ' and add to DataTable.

        Dim i As Integer
        For i = 0 To 4
            row = table.NewRow()
            row("childID") = i
            row("ChildItem") = "Item " + i.ToString()
            row("ParentID") = 0
            table.Rows.Add(row)
        Next i
        For i = 0 To 4
            row = table.NewRow()
            row("childID") = i + 5
            row("ChildItem") = "Item " + i.ToString()
            row("ParentID") = 1
            table.Rows.Add(row)
        Next i
        For i = 0 To 4
            row = table.NewRow()
            row("childID") = i + 10
            row("ChildItem") = "Item " + i.ToString()
            row("ParentID") = 2
            table.Rows.Add(row)
        Next i

        returnvalue = table


        Return returnvalue

    End Function
    Shared Function GetDataTableFieldList(ByVal dt As DataTable) As String
        'Dim strSQL As String
        Dim i As Integer
        'Dim mDbFolder As String = "App_Data/"
        Dim TableFiledlist As String = ""
        ' Create a connection to the data source. 
        'Dim MyConnection As OleDbConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Current.Server.MapPath(mDbFolder & fDBName))
        'MyConnection.Open()
        'strSQL = "SELECT * FROM " & fTableName
        ' Create a Command object with the SQL statement.

        Dim dtReader As DataTableReader = GetDataReaderFromDataTable(dt)
        'Dim objCmd As New OleDbCommand(strSQL, MyConnection)
        'dtReader = objCmd.ExecuteReader()

        TableFiledlist = "<b>Table has " & dtReader.FieldCount & " Fields</b>"

        For i = 0 To dtReader.FieldCount - 1
            TableFiledlist = TableFiledlist + "<br>" & dtReader.GetName(i)
        Next
        dtReader.Close()
        dtReader = Nothing
        Return TableFiledlist
    End Function

    Shared Function GetDataReaderFromDataTable(ByVal dt As DataTable) As DataTableReader
        ' Given a DataTable, retrieve a DataTableReader
        ' allowing access to all the tables's data:
        Dim DTR As DataTableReader = dt.CreateDataReader()
        Return DTR
    End Function
    Public Shared Function ConvertListToDataTable(Of T)(ByVal list As IList(Of T)) As DataTable
        Dim table As New DataTable()
        Dim fields() As FieldInfo = GetType(T).GetFields()
        For Each field As FieldInfo In fields
            table.Columns.Add(field.Name, field.FieldType)
        Next
        For Each item As T In list
            Dim row As DataRow = table.NewRow()
            For Each field As FieldInfo In fields
                row(field.Name) = field.GetValue(item)
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function

    'Public Shared Function ListToDT(Of T)(list As IList(Of T)) As DataTable
    '    Dim table As New DataTable()
    '    Dim row As DataRow = table.NewRow()
    '    For i As Integer = 0 To list.Count - 1
    '        '            For Each item As T In list
    '        For Each field As FieldInfo In fields
    '            row(1) = field.GetValue(item)
    '        Next
    '        table.Rows.Add(row)
    '        '           Next
    '    Next

    '    Return table
    'End Function
    Public Shared Function GetDistinctRecords(dt As DataTable, Columns As String) As DataTable
        Dim dtUniqRecords As New DataTable()
        dtUniqRecords = dt.DefaultView.ToTable(True, Columns)
        Return dtUniqRecords
    End Function
    ''' <summary>
    ''' returns 
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="FieldToString">Filedname to be converted to string</param>
    ''' <returns></returns>
    Shared Function GetStringFromDataTable(dt As DataTable, FieldToString As String) As String
        Dim Tabletostring As String = ""
        Dim NoOfRws As Integer = dt.Rows.Count

        If NoOfRws > 0 Then
            For Each row In dt.Rows
                Tabletostring = Tabletostring & row("Order_id") & ","
            Next
        End If
        ' If Right(Trim(Tabletostring), 1) = "," Then
        Tabletostring = TextCSVClass.RemoveEndCharacters(Tabletostring, True)
        ' End If
        Return Tabletostring


    End Function

    ''' <summary>
    ''' To workoput if some dataexist in result of running a query
    ''' </summary>
    ''' <param name="strsql">SQL String</param>
    ''' <returns>Boolean figure if data exists in executed SQL query</returns>
    Shared Function ifexists_Boolean(strsql As String) As Boolean
        Return SQLFunctions.GetSearchDataSQLWithOutConStr("if ( EXISTS  (" & strsql & ")) begin select 'True' end else begin select 'false' end ;")
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strsql"></param>
    ''' <param name="SQLconstring"></param>
    ''' <returns>Returns String Value</returns>
    Shared Function ifexists(strsql As String, Optional SQLconstring As String = "Escapade_NewConnectionString") As String
        Return SqlReturnWOConstring(strsql, SQLconstring)
    End Function

    Public Shared Function CreateSQLTABLE(tableName As String, table As DataTable) As String
        Dim sqlsc As String
        sqlsc = (Convert.ToString("CREATE TABLE ") & tableName) + "("
        For i As Integer = 0 To table.Columns.Count - 1
            sqlsc += vbLf & " [" + table.Columns(i).ColumnName + "] "
            Dim columnType As String = table.Columns(i).DataType.ToString()
            Select Case columnType
                Case "System.Int32"
                    sqlsc += " int "
                    Exit Select
                Case "System.Int64"
                    sqlsc += " bigint "
                    Exit Select
                Case "System.Int16"
                    sqlsc += " smallint"
                    Exit Select
                Case "System.Byte"
                    sqlsc += " tinyint"
                    Exit Select
                Case "System.Decimal"
                    sqlsc += " decimal "
                    Exit Select
                Case "System.DateTime"
                    sqlsc += " datetime "
                    Exit Select
                Case "System.String"
                    sqlsc += String.Format(" nvarchar({0}) ", If(table.Columns(i).MaxLength = -1, "max", table.Columns(i).MaxLength.ToString()))
                    Exit Select
                Case Else
                    sqlsc += String.Format(" nvarchar({0}) ", If(table.Columns(i).MaxLength = -1, "max", table.Columns(i).MaxLength.ToString()))
                    Exit Select
            End Select
            If table.Columns(i).AutoIncrement Then
                sqlsc += " IDENTITY(" + table.Columns(i).AutoIncrementSeed.ToString() + "," + table.Columns(i).AutoIncrementStep.ToString() + ") "
            End If
            If Not table.Columns(i).AllowDBNull Then
                sqlsc += " NOT NULL "
            End If
            sqlsc += ","
        Next
        Return sqlsc.Substring(0, sqlsc.Length - 1) + vbLf & ")"
    End Function
    Shared Function IfSQLTableExists(tablename As String) As String
        Dim returnvalse As String = False
        Dim strsql As String = "IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE  TABLE_NAME = '" & tablename & "')) BEGIN select 'False' END  else begin select 'True' end;"
        returnvalse = GetSearchDataSQLWithOutConStr(strsql)
        Return returnvalse
    End Function
    Shared Function AddColumnToDataTable(dt As DataTable, NewColumnName As String, Optional ColumnValue As String = "") As DataTable
        Dim newColumn As New Data.DataColumn(NewColumnName, GetType(System.String))
        newColumn.DefaultValue = ColumnValue
        dt.Columns.Add(newColumn)
        Return dt
    End Function
    Shared Function AddDTtoSQLTable(dt As DataTable, Optional filename As String = "", Optional NewSQLlTable As String = "tbl_DATA_Import_01_tmp", Optional truncatetable As Boolean = False, Optional CheckRemoveDuplicationfield As String = "") As Integer
        Dim returnvalue As Integer = 0
        Dim NumberOfFields As Integer = 0
        Dim containscolumn As String = ""
        Dim checkifSQLTableexists As String = ""
        Try
 'if 1 = 1 then HttpContext.Current.Response.Write("<br>filename=" & filename & " <br>Column Names" & DTClass.GetDTColumnNames(dt))
            containscolumn = DTContainColumn(filename, dt)
            '       Dim NewSQLlTable As String = "tbl_WH_DailyInventotyImport_tmp"

            If DTContainColumn("File_Name", dt) <> "File_Name" Then
                dt = SQLFunctions.AddColumnToDataTable(dt, "File_Name", filename)
            End If


            If NewSQLlTable = "" Then
                NewSQLlTable = "tbl_" & Replace(filename, ".", "_") & "_tmp"
            End If


            checkifSQLTableexists = SQLFunctions.IfSQLTableExists(NewSQLlTable)
            If checkifSQLTableexists = "False" Then
                Dim sqlstrCreateTable As String = SQLFunctions.CreateSQLTABLE(NewSQLlTable, dt)
                SQLFunctions.RunSQLStringWOConstring(sqlstrCreateTable)
            Else
                If truncatetable = True Then
                    SQLFunctions.RunSQLStringWOConstring("truncate table " & NewSQLlTable & " ;")
                End If
                Dim isProcessed As String = SQLFunctions.GetSearchDataSQLWithOutConStr("if ( EXISTS  (select * from " & NewSQLlTable & " where File_name = '" & filename & "' )) begin select 'True' end else begin select 'false' end ;")
                ' litResponse.Text = isProcessed

                If isProcessed = "false" Then

                    SQLFunctions.SaveDTtoSqlBulk(dt, NewSQLlTable)
                    returnvalue = dt.Rows.Count.ToString

                End If
            End If



            dt = Nothing

        Catch ex As Exception
            HttpContext.Current.Response.Write("AddDTtoSQLTable Error= " & ex.Message.ToString)
            HttpContext.Current.Response.Write("<br>checkifSQLTableexists=" & checkifSQLTableexists)
            HttpContext.Current.Response.Write("<br>NewSQLlTable=" & NewSQLlTable)
            HttpContext.Current.Response.Write("<br>containscolumn=" & containscolumn)
            HttpContext.Current.Response.End()
        End Try
        Return returnvalue
    End Function
	Shared Function AddDTtoSQLTable2(dt As DataTable, Optional filename As String = "", Optional NewSQLlTable As String = "tbl_DATA_Import_01_tmp", Optional truncatetable As Boolean = False) As Integer
			Dim returnvalue As Integer = 0
			Dim NumberOfFields As Integer = 0
			Dim containscolumn As String = ""
			Try

				containscolumn = DTContainColumn(filename, dt)
				'       Dim NewSQLlTable As String = "tbl_WH_DailyInventotyImport_tmp"

				If DTContainColumn("File_Name", dt) <> "File_Name" Then
					dt = SQLFunctions.AddColumnToDataTable(dt, "File_Name", filename)
				End If


				If NewSQLlTable = "" Then
					NewSQLlTable = "tbl_" & Replace(filename, ".", "_") & "_tmp"
				End If

				Dim isProcessed As String = SQLFunctions.GetSearchDataSQLWithOutConStr("if ( EXISTS  (select * from " & NewSQLlTable & " where File_name = '" & filename & "' )) begin select 'True' end else begin select 'false' end ;")
				' litResponse.Text = isProcessed

				If isProcessed = "false" Then
					Dim checkifSQLTableexists As String = SQLFunctions.IfSQLTableExists(NewSQLlTable)
					If checkifSQLTableexists = "False" Then
						Dim sqlstrCreateTable As String = SQLFunctions.CreateSQLTABLE(NewSQLlTable, dt)
						SQLFunctions.RunSQLStringWOConstring(sqlstrCreateTable)
					Else
						If truncatetable = True Then
							SQLFunctions.RunSQLStringWOConstring("truncate table " & NewSQLlTable & " ;")
						End If

					End If
					SQLFunctions.SaveDTtoSqlBulk(dt, NewSQLlTable)
					returnvalue = dt.Rows.Count.ToString

				End If
				dt = Nothing

			Catch ex As Exception
				HttpContext.Current.Response.Write("AddDTtoSQLTable" & ex.Message.ToString)
				HttpContext.Current.Response.Write(containscolumn)
				HttpContext.Current.Response.End()
			End Try
			Return returnvalue
		End Function
		Shared Function DTContainColumn(columnName As String, table As DataTable) As String
			Dim returnvalue As String = "0"
			Dim columns As DataColumnCollection = table.Columns
			If columns.Contains(columnName) Then
				returnvalue = columnName
			End If
			Return returnvalue
		End Function
    ''' <summary>
    ''' returns 
    ''' </summary>
    ''' <param name="dt2">Data tavke to apply filter to</param>
    ''' <param name="RemoveItfilter">simple filter like City <> 'London' </param>
    ''' <returns>takes a datatable applys filter and returnd datatable after removing rows</returns>
    Shared Function DeleteFromDataTable(dt2 As DataTable, RemoveItfilter As String) As DataTable
        dt2 = SQLFunctions.DTtoDataView(dt2, "", RemoveItfilter).ToTable()
        Return dt2
    End Function
    Shared Function GetConnectionString(ByVal name As String) As String
        Try
            Return ConfigurationManager.ConnectionStrings(name).ConnectionString
        Catch ex As Exception
            ' Handle the error appropriately (e.g., log the error)
            Return String.Empty
        End Try
    End Function
    Shared Function GetDatabaseNameFromConfig(ConnectionStringName As String) As String
        ' Retrieve the connection string from the web.config file
        Dim connectionString As String = ConfigurationManager.ConnectionStrings(ConnectionStringName).ConnectionString

        ' Initialize an empty database name
        Dim databaseName As String = String.Empty

        ' Parse the connection string
        Dim builder As New SqlConnectionStringBuilder(connectionString)

        ' Retrieve the database name from the parsed connection string
        databaseName = builder.InitialCatalog

        Return databaseName
    End Function
    ''' <summary>
    ''' converts single record intie s string dictionary to be used and variables
    ''' </summary>
    ''' <param name="queryString">SQL query to be preocessed for a record</param>
    ''' <param name="connstring">connectionstring</param>
    ''' <returns>Usage Dim result As Dictionary(Of String, Object) = ExecuteQuery(queryString)##Dim variable1 As Object = result("Column1") ' Replace "Column1" with the actual column name</returns>
    Shared Function GetVariblesFromSQLQuery(queryString As String, Optional connstring As String = "DSTConnectionString") As Dictionary(Of String, Object)
        ' Define a dictionary to store column values
        Dim recordValues As New Dictionary(Of String, Object)

        ' Define your SQL Server connection string
        Dim connectionString As String = SQLFunctions.GetConnectionString(connstring)

        Try
            ' Establish the SQL connection
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Create the SQL command
                Using command As New SqlCommand(queryString, connection)
                    ' Execute the query
                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            ' Loop through each column in the record
                            For i As Integer = 0 To reader.FieldCount - 1
                                ' Add column name and value to the dictionary
                                recordValues.Add(reader.GetName(i), reader.GetValue(i))
                            Next
                        Else
                            Console.WriteLine("No records found.")
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error executing query: " & ex.Message)
        End Try

        ' Return the dictionary containing column values
        Return recordValues
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sqlQuery">ie:SELECT ISNULL(MAX(TicketID), 0) + 1 FROM [DST].[dbo].[tblTicket];</param>
    ''' <param name="connectionStringName"></param>
    ''' <returns></returns>
    Shared Function GetValueFromDatabase(ByVal sqlQuery As String, Optional ByVal connectionStringName As String = "DSTConnectionString") As String
        Dim result As String = String.Empty

        Try
            ' Get the connection string from configuration
            Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings(connectionStringName).ConnectionString

            ' Establish the SQL connection
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Create the SQL command
                Using command As New SqlCommand(sqlQuery, connection)
                    ' Execute the query and get the result
                    Dim queryResult As Object = command.ExecuteScalar()

                    ' Check if the result is not null and convert it to a trimmed string
                    If queryResult IsNot Nothing AndAlso queryResult IsNot DBNull.Value Then
                        result = queryResult.ToString().Trim()
                    End If
                End Using
            End Using

        Catch ex As Exception
            ' Handle exceptions (log or rethrow, depending on your needs)
            Throw New Exception("An error occurred while retrieving the value from the database: " & ex.Message, ex)
        End Try

        ' Return the result as a trimmed string
        Return result
    End Function

End Class
