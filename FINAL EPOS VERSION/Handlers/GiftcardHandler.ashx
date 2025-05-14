<%@ WebHandler Language="C#" Class="Handlers.GiftCardHandler" %>

using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;

namespace Handlers
{
    public class GiftCardHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string connectionString = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

            // Get the GiftCardID parameter from the query string
            string giftCardID = context.Request["GiftCardID"];

            string query;

            if (!string.IsNullOrEmpty(giftCardID))
            {
                // Query to filter specific GiftCardID
                query = @"SELECT 
                            [GiftCardID], 
                            [CardNumber], 
                            [CardName], 
                            [Balance], 
                            [DiscountPercentage], 
                            [IsActive], 
                            [CreatedDate], 
                            [ExpiryDate] 
                          FROM [DST].[dbo].[tblGiftCard]
                          WHERE GiftCardID = @GiftCardID";
            }
            else
            {
                // Query to retrieve all records
                query = @"SELECT 
                            [GiftCardID], 
                            [CardNumber], 
                            [CardName], 
                            [Balance], 
                            [DiscountPercentage], 
                            [IsActive], 
                            [CreatedDate], 
                            [ExpiryDate] 
                          FROM [DST].[dbo].[tblGiftCard]";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(giftCardID))
                    {
                        // Add SQL parameter for filtering
                        command.Parameters.AddWithValue("@GiftCardID", giftCardID);
                    }

                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Create JSON response
                    var jsonResponse = new
                    {
                        data = dataTable
                    };

                    string jsonResult = JsonConvert.SerializeObject(jsonResponse);
                    context.Response.Write(jsonResult);
                }
            }
        }

        public bool IsReusable
        {
            get { return false; } // Explicit getter for C# 5 compatibility
        }
    }
}