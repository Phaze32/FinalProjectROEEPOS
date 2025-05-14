
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic

Public Class faststring
    '// MODULE VARIABLES
    Dim m_sArr            '// Array holding the strings.
    Dim m_lResize        '// Factor for rezising the array.
    Dim m_lStrings       '// Number of strings appended.
    Private _count As Integer
    '// PROPERTIES
    Public Property NumberOfStrings() As Integer
        Get
            Return _count
        End Get
        Set(ByVal m_lStrings As Integer)
            _count = m_lStrings
        End Set
    End Property
    Public Property NumberOfBytes() As Integer
        Get
            Return _count
        End Get
        Set(ByVal m_lStrings As Integer)
            _count = Len(Join(m_sArr, ""))
        End Set
    End Property
    Public Property NumberOfCharacters() As Integer
        Get
            Return _count
        End Get
        Set(ByVal m_lStrings As Integer)
            _count = Marshal.SizeOf(Join(m_sArr, ""))
        End Set
    End Property

    '------------------------------------------------------------------------------------------------------------
    ' Comment: Initialize default values.
    '------------------------------------------------------------------------------------------------------------
    Private Sub Class_Initialize()
        m_lResize = CLng(50)
        m_lStrings = CLng(0)
        ReDim m_sArr(m_lResize)
    End Sub

    '------------------------------------------------------------------------------------------------------------
    ' Comment: Add a new string to the string array.
    '------------------------------------------------------------------------------------------------------------
    Public Sub Append(sNewString)

        If Len(sNewString & "") = 0 Then Exit Sub

        '// If we have filled the array, resize it.
        If m_lStrings > UBound(m_sArr) Then ReDim Preserve m_sArr(UBound(m_sArr) + m_lResize)

        '// Append the new string to the next unused position in the array.
        m_sArr(m_lStrings) = sNewString
        m_lStrings = (m_lStrings + 1)
    End Sub

    '------------------------------------------------------------------------------------------------------------
    ' Comment: Return the strings as one big string.
    '------------------------------------------------------------------------------------------------------------
    Public Function ToString()
        ToString = Join(m_sArr, "")
    End Function

    '------------------------------------------------------------------------------------------------------------
    ' Comment: Reset everything.
    '------------------------------------------------------------------------------------------------------------
    Public Sub Reset()
        Class_Initialize()
    End Sub
End Class
