<%@ WebHandler Language="C#" Class="Handlers.ProductSaveUpdateHandler" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace Handlers
{
    public class ProductSaveUpdateHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string connStr = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

            // Retrieve form values
            // following line checks if the PLUID is 0 then it gets the next PLUID from fucntion
            string pluid = GetNextPLUID(context.Request["PLUID"]);
            string productImage = context.Request["ProductImage"];
            string type = context.Request["Type"];
            string description = context.Request["Description"];
            string Department = context.Request["Department"];
            bool pluList = context.Request["PLUList"] == "true";

            int listId = 0;
            int.TryParse(context.Request["ListID"], out listId);

            int pieces = 0;
            int.TryParse(context.Request["Pieces"], out pieces);

            decimal price = 0;
            decimal.TryParse(context.Request["Price"], out price);

            bool allowDiscount = context.Request["AllowDiscount"] == "true";
            string created = context.Request["Created"];
            string createdBy = context.Request["CreatedBy"];

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
IF EXISTS (SELECT 1 FROM tblPLU WHERE PLUID = @PLUID)
BEGIN
    UPDATE tblPLU SET
        ProductImage = @ProductImage,
        Type = @Type,
        Description = @Description,
        Department =@Department,
        PLUList = @PLUList,
        ListID = @ListID,
        Pieces = @Pieces,
        Price = @Price,
        AllowDiscount = @AllowDiscount,
        Created = @Created,
        CreatedBy = @CreatedBy
    WHERE PLUID = @PLUID
END
ELSE
BEGIN
    INSERT INTO tblPLU (PLUID, ProductImage, Type, Description, Department, PLUList, ListID, Pieces, Price, AllowDiscount, Created, CreatedBy)
    VALUES (@PLUID, @ProductImage, @Type, @Description, @Department, @PLUList, @ListID, @Pieces, @Price, @AllowDiscount, @Created, @CreatedBy)
END";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PLUID", pluid);
                    cmd.Parameters.AddWithValue("@ProductImage", productImage ?? "");
                    cmd.Parameters.AddWithValue("@Type", type ?? "");
                    cmd.Parameters.AddWithValue("@Description", description ?? "");
                    cmd.Parameters.AddWithValue("@Department", Department ?? "");
                    cmd.Parameters.AddWithValue("@PLUList", pluList);
                    cmd.Parameters.AddWithValue("@ListID", listId);
                    cmd.Parameters.AddWithValue("@Pieces", pieces);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@AllowDiscount", allowDiscount);
                    cmd.Parameters.AddWithValue("@Created", string.IsNullOrEmpty(created) ? (object)DBNull.Value : (object)DateTime.Parse(created));
                    cmd.Parameters.AddWithValue("@CreatedBy", createdBy ?? "");

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    context.Response.Write("Success");
                }
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public static string GetNextPLUID(string pluidStr)
        {
            int pluid = 0;

            // Convert string PLUID into integer
            if (!string.IsNullOrEmpty(pluidStr) && int.TryParse(pluidStr, out pluid))
            {
                if (pluid != 0)
                {
                    return pluid.ToString(); // If PLUID is valid and non-zero, return it as a string
                }
            }

            // If PLUID is 0 or invalid, get the next available PLUID from the database
            string connectionString = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;
            int newPLUID = 0;

            string query = "SELECT MAX(PLUID) + 1 AS NewPLUID FROM [DST].[dbo].[tblPLU] WHERE description IS NOT NULL AND pluid < 9000";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out newPLUID))
                        {
                            return newPLUID.ToString(); // Convert newPLUID to string before returning
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching next PLUID: " + ex.Message);
                }
            }

            return newPLUID.ToString(); // Ensures return value is always a string
        }
    }




}



