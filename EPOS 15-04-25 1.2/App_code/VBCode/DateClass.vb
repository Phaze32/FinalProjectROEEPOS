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
Public Class DateClass
    Public Shared LastLine As Integer = 10
    Public Shared Function GetMonthNameFromNum(ByVal mmonth As String) As String
        Dim returnvalue As String = "XX"
        mmonth = Val(MiscClass.MyReplace(MiscClass.MyReplace(mmonth, "/", ""), "-", ""))
        'HttpContext.Current.Response.Write("mmonthVale:" & mmonth & "<br>")
        Select Case val(mmonth)
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
	
	''' <summary>
    ''' returns date time in a format readable by SQL formats supports ddmmyyyy,yyyymmdd,mmddyyyy
    ''' </summary>
    ''' <param name="strDateTime"></param>
    ''' <param name="dateformat">format of the date submitted,supports ddmmyyyy,yyyymmdd,mmddyyyy </param>
    ''' <returns>20-Jan-2018 10:20:33 123:00 or 20/Jan/2018 10:20:33 123:00</returns>
    Shared Function ConvertToSQLMonthNameDateTime(strDateTime As String, Optional dateformat As String = "ddmmyyyy") As String
        Dim startpos As Integer = 0
        Dim endpos As Integer = 0
        Dim rv As String = strDateTime
        If dateformat = "ddmmyyyy" Or dateformat = "yyyymmdd" Then
            If InStr(strDateTime, "/") > 0 Then
                startpos = InStr(strDateTime, "/")
                endpos = InStr(startpos + 1, strDateTime, "/")
            ElseIf InStr(strDateTime, "-") > 0 Then
                startpos = InStr(strDateTime, "-")
                endpos = InStr(startpos + 1, strDateTime, "-")
            End If
           ' HttpContext.Current.Response.Write("ddmmyyyy;  startpos=" & startpos & ", endpos=" & endpos & ",strDateTime=" & strDateTime & ",strDateTime=" & strDateTime & ", value=" & Mid((strDateTime), startpos + 1, (endpos - 1-startpos) ) & ", dateformat=" & dateformat)
            rv = Mid(strDateTime, 1, startpos) & DateClass.GetMonthNameFromNum(Mid((strDateTime), startpos + 1, (endpos - 1-startpos))) + Mid(strDateTime, endpos)
        ElseIf dateformat = "mmddyyyy" Then
            If InStr(strDateTime, "/") > 0 Then
                startpos = 1
                endpos = InStr(startpos, strDateTime, "/")
            ElseIf InStr(strDateTime, "-") > 0 Then
                startpos = 1
                endpos = InStr(startpos + 1, strDateTime, "-")
            End If
            'HttpContext.Current.Response.Write("mmddyyyy;  startpos=" & startpos & ", endpos=" & endpos)
            rv = Mid(strDateTime, 1, startpos) & DateClass.GetMonthNameFromNum(Mid((strDateTime), startpos + 1, (endpos - 1-startpos))) + Mid(strDateTime, endpos)
        End If


        Return rv

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

    Public Shared Function GetDaysInMonth(myear As Integer, mmonth As Integer) As Integer
        Dim mnoofdays As Integer = 0
        mnoofdays = DateTime.DaysInMonth(myear, mmonth)
        Return mnoofdays
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="mdate"></param>
    ''' <param name="datecomponent">give letter YY for year, MM for Month, DD for day defualt value = AA returns DD/MM/YYYY</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDateComponent(ByRef mdate As Date, Optional datecomponent As String = "ALL") As String
        Dim returnvalue As String = 0
        Select Case datecomponent
            Case "YY"
                returnvalue = DatePart(DateInterval.Year, mdate)
            Case "MM"
                returnvalue = DatePart(DateInterval.Month, mdate)
            Case "DD"
                returnvalue = DatePart(DateInterval.Day, mdate)
            Case Else
                returnvalue = DatePart(DateInterval.Day, mdate).ToString & "/" & DatePart(DateInterval.Month, mdate).ToString & "/" & DatePart(DateInterval.Year, mdate).ToString
        End Select
        returnvalue = returnvalue
        Return returnvalue
    End Function
   
    ''' <summary>
    '''  gives time component from date
    ''' </summary>
    ''' <param name="mdate"></param>
    ''' <param name="timecomponebt">give letter H for hour, M for minute, S for second defualt value = AA returns HH:MM:SS</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetTimeComponentfromDate(ByRef mdate As Date, Optional timecomponebt As String = "all") As String
        Dim returnvalue As String = 0
        Select Case timecomponebt
            Case "H"
                returnvalue = DatePart(DateInterval.Hour, mdate)
            Case "M"
                returnvalue = DatePart(DateInterval.Minute, mdate)
            Case "S"
                returnvalue = DatePart(DateInterval.Second, mdate)
            Case Else
                returnvalue = DatePart(DateInterval.Hour, mdate).ToString & ":" & DatePart(DateInterval.Minute, mdate).ToString & ":" & DatePart(DateInterval.Second, mdate).ToString
        End Select
        Return returnvalue
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

 
    ''If null get a different value passed as a second parameter else return the same value as first parameter
   
 
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
    Public Shared Function DaysFromFirstSunday(ByRef mdate As Date) As String
     '   Dim returnvalue As String = ""
     '   returnvalue = DateDiff("d", FirstSundayOfMonth(mdate), mdate) - 7 
     '   Return returnvalue
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
        ElseIf mFirstDaysName = "Tuesday" Then
			DaysToMinus = 0
		ElseIf mFirstDaysName = "Wednesday" Then
            DaysToMinus = 0
		ElseIf mFirstDaysName = "Thursday" Then
            DaysToMinus = 0
		ElseIf mFirstDaysName = "Friday" Then
            DaysToMinus = 0
		ElseIf mFirstDaysName = "Saturday" Then
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
		If mdaysfromsunday < 0 Then
		  mdaysfromsunday = 7 + mdaysfromsunday
		else if mdaysfromsunday = 7 then 
		  mdaysfromsunday = 7 + mdaysfromsunday
		else if mdaysfromsunday < 7 then 
		  mdaysfromsunday =  mdaysfromsunday
		  else
		  mdaysfromsunday =  mdaysfromsunday
        End If
'		mdaysfromsunday = 7+1
        returnvalue = DateAdd("D", mdaysfromsunday, FirstSundayOfMonth("01/" & mMONTH & "/" & myear - 1))
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
            If "Sunday" = DaysNameFromDate(datemade) Then
                returnvalue = datemade
                Exit For
            End If
        Next
        Return returnvalue
    End Function
'***********************
	
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
    ''' <summary>
    ''' usage (1) Dim ddlday1 As New DropDownList:(2) ddlday1 = MiscClass.PopulateDay(myearfrom, monthto, ddlDay):(3) ddlDay = ddlday1
    ''' </summary>
    ''' <param name="MYEAR">year as integer like 2015</param>
    ''' <param name="MMONTH">month as ineger like 3 </param>
    ''' <param name="ddlday">ddlday as dropdownlist object and not just objectname </param>
    ''' <returns> dropdownlist with number of days in the selected month of the selected year</returns>
    Shared Function PopulateDay(MYEAR As Integer, MMONTH As Integer, ddlday As DropDownList) As DropDownList
        ddlday.Items.Clear()
        Dim lt As ListItem = New ListItem
        lt.Text = "DD"
        lt.Value = "0"
        ddlday.Items.Add(lt)
        '        Response.Write(SelectMonth.SelectedValue)
        Dim days As Integer = Date.DaysInMonth(MYEAR, MMONTH)
        Dim i As Integer = 1
        Do While (i <= days)
            lt = New ListItem
            lt.Text = i.ToString
            lt.Value = i.ToString
            ddlday.Items.Add(lt)
            i = (i + 1)
        Loop
        Return ddlday
    End Function
	    ''' <summary>
    ''' returns everyday between two dates
    ''' </summary>
    ''' <param name="StartDate">date format</param>
    ''' <param name="EndDate">date format</param>
    ''' <param name="DayInterval">Optional integer defualt value = 1 </param>
    ''' <returns>List(of datetime) : eg:  Dim dt As DataTable = DateClass.GetEachDayBtweenDates("01/01/2016", "02/02/2016"); GridView1.DataSource = dt </returns>
    Shared Function GetEachDayBtweenDates(StartDate As DateTime, EndDate As DateTime, Optional DayInterval As Integer = 1, Optional ColumName As String = "DateList") As DataTable
        Dim retuenvalue As DataTable
        'Dim StartDate As New DateTime(2009, 3, 10)
        'Dim EndDate As New DateTime(2009, 3, 26)
        'Dim DayInterval As Integer = 3

        Dim dateList As New DataTable
        dateList.Columns.Add(ColumName)
        Dim addinterval As Integer = 0
        While StartDate.AddDays(addinterval) <= EndDate
            StartDate = StartDate.AddDays(addinterval)
            dateList.Rows.Add(StartDate)
            addinterval = DayInterval
        End While
        retuenvalue = dateList
        Return retuenvalue
    End Function
    Shared Function AddZeroBeforeNumber(NUMBER As String) As String
        If NUMBER < 10 Then
            NUMBER = "0" & NUMBER
        End If
        Return NUMBER
    End Function
    Shared Function GetZeroBeforeNumber(NUMBER As String) As String
        If NUMBER < 10 Then
            NUMBER = "0" & NUMBER
        End If
        Return NUMBER
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="TimeString"></param>
    ''' <param name="timepart"> use "Sec", "Min", "Hour" </param>
    ''' <returns></returns>
    Shared Function gettimepart(TimeString As String, Optional timepart As String = "ALL") As String
        Dim returnvalue As String = True
        Select Case timepart
            Case "Sec"
                returnvalue = Mid(TimeString, 7, 2)
            Case "Min"
                returnvalue = Mid(TimeString, 4, 2)
            Case "Hour"
                returnvalue = Mid(TimeString, 1, 3)
            Case Else
                returnvalue = TimeString
        End Select
        ' If TimeOfDay Then
        Return returnvalue
    End Function
	 Public Shared Function timetaken(starttime As String, endtime As String) As String
        Dim dt1 As DateTime = Convert.ToDateTime(starttime)
        Dim dt2 As DateTime = Convert.ToDateTime(endtime)
        Dim ts As TimeSpan = dt2.Subtract(dt1)
        Return ts.ToString
    End Function
    Public Shared Function GetDateTimeDifference(starttime As String, endtime As String) As String
        Dim dt1 As DateTime
        Dim dt2 As DateTime
        Dim ts As TimeSpan
        Dim returnvalue As String
        Try
            dt1 = Convert.ToDateTime(starttime)
            dt2 = Convert.ToDateTime(endtime)
            ts = dt2.Subtract(dt1)
            returnvalue = ts.ToString
        Catch ex As Exception
            returnvalue =  starttime
        End Try
        Return returnvalue
    End Function
	
    ''' <summary>
    ''' use following line with required data in _LoadComplete like DateClass.GetYearsList(DDL_Year, 2006, myear) use it before the ddl_selected valuse is changed
    ''' </summary>
    ''' <param name="ddl_Year"></param>
    ''' <param name="StartingYear">starting year value like 2006, else it will take it 10 years back</param>
    ''' <param name="selectedyear">if no value is given it will take current year as default</param>
    ''' <returns>drop down list of years as text ans year as value of the dropdownlist name provided</returns>
    Public Shared Function GetYearsList(ddl_Year As DropDownList, Optional StartingYear As Integer = 0, Optional selectedyear As Integer = 0) As DropDownList
        ddl_Year.Items.Clear()
        Dim lt As ListItem = New ListItem
        Dim currentyear As Integer = Today.Year
        If selectedyear = 0 Then selectedyear = currentyear
        If StartingYear = 0 Then StartingYear = currentyear - 10
        Dim YearListOPtion As Integer = currentyear + 1
        For i = StartingYear To (currentyear + 1)
			lt = New ListItem
            lt.Text = YearListOPtion.ToString
            lt.Value = YearListOPtion.ToString
            If YearListOPtion = selectedyear Then lt.Selected = True
            ddl_Year.Items.Add(lt)
            YearListOPtion = YearListOPtion - 1
        Next
        'List<Order> SortedList = objListOrder.OrderBy(o=>o.OrderDate).ToList();
        Dim listItems As List(Of String) = ddl_Year.Items.Cast(Of ListItem)().Select(Function(item) item.Text).ToList()
        'listItems.Sort(Function(a, b) String.Compare(a, b))
        ddl_Year.DataSource = listItems
        ddl_Year.DataBind()

        Return ddl_Year
    End Function
    
	''' <summary>
    ''' use following line with required data in _LoadComplete like DateClass.GetYearsList(2006, today.year.tostring)
    ''' </summary>
    ''' <param name="StartingYear">starting year value like 2006, else it will take it 10 years back</param>
    ''' <param name="selectedyear">if no value is given it will take current year as default</param>
    ''' <returns></returns>
    Public Shared Function GetYearsListV2(Optional StartingYear As Integer = 0, Optional selectedyear As Integer = 0) As DropDownList
        Dim ddl_Year As New DropDownList
        ddl_Year.Items.Clear()
        Dim lty As ListItem = New ListItem
        Dim currentyear As Integer = Today.Year
        If selectedyear = 0 Then selectedyear = currentyear
        If StartingYear = 0 Then StartingYear = currentyear - 10
        For i = StartingYear To (currentyear + 1)
            lty = New ListItem
            lty.Text = i.ToString
            lty.Value = i.ToString
            If i = selectedyear Then lty.Selected = True
            ddl_Year.Items.Add(lty)
        Next
        'List<Order> SortedList = objListOrder.OrderBy(o=>o.OrderDate).ToList();
        Dim listItems As List(Of String) = ddl_Year.Items.Cast(Of ListItem)().Select(Function(item) item.Text).ToList()
        listItems.Sort(Function(a, b) String.Compare(a, b))
        ddl_Year.DataSource = listItems
        ddl_Year.DataBind()

        Return ddl_Year
    End Function	
	
	
  Shared Function AddHoursToDateTime(ByRef TimeString As String, hours As Double) As String
        Dim returnvalue As String
        Dim dt1 As DateTime = Convert.ToDateTime(TimeString)
        dt1.AddHours(hours)
        returnvalue = dt1.ToString
        Return returnvalue
    End Function
	
	''' <summary>
    ''' takes string and converts it into datetime
    ''' </summary>
    ''' <param name="datestring">like "27062016112100" for mat DDMMYYYHHMMSS</param>
    ''' <returns>27/06/2016 11:21:00 in datetime format</returns>
	Shared Function ConvertToDateTime(datestring As String) As DateTime
        Dim returnvalue As DateTime
        Dim day As String = Mid(datestring, 1, 2)
        Dim month As String = Mid(datestring, 3, 2)
        Dim year As String = Mid(datestring, 5, 4)
        Dim hours As String = Mid(datestring, 9, 2)
        Dim minutes As String = Mid(datestring, 11, 2)
        Dim seconds As String = Mid(datestring, 13, 2)
        returnvalue = Convert.ToDateTime(day & "/" & month & "/" & year & " " & hours & ":" & minutes & ":" & seconds)
        Return returnvalue
    End Function
	
	    ''' <summary>
    ''' returns string value in three letter
    ''' </summary>
    ''' <param name="dateToProcess"> Dim mdaqte As DateTime = "02-dec-1962"</param>
    ''' <returns> returns string value in three letter in capital like MON</returns>
    Shared Function GetDayName(dateToProcess As DateTime) As String
        Dim returnvalue As String = dateToProcess.ToString("ddd", CultureInfo.InvariantCulture).ToUpper().ToString
        Return returnvalue
    End Function
	
	    ''' <summary>
    '''  usage If DateClass.isMyDay(Now,"SATSUN") Then
    ''' </summary>
    ''' <param name="datetocheck"></param>
    ''' <param name="DayNamesToCheck"> defualt value is "SATSUN"</param>
    ''' <returns>true or false</returns>
    Shared Function isMyDay(datetocheck As DateTime, Optional DayNamesToCheck As String = "SATSUN") As Boolean
        Dim returnvalue As Boolean = False
        If InStr(UCase(DayNamesToCheck), UCase(DateClass.GetDayName(datetocheck))) > 0 Then returnvalue = True
        Return returnvalue
    End Function
	 Shared Function GetDateMonthsBack(mYear As String, mMonth As String, Optional mday As String = "01", Optional mMonthsBack As String = "-1") As DateTime
        Dim returnvalue As DateTime
        If mYear = "" Then mYear = Now.Year.ToString : LastLine = 572
        Dim DateString As String = MiscClass.addpadding(mday, 2, "0") & MiscClass.addpadding(mMonth, 2, "0") & mYear & "000001" : LastLine = 573 'like "27062016112100" for mat DDMMYYYYHHMMSS
        Dim day As String = Mid(DateString, 1, 2)
        Dim month As String = Mid(DateString, 3, 2)
        Dim year As String = Mid(DateString, 5, 4)
        Dim hours As String = Mid(DateString, 9, 2)
        Dim minutes As String = Mid(DateString, 11, 2)
        Dim seconds As String = Mid(DateString, 13, 2)
        Try : LastLine = 580
            returnvalue = Convert.ToDateTime(day & "/" & month & "/" & year & " " & hours & ":" & minutes & ":" & seconds) : LastLine = 581
            'HttpContext.Current.Response.Write("returnvalue=" & returnvalue & "<br>")
            returnvalue = returnvalue.AddMonths(mMonthsBack) : LastLine = 582
        Catch ex As Exception
            HttpContext.Current.Response.Write("GetDateMonthsBack = " & ex.Message.ToString)
            HttpContext.Current.Response.Write("<br>mYear=" & mYear & ", mMonth=" & mMonth & ", Mday=" & mday & ", mMonthsBack=" & mMonthsBack)
            HttpContext.Current.Response.Write("<br>DateString = " & DateString)
            HttpContext.Current.Response.Write("<br> LastLine=" & LastLine & "<br>")
        End Try

        Return returnvalue
    End Function
    Shared Function StringToDate(DateString As String, Optional FormatIn As String = "yyyymmdd") As Date
        'HttpContext.Current.Response.Write("<br>DateString = " & DateString)
        Dim rv As Date
        Dim str As String = DateString
        Try : LastLine = 597
            If ifitsYear(Mid(str, 1, 4)) Then
                FormatIn = "yyyymmdd"
            Else
                FormatIn = "ddmmyyyy"
            End If
            'HttpContext.Current.Response.Write("<br>StringToDate.DateString = " & DateString & "...FormatIn=" & FormatIn & ", ifitsYear(Mid(str, 1, 4))= " & ifitsYear(Mid(str, 1, 4)))
            Select Case FormatIn
                Case "yyyymmdd"
                    str = Mid(str, 7, 2) & "/" & Mid(str, 5, 2) & "/" & Mid(str, 1, 4)
                Case "ddmmyyyy"
                    str = Mid(str, 1, 2) & "/" & Mid(str, 3, 2) & "/" & Mid(str, 5, 4)
            End Select
            ' HttpContext.Current.Response.Write(", str=" & str) : LastLine = 610
            rv = Date.ParseExact(str, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) : LastLine = 611
            ' HttpContext.Current.Response.Write(", rv=" & rv.ToShortDateString) : LastLine = 610
        Catch ex As Exception
            HttpContext.Current.Response.Write("<br>DatClass.StringToDate Error = " & ex.Message.ToString)
            HttpContext.Current.Response.Write("<br>DateString = " & DateString)
            HttpContext.Current.Response.Write("<br>str = " & str)
            HttpContext.Current.Response.Write("<br> LastLine=" & LastLine & "<br>")

        End Try
        Return rv
    End Function

    Shared Function ifitsYear(st As String) As Boolean
        Dim rv As Boolean = False

        For i = 1900 To 2020
            If st = i Then
                rv = True
                Exit For

            End If
        Next

        Return rv
    End Function
	Shared Function StringToDateV2(StrDatetime As String) As DateTime
        Return DateTime.Parse(StrDatetime)
    End Function
	 Shared Function DateStringToDate(StrDate As String) As DateTime
        Dim rv As DateTime = Date.ParseExact(StrDate, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        Return rv
    End Function
	''' <summary>
    ''' adds or subtracts values from a date value
    ''' </summary>
    ''' <param name="StartDate">StartDate in dd/mm/yyyy</param>
    ''' <param name="ChangeValue">Year, Month, Day</param>
    ''' <param name="ValuetoAdd">integer -10,,0,10 etc </param>
    ''' <returns></returns>
    Shared Function DateAddSubtract(StartDate As String, ChangeValue As String, ValuetoAdd As Integer) As Date
        Dim rv As Date
        Dim IntervalType As DateInterval
        Select Case LCase(ChangeValue)
            Case "year"
                IntervalType = DateInterval.Year
            Case "month"
                IntervalType = DateInterval.Month
            Case "day"
                IntervalType = DateInterval.Day
        End Select
        Dim mStartDate = CDate(StartDate)
        rv = DateAdd(IntervalType, ValuetoAdd, mStartDate)
        Return rv
    End Function
	Shared Function W3CToDateTime(w3cDateTime As String) As String
        w3cDateTime = Left(Replace(w3cDateTime, "T", " "), 19)
        Return w3cDateTime
    End Function
    Shared Function GetW3CFormatToDateTime(DateTime As DateTime) As String
        Dim rv As String = ""
        rv = DateTime.ToString("yyyy-MM-ddTHH:mm:ss+ff:00")
        Return rv
    End Function
    Shared Function GetDateTimeFromW3CFormat(DateTime As DateTime) As String
        Dim rv As String = ""
        rv = DateTime.ToString("yyyy-MM-dd HH:mm:ss")
        Return rv
    End Function
    Shared Function GetW3CFormatFromDateTime(DateTime As DateTime) As String
        Dim rv As String = ""
        rv = DateTime.ToString("yyyy-MM-ddTHH:mm:ss+ff:00")
        Return rv
    End Function
End Class
