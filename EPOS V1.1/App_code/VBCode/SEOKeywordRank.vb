Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Web
Imports System.Threading
Imports Microsoft.VisualBasic
Imports System.Web.HttpContext
Imports System.Web.HttpUtility
Public Enum eRequestType
    ePost = 1
    eGet = 2
End Enum

Public Class SEOKeywordRank
    Inherits System.Web.UI.Page

    Dim sRetHTML As String = ""
    'Protected WithEvents txtWordsToCheck As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtSiteToCheck As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblResults As System.Web.UI.HtmlControls.HtmlGenericControl
    'Protected WithEvents btnSubmit As System.Web.UI.WebControls.Button
    '#Region " Web Form Designer Generated Code "

    '    'This call is required by the Web Form Designer.
    '    <System.Diagnostics.DebuggerStepThrough()>
    '    Private Sub InitializeComponent()

    '    End Sub

    'Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
    '    'CODEGEN: This method call is required by the Web Form Designer
    '    'Do not modify it using the code editor.
    '    InitializeComponent()
    'End Sub

    '#End Region
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    'End Sub

    Shared Function sGetPostData(ByVal sRequestURL As String, ByVal RequestType As eRequestType) As String

        Dim Writer As StreamWriter = Nothing

        Dim WebRequestObject As HttpWebRequest
        Dim sr As StreamReader
        Dim WebResponseObject As HttpWebResponse
        Dim iTries As Int16
        Dim bOK As Boolean
        Dim Results As String
        Dim sbResultsBuilder As New StringBuilder()
        Dim sTemp As String
        Dim sBuffer(8192) As Char
        Dim iRetChars As Integer
        Dim sLASTCHARS As String

        bOK = False
        iTries = 0

        Do While bOK = False And iTries < 10
            Try
                WebRequestObject = CType(WebRequest.Create(sRequestURL), HttpWebRequest)
                WebRequestObject.ContentType = "application/x-www-form-urlencoded"
                WebRequestObject.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)"

                If RequestType = eRequestType.ePost Then
                    WebRequestObject.Method = "POST"
                    Writer = New StreamWriter(WebRequestObject.GetRequestStream())
                    Writer.Close()
                Else
                    WebRequestObject.Method = "GET"
                End If


                WebResponseObject = CType(WebRequestObject.GetResponse(), HttpWebResponse)
                sr = New StreamReader(WebResponseObject.GetResponseStream)

                Results = ""
                Do
                    iRetChars = sr.Read(sBuffer, 0, sBuffer.Length)
                    If iRetChars > 0 Then
                        sbResultsBuilder.Append(sBuffer, 0, iRetChars)
                        sTemp = sBuffer
                        If InStr(UCase(sTemp), "</HTML>") <> 0 Then
                            Exit Do
                        End If
                    End If
                Loop While iRetChars > 0
                Results = sbResultsBuilder.ToString

                'Results = sr.ReadToEnd
                sGetPostData = Results
                sr.Close()
                WebResponseObject.Close()

                If sGetPostData <> "" Then
                    bOK = True
                Else
                    iTries = iTries + 1
                End If
            Catch ex As Exception
                iTries = iTries + 1
            End Try
        Loop

    End Function

    ''' <summary>
    ''' Brings back rank of the website on google listing based on a specific search
    ''' </summary>
    ''' <param name="sSite">your website URl that needes to be checked for keywords</param>
    ''' <param name="sWordsToCheck">Keywords used for search on google</param>
    ''' <returns>single integer providing the rank of the website based on a specific search</returns>
    ''' <remarks></remarks>
    Shared Function GetKeyWordRanking(ByVal sSite As String, ByVal sWordsToCheck As String) As String
        Try
            Dim Returnvalue As String
            Dim sRetHTML As String
            Dim sPlaces As String
            sWordsToCheck = HttpUtility.UrlPathEncode(sWordsToCheck)

            If Left$(sSite, 7) <> "http://" Then
                sSite = "http://" & sSite
            End If
            'HttpContext.Current.Response.End()
            ''Get MSN result
            'sRetHTML = sGetPostData("http://search.msn.co.uk/results.aspx?first=1&ie=utf-8&oe=utf-8&count=100&q=" & sWordsToCheck, eRequestType.eGet)
            'sPlaces = sPlaces & "MSN: "
            'sPlaces = sPlaces & sFindPlace(sRetHTML, "<h2>results</h2>", "didn't get the results you expected", sSite, "<h3>")

            'Get Google result
            sRetHTML = sGetPostData("http://www.google.co.uk/search?hl=en&q=" & sWordsToCheck & "&num=100", eRequestType.eGet)
            'HttpContext.Current.Response.Write(sRetHTML & "</br>")
            'Literal1.Text = sRetHTML
            '           sPlaces = "Google: "
            sPlaces = sFindPlace(sRetHTML, "<div id=""search", "<div id=""foot", sSite, "<li class=""g")

            'Get Yahoo result
            'sRetHTML = sGetPostData("http://search.yahoo.co.uk/search?p=" & sWordsToCheck & "&n=100", eRequestType.ePost)
            'sPlaces = sPlaces & "Yahoo: "
            'sPlaces = sPlaces & sFindPlace(sRetHTML, "about this page", "<b>results page:</b>", sSite, "<a class=yschttl")
            'Label11.Text = "Results :" & sPlaces
            'lblResults.InnerHtml = sPlaces
            Returnvalue = sPlaces
            sPlaces = ""
            sRetHTML = ""
            Return Returnvalue
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message.ToString)
            HttpContext.Current.Response.Write("sSite=" & sSite & "</br>")
            HttpContext.Current.Response.Write("sWordsToCheck=" & sWordsToCheck & "</br>")
        End Try
    End Function
    Shared Function GetKeyWordRankingList(ByVal sWordsToCheck As String) As String
        Try
            Dim Returnvalue As String
            Dim sRetHTML As String
            Dim sPlaces As String
            sWordsToCheck = HttpUtility.UrlPathEncode(sWordsToCheck)

            'Get Google result
            sRetHTML = sGetPostData("http://www.google.co.uk/search?hl=en&q=" & sWordsToCheck & "&num=100", eRequestType.eGet)
            'HttpContext.Current.Response.Write(sRetHTML & "</br>")

            Returnvalue = sRetHTML
            sRetHTML = ""
            Return Returnvalue
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message.ToString)
            HttpContext.Current.Response.Write("sWordsToCheck=" & sWordsToCheck & "</br>")
        End Try
    End Function
    Shared Function GetKeySERank(ByVal sSite As String, ByVal sWordsToCheck As String) As String
        Try
            Dim Returnvalue As String
            Dim sRetHTML As String
            Dim sPlaces As String
            sWordsToCheck = HttpUtility.UrlPathEncode(sWordsToCheck)

            If Left$(sSite, 7) <> "http://" Then
                sSite = "http://" & sSite
            End If
            'HttpContext.Current.Response.End()
            ''Get MSN result
            'sRetHTML = sGetPostData("http://search.msn.co.uk/results.aspx?first=1&ie=utf-8&oe=utf-8&count=100&q=" & sWordsToCheck, eRequestType.eGet)
            'sPlaces = sPlaces & "MSN: "
            'sPlaces = sPlaces & sFindPlace(sRetHTML, "<h2>results</h2>", "didn't get the results you expected", sSite, "<h3>")

            'Get Google result
            sRetHTML = sGetPostData("http://www.google.co.uk/search?hl=en&q=" & sWordsToCheck & "&num=100", eRequestType.eGet)
            'HttpContext.Current.Response.Write(sRetHTML & "</br>")
            'Literal1.Text = sRetHTML
            sPlaces = sPlaces & sFindPlace(sRetHTML, "<div id=""search", "<div id=""foot", sSite, "<li class=""g")

            'Get Yahoo result
            'sRetHTML = sGetPostData("http://search.yahoo.co.uk/search?p=" & sWordsToCheck & "&n=100", eRequestType.ePost)
            'sPlaces = sPlaces & "Yahoo: "
            'sPlaces = sPlaces & sFindPlace(sRetHTML, "about this page", "<b>results page:</b>", sSite, "<a class=yschttl")
            'Label11.Text = "Results :" & sPlaces
            'lblResults.InnerHtml = sPlaces
            Returnvalue = sPlaces
            sPlaces = ""
            Return Returnvalue
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message.ToString)
            HttpContext.Current.Response.Write("sSite=" & sSite & "</br>")
            HttpContext.Current.Response.Write("sWordsToCheck=" & sWordsToCheck & "</br>")
        End Try
    End Function
    Shared Function sFindPlace(ByVal sInput As String, ByVal sStart As String, ByVal sEnd As String, ByVal sSite As String, ByVal sSeparator As String) As String
        ' HttpContext.Current.Response.Write("values fed:sInput" & sInput & "-" & sStart & "-" & sEnd & "-" & sSite & "-" & sSeparator)
        'HttpContext.Current.Response.Write("values fed:sStart" & sSite & "sSeparator: #" & sSeparator & "#<br>")
        'HttpContext.Current.Response.End()
        Try
            Dim sPlace As String = ""

            If sInput = "" Then
                sFindPlace = "Not responding<br>"
                Exit Function
            End If

            sInput = LCase(sInput)
            sInput = sInput.Replace("www.", "")

            sInput = Mid$(sInput, InStr(sInput, sStart))
            sInput = Left$(sInput, InStr(sInput, sEnd))

            sSite = sSite.Replace("www.", "")

            If InStr(sInput, sSite) = 0 Then
                If InStr(sInput, HttpUtility.UrlPathEncode(sSite)) = 0 Then
                    sFindPlace = "Not found<br>"
                    Exit Function
                Else
                    sSite = HttpUtility.UrlPathEncode(sSite)
                End If
            End If

            sInput = Left$(sInput, InStr(sInput, sSite))

            sInput = sInput.Replace(sSeparator, Chr(31))
            sFindPlace = CStr(sInput.Split(Chr(31)).GetUpperBound(0)) & "<br>"

        Catch e As Exception
            e.Message.ToString()
        End Try
    End Function
End Class
