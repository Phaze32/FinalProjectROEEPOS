<%@ Page Language="C#" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
 <%@ Import Namespace="System.Data" %>
<!DOCTYPE html>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        // Handle AJAX requests
        string action = Request.QueryString["act"];
        if (action == "addcustomer")
        {
            AddCustomer();
        }
        else if (action == "searchcustomers")
        {
            Response.Write("");
            SearchCustomers();
        }
    }

    private void AddCustomer()
    {
        // Retrieve query string parameters
        var queryString = Request.QueryString.ToString();
        var tableName = "[dbo].[tblCustomer]";
        var connectionString = "DSTConnectionString";
        var sqlquery = ConvertQueryStringToInsert(queryString, tableName);

        // Execute the query to add the customer
        ExecuteNonQuery(sqlquery, connectionString);

        // Return a success message
        Response.ContentType = "text/plain";
        Response.Write("Customer added successfully!");
        Response.End();
    }

    private void SearchCustomers()
    {
        // Retrieve query string parameters
        string query = Request.QueryString["query"];
        var tableName = "[dbo].[tblcustomer]";
        var connectionString = "DSTConnectionString";
        var sqlquery = "SELECT * FROM " + tableName + " WHERE Forename LIKE @query OR Surname LIKE @query OR [Telephone] like @query";
        
        // Execute the query to search for customers
        DataSet ds = ExecuteQueryWithParameters(sqlquery, connectionString, query);

        // Convert the results to JSON
        var customers = new List<object>();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            customers.Add(new
            {
                id = row["CustomerID"],
                name = row["Forename"] + " " + row["Surname"],
                CustomerAccNo = row["CustomerAccNo"].ToString().Trim()
            });
        }
        var json = new JavaScriptSerializer().Serialize(customers);

        // Return the JSON response
        Response.ContentType = "application/json";
        Response.Write(json);
        Response.End();
    }

    public static string ConvertQueryStringToInsert(string queryString, string tableName, string excludekey = "act")
    {
        var queryParameters = HttpUtility.ParseQueryString(queryString);
        var columns = new List<string>();
        var values = new List<string>();

        foreach (string key in queryParameters)
        {
            if (!key.Equals(excludekey, StringComparison.OrdinalIgnoreCase) && !key.Equals("_"))
            {
                columns.Add("[" + key + "]");
                values.Add("'" + queryParameters[key] + "'");
            }
        }

        string columnsAndParameters = string.Join(", ", columns);
        string valuesString = string.Join(", ", values);

        string query = "INSERT INTO [" + tableName + "] (" + columnsAndParameters + ") VALUES (" + valuesString + ")";
        return query;
    }

    public static DataSet ExecuteQueryWithParameters(string query, string connectionStringName, string queryParam)
    {
        // Retrieve the connection string from the web.config file
        string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@query", "%" + queryParam + "%");
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataSet dataSet = new DataSet();
                    try
                    {
                        connection.Open();
                        adapter.Fill(dataSet);
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                    return dataSet;
                }
            }
        }
    }

    public static void ExecuteNonQuery(string query, string connectionStringName = "DTSconnectionstring")
    {
        // Retrieve the connection string from the web.config file
        string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Literal EnableViewState="false" ID="testresponse" runat="server" Text="Label"></asp:Literal>
        </div>
    </form>
</body>
</html>
