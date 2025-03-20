using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for SQLFunctions
/// </summary>
public class SQLFunctionsCS
{
	public SQLFunctionsCS()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static void SqlExecuteWOConstring(string sqlstr, string SQLconstring = "DSTConnectionString")
    {
        string DatabaseConnectionString = "";
        try
        {
            string databaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[SQLconstring].ConnectionString;

            // this is a shortcut for your connection string
            using (var conn = new SqlConnection(DatabaseConnectionString))
            {
                var cmd = new SqlCommand(sqlstr, conn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("ex:" + ex.Message.ToString() + "<br>");
            HttpContext.Current.Response.Write("SqlExecuteWOConstring.sqlstr:" + sqlstr + "<br>");
            HttpContext.Current.Response.Write("DatabaseConnectionString:" + DatabaseConnectionString + "<br>");
            HttpContext.Current.Response.End();

        }

    }

    public static void SqlExecute(string SQLconstring, string sqlstr)
    {
        string DatabaseConnectionString = "";
        try
        {
            string databaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[SQLconstring].ConnectionString;
            // this is a shortcut for your connection string
            using (var conn = new SqlConnection(DatabaseConnectionString))
            {
                var cmd = new SqlCommand(sqlstr, conn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("ex:" + ex.Message.ToString() + "<br>");
            HttpContext.Current.Response.Write("sqlstr:" + sqlstr + "<br>");
            HttpContext.Current.Response.Write("DatabaseConnectionString:" + DatabaseConnectionString + "<br>");
            HttpContext.Current.Response.End();

        }

    }

    public static DataTable GetDataTableFromSqlstr(ref string strsql, string SQLconstring = "DSTConnectionString")
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[SQLconstring].ConnectionString;
        string sql = strsql;
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand myCommand = new SqlCommand(sql, myConnection))
                {
                    myConnection.Open();
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        DataTable myTable = new DataTable();
                        myTable.Load(myReader);
                        myConnection.Close();
                        return myTable;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message + "<br>");
            HttpContext.Current.Response.Write("ln.2019 GetDataTableFromSqlstr.sql=" + sql);
            HttpContext.Current.Response.End();
            // You might want to handle the exception differently here
            return null;
        }
    }



    public static string GetSQLTableColumnNamesAsCsv( string sqlstr, string constring = "DSTConnectionString")
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                connection.Open();
                int mFieldCount = 10;
                SqlCommand command = new SqlCommand(sqlstr, connection);
                SqlDataReader reader = command.ExecuteReader();
                mFieldCount=reader.FieldCount;
                string columnNamesCsv = string.Empty;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    columnNamesCsv += columnName + ",";
                }

                reader.Close();
                connection.Close();

                // Remove the trailing comma
                if (!string.IsNullOrEmpty(columnNamesCsv))
                    columnNamesCsv = columnNamesCsv.TrimEnd(',');

                return columnNamesCsv;
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., invalid SQL or connection issues)
            Console.WriteLine("Error: " + ex.Message);
            return "Error: " + ex.Message;
        }
    }

    public static List<string> SearchCustomers(string prefixText, int count)
    {
        string sqlstr1 = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " +
                         "where isnull(Description,'') <> '' and (Description Like '%' + " +
                         "Replace(Replace(@SearchText, '  ', ' '), ' ', '%')  + '%');";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(sqlstr1, conn))
            {
                cmd.Parameters.AddWithValue("@SearchText", prefixText);
                conn.Open();
                List<string> searchTextList = new List<string>();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        searchTextList.Add(sdr["Description"].ToString());
                    }
                }
                return searchTextList;
            }
        }
    }

    public string ExecuteSqlString(string sqlString, string connectionString = "DSTConnectionString")
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlString, connection);
                connection.Open();
                command.ExecuteNonQuery();
                return "Success";
            }
        }
        catch (Exception)
        {
            return "Failed";
        }
    }

    public string ExecuteStoredProcedure(string storedProcedureName, SqlParameter[] parameters, string connectionString = "DSTConnectionString")
    {
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(storedProcedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    connection.Open();
                    command.ExecuteNonQuery();
                    return "Success";
                }
            }
            catch (Exception)
            {
                return "Failed";
            }
        }
    }

}

//"Select [CustomerAccNo],[Title],[Forename],[Surname],[Company],[AddressName],[AddressStreet],[AddressTown],[AddressCounty][Postcode],[NothingByPost],[Telephone] FROM[DST].[dbo].[tblCustomer] where isnull([Forename],'') <> '' and ([Forename] Like '%' + Replace(Replace(@SearchText, '  ', ' '), ' ', '%')  + '%');";


