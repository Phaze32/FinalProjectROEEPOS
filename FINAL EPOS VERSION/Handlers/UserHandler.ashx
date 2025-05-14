<%@ WebHandler Language="C#" Class="UserHandler" %>

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;

public class UserHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        // Retrieve values from the request
        string action = context.Request["action"];
        string editUserID = context.Request["editUserID"];
        string editForeName = context.Request["editForeName"];
        string editSurName = context.Request["editSurName"];
        string editTelephone = context.Request["editPhoneNumber"];
        string editAddress = context.Request["editAddress"];
        string editPostCode = context.Request["editPostCode"];
        string editCity = context.Request["editCity"];
        string editEmail = context.Request["editEmail"];
        string editLogin = context.Request["editLogin"];
        string editPassword = context.Request["editPassword"];

        try
        {
            // Connection string from configuration file
            string connectionString = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Check if a user already exists based on name + surname or telephone number
                string checkQuery = @"SELECT UserID FROM [tblUser]
                                      WHERE (ForeName = @ForeName AND SurName = @SurName)
                                      OR Telephone = @Telephone";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ForeName", editForeName);
                    checkCmd.Parameters.AddWithValue("@SurName", editSurName);
                    checkCmd.Parameters.AddWithValue("@Telephone", editTelephone);

                    object userID = checkCmd.ExecuteScalar(); // Get existing UserID

                    if (userID != null)
                    {
                        // If user exists, update their record
                        string updateQuery = @"UPDATE [tblUser]
                                               SET ForeName = @ForeName,
                                                   SurName = @SurName,
                                                   [AddressName] = @Address,
                                                   PostCode = @PostCode,
                                                   AddressTown = @City,
                                                   Telephone = @Telephone,
                                                   Email = @Email,
                                                   Login = @Login,
                                                   Password = @Password
                                               WHERE UserID = @UserID";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userID);
                            cmd.Parameters.AddWithValue("@ForeName", editForeName);
                            cmd.Parameters.AddWithValue("@SurName", editSurName);
                            cmd.Parameters.AddWithValue("@Address", editAddress);
                            cmd.Parameters.AddWithValue("@PostCode", editPostCode);
                            cmd.Parameters.AddWithValue("@City", editCity);
                            cmd.Parameters.AddWithValue("@Telephone", editTelephone);
                            cmd.Parameters.AddWithValue("@Email", editEmail);
                            cmd.Parameters.AddWithValue("@Login", editLogin);
                            cmd.Parameters.AddWithValue("@Password", editPassword);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                context.Response.Write(JsonConvert.SerializeObject(new
                                {
                                    success = true,
                                    message = "User updated successfully!"
                                }));
                            }
                            else
                            {
                                context.Response.Write(JsonConvert.SerializeObject(new
                                {
                                    success = false,
                                    message = "No changes were made to the user record."
                                }));
                            }
                        }
                        return; // Exit after updating
                    }
                }

                // If user doesn't exist, add a new record
                if (action == "add" || action == "update") // Allow both "add" and "update" to create a new user
                {
                    string addQuery = @"INSERT INTO [tblUser]
                                        (ForeName, SurName, [AddressName], PostCode, AddressTown, Telephone, Email, Login, Password)
                                        VALUES (@ForeName, @SurName, @Address, @PostCode, @City, @Telephone, @Email, @Login, @Password)";

                    using (SqlCommand cmd = new SqlCommand(addQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ForeName", editForeName);
                        cmd.Parameters.AddWithValue("@SurName", editSurName);
                        cmd.Parameters.AddWithValue("@Address", editAddress);
                        cmd.Parameters.AddWithValue("@PostCode", editPostCode);
                        cmd.Parameters.AddWithValue("@City", editCity);
                        cmd.Parameters.AddWithValue("@Telephone", editTelephone);
                        cmd.Parameters.AddWithValue("@Email", editEmail);
                        cmd.Parameters.AddWithValue("@Login", editLogin);
                        cmd.Parameters.AddWithValue("@Password", editPassword);

                        cmd.ExecuteNonQuery();
                        context.Response.Write(JsonConvert.SerializeObject(new
                        {
                            success = true,
                            message = "User added successfully!"
                        }));
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