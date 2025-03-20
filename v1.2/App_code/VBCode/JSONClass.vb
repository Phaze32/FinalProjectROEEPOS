Imports System
Imports System.Data
Imports System.ComponentModel
Imports System.Net
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.Web.HttpContext
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Script
Imports System.Web.Script.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports System.Xml
Imports System.Threading
'Imports System.Net.Http
'Imports System.Net.Http.Headers
'Imports System.Net.Http.Formatting
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace JSON_NS

    Public Class JSONClass
        Shared serializer As JavaScriptSerializer
        Shared ScriptConverter As JavaScriptConverter
        '********************
        Shared Function CheckAndFixJson(ByVal jsonData As String) As String
            ' Attempt to parse the JSON data
            Dim errorMessage As String = ""
            Try
                Dim parsedData = JsonConvert.DeserializeObject(jsonData)
                'Return jsonData ' No issues found, return original data
                'Catch ex As JsonReaderException
                'Dim errorMessage As String = ex.Message

                ' Fix common issues:
                jsonData = jsonData.Replace("\r\n", "") ' Remove newlines
                jsonData = jsonData.Replace("\t", "") ' Remove tabs
                jsonData = jsonData.Replace("\\", "") ' Remove escaped backslashes
                jsonData = jsonData.Replace("\", "") ' Remove escaped backslashes
                ' Try parsing again after fixing
                'Try
                'parsedData = JsonConvert.DeserializeObject(jsonData)
                Return jsonData ' Fix successful, return fixed data
            Catch ex As Exception ' Catch any other exceptions during parsing
                errorMessage = String.Concat(errorMessage, vbCrLf, "Fix unsuccessful: ", ex.Message)
                Throw New Exception(errorMessage) ' Re-throw with combined error message
                'End Try
            End Try
        End Function
        Shared Function JsonCallGetWithString(URI As String, jsonstring As String, Optional RequestMethod As String = "GET", Optional token As String = "", Optional updatedAtFrom As String = "2017-01-16T13:33:00+00:00", Optional updatedAtTo As String = "2018-12-30T16:33:00+00:00") As String
            Dim str As String = "Nothing"
            Dim res As WebResponse
            Dim stringWriter As StringWriter = New StringWriter()
            Dim obj1 = New With {
                                   Key .updatedAtFrom = updatedAtFrom,
                                   Key .updatedAtTo = updatedAtTo
                               }
            'obj = obj1
            Try
                Dim webclient As New System.Net.WebClient()
                Dim callbackurl As String = String.Empty

                Dim httpWebRequest As HttpWebRequest = DirectCast(WebRequest.Create(URI), HttpWebRequest)
                ' Dim request As HttpWebRequest = httpWebRequest.Create(url)

                httpWebRequest.Headers.Add("Authorization", "Bearer " & token)
                httpWebRequest.Method = RequestMethod
                httpWebRequest.ContentType = "application/json; charset=utf-8"
                If RequestMethod.ToUpper = "POST" Then
                    Dim jsz As New System.Web.Script.Serialization.JavaScriptSerializer()
                    'Dim postData As String = jsz.Serialize(jsonstring)
                    Dim data As Byte() = Encoding.ASCII.GetBytes(jsonstring)
                    'HttpContext.Current.Response.Write("URI=" & URI.ToString & "<br>RequestMethod=" & RequestMethod & "<br>data.Length=" & data.Length & "<br>")
                    'HttpContext.Current.Response.Write("postData=" & jsonstring.ToString )
                    httpWebRequest.ContentLength = data.Length
                    Using stream As System.IO.Stream = httpWebRequest.GetRequestStream()
                        stream.Write(data, 0, data.Length)
                        stream.Close()
                    End Using
                End If

                res = httpWebRequest.GetResponse()
                'HttpContext.Current.Response.Write("JSONClass.JsonCallGet.res=" & res.ToString)
                Dim sr = New StreamReader(res.GetResponseStream(), System.Text.Encoding.Default)
                str = sr.ReadToEnd()
                'HttpContext.Current.Response.Write("JSONClass.JsonCallGet.sr=" & sr.ToString)
            Catch ex As Exception
                ' Console.WriteLine("Status Code : {0}", CType(Res, HttpWebResponse).StatusCode)
                'Console.SetOut(stringWriter)
                Dim err As String = stringWriter.ToString
                str = "<font color='red'>" & (ex.Message & "<br>" & err & "</font>")
            End Try

            Return str
        End Function
        '********************
        Shared Function JSONtoArray(JsonText As String) As Array
            serializer = New JavaScriptSerializer()
            'serializer.RegisterConverters(New JavaScriptConverter() {New System.Web.Script.Serialization.VB.ListItemCollectionConverter()})
            Dim recoveredList As ListItemCollection = serializer.Deserialize(Of ListItemCollection)(JsonText)
            Dim newListItemArray(recoveredList.Count - 1) As ListItem
            recoveredList.CopyTo(newListItemArray, 0)
            Return newListItemArray
        End Function

        Shared Function ListItemsToJSON(ByVal LB As ListBox) As String
            serializer = New JavaScriptSerializer()
            'serializer.RegisterConverters(New JavaScriptConverter() {New System.Web.Script.Serialization.VB.ListItemCollectionConverter()})
            Return serializer.Serialize(LB.Items)
        End Function
        ''' <summary>
        ''' to check if json file has data and its size
        ''' </summary>
        ''' <param name="json">jason data as string</param>
        ''' <returns>length of the json data set or table</returns>
        Shared Function JSONtoDTLength(json As String) As String
            serializer = New JavaScriptSerializer()
            'serializer.RegisterConverters(New JavaScriptConverter() {New System.Web.Script.Serialization.VB.ListItemCollectionConverter()})
            Dim data As DataSet = serializer.Deserialize(Of DataSet)(json)
            ' Dim dt As DataTable = CType(serializer.Deserialize(json, GetType(DataTable)), DataTable)
            Dim dt As DataTable = serializer.Deserialize(Of DataTable)(json.ToString)
            'HttpContext.Current.Response.Write(json.Length)
            Return json.Length
        End Function
        ''' <summary>
        ''' COnverts json data  into data set to be processed
        ''' </summary>
        ''' <param name="json">JSOn file as data as a string to be converted</param>
        ''' <returns>returns a vaid dataset table</returns>
        Shared Function JSONtoDT(json As String) As DataTable
            serializer = New JavaScriptSerializer()
            'serializer.RegisterConverters(New JavaScriptConverter() {New System.Web.Script.Serialization.VB.ListItemCollectionConverter()})
            Dim data As DataSet = serializer.Deserialize(Of DataSet)(json)
            ' Dim dt As DataTable = CType(serializer.Deserialize(json, GetType(DataTable)), DataTable)
            Dim dt As DataTable = serializer.Deserialize(Of DataTable)(json.ToString)
            'HttpContext.Current.Response.Write(json.Length)
            Return dt
        End Function
        ''' <summary>
        ''' converts a list into Datatable 
        ''' </summary>
        ''' <typeparam name="T">IList<T> can store a collection of elements of any type T</typeparam>
        ''' <param name="data"></param>
        ''' <returns>dt as datatable</returns>
        Shared Function ToDataTable(Of T)(ByVal data As IList(Of T)) As DataTable
            Dim props As PropertyDescriptorCollection = TypeDescriptor.GetProperties(GetType(T))
            Dim dt As DataTable = New DataTable
            Dim i As Integer = 0
            Do While (i < props.Count)
                Dim prop As PropertyDescriptor = props(i)
                dt.Columns.Add(prop.Name, prop.PropertyType)
                i = (i + 1)
            Loop

            Dim values() As Object = New Object((props.Count) - 1) {}
            For Each item As T In data
                ' Dim i As Integer = 0
                Do While (i < values.Length)
                    values(i) = props(i).GetValue(item)
                    i = (i + 1)
                Loop

                dt.Rows.Add(values)
            Next
            Return dt
        End Function
        ''' <summary>
        ''' converts a list into Datatable 
        ''' </summary>
        ''' <typeparam name="T">IList<T> can store a collection of elements of any type T</typeparam>
        ''' <param name="data"></param>
        ''' <returns>dt as datatable</returns>
        Shared Function ListToDataTable(Of T)(ByVal data As IList(Of T)) As DataTable
            Dim props As PropertyDescriptorCollection = TypeDescriptor.GetProperties(GetType(T))
            Dim dt As DataTable = New DataTable
            Dim i As Integer = 0
            Do While (i < props.Count)
                Dim prop As PropertyDescriptor = props(i)
                dt.Columns.Add(prop.Name, prop.PropertyType)
                i = (i + 1)
            Loop

            Dim values() As Object = New Object((props.Count) - 1) {}
            For Each item As T In data
                ' Dim i As Integer = 0
                Do While (i < values.Length)
                    values(i) = props(i).GetValue(item)
                    i = (i + 1)
                Loop

                dt.Rows.Add(values)
            Next
            Return dt
        End Function
        Function jsoncall() As String
            Dim obj = New With {
        Key .customer_name = "Manzar",
        Key .user_name = "mqureshi",
        Key .password = "E5capade!"
    }

            Dim webclient As New System.Net.WebClient()
            Dim callbackurl As String = String.Empty

            Dim httpWebRequest As HttpWebRequest = DirectCast(WebRequest.Create(callbackurl), HttpWebRequest)
            Dim jsz As New System.Web.Script.Serialization.JavaScriptSerializer()

            Dim postData As String = jsz.Serialize(obj)
            Dim data As Byte() = Encoding.ASCII.GetBytes(postData)

            httpWebRequest.Method = "POST"
            httpWebRequest.ContentType = "application/json"
            httpWebRequest.ContentLength = data.Length

            Using stream As System.IO.Stream = httpWebRequest.GetRequestStream()
                stream.Write(data, 0, data.Length)
            End Using

            Dim res As WebResponse = httpWebRequest.GetResponse()
            Dim sr = New StreamReader(res.GetResponseStream(), System.Text.Encoding.Default)
            Dim str As String = sr.ReadToEnd()
            Return str
        End Function
        Shared Function JsonCallAutentication(URI As String, Optional username As String = "mqureshi", Optional password As String = "E5capade!") As String
            Dim obj = New With {
        Key .username = username,
        Key .password = password
    }

            Dim webclient As New System.Net.WebClient()
            Dim callbackurl As String = String.Empty

            Dim httpWebRequest As HttpWebRequest = DirectCast(WebRequest.Create(URI), HttpWebRequest)
            ' Dim request As HttpWebRequest = httpWebRequest.Create(url)
            Dim jsz As New System.Web.Script.Serialization.JavaScriptSerializer()

            Dim postData As String = jsz.Serialize(obj)
            Dim data As Byte() = Encoding.ASCII.GetBytes(postData)

            httpWebRequest.Method = "POST"
            httpWebRequest.ContentType = "application/json"
            httpWebRequest.ContentLength = data.Length

            Using stream As System.IO.Stream = httpWebRequest.GetRequestStream()
                stream.Write(data, 0, data.Length)
            End Using

            Dim res As WebResponse = httpWebRequest.GetResponse()
            Dim sr = New StreamReader(res.GetResponseStream(), System.Text.Encoding.Default)
            Dim str As String = sr.ReadToEnd()
            Return str
        End Function
        Public ReadOnly objc As Object

        Shared Function JsonCallGet(URI As String, obj As Object, Optional RequestMethod As String = "GET", Optional token As String = "", Optional updatedAtFrom As String = "2017-09-19T13:33:00+00:00", Optional updatedAtTo As String = "2019-09-20T16:33:00+00:00", Optional debugit As Boolean = False, Optional DateRangeIncludedIncallfor As Boolean = False) As String
            Dim str As String = "Nothing"
            Dim res As WebResponse
            Dim stringWriter As StringWriter = New StringWriter()
            Dim obj1 = New With {
                                   Key .updatedAtFrom = updatedAtFrom,
                                   Key .updatedAtTo = updatedAtTo
                               }
            'obj = obj1
            ' URI += "&updatedAtFrom=" & MiscClass.GetUrlEncode(updatedAtFrom) '& "&updatedAtTo=" & MiscClass.GetUrlEncode(updatedAtTo)
            If updatedAtTo = "2019-09-20T16:33:00+00:00" Then updatedAtTo = DateClass.GetW3CFormatFromDateTime(Now())
            If DateRangeIncludedIncallfor Then URI += "&updatedAtFrom=" & MiscClass.GetUrlEncode(updatedAtFrom) '& "&updatedAtTo=" & MiscClass.GetUrlEncode(updatedAtTo)
            HttpContext.Current.Response.Write("<hr>JsonCallGet." & "URI=" & URI & "<br>JsonCallGet.updatedAtFrom=" & MiscClass.GetUrlEncode(updatedAtFrom) & "updatedAtTo=" & MiscClass.GetUrlEncode(updatedAtTo) & ", obj1.updatedAtFrom = " & obj1.updatedAtFrom.ToString & ", obj1.updatedAtTo = " & obj1.updatedAtTo.ToString)
            Try
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

                Dim webclient As New System.Net.WebClient()
                Dim callbackurl As String = String.Empty


                Dim httpWebRequest As HttpWebRequest = DirectCast(WebRequest.Create(URI), HttpWebRequest)
                ' Dim request As HttpWebRequest = httpWebRequest.Create(url)


                httpWebRequest.Headers.Add("Authorization", "Bearer " & token)
                httpWebRequest.Method = RequestMethod
                httpWebRequest.ContentType = "application/json"
                If RequestMethod = "POST" Then
                    Dim jsz As New System.Web.Script.Serialization.JavaScriptSerializer()
                    Dim postData As String = jsz.Serialize(obj)
                    If debugit = True Then HttpContext.Current.Response.Write("<br>postData=" & obj.ToString)
                    Dim data As Byte() = Encoding.ASCII.GetBytes(postData)
                    httpWebRequest.ContentLength = data.Length
                    Using stream As System.IO.Stream = httpWebRequest.GetRequestStream()
                        stream.Write(data, 0, data.Length)
                    End Using
                End If


                res = httpWebRequest.GetResponse()
                If debugit = True Then HttpContext.Current.Response.Write("<br>JSONClass.JsonCallGet.res=" & res.ToString)
                Dim sr = New StreamReader(res.GetResponseStream(), System.Text.Encoding.Default)
                str = sr.ReadToEnd()
                If debugit = True Then HttpContext.Current.Response.Write("<br>JSONClass.JsonCallGet.sr=" & sr.ToString & "<br>debugit=" & debugit & "<br>URI=" & URI & "<br>str=" & str) : HttpContext.Current.Response.End()
            Catch ex As Exception
                ' Console.WriteLine("Status Code : {0}", CType(Res, HttpWebResponse).StatusCode)
                'Console.SetOut(stringWriter)
                Dim err As String = stringWriter.ToString
                str = "<font color='red'>" & (ex.Message & "<br>" & err & "</font>")
            End Try

            Return str
        End Function



        Function GetHeaders(keys As String, ByVal webHeaderCollection As WebHeaderCollection) As KeyValuePair(Of String, String)()
            'Dim keys As String() = webHeaderCollection.AllKeys
            Dim keyVals = New KeyValuePair(Of String, String)(keys.Length - 1) {}
            For i As Integer = 0 To keys.Length - 1
                keyVals(i) = New KeyValuePair(Of String, String)(keys(i), webHeaderCollection(keys(i)))
            Next

            Return keyVals
        End Function
        ''' <summary>
        ''' Pass in three parameters (url, type ["Get", "Post", etc.. ], withCredentials [true/false])
        ''' </summary>
        ''' <param name="url"></param>
        ''' <param name="type"></param>
        ''' <param name="withCredentials"></param>
        ''' <returns>Returns a deserialized json object that can be looped (similar to jQuery $.ajax)</returns>
        ''' previously it was named as Ajax
        Shared Function JasonRequestResponse(url As String, type As String, withCredentials As Boolean, len As Byte, Optional ContentType As String = "application/json", Optional token As String = "") As Object

            'create the request
            Dim request As HttpWebRequest = HttpWebRequest.Create(url)
            If withCredentials = True Then
                request.Credentials = CredentialCache.DefaultCredentials
            End If
            request.Method = type
            If token <> "" Then
                Dim WHC As WebHeaderCollection = request.Headers
                WHC.Add("Authorization", token)
            End If
            request.ContentLength = len
            request.ContentType = ContentType

            'create the response
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)

            Dim sr As New StreamReader(response.GetResponseStream())
            Dim jsonResponse As String = ""
            jsonResponse = sr.ReadToEnd()

            'deserialize and convert the plain json string into an object we can work with
            Dim deserializer As New JavaScriptSerializer()
            Dim objDeserialized As Object = deserializer.DeserializeObject(jsonResponse)

            'return the generic json object
            Return objDeserialized
        End Function
        Public Shared Function GetJSONFromDatasetWithTableName(ByVal ds As DataSet) As String
            Dim rv As String = JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented)
            Return rv
        End Function
    End Class




    ''' <summary>
    ''' JSON Serialization and Deserialization Assistant ClassS
    ''' </summary>
    Public Class JsonHelper
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse


        ''' <summary>
        ''' JSON Serialization
        ''' </summary>
        Public Shared Function JsonSerializer(Of T)(ByVal obj As T) As String
            Dim ser As New DataContractJsonSerializer(GetType(T))
            Dim ms As New MemoryStream()
            ser.WriteObject(ms, obj)
            Dim jsonString As String = Encoding.UTF8.GetString(ms.ToArray())
            ms.Close()
            'Replace Json Date String                                         
            Dim p As String = "\\/Date\((\d+)\+\d+\)\\/"
            Dim matchEvaluator As New MatchEvaluator(AddressOf ConvertJsonDateToDateString)
            Dim reg As New Regex(p)
            jsonString = reg.Replace(jsonString, matchEvaluator)
            Return jsonString
        End Function

        ''' <summary>
        ''' JSON Deserialization
        ''' </summary>
        Public Shared Function JsonDeserialize(Of T)(ByVal jsonString As String) As T
            'Convert "yyyy-MM-dd HH:mm:ss" String as "\/Date(1319266795390+0800)\/"
            Dim p As String = "\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}"
            Dim matchEvaluator As New MatchEvaluator(AddressOf ConvertDateStringToJsonDate)
            Dim reg As New Regex(p)
            jsonString = reg.Replace(jsonString, matchEvaluator)
            Dim ser As New DataContractJsonSerializer(GetType(T))
            Dim ms As New MemoryStream(Encoding.UTF8.GetBytes(jsonString))
            Dim obj As T = DirectCast(ser.ReadObject(ms), T)
            Return obj
        End Function

        Public Shared Function Deserialize(ByVal jsonResponse As String) As Object
            Dim deserializer As New JavaScriptSerializer()
            Dim objDeserialized As Object = deserializer.DeserializeObject(jsonResponse)
            Return objDeserialized
        End Function

        ''' <summary>
        ''' Convert Serialization Time /Date(1319266795390+0800) as String
        ''' </summary>
        Private Shared Function ConvertJsonDateToDateString(ByVal m As Match) As String
            Dim result As String = String.Empty
            Dim dt As New DateTime(1970, 1, 1)
            dt = dt.AddMilliseconds(Long.Parse(m.Groups(1).Value))
            dt = dt.ToLocalTime()
            result = dt.ToString("yyyy-MM-dd HH:mm:ss")
            Return result
        End Function

        ''' <summary>
        ''' Convert Date String as Json Time
        ''' </summary>
        Private Shared Function ConvertDateStringToJsonDate(ByVal m As Match) As String
            Dim result As String = String.Empty
            Dim dt As DateTime = DateTime.Parse(m.Groups(0).Value)
            dt = dt.ToUniversalTime()
            Dim ts As TimeSpan = dt - DateTime.Parse("1970-01-01")
            result = String.Format("\/Date({0}+0800)\/", ts.TotalMilliseconds)
            Return result
        End Function
        ''' <summary>
        ''' returns json string deserializer data into Datatable as datatable
        ''' </summary>
        ''' <param name="jsonString"></param>
        ''' <returns></returns>
        Shared Function ConvertJSONToDataTable(ByVal jsonString As String) As DataTable
            Dim dt As DataTable = New DataTable()
            Dim jsonParts As String() = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{")
            Dim dtColumns As List(Of String) = New List(Of String)()
            For Each jp As String In jsonParts
                Dim propData As String() = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",")
                For Each rowData As String In propData
                    Try
                        Dim idx As Integer = rowData.IndexOf(":")
                        Dim n As String = rowData.Substring(0, idx - 1)
                        Dim v As String = rowData.Substring(idx + 1)
                        If Not dtColumns.Contains(n) Then
                            dtColumns.Add(n.Replace("""", ""))
                        End If
                    Catch ex As Exception
                        'Throw New Exception(String.Format("Error Parsing Column Name : {0}", rowData))

                        HttpContext.Current.Response.Write(ex.Message)
                        HttpContext.Current.Response.Write("<br>" & jsonString)
                    End Try
                Next
                Exit For
            Next
            For Each c As String In dtColumns
                dt.Columns.Add(c)
            Next
            For Each jp As String In jsonParts
                Dim propData As String() = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",")
                Dim nr As DataRow = dt.NewRow()
                For Each rowData As String In propData
                    Try
                        Dim idx As Integer = rowData.IndexOf(":")
                        Dim n As String = rowData.Substring(0, idx - 1).Replace("""", "")
                        Dim v As String = rowData.Substring(idx + 1).Replace("""", "")
                        nr(n) = v
                    Catch ex As Exception
                        Continue For
                    End Try
                Next
                dt.Rows.Add(nr)
            Next
            Return dt
        End Function
        ''' <summary>
        ''' Converts datatable into JSON Object
        ''' </summary>
        ''' <param name="dt">datatable</param>
        ''' <returns>returns string after co0nverting it into JSONString</returns>
        Public Function DataTableToJsonObj(ByVal dt As DataTable) As String
            Dim ds As DataSet = New DataSet()
            ds.Merge(dt)
            Dim JsonString As StringBuilder = New StringBuilder()
            If ds IsNot Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                JsonString.Append("[")
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    JsonString.Append("{")
                    For j As Integer = 0 To ds.Tables(0).Columns.Count - 1
                        If j < ds.Tables(0).Columns.Count - 1 Then
                            JsonString.Append("""" & ds.Tables(0).Columns(j).ColumnName.ToString() & """:" & """" + ds.Tables(0).Rows(i)(j).ToString() & """,")
                        ElseIf j = ds.Tables(0).Columns.Count - 1 Then
                            JsonString.Append("""" & ds.Tables(0).Columns(j).ColumnName.ToString() & """:" & """" + ds.Tables(0).Rows(i)(j).ToString() & """")
                        End If
                    Next

                    If i = ds.Tables(0).Rows.Count - 1 Then
                        JsonString.Append("}")
                    Else
                        JsonString.Append("},")
                    End If
                Next

                JsonString.Append("]")
                Return JsonString.ToString()
            Else
                Return Nothing
            End If
        End Function
        Shared Function ConvertJsonStringToDataSet(ByVal jsonString As String) As DataSet
            Try
                Dim xd As XmlDocument = New XmlDocument()
                jsonString = "{ ""rootNode"": {" & jsonString.Trim().TrimStart("{"c).TrimEnd("}"c) & "} }"
                xd = CType(JsonConvert.DeserializeXmlNode(jsonString), XmlDocument)
                Dim ds As DataSet = New DataSet()
                ds.ReadXml(New XmlNodeReader(xd))
                Return ds
            Catch ex As Exception
                HttpContext.Current.Response.Write(ex.Message)
                HttpContext.Current.Response.Write("<br>" & jsonString)
                HttpContext.Current.Response.End()
            End Try
        End Function

    End Class
    Class HTTPRequest(Of T As Class)

        ' We will need a callback function
        Public Delegate Sub CallbackHandler(ByVal sender As Object, ByVal t As T)
        Private callbackHandler2 As CallbackHandler

        ' The host
        Private ReadOnly host As String

        ' The method to use, i.e POST or GET
        Private ReadOnly method As String

        'As the response stream in this instance is always going to be JSON.
        Const type As String = "application/json"

        Public Sub New(ByVal h As String, ByVal m As String, ByVal callback As CallbackHandler)
            host = h
            method = m
            callbackHandler2 = callback
        End Sub

        Public Sub Post(ByVal bytes As Byte(), ByVal uri As String)
            'For HTTPS requests add this callback.
            ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf Accept)
            Dim len = bytes.Length

            'Create request... 
            Dim request = CType(WebRequest.Create(uri), HttpWebRequest)
            request.Proxy = Nothing
            request.Credentials = CredentialCache.DefaultCredentials
            request.ContentLength = len
            request.ContentType = type
            request.Accept = type
            request.Method = method
            request.Host = host
            Dim dataStream = request.GetRequestStream()
            dataStream.Write(bytes, 0, len)
            dataStream.Close()
            Try
                Dim t = Read(CType(request.GetResponse(), HttpWebResponse))
                'Pass the response back already encoded into any class
                callbackHandler2(Me, t)
            Catch __unusedException1__ As Exception
                callbackHandler2(Me, Nothing)
            End Try
        End Sub

        Private Function Read(ByVal resp As HttpWebResponse) As T
            Dim jsonSerializer As DataContractJsonSerializer = New DataContractJsonSerializer(GetType(T))
            Dim e As T
            Using memoryStream As MemoryStream = New MemoryStream()
                ' Read in the response
                e = CType(jsonSerializer.ReadObject(resp.GetResponseStream()), T)
                memoryStream.Close()
            End Using

            resp.Close()
            Return e
        End Function

        Private Function Accept(ByVal sender As Object, ByVal cert As X509Certificate, ByVal chn As X509Chain, ByVal ssl As SslPolicyErrors) As Boolean
            Return True
        End Function

    End Class

    Public Class WebRequestGet

        Public Shared Function Main(url As String) As String
            Dim Rv As String = "No response"
            Dim request As WebRequest = WebRequest.Create(url)
            request.Credentials = CredentialCache.DefaultCredentials
            request.Method = "GET"
            request.ContentType = "application/json"
            Dim response As WebResponse = request.GetResponse()
            Console.WriteLine((CType(response, HttpWebResponse)).StatusDescription)
            Dim dataStream As Stream = response.GetResponseStream()
            Dim reader As StreamReader = New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            'Console.WriteLine(responseFromServer)
            Rv = responseFromServer.ToString
            reader.Close()
            response.Close()
            Return Rv
        End Function
    End Class
    Public Class ConvertJsonStringToDataTable

        Shared Function JsonStringToDataTable(ByVal jsonString As String) As DataTable
            Dim dt As DataTable = New DataTable()
            Dim jsonStringArray As String() = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{")
            Dim ColumnsName As List(Of String) = New List(Of String)()
            ' create column names 
            For Each jSA As String In jsonStringArray
                Dim jsonStringData As String() = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",")
                For Each ColumnsNameData As String In jsonStringData
                    Try
                        Dim idx As Integer = ColumnsNameData.IndexOf(":")
                        Dim ColumnsNameString As String = ColumnsNameData.Substring(0, idx - 1).Replace("""", "")
                        If Not ColumnsName.Contains(ColumnsNameString) Then
                            ColumnsName.Add(ColumnsNameString)
                        End If
                    Catch ex As Exception
                        Throw New Exception(String.Format("Error Parsing Column Name : {0}", ColumnsNameData))
                    End Try
                Next

                Exit For
            Next
            ' Adds Column names to the data table
            For Each AddColumnName As String In ColumnsName
                dt.Columns.Add(AddColumnName)
            Next
            HttpContext.Current.Response.Write("JsonStringToDataTable.ln652.jsonStringArray=" & jsonStringArray.ToString)
            jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{")
            For Each jSA As String In jsonStringArray
                Dim RowData As String() = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",")
                Dim nr As DataRow = dt.NewRow()
                For Each Row As String In RowData
                    Try
                        Dim idx As Integer = Row.IndexOf(":")
                        Dim RowColumns As String = Row.Substring(0, idx - 1).Replace("""", "")
                        Dim RowDataString As String = Row.Substring(idx + 1).Replace("""", "")
                        nr(RowColumns) = RowDataString
                    Catch ex As Exception
                        Continue For
                    End Try
                Next

                dt.Rows.Add(nr)
            Next

            Return dt
        End Function
    End Class
    Public Class DataRowConverter
        '        Inherits JsonConverter

        Public Sub WriteJson(ByVal writer As JsonWriter, ByVal dataRow As Object)
            Dim row As DataRow = TryCast(dataRow, DataRow)
            Dim ser As JsonSerializer = New JsonSerializer()
            writer.WriteStartObject()
            For Each column As DataColumn In row.Table.Columns
                writer.WritePropertyName(column.ColumnName)
                ser.Serialize(writer, row(column))
            Next

            writer.WriteEndObject()
        End Sub

        Public Function CanConvert(ByVal valueType As Type) As Boolean
            Return GetType(DataRow).IsAssignableFrom(valueType)
        End Function

        Public Function ReadJson(ByVal reader As JsonReader, ByVal objectType As Type) As Object
            Throw New NotImplementedException()
        End Function
    End Class

    Public Class DataTableConverter
        '       Inherits JsonConverter

        Public Sub WriteJson(ByVal writer As JsonWriter, ByVal dataTable As Object)
            Dim table As DataTable = TryCast(dataTable, DataTable)
            Dim converter As DataRowConverter = New DataRowConverter()
            writer.WriteStartObject()
            writer.WritePropertyName("Rows")
            writer.WriteStartArray()
            For Each row As DataRow In table.Rows
                converter.WriteJson(writer, row)
            Next

            writer.WriteEndArray()
            writer.WriteEndObject()
        End Sub

        Public Function CanConvert(ByVal valueType As Type) As Boolean
            Return GetType(DataTable).IsAssignableFrom(valueType)
        End Function

        Public Function ReadJson(ByVal reader As JsonReader, ByVal objectType As Type) As Object
            Throw New NotImplementedException()
        End Function
    End Class

    Public Class DataSetConverter
        '       Inherits JsonConverter

        Public Sub WriteJson(ByVal writer As JsonWriter, ByVal mdataset As Object)
            Dim dataSet As DataSet = TryCast(mdataset, DataSet)
            Dim converter As DataTableConverter = New DataTableConverter()
            writer.WriteStartObject()
            writer.WritePropertyName("Tables")
            writer.WriteStartArray()
            For Each table As DataTable In dataSet.Tables
                converter.WriteJson(writer, table)
            Next

            writer.WriteEndArray()
            writer.WriteEndObject()
        End Sub

        Public Function CanConvert(ByVal valueType As Type) As Boolean
            Return GetType(DataSet).IsAssignableFrom(valueType)
        End Function

        Public Function ReadJson(ByVal reader As JsonReader, ByVal objectType As Type) As Object
            Throw New NotImplementedException()
        End Function
    End Class

    Class JsonHelperClass
        Public Function CheckAndFixJson(ByVal jsonData As String) As String
            ' Attempt to parse the JSON data
            Dim errorMessage As String = ""
            Try
                Dim parsedData = JsonConvert.DeserializeObject(jsonData)
                Return jsonData ' No issues found, return original data
                'Catch ex As JsonReaderException
                'Dim errorMessage As String = ex.Message

                ' Fix common issues:
                jsonData = jsonData.Replace("\r\n", "") ' Remove newlines
                jsonData = jsonData.Replace("\t", "") ' Remove tabs
                jsonData = jsonData.Replace("\\", "") ' Remove escaped backslashes
                jsonData = jsonData.Trim() ' Remove leading/trailing whitespace

                ' Try parsing again after fixing
                'Try
                parsedData = JsonConvert.DeserializeObject(jsonData)
                Return jsonData ' Fix successful, return fixed data
            Catch ex As Exception ' Catch any other exceptions during parsing
                errorMessage = String.Concat(errorMessage, vbCrLf, "Fix unsuccessful: ", ex.Message)
                Throw New Exception(errorMessage) ' Re-throw with combined error message
                'End Try
            End Try
        End Function
        Public Function ReadJsonFile(filePath As String) As JObject
            Dim jsonData As String = File.ReadAllText(filePath)
            Dim jsonObject As JObject = JObject.Parse(jsonData)
            Return jsonObject
        End Function

        ''' <summary>
        ''' this functon write the data structure of Json File
        ''' </summary>
        ''' <param name="jsonObject">Json File as object</param>
        ''' <param name="filePath">File Path of the output file</param>
        Public Sub WriteJsonFile(jsonObject As JObject, filePath As String)
            Dim jsonData As String = JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented)
            File.WriteAllText(filePath, jsonData)
        End Sub


    End Class

End Namespace
