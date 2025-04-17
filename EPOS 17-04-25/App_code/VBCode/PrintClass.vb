Imports Microsoft.VisualBasic

Public Class PrintClass






    'Protected Sub PrintCurrentPage(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintCurrent.Click
    '    Try
    '        GridView21.PagerSettings.Visible = False
    '        GridView21.DataBind()
    '        Dim sw As New StringWriter()
    '        Dim hw As New HtmlTextWriter(sw)
    '        VerifyRenderingInServerForm(GridView21)
    '        GridView21.RenderControl(hw)
    '        Dim gridHTML As String = sw.ToString().Replace("""", "'") _
    '           .Replace(System.Environment.NewLine, "")
    '        Dim sb As New StringBuilder()
    '        sb.Append("<script type = 'text/javascript'>")
    '        sb.Append("window.onload = new function(){")
    '        sb.Append("var printWin = window.open('', '', 'left=0")
    '        sb.Append(",top=0,width=1000,height=600,status=0');")
    '        sb.Append("printWin.document.write(""")
    '        sb.Append(gridHTML)
    '        sb.Append(""");")
    '        sb.Append("printWin.document.close();")
    '        sb.Append("printWin.focus();")
    '        sb.Append("printWin.print();")
    '        sb.Append("printWin.close();};")
    '        sb.Append("</script>")
    '        ClientScript.RegisterStartupScript(Me.GetType(), "GridPrint", sb.ToString())
    '        GridView21.PagerSettings.Visible = True
    '        GridView21.DataBind()
    '    Catch ex As Exception
    '        HttpContext.Current.Response.Write(ex.Message)
    '        HttpContext.Current.Response.End()
    '    End Try

    'End Sub
    'Public Overrides Sub VerifyRenderingInServerForm(control As Control)
    '    ' Verifies that the control is rendered 

    'End Sub

    'Protected Sub PrintAllPages(ByVal sender As Object, ByVal e As EventArgs)
    '    GridView21.AllowPaging = False
    '    GridView21.DataBind()
    '    Dim sw As New StringWriter()
    '    Dim hw As New HtmlTextWriter(sw)
    '    GridView21.RenderControl(hw)
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
    '    GridView21.AllowPaging = True
    '    GridView21.DataBind()
    'End Sub
End Class
