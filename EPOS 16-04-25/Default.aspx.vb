
Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("breadcrumbs") = Session("breadcrumbshome")
        Server.ScriptTimeout = 3600
        If Request.ServerVariables("HTTP_X_FORWARDED_FOR") = "213.123.190.145" Or Request.ServerVariables("HTTP_X_FORWARDED_FOR") = "213.123.190.145" Or Request.ServerVariables("HTTP_X_FORWARDED_FOR") = "81.201.136.253" Or Request.ServerVariables("HTTP_X_FORWARDED_FOR") = "192.168.2.249" Or Request.ServerVariables("REMOTE_ADDR") = "1" Or Request.ServerVariables("HTTP_HOST") = "localhost" Then
        Else
            'Response.Redirect("Unauthorised.aspx")
        End If
    End Sub
End Class


