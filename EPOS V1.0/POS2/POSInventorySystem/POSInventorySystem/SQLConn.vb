Imports MySql.Data.MySqlClient
Module SQLConn
    Public ServerMySQL As String
    Public PortMySQL As String
    Public UserNameMySQL As String
    Public PwdMySQL As String
    Public DBNameMySQL As String
    Public sqL As String
    Public ds As New DataSet()
    Public cmd As MySqlCommand
    Public dr As MySqlDataReader
    Public da As New MySqlDataAdapter

    Public conn As New MySqlConnection

    Sub getData()
        Dim AppName As String = Application.ProductName

        Try
            DBNameMySQL = GetSetting(AppName, "DBSection", "DB_Name", "temp")
            ServerMySQL = GetSetting(AppName, "DBSection", "DB_IP", "temp")
            PortMySQL = GetSetting(AppName, "DBSection", "DB_Port", "temp")
            UserNameMySQL = GetSetting(AppName, "DBSection", "DB_User", "temp")
            PwdMySQL = GetSetting(AppName, "DBSection", "DB_Password", "temp")
        Catch ex As Exception
            MsgBox("System registry was not established, you can set/save " & _
            "these settings by pressing F1", MsgBoxStyle.Information)
        End Try

    End Sub

    Public Sub ConnDB()
        conn.Close()
        Try
            conn.ConnectionString = "Server = '" & ServerMySQL & "';  " _
                                         & "Port = '" & PortMySQL & "'; " _
                                         & "Database = '" & DBNameMySQL & "'; " _
                                         & "user id = '" & UserNameMySQL & "'; " _
                                         & "password = '" & PwdMySQL & "'"

            conn.Open()
        Catch ex As Exception
            MsgBox("The system failed to establish a connection", MsgBoxStyle.Information, "Database Settings")
        End Try

    End Sub

    Public Sub DisconnMy()

        conn.Close()
        conn.Dispose()

    End Sub

    Sub SaveData()
        Dim AppName As String = Application.ProductName

        SaveSetting(AppName, "DBSection", "DB_Name", DBNameMySQL)
        SaveSetting(AppName, "DBSection", "DB_IP", ServerMySQL)
        SaveSetting(AppName, "DBSection", "DB_Port", PortMySQL)
        SaveSetting(AppName, "DBSection", "DB_User", UserNameMySQL)
        SaveSetting(AppName, "DBSection", "DB_Password", PwdMySQL)

        MsgBox("Database connection settings are saved.", MsgBoxStyle.Information)
    End Sub


   
End Module

