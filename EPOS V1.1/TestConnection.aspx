<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<!DOCTYPE html>
<html>
<head>
    <title>Test Database Connection</title>
</head>
<body>
    <h2>Test Database Connection</h2>
    <asp:Label ID="lblResult" runat="server" Text="Result will be displayed here." />

    <script runat="server">
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = "Server=Dorkus_Primus_2\\SQLEXPRESS;Database=DST;User Id=sr2;Password=Redcat17;";

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    lblResult.Text = "Connection successful!";
                    // Optional: Perform a simple query
                    var command = new SqlCommand("SELECT 1", connection);
                    command.ExecuteScalar();
                    lblResult.Text += "<br />Query executed successfully! with connection string = " + connectionString ;
                }
                catch (SqlException sqlEx)
                {
                    lblResult.Text = "SQL Error: " + sqlEx.Message;
                    foreach (SqlError error in sqlEx.Errors)
                    {
                        lblResult.Text += string.Format("<br/>Error: {0} - {1}", error.Number, error.Message);
                    }
                }
                catch (Exception ex)
                {
                    lblResult.Text = "General Error: " + ex.Message;
                }
            }
        }
    </script>
</body>
</html>
