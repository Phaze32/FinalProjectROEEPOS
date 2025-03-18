Imports System
Imports System.Data
Partial Class Process_Payment
    Inherits System.Web.UI.Page
    Shared productinfo As String = ""
    Shared ticketID As String = ""
    Private Sub Process_PaymentComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        ' pluid = Request.QueryString("productid")
        'literal1.Text = ""
        'If Request.QueryString("ticketid") <> "" Then ticketID = Request.QueryString("ticketid")
        'Dim sqlstr As String = "SELECT TOP (1) sum(qty) as qty,sum([PLUPrice]*Qty) as totalPayable " _
        ' & "FROM [DST].[dbo].[tblTicketTrans_basket] where TicketID='" & ticketID & "' ;"
        ' productinfo = ""

        'Dim sqlstr2 As String = sqlstr '" select *  FROM  [DST].[dbo].[tblTicketTrans_basket] where [TicketID] ='66';"
        'Dim DT2 As DataTable = SQLFunctions.GetDataTableFromSqlstr(sqlstr2, "DSTConnectionString")
        'Dim DT_Sums As DataTable = DT2 ' DTClass.SumDTColumns(DT2, "PLUPrice,Qty", " 1=1 ")
        'Response.Write("#######" & DTClass.GetDTColumnNames(DT2) & ", DT_Sums.RecordCount=" & DT_Sums.Rows.Count.ToString)
        'Response.Write("#######DT_Sums.GetDTColumnNames=" & DTClass.GetDTColumnNames(DT_Sums) & "<br>sqlstr=" & sqlstr)
        productinfo = "process_payment ok "
        ' populateform(DT_Sums)
        'If productinfo = "" Then productinfo = "ErrorFormatNumber"
        literal1.Text = productinfo
        'DT2 = Nothing
        'DT_Sums = Nothing
    End Sub


    Private Function populateform(DT_Sums As DataTable) As String
        Dim TOTAL_ITEMS As String = DT_Sums.Rows.Item(0).Item("qty") 'DT_Sums.Rows.Item(0).Item("qty") '"00"
        Dim TOTAL As String = DT_Sums.Rows.Item(0).Item("totalPayable") 'DT_Sums.Rows.Item(0).Item("PLUPrice") * DT_Sums.Rows.Item(0).Item("qty") '"00"
        Dim TOTAL_Payable As String = "" 'DT_Sums.Rows.Item(0).Item("totalPayable") ' DT_Sums.Rows.Item(0).Item("PLUPrice") * DT_Sums.Rows.Item(0).Item("qty") '"00"
        Dim Discount As String = "00"
        Dim Order_Tax As String = "00"
        Dim RV As String = ""
        TOTAL_Payable = TOTAL + Order_Tax - Discount
        TOTAL_Payable = MiscClass.myCurrencyFormat(TOTAL_Payable, 2)
        'Response.Write("TOTAL_Payable=" & TOTAL_Payable)
        RV = " <table id = ""totaltbl"" ClTOTAL_Payableass=""table table-condensed totals"" style=""margin-bottom:10px; width:100%;""> " _
        & " <tbody> " _
        & "     <tr Class=""info""> " _
        & " <td width = ""25%"">Total Items..</td> " _
        & " <td Class=""text-right"" style=""padding-right:10px;""><span id=""count"">" & TOTAL_ITEMS & " </span></td> " _
        & " <td width = ""25%"">Total</td> " _
        & " <td Class=""text-right"" colspan=""2""><span id=""total"">" & MiscClass.myCurrencyFormat(TOTAL, 2) & "</span></td> " _
        & " </tr> " _
        & " </tbody>" _
        & " </table>"
        Return RV
    End Function


End Class
