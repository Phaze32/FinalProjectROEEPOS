Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Web.HttpContext
Imports System.IO
Imports System.Text
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.ClientScriptManager
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Public Class ExportPrint
    Protected Sub ExportToExcel(ByRef gridname As String, ByRef datasource As SqlDataSource, ByRef SaveToFileName As String)
        Dim mfilename As String = SaveToFileName
        Dim dg1 As New DataGrid()
        dg1.DataSource = datasource
        dg1.DataBind()
        ExportToExcel(mfilename, dg1)
        dg1 = Nothing
        dg1.Dispose()
    End Sub

    Private Sub ExportToExcel(strFileName As String, dg As DataGrid)
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & strFileName)
        HttpContext.Current.Response.ContentType = "application/excel"
        Dim sw As New System.IO.StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        dg.RenderControl(htw)
        HttpContext.Current.Response.Write(sw.ToString())
        HttpContext.Current.Response.End()
    End Sub
    ''' <summary>
    ''' saves sql statement into a csv file
    ''' </summary>
    ''' <param name="filedsToget">fields to be used in csvfile</param>
    ''' <param name="fTableName">table or view to be used to get the fields from </param>
    ''' <param name="fCriteria">criteria used if none then use 1 = 1 and cant be left blank </param>
    ''' <param name="mgroupby">group by clause if neede eles send a blank ""</param>
    ''' <param name="mcolumnname">optional prints column name undless fed false</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ReturnDataSetAsCSV(ByVal filedsToget As String, ByVal fTableName As String, ByVal fCriteria As String, mgroupby As String, Optional mcolumnname As Boolean = 1) As String
        'Declaration of Variables
        Dim SQLconstring As String = "Escapade_NewConnectionString"

        If mgroupby <> "" Then
            mgroupby = " Group by " & mgroupby
        End If

        Dim fReturnData As String = "nothing"
        Dim bFirstRecord As Boolean = True
        Dim SqlString As String = "SELECT " & filedsToget & " FROM " & fTableName & " WITH(NOLOCK) where " & fCriteria & mgroupby

        '        Dim myWriter As New System.IO.StreamWriter(filetoSaveWithPAth)
        Dim conStr As String = "Provider=SQLOLEDB;" & System.Configuration.ConfigurationManager.ConnectionStrings(SQLconstring).ConnectionString
        Dim myString As New System.Text.StringBuilder()
        Dim dbConnection As New OleDbConnection(conStr)


        Dim ds As New DataSet
        dbConnection.Open()

        Dim command As New OleDbCommand(SqlString, dbConnection)
        Dim da As New OleDbDataAdapter(command)
        Dim dr_column_count As Integer = 0
        Dim titlegiven As Boolean = False
        Try
            da.Fill(ds)
            Console.WriteLine("Made connection to the database")
            For Each dr As DataRow In ds.Tables(0).Rows
                dr_column_count = dr.Table.Columns.Count - 1
                If mcolumnname And Not titlegiven Then
                    For Each c As DataColumn In dr.Table.Columns
                        'loops through the columns. 
                        myString.Append(Chr(34) & c.ColumnName & Chr(34))
                        If dr_column_count > 0 Then
                            myString.Append(",")
                        End If
                        '    HttpContext.Current.Response.Write("c.ColumnName=" & c.ColumnName & "<br/>")
                        dr_column_count = dr_column_count - 1
                    Next
                    myString.Append(Environment.NewLine)
                    titlegiven = True

                End If
                bFirstRecord = True
                For Each field As Object In dr.ItemArray
                    If Not bFirstRecord Then
                        myString.Append(",")
                    End If
                    'replacing bad data extra " and , to make it run as csv
                    myString.Append(Chr(34) & field.ToString.Replace(Chr(34), "'") & Chr(34))
                    bFirstRecord = False
                Next
                'New Line to differentiate next row
                myString.Append(Environment.NewLine)
            Next
            fReturnData = myString.ToString
        Catch ex As Exception
            HttpContext.Current.Response.Write(SqlString & "<br/>")
            HttpContext.Current.Response.Write(filedsToget & "<br/>")
            HttpContext.Current.Response.Write(fTableName & "<br/>")
            '        HttpContext.Current.Response.Write(filetoSaveWithPAth & "<br/>")
            HttpContext.Current.Response.Write(SqlString & "<br/>")
            '      HttpContext.Current.Response.End()
            HttpContext.Current.Response.Write(ex.Message)
            MsgBox(ex.Message)
            ' HttpContext.Current.Response.End()
        End Try

        '      End Using

        Return fReturnData


    End Function
    'Function savecsv(ByRef filenameTosave As String, ByRef datatosavwe As String) As String
    '    Dim str As New StringBuilder
    '    Dim dataset As New DataSet
    '    For Each dr As DataRow In dataset.
    '        For Each field As Object In dr.ItemArray
    '        str.Append(field.ToString & ",")
    '    Next
    '    str.Replace(",", vbNewLine, str.Length - 1, 1)
    '    Next
    '    Try
    '        My.Computer.FileSystem.WriteAllText("C:\temp\testcsv.csv", str.ToString, False)
    '    Catch ex As Exception
    '        HttpContext.Current.Response.Write(ex.ME)
    '    End Try
    'End Function
    'Protected Sub PrintAllPages(ByRef gridname As String, ByRef datasource As SqlDataSource)
    '    Dim GridView1 As New GridView()
    '    GridView1.ID = gridname
    '    GridView1.AllowPaging = False
    '    GridView1.DataBind()
    '    Dim sw As New StringWriter()
    '    Dim hw As New HtmlTextWriter(sw)
    '    GridView1.RenderControl(hw)
    '    Dim gridHTML As String = sw.ToString().Replace("""", "'") _
    '         .Replace(System.Environment.NewLine, "")
    '    Dim sb As New StringBuilder()
    '    sb.Append("<script type = 'text/javascript'>")
    '    sb.Append("window.onload = new function(){")
    '    sb.Append("var printWin = window.open('', '', 'left=0")
    '    sb.Append(",top=0,width=1000,height=1000,status=0');")
    '    sb.Append("printWin.document.write(""")
    '    sb.Append(gridHTML)
    '    sb.Append(""");")
    '    sb.Append("printWin.document.close();")
    '    sb.Append("printWin.focus();")
    '    sb.Append("printWin.print();")
    '    sb.Append("printWin.close();};")
    '    sb.Append("</script>")
    '    ClientScript.RegisterStartupScript(Me.[GetType](), "GridPrint", sb.ToString())
    '    GridView1.AllowPaging = True
    '    GridView1.DataBind()
    'End Sub

    'Protected Sub PrintCurrentPage(ByRef gridname As String, ByRef datasource As SqlDataSource)
    '    Dim GridView1 As New GridView()
    '    GridView1.ID = gridname
    '    GridView1.PagerSettings.Visible = False
    '    GridView1.DataBind()
    '    Dim sw As New StringWriter()
    '    Dim hw As New HtmlTextWriter(sw)
    '    GridView1.RenderControl(hw)
    '    Dim gridHTML As String = sw.ToString().Replace("""", "'") _
    '       .Replace(System.Environment.NewLine, "")
    '    Dim sb As New StringBuilder()
    '    sb.Append("<script type = 'text/javascript'>")
    '    sb.Append("window.onload = new function(){")
    '    sb.Append("var printWin = window.open('', '', 'left=0")
    '    sb.Append(",top=0,width=1000,height=600,status=0');")
    '    sb.Append("printWin.document.write(""")
    '    sb.Append(gridHTML)
    '    sb.Append(""");")
    '    sb.Append("printWin.document.close();")
    '    sb.Append("printWin.focus();")
    '    sb.Append("printWin.print();")
    '    sb.Append("printWin.close();};")
    '    sb.Append("</script>")
    '    ClientScriptManager.ClientScript.RegisterStartupScript(Me.GetType(), "GridPrint", sb.ToString())
    '    GridView1.PagerSettings.Visible = True
    '    GridView1.DataBind()
    'End Sub

End Class
