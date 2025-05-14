Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Text
Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Web.HttpContext
Imports System.Linq
Imports System.Globalization
Imports System.Reflection
Imports System.Text.RegularExpressions

Public Class TextCSVClass
    Public Shared Sub WriteNewCSVFile(ByVal newFileName As String, ByVal fileContents As String)
        Dim oWrite As System.IO.StreamWriter
        Try
            If File.Exists(newFileName) Then
                'If file exists then open for appending.
                'You can just overwrite it by putting in 'False' instead
                oWrite = New StreamWriter(newFileName, True)
            Else
                'If file doesn't exist, create it
                oWrite = File.CreateText(newFileName)
            End If
            'Write contents to file
            oWrite.Write(fileContents)
        Catch ex As Exception
            'Catch error here
            HttpContext.Current.Response.Write("Error ReArrangeCSV fields: " & ex.Message)
        Finally
            oWrite.Close()
        End Try
    End Sub
    Public Shared Function ReArrangeCSV(ByVal fileFullPath As String, ByVal seperator As Char) As String
        Dim output As String = ""
        Dim line As String
        Dim temp As String = ""
        Dim i As Integer
        Dim fieldValues, parts As String()
        Dim myReader As IO.StreamReader
        Dim oldLine As String = ""
        Try
            'Open file and read first line to determine how many fields there are.
            myReader = IO.File.OpenText(fileFullPath)
            While myReader.Peek() <> -1
                oldLine = myReader.ReadLine()
                HttpContext.Current.Response.Write("oldLine: " & oldLine.ToString)
                HttpContext.Current.Response.End()
                fieldValues = oldLine.Split(seperator)
                'Grab the first field since there's no change to it
                line = fieldValues(0)
                'Grabing the 3rd field and parse it
                temp = fieldValues(2).Replace("""", "") 'Remove the quotation marks
                temp = temp.Replace("<", "") 'Remove the < marks
                temp = temp.Replace(":", ";") 'Replace colons with semi-colons
                'Split it into parts
                parts = temp.Split(";")
                'Now add each part to the line
                For i = 0 To parts.Length() - 1
                    If parts(i).Trim <> "" Then
                        line &= "," & """" & parts(i) & """"
                    End If
                Next
                'Adding 2nd field to the end
                line &= "," & fieldValues(1)
                'Adding this line to the output
                output &= line & vbCrLf
                MsgBox(output)
            End While
        Catch ex As Exception
            HttpContext.Current.Response.Write("Error ReArrangeCSV fields: " & ex.Message)
        Finally
            ' myReader.Close()
        End Try
        Return output
    End Function
    Shared Function CsvToTable(ByVal filePathName As String, Optional ByVal hasHeader As Boolean = False, Optional Delimiters As String = ",", Optional itHasFieldsEnclosedInQuotes As Boolean = False) As DataTable
        ' Parses a csv into a datatable.
        '
        Try
            Dim result As New DataTable
            If System.IO.File.Exists(filePathName) Then
                Dim parser As New Microsoft.VisualBasic.FileIO.TextFieldParser(filePathName)
                parser.Delimiters = New String() {Delimiters}
                parser.HasFieldsEnclosedInQuotes = itHasFieldsEnclosedInQuotes ' True 'use if data may contain delimiters 
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
                ' HttpContext.Current.Response.Write("result.Rows.Count=" & result.Rows.Count)
                Return result
            Else : Return Nothing
            End If
        Catch ex As Exception
            HttpContext.Current.Response.Write("Error CsvToTable: " & ex.Message & "<BR>")
            HttpContext.Current.Response.Write("filePathName: " & filePathName & "<BR>")
            Console.WriteLine(ex.ToString())
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sourceTable"></param>
    ''' <param name="filePathName"></param>
    ''' <param name="HasHeader"></param>
    ''' <param name="sepChar"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function TableToCSVtab(ByVal sourceTable As DataTable, ByVal filePathName As String, Optional ByVal HasHeader As Boolean = True, Optional ByVal sepChar As String = vbTab) As Boolean
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
                sb.AppendLine(String.Join(sepChar, Array.ConvertAll(Of Object, String)(nameArray,
                                Function(o As Object) If(o.ToString.Contains(sepChar),
                                ControlChars.Quote & o.ToString & ControlChars.Quote, o))))
            End If
            For Each dr As DataRow In sourceTable.Rows
                sb.AppendLine(String.Join(sepChar, Array.ConvertAll(Of Object, String)(dr.ItemArray,
                                Function(o As Object) If(o.ToString.Contains(sepChar),
                                ControlChars.Quote & o.ToString & ControlChars.Quote, o.ToString))))
                '   HttpContext.Current.Response.Write(sb.ToString & "<br>")
            Next

            'HttpContext.Current.Response.Write("TextCSVClass.TableToCSVtab=" & sb.ToString)
            'HttpContext.Current.Response.Write("filePathName=" & filePathName)
            'HttpContext.Current.Response.End()
            System.IO.File.WriteAllText(filePathName, sb.ToString)
            Return True
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
            Return False
        End Try
    End Function
    Shared Function TableToStringTab(ByVal sourceTable As DataTable, Optional ByVal HasHeader As Boolean = False, Optional ByVal sepChar As String = vbTab) As String
        'Writes a datatable back into a csv 
        Dim returnvale As String = ""
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
                sb.AppendLine(String.Join(sepChar, Array.ConvertAll(Of Object, String)(nameArray,
                                Function(o As Object) If(o.ToString.Contains(sepChar),
                                ControlChars.Quote & o.ToString & ControlChars.Quote, o))))
            End If
            For Each dr As DataRow In sourceTable.Rows
                sb.AppendLine(String.Join(sepChar, Array.ConvertAll(Of Object, String)(dr.ItemArray,
                                Function(o As Object) If(o.ToString.Contains(sepChar),
                                ControlChars.Quote & o.ToString & ControlChars.Quote, o.ToString))))
                'HttpContext.Current.Response.Write(sb.ToString & "<br>")
            Next
            returnvale = sb.ToString
            'System.IO.File.WriteAllText(filePathName, returnvale)
            Return returnvale
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
            Return returnvale
        End Try
    End Function
    Shared Function TableToCSV(ByVal sourceTable As DataTable, ByVal filePathName As String, Optional ByVal HasHeader As Boolean = False) As Boolean
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
                sb.AppendLine(String.Join(",", Array.ConvertAll(Of Object, String)(nameArray,
                                Function(o As Object) If(o.ToString.Contains(","),
                                ControlChars.Quote & o.ToString & ControlChars.Quote, o))))
            End If
            For Each dr As DataRow In sourceTable.Rows
                sb.AppendLine(String.Join(",", Array.ConvertAll(Of Object, String)(dr.ItemArray,
                                Function(o As Object) If(o.ToString.Contains(","),
                                ControlChars.Quote & o.ToString & ControlChars.Quote, o.ToString))))
                HttpContext.Current.Response.Write(sb.ToString & "<br>")
            Next
            System.IO.File.WriteAllText(filePathName, sb.ToString)
            Return True
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
            Return False
        End Try
    End Function
    Public Shared Sub addlinetocsv(path As String, filename As String, TextToAdd As String, Optional firstrow As String = "")
        ' This text is added only once to the file. 
        If System.IO.File.Exists(path & filename) = False Then
            ' Create a file to write to. 
            Dim createText As String = firstrow + Environment.NewLine
            System.IO.File.WriteAllText(path, createText)
        End If
        ' This text is always added, making the file longer over time 
        ' if it is not deleted. 
        Dim appendText As String = TextToAdd + Environment.NewLine
        System.IO.File.AppendAllText(path, appendText)

        ' Open the file to read from. 
        Dim readText As String = System.IO.File.ReadAllText(path)
        Console.WriteLine(readText)
    End Sub
    Shared Function AddValuesToTable(ByRef source() As String, ByVal destination As DataTable, Optional ByVal HeaderFlag As Boolean = False) As Boolean
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

    Shared Sub Resolve_Duplicate_Names(ByRef source() As String)
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


    Public Shared Sub repalcetextsmallfile(fileToChange As String, NewFile As String, text_top_replace As String, New_text As String)
        Dim myStreamReaderL1 As System.IO.StreamReader
        Dim myStream As System.IO.StreamWriter

        Dim myStr As String
        myStreamReaderL1 = System.IO.File.OpenText(fileToChange)
        myStr = myStreamReaderL1.ReadToEnd()

        myStreamReaderL1.Close()


        myStr = myStr.Replace(text_top_replace, New_text)
        'Save myStr
        myStream = System.IO.File.CreateText(NewFile)
        myStream.WriteLine(myStr)
        myStream.Close()
    End Sub
    Public Shared Sub RepalceTextLargeFile(fileToChange As String, text_top_replace As String, New_text As String)
        Dim myStreamReaderL1 As System.IO.StreamReader
        'Dim myStream As System.IO.StreamWriter
        Dim myStr As String = ""
        Dim intCounter As Integer = 0


        myStreamReaderL1 = System.IO.File.OpenText(fileToChange)
        'HttpContext.Current.Response.Write("<br>myStreamReaderL1.Peek=" & myStreamReaderL1.Peek.ToString)
        Do While myStreamReaderL1.Peek > -1
            'HttpContext.Current.Response.Write("<br>myStreamReaderL1.Peek=" & myStreamReaderL1.ReadLine.ToString)
            myStr = myStreamReaderL1.ReadLine()
            myStr = myStreamReaderL1.ReadToEnd()

            If intCounter = 0 Then
                HttpContext.Current.Response.Write("<br>In here <br>" & vbCrLf)
                HttpContext.Current.Response.Write("<br><New_text=" & New_text & "<br>")
                HttpContext.Current.Response.Write("<br><myStr=" & myStr & "<br>")

                HttpContext.Current.Response.Write("<br><br>before myStr=" & myStr & "<br>" & vbCrLf)
                myStr = myStr.Replace(text_top_replace, New_text)
                HttpContext.Current.Response.Write("<br><br>after myStr=" & myStr & "<br>" & vbCrLf)

            End If

            If myStr = text_top_replace Then
                Exit Do
            End If

            intCounter = intCounter + 1
        Loop

        myStreamReaderL1.Close()
        'HttpContext.Current.Response.Write("<br>text_top_replace=" & text_top_replace & "<br>" & " New Text=" & New_text & "<br>")

        Dim StreamWriter = New StreamWriter(fileToChange)
        'myStr = myStr.Replace(text_top_replace, New_text)
        'StreamWriter.WriteLine(myStr)
        StreamWriter.Close()
        myStreamReaderL1.Close()
    End Sub

    Shared Function ReadLineWithNumberFrom(filePath As String, ByVal lineNumber As Integer) As String
        Using file As New StreamReader(filePath)
            ' Skip all preceding lines: '
            For i As Integer = 1 To lineNumber - 1
                If file.ReadLine() Is Nothing Then
                    Throw New ArgumentOutOfRangeException("lineNumber")
                End If
            Next
            ' Attempt to read the line you're interested in: '
            Dim line As String = file.ReadLine()
            If line Is Nothing Then
                Throw New ArgumentOutOfRangeException("lineNumber")
            End If
            ' Succeded!
            Return line
        End Using
    End Function
    Shared Function CountCharacter(ByVal strvalue As String, ByVal ch As Char) As Integer
        Return strvalue.Count(Function(c As Char) c = ch)
    End Function
    ' Save the input DataTable to a CSV file. By default the values are Tab 
    ' delimited, but you can use the second overload version to use any other 
    ' string you want.
    '
    ' Example:
    '    Dim ds As New DataSet
    '    SqlDataAdapter1.Fill(ds, "Users")
    '    DataTable2CSV(ds.Tables("Users"), "D:\Users.txt")

    'Sub DataTable2CSV(ByVal table As DataTable, ByVal filename As String)
    '    DataTable2CSV(table, filename, vbTab)
    'End Sub

    Shared Function DataTable2CSV(ByVal table As DataTable, ByVal filename As String, Optional ByVal sepChar As String = ",", Optional ByVal HasHeader As Boolean = False) As String
        Dim writer As System.IO.StreamWriter
        Try
            writer = New System.IO.StreamWriter(filename)

            Dim sep As String = ""
            Dim builder As New System.Text.StringBuilder

            ' first write a line with the columns name           
            If HasHeader = True Then
                For Each col As DataColumn In table.Columns
                    builder.Append(sep).Append(col.ColumnName)
                    sep = sepChar
                Next
                writer.WriteLine(builder.ToString())
            End If

            ' then write all the rows
            For Each row As DataRow In table.Rows
                sep = ""
                builder = New System.Text.StringBuilder

                For Each col As DataColumn In table.Columns
                    builder.Append(sep).Append(row(col.ColumnName))
                    sep = sepChar
                Next
                writer.WriteLine(builder.ToString())
            Next
            Return True
        Catch ex As Exception
            HttpContext.Current.Response.Write("<br> DataTable2CSV Error:" & ex.message.ToString())
            Return False
        Finally
            'If Not writer Is Nothing Then writer.Close()
            If Not writer Is Nothing Then
                writer.Close()
            End If
        End Try
    End Function
    Shared Function DataTable2CSVNN(ByVal table As DataTable, ByVal filename As String, Optional ByVal sepChar As String = ",", Optional ByVal HasHeader As Boolean = False, Optional AdditionalQuotes As String = "") As String
        Dim writer As System.IO.StreamWriter
        Try
            writer = New System.IO.StreamWriter(filename)


            Dim sep As String = ""
            Dim builder As New System.Text.StringBuilder

            ' first write a line with the columns name           
            If HasHeader = True Then
                For Each col As DataColumn In table.Columns
                    builder.Append(sep).Append(AdditionalQuotes).Append(col.ColumnName).Append(AdditionalQuotes)
                    sep = sepChar
                Next
                writer.WriteLine(builder.ToString())
            End If

            ' then write all the rows
            For Each row As DataRow In table.Rows
                sep = ""
                builder = New System.Text.StringBuilder

                For Each col As DataColumn In table.Columns
                    builder.Append(sep).Append(AdditionalQuotes).Append(row(col.ColumnName)).Append(AdditionalQuotes)
                    sep = sepChar
                Next
                writer.WriteLine(builder.ToString())
            Next
            Return True
        Finally
            If Not writer Is Nothing Then
                writer.Close()
            End If
        End Try
    End Function


    Shared Function CompareFiles(file1path As String, file2path As String, newfilepath As String) As String
        Dim File1Lines As String() = File.ReadAllLines(file1path)
        Dim File2Lines As String() = File.ReadAllLines(file2path)
        Dim NewLines As New List(Of String)()
        Dim returnvalue As String = ""
        For lineNo As Integer = 0 To File1Lines.Length - 1
            If Not [String].IsNullOrEmpty(File1Lines(lineNo)) AndAlso Not [String].IsNullOrEmpty(File2Lines(lineNo)) Then
                If [String].Compare(File1Lines(lineNo), File2Lines(lineNo)) <> 0 Then
                    NewLines.Add(File2Lines(lineNo))
                End If
            ElseIf Not [String].IsNullOrEmpty(File1Lines(lineNo)) Then
            Else
                NewLines.Add(File2Lines(lineNo))
            End If
        Next
        If NewLines.Count > 0 Then
            'File.WriteAllLines(newfilepath, NewLines)
            returnvalue = NewLines.ToString
        Else
            returnvalue = "00"
        End If
        Return returnvalue
    End Function
    ''' <summary>
    ''' tested and works fine
    ''' </summary>
    ''' <param name="SqlDataSource">data source  as Object</param>
    ''' <param name="failename">Name of the File to save as string</param>
    Protected Sub ExportToCSV(SqlDataSource As SqlDataSource, Optional failename As String = "DataTable.csv")
        'Get the data from database into datatable
        Dim dt As DataTable = SQLFunctions.GetDataTablefromSQLDatasource(SqlDataSource) 'GetData(cmd)
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" & failename & "")
        HttpContext.Current.Response.Charset = ""
        HttpContext.Current.Response.ContentType = "application/text"

        Dim sb As New StringBuilder()
        For k As Integer = 0 To dt.Columns.Count - 1
            'add separator
            sb.Append(dt.Columns(k).ColumnName + ","c)
        Next
        'append new line
        sb.Append(vbCr & vbLf)
        For i As Integer = 0 To dt.Rows.Count - 1
            For k As Integer = 0 To dt.Columns.Count - 1
                'add separator
                sb.Append(dt.Rows(i)(k).ToString().Replace(",", ";") + ","c)
            Next
            'append new line
            sb.Append(vbCr & vbLf)
        Next
        HttpContext.Current.Response.Output.Write(sb.ToString())
        HttpContext.Current.Response.Flush()
        HttpContext.Current.Response.End()
    End Sub
    Shared Function GetLastCharacters(str As String, Optional NoOfCharacter As Integer = 1, Optional Trimstring As Boolean = False) As String
        Dim result As String = Trim(str)
        If Trimstring = True Then result = Trim(result)
        If str.Length > NoOfCharacter Then
            result = str.Substring(str.Length - NoOfCharacter, NoOfCharacter)
        End If
        Return result
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
    Public Shared Function RemoveEndCharacters(str As String, Optional NoOfCharacter As Integer = 1, Optional Trimstring As Boolean = False) As String
        Dim result As String = (str)
        If Trimstring = True Then result = Trim(result)
        If str.Length > NoOfCharacter Then
            result = str.Substring(0, str.Length - (NoOfCharacter))
        End If
        Return result
    End Function
    Public Shared Sub WiteToServer(FILEpath As String, FILENAME As String, FileContent As String)
        Dim path As String = Current.Server.MapPath(FILEpath & FILENAME)
        If File.Exists(path) = False Then
            ' Create a file to write to.
            Dim sw As StreamWriter = File.CreateText(path)
            sw.WriteLine(FileContent)
            sw.Flush()
            sw.Close()
        End If
    End Sub
    Shared Sub ExportToCSV(filename As String, dt As DataTable, Optional addheader As Boolean = True, Optional separator As String = "~"c)
        'Get the data from SQLRecordSet into datatable

        Current.Response.Clear()
        Current.Response.Buffer = True
        Current.Response.AddHeader("content-disposition", "attachment;filename=" & filename)
        Current.Response.Charset = ""
        Current.Response.ContentType = "application/text"
        Dim row As String
        Dim sb As New StringBuilder()
        ' adds header to the list
        If addheader = True Then
            For k As Integer = 0 To dt.Columns.Count - 1
                'add separator
                Dim addseprater As String = separator
                If k = (dt.Columns.Count - 1) Then addseprater = "" ' removes trailing coma from the string
                row = dt.Columns(k).ColumnName + addseprater
                row = row
                sb.Append(row)
            Next
            row = ""
            'append new line
            sb.Append(vbCr & vbLf)
        End If

        ' Adds listed data the the table
        For i As Integer = 0 To dt.Rows.Count - 1
            For k As Integer = 0 To dt.Columns.Count - 1
                'add separator
                Dim addseprater As String = separator
                If k = (dt.Columns.Count - 1) Then addseprater = "" ' removes trailing coma from the string
                row = dt.Rows(i)(k).ToString().Replace(separator, ";") + addseprater
                'Response.Write("#")
                sb.Append(row)
            Next
            row = ""
            'append new line
            sb.Append(vbCr & vbLf)
        Next
        Current.Response.Output.Write(sb.ToString())
        Current.Response.Flush()
        Current.Response.End()
    End Sub
    Shared Function DTtoStringBuilder(filename As String, dt As DataTable, Optional addheader As Boolean = True, Optional separator As String = "~"c) As StringBuilder
        'Get the data from SQLRecordSet into datatable
        Dim returnvalue As New StringBuilder
        Current.Response.Clear()
        Current.Response.Buffer = True
        Current.Response.AddHeader("content-disposition", "attachment;filename=" & filename)
        Current.Response.Charset = ""
        Current.Response.ContentType = "application/text"
        Dim row As String
        Dim sb As New StringBuilder()
        ' adds header to the list
        If addheader = True Then
            For k As Integer = 0 To dt.Columns.Count - 1
                'add separator
                Dim addseprater As String = separator
                If k = (dt.Columns.Count - 1) Then addseprater = "" ' removes trailing coma from the string
                row = dt.Columns(k).ColumnName + addseprater
                row = row
                sb.Append(row)
            Next
            row = ""
            'append new line
            sb.Append(vbCr & vbLf)
        End If

        ' Adds listed data the the table
        For i As Integer = 0 To dt.Rows.Count - 1
            For k As Integer = 0 To dt.Columns.Count - 1
                'add separator
                Dim addseprater As String = separator
                If k = (dt.Columns.Count - 1) Then addseprater = "" ' removes trailing coma from the string
                row = dt.Rows(i)(k).ToString().Replace(separator, ";") + addseprater
                'Response.Write("#")
                sb.Append(row)
            Next
            row = ""
            'append new line
            sb.Append(vbCr & vbLf)
        Next
        returnvalue = sb
        Return returnvalue
    End Function
    Shared Function DTtoCSVonServer(ServerSaveFolder As String, filename As String, dt As DataTable, Optional addheader As Boolean = True, Optional separator As String = "~"c) As Boolean
        Dim returnvalue As Boolean = False
        'HttpContext.Current.Response.Write("DTtoCSVonServer.dt.Rows.Count.ToString=" & dt.Rows.Count.ToString)
        returnvalue = TableToCSVtab(dt, filename, addheader, separator)
        Return returnvalue
    End Function
    Shared Function DTtoCSVonServerV2(ServerSaveFolder As String, filename As String, dt As DataTable, Optional addheader As Boolean = True, Optional separator As String = "~"c) As Boolean
        Dim returnvalue As Boolean = False
        If ServerSaveFolder <> "" Then filename = HttpContext.Current.Server.MapPath(ServerSaveFolder) & filename
        'HttpContext.Current.Response.Write("DTtoCSVonServer.dt.Rows.Count.ToString=" & dt.Rows.Count.ToString)
        returnvalue = TableToCSVtab(dt, filename, addheader, separator)
        Return returnvalue
    End Function
    Shared Sub DownLoadedToLocal(FILEpath As String, FILENAME As String)


        Const bufferLength As Integer = 20000
        Dim buffer As Byte() = New [Byte](bufferLength - 1) {}
        Dim length As Integer = 0
        Dim download As Stream = Nothing
        Dim strFilePath As String = Current.Server.MapPath(FILEpath & FILENAME)
        Try
            download = New FileStream(strFilePath, FileMode.Open, FileAccess.Read)
            Do
                If Current.Response.IsClientConnected Then
                    length = download.Read(buffer, 0, bufferLength)
                    Current.Response.OutputStream.Write(buffer, 0, length)
                    buffer = New [Byte](bufferLength - 1) {}
                Else
                    length = -1
                End If
            Loop While length > 0
            Current.Response.Flush()
            Current.Response.End()


        Catch ex As Exception
            'HttpContext.Current.Response.Write("DownLoadedToLocal=" & ex.Message.ToString)
            'HttpContext.Current.Response.Write("<br>strFilePath=" & strFilePath)
            'HttpContext.Current.Response.Write("<br>FILENAME=" & FILENAME)
        Finally
            If download IsNot Nothing Then
                download.Close()
            End If
        End Try

    End Sub
    Public Shared Function ConvertDataTableToString(ByVal sourceTable As DataTable, ByVal filePathName As String, Optional ByVal HasHeader As Boolean = True, Optional ByVal sepChar As String = vbTab) As String
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
                sb.AppendLine(String.Join(sepChar, Array.ConvertAll(Of Object, String)(nameArray,
                                Function(o As Object) If(o.ToString.Contains(sepChar),
                                ControlChars.Quote & o.ToString & ControlChars.Quote, o))))
            End If
            For Each dr As DataRow In sourceTable.Rows
                sb.AppendLine(String.Join(sepChar, Array.ConvertAll(Of Object, String)(dr.ItemArray,
                                Function(o As Object) If(o.ToString.Contains(sepChar),
                                ControlChars.Quote & o.ToString & ControlChars.Quote, o.ToString))))
            Next
            System.IO.File.WriteAllText(filePathName, sb.ToString)
            Return sb.ToString
        Catch ex As Exception
            Current.Response.Write(ex.Message.ToString())
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CSVFileNameWithPath">filename with folderpath and extension</param>
    ''' <param name="hasheader"></param>
    ''' <param name="delimiter"></param>
    ''' <param name="filedsenclosedincomas"></param>
    ''' <returns>give filename with complete folderpath and extension, returns Integer</returns>
    Shared Function RowsInSCVFile(CSVFileNameWithPath As String, Optional hasheader As Boolean = True, Optional delimiter As String = ",", Optional FiledsEnclosedinComas As Boolean = False) As Integer
        Dim RV As Integer = 0
        Dim dt As New DataTable
        dt = CsvToTable(CSVFileNameWithPath, hasheader, delimiter, filedsenclosedincomas)
        RV = dt.Rows.Count
        dt = Nothing
        Return RV
    End Function
    Shared Function RemoveSubStringFromRightLeft(MainString As String, SubStringToRemove As String, Optional RemoveRightOrLeft As String = "right") As String
        Dim rv As String = MainString
        Dim SubStringLength As Integer = SubStringToRemove.Length
        If LCase(RemoveRightOrLeft) = "right" Then
            rv = Left(rv, (rv.Length - SubStringLength))
        ElseIf LCase(RemoveRightOrLeft) = "left" Then
            rv = Right(rv, (rv.Length - SubStringLength))
        Else
            rv = "##" & rv
        End If
        rv = rv
        Return rv
    End Function
    Shared Function CSVStringToQualifierSQLString(ByVal CSVString As String, Optional qualifier As String = "'", Optional seperator As String = ",") As String
        Dim rv As String = qualifier
        rv = qualifier & Replace(CSVString, seperator, qualifier & seperator & qualifier) & qualifier
        Return rv
    End Function
    Public Shared Sub PrintArray(ByVal Array As String)
        For Each item In Array
            HttpContext.Current.Response.Write(item)
        Next
    End Sub
    Shared Function itemsinArray(array As String, Optional seperator As String = ",") As Integer
        Dim words As String() = array.Split(seperator)
        Dim rv As Integer = words.Count - 1
        Return rv
    End Function
    Shared Function GetArrayItem(ByVal Array As Array, Optional itemnumber As Integer = 0) As String
        'For Each item In Array
        'HttpContext.Current.Response.Write(item)
        'Next
        Dim rv As String = ""
        For i As Integer = 0 To Array.Length - 1
            'Console.Write(Array(i))

            If i = itemnumber Then
                rv = Array(i)
            End If
        Next
        Return rv
    End Function
    Shared Function GetElementFromArray_bad(phrase As String, Optional seperator As String = ",", Optional ElementNUmber As Integer = 0) As String
        'Dim phrase As String = "The quick brown    fox     jumps over the lazy dog."
        Dim words As String() = phrase.Split(seperator)
        Dim rv As String = ""
        'Dim count As Integer = words.Count
        'For Each word In words
        'For i As Integer = 0 To count - 1
        'Each word 
        rv = words.ElementAt(ElementNUmber)
        'Next
        Return rv
    End Function
    Shared Function GetElementFromArray(phrase As String, Optional separator As String = ",", Optional ElementNumber As Integer = 0) As String
        ' Split the phrase into an array using the provided separator
        Dim elements() As String = phrase.Split(New String() {separator}, StringSplitOptions.None)

        ' Check if the provided element number is within the bounds of the array
        If ElementNumber >= 0 And ElementNumber < elements.Length Then
            ' Return the element at the specified position
            Return elements(ElementNumber).Trim()
        Else
            ' Return an empty string if the element number is out of bounds
            Return String.Empty
        End If
    End Function
    Shared Function usingGetElementFromArry(phrase As String, separator As String, elementNumber As Integer) As String
        Dim element As String = ""
        If phrase = "" Then phrase = "Apple, Banana, Cherry, Date"
        element = GetElementFromArray(phrase, separator, elementNumber)
    End Function
    Shared Function GetitemCountInArray(array As String, Optional seperator As String = ",") As Integer
        Dim words As String() = array.Split(seperator)
        Dim rv As Integer = words.Count - 1
        Return rv
    End Function
    Public Function ConvertTextToDictionary(filePath As String, Optional delimiter As String = ",") As Object
        ConvertTextToDictionary = CreateObject("Scripting.Dictionary")

        ' Open the CSV file for reading using a FileSystemObject
        Dim ForReading As String = ""
        Dim objFSO = CreateObject("Scripting.FileSystemObject")
        Dim objFile = objFSO.OpenTextFile(filePath, ForReading)

        ' Read the header line (optional)
        Dim strHeaderLine = objFile.ReadLine
        ' If you want to use the header line for keys, uncomment the following lines:
        ' arrHeaders = Split(strHeaderLine, ",")
        ' 

        ' Read each line of the file
        Do While Not objFile.AtEndOfStream
            Dim strLine = objFile.ReadLine
            ' Split the line based on comma (delimiter)
            Dim arrValues = Split(strLine, delimiter)

            ' Check for valid data (at least two values for a key-value pair)
            If UBound(arrValues) >= 1 Then
                ' Assuming the first value is the key (adjust as needed)
                Dim key = arrValues(0)
                ' You can access other values in the array using their index (e.g., arrValues(1) for second value)
                Dim value = arrValues(1) ' Assuming the second value is the value (adjust as needed)
                ' Add the key-value pair to the dictionary
                ConvertTextToDictionary.Add(key, value)
            End If
        Loop

        ' Close the file
        objFile.Close

        ' Set the return value to the dictionary
        'Dim rv As Object = ConvertTextToDictionary
    End Function
    Shared Function ContainsString(ByVal fullString As String, ByVal subString As String) As Boolean
        ' Use InStr function to find the starting position of the substring within the full string
        Dim position As Integer = InStr(fullString, subString)

        ' Check if the position is greater than zero (meaning the substring was found)
        Return position > 0
    End Function

End Class
