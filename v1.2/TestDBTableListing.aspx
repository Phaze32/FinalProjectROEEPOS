<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<!DOCTYPE html>
<html>
<head>
    <title>List Time Records</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>List of Time Records</h2>
        <asp:Label ID="lblResult" runat="server" Text="Result will be displayed here." />
        <asp:GridView ID="gvTimeRecords" runat="server" AutoGenerateColumns="true"></asp:GridView>

        <script runat="server">
            protected void Page_Load(object sender, EventArgs e)
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

                DataTable dataTable = new DataTable();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "SELECT * FROM tblTimeRecord";
                        SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            gvTimeRecords.DataSource = dataTable;
                            gvTimeRecords.DataBind();
                        }
                        else
                        {
                            lblResult.Text = "No records found.";
                        }
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
                        lblResult.Text = "General Error: " + ex.Message + "<br/>" + ex.StackTrace;
                    }
                }
            }
        </script>
    </form>
</body>
</html>
