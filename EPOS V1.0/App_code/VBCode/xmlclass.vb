Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Xml
Imports System.Xml.Xsl
Imports System.Xml.XPath

Imports System.Text
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Web
Imports System.Web.HttpUtility

Public Class xmlclass

    Public Shared Function showxml(ByRef filepath As String) As String
        Dim returnvalue As String = ""
        Dim theXML As String = HttpUtility.HtmlEncode(File.ReadAllText(filepath))
        returnvalue = theXML
        Return returnvalue
    End Function

    Public Shared Function convertxmltostring(ByRef xmldoc As XmlDocument) As String
        Using stringWriter = New StringWriter
            Using xmlTextWriter = XmlWriter.Create(stringWriter)
                xmldoc.WriteTo(xmlTextWriter)
                xmlTextWriter.Flush()
                Return stringWriter.GetStringBuilder().ToString()
            End Using
        End Using
    End Function
    Public Shared Function BuildXmlString(xmlRootName As String, values As String()) As String
        Dim xmlString As New StringBuilder()

        xmlString.AppendFormat("<{0}>", xmlRootName)
        For i As Integer = 0 To values.Length - 1
            xmlString.AppendFormat("<value>{0}</value>", values(i))
        Next
        xmlString.AppendFormat("</{0}>", xmlRootName)

        Return xmlString.ToString()
    End Function


    Private Sub InsertXMLtoSQLfields(mfilename As XmlDocument, sqltable As String)
        '      Dim connetionString As String
        Dim command As SqlCommand
        Dim adpter As New SqlDataAdapter
        Dim ds As New DataSet
        Dim xmlFile As XmlReader
        Dim sql As String
        Dim name As String
        Dim telephone As String
        Dim email As Double
        Dim myConnection As SqlConnection = New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
        xmlFile = XmlReader.Create("sample1.xml", New XmlReaderSettings())
        ds.ReadXml(xmlFile)
        Dim i As Integer
        myConnection.Open()
        For i = 0 To ds.Tables(0).Rows.Count - 1
            name = ds.Tables(0).Rows(i).Item(0)
            telephone = ds.Tables(0).Rows(i).Item(1)
            email = ds.Tables(0).Rows(i).Item(2)
            sql = "insert into " & sqltable & " values(" & name & ",'" & telephone & "'," & email & ")"
            command = New SqlCommand(sql, myConnection)
            adpter.InsertCommand = command
            adpter.InsertCommand.ExecuteNonQuery()
        Next
        myConnection.Close()
    End Sub
    Shared Sub ReadxmlfileMain(fileName As String)
        ' not tested yet
        Dim reader As XmlTextReader = New XmlTextReader(fileName)

        Do While (reader.Read())
            Select Case reader.NodeType
                Case XmlNodeType.Element 'Display beginning of element.
                    Console.Write("<" + reader.Name)
                    If reader.HasAttributes Then 'If attributes exist
                        While reader.MoveToNextAttribute()
                            'Display attribute name and value.
                            Console.Write(" {0}='{1}'", reader.Name, reader.Value)
                        End While
                    End If
                    Console.WriteLine(">")
                Case XmlNodeType.Text 'Display the text in each element.
                    Console.WriteLine(reader.Value)
                Case XmlNodeType.EndElement 'Display end of element.
                    Console.Write("</" + reader.Name)
                    Console.WriteLine(">")
            End Select
        Loop
        Console.ReadLine()

    End Sub
    ''' <summary>
    ''' Reads from XML File Converts converts it into Datatable asn returns the datatable
    ''' </summary>
    ''' <param name="xmlData">like C:\inetpub\wwwroot\EscapadeMIS_wip\XMLdata\Schedules.xml</param>
    ''' <param name="TableNameToReturn">Name of the table in  XML Data default returns first table</param>
    ''' <returns>data table </returns>
    ''' <remarks>Example Dim xmlfilepath As String =C:\inetpub\wwwroot\EscapadeMIS_wip\XMLdata\Schedules.xml;Dim schedule As String = xmlclass.showxml(xmlfilepath);Dim Dt As DataTable = xmlclass.XMLtoDataTable(xmlfilepath);GridView1.DataSource = Dt;GridView1.DataBind()</remarks>
    Shared Function XMLtoDataTable(xmlData As String, Optional TableNameToReturn As String = "0") As DataTable
        Dim ds As New DataSet()
        Dim Dt As DataTable
		 Dim reader = New XmlTextReader(xmlData)
        Try

            ds.ReadXml(reader)
            If TableNameToReturn = "0" Then
                Dt = ds.Tables(0)
            Else
                Dt = ds.Tables(TableNameToReturn)
            End If
            Dt = ds.Tables(0)
            ' Dt.DefaultView.Sort = "Time" & " " & "ASC"
            Return Dt
            Return Nothing
        Finally
            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try
    End Function
    ''' <summary>
    ''' Reads from XML File Converts converts it into Datatable asn returns the datatable
    ''' </summary>
    ''' <param name="xmlData">like C:\inetpub\wwwroot\EscapadeMIS_wip\XMLdata\Schedules.xml</param>
    ''' <param name="TableNameToReturn">Name of the table in  XML Data default returns first table</param>
    ''' <returns>data table </returns>
    ''' <remarks></remarks>
    Shared Function DataTabletoXML(xmlData As String, Optional TableNameToReturn As String = "0") As DataTable
        Dim ds As New DataSet()
        Dim Dt As DataTable
        Try

            ds.ReadXml(New XmlTextReader((xmlData)))
            If TableNameToReturn = "0" Then
                Dt = ds.Tables(0)
            Else
                Dt = ds.Tables(TableNameToReturn)
            End If
            Dt = ds.Tables(0)
            Return Dt
            Return Nothing
        Finally
            'If reader IsNot Nothing Then
            '    reader.Close()
            'End If
        End Try
    End Function
	Shared Function ConvertXMLToDataSet(xmlData As String, Optional datasetname As String = "") As DataSet
        Dim stream As StringReader = Nothing
        Dim reader As XmlTextReader = Nothing
        Try
            Dim xmlDS As New DataSet(datasetname)
            stream = New StringReader(xmlData)
            ' Load the XmlTextReader from the stream
            reader = New XmlTextReader(stream)
            xmlDS.ReadXml(reader)
            Return xmlDS
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message.ToString)
            HttpContext.Current.Response.End()
            Return Nothing
        Finally
            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try
    End Function
    Shared Function ConvertXMLToDataSet_old(xmlData As String) As DataSet
        Dim stream As StringReader = Nothing
        Dim reader As XmlTextReader = Nothing
        Try
            Dim xmlDS As New DataSet()
            stream = New StringReader(xmlData)
            ' Load the XmlTextReader from the stream
            reader = New XmlTextReader(stream)
            xmlDS.ReadXml(reader)
            Return xmlDS
        Catch
            Return Nothing
        Finally
            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try
    End Function
    ' Use this function to get XML string from a dataset
    ' Function to convert passed dataset to XML data
    Shared Function ConvertDataSetToXML(xmlDS As DataSet) As String
        Dim stream As MemoryStream = Nothing
        Dim writer As XmlTextWriter = Nothing
        Try
            stream = New MemoryStream()
            ' Load the XmlTextReader from the stream
            writer = New XmlTextWriter(stream, Encoding.Unicode)
            ' Write to the file with the WriteXml method.
            xmlDS.WriteXml(writer)
            Dim count As Integer = CInt(stream.Length)
            Dim arr As Byte() = New Byte(count - 1) {}
            stream.Seek(0, SeekOrigin.Begin)
            stream.Read(arr, 0, count)
            Dim utf As New UnicodeEncoding()
            Return utf.GetString(arr).Trim()
        Catch
            Return [String].Empty
        Finally
            If writer IsNot Nothing Then
                writer.Close()
            End If
        End Try
    End Function


    'Private Function WriteGridDatatoCSV(ByVal filedsToget As String, ByVal fTableName As String, ByVal fCriteria As String) As String
    '    'Declaration of Variables
    '    Dim SQLconstring As String = "Escapade_NewConnectionString"
    '    'Dim conStr As String = "Provider=SQLOLEDB;Data Source=ESCAPADEDEVELOP\SQLEXPRESS;Initial Catalog=Escapade_New;Persist Security Info=True;User ID=sa1;Password=password"
    '    Dim conStr As String = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
    '    Dim myString As New System.Text.StringBuilder()
    '    Dim dbConnection As New OleDbConnection(conStr)
    '    Dim fReturnData As String = "nothing"

    '    Dim ds As New DataSet
    '    dbConnection.Open()
    '    Dim SqlString As String = "SELECT " & filedsToget & " FROM " & fTableName & " WITH(NOLOCK) where " & fCriteria
    '    Dim command As New OleDbCommand(SqlString, dbConnection)

    '    Dim bFirstRecord As Boolean = True
    '    Dim myWriter As New System.IO.StreamWriter("C:\MyTestCSV.csv")

    '    Dim da As New OleDbDataAdapter(command)

    '    ' Dim SqlString As String = "SELECT " & fSeekFields & " FROM " & fTableName & "  WITH (NOLOCK)  WHERE " & fSearchCriteria
    '    'SqlString = Replace(SqlString, "'", "''")

    '    'Dim dbConnection As New OleDbConnection
    '    'dbConnection.ConnectionString = conStr
    '    'dbConnection.Open()

    '    '      Using connection As New OleDbConnection(conStr)

    '    Dim da As New OleDbDataAdapter(command)
    '    'da.Fill(ds)
    '    '*******************
    '    Try
    '        da.Fill(ds)
    '        Console.WriteLine("Made connection to the database")
    '        For Each dr As DataRow In ds.Tables(0).Rows
    '            bFirstRecord = True
    '            For Each field As Object In dr.ItemArray
    '                If Not bFirstRecord Then
    '                    myString.Append(",")
    '                End If
    '                myString.Append(field.ToString)
    '                bFirstRecord = False
    '            Next
    '            'New Line to differentiate next row
    '            myString.Append(Environment.NewLine)
    '        Next

    '    Catch ex As Exception
    '        HttpContext.Current.Response.Write(SqlString & "<br/>")
    '        HttpContext.Current.Response.Write(ex.Message)
    '        MsgBox(ex.Message)
    '        ' HttpContext.Current.Response.End()
    '    End Try

    '    '      End Using

    '    Return myString.ToString


    'End Function
    'Public Sub SaveFileIntoDatabase(filename As String, tempFilename As String)
    '    ' Dim sqlDatabase As New SqlDatabase(ConfigurationManager.ConnectionStrings("ftConnString").ConnectionString)
    '    Dim myConnection As SqlConnection = New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
    '    'Now we must get all bytes from file and put them into fileData byte array.
    '    'This byte array we later save on database into File table.

    '    Dim fileData As Byte() = GetBytes(tempFilename)

    '    Dim sql As String = "spFile_Add"

    '    Dim sqlCommand As SqlCommand = TryCast(myConnection.GetStoredProcCommand(sql), SqlCommand)
    '    Try
    '        sqlDatabase.AddInParameter(sqlCommand, "@Filename", SqlDbType.VarChar, filename)
    '        sqlDatabase.AddInParameter(sqlCommand, "@Content", SqlDbType.VarBinary, fileData)
    '        sqlDatabase.ExecuteNonQuery(sqlCommand)
    '    Catch err As Exception
    '        Throw New Exception("SaveFileIntoDatabase: " + err.Message)
    '    End Try
    'End Sub

    Public Function GetBytes(filePath As String) As Byte()

        ' give serverpath using Server.MapPath(path)
        Dim fs As FileStream = Nothing
        Dim br As BinaryReader = Nothing
        Try
            Dim buffer As Byte() = Nothing
            fs = New FileStream(filePath, FileMode.Open, FileAccess.Read)
            br = New BinaryReader(fs)
            Dim numBytes As Long = New FileInfo(filePath).Length
            buffer = br.ReadBytes(CInt(numBytes))
            br.Close()
            fs.Close()
            Return buffer
        Catch err As Exception
            Throw New Exception("SaveFileIntoDatabase: " + err.Message)
        End Try
    End Function

    'Public Shared Sub convertCSVtoDT(Path As String)
    '    Dim fieldcount As Integer = 0
    '    Dim CSVFILE = IO.File.ReadAllLines(Path)
    '    Dim Row As DataRow
    '    Dim rowcount As Integer = CSVFILE.GetLength(0) - 1
    '    Dim Fields As String()
    '    Dim dc As New DataColumn()
    '    Fields = CSVFILE(0).Split(New Char() {","c})
    '    Try

    '        fieldcount = CSVFILE.First.Split(","c).Length
    '        For i As Int32 = 1 To fieldcount
    '            ' creating columns
    '            dc = New DataColumn("Column_" & i, GetType(String))
    '            dt.Columns.Add(dc)
    '        Next

    '    Catch ex As Exception
    '        HttpContext.Current.Response.Write("<hr>" & "FN.convertCSVtoDT P1 Error" & "<hr>" & "<br>")
    '        HttpContext.Current.Response.Write("<hr>" & " Error Message" & ex.Message.ToString & "<br>")
    '        HttpContext.Current.Response.Write("File With Path:" & Path & "<br>")
    '        HttpContext.Current.Response.Write("File With Path:" & CSVFILE.ToString & "<br>")
    '        HttpContext.Current.Response.Write("Error convertCSVtoDT: " & fieldcount)
    '        HttpContext.Current.Response.Write("dc.ColumnName: " & dc.ColumnName.ToString)

    '    End Try

    '    Try
    '        ' crerating rows
    '        For Each line In CSVFILE
    '            Row = dt.NewRow()
    '            For f As Integer = 0 To fieldcount - 1
    '                ' popuating fields
    '                Row(f) = MiscClass.MyReplace(Fields(f).ToString, Chr(34), "")
    '            Next
    '            dt.Rows.Add(Row)

    '        Next

    '    Catch ex As Exception
    '        HttpContext.Current.Response.Write("<hr>" & "FN.convertCSVtoDT P2 Error" & "<hr>" & "<br>")
    '        HttpContext.Current.Response.Write("<hr>" & " Error Message" & ex.Message.ToString & "<br>")
    '        HttpContext.Current.Response.Write("File With Path:" & Path & "<br>")
    '        HttpContext.Current.Response.Write("File With Path:" & CSVFILE.ToString & "<br>")
    '        HttpContext.Current.Response.Write("Error convertCSVtoDT: " & fieldcount)
    '    End Try
    'End Sub



End Class
