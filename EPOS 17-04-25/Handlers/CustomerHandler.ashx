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

            // Check if customerID is provided
            string customerId = context.Request.QueryString["customerId"];
            string query;

            if (!string.IsNullOrEmpty(customerId) && customerId != "-1")
            {
                // Query to filter by customerID
                query = "SELECT [CustomerID], [ForeName], [SurName], [AddressName] as address, [PostCode], [AddressTown] as City, [Telephone] as PhoneNumber, [Email], [Status] " +
                        "FROM [DST].[dbo].[tblcustomer] WHERE [CustomerID] = @CustomerID";
            }
            else
            {
                // Query to get unfiltered data
                query = "SELECT [CustomerID], [ForeName], [SurName], [AddressName] as address, [PostCode], [AddressTown] as City, [Telephone] as PhoneNumber, [Email], [Status] " +
                        "FROM [DST].[dbo].[tblcustomer]";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(customerId) && customerId != "-1")
                    {
                        // Add parameter for filtering
                        command.Parameters.AddWithValue("@CustomerID", customerId);
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