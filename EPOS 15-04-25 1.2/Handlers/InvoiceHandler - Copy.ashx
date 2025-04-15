<%@ WebHandler Language="C#" Class="Handlers.InvoiceHandler" %>

using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;

namespace Handlers
{
    public class InvoiceHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string connectionString = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

            // Check if CustomerID is provided
            string customerID = context.Request.QueryString["CustomerID"];
            string query;

            if (!string.IsNullOrEmpty(customerID) && customerID != "-1")
            {
                // Query to filter invoices by CustomerID
                query = @"
                    SELECT TOP (1000) [TicketID], [TicketDate], [CollectDate], [CollectTime], [AlertDate], 
                           [ForeName], [SurName], [City], [PhoneNumber], [PostCode], [Deliver], [HotelsOwnWork], 
                           [Complete], [CustomerID], [TicketTotal], [PiecesTotal], [GarmentTotal]
                    FROM [DST].[dbo].[Vw_OrderDetail]
                    WHERE [CustomerID] = @CustomerID AND (SurName + ForeName) <> ''";
            }
            else
            {
                // Query to get all invoice data
                query = @"
                    SELECT TOP (1000) [TicketID], [TicketDate], [CollectDate], [CollectTime], [AlertDate], 
                           [ForeName], [SurName], [City], [PhoneNumber], [PostCode], [Deliver], [HotelsOwnWork], 
                           [Complete], [CustomerID], [TicketTotal], [PiecesTotal], [GarmentTotal]
                    FROM [DST].[dbo].[Vw_OrderDetail]
                    WHERE (SurName + ForeName) <> ''";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(customerID) && customerID != "-1")
                    {
                        // Add parameter for filtering
                        command.Parameters.AddWithValue("@CustomerID", customerID);
                    }

                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    var jsonResponse = new
                    {
                        data = dataTable
                    };

                    // Convert to JSON and send response
                    string jsonResult = JsonConvert.SerializeObject(jsonResponse);
                    context.Response.Write(jsonResult);
                }
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}