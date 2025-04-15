<%@ WebHandler Language="C#" Class="SalesSummaryTodayHandler" %>

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;
using System.Configuration;

public class SalesSummaryTodayHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        // Set the response type to JSON
        context.Response.ContentType = "application/json";

        // SQL connection string
        string connectionString = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

        // Determine the number of top records from the query string or default to 10
        int topRecords = 10;
        if (context.Request.QueryString["top"] != null)
        {
            int.TryParse(context.Request.QueryString["top"], out topRecords);
        }

        // SQL query to fetch the top records dynamically
        string query = string.Format(@"
            SELECT TOP ({0})
                [TicketID],
                CAST([TicketDate] AS DATE) AS TicketDate,
                CAST([CollectDate] AS DATE) AS CollectDate,
                CAST([AlertDate] AS DATE) AS AlertDate,
                [SurName],
                [TicketTotal] AS total,
                [PiecesTotal] AS qty
            FROM [DST].[dbo].[Vw_OrderDetail_Trans]
            GROUP BY 
                [TicketID], 
                CAST([TicketDate] AS DATE), 
                CAST([CollectDate] AS DATE), 
                CAST([AlertDate] AS DATE), 
                [SurName], 
                [TicketTotal], 
                [PiecesTotal]
            ORDER BY TicketID DESC", topRecords);

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