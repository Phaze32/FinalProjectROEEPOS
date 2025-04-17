<%@ WebHandler Language="C#" Class="SalesSummaryHandler" %>

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;
using System.Configuration;

public class SalesSummaryHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        // Set the response type to JSON
        context.Response.ContentType = "application/json";

        // SQL connection string (replace with your actual connection details)
        string connectionString = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

        // Determine whether to group by month or by day based on the query string
        string groupByClause;
        string orderByClause;
        bool isDaily = context.Request.QueryString["type"] != null && context.Request.QueryString["type"].ToLower() == "daily";

        if (isDaily)
        {
            // Group by day (daily totals)
            groupByClause = "YEAR(TicketDate), MONTH(TicketDate), DAY(TicketDate)";
            orderByClause = "year ASC, month ASC, day ASC";
        }
        else
        {
            // Group by month (monthly totals)
            groupByClause = "YEAR(TicketDate), MONTH(TicketDate)";
            orderByClause = "year ASC, month ASC";
        }

        // SQL query to fetch sales summary data dynamically based on query string
        string query = string.Format(@"
            SELECT 
                YEAR(TicketDate) AS year, 
                MONTH(TicketDate) AS month, 
                {0}
                ROUND(SUM(CONVERT(float, TicketTotal)), 0) AS TicketTotal, 
                ROUND(SUM(CONVERT(int, PiecesTotal)), 0) AS PiecesTotal
            FROM 
                dbo.tblTicket
            GROUP BY 
                {1}
            ORDER BY 
                {2}", isDaily ? "DAY(TicketDate) AS day," : "", groupByClause, orderByClause);

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Load the data into a DataTable
                    DataTable table = new DataTable();
                    table.Load(reader);

                    // Convert the DataTable to JSON
                    string jsonResponse = JsonConvert.SerializeObject(table);

                    // Write the JSON response to the client
                    context.Response.Write(jsonResponse);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle errors and return a meaningful error response
            context.Response.StatusCode = 500;
            context.Response.Write("Error: " + ex.Message);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}