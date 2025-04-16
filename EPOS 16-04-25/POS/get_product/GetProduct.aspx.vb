
Partial Class POS_get_product_GetProduct_
    Inherits System.Web.UI.Page
    Shared productinfo As String = ""
    Shared pluid As String = ""
    Private Sub POS_get_product_GetProduct__LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Request.QueryString("product_id") <> "" Then pluid = Request.QueryString("product_id")
        productinfo = SQLFunctions.GetSearchDataSQLWithOutConStr("select description +','+ convert(varchar(5),round(price,2)) from [DST].[dbo].[tblPLU] where PLUID=" & pluid & " ;")
        Label1.Text = "." 'productinfo
    End Sub
End Class
