<%@ WebHandler Language="C#" Class="CategorySaveHandler" %>

using System;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;

public class CategorySaveHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        // Retrieve values from the request
        string action = context.Request["action"];
        string recId = context.Request["Rec_id"];
        string department = context.Request["Department"];
        string deparmentName = context.Request["DeparmentName"];
        string shopId = context.Request["ShopID"];

        try
        {
            // Connection string
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (action == "update" && !string.IsNullOrEmpty(recId))
                {
                    // Update record
                    string query = @"UPDATE [tblDeparments]
                                     SET Department = @Department, DeparmentName = @DeparmentName, ShopID = @ShopID
                                     WHERE Rec_id = @Rec_id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Rec_id", recId);
                        cmd.Parameters.AddWithValue("@Department", department);
                        cmd.Parameters.AddWithValue("@DeparmentName", deparmentName);
                        cmd.Parameters.AddWithValue("@ShopID", shopId);

                        cmd.ExecuteNonQuery();
                    }
                }
                else if (action == "create")
                {
                    // Create a new record
                    string query = @"INSERT INTO [tblDeparments] (Department, DeparmentName, ShopID)
                                     VALUES (@Department, @DeparmentName, @ShopID)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Department", department);
                        cmd.Parameters.AddWithValue("@DeparmentName", deparmentName);
                        cmd.Parameters.AddWithValue("@ShopID", shopId);

                        cmd.ExecuteNonQuery();
                    }
                }

                // Send success response
                context.Response.Write(JsonConvert.SerializeObject(new { success = true }));
            }
        }
        catch (Exception ex)
        {
            // Send failure response
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = ex.Message }));
        }
    }

    public bool IsReusable
    {
        get { return false; } // Explicit getter for property compatibility with C# 5
    }
}