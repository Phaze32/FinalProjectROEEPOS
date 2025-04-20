
Imports System.Data.SqlClient
Imports System.Globalization
Imports eBay.Service.Core.Soap

Partial Class POSOrderList
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("POSProductList.aspx?mode=summary")
    End Sub
    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Response.Redirect("POSProductList.aspx?mode=details")
    End Sub

    Private Sub POSProductList_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        SqlDataSource1.FilterExpression = ""
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
        TextBoxCustomerID.Text = row.Cells(7).Text
    End Sub

    Protected Sub ButtonSave_Click(sender As Object, e As EventArgs)
        Try
            Dim OrderID As String = TextBoxOrderID.Text
            Dim InvoiceNo As String = TextBoxInvoiceNo.Text
            Dim OrderStatus As String = TextBoxOrderStatus.Text
            Dim CustomerID As Integer = 1 'TextBoxCustomerID.Text
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
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("Drycleaning_DBConnectionString").ConnectionString
            Using connection As New SqlConnection(connectionString)
                Dim query As String = "UPDATE dbo.tbl_order SET [CustomerID] = @CustomerID, OrderDate = @OrderDate, " &
                                      "TotalCost = @TotalCost, Quantity = @Quantity WHERE OrderID = @OrderID"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@OrderID", OrderID)
                    command.Parameters.AddWithValue("@OrderStatus", OrderStatus)
                    command.Parameters.AddWithValue("@CustomerID", CustomerID)
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
    Protected Sub TextBoxFilter_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            LabelError.Text = "TextBoxFilter_TextChanged Triggered"
            Dim filterText As String = TextBoxFilter.Text.Trim()
            If String.IsNullOrEmpty(filterText) Then
                filterText = "%"
            End If
            SqlDataSource1.SelectParameters("Filter").DefaultValue = filterText
            GridView1.DataBind()
        Catch ex As Exception
            LabelError.Text &= "<br />An error occurred: " & ex.Message
        End Try
    End Sub

    Protected Sub Button5_Click_old(ByVal sender As Object, ByVal e As EventArgs)
        LabelError.Visible = True
        LabelError.Text = "Button5_Click TRIGGERED: " & Button5.Text.Trim()
        'Dim filterText As String = Button5.Text.Trim()
        TextBoxFilter.Text = Button5.Text.Trim()
        Dim TRIGGERLOCATION As String = "ZERO"
        Try
            Dim filterText As String = Button5.Text.Trim()
            If String.IsNullOrEmpty(filterText) Then
                SqlDataSource1.FilterExpression = ""
            Else
                TRIGGERLOCATION = "LEVEL 2 "
                SqlDataSource1.FilterExpression = "OrderID LIKE '%" & filterText & "%' OR " &
                                                   "InvoiceNo LIKE '%" & filterText & "%' OR " &
                                                   "OrderStatus LIKE '%" & filterText & "%' OR " &
                                                   "Name LIKE '%" & filterText & "%' OR " &
                                                   "OrderDate LIKE '%" & filterText & "%'"
            End If
            GridView1.DataBind()
        Catch ex As Exception
            ' Handle the exception
            Response.Write("An error occurred: " & ex.Message)
        End Try
        LabelError.Text = SqlDataSource1.FilterExpression.ToString & ">>TRIGGERLOCATION=" & TRIGGERLOCATION
    End Sub

    Protected Sub Button5_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim filterText As String = Button5.Text.Trim()
            LabelError.Text = "Button5_Click TRIGGERED: " & filterText
            If String.IsNullOrEmpty(filterText) Then
                filterText = "%" ' If no filter, use wildcard
            End If
            SqlDataSource1.SelectParameters("Filter").DefaultValue = filterText
            GridView1.DataBind()
        Catch ex As Exception
            LabelError.Text &= "<br />An error occurred Button5_Click: " & ex.Message
        End Try
    End Sub
    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim filterText As String = Button3.Text.Trim()
            LabelError.Text = "Button3_Click TRIGGERED: " & filterText
            If String.IsNullOrEmpty(filterText) Then
                filterText = "%" ' If no filter, use wildcard
            End If
            SqlDataSource1.SelectParameters("Filter").DefaultValue = filterText
            GridView1.DataBind()
        Catch ex As Exception
            LabelError.Text &= "<br />An error occurred Button3_Click: " & ex.Message
        End Try
    End Sub
    Protected Sub Button6_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim filterText As String = Button6.Text.Trim()
            LabelError.Text = "Button6_Click TRIGGERED: " & filterText
            If String.IsNullOrEmpty(filterText) Then
                filterText = "%" ' If no filter, use wildcard
            End If
            SqlDataSource1.SelectParameters("Filter").DefaultValue = filterText
            GridView1.DataBind()
        Catch ex As Exception
            LabelError.Text &= "<br />An error occurred Button6_Click: " & ex.Message
        End Try
    End Sub
    Protected Sub Button7_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim filterText As String = Button7.Text.Trim()
            LabelError.Text = "Button7_Click TRIGGERED: " & filterText
            If String.IsNullOrEmpty(filterText) Then
                filterText = "%" ' If no filter, use wildcard
            End If
            SqlDataSource1.SelectParameters("Filter").DefaultValue = filterText
            GridView1.DataBind()
        Catch ex As Exception
            LabelError.Text &= "<br />An error occurred Button7_Click: " & ex.Message
        End Try
    End Sub
End Class
