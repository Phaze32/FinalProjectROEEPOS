Imports Microsoft.VisualBasic
Imports System.Globalization
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Web.HttpContext
Imports System.IO
Public Class StyleDynamicClass
    ''' <summary>
    ''' Aligns provided value based on the selection
    ''' </summary>
    ''' <param name="value"> value that needs to be aligned has to sent as string</param>
    ''' <param name="align"> R = right, C = Center, J = Justify, L = Left, default = Left </param>
    ''' <param name="isbold"> Boolean True for bold False for nornal</param>
    ''' <returns> returns value wraped in <div> value </div> with selected perameters </returns>
    ''' <remarks>this function retuens value in string along with the format parameters of alighnment and text/value weight</remarks>
    Public Shared Function AlignDiv(ByRef value As String, ByRef align As String, ByRef isbold As Boolean) As String
        Dim returnvalue As String = ""
        align = UCase(align)
        If align = "R" Then
            align = "right"
        ElseIf align = "C" Then
            align = "center"
        ElseIf align = "J" Then
            align = "justify"
        Else
            align = "Left"
        End If

        If isbold = True Then
            returnvalue = "<div align='" & align & "' style='font-weight:bold' >" & value & "</div>"
        Else
            returnvalue = "<div align='" & align & "' >" & value & "Normal" & "</div>"
        End If
        ' HttpContext.Current.Response.Write(returnvalue)
        ' HttpContext.Current.Response.End()
        Return returnvalue

    End Function
End Class
