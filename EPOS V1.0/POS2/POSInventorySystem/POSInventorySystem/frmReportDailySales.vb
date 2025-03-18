
Imports MySql.Data.MySqlClient
Public Class frmReportDailySales


    Dim strMonthNo As String
    Dim y As Integer

    Private Sub LoadStocksOutReport()
       
        Dim totStockOut As Double
        Dim totSales As Double
        Try
           
            sqL = "SELECT ProductCode, P.Description, TDate, SUM(TD.Quantity) as totalQuantity, TD.ItemPrice FROM Product as P, Transactions as T, TransactionDetails as TD WHERE P.ProductNo = TD.ProductNo AND TD.InvoiceNo = T.InvoiceNo AND  TDate LIKE '" & frmFilterDailySales.DateTimePicker1.Value.ToString("MM/dd/yyyy") & "' GROUP BY P.ProductNo, TDate ORDER By TDate"


            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            dr = cmd.ExecuteReader

            dgw.Rows.Clear()
            totStockOut = 0.0
            totSales = 0.0
            y = 0
            Do While dr.Read = True
                dgw.Rows.Add(dr("ProductCode"), dr("Description"), dr("TDate"), dr("totalQuantity"), Format(dr("ItemPrice"), "#,##0.00"), Format((dr("ItemPrice") * dr("TotalQuantity")), "#,##0.00"))
                y += 17
                totStockOut += dr("totalQuantity")
                totSales += (dr("ItemPrice") * dr("TotalQuantity"))
            Loop
            dgw.Height += y
            lblTotalStocksIn.Text = totStockOut
            lblSales.Text = Format(totSales, "#,##0.00")
            Panel3.Location = New Point(Me.Panel3.Location.X, Me.Panel3.Location.Y + y)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub

    Private Sub frmReportStockOut_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        lblCollections.Text = " Sales of " & frmFilterDailySales.DateTimePicker1.Value.ToString("MMMM dd, yyyy")

        LoadStocksOutReport()
        lbldate.Text = Date.Now.ToString("MMMM dd, yyyy")
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim bm As New Bitmap(Me.Panel1.Width, Me.Panel1.Height)

        Panel1.DrawToBitmap(bm, New Rectangle(0, 0, Me.Panel1.Width, Me.Panel1.Height))

        e.Graphics.DrawImage(bm, 0, 40)
        Dim aPS As New PageSetupDialog
        aPS.Document = PrintDocument1
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        PrintDialog1.Document = Me.PrintDocument1

        Dim ButtonPressed As DialogResult = PrintDialog1.ShowDialog()
        If (ButtonPressed = DialogResult.OK) Then
            Panel1.Height += y
            PrintDocument1.Print()
            Me.Close()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

End Class