<%@ Page Language="VB" %>

<!DOCTYPE html>

  <script runat="server">
      Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
          ' Set the current time
          currentTime.Text = DateTime.Now.ToString("HH:mm:ss")

          ' Set the ticket ID (from query string or default to 66)
          If Request.QueryString("ticketID") IsNot Nothing Then
              ticketID.Text = Request.QueryString("ticketID")
          Else
              ticketID.Text = "66"
          End If
      End Sub
      Protected Sub UpdateAddCustomerData()
          Dim @CustomerAccNo, @Title, @Forename, @Surname, @Company, @AddressName, @AddressStreet, @AddressTown, @AddressCounty, @Postcode, @NothingByPost, @Telephone, @Fax, @EMail, @AccountType as string
          Dim sqlstr As String = "INSERT INTO [DST].[dbo].[tblCustomer] 
                ([CustomerAccNo], [Title], [Forename], [Surname], [Company], [AddressName], [AddressStreet], [AddressTown], [AddressCounty], [Postcode], [NothingByPost], [Telephone], [Fax], [EMail], [AccountType])
                VALUES( 
                (@CustomerAccNo, @Title, @Forename, @Surname, @Company, @AddressName, @AddressStreet, @AddressTown, @AddressCounty, @Postcode, @NothingByPost, @Telephone, @Fax, @EMail, @AccountType)"

      End Sub
    </script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
                <asp:literal EnableViewState="false" ID="currentTime" runat="server" Text="Label"></asp:literal>
                <asp:literal EnableViewState="false" ID="ticketID" runat="server" Text="Label"></asp:literal>
        </div>
    </form>

</body>
</html>

