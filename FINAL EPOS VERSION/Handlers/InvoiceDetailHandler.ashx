﻿<%@ WebHandler Language="C#" Class="Handlers.InvoiceHandler" %>

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

            // Retrieve query parameters
            string customerID = context.Request.QueryString["CustomerID"];
            string ticketID = context.Request.QueryString["TicketID"];
            
            string query = @" SELECT  [TicketID],[TicketDate],[CollectDate],[CollectTime],[AlertDate],[Deliver]
      ,[HotelsOwnWork],[CustomerID],[TicketTotal],[PiecesTotal],[GarmentTotal],[ForeName],[SurName]
      ,[City],[PhoneNumber],[PostCode],[Qty],[PLUPrice],[PLUID],[PLUDescription],[OrderStatus]
      ,[OrderStatusID] FROM [DST].[dbo].[Vw_OrderDetail_Trans] where 1=1 ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(customerID) && customerID != "-1")
                    {
                        query += " AND [CustomerID] = @CustomerID";
                        command.Parameters.AddWithValue("@CustomerID", customerID);
                    }

                    if (!string.IsNullOrEmpty(ticketID) && ticketID != "-1")
                    {
                        query += " AND [TicketID] = @TicketID";
                        command.Parameters.AddWithValue("@TicketID", ticketID);
                    }

                    command.CommandText = query;
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