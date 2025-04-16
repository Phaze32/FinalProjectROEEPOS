Imports System.IO
Imports System.Net
Imports Microsoft.VisualBasic
Imports System.Web.HttpContext
Imports System.Web
Imports System.Web.Script
Imports System.Web.Script.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Xml
Imports System.Threading
'Imports System.Net.Http
'Imports System.Net.Http.Headers
'Imports System.Net.Http.Formatting
Imports Newtonsoft.Json


Public Class HttpWebRequestControllerClass
    Public Class MyController
        ' This method handles GET requests to the "/get-data" endpoint
        <HttpGet("get-data")>
        Public Function GetData(ByVal url As String) As String
            If String.IsNullOrEmpty(url) Then
                Return "Please provide a valid URL."
            End If

            Dim request As WebRequest = WebRequest.Create(url)
            Dim response As WebResponse = request.GetResponse()
            Dim dataStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)

            Dim responseData As String = reader.ReadToEnd()

            reader.Close()
            dataStream.Close()
            response.Close()

            Return responseData ' Return the retrieved data
        End Function
    End Class


End Class
