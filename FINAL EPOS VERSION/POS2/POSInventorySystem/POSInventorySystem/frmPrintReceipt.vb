
Imports MySql.Data.MySqlClient
Public Class frmPrintReceipt


    Private Sub LoadItemstoDatagrid()
        Dim y As Integer
        Try
            sqL = "Select SUM(Quantity) as TotalQuantity, Description, ItemPrice, (ItemPrice * SUM(Quantity)) as Amount FROM Product as P, TransactionDetails as TD WHERE TD.ProductNo = P.ProductNo AND InvoiceNo ='' GROUP BY TD.ProductNo"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            dr = cmd.ExecuteReader
            dgw.Rows.Clear()
            Do While dr.Read = True
                dgw.Rows.Add(dr("TotalQuantity"), dr("Description"), dr("ItemPrice"), dr("Amount"))
                dgw.Height += 19
                y += 19
            Loop

            Panel2.Location = New Point(9, 187 + y)
            Panel1.Height += y
            Me.Height += y
            dgw.Height -= 20
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub
    Private Sub LoadReceiptInfo()
        Try
            sqL = "SELECT T.InvoiceNo, CONCAT(Lastname,', ', Firstname, ' ', MI, '.') as StaffName, Quantity, Description, ItemPrice, (ItemPRice * Quantity) as ItemAmount, VatAmount, NonVatTotal, TotalAmount, Cash, PChange FROM Product as P, TransactionDetails as TD, Transactions as T, Staff as S, Payment as Pay WHERE P.ProductNo = TD.ProductNo AND TD.InvoiceNo = T.InvoiceNo AND S.StaffID = T.StaffID AND Pay.InvoiceNo = T.InvoiceNO AND T.InvoiceNo = ''"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            dr = cmd.ExecuteReader

            If dr.Read = True Then
                lblInvoice.Text = dr("InvoiceNo")
                lblEmpName.Text = dr("StaffName")
                lblDate.Text = Date.Now.ToString("MM/dd/yyyy")
                lblTime.Text = Date.Now.ToString("hh:mm:ss")
                lblVat.Text = FormatNumber(dr("VatAmount"))
                lblSubtotal.Text = FormatNumber(dr("NonVatTotal"))
                lblTotal.Text = FormatNumber(dr("TotalAmount"))
                lblCash.Text = FormatNumber(dr("Cash"))
                lblChange.Text = FormatNumber(dr("PChange"))
                LoadItemstoDatagrid()
            End If
           
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub

    Private Sub frmPrintReceipt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadReceiptInfo()
        PrintDocument1.Print()
        Me.Close()
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim bm As New Bitmap(Me.Panel1.Width, Me.Panel1.Height)
        Panel1.DrawToBitmap(bm, New Rectangle(0, 0, Me.Panel1.Width, Me.Panel1.Height))
        e.Graphics.DrawImage(bm, 0, 0)
        Dim aPS As New PageSetupDialog
        aPS.Document = PrintDocument1
    End Sub
End Class