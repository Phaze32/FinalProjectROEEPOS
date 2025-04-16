Imports System.Data
Imports System.Net
Imports Newtonsoft.Json
Imports System.Web.Script.Serialization

Partial Class POSCustomerList
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindGridView()
        End If
    End Sub

    Private Sub BindGridView()
        ' Define the URL of your handler
        Dim url As String = "Handlers/CustomerHandler.ashx"

        ' Create a WebClient to fetch the JSON response
        Using client As New WebClient()
            Try
                ' Fetch data from the handler
                Dim jsonResponse As String = client.DownloadString(url)

                ' Parse the JSON response into a DataTable
                Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(jsonResponse)

                ' Bind the DataTable to the GridView
                GridView1.DataSource = dt
                GridView1.DataBind()
            Catch ex As Exception
                ' Handle any errors during the process
                Console.WriteLine("Error fetching or processing data: " & ex.Message)
            End Try
        End Using
    End Sub

End Class
