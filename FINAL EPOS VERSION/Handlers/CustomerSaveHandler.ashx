<%@ WebHandler Language="C#" Class="CustomerSaveHandler" %>

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;

public class CustomerSaveHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        // Retrieve values from the request
        string action = context.Request["action"];
        string customerID = context.Request["editCustomerID"]; // Using editCustomerID as the key
        string foreName = context.Request["editForeName"];
        string surName = context.Request["editSurName"];
        string address = context.Request["editAddress"];
        string postCode = context.Request["editPostCode"];
        string city = context.Request["editCity"];
        string phoneNumber = context.Request["editPhoneNumber"];
        string email = context.Request["editEmail"];
        string status = context.Request["editStatus"]; // For "Status" field

        try
        {
            // Connection string from configuration file
            string connectionString = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (action == "add")
                {
                    // Insert new customer and retrieve the generated CustomerID
                    string addQuery = @"INSERT INTO [tblCustomer] 
                                        (ForeName, SurName, AddressName, PostCode, AddressTown, Telephone, Email, Status, DateModified,CustomerAccNo)
                                        OUTPUT INSERTED.CustomerID
                                        VALUES (@ForeName, @SurName, @Address, @PostCode, @City, @PhoneNumber, @Email, @Status, GETDATE(),@PhoneNumber)";

                    using (SqlCommand cmd = new SqlCommand(addQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ForeName", foreName);
                        cmd.Parameters.AddWithValue("@SurName", surName);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@PostCode", postCode);
                        cmd.Parameters.AddWithValue("@City", city);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Status", status);

                        // Execute the query and get the new CustomerID
                        object newCustomerID = cmd.ExecuteScalar();

                        if (newCustomerID != null)
                        {
                            context.Response.Write(JsonConvert.SerializeObject(new
                            {
                                success = true,
                                message = "Customer added successfully!",
                                newCustomerID = newCustomerID,
                                newForeName = foreName,
                                newSurName = surName
                            }));
                        }
                        else
                        {
                            context.Response.Write(JsonConvert.SerializeObject(new
                            {
                                success = false,
                                message = "Failed to add customer."
                            }));
                        }
                    }
                }
                else if (action == "update" && !string.IsNullOrEmpty(customerID))
                {
                    // Update existing customer
                    string updateQuery = @"UPDATE [tblCustomer]
                                           SET ForeName = @ForeName,
                                               SurName = @SurName,
                                               AddressName = @Address,
                                               PostCode = @PostCode,
                                               AddressTown = @City,
                                               Telephone = @PhoneNumber,
                                               Email = @Email,
                                               Status = @Status
                                           WHERE CustomerID = @CustomerID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customerID);
                        cmd.Parameters.AddWithValue("@ForeName", foreName);
                        cmd.Parameters.AddWithValue("@SurName", surName);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@PostCode", postCode);
                        cmd.Parameters.AddWithValue("@City", city);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Status", status);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            context.Response.Write(JsonConvert.SerializeObject(new
                            {
                                success = true,
                                message = "Customer updated successfully!"
                            }));
                        }
                        else
                        {
                            context.Response.Write(JsonConvert.SerializeObject(new
                            {
                                success = false,
                                message = "No changes were made to the customer record."
                            }));
                        }
                    }
                }
                else
                {
                    context.Response.Write(JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "Invalid action. Use 'update' or 'add'."
                    }));
                }
            }
        }
        catch (Exception ex)
        {
            // Return error message
            context.Response.Write(JsonConvert.SerializeObject(new
            {
                success = false,
                message = ex.Message
            }));
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}