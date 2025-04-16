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
Public Class MiscClass
Shared LastLineUsed As Integer = 12
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
			Case mmonth = "Jan" Or mmonth = "january" Or mmonth = "jan"
                returnvalue = "1"
            Case mmonth = "Feb" Or mmonth = "February" Or mmonth = "feb"
                returnvalue = "2"
            Case mmonth = "Mar" Or mmonth = "March" Or mmonth = "mar"
                returnvalue = "3"
            Case mmonth = "Apr" Or mmonth = "April" Or mmonth = "apr"
                returnvalue = "4"
            Case mmonth = "May" Or mmonth = "May" Or mmonth = "may"
                returnvalue = "5"
            Case mmonth = "Jun" Or mmonth = "June" Or mmonth = "jun"
                returnvalue = "6"
            Case mmonth = "Jul" Or mmonth = "July" Or mmonth = "jul"
                returnvalue = "7"
            Case mmonth = "Aug" Or mmonth = "August" Or mmonth = "aug"
                returnvalue = "8"
            Case mmonth = "Sep" Or mmonth = "September" Or mmonth = "sep"
                returnvalue = "9"
            Case mmonth = "Oct" Or mmonth = "october" Or mmonth = "oct"
                returnvalue = "10"
            Case mmonth = "Nov" Or mmonth = "November" Or mmonth = "nov"
                returnvalue = "11"
            Case mmonth = "Dec" Or mmonth = "December" Or mmonth = "dec"
                returnvalue = "12"
            Case Else
                returnvalue = "01"
        End Select
        'HttpContext.Current.Response.Write(returnvalue & "<br>")
        'HttpContext.Current.Response.End()
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
	 Public Shared Function SameDatePreviousYear(ByVal currentdate As String, Optional ByVal monthsback As String = "12") As String
        'HttpContext.Current.Response.Write("currentdate:" & currentdate & ",monthsback " & monthsback & " <br>")
        Dim returnvalue As String = ""
        Dim Sentdate As DateTime = DateTime.Parse(currentdate)
        Sentdate = Sentdate.AddMonths(-monthsback)
        returnvalue = Mid(Sentdate.ToString, 1, 10)
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

    Public Shared Function GetDaysInMonth(myear As Integer, mmonth As Integer) As Integer
        Dim mnoofdays As Integer = 0
        mnoofdays = DateTime.DaysInMonth(myear, mmonth)
        Return mnoofdays
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
        If symbol = "PC" Then
            ReturnStringNumber = fAmount.ToString("N", nfi) & insertsymbol
        Else
            ReturnStringNumber = insertsymbol & fAmount.ToString("N", nfi)
        End If
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
        ReturnDate = Mid(DateValueIn, 1, 2) & "/" & GetMonthNameFromNum(Mid(DateValueIn, 4, 2)) & "/" & Mid(DateValueIn, 7, 4)
        'ReturnDate = DateValueIn
        Return ReturnDate
    End Function
    Public Shared Function GetsqlDateWithMonthName(ByVal DateValueIn As String) As String
        Dim ReturnDate As String = ""
		if DateValueIn <> "" then 
		'HttpContext.Current.Response.Write(DateValueIn)
		'HttpContext.Current.Response.end()
        'Dim MyDate As New DateTime(mdateformat)
        'ReturnDate = MyDate.ToString(mdateformat)
        ReturnDate = Mid(DateValueIn, 1, 2) & "/" & GetMonthNameFromNum(Month(DateValueIn)) & "/" & Year(DateValueIn)
        'ReturnDate = DateValueIn
		end if
        Return ReturnDate
    End Function

    'Private Function StripSpecialChars(ByRef Data As String) As String
    '    If ifnullget(Data, "") <> "" Then
    '        Data = Trim(Data)
    '        Dim regEx As String
    '        regEx = New RegExp
    '        regEx.Global = True
    '        regEx.IgnoreCase = True
    '        regEx.Pattern = "[^a-zA-Z0-9\.\-\_\s()\[\]\&\/\\\@\'\,]"
    '        Data = regEx.Replace(Data, "")
    '    End If
    '    StripSpecialChars = Data
    'End Function

    ''If null get a different value passed as a second parameter else return the same value as first parameter
    Public Shared Function ifnullget(ByRef CheckValue As object, ByRef ReplaceValue As String) As String
        Dim ReturnValue As String = ""

        If Not (IsDBNull(CheckValue)  Or IsNothing(CheckValue)) Then
            ReturnValue = CheckValue
        Else
            ReturnValue = ReplaceValue
        End If
        Return ReturnValue
    End Function
 '   Public Shared Function ifnullget(ByRef val As String, ByVal retval As String) As String
 '       Dim fReturnData As String = ""
 ' function returns wrong value not to be used till fixed
 '       If IsNumeric(val) Then
 '           If IsDBNull(val) Or IsNothing(val) Or val = "" Then fReturnData = retval Else ifnullget = val
 '       Else
 '            If IsNothing(val) Or val = "" Then fReturnData = retval Else ifnullget = val
 '        End If
 '        Return fReturnData
 '    End Function  
 ' for clasic asp only
 'Public Function ifnullget(ByVal val, ByVal retval) As String
 '    If IsDBNull(val) Or IsNothing(val) Or val = "" Then ifnullget = retval Else ifnullget = val
 'End Function
'***********************
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
	Public Shared Function DatetoSQLMonthName(ByRef mdate As Date) As String
        Dim datepart As String = MiscClass.GetsqlDateWithMonthName(Mid(mdate.ToString, 1, 10))
        Dim timepart As String = If(Len(mdate.ToString) > 10, Mid((mdate.ToString), 11), "")
        Dim returnvalue As String = ""
        returnvalue = datepart & " " & timepart
        Return returnvalue
    End Function
    Public Shared Function DaysFromFirstSunday2(ByRef mdate As Date) As String
     '   Dim returnvalue As String = ""
     '   returnvalue = DateDiff("d", FirstSundayOfMonth(mdate), mdate) - 7 
     '   Return returnvalue
	    Dim returnvalue As String = ""
        Dim DaysToMinus As Integer = 0
		
        Dim mMONTH As Integer = DatePart(DateInterval.Month, mdate)
        Dim myear As Integer = DatePart(DateInterval.Year, mdate)
		dim mday as integer = DatePart(DateInterval.day, mdate)
        Dim datemade As String = ""
        datemade = (1 & "/" & mMONTH & "/" & myear)
		dim datemade_lastyear As String = ""
		datemade_lastyear=(1 & "/" & mMONTH & "/" & myear-1)
		Dim mFirstDaysName As String = DaysNameFromDate(datemade)
		Dim mFirstDaysName_lastyear As String = DaysNameFromDate(datemade_lastyear)
		'HttpContext.Current.Response.Write("datemade=" & datemade & "###DaysNameFromDate(datemade)=" & DaysNameFromDate(datemade) &"mday=" & mday & "###" )
		'HttpContext.Current.Response.Write("datemade=" & datemade & "###DaysNameFromDate(datemade)=" & DaysNameFromDate(datemade) & "###")
		'HttpContext.Current.Response.Write("mFirstDaysName_lastyear=" & mFirstDaysName_lastyear & "###DayNumberFromDayname=" & DayNumberFromDayname(mFirstDaysName_lastyear) -DayNumberFromDayname(mFirstDaysName) & "###")
		If     mFirstDaysName ="Sunday" Then
            DaysToMinus = 0
        ElseIf mFirstDaysName = "Monday" and mday <= 7 Then
			DaysToMinus = 7
        ElseIf mFirstDaysName = "Tuesday"  and mday <= 7 Then
			DaysToMinus = 7
		ElseIf mFirstDaysName = "Wednesday"  and mday <= 7 Then
            DaysToMinus = 7
		ElseIf mFirstDaysName = "Thursday"  and mday <= 7 Then
            DaysToMinus = 7
		ElseIf mFirstDaysName = "Friday"  and mday <= 7 Then
            DaysToMinus = 7
		ElseIf mFirstDaysName = "Saturday"  and mday <= 7 Then
            DaysToMinus = 7	
			End If
			'DaysToMinus =7
        returnvalue = DateDiff("d", FirstSundayOfMonth(mdate), mdate) - DaysToMinus 
		'HttpContext.Current.response.write ("<p>days to minus=" & returnvalue)
        Return returnvalue
    End Function
	Public Shared Function DaysFromFirstSunday(ByRef mdate As Date) As String
        Dim returnvalue As String = ""
        Dim DaysToMinus As Integer = 7
        Dim mMONTH As Integer = DatePart(DateInterval.Month, mdate)
        Dim myear As Integer = DatePart(DateInterval.Year, mdate)
        Dim datemade As String = ""
        datemade = (1 & "/" & mMONTH & "/" & myear)
        Dim mFirstDaysName As String = DaysNameFromDate(datemade)
        'HttpContext.Current.Response.Write("datemade=" & datemade & "###DaysNameFromDate(datemade)=" & DaysNameFromDate(datemade) & "###")
        If "Sunday" = mFirstDaysName Then
            DaysToMinus = 0
        ElseIf mFirstDaysName = "Monday" Then
            DaysToMinus = 0
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
        'HttpContext.Current.response.write ("ln:377 miscfunc.LastYearMatchDayFromFirstSunday mdaysfromsunday=" & mdaysfromsunday & "::")
		If mdaysfromsunday < 0 Then
		  mdaysfromsunday = 7 + mdaysfromsunday' + 1
		else if mdaysfromsunday = 7 then 
		  mdaysfromsunday = 7 + mdaysfromsunday
		else if mdaysfromsunday < 7 then 
		  mdaysfromsunday =  mdaysfromsunday + 7
		else
		  mdaysfromsunday =  mdaysfromsunday 
        End If
'		mdaysfromsunday = 7+1
' HttpContext.Current.response.write ("ln 388: miscfunc.LastYearMatchDayFromFirstSunday mdaysfromsunday=" & mdaysfromsunday & "::")
        returnvalue = DateAdd("D", (mdaysfromsunday), FirstSundayOfMonth("01/" & mMONTH & "/" & myear - 1))
        Return returnvalue
    End Function
    Public Shared Function DaysInMonthOfDate(ByRef mdate As Date) As String
        Dim returnvalue As String = ""
        Dim myear As Integer = DatePart(DateInterval.Year, mdate)
        Dim mMONTH As Integer = DatePart(DateInterval.Month, mdate)
        returnvalue = System.DateTime.DaysInMonth(myear, mMONTH)
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
            If "Sunday".ToUpper() = trim(DaysNameFromDate(datemade)).ToUpper() Then
                returnvalue = datemade
                Exit For
            End If
        Next
        Return returnvalue
    End Function
'***********************
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
        Return returnvalue
    End Function
	 
   Public Shared Function StripOutNonNumericValues(StringToStrip As String) As String
        Dim CurrentPosition As Integer, CurrentCharacter As String, NumericString As String
        For CurrentPosition = 1 To Len(StringToStrip)
            CurrentCharacter = Mid(StringToStrip, CurrentPosition, 1)
            If IsNumeric(CurrentCharacter) Then NumericString = NumericString + CurrentCharacter
        Next
        StripOutNonNumericValues = NumericString
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
	 ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DateValueIn">give date value like 01/oct/2015</param>
    ''' <returns>01/10/2015 amd dd/mm/yyyy</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNormalDateFromMonthNameDate(ByVal DateValueIn As String) As String
        Dim returnvalue As String = ""
        Dim mmonth As String = ""
        Dim mday As String = ""
        Dim myear As String = ""
        myear = Mid(DateValueIn, 8, 4)
        mday = Mid(DateValueIn, 1, 2)
        mmonth = Mid(DateValueIn, 4, 3)
        mmonth = GetMonthNumFromName(mmonth)
        returnvalue = mday & "/" & mmonth & "/" & myear
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
    'Public Shared Function giveUserCount() As String
    '    Dim i As Integer = 0
    '    For Each i In Session.StaticObjects
    '        Response.Write(i & "<br>")
    '    Next
    'End Function

    Shared Function addpadding(ByRef oldstring As String, ByRef newstringlength As String, Optional ByRef paddingcharacted As String = "0") As String

        Dim returnvalue As String = ""
        Dim oldstringlength As Integer = Len(oldstring)
        'HttpContext.Current.Response.Write("<br>ERR1: FN_addpadding.oldstring = " & oldstring & ";oldstringlength =" & oldstringlength)
        Try
            For i = 1 To (newstringlength - oldstringlength)
                returnvalue = returnvalue + paddingcharacted
            Next
            returnvalue = returnvalue + oldstring.ToString
            'HttpContext.Current.Response.Write("<br>returnvalue=" & returnvalue & ";<br>")
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
    Shared Function RemoveItemfromLabel(myValue As Object, itemtoremove As String) As String
        'HttpContext.Current.Response.Write("myValue.ToString():" & myValue.ToString())
        If myValue = itemtoremove Then
            Return ""
        Else
            Return myValue
        End If

    End Function
	Shared Function GetFileName(ByRef filePrefix As String, ByRef filename As String, Optional extention As String = "csv", Optional Dateformat As String = "YYYYMMDDHHMMSS", Optional monthname As Boolean = False, Optional addrandom As Boolean = True) As String
        Dim mCountryslect As String = ""
        Dim returnvlaue As String = ""
        Dim mFilename As String = ""
        Dim yearFrom As String = ""
        Dim mMonthName As String = ""
        Dim mday As String = ""
        Dim mhour As String = ""
        Dim mminute As String = ""
        Dim mseconds As String = ""
        Dim datetilmestring As String = ""
        Dim rno As Integer = CInt(Int((9 * Rnd()) + 1))
        mMonthName = addpadding(DateTime.Now.Month.ToString(), 2).ToString
        yearFrom = DateTime.Now.Year.ToString()
        mday = addpadding(DateTime.Now.Day.ToString(), 2)
        mhour = addpadding(DateTime.Now.Hour.ToString(), 2)
        mminute = addpadding(DateTime.Now.Minute.ToString(), 2)
        mseconds = addpadding(DateTime.Now.Second.ToString(), 2)

        Select Case Dateformat
            Case "YYYYMMDDHHMMSS"
                datetilmestring = yearFrom.ToString & mMonthName.ToString & mday.ToString & mhour.ToString & mminute.ToString & mseconds.ToString
            Case "YYYYMMDDHHMM"
                datetilmestring = yearFrom.ToString & mMonthName.ToString & mday.ToString & mhour.ToString & mminute.ToString
            Case "YYYYMMDDHH"
                datetilmestring = yearFrom.ToString & mMonthName.ToString & mday.ToString & mhour.ToString
            Case "YYYYMMDD"
                datetilmestring = yearFrom.ToString & mMonthName.ToString & mday.ToString
            Case "DDMMYYYYHHMMSS"
                datetilmestring = mday & mMonthName & yearFrom & mhour & mminute & mseconds
            Case "DDMMYYYYHHMM"
                datetilmestring = mday & mMonthName & yearFrom & mhour & mminute
            Case "DDMMYYYYHH"
                datetilmestring = mday & mMonthName & yearFrom & mhour & mminute
            Case "DDMMYYYY"
                datetilmestring = mday & mMonthName & yearFrom
            Case Else
                datetilmestring = "BAD_"
        End Select
        If addrandom Then
            mFilename = filePrefix & filename & datetilmestring.ToString & rno.ToString
        Else
            mFilename = filePrefix & filename & datetilmestring.ToString
        End If
        If extention Is "Null" Then
            returnvlaue = mFilename
        Else
            returnvlaue = mFilename & "." & extention
        End If

        Return returnvlaue
    End Function
 Shared Function getfilename2(ByRef filePrefix As String, ByRef filename As String, Optional extention As String = "csv", Optional Dateformat As String = "YYYYMMDDHHMMSS", Optional monthname As Boolean = False) As String
        Dim mCountryslect As String = ""
        Dim returnvlaue As String = ""
        Dim mFilename As String = ""
        Dim yearFrom As String = ""
        Dim mMonthName As String = ""
        Dim mday As String = ""
        Dim mhour As String = ""
        Dim mminute As String = ""
        Dim mseconds As String = ""
        Dim datetilmestring As String = ""
        mMonthName = addpadding(DateTime.Now.Month.ToString(), 2)
        yearFrom = DateTime.Now.Year.ToString()
        mday = addpadding(DateTime.Now.Day.ToString(), 2)
        mhour = addpadding(DateTime.Now.Hour.ToString(), 2)
        mminute = addpadding(DateTime.Now.Minute.ToString(), 2)
        mseconds = addpadding(DateTime.Now.Second.ToString(), 2)

        Select Case Dateformat
            Case "YYYYMMDDHHMMSS"
                datetilmestring = yearFrom.ToString & mMonthName.ToString & mday.ToString & mhour.ToString & mminute.ToString & mseconds.ToString
            Case "YYYYMMDDHHMM"
                datetilmestring = yearFrom.ToString & mMonthName.ToString & mday.ToString & mhour.ToString & mminute.ToString
            Case "YYYYMMDD"
                datetilmestring = yearFrom & mMonthName & mday
            Case "DDMMYYYYHHMMSS"
                datetilmestring = mday & mMonthName & yearFrom & mhour & mminute & mseconds
            Case "DDMMYYYY"
                datetilmestring = mday & mMonthName & yearFrom
            Case Else
                datetilmestring = ""
        End Select
        mFilename = filePrefix & filename & datetilmestring
        If extention Is "Null" Then
            returnvlaue = mFilename
        Else
            returnvlaue = mFilename & "." & extention
        End If

        Return returnvlaue
    End Function
	Public Shared Function DayNumberFromDayname(ByRef DaysName As String) As Integer
		Dim returnvalue As Integer
		If DaysName = "Sunday" Then
			returnvalue = 1
		ElseIf DaysName = "Monday" Then
			returnvalue = 2
		ElseIf DaysName = "Tuesday" Then
			returnvalue = 3
		ElseIf DaysName = "Wednesday" Then
			returnvalue = 4
		ElseIf DaysName = "Thursday" Then
			returnvalue = 5
		ElseIf DaysName = "Friday" Then
			returnvalue = 6
		ElseIf DaysName = "Saturday" Then
			returnvalue = 7
		End If
		Return returnvalue
	End Function
	Public Shared Function GetQuarterFromDate([date] As DateTime) As Integer
        If [date].Month >= 4 AndAlso [date].Month <= 6 Then
            Return 1
        ElseIf [date].Month >= 7 AndAlso [date].Month <= 9 Then
            Return 2
        ElseIf [date].Month >= 10 AndAlso [date].Month <= 12 Then
            Return 3
        Else
            Return 4
        End If

    End Function
	Shared Function islocalhost() As Boolean
        Dim returnvalue As Boolean = False
        If "localhost" = HttpContext.Current.Request.ServerVariables("HTTP_HOST") Then returnvalue = True
        Return returnvalue
    End Function
	Shared Function LastYearsSameDayNumber() As Integer
        Dim edate As String = (MiscClass.FirstSundayOfMonth(Now))
        Dim edateConvertedToDate = DateClass.DateStringToDate(edate)
        Dim LastYearDay As String = MiscClass.LastYearMatchDayFromFirstSunday(Now.ToString("dd/MM/yyyy"))
        Return Day(LastYearDay) + 1 + edateConvertedToDate.Day
    End Function
	Shared Function ButtonUsageAppend(PageName As String, Optional ButtonName As String = "%", Optional [UserID] As String = "0") As String
		Dim rv As String = ""
		Dim strsql As String = ""
		Dim ifexists As String = "false"
		 Try
            strsql = ("select top 1 * from [Escapade_New].[dbo].[MIS_ButtonUseMap] where [ButtonName] like '%" & ButtonName & "%' and [PageName]='" & [PageName] & "' and [UserID]='" & [UserID] & "'") : LastLineUsed = 885
            ' Current.Response.Write("<br>" & strsql)
            Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql) : LastLineUsed = 887
            If dt.Rows.Count > 0 Then ifexists = True
           ' Current.Response.Write("<br>#ifexists=" & ifexists.ToString & ",   dt.Rows.Count=" & dt.Rows.Count.ToString) : LastLineUsed = 888

            dt = Nothing
            If UCase(ifexists.ToString) = UCase("True") Then
                Dim strsql_Update As String = "Update [MIS_ButtonUseMap] set Usage = (isnull([Usage],0)+1),LastUsed=getdate()  where [ButtonName] like '%" & ButtonName & "%' and [PageName]='" & [PageName] & "' and [UserID]='" & [UserID] & "';"
                SQLFunctions.RunSQLStringWOConstring(strsql_Update) : LastLineUsed = 891
                'Current.Response.Write("<br>##ifexists=" & ifexists.ToString) : LastLineUsed = 894
                Return True
            ElseIf ucase(ifexists.ToString) = UCase("false") Then
                SQLFunctions.RunSQLStringWOConstring("insert into [MIS_ButtonUseMap] ([ButtonName],[PageName],[UserID],Usage,[LastUsed],[FirstUse]) VALUES('" & ButtonName & "','" & PageName & "','" & [UserID] & "','1',getdate(),getdate());") : LastLineUsed = 894
                Return False
            '    Current.Response.Write("<br>###ifexists=" & ifexists.ToString) : LastLineUsed = 897
            End If
            'Current.Response.Write("<br>After Select Case, LastLineUsed=" & LastLineUsed) : LastLineUsed = 897
        Catch ex As Exception
            Current.Response.Write("<br>##Misclass.ButtonUsageAppend.Err=" & ex.Message.ToString)
            Current.Response.Write("<br>##LastLineUsed=" & LastLineUsed.ToString)
            Current.Response.Write("<br>##strsql=" & strsql)
            Return False
        End Try

        Try
            If ifexists = True Then

            End If
        Catch ex As Exception

        End Try
        Return rv
	
	end function
	
	 Shared Function PageUsageAppend(PageName As String, Optional [UserID] As String = "0") As String
        Dim rv As String = ""
        Dim strsql As String = ""
        Dim ifexists As String = "false"
        Try
            strsql = ("select top 1 * from [Escapade_New].[dbo].[MIS_ButtonUseMap] where [PageName]='" & [PageName] & "' and [UserID]='" & [UserID] & "'") : LastLineUsed = 885
            ' Current.Response.Write("<br>" & strsql)
            Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql) : LastLineUsed = 887
            If dt.Rows.Count > 0 Then ifexists = True
            'Current.Response.Write("<br>#ifexists=" & ifexists.ToString & ",   dt.Rows.Count=" & dt.Rows.Count.ToString) : LastLineUsed = 888

            dt = Nothing
            If UCase(ifexists.ToString) = UCase("True") Then
                Dim strsql_Update As String = "Update [MIS_ButtonUseMap] set [PageLoaded] = (isnull([PageLoaded],0)+1),[LastRefresh]=getdate()  where [PageName]='" & [PageName] & "' and [UserID]='" & [UserID] & "';"
                SQLFunctions.RunSQLStringWOConstring(strsql_Update) : LastLineUsed = 891
                'Current.Response.Write("<br>##ifexists=" & ifexists.ToString) : LastLineUsed = 894
                Return True
            ElseIf UCase(ifexists.ToString) = UCase("false") Then
                SQLFunctions.RunSQLStringWOConstring("insert into [MIS_ButtonUseMap] ([PageName],[UserID],PageLoaded,[LastRefresh]) VALUES('" & PageName & "','" & [UserID] & "','1',getdate());") : LastLineUsed = 894
                Return False
                'Current.Response.Write("<br>###ifexists=" & ifexists.ToString) : LastLineUsed = 897
            End If
            'Current.Response.Write("<br>After Select Case, LastLineUsed=" & LastLineUsed) : LastLineUsed = 897
        Catch ex As Exception
            Current.Response.Write("<br>##Misclass.ButtonUsageAppend.Err=" & ex.Message.ToString)
            Current.Response.Write("<br>##LastLineUsed=" & LastLineUsed.ToString)
            Current.Response.Write("<br>##strsql=" & strsql)
            Return False
        End Try

        Try
            If ifexists = True Then

            End If
        Catch ex As Exception

        End Try
        Return rv
    End Function
	
	
	
	Shared Function GetUsersId() As String
        Dim rv As String = "0"
        Return rv
    End Function
	 Shared Function GiveHeatColorPattren(cellvalueT As String, Optional maxvalue As Integer = 16000, Optional minvalue As Integer = 1) As String
	  'if maxvalue ="" then maxvalue = 0
	  maxvalue= ifnullget(maxvalue,1600)
	  cellvalueT = ifnullget(cellvalueT,0)
 'if maxvalue = ""  then maxvalue = "0"
 'if cellvalueT = ""  then cellvalueT = "0"
 
        Dim reduceby As double = maxvalue / 21
        Dim rv As String = " background-color:"
        If cellvalueT >= maxvalue Then
            rv = (" background-color:#FF0000; color:white;")
        ElseIf cellvalueT >= (maxvalue - (reduceby * 1)) And cellvalueT <= maxvalue Then
            rv = (" background-color:#FF2A00; color:white;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 2) And cellvalueT <= (maxvalue - (reduceby * 1)) Then
            rv = (" background-color:#FF1100; color:white;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 3) And cellvalueT <= (maxvalue - (reduceby * 2)) Then
            rv = (" background-color:#FF2200; color:white;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 4) And cellvalueT <= (maxvalue - (reduceby * 3)) Then
            rv = (" background-color:#FF3300; color:white;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 5) And cellvalueT <= (maxvalue - (reduceby * 4)) Then
            rv = (" background-color:#FF4400; color:white;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 6) And cellvalueT <= (maxvalue - (reduceby * 5)) Then
            rv = (" background-color:#FF5500; color:white;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 7) And cellvalueT <= (maxvalue - (reduceby * 6)) Then
            rv = (" background-color:#FF6600; color:white;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 8) And cellvalueT <= (maxvalue - (reduceby * 7)) Then
            rv = (" background-color:#FF7700; color:white;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 9) And cellvalueT <= (maxvalue - (reduceby * 8)) Then
            rv = (" background-color:#FF8800; color:white;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 10) And cellvalueT <= (maxvalue - (reduceby * 9)) Then
            rv = (" background-color:#FF9900;color:Black;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 11) And cellvalueT <= (maxvalue - (reduceby * 10)) Then
            rv = (" background-color:#FFAA00;color:Black;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 12) And cellvalueT <= (maxvalue - (reduceby * 11)) Then
            rv = (" background-color:#FFBB00;color:Black;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 13) And cellvalueT <= (maxvalue - (reduceby * 12)) Then
            rv = (" background-color:#FFC644;color:Black;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 14) And cellvalueT <= (maxvalue - (reduceby * 13)) Then
            rv = (" background-color:#FFD644;color:Black;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 15) And cellvalueT <= (maxvalue - (reduceby * 14)) Then
            rv = (" background-color:#FFFF00;color:Black;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 16) And cellvalueT <= (maxvalue - (reduceby * 15)) Then
            rv = (" background-color:#FFFF5B;color:Black;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 17) And cellvalueT <= (maxvalue - (reduceby * 16)) Then
            rv = (" background-color:#FFFF84;color:Black;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 18) And cellvalueT <= (maxvalue - (reduceby * 17)) Then
            rv = (" background-color:#FFFFC2;color:Black;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 19) And cellvalueT <= (maxvalue - (reduceby * 18)) Then
            rv = (" background-color:#FFFFCC;color:Black;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 20) And cellvalueT <= (maxvalue - (reduceby * 19)) Then
            rv = (" background-color:#FFF8C6;color:Black;")
        ElseIf cellvalueT >= maxvalue - (reduceby * 21) And cellvalueT <= (maxvalue - (reduceby * 20)) Then
            rv = (" background-color:#FFF8DC;color:Black;")
        ElseIf cellvalueT > 1 And cellvalueT <= 5000 Then
            rv = (" background-color:#FBF6D9;color:Black;")
        End If
        Return rv
    End Function
    ''' <summary>
    ''' use following line with required data in _LoadComplete like DateClass.GetYearsList(2006, today.year.tostring)
    ''' </summary>
    ''' <param name="StartNumber">starting year value like 2006, else it will take it 10 years back</param>
    ''' <param name="EndNumber">if no value is given it will take current year as default</param>
    ''' <returns></returns>
    Public Shared Function GetNumberSequenceList(Optional StartNumber As Integer = 0, Optional EndNumber As Integer = 0) As DropDownList
        Dim ddl_NumSeq As New DropDownList
        ddl_NumSeq.Items.Clear()
        Dim lt As ListItem = New ListItem
        If EndNumber = 0 Then EndNumber = 25
        If StartNumber = 0 Then StartNumber = 0
        For i = StartNumber To (EndNumber + 1)
            lt = New ListItem
            lt.Text = i.ToString
            lt.Value = i.ToString
            If i = EndNumber Then lt.Selected = True
            ddl_NumSeq.Items.Add(lt)
            'Current.Response.Write("<br>##MiscClass.GetNumberSequenceList.Value of i =" & i.ToString)
        Next


        Return ddl_NumSeq
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="baseDate">date in </param>
    ''' <param name="daysToAdd">integer number </param>
    ''' <returns>ie:AddDaysToDate(#17/03/2025#, 10) ' Adds 10 days to March 17,</returns>
    Shared Function AddDaysToDate(ByVal baseDate As Date, ByVal daysToAdd As Integer, Optional ByVal timeToAdd As String = "17:00:00") As String
        ' Combine the base date and default or specified time
        Dim fullDate As Date = DateAdd("d", daysToAdd, baseDate)
        Dim formattedDate As String = Format(fullDate, "yyyy-MM-dd") & " " & timeToAdd
        AddDaysToDate = formattedDate
    End Function


End Class
