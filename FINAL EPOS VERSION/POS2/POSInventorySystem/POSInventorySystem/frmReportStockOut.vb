Imports MySql.Data.MySqlClient
Public Class frmReportStockOut

    Dim strMonthNo As String
    Dim y As Integer

    Private Sub MonthInNumber()
        Select Case frmFilterStockOut.cmbMonth.Text
            Case "January"
                strMonthNo = "01"
            Case "February"
                strMonthNo = "02"
            Case "March"
                strMonthNo = "03"
            Case "April"
                strMonthNo = "04"
            Case "May"
                strMonthNo = "05"
            Case "June"
                strMonthNo = "06"
            Case "July"
                strMonthNo = "07"
            Case "August"
                strMonthNo = "08"
            Case "September"
                strMonthNo = "09"
            Case "October"
                strMonthNo = "10"
            Case "November"
                strMonthNo = "11"
            Case "December"
                strMonthNo = "12"
            Case Else
                strMonthNo = "00"
        End Select

    End Sub

    Private Sub LoadStocksOutReport()
        MonthInNumber()

        Dim totStockOut As Double
        Dim totSales As Double
        Try
            If frmFilterStockOut.chkMonthly.Checked = True Then
                sqL = "SELECT ProductCode, P.Description, TDate, SUM(TD.Quantity) as totalQuantity, TD.ItemPrice FROM Product as P, Transactions as T, TransactionDetails as TD WHERE P.ProductNo = TD.ProductNo AND TD.InvoiceNo = T.InvoiceNo AND TDate LIKE '" & strMonthNo & "%' AND TDate LIKE '%" & frmFilterStockOut.cmbYear.Text & "' GROUP BY P.ProductNo, TDate ORDER By TDate"
            Else
                sqL = "SELECT ProductCode, P.Description, TDate, SUM(TD.Quantity) as totalQuantity, TD.ItemPrice FROM Product as P, Transactions as T, TransactionDetails as TD WHERE P.ProductNo = TD.ProductNo AND TD.InvoiceNo = T.InvoiceNo AND  TDate LIKE '%" & frmFilterStockOut.cmbYear.Text & "' GROUP BY P.ProductNo, TDate ORDER By TDate"
            End If

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
        If frmFilterStockOut.chkMonthly.Checked = True Then
            lblCollections.Text = "Stocks-Out for the Month of " & frmFilterStockOut.cmbMonth.Text & " " & frmFilterStockOut.cmbYear.Text
        Else
            frmFilterStockOut.chkYearly.Checked = True
            lblCollections.Text = "Stocks-Out for the Year of " & frmFilterStockOut.cmbYear.Text
        End If

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