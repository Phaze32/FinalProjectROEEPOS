Imports System.Data
Imports System.Text
Partial Class POSRacking
    Inherits System.Web.UI.Page
    Private Sub posracking_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Dim strsql As String = "SELECT * FROM [Vw_Racking]"
        Dim dt_list As DataTable = DTClass.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
        Dim g As GridView = FindControl("GridView1")
        g.DataSource = dt_list
        g.DataBind()
    End Sub
End Class
