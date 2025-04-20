Imports Microsoft.VisualBasic
Imports System.Globalization
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Web.HttpContext
Imports System.Object
Imports System.EventArgs
Imports System.Linq
Public Class GraphClass
    Public Shared dtMinValue As Date, dtMaxValue As Date
    Public Shared dataSpan As Integer
    Public Shared count As Integer = 0
    Public Shared colors As String() = New String() {"navy", "rgb(150,0,0)", "#ff9090", "#ff6060", "#ff0000", "#00ff00", "#99ff00", "#0000ff", "#0030ff", "#0060ff", "#0090ff", "maroon", "orange", "red", "blue", "Green", "Grey", "LightBlue", "LightGreen"}
    'HttpContext.Current.Response.Write(SqlString)
    'HttpContext.Current.Response.End()
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="fTitlet"></param>
    ''' <param name="fStart"></param>
    ''' <param name="fwidth"></param>
    ''' <param name="dateSpan"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function doGraphHorBar(fTitlet As Object, fStart As Object, fwidth As Object, dateSpan As Object) As String

        Dim title As String = DirectCast(fTitlet, String)
        Dim mBarWidth As Integer = fwidth
        Dim howWide As Integer = fwidth
        dateSpan = 40000
        howWide = If((howWide) > 0, (howWide * 100 / dateSpan), 0)
        Dim startDivAt As Integer = If((dateSpan - mBarWidth) > 0, ((mBarWidth) * 100 / dateSpan), 0)
        Dim TitleWide As Integer = title.Length
        count += 1

        Return "<div class=""gantt"" style=""background:" & colors(count Mod colors.Count()) & ";width:" & howWide & "%; padding:0px; color:white; float:left; clear:left;" & "%; text-align:left;""> &nbsp;</div>"

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="fwidth">Width of the bar</param>
    ''' <param name="fHeight">Height of the bar</param>
    ''' <param name="dataSpan">Maximum range</param>
    ''' <param name="DivHeight">Height of the container of the bar</param>
    ''' <param name="DataFieldHeight">Height of the data field that is displayed below the bar like period</param>
    ''' <returns>returns the div that will </returns>
    ''' <remarks></remarks>
    Public Shared Function doGraphVerBar(fwidth As Object, fHeight As Object, dataSpan As Object, DivHeight As Object, DataFieldHeight As Object) As String

        ' Dim title As String = DirectCast(fTitlet, String)
        Dim mBarWidth As Integer = fHeight
        Dim howHigh As Integer = fHeight
        Dim howWide As Integer = fwidth
        'dataSpan = 40000
        howHigh = If((howHigh) > 0, (howHigh * 100 / dataSpan), 0)
        Dim startDivAt As Integer = If((dataSpan - mBarWidth) > 0, ((mBarWidth) * 100 / dataSpan), 0)
        'Dim TitleWide As Integer = title.Length
        count += 1
        Dim marginTop As Integer = (((100 - howHigh) * DivHeight) / 100) - DataFieldHeight
        Return "<div class=""gantt"" style=""background:" & colors(count Mod colors.Count()) & ";width:" & howWide & "px;margin-top:" & marginTop & "px;Height:" & howHigh & "%; padding:0px; color:white; " & "%; text-align:left; ""><font size='1'>" & Mid((fHeight / 1000), 1, 4) & "K</font></div>"

    End Function

    Public Shared Function GraphHeader() As String
        Dim GraphHeaderReturn As String
        GraphHeaderReturn = "<span style=""float:left;"">" & AccessDB_functions.getMinMaxvalue("App_Data\IMSStock.mdb", "CommsImportSale", "Period", "Min") & "</span><span style=""float:right;"">" & AccessDB_functions.getMinMaxvalue("App_Data\IMSStock.mdb", "CommsImportSale", "Period", "Max") & "</span>"
        Return GraphHeaderReturn
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ReturnLegendLineSVerBar
        Public Property AverageSaleValue As Integer
        Public Property MinTargetValue As Integer
        Public Property MaxTargetValue As Integer
        '        Public Property GraphBarHeight As Integer
        Public Property linelength As Integer
    End Class

    ''' <summary>
    '''     ''' Example for returning multiple values using object
    ''' Dim aaaaa As GraphClass.ReturnLegendLineValues = GraphClass.GetGraphLineValues("App_Data/ImsStock.mdb", "SaleComissionByMonthSummarylQry", "Sum(Sale) AS TotalSale, Count(Period) AS TotalMonths,Max(Sale) AS MaxSale, Min(sale) AS MinSale", 150)
    ''' Label14.Text = aaaaa.AverageSaleValue.ToString
    ''' </summary>
    ''' <param name="FDbname">Database name with the folder like "App_Data/ImsStock.mdb"</param>
    ''' <param name="TableName">Table of query name</param>
    ''' <param name="fFieldName">Field name</param>
    ''' <param name="GraphBarHeight"></param>
    ''' <param name="MaxTargetValue"></param>
    ''' <param name="BarWidth"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetGraphLinesVerBar(ByRef FDbname As String, ByRef TableName As String, ByRef fFieldName As String, ByRef GraphBarHeight As Integer, ByRef MaxTargetValue As Integer, ByRef BarWidth As Integer) As ReturnLegendLineSVerBar
        Dim returnValue As New ReturnLegendLineSVerBar
        returnValue.AverageSaleValue = 10
        returnValue.MinTargetValue = 322
        returnValue.MaxTargetValue = 4444
        '       returnValue.GraphBarHeight = 150
        returnValue.linelength = 475

        Dim AverageSale As Integer = AccessDB_functions.getMinMaxvalue(FDbname, TableName, fFieldName, "Avg")
        Dim Minsale As Integer = AccessDB_functions.getMinMaxvalue(FDbname, TableName, fFieldName, "Min")
        Dim Maxsale As Integer = AccessDB_functions.getMinMaxvalue(FDbname, TableName, fFieldName, "Max")
        Dim linelength As Integer = 475
        Dim NumberOfPeriods As Integer = AccessDB_functions.getMinMaxvalue(FDbname, TableName, "Period", "Count")
        Dim NextRoundTO As Integer = 5000
        linelength = NumberOfPeriods * BarWidth

        If MaxTargetValue > Maxsale Then
            dataSpan = Val(AccessDB_functions.RoundToNext(MaxTargetValue, NextRoundTO))
        Else
            dataSpan = Val(AccessDB_functions.RoundToNext(Maxsale, NextRoundTO))
        End If

        returnValue.AverageSaleValue = 13 + GraphBarHeight - (AverageSale * GraphBarHeight) / dataSpan

        returnValue.MinTargetValue = 11 + GraphBarHeight - (Minsale * GraphBarHeight) / dataSpan

        returnValue.MaxTargetValue = 18 + GraphBarHeight - (MaxTargetValue * GraphBarHeight) / dataSpan

        returnValue.linelength = NumberOfPeriods * BarWidth
        'HttpContext.Current.Response.Write(returnValue.AverageSaleValue)
        'HttpContext.Current.Response.End()
        Return returnValue
    End Function

    Public Shared Function GetGraphLinesVerBarWithCriteria(ByRef FDbname As String, ByRef TableName As String, ByRef fFieldName As String, ByRef GraphBarHeight As Integer, ByRef MaxTargetValue As Integer, ByRef BarWidth As Integer, ByRef fSearchFieldName As String, ByRef fSearchFieldVale As String) As ReturnLegendLineSVerBar
        Dim returnValue As New ReturnLegendLineSVerBar
        returnValue.AverageSaleValue = 10
        returnValue.MinTargetValue = 322
        returnValue.MaxTargetValue = 4444
        '       returnValue.GraphBarHeight = 150
        returnValue.linelength = 475

        Dim AverageSale As Integer = AccessDB_functions.GetMinMaxValueWithCriteria(FDbname, TableName, fFieldName, "Avg", fSearchFieldName, fSearchFieldVale)
        Dim Minsale As Integer = AccessDB_functions.GetMinMaxValueWithCriteria(FDbname, TableName, fFieldName, "Min", fSearchFieldName, fSearchFieldVale)
        Dim Maxsale As Integer = AccessDB_functions.GetMinMaxValueWithCriteria(FDbname, TableName, fFieldName, "Max", fSearchFieldName, fSearchFieldVale)
        Dim linelength As Integer = 475
        Dim NumberOfPeriods As Integer = AccessDB_functions.GetMinMaxValueWithCriteria(FDbname, TableName, "Period", "Count", fSearchFieldName, fSearchFieldVale)
        Dim NextRoundTO As Integer = 5000
        linelength = NumberOfPeriods * BarWidth

        If MaxTargetValue > Maxsale Then
            dataSpan = Val(AccessDB_functions.RoundToNext(MaxTargetValue, NextRoundTO))
        Else
            dataSpan = Val(AccessDB_functions.RoundToNext(Maxsale, NextRoundTO))
        End If

        returnValue.AverageSaleValue = 13 + GraphBarHeight - (AverageSale * GraphBarHeight) / dataSpan

        returnValue.MinTargetValue = 11 + GraphBarHeight - (Minsale * GraphBarHeight) / dataSpan

        returnValue.MaxTargetValue = 18 + GraphBarHeight - (MaxTargetValue * GraphBarHeight) / dataSpan

        returnValue.linelength = NumberOfPeriods * BarWidth
        'HttpContext.Current.Response.Write(returnValue.AverageSaleValue)
        'HttpContext.Current.Response.End()
        Return returnValue
    End Function
End Class
