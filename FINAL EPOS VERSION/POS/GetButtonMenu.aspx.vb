Imports System.Data
Partial Class POS_GetButtonMenu
    Inherits System.Web.UI.Page

    Private Sub POS_GetButtonMenu_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Dim department As String = Request.QueryString("DepartmentList")
        Submenu(department)
    End Sub
    Private Sub Submenu(ByRef department As String)
        department = TextCSVClass.CSVStringToQualifierSQLString(department)
        Dim strsql As String = "Select [Rec_id],[Department],[DeparmentName],[ShopID] FROM [DST].[dbo].[tblDeparments] " _
                & "   where 1=1 ;"
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
        Literal1.Text = GetSubmenu(dt)

    End Sub
    Shared Function GetSubmenu(dt As DataTable) As String
        Dim rv As String = "" '"<script type = ""text/javascript"" > Function() TestAlert() {alert(""Test ok!"");}</script>"
        rv += "<div id ='ButtonMenu'>"
        'HttpContext.Current.Response.Write("###" & DTClass.GetDTColumnNames(dt))
        Dim num As Int16 = 1
        For Each row In dt.Rows
            Dim imagefile As String = "6988655f95602f9394f9315165f920fe.png"
            Dim mDeparmentName As String = row("DeparmentName").ToString
            Dim mDepartment As String = row("Department").ToString
            'If row("ProductImage").ToString <> "" Then imagefile = row("ProductImage").ToString
            If mDepartment = "A" Then mDepartment = "D,H,L,R,S,W"
            rv += "<Button ID='btn_FetchPods" & num & "'  Text='Dryclean'  Height='70' Width='70' style=""height:70px;Width=70px"" onclick=""FetchPOds('" & mDepartment & "','0'); return false;"">" & mDeparmentName & "</button>"
            num = num + 1
        Next
        'rv += "<Button ID='btn_FetchPods'  Text='Dryclean'  Height='70' Width='70'  onclick=""FetchPOds('D'); return false;"">Dryclean</button>"
        'rv += "<Button ID='btn_FetchPods0'  Text='Repairs'  Height='70' Width='70'  onclick=""FetchPOds('R'); return false;""  >Repairs</button>"
        'rv += "<Button ID='btn_FetchPods1'  Text='Laundry'  Height='70' Width='70'  onclick=""FetchPOds('L,W'); return false;"" >Laundry</button>"
        'rv += "<Button ID='btn_FetchPods2'  Text='Laundry'  Height='70' Width='70'  onclick=""FetchPOds(''H,S'); return false;"" >Stain Removal</button>"
        'rv += "<Button ID='btn_FetchPods99'  Text='All'  Height='70' Width='70'  onclick=""FetchPOds('D,H,L,R,S,W'); return false;"">All</button>"
        rv = rv & "</div>"
        Return rv
    End Function
End Class
