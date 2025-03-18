Public Class frmFilterStockOut

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If cmbMonth.Text = "" And chkMonthly.Checked = True Then
            MsgBox("Please select month", MsgBoxStyle.Exclamation, "Stock In")
            Exit Sub
        ElseIf cmbYear.Text = "" Then
            MsgBox("Please select year", MsgBoxStyle.Exclamation, "Stock In")
            Exit Sub
        ElseIf chkMonthly.Checked = False And chkYearly.Checked = False Then
            MsgBox("Please select one of the check box. Either monthly or yearly.", MsgBoxStyle.Exclamation, "Stock In")
            Exit Sub
        End If

        frmReportStockOut.ShowDialog()
    End Sub
    Private Sub chkMonthly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMonthly.CheckedChanged
        If chkMonthly.Checked = True Then
            chkYearly.CheckState = CheckState.Unchecked
        End If
    End Sub

    Private Sub chkYearly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkYearly.CheckedChanged
        If chkYearly.Checked = True Then
            chkMonthly.CheckState = CheckState.Unchecked
        End If
    End Sub

    Private Sub frmFilterStockOut_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbMonth.Text = Date.Now.ToString("MMMM")
        cmbYear.Text = Date.Now.ToString("yyyy")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class