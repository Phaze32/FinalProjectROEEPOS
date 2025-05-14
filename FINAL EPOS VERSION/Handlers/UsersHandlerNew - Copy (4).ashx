<%@ WebHandler Language="C#" Class="Handlers.CustomerHandler" %>

using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;

namespace Handlers
{
    public class CustomerHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string connectionString = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;
            
            // Check if userId is provided
            string userId = context.Request.QueryString["userId"];
            string query;

            if (!string.IsNullOrEmpty(userId) && userId != "-1")
            {
                // Query to filter by userId
                query = "SELECT [userID], [ForeName], [SurName], [Addressname] as address, [PostCode], [AddressTown] as City, " +
                        "[telephone] as PhoneNumber, [Email], [Login], [Password] " +
                        "FROM [DST].[dbo].[tblUser] WHERE [userID] = @UserID";
            }
            else
            {
                // Query to get unfiltered data
                query = "SELECT [userID], [ForeName], [SurName], [Addressname] as address, [PostCode], [AddressTown] as City, " +
                        "[telephone] as PhoneNumber, [Email], [Login], [Password] " +
                        "FROM [DST].[dbo].[tblUser]";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(userId) && userId != "-1")
                    {
                        // Add parameter for filtering
                        command.Parameters.AddWithValue("@UserID", userId);
                    }

                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

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
            get { return false; }
        }
    }
}