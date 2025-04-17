Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports eBay.Service.Core.Soap
Imports System.Web.UI.WebControls

Partial Class POSOrderList
    Inherits System.Web.UI.Page

    ' Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '     Response.Redirect("POSProductList.aspx?mode=summary")
    ' End Sub
    ' Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
    '     Response.Redirect("POSProductList.aspx?mode=details")
    ' End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'CreateButtons()
            BindOrderStatusDropdown()
        End If
    End Sub
    Private Sub POSProductList_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        SqlDataSource1.FilterExpression = ""
        Button11.BackColor = Drawing.Color.LightPink
        Button14.BackColor = Drawing.Color.LightBlue
        Dim mmode As String = Request.QueryString("mode")
        If mmode = "summary" Then
            Button12.BackColor = Drawing.Color.LightPink
            Button13.BackColor = Drawing.Color.LightPink
            Button15.BackColor = Drawing.Color.LightPink
        Else
            Button12.BackColor = Drawing.Color.LightBlue
            Button13.BackColor = Drawing.Color.LightBlue
            Button15.BackColor = Drawing.Color.LightBlue
        End If
    End Sub
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Dim mmode As String = Request.QueryString("mode")
        If mmode = "summary" Then

        Else

        End If
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim row As GridViewRow = GridView1.SelectedRow
        TextBoxOrderID.Text = row.Cells(0).Text
        TextBoxInvoiceNo.Text = row.Cells(1).Text

        ' Find the item in DropDownListOrderStatus with the text value from the GridView cell and set it as selected
        Dim internalText As String = row.Cells(2).Text
        Dim listItem As ListItem = DropDownListOrderStatus.Items.FindByText(internalText)

        ' If the item exists, set the selected value; otherwise, handle the missing value case
        If listItem IsNot Nothing Then
            DropDownListOrderStatus.SelectedValue = listItem.Value
        Else
            ' Handle the case where the item does not exist in the dropdown list
            DropDownListOrderStatus.ClearSelection() ' Clear selection if item is not found
        End If

        TextBoxName.Text = row.Cells(3).Text
        TextBoxOrderDate.Text = row.Cells(4).Text
        TextBoxTotalCost.Text = row.Cells(5).Text
        TextBoxQuantity.Text = row.Cells(6).Text
        TextBoxCustomerID.Text = row.Cells(7).Text
    End Sub


    Protected Sub ButtonSave_Click(sender As Object, e As EventArgs)
        'Response.Write("######.OrderStatusID=")
        'Response.Write("######.OrderStatusID=" & DropDownListOrderStatus.SelectedValue & "OrderStatus=" & TextBoxOrderStatus.Text)
        'LabelError.Text = "ButtonSave_Click.OrderStatusID="
        'Response.End()
        Try
            Dim OrderID As String = TextBoxOrderID.Text
            Dim InvoiceNo As String = TextBoxInvoiceNo.Text
            Dim OrderStatus As String = TextBoxOrderStatus.Text
            'Dim CustomerID As Integer = TextBoxCustomerID.Text
            Dim OrderDate As DateTime = TextBoxOrderDate.Text
            Dim OrderStatusID As String = DropDownListOrderStatus.SelectedValue
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
                Dim query As String = "UPDATE dbo.tbl_order SET OrderDate = @OrderDate, " &
                          "OrderStatusID = @OrderStatusID, TotalCost = @TotalCost, Quantity = @Quantity WHERE OrderID = @OrderID"

                Using command As New SqlCommand(query, connection)
                    Dim parameters As New List(Of SqlParameter)()
                    parameters.Add(New SqlParameter("@OrderID", OrderID))
                    'parameters.Add(New SqlParameter("@CustomerID", CustomerID))
                    parameters.Add(New SqlParameter("@OrderDate", OrderDate))
                    parameters.Add(New SqlParameter("@OrderStatusID", OrderStatusID))
                    parameters.Add(New SqlParameter("@TotalCost", TotalCost))
                    parameters.Add(New SqlParameter("@Quantity", Quantity))

                    For Each param As SqlParameter In parameters
                        command.Parameters.Add(param)
                    Next

                    connection.Open()
                    command.ExecuteNonQuery()

                    ' Format the query with parameter values for debugging
                    'Dim formattedQuery As String = FormatQuery(query, parameters)
                    'LabelError.Text = "Query: " & formattedQuery
                    'Response.Write("Query: " & formattedQuery)
                    'Response.End()
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
    Public Function FormatQuery(ByVal query As String, ByVal parameters As List(Of SqlParameter)) As String
        For Each param As SqlParameter In parameters
            query = query.Replace(param.ParameterName, param.Value.ToString())
        Next
        Return query
    End Function
    Private Sub BindOrderStatusDropdown()
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("Drycleaning_DBConnectionString").ConnectionString
        Dim query As String = "SELECT OrderStatusID, OrderStatus FROM [Drycleaning_DB].[dbo].[tbl_OrderStatus]"

        Using con As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand(query, con)
            con.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            DropDownListOrderStatus.DataSource = reader
            DropDownListOrderStatus.DataTextField = "OrderStatus"
            DropDownListOrderStatus.DataValueField = "OrderStatusID"
            DropDownListOrderStatus.DataBind()
        End Using
    End Sub
    Protected Sub Common_Click(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim filterText As String = String.Empty

            If TypeOf sender Is Button Then
                Dim button As Button = CType(sender, Button)
                filterText = button.Text.Trim()
                LabelError.Text = button.ID & "_Click TRIGGERED: " & filterText
            ElseIf TypeOf sender Is TextBox Then
                Dim textBox As TextBox = CType(sender, TextBox)
                filterText = textBox.Text.Trim()
                LabelError.Text = textBox.ID & "_TextChanged Triggered: " & filterText
            End If

            If String.IsNullOrEmpty(filterText) Then
                filterText = "%" ' If no filter, use wildcard
            End If

            SqlDataSource1.SelectParameters("Filter").DefaultValue = filterText
            GridView1.DataBind()
        Catch ex As Exception
            If TypeOf sender Is Button Then
                LabelError.Text &= "<br />An error occurred " & CType(sender, Button).ID & "_Click: " & ex.Message
            ElseIf TypeOf sender Is TextBox Then
                LabelError.Text &= "<br />An error occurred " & CType(sender, TextBox).ID & "_TextChanged: " & ex.Message
            End If
        End Try
    End Sub

    Public Sub CreateButtons()
        Dim orderStatusTable As DataTable = GetOrderStatusValues()

        For Each row As DataRow In orderStatusTable.Rows
            Dim orderStatusID As Integer = Convert.ToInt32(row("OrderStatusID"))
            Dim orderStatus As String = row("OrderStatus").ToString()

            Dim button As New Button()
            button.ID = "Button" & orderStatusID.ToString()
            button.Text = orderStatus
            button.CommandArgument = orderStatusID.ToString()
            AddHandler button.Click, AddressOf Common_Click

            ' Add the button to a placeholder or a container on the page
            PlaceHolder1.Controls.Add(button)
        Next
    End Sub
    Public Function GetOrderStatusValues() As DataTable
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("Drycleaning_DBConnectionString").ConnectionString
        Dim query As String = "SELECT OrderStatusID, OrderStatus FROM [Drycleaning_DB].[dbo].[tbl_OrderStatus]"
        Dim dataTable As New DataTable()

        Using con As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand(query, con)
            con.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                dataTable.Load(reader)
            End Using
        End Using

        Return dataTable
    End Function
End Class
