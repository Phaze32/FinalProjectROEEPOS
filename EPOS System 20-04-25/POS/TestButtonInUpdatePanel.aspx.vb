Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Partial Class POS_TestButtonInUpdatePanel
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        txtBox.Text = "Page_Loaded"
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
             & "   where 1=1   And department ='d';"

        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
        txtBox.Text = (txtBox.Text + ("" & vbLf + DateTime.Now.ToString("mm:ss:fff")))
        Literal1.Text = GetProductPod(dt)
    End Sub

    Protected Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As EventArgs)
        txtBox.Text = (txtBox.Text + "" & vbLf & "PreRenderComplete")
    End Sub

    Shared Function GetProductPod(dt As DataTable) As String
        Dim rv As String = "" ' "<div style='float:left;width:100%;'>"
        'HttpContext.Current.Response.Write("###" & DTClass.GetDTColumnNames(dt))
        For Each row In dt.Rows
            Dim imagefile As String = "6988655f95602f9394f9315165f920fe.png"
            If row("ProductImage").ToString <> "" Then imagefile = row("ProductImage").ToString

            'rv += "<button type=""button"" data-name=" & row("Description").ToString & " id=""" & "Butt_" & row("PLUID").ToString & """ " _
            '    & "  value='" & row("PLUID").ToString & "' class=""btn btn-both btn-flat product""><span class=""bg-img"">" _
            '    & " <img src=""../Images/POSImages/" & imagefile & """ alt='" & row("Description").ToString & "' " _
            '    & "  style=""width: 100px; height: 100px;""></span>" _
            '   & "<span><span>" & row("Description").ToString & " " & MiscClass.myCurrencyFormatWithSymbol(row("price"), 2, "GBP") & "</span></span></button>"


            rv += "<Button type=""button"" data-name=" & row("Description").ToString & " id=""" & "Butt_" & row("PLUID").ToString & """ " _
                & "  value='" & row("PLUID").ToString & "' class=""btn btn-both btn-flat product"" OnClick='base_url()' ><span class=""bg-img"">" _
                & " <img src=""../Images/POSImages/" & imagefile & """ alt='" & row("Description").ToString & "' " _
                & "  style=""width: 100px; height: 100px;""></span>" _
               & "<span><span>" & row("Description").ToString & " " & MiscClass.myCurrencyFormatWithSymbol(row("price"), 2, "GBP") & "</span></span></button>"
        Next
        Return rv
    End Function
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
            & "   where 1=1   And department ='d';"
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
        Literal1.Text = GetProductPod(dt)
    End Sub
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
     & "   where 1=1   and    department in ('L','W') ;"
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
        Literal1.Text = GetProductPod(dt)
    End Sub
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
     & "   where 1=1   and    department in ('r') ;"
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
        Literal1.Text = GetProductPod(dt)
    End Sub
    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
                & "   where 1=1 and department not in ('r','d') and  department is not null;"
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
        Literal1.Text = GetProductPod(dt)
    End Sub
    Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
        & "   where 1=1 and  department is not null;"
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
        Literal1.Text = GetProductPod(dt)
    End Sub
End Class
