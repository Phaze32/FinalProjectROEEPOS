<%@ WebHandler Language="VB" Class="SaveOrder" %>

Imports System
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration

Public Class SaveOrder : Implements IHttpHandler
    Shared ticketID As String
    Shared newTicketID As Integer
    Shared maxTicketID As Integer
    Shared fullQuery As String
    Shared sqlqueryTicket As String
    Shared CustomerID As Integer
    Shared vQty As Decimal = 0
    Shared vOrderTotal As Decimal = 0
    Shared res As String
    Shared CustomerAccNo As String
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        ' Set response content type to plain text
        context.Response.ContentType = "text/plain"

        ' Get TicketID from query string
        ticketID = context.Request.QueryString("TicketID")

        ' Get CustomerID from query string
        CustomerID = context.Request.QueryString("CustomerID")

        ' Get CustomerID from query string
        CustomerAccNo = context.Request.QueryString("CustomerAccNo")

        ' Validate TicketID
        If Not String.IsNullOrEmpty(ticketID) Then
            Dim result As String = InsertData(ticketID)
            context.Response.Write(result)
        Else
            context.Response.Write("Invalid TicketID")
        End If
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property


    Private Function InsertData(ByVal ticketID As String) As String
        Dim connectionString As String = SQLFunctions.GetConnectionString("DSTConnectionString")
        Dim ticketresult As String

        Dim results As Dictionary(Of String, Object) = SQLFunctions.GetVariblesFromSQLQuery("select top 1 sum(qty) as qty, sum(qty*PLUPrice) as OrderTotal from [DST].[dbo].[tblTicketTrans_basket]")

        If results.ContainsKey("qty") Then
            vQty = Convert.ToDecimal(results("qty")) ' Extract and convert "qty" value
        End If

        If results.ContainsKey("OrderTotal") Then
            vOrderTotal = Convert.ToDecimal(results("OrderTotal")) ' Extract and convert "OrderTotal" value
        End If
        'Response.Write("Results:####" & "vQty=" & vQty & "##vOrderTotal=" & vOrderTotal)
        'Literal1.Text = ("Results:####" & "vQty=" & vQty & "##vOrderTotal=" & vOrderTotal)
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()

                ' Get the highest TicketID from the destination tables
                Dim getMaxTicketIDQuery As String = "SELECT ISNULL(MAX(TicketID), 0) + 1 FROM [DST].[dbo].[tblTicket];"
                Using getMaxTicketIDCommand As New SqlCommand(getMaxTicketIDQuery, connection)
                    newTicketID = Convert.ToInt32(getMaxTicketIDCommand.ExecuteScalar())
                End Using

                Dim sqlqueryTicket As String = "INSERT INTO [DST].[dbo].[tblTicket] ([TicketID], [TicketDate],[CollectDate], [CollectTime]," &
                    " [AlertDate],[Deliver],[HotelsOwnWork], [Complete], [CustomerID],[TicketTotal],[PiecesTotal],[GarmentTotal],[OrderStatusID],[CollectAMPM],[CustomerAccNo]) " &
                     " VALUES (" & newTicketID & ",'" & MiscClass.AddDaysToDate(Now(), 0, Now.ToString("HH:mm:ss")) &
                     "','" & MiscClass.AddDaysToDate(Now(), 4) &
                     "','5PM','" & MiscClass.AddDaysToDate(Now(), 3) &
                     "',0,0,0," & CustomerID & "," & vOrderTotal & "," & vQty & "," & vQty & ", 2, 'PM'," & CustomerAccNo & ")"
                Dim connectionString1 As String = "Data Source=HP2022;Initial Catalog=DST;User ID=sa;Password=Redcat17;"
                Dim sqlquerydeletebasket As String = " TRUNCATE TABLE [DST].[dbo].[tblTicketTrans_basket];"
                ticketresult = RunSqlQuery(sqlqueryTicket, connectionString1)
                ticketresult += "\n sqlqueryTicket=" & sqlqueryTicket
                'ticketresult += "</p>DB Name=" & SQLFunctions.GetDatabaseNameFromConfig("DSTConnectionString") & ", connection String = " & SQLFunctions.GetConnectionString("DSTConnectionString")
                ' Insert data into the destination table
                Dim insertQuery As String = "INSERT INTO [DST].[dbo].[tblTicketTrans] " &
                                                "([Barcode], [TicketID], [BillID], [BillDate], [CollectDate], [Qty], [PLUID], " &
                                                "[PLUDescription], [PLUPrice], [ExtrasPrice], [OriginalTicketTotal], [CurrentTicketTotal], " &
                                                "[UnreportedFormerTicketTotal], [PriceChangeChanged], [PriceChangeChangedBy], [DiscountTotal], " &
                                                "[DiscountType], [DiscountReason], [GarmentStatus], [GarmentStatusDate], [GarmentStatusChangedBy], " &
                                                "[StockSales], [PiecesTotal], [RailedDueDate], [Rail], [RailDate], [InvoiceID], [InvoiceDate], " &
                                                "[ElectronicDate], [Commission], [CommissionDisc], [VAT], [VATDisc], [SellingPrice], [SellingPriceDisc], " &
                                                "[Charity], [HalfPrice], [CommissionPaidDate], [CharityToDate], [CustomerDateBack], [ApprovalDate], [Notes], " &
                                                "[ActualSellingPrice], [LoyaltyPoints], [CollectType], [TillNum], [UniqueID], [BatchNo], [TempID], [SlotUnits], " &
                                                "[ColourFabric], [Deposit], [CollectID], [Department], [PLUDeptDisc], [CustomerID]) " &
                                                "Select [Barcode], @NewTicketID, [BillID], [BillDate], [CollectDate], [Qty], [PLUID], " &
                                                "[PLUDescription], [PLUPrice], [ExtrasPrice], [OriginalTicketTotal], [CurrentTicketTotal], " &
                                                "[UnreportedFormerTicketTotal], [PriceChangeChanged], [PriceChangeChangedBy], [DiscountTotal], " &
                                                "[DiscountType], [DiscountReason], [GarmentStatus], [GarmentStatusDate], [GarmentStatusChangedBy], " &
                                                "[StockSales], [PiecesTotal], [RailedDueDate], [Rail], [RailDate], [InvoiceID], [InvoiceDate], " &
                                                "[ElectronicDate], [Commission], [CommissionDisc], 'False' as [VAT], [VATDisc], [SellingPrice], [SellingPriceDisc], " &
                                                "[Charity], [HalfPrice], [CommissionPaidDate], [CharityToDate], [CustomerDateBack], [ApprovalDate], [Notes], " &
                                                "[ActualSellingPrice], [LoyaltyPoints], [CollectType], [TillNum], [UniqueID], [BatchNo], [TempID], [SlotUnits], " &
                                                "[ColourFabric], [Deposit], [CollectID], [Department], [PLUDeptDisc], @CustomerID " &
                                                "FROM [dbo].[tblTicketTrans_basket]"

                Using insertCommand As New SqlCommand(insertQuery, connection)
                    insertCommand.Parameters.AddWithValue("@NewTicketID", newTicketID)
                    insertCommand.Parameters.AddWithValue("@CustomerID", CustomerID)

                    fullQuery = insertQuery
                    For Each param As SqlParameter In insertCommand.Parameters
                        fullQuery = fullQuery.Replace(param.ParameterName, "'" & param.Value.ToString() & "'")
                    Next

                    insertCommand.ExecuteNonQuery()

                End Using
                ticketresult += "</p>BasketDelete=" & RunSqlQuery(sqlquerydeletebasket, connectionString1)
                'Return "Data inserted successfully with new TicketID: " & newTicketID & ">>ln119.InsertData>.>>" & fullQuery
                Return newTicketID
                'Return "Data inserted successfully with new TicketID: " & ticketID & ": newTicketID=" & newTicketID & "<p>fullQuery=" & fullQuery & "<br>sqlqueryTicket=" & sqlqueryTicket & "<p>TicketResult=" & ticketresult
            Catch ex As Exception

                Return "Error: " & ex.Message
            End Try
        End Using
    End Function
    Public Shared Function RunSqlQuery(ByVal sqlquery As String, Optional ByVal connectionString As String = "DSTConnectionString") As String
        Try
            ' Create a new connection object
            Using connection As New SqlConnection(connectionString)
                ' Open the connection
                connection.Open()

                ' Create a new command object with the provided query
                Using command As New SqlCommand(sqlquery, connection)
                    ' Execute the SQL query
                    command.ExecuteNonQuery()
                End Using
            End Using

            ' Return success message if everything works
            Return "Query Successful"
        Catch ex As Exception
            ' Return failure message along with the reason
            Return "Query Failed: " & ex.Message
        End Try
    End Function
End Class