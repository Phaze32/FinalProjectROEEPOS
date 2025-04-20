Public Class frmDatabase
    Private TstServerMySQL As String
    Private TstPortMySQL As String
    Private TstUserNameMySQL As String
    Private TstPwdMySQL As String
    Private TstDBNameMySQL As String
    Private Sub cmdTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTest.Click
        'Test database connection

        TstServerMySQL = txtServerHost.Text
        TstPortMySQL = txtPort.Text
        TstUserNameMySQL = txtUserName.Text
        TstPwdMySQL = txtPassword.Text
        TstDBNameMySQL = txtDatabase.Text


        Try
            conn.ConnectionString = "Server = '" & TstServerMySQL & "';  " _
                                         & "Port = '" & TstPortMySQL & "'; " _
                                         & "Database = '" & TstDBNameMySQL & "'; " _
                                         & "user id = '" & TstUserNameMySQL & "'; " _
                                         & "password = '" & TstPwdMySQL & "'"


            conn.Open()
            MsgBox("Test connection successful", MsgBoxStyle.Information, "Database Settings")

        Catch ex As Exception
            MsgBox("The system failed to establish a connection", MsgBoxStyle.Information, "Database Settings")

        End Try
        Call DisconnMy()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        TstServerMySQL = txtServerHost.Text
        TstPortMySQL = txtPort.Text
        TstUserNameMySQL = txtUserName.Text
        TstPwdMySQL = txtPassword.Text
        TstDBNameMySQL = txtDatabase.Text

        Try
            conn.ConnectionString = "Server = '" & TstServerMySQL & "';  " _
                                         & "Port = '" & TstPortMySQL & "'; " _
                                         & "Database = '" & TstDBNameMySQL & "'; " _
                                         & "user id = '" & TstUserNameMySQL & "'; " _
                                         & "password = '" & TstPwdMySQL & "'"
            conn.Open()

            DBNameMySQL = txtDatabase.Text
            ServerMySQL = txtServerHost.Text
            UserNameMySQL = txtUserName.Text
            PortMySQL = txtPort.Text
            PwdMySQL = txtPassword.Text

            Call SaveData()
            Me.Close()
        Catch ex As Exception
            MsgBox("The system failed to establish a connection", MsgBoxStyle.Information, "Database Settings")
        End Try
        Call DisconnMy()
        'save database
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub frmDatabase_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(178, 127)
        txtServerHost.Text = ServerMySQL
        txtPort.Text = PortMySQL
        txtUserName.Text = UserNameMySQL
        txtPassword.Text = PwdMySQL
        txtDatabase.Text = DBNameMySQL
    End Sub
End Class