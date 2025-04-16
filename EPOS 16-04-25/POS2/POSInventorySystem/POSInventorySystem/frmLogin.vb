

Imports MySql.Data.MySqlClient
Public Class frmLogin

    Public isCashier As Boolean
    Public isAdmin As Boolean
    Public strCashierID As String
    Private Sub Login()
        Try
            sqL = "SELECT * FROM Staff WHERE Username = '" & txtUsername.Text & "' AND UPassword = '" & txtPassword.Text & "'"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            dr = cmd.ExecuteReader()

            If dr.Read = True Then

                If LCase(dr("role")) = "admin" Then
                    MsgBox("Welcome to POS and Inventory System", MsgBoxStyle.Information, "Login")
                    frmMain.Show()

                    frmMain.tsslUser.Text = UCase(txtUsername.Text)
                    frmMain.tsslUser.Tag = dr("StaffID")
                    isCashier = False
                    isAdmin = True
                    frmMain.Show()
                    Me.Hide()
                End If

                If LCase(dr("role")) = "cashier" Then
                    MsgBox("Welcome to POS and Inventory System", MsgBoxStyle.Information, "Login")
                    strCashierID = dr("StaffID")
                    ' frmPOS.Show()

                    isCashier = True
                    isAdmin = False
                    Me.Hide()
                End If





            Else
                MsgBox("Incorrect username or password.", MsgBoxStyle.Critical, "Login")
                txtUsername.SelectAll()
                txtPassword.SelectAll()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub


    Private Sub TextBox1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUsername.GotFocus
        AcceptButton = Button1
    End Sub

    Private Sub TextBox2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPassword.GotFocus
        AcceptButton = Button1
        If txtUserName.Text = "" Then
            txtUserName.Focus()

        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Login()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If MsgBox("Are you sure you want to close?", MsgBoxStyle.YesNo, "Close Window") = MsgBoxResult.Yes Then
            End
        End If
    End Sub

    Private Sub frmLogin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F4 And e.Modifiers = Keys.Alt Then
            e.Handled = True
        End If

        If e.KeyCode = Keys.F12 Then
            frmDatabase.Show()
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        For Each selProcess As Process In Process.GetProcesses
            If selProcess.ProcessName = "taskmgr" Then
                selProcess.Kill()
                Exit For
            End If
        Next
    End Sub

    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getData()
    End Sub
End Class