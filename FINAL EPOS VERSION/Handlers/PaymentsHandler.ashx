<%@ WebHandler Language="C#" Class="PaymentsHandler" %>

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

public class PaymentsHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        // Read the "action" parameter (should be "add" or "edit")
        string action = (context.Request["action"] ?? "").ToLower();
        string resultMessage = string.Empty;

        try
        {
            switch (action)
            {
                case "add":
                    // Create a new Payment object from the incoming request parameters.
                    Payment newPayment = new Payment
                    {
                        TicketID = context.Request["TicketID"],
                        CustomerID = Convert.ToInt32(context.Request["CustomerID"]),
                        CustomerAccNo = context.Request["CustomerAccNo"],
                        Amount = Convert.ToDecimal(context.Request["Amount"]),
                        PaidBy = context.Request["PaidBy"],
                        PaymentNote = context.Request["PaymentNote"],
                        Discount = Convert.ToDecimal(context.Request["Discount"]),
                        OrderTax = Convert.ToDecimal(context.Request["OrderTax"]),
                        PaymentDate =  DateTime.Now
                    };

                    if (AddPayment(newPayment))
                        resultMessage = "Payment added successfully.";
                    else
                        resultMessage = "Failed to add payment.";
                    break;

                case "edit":
                    // For edit, PaymentID must be provided.
                    string paymentIDStr = context.Request["PaymentID"];
                    if (string.IsNullOrEmpty(paymentIDStr))
                    {
                        resultMessage = "PaymentID is required for edit.";
                    }
                    else
                    {
                        Payment editPayment = new Payment
                        {
                            PaymentID = Convert.ToInt32(paymentIDStr),
                            TicketID = context.Request["TicketID"],
                            CustomerID = Convert.ToInt32(context.Request["CustomerID"]),
                            CustomerAccNo = context.Request["CustomerAccNo"],
                            Amount = Convert.ToDecimal(context.Request["Amount"]),
                            PaidBy = context.Request["PaidBy"],
                            PaymentNote = context.Request["PaymentNote"],
                            Discount = Convert.ToDecimal(context.Request["Discount"]),
                            OrderTax = Convert.ToDecimal(context.Request["OrderTax"]),
                            PaymentDate = DateTime.Now
                        };

                        if (UpdatePayment(editPayment))
                            resultMessage = "Payment updated successfully.";
                        else
                            resultMessage = "Failed to update payment.";
                    }
                    break;

                default:
                    resultMessage = "Invalid action. Please specify 'add' or 'edit'.";
                    break;
            }
        }
        catch (Exception ex)
        {
            resultMessage = "Error: " + ex.Message;
        }

        context.Response.Write(resultMessage);
    }

    private bool AddPayment(Payment payment)
    {
        string query = @"INSERT INTO [DST].[dbo].[tblPayments]
                         (TicketID, CustomerID, CustomerAccNo, Amount, PaidBy, PaymentNote, Discount, OrderTax, PaymentDate)
                         VALUES (@TicketID, @CustomerID, @CustomerAccNo, @Amount, @PaidBy, @PaymentNote, @Discount, @OrderTax, @PaymentDate)";

        string connStr = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TicketID", payment.TicketID);
                cmd.Parameters.AddWithValue("@CustomerID", payment.CustomerID);
                cmd.Parameters.AddWithValue("@CustomerAccNo", payment.CustomerAccNo);
                cmd.Parameters.AddWithValue("@Amount", payment.Amount);
                cmd.Parameters.AddWithValue("@PaidBy", payment.PaidBy);
                cmd.Parameters.AddWithValue("@PaymentNote", payment.PaymentNote);
                cmd.Parameters.AddWithValue("@Discount", payment.Discount);
                cmd.Parameters.AddWithValue("@OrderTax", payment.OrderTax);
                cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return (rowsAffected > 0);
            }
        }
    }

    private bool UpdatePayment(Payment payment)
    {
        string query = @"UPDATE [DST].[dbo].[tblPayments]
                         SET TicketID = @TicketID,
                             CustomerID = @CustomerID,
                             CustomerAccNo = @CustomerAccNo,
                             Amount = @Amount,
                             PaidBy = @PaidBy,
                             PaymentNote = @PaymentNote,
                             Discount = @Discount,
                             OrderTax = @OrderTax,
                             PaymentDate = @PaymentDate
                         WHERE PaymentID = @PaymentID";

        string connStr = ConfigurationManager.ConnectionStrings["DSTConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TicketID", payment.TicketID);
                cmd.Parameters.AddWithValue("@CustomerID", payment.CustomerID);
                cmd.Parameters.AddWithValue("@CustomerAccNo", payment.CustomerAccNo);
                cmd.Parameters.AddWithValue("@Amount", payment.Amount);
                cmd.Parameters.AddWithValue("@PaidBy", payment.PaidBy);
                cmd.Parameters.AddWithValue("@PaymentNote", payment.PaymentNote);
                cmd.Parameters.AddWithValue("@Discount", payment.Discount);
                cmd.Parameters.AddWithValue("@OrderTax", payment.OrderTax);
                cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                cmd.Parameters.AddWithValue("@PaymentID", payment.PaymentID);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return (rowsAffected > 0);
            }
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}

public class Payment
{
    public int PaymentID { get; set; }      // Primary key (required for "edit" operations)
    public string TicketID { get; set; }
    public int CustomerID { get; set; }
    public string CustomerAccNo { get; set; }
    public decimal Amount { get; set; }
    public string PaidBy { get; set; }
    public string PaymentNote { get; set; }
    public decimal Discount { get; set; }
    public decimal OrderTax { get; set; }
    public DateTime PaymentDate { get; set; }
}