Imports Microsoft.VisualBasic

Public Class mysqlFunctions

    Shared Sub getmysqlconnection(ByRef mysqlconn As String)


        Dim mySqlConnection As New MySql.Data.MySqlClient.MySqlConnection()
        mySqlConnection.ConnectionString = mysqlconn ' "your_ConnectionString"

        Try
            mySqlConnection.Open()

            Select Case mySqlConnection.State
                Case System.Data.ConnectionState.Open
                    ' Connection has been made
                    Exit Select
                Case System.Data.ConnectionState.Closed
                    ' Connection could not be made, throw an error
                    Throw New Exception("The database connection state is Closed")
                    Exit Select
                Case Else
                    ' Connection is actively doing something else
                    Exit Select

                    ' Place Your Code Here to Process Data //
            End Select
            ' Use the mySqlException object to handle specific MySql errors
        Catch mySqlException As MySql.Data.MySqlClient.MySqlException
            ' Use the exception object to handle all other non-MySql specific errors
        Catch exception As Exception
        Finally
            ' Make sure to only close connections that are not in a closed state
            If mySqlConnection.State <> System.Data.ConnectionState.Closed Then
                ' Close the connection as a good Garbage Collecting practice
                mySqlConnection.Close()
            End If
        End Try
    End Sub
End Class
