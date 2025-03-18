
Imports System.Data
Partial Class GetTotals
    Inherits System.Web.UI.Page
    Shared productinfo As String = ""
    Shared ticketID As String = ""
    Private Sub GetTotals_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        ' pluid = Request.QueryString("productid")
        literal1.Text = ""
        If Request.QueryString("ticketid") <> "" Then ticketID = Request.QueryString("ticketid")
        Dim sqlstr As String = "SELECT TOP (1) sum(qty) as qty,sum([PLUPrice]*Qty) as totalPayable " _
        & "FROM [DST].[dbo].[tblTicketTrans_basket] where TicketID='" & ticketID & "' ;"

        productinfo = ""

        Dim sqlstr2 As String = sqlstr '" select *  FROM  [DST].[dbo].[tblTicketTrans_basket] where [TicketID] ='66';"
        Dim DT2 As DataTable = SQLFunctions.GetDataTableFromSqlstr(sqlstr2, "DSTConnectionString")
        Dim DT_Sums As DataTable = DT2 ' DTClass.SumDTColumns(DT2, "PLUPrice,Qty", " 1=1 ")
        'Response.Write("#######" & DTClass.GetDTColumnNames(DT2) & ", DT_Sums.RecordCount=" & DT_Sums.Rows.Count.ToString)
        'Response.Write("#######DT_Sums.GetDTColumnNames=" & DTClass.GetDTColumnNames(DT_Sums) & "<br>sqlstr=" & sqlstr)
        productinfo = populateform(DT_Sums)
        If productinfo = "" Then productinfo = "ErrorFormatNumber"
        literal1.Text = productinfo
        DT2 = Nothing
        DT_Sums = Nothing
    End Sub
    Private Sub TestGetTotals()
        'dim pluid = Request.QueryString("pluid")
        literal1.Text = ""
        If Request.QueryString("TicketID") <> "" Then ticketID = Request.QueryString("TicketID").ToLower
        Dim sqlstr As String = " SELECT TOP (1) sum(qty) as qty,sum([PLUPrice]*Qty) as totalPayable " _
        & "FROM [DST].[dbo].[tblTicketTrans_basket] where TicketID='" & ticketID & "' ;"
        Dim DT_Sums As DataTable
        productinfo = ""

        Dim sqlstr2 As String = " select *  FROM  [DST].[dbo].[tblTicketTrans_basket] where [TicketID] ='" & ticketID & "';"
        Dim DT2 As DataTable = SQLFunctions.GetDataTableFromSqlstr(sqlstr2, "DSTConnectionString")
        If DTClass.RowsinDataTable(DT2) > 0 Then
            DT_Sums = DTClass.SumDTColumns(DT2, "PLUPrice,Qty", " 1=1 ")
            Response.Write("sqlstr2=" & sqlstr2)
            Response.Write("#######" & DTClass.GetDTColumnNames(DT2) & "<br>####, DT_Sums.RecordCount=" & DT_Sums.Rows.Count.ToString)
            productinfo = populateform(DT_Sums)
        Else
            productinfo = "0"
        End If
        If productinfo = "" Then productinfo = "0"
        literal1.Text = productinfo
        DT2 = Nothing
        DT_Sums = Nothing
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
        & " <tr Class=""info""> " _
        & " <td width = ""25%""><a href=""#"" id=""add_discount"">Discount</a></td> " _
        & " <td Class=""text-right"" style=""padding-right:10px;""><span id=""ds_con"">" & Discount & "</span></td> " _
        & " <td width = ""25%""><a href=""#"" id=""add_tax"">Order Tax</a></td> " _
        & " <td Class=""text-right""><span id=""ts_con"">" & Order_Tax & "</span></td> " _
        & " </tr> " _
        & " <tr Class=""success""> " _
        & " <td colspan = ""2"" style=""font-weight:bold;""> " _
        & " Total Payable <a role=""button"" data-toggle=""modal"" data-target=""#noteModal""> " _
        & " <i Class=""fa fa-comment""></i> " _
        & " </a> " _
        & " </td>  " _
        & " <td Class=""text-right"" colspan=""2"" style=""font-weight:bold;""><span id=""total-payable"">" & TOTAL_Payable & "</span></td>" _
        & " </tr>" _
        & " </tbody>" _
        & " </table>"
        Return RV
    End Function


End Class
