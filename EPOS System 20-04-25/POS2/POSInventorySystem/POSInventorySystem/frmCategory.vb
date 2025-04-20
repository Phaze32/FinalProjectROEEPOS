
Imports MySql.Data.MySqlClient

Public Class frmCategory

    Public adding As Boolean
    Public updating As Boolean

    Public Sub LoadCategories()
        Try
            sqL = "SELECT * FROM CATEGORY WHERE CategoryName LIKE '" & lblSearch.Text & "%' ORDER By CategoryName"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

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

    Private Sub frmCategory_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        adding = True
        updating = False
        frmAddUpdateCategory.ShowDialog()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        Dim strSearch As String = InputBox("Enter category name :", "Search Category", " ")
        If strSearch.Length >= 1 Then
            lblSearch.Text = strSearch.Trim
            LoadCategories()
        ElseIf strSearch = "" Then
            Exit Sub
        End If
       
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        If ListView1.Items.Count = 0 Then
            MsgBox("Please select record to update", MsgBoxStyle.Exclamation, "Update")
            Exit Sub
        End If
        Try
            If ListView1.FocusedItem.Text = "" Then
            Else
                adding = False
                updating = True
                frmAddUpdateCategory.ShowDialog()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class