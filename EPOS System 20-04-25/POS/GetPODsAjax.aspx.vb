Imports System.Data
Partial Class POS_GetPODsAjax
    Inherits System.Web.UI.Page

    Private Sub getpods(strsql As String)
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
        Response.Write(dt.Rows.Count())
        Literal1.Text = GetProductPod(dt)
    End Sub
    Private Sub createpods(ByRef department As String, Optional submenuid As String = "0")
        department = TextCSVClass.CSVStringToQualifierSQLString(department)
        Dim strsql As String = "IF EXISTS (SELECT " & submenuid & " where " & submenuid & " = '0')"
        strsql += " begin "
        strsql += " Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
                & "   where 1=1   And department in (" & department & ");"
        strsql += " end "
        strsql += " else "
        strsql += " begin "
        strsql += " Select ProductImage,Description,round(price,2) price,PLUID FROM [dbo].[Vw_SubmenuItems] " _
                & "   where 1=1   And [SubMenu_ID] in (" & submenuid & ");"
        strsql += " end "
        'Dim ss As String = "IF EXISTS (SELECT 1 FROM Pages_Language WHERE Language='" & department & "' and Page_ID=" & Page_ID & ") BEGIN"
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
        Literal1.Text = GetProductPod(dt)
    End Sub


    Private Sub POS_GetPODsAjax_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Dim department As String = Request.QueryString("DepartmentList")
        Dim smid As String = Request.QueryString("smid")
        If smid = "" Then smid = "0"
        createpods(department, smid)
    End Sub

    Shared Function GetProductPod(dt As DataTable) As String
        Dim rv As String = "" '"<script type = ""text/javascript"" > Function() TestAlert() {alert(""Test ok!"");}</script>"
        rv += "<div style='float:left;width:100%;'>"
        'HttpContext.Current.Response.Write("###" & DTClass.GetDTColumnNames(dt))
        For Each row In dt.Rows
            Dim imagefile As String = "6988655f95602f9394f9315165f920fe.png"
            If row("ProductImage").ToString <> "" Then imagefile = row("ProductImage").ToString

            rv += "<button type=""button"" data-name=" & row("Description").ToString & " id=""" & "Butt_" & row("PLUID").ToString & """ " _
                & "  value='" & row("PLUID").ToString & "' class=""btn btn-both btn-flat ""  OnClick='myFunc(" & row("PLUID").ToString & ")'><span class=""bg-img"">" _
                & " <img src=""../Images/POSImages/" & imagefile & """ alt='" & row("Description").ToString & "' " _
                & "  style=""width: 100px; height: 100px;""></span>" _
               & "<span><span>" & row("Description").ToString & " " & MiscClass.myCurrencyFormatWithSymbol(row("price"), 2, "GBP") & "</span></span></button>"
        Next
        rv = rv & "</div>"
        Return rv
    End Function
End Class
