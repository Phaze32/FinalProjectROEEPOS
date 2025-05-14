Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Reflection
Public Class DictionaryClass
    Public Sub WriteToConsole(ByVal items As IEnumerable)
        For Each o As Object In items
            Console.WriteLine(o)
        Next
    End Sub

    Public Function CreateDictionaryForTest() As Dictionary(Of String, String)
        Dim AuthorList As Dictionary(Of String, String) = New Dictionary(Of String, String)()
        'Dim AuthorList As Dictionary(Of String, Int16) = New Dictionary(Of String, Int16)()
        AuthorList.Add("Mahesh Chand", "35")
        AuthorList.Add("Mike Gold", "25")
        AuthorList.Add("Praveen Kumar", "29")
        AuthorList.Add("Raj Beniwal", "21")
        AuthorList.Add("Dinesh Beniwal", "84")
        Console.WriteLine("Authors List")
        'For Each author As KeyValuePair(Of String, Int16) In AuthorList
        For Each author As KeyValuePair(Of String, String) In AuthorList
            Console.WriteLine("Key: {0}, Value: {1}", author.Key, author.Value)
        Next
        Return AuthorList
    End Function
    Shared Function DictonaryToDataTable(dict As Dictionary(Of String, String)) As DataTable
        Dim table As DataTable = New DataTable()

        'For Each dict As Dictionary(Of String, String) In List

        For Each entry As KeyValuePair(Of String, String) In dict

            If Not table.Columns.Contains(entry.Key.ToString()) Then
                table.Columns.Add(entry.Key)
            End If
        Next

        table.Rows.Add(dict.Values.ToArray())
        'Next
        Return table
    End Function
    Public Shared Function QueryStringToDictionaryV2(ByVal queryString As String) As Dictionary(Of String, String)
        Dim nvc = HttpUtility.ParseQueryString(queryString)
        Return nvc.AllKeys.ToDictionary(Function(k) k, Function(k) nvc(k))
    End Function
    Public Shared Function DictionaryToQueryString(ByVal parameters As Dictionary(Of String, String)) As String
        Return String.Join("&", parameters.[Select](Function(kvp) String.Format("{0}={1}", kvp.Key, HttpUtility.UrlEncode(kvp.Value))))
    End Function
    Public Shared Function ListToDT(Of T)(list As IList(Of T)) As DataTable
        Dim table As New DataTable()
        Dim row As DataRow = table.NewRow()
        For i As Integer = 0 To list.Count - 1
            For Each item As T In list
                For Each field As FieldInfo In table.Rows
                    row(1) = field.GetValue(item)
                Next
                table.Rows.Add(row)
            Next
        Next
        Return table
    End Function
    Private Function DictonaryListToDataTable(ByVal list As List(Of Dictionary(Of String, String))) As DataTable
        Dim table As DataTable = New DataTable()

        For Each dict As Dictionary(Of String, String) In list

            For Each entry As KeyValuePair(Of String, String) In dict

                If Not table.Columns.Contains(entry.Key.ToString()) Then
                    table.Columns.Add(entry.Key)
                End If
            Next

            table.Rows.Add(dict.Values.ToArray())
        Next

        Return table
    End Function
    Public Shared Function QueryStringToDictionary(ByVal query As String) As Dictionary(Of String, String)
        Dim queryDict As Dictionary(Of String, String) = New Dictionary(Of String, String)()

        For Each token As String In query.TrimStart(New Char() {"?"c}).Split(New Char() {"&"c}, StringSplitOptions.RemoveEmptyEntries)
            Dim parts As String() = token.Split(New Char() {"="c}, StringSplitOptions.RemoveEmptyEntries)
            If parts.Length = 2 Then
                queryDict(parts(0).Trim()) = HttpUtility.UrlDecode(parts(1)).Trim()
            Else
                queryDict(parts(0).Trim()) = ""
            End If
        Next

        Return queryDict
    End Function
End Class
