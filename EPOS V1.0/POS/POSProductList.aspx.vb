
Partial Class POSProductList
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("POSProductList.aspx?mode=summary")
    End Sub
    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Response.Redirect("POSProductList.aspx?mode=details")
    End Sub

    Private Sub POSProductList_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Button1.BackColor = Drawing.Color.LightPink
        Button4.BackColor = Drawing.Color.LightBlue
        Dim mmode As String = Request.QueryString("mode")
        If mmode = "summary" Then
            Button2.BackColor = Drawing.Color.LightPink
            Button3.BackColor = Drawing.Color.LightPink
            Button5.BackColor = Drawing.Color.LightPink
        Else
            Button2.BackColor = Drawing.Color.LightBlue
            Button3.BackColor = Drawing.Color.LightBlue
            Button5.BackColor = Drawing.Color.LightBlue
        End If
    End Sub
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim mmode As String = Request.QueryString("mode")
        If mmode = "summary" Then

        Else

        End If
    End Sub
End Class
