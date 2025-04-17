Imports Microsoft.VisualBasic
Imports System.Globalization
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Web.HttpContext
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Net

Public Class MiscClass_backup

    'Public Shared Function giveUserCount() As String
    '    Dim i As Integer = 0
    '    For Each i In Session.StaticObjects
    '        Response.Write(i & "<br>")
    '    Next
    'End Function

    Shared Function addpadding(ByRef oldstring As String, ByRef newstringlength As String, Optional ByRef paddingcharacted As String = "0") As String

        Dim returnvalue As String = ""
        Dim oldstringlength As Integer = Len(oldstring)
        HttpContext.Current.Response.Write("<br>ERR1: FN_addpadding.oldstring = " & oldstring & ";oldstringlength =" & oldstringlength)
        Try
            For i = 1 To (newstringlength - oldstringlength)
                returnvalue = returnvalue + paddingcharacted
            Next
            returnvalue = returnvalue + oldstring.ToString
            HttpContext.Current.Response.Write("<br>returnvalue=" & returnvalue & ";<br>")
            Return returnvalue
        Catch ex As Exception
            HttpContext.Current.Response.Write("<br>ERR: FN_addpadding = " & oldstringlength)
            HttpContext.Current.Response.Write("<br>oldstringlength = " & oldstringlength)
            HttpContext.Current.Response.Write("<br>oldstring = " & oldstring & "<br>")
            HttpContext.Current.Response.Write("paddingcharacted = " & paddingcharacted & "<br>")
            HttpContext.Current.Response.Write("newstringlength=" & newstringlength & ";<br>")
            HttpContext.Current.Response.Write("returnvalue=" & returnvalue & ";<br>")
        End Try
    End Function
    Private Shared Sub ChangeFileExtentionMove(ByVal OriginalFilesFolder As String, ByVal OriginalFilesExt As String, ByRef NewFileFolder As String, ByRef NewFileExt As String)

        Dim Original As String()
        Original = IO.Directory.GetFiles(OriginalFilesFolder, "*." & OriginalFilesExt)

        Dim newFilePath As String
        For Each filepath As String In Original
            newFilePath = filepath.Replace("*." & OriginalFilesExt, "*." & NewFileExt)
            System.IO.File.Move(filepath, newFilePath)
        Next

    End Sub
    Public Shared Function DaysNameFromDate(ByRef mdate As Date) As String
        Dim returnvalue As String = ""
        Dim thisCulture = Globalization.CultureInfo.CurrentCulture
        Dim dayOfWeek As DayOfWeek = thisCulture.Calendar.GetDayOfWeek(mdate)
        returnvalue = thisCulture.DateTimeFormat.GetDayName(dayOfWeek)

        Return returnvalue
    End Function
    Public Shared Function DatetoSQLC(ByRef mdate As Date) As String
        Dim returnvalue As String = ""
        returnvalue = "Convert(VARCHAR(11)," & mdate & ", 103)"
        Return returnvalue
    End Function
    Public Shared Function DaysFromFirstSunday(ByRef mdate As Date) As String
        Dim returnvalue As String = ""
        Dim DaysToMinus As Integer = 0
        Dim mMONTH As Integer = DatePart(DateInterval.Month, mdate)
        Dim myear As Integer = DatePart(DateInterval.Year, mdate)
        Dim datemade As String = ""
        datemade = (1 & "/" & mMONTH & "/" & myear)
        If "Sunday" = DaysNameFromDate(datemade) Then
            DaysToMinus = 7
        End If
        returnvalue = DateDiff("d", FirstSundayOfMonth(mdate), mdate) - DaysToMinus
        Return returnvalue
    End Function
    Public Shared Function LastYearMatchDayFromFirstSunday(ByRef mdate As Date) As String
        Dim returnvalue As String = ""
        Dim myear As Integer = DatePart(DateInterval.Year, mdate)
        Dim mMONTH As Integer = DatePart(DateInterval.Month, mdate)
        Dim x As Integer = DateDiff("d", FirstSundayOfMonth(mdate), mdate)
        Dim mdaysfromsunday As Integer = DaysFromFirstSunday(mdate)
        If mdaysfromsunday < 0 Then
            mdaysfromsunday = 0
        End If
        Dim mFirstSundayOfMonth As String = FirstSundayOfMonth("01/" & mMONTH & "/" & myear - 1)
        ' HttpContext.Current.Response.Write(mdaysfromsunday)
        returnvalue = DateAdd("D", mdaysfromsunday, mFirstSundayOfMonth)
        Return returnvalue
    End Function
    Public Shared Function DaysInMonthOfDate(ByRef mdate As Date) As String
        Dim returnvalue As String = ""
        Dim myear As Integer = DatePart(DateInterval.Year, mdate)
        Dim mMONTH As Integer = DatePart(DateInterval.Month, mdate)
        returnvalue = System.DateTime.DaysInMonth(myear, mMONTH)
        Return returnvalue
    End Function
    Public Shared Function LastDateOfMonthFromDate(ByRef mdate As Date) As String
        Dim returnvalue As String = ""
        Dim myear As Integer = DatePart(DateInterval.Year, mdate)
        Dim mMONTH As Integer = DatePart(DateInterval.Month, mdate)
        returnvalue = System.DateTime.DaysInMonth(myear, mMONTH) & "/" & mMONTH & "/" & myear
        Return returnvalue
    End Function
    Public Shared Function FirstDateOfMonthFromDate(ByRef mdate As Date) As String
        Dim returnvalue As String = ""
        Dim myear As Integer = DatePart(DateInterval.Year, mdate)
        Dim mMONTH As Integer = DatePart(DateInterval.Month, mdate)
        returnvalue = "01/" & mMONTH & "/" & myear
        Return returnvalue
    End Function
    Public Shared Function SameDatePreviousMonth(ByVal currentdate As String, Optional ByVal monthsback As String = "1") As String
        'HttpContext.Current.Response.Write("currentdate:" & currentdate & ",monthsback " & monthsback & " <br>")
        Dim returnvalue As String = ""
        Dim Sentdate As DateTime = DateTime.Parse(currentdate)
        Sentdate = Sentdate.AddMonths(-monthsback)
        returnvalue = Mid(Sentdate.ToString, 1, 10)
        Return returnvalue
    End Function
    Public Shared Function FirstSundayOfMonth(ByRef mdate As Date) As String
        Dim returnvalue As String = ""
        Dim maxdays As Integer = DaysInMonthOfDate(mdate)
        Dim myear As Integer = DatePart(DateInterval.Year, mdate)
        Dim mMONTH As Integer = DatePart(DateInterval.Month, mdate)
        Dim datemade As String = ""
        For x As Integer = 1 To maxdays
            datemade = (x & "/" & mMONTH & "/" & myear)
            If "Sunday" = DaysNameFromDate(datemade) Then
                returnvalue = datemade
                Exit For
            End If
        Next
        Return returnvalue
    End Function
    Public Shared Function GetDaysInMonth(myear As Integer, mmonth As Integer) As Integer
        Dim mnoofdays As Integer = 0
        mnoofdays = DateTime.DaysInMonth(myear, mmonth)
        Return mnoofdays
    End Function

    Public Shared Function GetMonthNameFromNum(ByVal mmonth As String) As String
        Dim returnvalue As String = "XX"
        mmonth = Val(MyReplace(MyReplace(mmonth, "/", ""), "-", ""))
        'HttpContext.Current.Response.Write("mmonthVale:" & mmonth & "<br>")
        Select Case mmonth
            Case 1
                returnvalue = "Jan"
            Case 2
                returnvalue = "Feb"
            Case 3
                returnvalue = "Mar"
            Case 4
                returnvalue = "Apr"
            Case 5
                returnvalue = "May"
            Case 6
                returnvalue = "Jun"
            Case 7
                returnvalue = "Jul"
            Case 8
                returnvalue = "Aug"
            Case 9
                returnvalue = "Sep"
            Case 10
                returnvalue = "Oct"
            Case 11
                returnvalue = "Nov"
            Case 12
                returnvalue = "Dec"
            Case Else
                returnvalue = "Jan"
        End Select
        'HttpContext.Current.Response.Write(returnvalue & "<br>")
        Return returnvalue
    End Function
    Public Shared Function GetMonthNumFromName(ByVal mmonth As String) As String
        Dim returnvalue As String = "1"
        'mmonth = Val(MyReplace(MyReplace(mmonth, "/", ""), "-", ""))
        'HttpContext.Current.Response.Write("mmonthVale:" & mmonth & "<br>")
        'HttpContext.Current.Response.End()
        Select Case mmonth
            Case mmonth = "Jan" Or mmonth = "january"
                returnvalue = "1"
            Case mmonth = "Feb" Or mmonth = "February"
                returnvalue = "2"
            Case mmonth = "Mar" Or mmonth = "March"
                returnvalue = "3"
            Case mmonth = "Apr" Or mmonth = "April"
                returnvalue = "4"
            Case mmonth = "May" Or mmonth = "May"
                returnvalue = "5"
            Case mmonth = "Jun" Or mmonth = "June"
                returnvalue = "6"
            Case mmonth = "Jul" Or mmonth = "July"
                returnvalue = "7"
            Case mmonth = "Aug" Or mmonth = "August"
                returnvalue = "8"
            Case mmonth = "Sep" Or mmonth = "September"
                returnvalue = "9"
            Case mmonth = "Oct" Or mmonth = "october"
                returnvalue = "10"
            Case mmonth = "Nov" Or mmonth = "November"
                returnvalue = "11"
            Case mmonth = "Dec" Or mmonth = "December"
                returnvalue = "12"
            Case Else
                returnvalue = "0"
        End Select
        'HttpContext.Current.Response.Write(returnvalue & "<br>")
        'HttpContext.Current.Response.End()
        Return returnvalue
    End Function

    Public Shared Function myCurrencyFormat(ByVal fAmount As Double, ByVal fZerosAfterDecimal As String) As String
        'the function converts intiger amount into comma groups of three and you can also derfine the number of zeros after decimal
        Dim ReturnStringNumber As String
        Dim nfi As NumberFormatInfo = New CultureInfo("en-US", False).NumberFormat
        nfi.NumberGroupSeparator = ","
        nfi.NumberDecimalDigits = fZerosAfterDecimal
        ReturnStringNumber = fAmount.ToString("N", nfi)
        Return ReturnStringNumber
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="fAmount"></param>
    ''' <param name="fZerosAfterDecimal"></param>
    ''' <param name="symbol">EUR=€,GBP=£,USD=$,OTH</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function myCurrencyFormatWithSymbol(ByVal fAmount As Double, ByVal fZerosAfterDecimal As String, ByVal symbol As String) As String
        'the function converts intiger amount into comma groups of three and you can also derfine the number of zeros after decimal
        Dim ReturnStringNumber As String
        Dim insertsymbol As String = ""
        Select Case symbol
            Case Is = "EUR"
                insertsymbol = " €"
            Case Is = "EURO"
                insertsymbol = " €"
            Case Is = "GBP"
                insertsymbol = " £"
            Case Is = "POUND"
                insertsymbol = " £"
            Case Is = "UKP"
                insertsymbol = " £"
            Case Is = "USD"
                insertsymbol = " $"
            Case Is = "DOLLAR"
                insertsymbol = " $"
            Case Is = "USDOLLAR"
                insertsymbol = " $"
            Case Is = "OTH"
                insertsymbol = " "
            Case Is = "PC"
                insertsymbol = " %"
            Case Else
                insertsymbol = " "
        End Select

        Dim nfi As NumberFormatInfo = New CultureInfo("en-US", False).NumberFormat
        nfi.NumberGroupSeparator = ","
        nfi.NumberDecimalDigits = fZerosAfterDecimal
        'ReturnStringNumber = fAmount.ToString("N", nfi)
        ReturnStringNumber = insertsymbol & fAmount.ToString("N", nfi)
        Return ReturnStringNumber
    End Function

    Public Shared Function RoundToNearest(Amount As Double, RoundTo As Double) As Double
        Dim ExcessAmount As Double = Amount Mod RoundTo
        If ExcessAmount < (RoundTo / 2) Then
            Amount -= ExcessAmount
        Else
            Amount += (RoundTo - ExcessAmount)
        End If
        Return Amount
    End Function

    Public Shared Function RoundToNext(Amount As Double, RoundTo As Double) As Double
        Dim ExcessAmount As Double = Amount Mod RoundTo
        Amount += (RoundTo - ExcessAmount)
        Return Amount
    End Function
    Public Shared Function MyReplace(ByVal ValueIn As String, ByVal WhatToReplace As String, ByVal ReplaceValue As String) As String
        Dim Temp As String
        Dim P As Long
        Dim fReplace As String = ""

        Temp = ValueIn
        P = InStr(Temp, WhatToReplace)
        Do While P > 0
            Temp = Left(Temp, P - 1) & ReplaceValue & Mid(Temp, P + Len(WhatToReplace))
            P = InStr(P + Len(ReplaceValue), Temp, WhatToReplace, 1)
        Loop
        fReplace = Temp
        Return fReplace
    End Function
    Public Shared Function PeriodToDate(ByVal mPeriod As String) As String
        Dim TheDateReturn As String = ""
        TheDateReturn = MonthName(Mid(mPeriod, 4, 5)) & " 20" & Mid(mPeriod, 1, 2)
        Return TheDateReturn
    End Function

    Public Shared Sub savetextfile(ByRef mFILEpath As String, ByRef mfilenane As String, ByVal mfilecontent As String)
        Dim FileContent As String = mfilecontent
        Dim saved As Boolean = False
        Dim mfileNamePostfix As Integer = 0
        Dim fileExtension As String = ".csv"
        Dim FILEpath As String = mFILEpath
        Dim path As String = ""

        Try
            'Open a file for writing
            Do While Not saved

                If mfileNamePostfix > 0 Then

                    'Open a file for writing                
                    path = Current.Server.MapPath(FILEpath & mfilenane & "_" & mfileNamePostfix & fileExtension)
                Else
                    path = Current.Server.MapPath(FILEpath & mfilenane & fileExtension)
                End If

                If File.Exists(path) = False Then
                    ' Create a file to write to.
                    Dim sw As StreamWriter = File.CreateText(path)
                    sw.WriteLine(FileContent)
                    sw.Flush()
                    sw.Close()
                    saved = True
                Else
                    mfileNamePostfix = mfileNamePostfix + 1

                End If
                Dim SERC As String = mfilenane
            Loop
        Catch
            HttpContext.Current.Response.Write(FILEpath & mfilenane)
            HttpContext.Current.Response.Write("savetextfile.mfilenane=" & mfilenane & "<br/>")
            HttpContext.Current.Response.Write("savetextfile.mFILEpath: " & path & "<br/>")
            HttpContext.Current.Response.End()
        End Try
    End Sub


    Public Shared Sub WriteServerCookie()
        Dim FileContent As String = Current.Session.SessionID

        'Open a file for writing

        Dim SESSION_FILENAME = Current.Session.SessionID & ".txt"
        Dim FILEpath As String = "sc\"

        'Open a file for writing

        Dim path As String = Current.Server.MapPath(FILEpath & SESSION_FILENAME)
        If File.Exists(path) = False Then
            ' Create a file to write to.
            Dim sw As StreamWriter = File.CreateText(path)
            sw.WriteLine(FileContent)
            sw.Flush()
            sw.Close()
        End If
        Dim SERC As String = SESSION_FILENAME
        'OSV001.Text = SESSION_FILENAME

    End Sub

    Public Shared Function StripLongFileAddress(ByVal ValueIn As String) As String
        Dim fReplace As String = ""
        fReplace = Mid(ValueIn, InStrRev(ValueIn, "\") + 1)
        Return fReplace
    End Function

    Public Shared Function GetBritishShortDate(ByVal ValueIn As String) As String
        Dim fReplace As String = ""
        fReplace = FormatDateTime(ValueIn, DateFormat.ShortDate)
        Return fReplace
    End Function
    ''' <summary>
    '''  converts a unicode date format into the dateformat defined by the user or vice verca 
    ''' </summary>
    ''' <param name="DateValueIn">DateValueIn as standard British date format 29/12/2002</param>
    ''' <returns>2012-12-29</returns>
    ''' <remarks></remarks>
    Public Shared Function GetuNICODEDate(ByVal DateValueIn As String) As String
        Dim ReturnDate As String = ""
        'Dim MyDate As New DateTime(mdateformat)
        'ReturnDate = MyDate.ToString(mdateformat)
        ReturnDate = Mid(DateValueIn, 4, 2) & "/" & Mid(DateValueIn, 1, 2) & "/" & Mid(DateValueIn, 7, 4)
        'ReturnDate = DateValueIn
        Return ReturnDate
    End Function
    Public Shared Function GetsqlDate(ByVal DateValueIn As String) As String
        Dim ReturnDate As String = ""
        'Dim MyDate As New DateTime(mdateformat)
        'ReturnDate = MyDate.ToString(mdateformat)
        ReturnDate = Mid(DateValueIn, 1, 2) & "/" & GetMonthNameFromNum(Mid(DateValueIn, 4, 2)) & "/" & Mid(DateValueIn, 8, 5)
        'ReturnDate = DateValueIn
        Return ReturnDate
    End Function
    Public Shared Function GetsqlDateWithMonthName(ByVal DateValueIn As String) As String
        Dim ReturnDate As String = ""
        'Dim MyDate As New DateTime(mdateformat)
        'ReturnDate = MyDate.ToString(mdateformat)

        ReturnDate = Mid(DateValueIn, 1, 2) & "/" & GetMonthNameFromNum(Month(DateValueIn)) & "/" & Year(DateValueIn)
        'ReturnDate = DateValueIn
        Return ReturnDate
    End Function

    'Shared Function StripSpecialChars(ByRef Data As String) As String
    '    If ifnullget(Data, "") <> "" Then
    '        Data = Trim(Data)
    '        Dim regEx As Object
    '        regEx = New Regex
    '        regEx.Global = True
    '        regEx.IgnoreCase = True
    '        regEx.Pattern = "[^a-zA-Z0-9\.\-\_\s()\[\]\&\/\\\@\'\,]"
    '        Data = regEx.Replace(Data, "")
    '    End If
    '    StripSpecialChars = Data
    'End Function
    Shared Function getpagename(ByVal input As String) As String
        ' First we see the input string.
        'Dim input As String = "/content/alternate-1.aspx"
        Dim key As String = "kk"
        ' Here we call Regex.Match "content/?????\.aspx$" is a value given by ural  "/content/alternate-1.aspx"
        'Dim match As Match = Regex.Match(input, "content/([A-Za-z0-9\-]+)\.aspx$", RegexOptions.IgnoreCase)
        Dim match As Match = Regex.Match(input, "/([A-Za-z0-9\-]+)\.aspx$", RegexOptions.IgnoreCase)
        ' Here we check the Match instance.
        If match.Success Then
            ' Finally, we get the Group value and display it.
            key = match.Groups(1).Value
            ' Console.WriteLine(key)
        End If
        Return key
    End Function

    ''If null get a different value passed as a second parameter else return the same value as first parameter
    Public Shared Function ifnullget(ByRef CheckValue As String, ByRef ReplaceValue As String) As String
        Dim ReturnValue As String = ""
        If Not (IsDBNull(CheckValue) Or Trim(CheckValue) = "" Or IsNothing(CheckValue)) Then
            ReturnValue = CheckValue
        Else
            ReturnValue = ReplaceValue
        End If
        Return ReturnValue
    End Function
    'Public Shared Function ifnullget(ByRef val As String, ByVal retval As String) As String
    '    Dim fReturnData As String = ""

    '    If IsNumeric(val) Then
    '        If IsDBNull(val) Or IsNothing(val) Or val = "" Then fReturnData = retval Else ifnullget = val
    '    Else
    '        If IsNothing(val) Or val = "" Then fReturnData = retval Else ifnullget = val
    '    End If
    '    Return fReturnData
    'End Function
    ' for clasic asp only
    'Public Function ifnullget(ByVal val, ByVal retval) As String
    '    If IsDBNull(val) Or IsNothing(val) Or val = "" Then ifnullget = retval Else ifnullget = val
    'End Function

    Public Shared Function GetColumnIndexByName(row As GridViewRow, columnName As String) As Integer
        Dim columnIndex As Integer = 0
        For Each cell As DataControlFieldCell In row.Cells
            If TypeOf cell.ContainingField Is BoundField Then
                If DirectCast(cell.ContainingField, BoundField).DataField.Equals(columnName) Then
                    Exit For
                End If
            End If
            ' keep adding 1 while we don't have the correct name
            columnIndex += 1
        Next
        Return columnIndex
    End Function
    Public Shared Function DateNow() As String
        Dim returnvalue As String
        returnvalue = Mid(Date.Now.ToString(), 1, 10)
        Return returnvalue
    End Function
    Public Shared Function YearStartDate() As String
        Dim returnvalue As String
        returnvalue = "01/01/" & Year(Date.Now).ToString
        Return returnvalue
    End Function
    Public Shared Function GetNormalDateFromNameDate(ByVal DateValueIn As String) As String
        Dim returnvalue As String = ""
        Dim mmonth As String = ""
        Dim mday As String = ""
        mmonth = Month(DateValueIn)
        mday = Day(DateValueIn)
        If mmonth < 10 Then
            mmonth = "0" & mmonth
        End If
        If mday < 10 Then
            mday = "0" & mday
        End If

        returnvalue = mday & "/" & mmonth & "/" & Year(DateValueIn)
        Return returnvalue
    End Function
    Public Shared Function validatedata(ByRef datatocheck As String) As String
        Dim mDataType As String = ""
        Select Case datatocheck
            Case IsNumeric(datatocheck)
                mDataType = "N"
            Case IsDate(datatocheck)
                mDataType = "D"
            Case IsDate(datatocheck)
                mDataType = "D"
            Case Else
                mDataType = "S"
        End Select

        Return mDataType
    End Function
    Public Shared Function GetIPAddress() As String
        Dim strHostName As String = System.Net.Dns.GetHostName()
        'IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
        Dim ipHostInfo As IPHostEntry = Dns.GetHostEntry(strHostName)
        Dim ipAddress As IPAddress = ipHostInfo.AddressList(0)

        Return ipAddress.ToString()
    End Function

    Public Shared Function GetUrlDecode(ByRef URLtoDecode As String) As String
        Dim returnValue As String = ""
        returnValue = HttpContext.Current.Server.UrlDecode(URLtoDecode)
        Return returnValue
    End Function
    Public Shared Function GetUrlEncode(ByRef URLtoDecode As String) As String
        Dim returnValue As String = ""
        returnValue = HttpContext.Current.Server.UrlEncode(URLtoDecode)
        Return returnValue
    End Function
    Public Shared Function StripOutNonNumericValues(StringToStrip As String) As String
        Dim CurrentPosition As Integer, CurrentCharacter As String, NumericString As String
        For CurrentPosition = 1 To Len(StringToStrip)
            CurrentCharacter = Mid(StringToStrip, CurrentPosition, 1)
            If IsNumeric(CurrentCharacter) Then NumericString = NumericString + CurrentCharacter
        Next
        StripOutNonNumericValues = NumericString
    End Function
    Shared Function splitAndHyperlink(ByRef StringToHyperlink As String, Optional hyperlink As String = "asas") As String
        Dim returnvalue As String = ""
        Dim a = Split(StringToHyperlink, ",", -1)
        Dim mLockUnlock As Boolean = True
        Dim mOrder_id As Integer = 0
        Dim counter As Integer = 1
        Try
            For Each x In a
                returnvalue = returnvalue & "<a href=""" & hyperlink & x & """ target='blank' >" & x & "</a>&nbsp;"
                counter += counter
            Next
        Catch ex As Exception
            HttpContext.Current.Response.Write("Command Argument:" & (StringToHyperlink & "<br />"))
            HttpContext.Current.Response.Write("Command Argument:" & (hyperlink & "<br />"))
            HttpContext.Current.Response.Write("1:" & ex.Message & "<br/>")
            HttpContext.Current.Response.End()
        End Try
        ' returnvalue = "<a href=""" & hyperlink & """>" & StringToHyperlink & "</a>"
        'HttpContext.Current.Response.Write(StringToHyperlink & vatrate & "<hr />")
        'If vatrate > 0 Then
        '    mconvertedPrice = (mconvertedPrice / (100 + vatrate)) * 100
        '    'HttpContext.Current.Response.Write(mconvertedPrice)
        '    returnvalue = (MiscClass.RoundToNext((mconvertedPrice + (mconvertedPrice * 0.21)), 1) - 0.01)
        'Else
        '    returnvalue = (MiscClass.RoundToNext((mconvertedPrice + (mconvertedPrice * 0.21)), 1) - 0.01)
        'End If
        Return returnvalue
    End Function

    'Shared Sub PopulateDay(MYEAR, MMONTH)
    '    ddlDay.Items.Clear()
    '    Dim lt As ListItem = New ListItem
    '    lt.Text = "DD"
    '    lt.Value = "0"
    '    ddlDay.Items.Add(lt)
    '    '        Response.Write(SelectMonth.SelectedValue)
    '    Dim days As Integer = DateTime.DaysInMonth(MYEAR, MMONTH)
    '    Dim i As Integer = 1
    '    Do While (i <= days)
    '        lt = New ListItem
    '        lt.Text = i.ToString
    '        lt.Value = i.ToString
    '        ddlDay.Items.Add(lt)
    '        i = (i + 1)
    '    Loop

    ' End Sub

    Public Shared Sub ConvertGridToTable(ByRef dt As DataTable, ByRef grd As GridView)
        Try
            If grd.Rows.Count <= 0 Then
                Return
            End If

            For i As Integer = 0 To grd.Columns.Count - 1
                If grd.Columns(i).[GetType]().Name.Equals("BoundField") Then
                    Dim bf As BoundField = DirectCast(grd.Columns(i), BoundField)
                    dt.Columns.Add(bf.DataField.ToString())
                End If
            Next

            For i As Integer = 0 To grd.Rows.Count - 1
                dt.Rows.Add()
                For j As Integer = 0 To grd.Columns.Count - 1
                    If grd.Columns(j).[GetType]().Name.Equals("BoundField") Then
                        Dim bf As BoundField = DirectCast(grd.Columns(j), BoundField)
                        For k As Integer = 0 To dt.Columns.Count - 1
                            If dt.Columns(k).ColumnName.Trim().Equals(bf.DataField.ToString()) Then
                                Dim value As String = If(grd.Rows(i).Cells(j).Text.Trim().Contains("&nbsp;"), grd.Rows(i).Cells(j).Text.Trim().Replace("&nbsp;", String.Empty), grd.Rows(i).Cells(j).Text.Trim())
                                dt.Rows(i)(bf.DataField.ToString()) = value
                            End If
                        Next
                    End If
                Next
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    '    Public Shared Sub gridviewtotable()
    'var dataTable = new DataTable();

    'Array.ForEach(
    'dataGridView1.Columns.Cast<DataGridViewColumn>().ToArray(), 
    'arg => dataTable.Columns.Add(arg.HeaderText, arg.ValueType));
    'Array.ForEach(
    'dataGridView1.Rows.Cast<DataGridViewRow>().ToArray(), 
    'arg => dataTable.Rows.Add(arg.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value)));

    'return dataTable;
    '    End Sub
    Function isReallyNumeric(ByRef str As String) As Boolean
        Dim d, i
        Dim misReallyNumeric As Boolean = True
        For i = 1 To Len(Str)
            d = Mid(Str, i, 1)
            If Asc(d) < 48 Or Asc(d) > 57 Then
                isReallyNumeric = False
                Exit For
            End If
        Next
        Return misReallyNumeric
    End Function

End Class
