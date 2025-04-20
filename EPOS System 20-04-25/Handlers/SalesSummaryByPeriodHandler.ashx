<%@ WebHandler Language="C#" Class="SalesSummaryByPeriodHandler" %>

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;
using System.Configuration;

public class SalesSummaryByPeriodHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        // Set the response type to JSON
        context.Response.ContentType = "application/json";

        // SQL connection string
        string connectionString = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

        // Get the mode from the query string, default to "year"
        string mode = context.Request.QueryString["mode"];
        string groupByClause = "YEAR(TicketDate)";
        string selectClause = "YEAR(TicketDate) AS Year";

        // Determine the grouping logic based on mode
        switch (mode != null ? mode.ToLower() : "year")
        {
            case "day":
                groupByClause = "YEAR(TicketDate), MONTH(TicketDate), DAY(TicketDate)";
                selectClause = "YEAR(TicketDate) AS Year, MONTH(TicketDate) AS Month, DAY(TicketDate) AS Day";
                break;

            case "month":
                groupByClause = "YEAR(TicketDate), MONTH(TicketDate)";
                selectClause = "YEAR(TicketDate) AS Year, MONTH(TicketDate) AS Month";
                break;

            case "quarter":
                groupByClause = "YEAR(TicketDate), DATEPART(QUARTER, TicketDate)";
                selectClause = "YEAR(TicketDate) AS Year, DATEPART(QUARTER, TicketDate) AS Quarter";
                break;

            case "year":
            default:
                // Default grouping by year
                groupByClause = "YEAR(TicketDate)";
                selectClause = "YEAR(TicketDate) AS Year";
                break;
        }

        // SQL query to fetch the summarized records
        string query = string.Format(@"
            SELECT 
                {0},
                ROUND(SUM(CONVERT(float, TicketTotal)), 0) AS TotalTicketSales,
                ROUND(SUM(CONVERT(int, PiecesTotal)), 0) AS TotalPiecesSold
            FROM [DST].[dbo].[Vw_OrderDetail_Trans]
            GROUP BY {1}
            ORDER BY {1}", selectClause, groupByClause);

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