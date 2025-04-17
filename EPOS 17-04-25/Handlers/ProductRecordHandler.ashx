<%@ WebHandler Language="C#" Class="Handlers.ProductRecordHandler" %>

using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;

namespace Handlers
{
    public class ProductRecordHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string connectionString = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

            // Retrieve filter parameters from the query string
            string pluid = context.Request.QueryString["PLUID"];
            string description = context.Request.QueryString["Description"];

            // Base query
            string query = "SELECT * FROM [tblPLU] WHERE Description <> ''";

            // Add filtering logic if PLUID or Description is provided
            if (!string.IsNullOrEmpty(pluid))
            {
                query += " AND PLUID = @PLUID";
            }
            else if (!string.IsNullOrEmpty(description))
            {
                query += " AND Description LIKE '%' + @Description + '%'";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters dynamically based on the filter
                    if (!string.IsNullOrEmpty(pluid))
                    {
                        command.Parameters.AddWithValue("@PLUID", pluid);
                    }
                    else if (!string.IsNullOrEmpty(description))
                    {
                        command.Parameters.AddWithValue("@Description", description);
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