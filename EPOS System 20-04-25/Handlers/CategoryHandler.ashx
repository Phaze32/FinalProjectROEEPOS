<%@ WebHandler Language="C#" Class="CategoryHandler" %>

using System;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;

public class CategoryHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        try
        {
            // Get connection string from Web.config
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

            // Get Rec_id parameter from the query string
            string recId = context.Request.QueryString["rec_id"];

            // Construct SQL query based on whether Rec_id is provided
            string query;
            if (!string.IsNullOrEmpty(recId))
            {
                query = @"
                    SELECT [Rec_id], [Department], trim([DeparmentName])[DeparmentName], [ShopID]
                    FROM [DST].[dbo].[tblDeparments]
                    WHERE [Rec_id] = @Rec_id";
            }
            else
            {
                query = @"
                    SELECT TOP (1000) [Rec_id], [Department], trim([DeparmentName])[DeparmentName], [ShopID]
                    FROM [DST].[dbo].[tblDeparments]";
            }

            // Execute SQL query and return the data
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(recId))
                    {
                        // Add Rec_id as a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@Rec_id", recId);
                    }

                    SqlDataReader reader = cmd.ExecuteReader();
                    var departments = new System.Collections.Generic.List<object>();

                    while (reader.Read())
                    {
                        departments.Add(new
                        {
                            Rec_id = reader["Rec_id"],
                            Department = reader["Department"],
                            DeparmentName = reader["DeparmentName"],
                            ShopID = reader["ShopID"]
                        });
                    }

                    // Serialize data to JSON and write it to the response
                    context.Response.Write(JsonConvert.SerializeObject(departments));
                }
            }
        }
        catch (Exception ex)
        {
            // Handle errors and return as JSON response
            var errorResponse = new { error = ex.Message };
            context.Response.Write(JsonConvert.SerializeObject(errorResponse));
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}