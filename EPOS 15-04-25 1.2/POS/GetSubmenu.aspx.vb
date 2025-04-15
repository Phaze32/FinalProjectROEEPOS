Imports System.Data
Partial Class POS_GetSubmenu
    Inherits System.Web.UI.Page

    Private Sub POS_GetSubmenu_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Dim department As String = Request.QueryString("Department_Code")
        Submenu(department)
    End Sub
    Private Sub Submenu(ByRef department As String)
        department = TextCSVClass.CSVStringToQualifierSQLString(department)
        Dim strsql As String = "SELECT TOP (1000) [DeparmentName],[SubmenuItemName],[Department],[SubMenuItemID]" _
                & " FROM [DST].[dbo].[Vw_MenuSubMenu] " _
                & " where 1=1   And department In (" & department & ");"
        'Response.Write("strsql=" & strsql)
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
        Literal1.Text = GetSubmenu(dt)

    End Sub
    Shared Function GetSubmenu(dt As DataTable) As String
        Dim qs As String = HttpContext.Current.Request.QueryString("DepartmentList")
        If qs = "" Then qs = "null"
        Dim rv As String = "" '"<script type = ""text/javascript"" > Function() TestAlert() {alert(""Test ok!"");}</script>"
        rv += "<div style='float:left;width:100%;background-color:aqua'>"
        'HttpContext.Current.Response.Write("###" & DTClass.GetDTColumnNames(dt))
        Dim dtname As String = DTClass.GetDTColumnNames(dt)
        Dim cnter As String = 0
        Dim itemName As String = "6988655f95602f9394f9315165f920fe.png"
        For Each row In dt.Rows
            Dim mDeparmentName As String = row("DeparmentName").ToString
            Dim mDepartment As String = row("Department").ToString
            Dim mSubmenuID As String = row("SubMenuItemID").ToString
            If row("SubmenuItemName").ToString <> "" Then itemName = row("SubmenuItemName").ToString
            cnter = cnter + 1
            itemName = cnter & "." & itemName
            rv += "<button type='button''style='height:70px;width:70px;'  onclick=""FetchPOds('" & mDepartment & "','" & mSubmenuID & "'); return false;"">" & itemName & "-" & mSubmenuID & "</button>"
        Next

        ' rv += "xx" & dtname & qs & "zz" ' <-for testing only
        'rv += rv + dt.Rows.Count.ToString
        'rv += "#####<button type='button''style='height:70px;width:70px;'>HEHE</button>"
        rv = rv & "</div>"
        Return rv
    End Function
End Class
