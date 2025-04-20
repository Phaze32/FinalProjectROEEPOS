<%@ WebHandler Language="C#" Class="SQLDBBackupHandler" %>

using System;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json; // Make sure you have Newtonsoft.Json installed for JSON serialization

public class SQLDBBackupHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json"; // JSON response format

        // Initialize response object
        var response = new
        {
            success = false,
            message = "",
            fileName = "",
            folder = ""
        };

        try
        {
            // Get the connection string
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;

            // Extract database name from the connection string
            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder(connectionString);
            string dbName = connBuilder.InitialCatalog; // The database name

            // Get the backup folder from the configuration table
            string backupFolder = GetBackupFolder(connectionString);

            if (string.IsNullOrEmpty(backupFolder))
            {
                response = new
                {
                    success = false,
                    message = "Error: Backup folder not configured.",
                    fileName = "",
                    folder = ""
                };
                context.Response.Write(JsonConvert.SerializeObject(response));
                return;
            }

            // Create the backup file name dynamically
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = backupFolder + dbName + "_" + timeStamp + ".bak";

            // Construct the backup query
            string backupQuery = string.Format(@"
                DECLARE @dbName NVARCHAR(500) = '{0}';
                DECLARE @Database_Backup_Folder NVARCHAR(500) = '{1}';
                DECLARE @FileName NVARCHAR(500);
                SET @FileName = @Database_Backup_Folder + @dbName + '_' + 
                                REPLACE(CONVERT(VARCHAR(19), GETDATE(), 120), ':', '-') + '.bak';
                BACKUP DATABASE @dbName
                TO DISK = @FileName
                WITH FORMAT;
            ", dbName, backupFolder);

            // Execute the backup query
            ExecuteBackupQuery(connectionString, backupQuery);

            // Update response object on success
            response = new
            {
                success = true,
                message = "Backup completed successfully!",
                fileName = fileName,
                folder = backupFolder
            };
        }
        catch (Exception ex)
        {
            // Update response object on failure
            response = new
            {
                success = false,
                message = "Error: " + ex.Message,
                fileName = "",
                folder = ""
            };
        }

        // Serialize response to JSON and write to context
        context.Response.Write(JsonConvert.SerializeObject(response));
    }

    private string GetBackupFolder(string connectionString)
    {
        // Query to fetch 'Database_Backup_Folder' from the configuration table
        string query = @"
            SELECT TOP 1 [Value]
            FROM [DST].[dbo].[tblConfiguration]
            WHERE [Name] = 'Database_Backup_Folder'";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
        }
    }

    private void ExecuteBackupQuery(string connectionString, string backupQuery)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(backupQuery, conn))
            {
                cmd.ExecuteNonQuery(); // Execute the backup query
            }
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}