Imports System.Data
Imports JSON_NS
Imports Newtonsoft.Json


Partial Class POS_TestGridviewFromJson
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindGridView()
        End If
    End Sub
    Shared Function getjasondata(url As String) As String
        Dim rv As String = HandlerUtility.GetJsonHandlerResponse(url)
        Return rv
    End Function

    Private Sub BindGridView()
        ' Handler URL
        Dim url As String = "http://localhost:8069/Handlers/CustomerHandler.ashx?customerid=1"

        Using client As New System.Net.WebClient()
            Try
                ' Fetch JSON response from the handler
                Dim jsonResponse As String = getjasondata(url)
                Response.Write("jsonResponse=" & jsonResponse)
                Response.End()
                ' Parse the JSON "data" property into a DataTable
                Dim jsonDict = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(jsonResponse)
                Dim dataArray As String = JsonConvert.SerializeObject(jsonDict("data"))
                Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(dataArray)

                ' Bind the DataTable to the GridView
                GridView1.DataSource = dt
                GridView1.DataBind()
            Catch ex As Exception
                ' Handle errors
                Console.WriteLine("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        BindGridView()
    End Sub

    Private Function GetBaseUrl() As String
        Dim request = HttpContext.Current.Request
        Dim baseUrl As String = request.Url.Scheme & "://" & request.Url.Authority
        Return baseUrl
    End Function

End Class
