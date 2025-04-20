
Imports System.Data.SqlClient
Imports System.Globalization

Partial Class POSOrderList
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("POSProductList.aspx?mode=summary")
    End Sub
    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Response.Redirect("POSProductList.aspx?mode=details")
    End Sub

    Private Sub POSProductList_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Button1.BackColor = Drawing.Color.LightPink
        Button4.BackColor = Drawing.Color.LightBlue
        Dim mmode As String = Request.QueryString("mode")
        If mmode = "summary" Then
            Button2.BackColor = Drawing.Color.LightPink
            Button3.BackColor = Drawing.Color.LightPink
            Button5.BackColor = Drawing.Color.LightPink
        Else
            Button2.BackColor = Drawing.Color.LightBlue
            Button3.BackColor = Drawing.Color.LightBlue
            Button5.BackColor = Drawing.Color.LightBlue
        End If
    End Sub
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim mmode As String = Request.QueryString("mode")
        If mmode = "summary" Then

        Else

        End If
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim row As GridViewRow = GridView1.SelectedRow
        TextBoxOrderID.Text = row.Cells(0).Text
        TextBoxInvoiceNo.Text = row.Cells(1).Text
        TextBoxOrderStatus.Text = row.Cells(2).Text
        TextBoxName.Text = row.Cells(3).Text
        TextBoxOrderDate.Text = row.Cells(4).Text
        TextBoxTotalCost.Text = row.Cells(5).Text
        TextBoxQuantity.Text = row.Cells(6).Text
    End Sub

    Protected Sub ButtonSave_Click(sender As Object, e As EventArgs)
        Try
            Dim OrderID As String = TextBoxOrderID.Text
            Dim InvoiceNo As String = TextBoxInvoiceNo.Text
            Dim OrderStatus As String = TextBoxOrderStatus.Text
            Dim Name As String = TextBoxName.Text
            Dim OrderDate As DateTime = TextBoxOrderDate.Text
            Dim TotalCost As String = TextBoxTotalCost.Text
            Dim Quantity As Integer = TextBoxQuantity.Text

            ' Validate PiecesTotal
            If Not Int32.TryParse(TextBoxQuantity.Text, Quantity) Then
                ' Handle invalid input (e.g., set to a default value or display an error message)
                LabelError.Text = "Invalid Pieces Total."
                LabelError.Visible = True
                Return
            End If

            ' Validate and convert TicketDate to British date format with time
            If DateTime.TryParseExact(TextBoxOrderDate.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, OrderDate) Then
                ' Ensure ticketDate is within the valid range for SQL Server datetime
                If OrderDate < New DateTime(1753, 1, 1) Or OrderDate > New DateTime(9999, 12, 31) Then
                    ' Handle date out of range
                    LabelError.Text = "Date must be between 01/01/1753 and 31/12/9999."
                    LabelError.Visible = True
                    Return
                End If
            Else
                ' Handle invalid date input
                LabelError.Text = "Invalid date format. Please enter the date in dd/MM/yyyy HH:mm:ss format."
                LabelError.Visible = True
                Return
            End If

            ' Update the record in the database using the collected values
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("DSTConnectionString").ConnectionString
            Using connection As New SqlConnection(connectionString)
                Dim query As String = "UPDATE dbo.tblTicket SET Name = @Name, TicketDate = @TicketDate, " &
                                      "TicketTotal = @TicketTotal, PiecesTotal = @PiecesTotal WHERE TicketID = @TicketID"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@OrderID", OrderID)
                    command.Parameters.AddWithValue("@OrderStatus", OrderStatus)
                    command.Parameters.AddWithValue("@Name", Name)
                    command.Parameters.AddWithValue("@OrderDate", OrderDate)
                    command.Parameters.AddWithValue("@TotalCost", TotalCost)
                    command.Parameters.AddWithValue("@Quantity", Quantity)
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            ' Optionally, rebind the GridView to reflect changes
            GridView1.DataBind()
        Catch ex As Exception
            ' Handle the exception (e.g., log the error, display an error message)
            ' Example: Display error message in a Label control
            LabelError.Text = "An error occurred: " & ex.Message
            LabelError.Visible = True
        End Try
    End Sub






End Class
