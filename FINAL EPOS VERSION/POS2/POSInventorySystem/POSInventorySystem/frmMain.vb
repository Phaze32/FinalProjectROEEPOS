Public Class frmMain
    Private Sub CloseChildForms()
        For Each frm As Form In Me.MdiChildren
            If frm.Name = "frmHome" Then
            Else
                frm.Close()
            End If
        Next
    End Sub
    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Application.Exit()
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getData()
        lblDate.Text = Date.Now.ToString("MM/dd/yyyy")
        frmHome.MdiParent = Me
        frmHome.Show()
        ' frmLogin.ShowDialog()
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        CloseChildForms()
        frmCategory.MdiParent = Me
        frmCategory.Show()
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        CloseChildForms()
        frmProduct.MdiParent = Me
        frmProduct.Show()
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        CloseChildForms()
        frmStaff.MdiParent = Me
        frmStaff.Show()
    End Sub

    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
        'frmPOS.ShowDialog()
        MsgBox("Message us on our facebook page - https://www.facebook.com/CyberVSS")
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        lblTimer.Text = Date.Now.ToString("hh:mm:ss")
    End Sub

    Private Sub VatToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VatToolStripMenuItem.Click
        frmDatabase.ShowDialog()
    End Sub

    Private Sub ToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton6.Click
        frmFilterStockIn.ShowDialog()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        frmFilterStockOut.ShowDialog()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub StaffToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StaffToolStripMenuItem.Click
        CloseChildForms()
        frmStaff.MdiParent = Me
        frmStaff.Show()
    End Sub

    Private Sub CategoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CategoryToolStripMenuItem.Click
        CloseChildForms()
        frmCategory.MdiParent = Me
        frmCategory.Show()
    End Sub

    Private Sub ProductToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProductToolStripMenuItem.Click
        CloseChildForms()
        frmProduct.MdiParent = Me
        frmProduct.Show()
    End Sub

    Private Sub POSToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles POSToolStripMenuItem.Click
        '  frmPOS.ShowDialog()
    End Sub

    Private Sub StocksInToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StocksInToolStripMenuItem.Click
        frmFilterStockIn.ShowDialog()
    End Sub

    Private Sub StocksOutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StocksOutToolStripMenuItem.Click
        frmFilterStockOut.ShowDialog()
    End Sub

    Private Sub CalculatorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalculatorToolStripMenuItem.Click
        Shell("Calc.exe")
    End Sub

    Private Sub NotepadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotepadToolStripMenuItem.Click
        Shell("Notepad.exe")
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        frmAbout.ShowDialog()
    End Sub

    Private Sub ToolStripButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton7.Click
        frmFilterDailySales.ShowDialog()
    End Sub
End Class
