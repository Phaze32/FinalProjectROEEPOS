
Imports MySql.Data.MySqlClient
Public Class frmAddUpdateProduct

    Private Sub GetProductNo()
        Try
            sqL = "SELECT ProductNo FROM Product ORDER BY ProductNo DESC"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            dr = cmd.ExecuteReader

            If dr.Read = True Then
                lblProductNo.Text = dr("ProductNo") + 1
            Else
                lblProductNo.Text = 1
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub


    Private Sub LoadUpdateCategory()
        Try
            sqL = "SELECT ProductNo, ProductCode, P.Description, Barcode, P.CategoryNo, CategoryName, UnitPrice, StocksOnHand, ReorderLevel FROM Product as P, Category as C WHERE C.CategoryNo = P.CategoryNo AND ProductNo = '" & frmProduct.ListView1.FocusedItem.Text & "'"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            dr = cmd.ExecuteReader

            If dr.Read = True Then
                lblProductNo.Text = dr("ProductNo")
                txtProductCode.Text = dr("ProductCode")
                txtDescription.Text = dr("Description")
                txtBarcode.Text = dr("Barcode")
                txtCategory.Text = dr("CategoryName")
                txtCategory.Tag = dr("CategoryNo")
                txtUnitPrice.Text = Format(dr("UnitPrice"), "#,##0.00")
                txtStocksOnHand.Text = dr("StocksOnHand")
                txtReorderLevel.Text = dr("ReorderLevel")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub


    Private Sub AddProducts()
        Try
            sqL = "INSERT INTO Product(ProductCode, Description, Barcode, UnitPrice, StocksOnHand, ReorderLevel, CategoryNo) VALUES('" & txtProductCode.Text & "', '" & txtDescription.Text & "', '" & txtBarcode.Text.Trim & "', '" & txtUnitPrice.Text.Replace(",", "") & "', '" & txtStocksOnHand.Text.Replace(",", "") & "', '" & txtReorderLevel.Text & "', '" & txtCategory.Tag & "')"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Product successfully added.", MsgBoxStyle.Information, "Add Product")
            AddStockIn()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub

    Private Sub AddStockIn()
        Try
            sqL = "INSERT INTO StockIn(ProductNo, Quantity, DateIn) Values('" & lblProductNo.Text & "', '" & txtStocksOnHand.Text & "', '" & Date.Now.ToString("MM/dd/yyyy") & "')"
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

    Private Sub UpdateProduct()
        Try
            sqL = "UPDATE Product SET ProductCode = '" & txtProductCode.Text & "', Description = '" & txtDescription.Text & "', Barcode = '" & txtBarcode.Text.Trim & "', UnitPrice = '" & txtUnitPrice.Text.Replace(",", "") & "', StocksOnHand = '" & txtStocksOnHand.Text.Replace(",", "") & "', ReorderLevel = '" & txtReorderLevel.Text & "', CategoryNo ='" & txtCategory.Tag & "' WHERE ProductNo = '" & lblProductNo.Text & "'"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            cmd.ExecuteNonQuery()

            MsgBox("Product successfully Updated.", MsgBoxStyle.Information, "Update Product")
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub

    Private Sub ClearFields()
        lblProductNo.Text = ""
        txtProductCode.Text = ""
        txtDescription.Text = ""
        txtBarcode.Text = ""
        txtCategory.Text = ""
        txtUnitPrice.Text = ""
        txtStocksOnHand.Text = ""
        txtReorderLevel.Text = ""
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub frmAddUpdateProduct_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If frmProduct.adding = True Then
            Label1.Text = "Adding New Product"
            ClearFields()
            GetProductNo()
        Else
            Label1.Text = "Updating Product"
            LoadUpdateCategory()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If frmProduct.adding = True Then
            AddProducts()
            Me.Close()
        Else
            UpdateProduct()
            frmProduct.LoadProducts()
            Me.Close()
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        frmLoadCategory.ShowDialog()
    End Sub

    Private Sub txtUnitPrice_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUnitPrice.LostFocus
        txtUnitPrice.Text = Format(Val(txtUnitPrice.Text.Replace(",", "")), "#,##0.00")
    End Sub
End Class