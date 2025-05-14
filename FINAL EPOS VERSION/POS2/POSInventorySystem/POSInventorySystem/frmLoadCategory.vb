
Imports MySql.Data.MySqlClient
Public Class frmLoadCategory

    Private Sub LoadCategory()
        Try
            sqL = "SELECT * FROM Category WHERE CategoryName LIKE '" & txtCatName.Text & "%' ORDER BY CategoryName "
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            dr = cmd.ExecuteReader()

            Dim x As ListViewItem
            ListView1.Items.Clear()

            Do While dr.Read = True
                x = New ListViewItem(dr("CategoryNo").ToString)
                x.SubItems.Add(dr("CategoryName"))
                x.SubItems.Add(dr("Description"))

                ListView1.Items.Add(x)
            Loop

        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub

    Private Sub frmLoadCategory_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadCategory()
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        frmAddUpdateProduct.txtCategory.Text = ListView1.FocusedItem.SubItems(1).Text
        frmAddUpdateProduct.txtCategory.Tag = ListView1.FocusedItem.Text
        Me.Close()
    End Sub

End Class