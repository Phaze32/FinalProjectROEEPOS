
Imports MySql.Data.MySqlClient

Public Class frmStockIn

    Private Sub GetProductInfo()
        With frmProduct.ListView1
            lblProductCode.Tag = .FocusedItem.Text
            lblProductCode.Text = .FocusedItem.SubItems(1).Text
            lblDescription.Text = .FocusedItem.SubItems(2).Text
            lblPrice.Text = .FocusedItem.SubItems(5).Text
            lblCurrentStocks.Text = .FocusedItem.SubItems(6).Text
        End With
    End Sub

    Private Sub AddStockIn()
        Try
            sqL = "INSERT INTO StockIn(ProductNo, Quantity, DateIn) Values('" & lblProductCode.Tag & "', '" & txtQuantity.Text & "', '" & Date.Now.ToString("MM/dd/yyyy") & "')"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Stocks successfully added.", MsgBoxStyle.Information, "Add Stocks")
            UpdateProductQuantity()
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub

    Private Sub UpdateProductQuantity()
        Try
            sqL = "UPDATE Product SET StocksOnhand = StocksOnHand + '" & Val(txtQuantity.Text.Replace(",", "")) & "' WHERE ProductNo = '" & lblProductCode.Tag & "'"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub

    Private Sub frmStockIn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetProductInfo()
        txtQuantity.Text = ""
        txtTotalStocks.Text = ""
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        AddStockIn()
        frmProduct.LoadProducts()
        Me.Close()
    End Sub

    Private Sub txtQuantity_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQuantity.TextChanged
        txtTotalStocks.Text = Format(Val(lblCurrentStocks.Text) + Val(txtQuantity.Text), "#,##0")
    End Sub
End Class