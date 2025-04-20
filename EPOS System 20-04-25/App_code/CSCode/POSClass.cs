using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for POSClass
/// </summary>
public class POSClass
{
	public POSClass()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public class CustomerHandler
    {
        public void SaveCustomer(HttpRequest request)
        {
            string connectionString = "Data Source=HP2022;Initial Catalog=DST;Integrated Security=True;"; // Replace with your actual connection string

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                INSERT INTO [DST].[dbo].[tblCustomer] 
                ([CustomerAccNo], [Title], [Forename], [Surname], [Company], [AddressName], [AddressStreet], [AddressTown], [AddressCounty], [Postcode], [NothingByPost], [Telephone], [Fax], [EMail], [AccountType])
                VALUES 
                (@CustomerAccNo, @Title, @Forename, @Surname, @Company, @AddressName, @AddressStreet, @AddressTown, @AddressCounty, @Postcode, @NothingByPost, @Telephone, @Fax, @EMail, @AccountType)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerAccNo", (object)request["CustomerAccNo"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Title", (object)request["Title"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Forename", (object)request["Forename"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Surname", (object)request["Surname"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Company", (object)request["Company"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AddressName", (object)request["AddressName"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AddressStreet", (object)request["AddressStreet"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AddressTown", (object)request["AddressTown"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AddressCounty", (object)request["AddressCounty"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Postcode", (object)request["Postcode"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NothingByPost", (object)request["NothingByPost"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Telephone", (object)request["Telephone"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Fax", (object)request["Fax"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EMail", (object)request["EMail"] ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AccountType", (object)request["AccountType"] ?? DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}