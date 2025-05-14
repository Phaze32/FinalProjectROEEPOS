Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Web.HttpContext

Public Class ExelDB_Functions
    'Sub MakeTableExcel()

    '    Dim adpData As OleDbDataAdapter
    '    Dim dsLineItems As DataSet
    '    Dim MyTable As DataTable
    '    Dim strSQL As String
    '    Dim loop2, fieldbumbers As Integer
    '    Dim loop1, numrows As Integer
    '    Dim connectionExcell As OleDbConnection
    '    Dim mPath As String = Session("DBPath")
    '    Dim filename As String = Session("sfilename")

    '    If filename = "" Then
    '        Label3.Text = "PLease Upload a new file before Processing"
    '        Exit Sub
    '    End If

    '    'Dim retunvalue = AccessDB_functions.GetAccessDb("CommsImportSale", "period= " & filename, "period")
    '    'Label3.Text = string(retunvalue)


    '    connectionExcell = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + mPath + "';Extended Properties=Excel 12.0;")

    '    strSQL = "select * from [Sheet1$]"


    '    ' Create a Command object with the SQL statement.
    '    adpData = New OleDbDataAdapter(strSQL, connectionExcell)

    '    ' Create a new dataset
    '    dsLineItems = New DataSet


    '    'Response.Write(mPath)
    '    'Response.End()

    '    ' Fill a DataSet with data returned from the Excel Sheet1.
    '    adpData.Fill(dsLineItems, "[Sheet1$]")

    '    ' Create a new DataTable object and assign to it
    '    ' the new table in the Tables collection.
    '    MyTable = New DataTable
    '    MyTable = dsLineItems.Tables(0)
    'End Sub
    'Public Shared Function GetAccessDb(ByVal fTableName As String, ByVal fSearchCriteria As String, ByVal fSeekFields As String) As AccessDataSource

    '    Dim mDbFolder As String = "/Uploads/"
    '    Dim MDatabseName As String = Current.Application("accessDB_name")

    '    Dim DbFilePAth As String = "~" & mDbFolder & MDatabseName
    '    'for single item search function enable the following line and disabe the string with mSearchCriteria
    '    'dim SqlString as string =  "SELECT "& mSeekFields & " FROM " & mTableName & " WHERE " & mSearchField & " = "  &mSearchValue

    '    Dim SqlString As String = "SELECT " & SafeSqlLiteral(fSeekFields, 2) & " FROM " & SafeSqlLiteral(fTableName, 2) & " WHERE " & SafeSqlLiteral(fSearchCriteria, 2)

    '    ' Create AccessDataSource
    '    Dim accessDS As New AccessDataSource(DbFilePAth, SqlString)

    '    Return accessDS
    'End Function

    'Public Shared Function SafeSqlLiteral(ByVal strValue As String, ByVal intLevel As Integer) As String

    '    ' intLevel represent how thorough the value will be checked for dangerous code
    '    ' intLevel (1) - Do just the basic. This level will already counter most of the SQL injection attacks
    '    ' intLevel (2) - &nbsp; (non breaking space) will be added to most words used in SQL queries to prevent unauthorized access to the database. Safe to be printed back into HTML code. Don't use for usernames or passwords

    '    If Not IsDBNull(strValue) Then
    '        If intLevel > 0 Then
    '            strValue = Replace(strValue, "'", "''") ' Most important one! This line alone can prevent most injection attacks
    '            strValue = Replace(strValue, "--", "")
    '            strValue = Replace(strValue, "[", "[[]")
    '            strValue = Replace(strValue, "%", "[%]")
    '        End If

    '        If intLevel > 1 Then
    '            Dim myArray As Array
    '            myArray = Split("xp_ ;update ;insert ;select ;drop ;alter ;create ;rename ;delete ;replace ", ";")
    '            Dim i, i2, intLenghtLeft As Integer
    '            For i = LBound(myArray) To UBound(myArray)
    '                Dim rx As New Regex(myArray(i), RegexOptions.Compiled Or RegexOptions.IgnoreCase)
    '                Dim matches As MatchCollection = rx.Matches(strValue)
    '                i2 = 0
    '                For Each match As Match In matches
    '                    Dim groups As GroupCollection = match.Groups
    '                    intLenghtLeft = groups.Item(0).Index + Len(myArray(i)) + i2
    '                    strValue = Left(strValue, intLenghtLeft - 1) & "&nbsp;" & Right(strValue, Len(strValue) - intLenghtLeft)
    '                    i2 += 5
    '                Next
    '            Next
    '        End If

    '        'strValue = replace(strValue, ";", ";&nbsp;")
    '        'strValue = replace(strValue, "_", "[_]")

    '        Return strValue
    '    Else
    '        Return strValue
    '    End If

    'End Function
End Class
