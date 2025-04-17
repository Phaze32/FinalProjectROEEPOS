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
 string query = "SELECT [userID], [ForeName], [SurName], [Addressname] as address , [PostCode], [AddressTown] as City, [telephone] as PhoneNumber, [Email],[Login] ,[Password] FROM [DST].[dbo].[tblUser]";



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    var jsonResponse = new {
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


