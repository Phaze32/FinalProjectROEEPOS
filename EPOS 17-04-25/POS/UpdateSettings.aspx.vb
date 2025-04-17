Imports System.Data
Imports System.Reflection
Imports System.Web.Script.Serialization
Partial Class POS_UpdateSettings
    Inherits System.Web.UI.Page

    Private Sub POS_UpdateSettings_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Dim quantity As Integer = 0
        Dim ticket_id As String = ""
        Dim pluid As String = ""
        Dim storid As String = "64"
        Dim site_name As String = ""
        Dim Qs As String = Request.QueryString().ToString
        If Request.QueryString("site_name") <> "" Then site_name = Request.QueryString("site_name")
        site_name = MiscClass.GetUrlEncode(site_name)
        If Request.QueryString("pin_code") <> "" Then quantity = Request.QueryString("pin_code")
        If Request.QueryString("ticket_id") <> "" Then ticket_id = Request.QueryString("ticket_id")
        'Dim result As String = updateTicketQUantity(ticket_id, pluid, quantity)
        Dim basket_Html As String = ("pin_code=" & quantity & "site_name=" & site_name) 'buildbasket(quantity, pluid)

        Dim strsql As String = "update [DST].[dbo].[tblConfiguration] set Value = '" & site_name & "' where name='site_name' and [StoreId] = '" & storid & "' ;"
        Dim resultsql As String = SQLFunctions.RunSQLStringWOConstring(strsql)
        literal1.Text = basket_Html & "test" & " strsql=" & strsql & ", resultsql" & resultsql 'If(result = "SQLstring Executed", "00", "404") ' & ", sqlstr=<br>" & result
        Response.Write(Qs & "<br>")
        Dim qscount As Int16 = Request.QueryString.Count
        For i = 1 To qscount
            If i >= 0 And i < qscount Then Response.Write((i) & Request.QueryString(i) & "<BR>")
        Next
        Dim D As Dictionary(Of String, String) = DictionaryClass.QueryStringToDictionary(Qs)
        'Dim dtqs As DataTable = DictonarysToDataTable(QueryStringToDictionary(Qs))
        Dim dt1 As DataTable = DictionaryClass.DictonaryToDataTable(D)
        dt1 = DTClass.AddIndexToDataTable(dt1)
        displayDictionaryForTest()
        GridView1.DataSource = dt1
        GridView1.DataBind()

        Dim v1 As String = DTClass.GetDTColumnNames(dt1)
        Dim rv As String = TextCSVClass.GetElementFromArray(v1,, 4)
        Dim itemnumber As String = TextCSVClass.GetitemCountInArray(v1)
        literal1.Text += "<br>" & v1 & DTClass.SearchDT(dt1, "prod_disp_limit", "SNo", "1") & " RV:" & rv & "<br>" _
            & itemnumber & ProcessCSVFile(v1)
    End Sub
    Public Function qstocsvstring(querystring As String) As String
        Dim Rv As String
        Rv = Replace(querystring, "?", "")
        'Dim ls As List(Of String) = querystring.Split(","c).ToList()
        Return Rv
    End Function

    Public Sub WriteToConsole(ByVal items As IEnumerable)
        For Each o As Object In items
            Response.Write(o)
        Next
    End Sub
    Public Sub displayDictionaryForTest() 'As Dictionary(Of String, String)
        Dim AuthorList As Dictionary(Of String, String) = New Dictionary(Of String, String)()
        'Dim AuthorList As Dictionary(Of String, Int16) = New Dictionary(Of String, Int16)()
        AuthorList.Add("Mahesh Chand", 35)
        AuthorList.Add("Mike Gold", 25)
        AuthorList.Add("Praveen Kumar", 29)
        AuthorList.Add("Raj Beniwal", 21)
        AuthorList.Add("Dinesh Beniwal", 84)
        Console.WriteLine("Authors List")
        'For Each author As KeyValuePair(Of String, Int16) In AuthorList
        For Each author As KeyValuePair(Of String, String) In AuthorList
            Response.Write(author.Key & ":" & author.Value & "<br>")
        Next
        'Return AuthorList
        Dim dt As DataTable = DictionaryClass.DictonaryToDataTable(AuthorList)
        'literal1.Text=TextCSVClass.DataTable2CSV(dt,)
        For Each row In dt.Rows
            Response.Write(row.ToString)
        Next
    End Sub
    Shared Function ProcessCSVFile(arry As String) As String
        Dim rv As String = "<br>ArrayElements:"
        Dim wordcount As Integer = 0
        wordcount = TextCSVClass.GetitemCountInArray(arry)
        Dim arrayvalue As String = ""
        Dim arrayKey As String = ""
        For i As Integer = 0 To wordcount
            arrayKey = TextCSVClass.GetElementFromArray(arry,, i)
            arrayvalue =
            rv = arrayKey
        Next

        'rv = wordcount.ToString
        Return rv
    End Function
    Function GetArrayItemValue(dict As Dictionary(Of String, String), key As String) As String
        If dict.ContainsKey(key) Then
            Return dict(key)
        Else
            Return "Key not found"
        End If
    End Function
    Function CreateDictionary() As Dictionary(Of String, String)
        Dim dict As New Dictionary(Of String, String)()
        dict.Add("Name", "John Doe")
        dict.Add("Age", "30");
        dict.Add("Occupation", "Developer")
        Return dict
    End Function
End Class

