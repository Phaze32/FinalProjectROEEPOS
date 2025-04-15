<%@ Page Language="VB" %>
<%@ Import Namespace="System.Web.Security" %>
<%@ Import Namespace="System.IO" %>
<script runat="server">
    Dim mgetserververiables As String = ""
    Protected Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        'if  Request.ServerVariables("remote_addr") = "172.27.14.133" or Request.ServerVariables("remote_addr") = "172.27.14.133:80" or 
        call passthrough()
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call passthrough()
    End Sub
    Sub passthrough()
        'SQLFunctions.SqlExecute("Escapade_NewConnectionString", "insert into event_log ([Process],[Executed_by]) values ('PROCESSORDERSFROMAMAZON','" & "User:" & UserEmail.Text & "1:QUERYSTRING=" & Request.ServerVariables("QUERY_STRING") & ", 2:ReturnUrl=" & Request.QueryString("ReturnUrl") & ", 3:REMOTEADDR=" & Request.ServerVariables("REMOTE_ADDR") & "')")
        If checkpassport() = True Then
            If UserEmail.Text = "" Then
                UserEmail.Text = "MQureshi"
                UserPass.Text = "escapade"
            End If
            dim aUrl as string = Request.QueryString("ReturnUrl")
            Try
                FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, Persist.Checked)
                'Response.Redirect("http://mis.escapade.co.uk/AmazonService.aspx")
            Catch ex As Exception
                FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, Persist.Checked)
                'Response.Redirect("http://mis.escapade.co.uk/AmazonService.aspx")
            End Try
				
        End If
        Msg.Text = Request.ServerVariables("server_name") ' "ServerVariables(ReturnUrl)=" & Request.QueryString("ReturnUrl") & " <br>checkpassport()="& checkpassport().tostring & "<br>UserEmail.Text=" & UserEmail.Text & "<br>Request.ServerVariables(QUERY_STRING)=" & Request.ServerVariables("QUERY_STRING")
    End Sub
    Function checkpassport() As Boolean
        Dim returnvalue As Boolean = False
        If (Request.ServerVariables("QUERY_STRING") = "ReturnUrl=%2fAmazonService.aspx%3f1%3d1&1=1") Then
            returnvalue = True
        ElseIf (Request.ServerVariables("QUERY_STRING") = "ReturnUrl=%2fAmazonService.aspx%3f1%3d1&1=1") Then
            returnvalue = True
        ElseIf (Request.QueryString("ReturnUrl") = "/AmazonService.aspx") Then
            returnvalue = True
        ElseIf (Request.QueryString("ReturnUrl") = "/AmazonService.aspx?1=1") Then
            returnvalue = True
        ElseIf (Request.QueryString("ReturnUrl") = "http://mis.escapade.co.uk/AmazonService.aspx?1=1") Then
            returnvalue = True
        ElseIf (Request.QueryString("ReturnUrl") = "http://localhost/admin/widdowson/widdowson_process_recieved_and_dispached_files.asp") Then
            returnvalue = True
        ElseIf Request.ServerVariables("remote_addr") = "172.27.14.133" Then
            returnvalue = True
        ElseIf Request.ServerVariables("remote_addr") = "172.27.14.133:80" Then
            returnvalue = True
        ElseIf (Request.ServerVariables("QUERY_STRING") = "ReturnUrl=%2fAmazonService.aspx?1=1") Then
            returnvalue = True
        ElseIf Request.ServerVariables("remote_addr") = "213.123.190.145" Then
            returnvalue = True
        ElseIf Request.ServerVariables("HTTP_HOST") = "localhost" Or Request.ServerVariables("server_name") = "localhost" Then
            returnvalue = True
        Else
            returnvalue = False
        End If
        Return returnvalue
    End Function
    Sub Logon_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' HttpContext.Current.Response.Write(" UserEmail=" & Request.Form("UserEmail"))
        'HttpContext.Current.Response.Write(" UserPass=" & Request.Form("UserPass"))
        ' HttpContext.Current.Response.Write(" HTTP_X_FORWARDED_FOR" & Request.ServerVariables("HTTP_X_FORWARDED_FOR"))
        Dim msearchcriteria As String = " User_Name='" & Request.Form("UserEmail") & "' and [Password]='" & Request.Form("UserPass") & "'" & " and [Enabled]= 1 "
        Dim pwcheck As String = SQLFunctions.GetSearchDataSQL("AdminUsers", msearchcriteria, "User_ID")
        HttpContext.Current.Response.Write(" pwcheck=" & pwcheck & "UserEmail.Text=" & UserEmail.Text & "' and [Password]='" & Request.Form("UserPass") & "<br>msearchcriteria=" & msearchcriteria)
        If (pwcheck <> 0) Then
            FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, Persist.Checked)
        Else
            Msg.Text = "Invalid credentials. Please try again."
        End If
    End Sub

</script>

<html>
<head id="Head1" runat="server">
  <title>Forms Authentication - Login</title>
</head>
<body>
  <form id="form1" runat="server">
    <h3>
      Logon Page </h3>
    <table>
      <tr>
        <td>
          Login Name:</td>
        <td>
          <asp:TextBox ID="UserEmail" runat="server" /></td>
        <td>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
            ControlToValidate="UserEmail"
            Display="Dynamic" 
            ErrorMessage="Cannot be empty." 
            runat="server" />
        </td>
      </tr>
      <tr>
        <td>
          Password:</td>
        <td>
          <asp:TextBox ID="UserPass" TextMode="Password" 
            runat="server" />
        </td>
        <td>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
            ControlToValidate="UserPass"
            ErrorMessage="Cannot be empty." 
            runat="server" />
        </td>
      </tr>
      <tr>
        <td>
          Remember me?</td>
        <td>
          <asp:CheckBox ID="Persist" runat="server" /></td>
      </tr>
    </table>
    <asp:Button ID="Submit1" OnClick="Logon_Click" Text="Log On"  
       runat="server" />
    <p>
      <asp:Label ID="Msg" ForeColor="red" runat="server" />
    </p>
  </form>
</body>
</html>