Imports Microsoft.VisualBasic
Imports System.Data
Imports System.IO
Public Class FileObjectClass
 ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="folderpath">folderpath from root folder</param>
    ''' <param name="Extensiontype">filters on file Extension default brings all type</param>
    ''' <param name="NameContains">Filter for the Name</param>
    ''' <returns>returns DataTable of List of Files in a folder</returns>
    Shared Function ReadFilesInFolderList(Optional folderpath As String = "c:\", Optional Extensiontype As String = "csv", Optional NameContains As String = "*") As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("File", GetType(String))
        dt.Columns.Add("size", GetType(String))
        dt.Columns.Add("extension", GetType(String))

        Dim di As New IO.DirectoryInfo(folderpath)
        Dim diar1 As IO.FileInfo() = di.GetFiles("*.*")
        Dim dra As IO.FileInfo
        'list the names of all files in the specified directory
        For Each dra In diar1
            If NameContains <> "*" Then
                If (dra.Name.Contains(NameContains)) Then
                    dt.Rows.Add(dra.Name, dra.Length, dra.Extension)
                End If
            Else
                dt.Rows.Add(dra.Name, dra.Length, dra.Extension)
            End If

        Next
        Return dt
    End Function
	
	    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="folderpath">folderpath from root folder</param>
    ''' <param name="Extensiontype">filters on file Extension default brings all type</param>
    ''' <param name="NameContains">Filter for the Name</param>
    ''' <param name="NameNotContains">Filter for the Name types to be exluded % cannot be part of the file Name</param>
    ''' <returns>returns DataTable of List of Files in a folder</returns>
    Shared Function ReadFilesInFolderListV2(Optional folderpath As String = "c:\", Optional Extensiontype As String = "csv", Optional NameContains As String = "*", Optional NameNotContains As String = "%") As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("File", GetType(String))
        dt.Columns.Add("size", GetType(String))
        dt.Columns.Add("extension", GetType(String))

        Dim di As New IO.DirectoryInfo(folderpath)
        Dim diar1 As IO.FileInfo() = di.GetFiles("*.*")
        Dim dra As IO.FileInfo
        'list the names of all files in the specified directory
        For Each dra In diar1
            If NameContains <> "*" Then
                If (dra.Name.Contains(NameContains)) Then
                    If Not dra.Name.Contains(NameNotContains) Then dt.Rows.Add(dra.Name, dra.Length, dra.Extension)
                End If
            Else
                If Not dra.Name.Contains(NameNotContains) Then dt.Rows.Add(dra.Name, dra.Length, dra.Extension)
            End If

        Next
        Return dt
    End Function
	
	
    ''' <summary>
    ''' returns list of files with size and extension in a datatable
    ''' </summary>
    ''' <param name="folderpath">folderpath from root folder</param>
    ''' <param name="Extensiontype"> filters on file Extension default brings all type</param>
    ''' <returns>datatable with file, size, extension</returns>
    Shared Function ReadFilesINOlderList_Old(Optional folderpath As String = "c:\", Optional Extensiontype As String = "*") As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("File", GetType(String))
        dt.Columns.Add("size", GetType(String))
        dt.Columns.Add("extension", GetType(String))

        Dim di As New IO.DirectoryInfo(folderpath)
        Dim diar1 As IO.FileInfo() = di.GetFiles("*." & Extensiontype)
        Dim dra As IO.FileInfo
        'list the names of all files in the specified directory
        For Each dra In diar1
            dt.Rows.Add(dra.Name, dra.Length, dra.Extension)
        Next
        Return dt
    End Function
    Shared Function CopyfileOnServer(FileToCopy As String, NewCopy As String) As String
        Dim returnvalue As String = "File Not Copied"
        If File.Exists(NewCopy) Then
            File.Delete(NewCopy)
        End If
        If File.Exists(FileToCopy) = True Then
            File.Copy(FileToCopy, NewCopy)
            returnvalue = ("File Copied")
        End If
        Return returnvalue
    End Function
	    ''' <summary>
    ''' returns list of files with size and extension in a datatable
    ''' </summary>
    ''' <param name="folderpath">folderpath from root folder</param>
    ''' <param name="Extensiontype"> filters on file Extension default brings all type</param>
    ''' <returns>datatable with file, size, extension</returns>
    Shared Function ReadFilesInFolderList(Optional folderpath As String = "c:\", Optional Extensiontype As String = "*") As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("File", GetType(String))
        dt.Columns.Add("size", GetType(String))
        dt.Columns.Add("extension", GetType(String))

        Dim di As New IO.DirectoryInfo(folderpath)
        Dim diar1 As IO.FileInfo() = di.GetFiles("*." & Extensiontype)
        Dim dra As IO.FileInfo
        'list the names of all files in the specified directory
        For Each dra In diar1
            dt.Rows.Add(dra.Name, dra.Length, dra.Extension)
        Next
        Return dt
    End Function
    
    ''' <summary>
    ''' moves a file from source to destination
    ''' </summary>
    ''' <param name="sourceDir">Source file name with folder path</param>
    ''' <param name="DestinationDir">New File name with flder path</param>
    ''' <returns></returns>
    Shared Function MovefileOnServer(sourceDir As String, DestinationDir As String) As String
        Try
            If File.Exists(DestinationDir) Then
                File.Delete(DestinationDir)
            End If
            If File.Exists(sourceDir) = True Then
                File.Move(sourceDir, DestinationDir)
            End If
            Return ("File Moved to " & DestinationDir)
        Catch ex As Exception
            Return "File Not Moved" & ex.Message.ToString
        End Try

    End Function
    Shared Function AppendTxtFileOnServer(TargetFile As String, TextToAppend As String) As String
        Try
            Dim returnvalue As String = "File Appended"

            If Not File.Exists(TargetFile) Then
                ' Create a file to write to.
                Using sw As StreamWriter = File.CreateText(TargetFile)
                    sw.WriteLine(TextToAppend)
                End Using
            Else
                Using sw As StreamWriter = File.AppendText(TargetFile)
                    sw.WriteLine(TextToAppend)
                End Using
            End If
            Return returnvalue
        Catch ex As Exception
            Return "File Not Appended" & ex.Message.ToString
        End Try

    End Function
    'Shared Function FileMoveOnServer(sourceFile As String, destinationFile As String) As String
    '    Dim Returnvalue As String = "false"
    '    Try
    '        ' To move a file or folder to a new location:
    '        File.Move(sourceFile, destinationFile)
    '        Returnvalue = destinationFile
    '    Catch ex As Exception

    '    End Try
    '    ' To move an entire directory. To programmatically modify or combine
    '    ' path strings, use the System.IO.Path class.
    '    'System.IO.Directory.Move("C:\Users\Public\public\test\", "C:\Users\Public\private")
    '    Return Returnvalue
    'End Function
	Shared Function FileCopyOnServer(fileName As String, sourcePath As String, targetPath As String) As String
        Dim Returnvalue As String = "false"
        'Dim fileName As String = "test.txt"
        'Dim sourcePath As String = "C:\Users\Public\TestFolder"
        'Dim targetPath As String = "C:\Users\Public\TestFolder\SubDir"

        ' Use Path class to manipulate file and directory paths.
        Dim sourceFile As String = Path.Combine(sourcePath, fileName)
        Dim destFile As String = Path.Combine(targetPath, fileName)


        If fileName <> "*.*" Then


            ' To copy a folder's contents to a new location:
            ' Create a new target folder, if necessary.
            If Not Directory.Exists(targetPath) Then
                Directory.CreateDirectory(targetPath)
            End If

            ' To copy a file to another location and 
            ' overwrite the destination file if it already exists.
            File.Copy(sourceFile, destFile, True)
            Returnvalue = ("Files copied to " & destFile)
        Else
            ' To copy all the files in one directory to another directory.
            ' Get the files in the source folder. (To recursively iterate through
            ' all subfolders under the current directory, see
            ' "How to: Iterate Through a Directory Tree.")
            ' Note: Check for target path was performed previously
            '       in this code example.
            If Directory.Exists(sourcePath) Then
                Dim files As String() = Directory.GetFiles(sourcePath)

                ' Copy the files and overwrite destination files if they already exist.
                For Each s As String In files
                    ' Use static Path methods to extract only the file name from the path.
                    fileName = Path.GetFileName(s)
                    destFile = Path.Combine(targetPath, fileName)
                    File.Copy(s, destFile, True)
                Next
                Returnvalue = ("All files copied.")
            Else
                Returnvalue = ("Source path does not exist!")
            End If
        End If
        ' Keep console window open in debug mode.
        Return Returnvalue
    End Function
    Private Shared Function FileCopyOnServer_Old(fileName As String, sourcePath As String, targetPath As String) As String
        Dim Returnvalue As String = "false"
        'Dim fileName As String = "test.txt"
        'Dim sourcePath As String = "C:\Users\Public\TestFolder"
        'Dim targetPath As String = "C:\Users\Public\TestFolder\SubDir"

        ' Use Path class to manipulate file and directory paths.
        Dim sourceFile As String = Path.Combine(sourcePath, fileName)
        Dim destFile As String = Path.Combine(targetPath, fileName)


        If fileName <> "*.*" Then


            ' To copy a folder's contents to a new location:
            ' Create a new target folder, if necessary.
            If Not Directory.Exists(targetPath) Then
                Directory.CreateDirectory(targetPath)
            End If

            ' To copy a file to another location and 
            ' overwrite the destination file if it already exists.
            File.Copy(sourceFile, destFile, True)
            Returnvalue = ("Files copied to " & destFile)
        Else
            ' To copy all the files in one directory to another directory.
            ' Get the files in the source folder. (To recursively iterate through
            ' all subfolders under the current directory, see
            ' "How to: Iterate Through a Directory Tree.")
            ' Note: Check for target path was performed previously
            '       in this code example.
            If Directory.Exists(sourcePath) Then
                Dim files As String() = Directory.GetFiles(sourcePath)

                ' Copy the files and overwrite destination files if they already exist.
                For Each s As String In files
                    ' Use static Path methods to extract only the file name from the path.
                    fileName = Path.GetFileName(s)
                    destFile = Path.Combine(targetPath, fileName)
                    File.Copy(s, destFile, True)
                Next
                Returnvalue = ("All files copied.")
            Else
                Returnvalue = ("Source path does not exist!")
            End If
        End If
        ' Keep console window open in debug mode.
        Return Returnvalue
    End Function
    Shared Function IfFileExists(DestinationDir As String) As Boolean
        Dim ReturnValue As Boolean = False
        If File.Exists(DestinationDir) Then
            ReturnValue = True
        Else
            ReturnValue = False
        End If
        Return ReturnValue
    End Function
	Shared Function movefiles(filename As String, Optional sourcefolderPath As String = "", Optional destinationfolderPath As String = "") As String
        Dim sourcefilepath As String = (sourcefolderPath & filename)
        Dim destinationfilepath As String = (destinationfolderPath & filename)
        ' HttpContext.Current.Response.Write("<br>sourcefilepath=" & sourcefilepath & "<br>" & destinationfilepath & "<br>" & filename & "<br>")
        Dim retuenvalue As String = FileObjectClass.MovefileOnServer(sourcefilepath, destinationfilepath)
        Return retuenvalue
    End Function
End Class
