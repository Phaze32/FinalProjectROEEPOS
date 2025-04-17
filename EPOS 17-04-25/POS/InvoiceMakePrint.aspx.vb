Imports System.Windows.Forms

Imports System.Drawing

Partial Class POS_InvoiceMakePrint
    Inherits System.Web.UI.Page



    Public Sub PrintInvoice(products As String(), prices As Decimal(), quantities As Integer(), totals As Decimal(), grandTotal As Decimal)
        ' Use PrintDocument to handle printing
        Dim printDoc As New Printing.PrintDocument

        AddHandler printDoc.PrintPage, Sub(sender, e)
                                           Dim font As New Font("Arial", 10)
                                           Dim startX As Integer = 10
                                           Dim startY As Integer = 10
                                           Dim offsetY As Integer = 20

                                           ' Add Title
                                           e.Graphics.DrawString("Peaches and Clean Dry Cleaning", New Font("Arial", 14, FontStyle.Bold), Brushes.Black, startX, startY)
                                           startY += offsetY
                                           e.Graphics.DrawString("Invoice", font, Brushes.Black, startX, startY)
                                           startY += offsetY

                                           ' Add Headers
                                           e.Graphics.DrawString("Product       Price (£)       Qty       Total (£)", font, Brushes.Black, startX, startY)
                                           startY += offsetY

                                           ' Add Data
                                           For i As Integer = 0 To products.Length - 1
                                               e.Graphics.DrawString($"{products(i)}       {prices(i):F2}       {quantities(i)}       {totals(i):F2}", font, Brushes.Black, startX, startY)
                                               startY += offsetY
                                           Next

                                           ' Add Grand Total
                                           startY += offsetY
                                           e.Graphics.DrawString($"Grand Total: {grandTotal:F2}", New Font("Arial", 12, FontStyle.Bold), Brushes.Black, startX, startY)
                                       End Sub

        ' Print
        printDoc.Print()
    End Sub

    Public Sub DisplayInvoice(products As String(), prices As Decimal(), quantities As Integer(), totals As Decimal(), grandTotal As Decimal)
        ' Create a new form to display the invoice
        Dim invoiceForm As New Form
        invoiceForm.Text = "Invoice Verification"
        invoiceForm.Size = New Size(400, 400)

        Dim richTextBox As New RichTextBox
        richTextBox.Dock = DockStyle.Fill
        richTextBox.ReadOnly = True

        ' Add invoice content to the RichTextBox
        richTextBox.AppendText("Peaches and Clean Dry Cleaning" & Environment.NewLine)
        richTextBox.AppendText("Invoice" & Environment.NewLine & Environment.NewLine)
        richTextBox.AppendText("Product       Price (£)       Qty       Total (£)" & Environment.NewLine)

        For i As Integer = 0 To products.Length - 1
            richTextBox.AppendText($"{products(i)}       {prices(i):F2}       {quantities(i)}       {totals(i):F2}" & Environment.NewLine)
        Next

        richTextBox.AppendText(Environment.NewLine & $"Grand Total: {grandTotal:F2}" & Environment.NewLine)

        ' Add a button for printing
        Dim printButton As New Button
        printButton.Text = "Print Invoice"
        printButton.Dock = DockStyle.Bottom
        AddHandler printButton.Click, Sub(sender, e)
                                          PrintInvoice(products, prices, quantities, totals, grandTotal)
                                      End Sub

        invoiceForm.Controls.Add(richTextBox)
        invoiceForm.Controls.Add(printButton)

        ' Show the form
        invoiceForm.ShowDialog()
    End Sub


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim products As String() = {"Trousers", "Evening Dress", "Shirt"}
        Dim prices As Decimal() = {4.95D, 15D, 2.6D}
        Dim quantities As Integer() = {3, 1, 1}
        Dim totals As Decimal() = {14.85D, 15D, 2.6D}
        Dim grandTotal As Decimal = totals.Sum()
        DisplayInvoice(products, prices, quantities, totals, grandTotal)
    End Sub
End Class
