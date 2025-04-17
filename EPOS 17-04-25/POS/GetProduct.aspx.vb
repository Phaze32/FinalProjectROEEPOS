
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Partial Class POS_get_product_GetProduct_
    Inherits System.Web.UI.Page
    Shared productinfo As String = ""
    Shared pluid As String = ""
    'Shared connestring As String = "Drycleaning_DBConnectionString" ' "DSTConnectionString" '
    Shared connestring As String = "DSTConnectionString" '
    Private Sub POS_get_product_GetProduct__LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        ' pluid = Request.QueryString("productid")
        literal1.Text = ""
        If Request.QueryString("Productid") <> "" Then pluid = Request.QueryString("Productid")
        Dim sqlstr As String = "select description +','+ convert(varchar(5),round(price,2)) from [tblPLU] where PLUID=" & pluid & " ;"
        sqlstr = "select top (1) * from [tblPLU] where PLUID=" & pluid & " ;"

        Dim DT As DataTable = SQLFunctions.GetDataTableFromSqlstr(sqlstr, connestring)
        'productinfo = SQLFunctions.GetSearchDataSQLWithOutConStr("select description +','+ convert(varchar(5),round(price,2)) from [tblPLU] where PLUID=" & pluid & " ;",connestring)
        Dim price As String = FormatNumber(DT.Rows(0)("price"), 2).ToString()
        Dim description As String = DT.Rows(0)("description").ToString()
        Dim ticketID As String = "66"
        Dim PLUDescription As String = ""
        Dim PLUPrice As String = ""
        Dim Qty As String = ""
        Dim LineTotal As String = ""
        Dim PLU_ID As String = ""
        'Dim description As String = DT.Rows(0)("description").ToString()
        'productinfo = description & "," & price

        SQLFunctions.SqlExecuteWOConstring("if Not exists(select *  FROM  [DST].[dbo].[tblTicketTrans_basket] where [TicketID] ='66' and pluid= " & pluid & " ) " _
                                           & "begin insert into [tblTicketTrans_basket]([TicketID],[Qty],[HalfPrice],[Charity],[VAT],[StockSales],[PLUID],[PLUPrice],[PLUDescription]) " _
                                           & "Values ( '" & ticketID & "','1','0','0','1','1','" & pluid & "','" & price & "','" & description & "') end " _
                                           & "else begin update tblTicketTrans_basket set qty = qty+ 1 where [TicketID] ='66' and pluid='" & pluid & "' end;", connestring)
        productinfo = ""
        Dim sqlstr2 As String = " select *  FROM  [DST].[dbo].[tblTicketTrans_basket] where [TicketID] ='" & ticketID & "';"
        Dim DT2 As DataTable = SQLFunctions.GetDataTableFromSqlstr(sqlstr2, connestring)
        productinfo = "<table id = ""Basket"" class=""table table-striped table-condensed table-hover list-table"">"
        Dim rows As Integer = DT2.Rows.Count
        'Response.Write("rows=" & DTClass.GetDTColumnNames(DT2))
        Dim Dts As String = DTClass.DataTableToCSV(DT2)
        TextCSVClass.WiteToServer("CSVFiles\", "basket.csv", Dts)
        'Response.Write("Dts.Drycleaning_DBConnectionString=" & GetDatabaseNameFromConfig("Drycleaning_DBConnectionString"))
        'Response.End()
        Dim a As Integer
        For a = 0 To rows - 1
            PLUDescription = DT2.Rows(a)("PLUDescription")
            PLUPrice = DT2.Rows(a)("PLUPrice")
            PLUPrice = MiscClass.myCurrencyFormat(PLUPrice, 2)
            Qty = DT2.Rows(a)("Qty")
            LineTotal = PLUPrice * Qty
            LineTotal = MiscClass.myCurrencyFormat(LineTotal, 2)
            PLU_ID = DT2.Rows(a)("pluid")
            'productinfo += "<tr><td>" & PLUDescription & "</td><td>" & PLUPrice & "</td><td>" & Qty & "</td><td>" & FormatNumber(LineTotal, 2) & "</td><td><asp:HiddenField ID=""Hidden_PLUID"" runat=""server"" Value=" & PLU_ID & " /><asp:HiddenField ID=""Hidden_TicketID"" runat=""server"" Value=" & ticketID & " /><i class=""fa fa-trash-o tip pointer posdel""></i></td></tr>"
            If Qty > 0 Then productinfo += GetSelectedProductRow(PLUDescription, PLUPrice, Qty, LineTotal, PLU_ID, ticketID)
        Next
        productinfo += "</table>"
        If productinfo = "" Then productinfo = "ErrorFormatNumber"
        literal1.Text = productinfo
        DT = Nothing
        DT2 = Nothing
    End Sub
    Private Function GetSelectedProductRow(PLUDescription As String, PLUPrice As String, Qty As String, LineTotal As String, PLU_ID As String, ticketID As String) As String
        Dim listrow As String = ""
        listrow = "<tr id = '" & PLU_ID & "' Class='2' data-item-id='2'>" _
                    & "<td>" _
                    & "<input name = 'product_id' type='hidden' Class='rid' value='" & PLU_ID & "'>" _
                     & "<input name = 'ticket_id' type='hidden' Class='rid' value='" & ticketID & "'>" _
                    & "<input name = 'item_comment' type='hidden' Class='ritem_comment' value=''>" _
                    & "<input name = 'product_code' type='hidden' value='" & PLU_ID & "'>" _
                    & "<input name = 'product_name' type='hidden' value='" & PLUDescription & "'>" _
                    & "<button type = 'button' Class='btn bg-purple btn-block btn-xs edit' id='" & PLU_ID & "_" & ticketID & "' data-item='2'>" _
                    & "<span Class='sname' id='" & PLUDescription & "_" & PLU_ID & "'>" & PLUDescription & "</span>" _
                    & "</button>" _
                    & "</td>" _
                    & "<td Class='text-right'>" _
                    & "<input class='realuprice' name='real_unit_price' type='hidden' value='" & PLUPrice & "'>" _
                    & "<input class='rdiscount' name='product_discount' type='hidden' id='discount_" & PLU_ID & "' value='0'>" _
                    & "<span class='text-right sprice' id='sprice_" & PLU_ID & "'>" & PLUPrice & "</span>" _
                    & "</td>" _
                    & "<td> <input name = 'item_was_ordered' type='hidden' Class='riwo' value='" & PLU_ID & "'>" _
                    & "<input Class='form-control input-qty kb-pad text-center rquantity2' name='quantity' type='text' value='" & Qty & "' data-id='" & PLU_ID & "' data-item='2' id='quantity_" & ticketID & "_" & PLU_ID & "' onchange='changeQty(this);'>" _
                    & "</td>" _
                    & "<td Class='text-right'><span class='text-right ssubtotal' id='subtotal_" & ticketID & "'>" & LineTotal & "</span></td>" _
                    & "<td class='text-center'>" _
                    & "<i Class='fa fa-trash-o tip pointer posdel' id='" & ticketID & "' title='Remove' value='0'data-id='" & PLU_ID & "' onc='changeQty(this);'></i>" _
                    & "</td>" _
                    & "</tr>"
        Return listrow
    End Function
    Public Function GetDatabaseNameFromConfig(YourConnectionStringName As String) As String
        ' Retrieve the connection string from the web.config file
        Dim connectionString As String = ConfigurationManager.ConnectionStrings(YourConnectionStringName).ConnectionString

        ' Initialize an empty database name
        Dim databaseName As String = String.Empty

        ' Parse the connection string
        Dim builder As New SqlConnectionStringBuilder(connectionString)

        ' Retrieve the database name from the parsed connection string
        databaseName = builder.InitialCatalog

        Return databaseName
    End Function


End Class
