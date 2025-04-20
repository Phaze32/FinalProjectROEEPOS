
Imports MySql.Data.MySqlClient

Public Class frmReportStockIn

    Dim strMonthNo As String
    Dim y As Integer

    Private Sub MonthInNumber()
        Select Case frmFilterStockIn.cmbMonth.Text
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

    Private Sub LoadStocksInReport()
        MonthInNumber()
        Dim y As Integer
        Dim totStocksIn As Double
        Try
            If frmFilterStockIn.chkMonthly.Checked = True Then
                sqL = "SELECT ProductCode, Description, SUM(Quantity) as totalQuantity, DateIN FROM Product as P, StockIn as S WHERE S.ProductNo = P.ProductNo AND DATEIN LIKE '" & strMonthNo & "%' AND DATEIN LIKE '%" & frmFilterStockIn.cmbYear.Text & "' GROUP BY P.ProductNo, DateIN ORDER BY DateIn"
            Else
                sqL = "SELECT ProductCode, Description, SUM(Quantity) as totalQuantity, DateIN FROM Product as P, StockIn as S WHERE S.ProductNo = P.ProductNo AND DATEIN LIKE '%" & frmFilterStockIn.cmbYear.Text & "' GROUP BY P.ProductNo, DateIN ORDER BY DateIn"
            End If

            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            dr = cmd.ExecuteReader

            dgw.Rows.Clear()
            y = 0
            totStocksIn = 0
            Do While dr.Read = True
                dgw.Rows.Add(dr("ProductCode"), dr("Description"), dr("DateIn"), dr("totalQuantity"))
                y += 17
                totStocksIn += dr("totalQuantity")
            Loop
            dgw.Height += y
            lblTotalStocksIn.Text = totStocksIn
            Panel3.Location = New Point(Me.Panel3.Location.X, Me.Panel3.Location.Y + y)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub

    Private Sub frmReportStockIn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If frmFilterStockIn.chkMonthly.Checked = True Then
            lblCollections.Text = "Stocks-In for the Month of " & frmFilterStockIn.cmbMonth.Text & " " & frmFilterStockIn.cmbYear.Text
        ElseIf frmFilterStockIn.chkYearly.Checked = True Then
            lblCollections.Text = "Stocks-In for the Year of " & frmFilterStockIn.cmbYear.Text
        End If

        LoadStocksInReport()
        lbldate.Text = Date.Now.ToString("MMMM dd, yyyy")
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        PrintDialog1.Document = Me.PrintDocument1

        Dim ButtonPressed As DialogResult = PrintDialog1.ShowDialog()
        If (ButtonPressed = DialogResult.OK) Then
            PrintDocument1.Print()
            Me.Close()
        End If
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim bm As New Bitmap(Me.Panel1.Width, Me.Panel1.Height)

        Panel1.DrawToBitmap(bm, New Rectangle(0, 0, Me.Panel1.Width, Me.Panel1.Height))

        e.Graphics.DrawImage(bm, 50, 60)
        Dim aPS As New PageSetupDialog
        aPS.Document = PrintDocument1
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class