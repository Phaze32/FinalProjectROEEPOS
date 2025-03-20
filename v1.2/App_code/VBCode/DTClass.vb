Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Linq
Imports System.Data.SqlClient
Imports System.Reflection

Public Class DTClass
    Public Shared linenumber As Integer = 3

    Shared Function SetColumnsOrder(DT As DataTable, ParamArray columnNames As [String]()) As DataTable
        Dim columnIndex As Integer = 0
        For Each columnName As String In columnNames
            DT.Columns(columnName).SetOrdinal(columnIndex)
            ' HttpContext.Current.Response.Write(columnName.ToString & ", columnIndex=" & columnIndex)
            columnIndex = columnIndex + 1
        Next
        Return DT
    End Function
    ''' <summary>
    ''' Changes the Name of a Data table Coulmn and Replaces with New 
    ''' </summary>
    ''' <param name="dt">datatable</param>
    ''' <param name="NameFrom">Name to be replaced as string</param>
    ''' <param name="NameTo">New name as string</param>
    ''' <returns>Returns Data Table with Nwe Name</returns>
    Shared Function ReNameDTColumn(dt As DataTable, NameFrom As String, NameTo As String) As DataTable
        dt.Columns(NameFrom).ColumnName = NameTo
        Return dt
    End Function
    Shared Function AddColumnToDataTable(dt As DataTable, NewColumnName As String, Optional ColumnValue As String = "") As DataTable
        Dim objecttype As String : If TypeOf dt Is DataTable Then objecttype = ("is DataTable") Else objecttype = ("is NOT a DataTable")
        Dim objectreference As String = IsReference(dt).ToString : linenumber = 30
        Dim colcount As String = "00"
        Try
            Dim newColumn As New Data.DataColumn(NewColumnName, GetType(System.String)) : linenumber = 32
            colcount = dt.Rows.Count.ToString : linenumber = 33
            newColumn.DefaultValue = ColumnValue : linenumber = 35
            dt.Columns.Add(newColumn) : linenumber = 36
            'HttpContext.Current.Response.Write("<br>AddColumnToDataTable." & NewColumnName & ".ColumnValue=" & ColumnValue)
            'HttpContext.Current.Response.Write("<br>AddColumnToDataTable.dt.Rows.Count." & dt.Rows.Count.ToString())
        Catch ex As Exception
            HttpContext.Current.Response.Write("<br>Fn AddColumnToDataTable=" & ex.Message.ToString & "<br>")
            HttpContext.Current.Response.Write("linenumber=" & linenumber & "<br>")
            HttpContext.Current.Response.Write("AddColumnToDataTable=" & NewColumnName & "<br>")
            HttpContext.Current.Response.Write("objectreference=" & objectreference.ToString & "<br>")
            HttpContext.Current.Response.Write("objecttype=" & objecttype & "<br>")
            HttpContext.Current.Response.Write("colcount=" & colcount & "<br>")
            HttpContext.Current.Response.Write("ColumnValue=" & ColumnValue)
        Finally

        End Try

        Return dt
    End Function
    Shared Function AddIndexToDataTable(dt As DataTable) As DataTable
        dt.Columns.Add(New DataColumn("SNo"))
        Dim RowIndex As Int16 = 1
        For Each row In dt.Rows
            row("SNo") = RowIndex
            RowIndex = RowIndex + 1
        Next
        dt = DTClass.SetColumnsOrder(dt, "SNo")
        Return dt
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
    Shared Function RowsinDataTable(dt As DataTable) As Integer
        Return dt.Rows.Count
    End Function
    Shared Function ColumnsinDataTable(dt As DataTable) As Integer
        Return dt.Columns.Count
    End Function
    Shared Function DTColumnNamesFromDataset(dataSet As DataSet, Optional separator As String = ",") As String
        Dim rv As String = ""
        Dim table As DataTable
        Dim column As DataColumn

        ' For each DataTable, print the ColumnName.
        For Each table In dataSet.Tables
            For Each column In table.Columns
                rv = rv & (column.ColumnName) & separator
            Next
        Next
        Return rv
    End Function
    Shared Function GetDTColumnNames(dt As DataTable, Optional separator As String = ",") As String
        Dim rv As String = ""
        Dim column As DataColumn

        ' For each DataTable, print the ColumnName.
        For Each column In dt.Columns
            rv = rv & (column.ColumnName) & separator
        Next
        Return rv
    End Function
    Shared Function RowsinTable(dt As DataTable) As Integer
        Return dt.Rows.Count
    End Function
    Shared Function GetTableNames(dataSet As DataSet, Optional separator As String = ",") As String
        ' Print each table's TableName.
        Dim rv As String = ""
        Dim table As DataTable
        For Each table In dataSet.Tables
            rv = rv & (table.TableName)
        Next table
        Return rv
    End Function
    Shared Function ifDTColumnExists(dt As DataTable, columntocheck As String) As Boolean
        Dim rv As Boolean = False
        Dim ColumnString As String = GetDTColumnNames(dt)
        If ColumnString.Contains(columntocheck) Then
            rv = True
        Else
            rv = False
        End If
        Return rv
    End Function

    Shared Function SearchDT(dt As DataTable, returnfield As String, searchfield As String, Optional searchvalue As String = "") As String
        Dim rv As String = "na"
        If ifDTColumnExists(dt, searchfield) Then
            For Each row In dt.Rows
                If row(searchfield) = searchvalue Then
                    rv = row(returnfield)
                End If
            Next
        End If
        Return rv
    End Function
    Shared Function filterDataTable(DataTable As DataTable, expression As String) As DataTable
        DataTable.DefaultView.RowFilter = expression
        Return DataTable
    End Function
    Shared Function FilterDataTableByRow(DT As DataTable, fieldName As String, filedVale As String, Optional str_operator As String = "=") As DataTable
        ' DataTable.DefaultView.RowFilter = expression
        Dim strRowFiter As String = ""
        If fieldName <> "" And filedVale <> "" Then
            strRowFiter = fieldName & str_operator & filedVale
        Else
            strRowFiter = " 1=1 "
        End If

        Dim dt2 As New DataTable
        dt2 = DT.Clone()
        dt2.Clear()
        'manipulate your data
        If strRowFiter <> "" Then
            For Each row As DataRow In DT.Rows
                If Trim(row(fieldName).ToString()) = Trim(filedVale) Then
                    dt2.Rows.Add(row.ItemArray)
                End If
            Next
        End If

        Return dt2
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dt">DataTable</param>
    ''' <param name="expression">like Sort order to descending [Col Name/Number] DESC "dateadded desc"</param>
    ''' <returns>Data table sorted by the column</returns>
    Shared Function DataTableSortOrdrBy(dt As DataTable, expression As String) As DataTable
        Try
            Dim dv As DataView = dt.DefaultView
            dv.Sort = (expression)
            dt = dv.Table

        Catch ex As Exception
            HttpContext.Current.Response.Write("<br>DataTableSortOrdrBy.error=" & ex.Message.ToString)
            HttpContext.Current.Response.Write("<br>expression=" & expression & "<br>")
        End Try
        Return dt
    End Function

    Shared Function DTChangeColumnName(DataTable As DataTable, columnIndex As Integer, NewCollumnname As String) As DataTable
        DataTable.Columns(columnIndex).ColumnName = NewCollumnname
        Return DataTable
    End Function
    Shared Function DTRemoveColumn(DataTable As DataTable, CollumnnameToRemove As String) As DataTable
        DataTable.Columns.Remove(CollumnnameToRemove)
        Return DataTable
    End Function

    Shared Function DataReadertoDataTable(dr As SqlDataReader) As DataTable
        Dim RV As New DataTable
        Dim conn As SqlConnection = Nothing
        Try
            Dim dt As DataTable = New DataTable()
            dt.Load(dr)
            RV = dt
            Return RV
        Catch ex As Exception
            HttpContext.Current.Response.Write("<br>DateReadertoDataTable.error=" & ex.Message.ToString & "<br>")
        Finally
            conn.Close()
        End Try
    End Function
    Public Shared Function GetDistinctRecords(dt As DataTable, Columns As String) As DataTable
        Dim dtUniqRecords As New DataTable()
        dtUniqRecords = dt.DefaultView.ToTable(True, Columns)
        Return dtUniqRecords
    End Function
    Shared Function GetDataTableFromDataset(ds As DataSet, Tablename As String) As DataTable
        Dim rv As DataTable
        Dim dt1 As DataTable = ds.Tables(Tablename)
        rv = dt1
        Return rv
    End Function
    Shared Function GetFieldValuesInCSVSteing(dt As DataTable, fieldname As String) As String
        Dim RV As String = ""
        Dim dttest As DataTable = DTClass.GetDistinctRecords(dt, "OrderRef1")
        For Each row In dttest.Rows
            RV = RV & ("'" & row("OrderRef1") & "',")
        Next
        RV = TextCSVClass.RemoveEndCharacters(RV, 1)
        Return RV
    End Function
    Shared Function DistinctRowsFromDT(DT As DataTable, Strcolumnlist As String) As DataTable
        Dim View As DataView = New DataView(DT)
        Dim distinctValuesTable As DataTable = View.ToTable(True, Strcolumnlist)
        Return distinctValuesTable
    End Function
    Shared Function DTContainColumn(ByVal table As DataTable, ByVal columnName As String) As Boolean
        Dim rv As Boolean = False
        Dim columns As DataColumnCollection = table.Columns
        If columns.Contains(columnName) Then rv = True
        Return rv
    End Function
    Shared Function GetGridViewfromDT(dt As DataTable, Optional gridviewcaption As String = "", Optional backcolour As Boolean = True, Optional captionalign As String = "Center") As GridView
        Dim rv As New GridView
        rv.DataSource = dt
        rv.Caption = gridviewcaption
        'rv.CaptionAlign = "Left"
        rv.HeaderStyle.BackColor = Drawing.Color.LightGray
        rv.HeaderStyle.ForeColor = Drawing.Color.Black
        If backcolour = True Then rv.AlternatingRowStyle.BackColor = Drawing.Color.Aquamarine

        rv.DataBind()

        Return rv
    End Function

    ''' <summary>
    ''' saves DataTable into SQL table, incase SQL table is not there, function will creates SQL Database table
    ''' </summary>
    ''' <param name="dt">Data Table To save</param>
    ''' <param name="filename">File Name to check if Processed, incase No NewtableName given, This Value is Used For New table Name</param>
    ''' <param name="NewSQLlTable">Sql Table to save datatable to, System wil create the table if missing</param>
    ''' <param name="truncatetable">if previous data in saveto tabel has to be truncated</param>
    ''' <param name="searchintofieldName"></param>
    ''' <returns>No of records saved</returns>
    Shared Function SaveDTtoSQLTable(dt As DataTable, Optional filename As String = "", Optional NewSQLlTable As String = "", Optional truncatetable As Boolean = False, Optional searchintofieldName As String = "File_name") As Integer
        Dim returnvalue As Integer = 0
        '** checks and adds field filename to add checkupfiled
        If Not ifDTColumnExists(dt, searchintofieldName) Then
            dt = SQLFunctions.AddColumnToDataTable(dt, searchintofieldName, filename) '** adds field filename if n missing
        End If
        If NewSQLlTable = "" Then
            NewSQLlTable = "tbl_" & Replace(filename, ".", "_") & "_tmp" : linenumber = 414
        End If
        '** if table needs truncations
        If truncatetable = True Then SQLFunctions.RunSQLStringWOConstring("truncate table " & NewSQLlTable & " ;") : linenumber = 417

        '** Cehcks and creates Table if needed
        Dim CheckifSQLTableExists As String = SQLFunctions.IfSQLTableExists(NewSQLlTable) ' Checks if newtable exists
        If CheckifSQLTableExists = "False" Then
            Dim sqlstrCreateTable As String = SQLFunctions.CreateSQLTABLE(NewSQLlTable, dt) : linenumber = 422
            SQLFunctions.RunSQLStringWOConstring(sqlstrCreateTable) : linenumber = 423
            CheckifSQLTableExists = "True" : linenumber = 424
        End If

        '** saves data into sqlTable
        If CheckifSQLTableExists Then SQLFunctions.SaveDTtoSqlBulk(dt, NewSQLlTable) : linenumber = 428
        Dim strsql_isprocessed As String = "if  EXISTS  (select * from " & NewSQLlTable & " where " & searchintofieldName & "  = '" & filename & "' ) begin select 'True' end else begin select 'false' end ;"
        Dim isProcessed As String = SQLFunctions.GetSearchDataSQLWithOutConStr(strsql_isprocessed)
        returnvalue = dt.Rows.Count.ToString
        dt = Nothing

        Return returnvalue
    End Function
    Public Shared Function CreateSQLTABLEFromDT(tableName As String, table As DataTable) As String
        Dim RV As String = ""

        RV = SQLFunctions.CreateSQLTABLE(tableName, table)
        Return RV
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
            HttpContext.Current.Response.Write(" Error DTClass.GetDataTablefromSQLDatasource :")
            HttpContext.Current.Response.Write("<br>" & ex.Message.ToString)
            ' HttpContext.Current.Response.Write("<br> mySQLDataSource = " & mySQLDataSource.ToString)
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dt">Data Table to be processed</param>
    ''' <param name="filename">Table Name to be Created</param>
    ''' <param name="NewSQLlTable">Table Name to be Created if empty it takes value from the filename</param>
    ''' <param name="truncatetable">if old table has to be truncated before saving new data</param>
    ''' <returns></returns>
    Shared Function SaveDTtoSQLTable_ForceTrough(dt As DataTable, Optional filename As String = "", Optional NewSQLlTable As String = "", Optional truncatetable As Boolean = False) As Integer
        Dim returnvalue As Integer = 0
        ''** checks and adds field filename to add checkupfiled
        'If Not ifDTColumnExists(dt, searchintofieldName) Then
        '    dt = SQLFunctions.AddColumnToDataTable(dt, searchintofieldName, filename) '** adds field filename if n missing
        'End If
        If NewSQLlTable = "" Then
            NewSQLlTable = "tbl_" & Replace(filename, ".", "_") & "_tmp" : linenumber = 460
        End If


        '** Cehcks and creates Table if needed
        Dim CheckifSQLTableExists As String = SQLFunctions.IfSQLTableExists(NewSQLlTable) ' Checks if newtable exists
        If CheckifSQLTableExists = "False" Then
            Dim sqlstrCreateTable As String = SQLFunctions.CreateSQLTABLE(NewSQLlTable, dt) : linenumber = 467
            SQLFunctions.RunSQLStringWOConstring(sqlstrCreateTable) : linenumber = 468
            CheckifSQLTableExists = "True" : linenumber = 469
        Else
            '** if table needs truncations
            If truncatetable = True Then SQLFunctions.RunSQLStringWOConstring("truncate table " & NewSQLlTable & " ;") : linenumber = 472
        End If

        '** saves data into sqlTable
        SQLFunctions.SaveDTtoSqlBulk(dt, NewSQLlTable) : linenumber = 476

        returnvalue = dt.Rows.Count.ToString
        dt = Nothing

        Return returnvalue
    End Function
    Shared Function GetRunningTotalinDT(dt As DataTable, SourseFieldName As String, Optional RuningtotalFieldName As String = "Running_total") As DataTable
        Dim rumnningTotal As Integer = 0
        For Each row In dt.Rows
            rumnningTotal = rumnningTotal + row(RuningtotalFieldName) + row(SourseFieldName)
            row(RuningtotalFieldName) = rumnningTotal
        Next
        Return dt
    End Function

    ''' <summary>
    ''' Removes columns with along with data and keeps only required Columns
    ''' </summary>
    ''' <param name="dt">datatable to purge</param>
    ''' <param name="ColumnsToKeep">Columns to Keep eg: "Product_id,Product_sku,Variation_SKU"</param>
    ''' <returns>returns datatable with onl;y required columns and remove excess columns</returns>
    Shared Function RemoveDTColumnsNotInList(dt As DataTable, ColumnsToKeep As String) As DataTable
        Dim dt2 As DataTable = dt
        Dim str As String = DTClass.GetDTColumnNames(dt2)
        For Each strKeyword In Split(str, ",")
            If InStr(ColumnsToKeep, strKeyword) = 0 Then
                DTClass.DTRemoveColumn(dt2, strKeyword)
            End If
        Next
        Return dt2
    End Function
    Shared Function GetTableNamesInDataset(dataSet As DataSet, Optional columnName As String = "name") As DataTable
        ' Print each table's TableName.
        Dim rv As New DataTable
        Dim dr As DataRow = rv.NewRow
        rv = DTClass.AddColumnToDataTable(rv, columnName)

        Dim table As DataTable
        For Each table In dataSet.Tables
            Dim r As DataRow = rv.NewRow
            r(0) = (table.TableName)
            rv.Rows.Add(r)
        Next table

        Return rv
    End Function
    Shared Function DDLtoDT(ddl As DropDownList) As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add(New DataColumn("Value", GetType(String)))
        dt.Columns.Add(New DataColumn("text", GetType(String)))
        For Each item As ListItem In ddl.Items
            Dim dr As DataRow = dt.NewRow()
            dr.SetField("text", item.Text)
            dr.SetField("Value", item.Value)
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function

    ''' <summary>
    ''' this function converts sqlstring result into a DataTable running the sql string provided
    ''' </summary>
    ''' <param name="strsql">simple sql string to execute</param>
    ''' <param name="SQLconstring">connection string if defualt is not using defualt else leave blank</param>
    ''' <returns>DataTable</returns>
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
            HttpContext.Current.Response.Write("ln.448 DTClass.GetDataTableFromSqlstr.sql=" & sql)
            HttpContext.Current.Response.Write("<br>SQLconstring=" & SQLconstring)
            HttpContext.Current.Response.End()
        End Try

    End Function

    ''' <summary>
    ''' matches structure of two tables to verify if they are identicle
    ''' </summary>
    ''' <param name="DT1">datatable number one</param>
    ''' <param name="DT2">datatable number two to match with </param>
    ''' <param name="ExceptColumns">any list of fields not to be matched</param>
    ''' <returns>boolean value true or false</returns>
    Shared Function MatchDTStructure(DT1 As DataTable, DT2 As DataTable, ExceptColumns As String) As Boolean
        Dim rv As Boolean = False
        Dim strDT1fileds As String = Replace(DTClass.GetDTColumnNames(DT1), ExceptColumns, "")
        Dim strDT2fileds As String = Replace(DTClass.GetDTColumnNames(DT2), ExceptColumns, "")
        If strDT1fileds = strDT2fileds Then rv = True
        Return rv
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DT"></param>
    ''' <param name="CSVlistofColumns">CSV List of columns</param>
    ''' <param name="strFilter"></param>
    ''' <returns></returns>
    Shared Function SumDTColumns(DT As DataTable, Optional CSVlistofColumns As String = "", Optional strFilter As String = " 1=1 ") As DataTable
        'HttpContext.Current.Response.Write("SumDTColumns.strDTColumnList:=" & DTClass.GetDTColumnNames(DT) & "<br>Row.Count=" & DT.Rows.Count.ToString)
        Dim RV As New DataTable
        RV = DTClass.AddColumnToDataTable(RV, "Sno", "007") : linenumber = 531
        Dim strDTColumnList As String = "nothing"
        ' HttpContext.Current.Response.Write("SumDTColumns.RV.strDTColumnList:=" & DTClass.GetDTColumnNames(RV) & "<br>Row.Count=" & RV.Rows.Count.ToString & "<br>Columns.Count=" & RV.Columns.Count.ToString)
        Try
            strDTColumnList = DTClass.GetDTColumnNames(DT) : linenumber = 523
            For Each strKeyword In Split(CSVlistofColumns, ",")
                If InStr(strDTColumnList, strKeyword) > 0 Then : linenumber = 525
                    Dim sum = Convert.ToDouble(DT.Compute("SUM(" & strKeyword & ")", strFilter))
                    RV = DTClass.AddColumnToDataTable(RV, strKeyword, sum.ToString) : linenumber = 527
                    ' HttpContext.Current.Response.Write("<br>SumDTColumns.sum." & strKeyword & "= :" & sum)
                End If
            Next
            Dim row As DataRow
            ' Add one row. Since it has default values, 
            ' no need to set values yet.
            row = RV.NewRow
            RV.Rows.Add(row)
        Catch ex As Exception
            HttpContext.Current.Response.Write("FN_SumDTColumns Error:" & ex.Message)
            HttpContext.Current.Response.Write("linenumber :" & linenumber)
            HttpContext.Current.Response.Write("strDTColumnList Error:" & strDTColumnList)
        End Try
        'HttpContext.Current.Response.Write("SumDTColumns.RV.strDTColumnList:=" & DTClass.GetDTColumnNames(RV) & "<br>Row.Count=" & RV.Rows.Count.ToString & "<br>Columns.Count=" & RV.Columns.Count.ToString)
        Return RV
    End Function
    Shared Function ConvertListToDataTable(Of T)(ByVal list As List(Of T)) As DataTable
        Dim table As New DataTable()

        ' Get all properties of the type T
        Dim fields() As FieldInfo = GetType(T).GetFields()
        ' Add columns to the DataTable for each property
        For Each field As FieldInfo In fields
            table.Columns.Add(field.Name, field.FieldType)
        Next

        ' Add a row for each object in the list
        For Each item As T In list
            Dim row As DataRow = table.NewRow()
            ' Set the value of each cell in the row based on the object's properties
            For Each field As FieldInfo In fields
                row(field.Name) = field.GetValue(item)
            Next
            table.Rows.Add(row)
        Next

        Return table
    End Function

End Class
