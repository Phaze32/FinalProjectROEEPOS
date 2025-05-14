Imports System.Data.SqlClient
Imports System.Data
Imports System.Windows.Controls.Primitives
Imports System.Drawing
Imports AIMLbot.AIMLTagHandlers
Partial Class POS_POSMain02
    Inherits System.Web.UI.Page
    Shared strsqlmain As String = "SELECT ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] WHERE (isnull([Description],'') <> '')"
    Shared ProductImageFolder As String = "../Images/POSImages/"
    Shared connestring As String = "Drycleaning_DBConnectionString" ' "DSTConnectionString" ' 
    Private Sub POSMain_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        ' Generate a new TicketID
        Dim newTicketid As String = getnewticketid()
        newTicketid += 1
        ' Update the Literal controls
        Literal_OrderID.Text = newTicketid
        Literal_TicketID.Text = newTicketid
        ' Response.Write("Literal_OrderID.Text=" & Literal_OrderID.Text & "LiteralLiteral_TicketID_OrderID.Text=" & Literal_TicketID.Text)

    End Sub
    Shared Function getnewticketid() As String
        Dim getMaxTicketIDQuery As String = "SELECT ISNULL(MAX(TicketID), 0) + 1 FROM [DST].[dbo].[tblTicket];"
        Dim newTicketID As String = SQLFunctions.GetMinMaxSumValueWithCriteriaWOConstr("tblTicket", "Ticketid", "Max", "1", "1", "DSTConnectionString")

        Return newTicketID
    End Function



    Public Function GetValueFromDatabase(ByVal sqlQuery As String, Optional ByVal connectionStringName As String = "DSTConnectionString") As String
        Dim result As String = String.Empty

        Try
            ' Get the connection string from configuration
            Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings(connectionStringName).ConnectionString

            ' Establish the SQL connection
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Create the SQL command
                Using command As New SqlCommand(sqlQuery, connection)
                    ' Execute the query and get the result
                    Dim queryResult As Object = command.ExecuteScalar()

                    ' Check if the result is not null and convert it to a trimmed string
                    If queryResult IsNot Nothing AndAlso queryResult IsNot DBNull.Value Then
                        result = queryResult.ToString().Trim()
                    End If
                End Using
            End Using

        Catch ex As Exception
            ' Handle exceptions (log or rethrow, depending on your needs)
            Throw New Exception("An error occurred while retrieving the value from the database: " & ex.Message, ex)
        End Try

        ' Return the result as a trimmed string
        Return result
    End Function
    Private Sub getsettings()
        Dim strsql As String = "Select configuration from tblPLU where"
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "Drycleaning_DBConnectionString")
    End Sub
    Private Sub getpods(strsql As String)
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "Drycleaning_DBConnectionString")
        Response.Write(dt.Rows.Count())
        Literal1.Text = GetProductPod(dt)
    End Sub
    'Private Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
    '    'There should be some checking done so that not all the errors
    '    'are cleared
    '    Context.ClearError()
    'End Sub
    Shared Function GetProductPod(dt As DataTable) As String
        Dim rv As String = "" '"<script type = ""text/javascript"" > Function() TestAlert() {alert(""Test ok!"");}</script>"
        rv += "<div style='float:left;width:100%;'>"
        'HttpContext.Current.Response.Write("###" & DTClass.GetDTColumnNames(dt))
        For Each row In dt.Rows
            Dim imagefile As String = "6988655f95602f9394f9315165f920fe.png"
            If row("ProductImage").ToString <> "" Then imagefile = row("ProductImage").ToString

            rv += "<button type=""button"" data-name=" & row("Description").ToString & " id=""" & "Butt_" & row("PLUID").ToString & """ " _
                & "  value='" & row("PLUID").ToString & "' class=""btn btn-both btn-flat ""  OnClick='myFunc(" & row("PLUID").ToString & ")'><span class=""bg-img"">" _
                & " <img src=""" & ProductImageFolder & imagefile & """ alt='" & row("Description").ToString & "' " _
                & "  style=""width: 100px; height: 100px;""></span>" _
               & "<span><span>" & row("Description").ToString & " " & MiscClass.myCurrencyFormatWithSymbol(row("price"), 2, "GBP") & "</span></span></button>"


            'rv += "<div stryle=""float:left""><asp: Button runat=""server"" type=""button"" data-name=" & row("Description").ToString & " id=""" & "Butt_" & row("PLUID").ToString & """ " _
            '    & "  value='" & "getproduct.aspx?product_id=" & row("PLUID").ToString & "' class=""btn btn-both btn-flat product"" OnClick='TestAlert()' ><span class=""bg-img"">" _
            '    & " <img src=""../Images/POSImages/" & imagefile & """ alt='" & row("Description").ToString & "' " _
            '    & "  style=""width: 100px; height: 100px;""></span>" _
            '   & "<span><span>" & row("Description").ToString & " " & MiscClass.myCurrencyFormatWithSymbol(row("price"), 2, "GBP") & "</span></span></button></div>"
        Next
        rv = rv & "</div>"
        Return rv
    End Function
    Protected Sub AddItem_Click(sender As Object, e As EventArgs)
        GETSEARCHLIST()
    End Sub

    Protected Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        GETSEARCHLIST()
    End Sub
    Sub GETSEARCHLIST()
        Dim searchtext As String = Trim(txtSearch.Text)
        Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
       & "   where 1=1 and description like ('%" & searchtext & "%');"

        getpods(strsql)
    End Sub
    <System.Web.Script.Services.ScriptMethod(), System.Web.Services.WebMethod()>
    Shared Function SearchCustomers(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sqlstr1 As String = ""

        sqlstr1 = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
             & " where isnull(Description,'') <> '' and  (Description Like '%' + Replace(Replace(@SearchText, '  ', ' '), ' ', '%')  + '%') ;"

        Dim conn As SqlConnection = New SqlConnection
        conn.ConnectionString = ConfigurationManager.ConnectionStrings("DSTConnectionString").ConnectionString
        Dim cmd As SqlCommand = New SqlCommand
        prefixText = Replace(Replace(prefixText, "  ", " "), " ", "%")
        cmd.CommandText = sqlstr1
        cmd.Parameters.AddWithValue("@SearchText", prefixText)
        cmd.Connection = conn
        conn.Open()
        Dim SearchTextList As List(Of String) = New List(Of String)
        Dim sdr As SqlDataReader = cmd.ExecuteReader
        While sdr.Read
            SearchTextList.Add(sdr("Description").ToString)
        End While
        conn.Close()
        Return SearchTextList
    End Function
    Shared Function SearchCustomers2(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sqlstr1 As String = ""

        sqlstr1 = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
             & " where isnull(Description,'') <> '' and  (Description Like '%' + Replace(Replace(@SearchText, '  ', ' '), ' ', '%')  + '%') ;"

        Dim conn As SqlConnection = New SqlConnection
        conn.ConnectionString = ConfigurationManager.ConnectionStrings("").ConnectionString
        Dim cmd As SqlCommand = New SqlCommand
        prefixText = Replace(Replace(prefixText, "  ", " "), " ", "%")
        cmd.CommandText = sqlstr1
        cmd.Parameters.AddWithValue("@SearchText", prefixText)
        cmd.Connection = conn
        conn.Open()
        Dim SearchTextList As List(Of String) = New List(Of String)
        Dim sdr As SqlDataReader = cmd.ExecuteReader
        While sdr.Read
            SearchTextList.Add(sdr("Description").ToString)
        End While
        conn.Close()
        Return SearchTextList
    End Function


    Protected Sub hold_ref_TextChanged(sender As Object, e As EventArgs)

    End Sub
    Protected Sub Button_Click(sender As Object, e As EventArgs)
        Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
        & "   where 1=1 and department not in ('r','d') and  department is not null;"

        getpods(strsql)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
        '& "   where 1=1 and department not in ('r','d') and  department is not null;"

        ' getpods(strsql)

    End Sub
    Private Sub addscrollbar()
        'Dim vScrollBar1 As ScrollBar = New VScrollBar()
        'vScrollBar1.Dock = DockStyle.Right
        'vScrollBar1.Scroll += Function(sender, e)
        '                          panel1.VerticalScroll.Value = vScrollBar1.Value

        '                      End Function

    End Sub

    'Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
    '    'txtBox.Text = (txtBox.Text & DateTime.Now.ToString)
    '    ' txtBox.Text = txtBox.Text + "ggggg"
    '    Literal1.Text = Literal1.Text + "ggggg" & TextCSVClass.CSVStringToQualifierSQLString("a,b,c")

    'End Sub
    Protected Sub GroupedAddToCartClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim sbSCseventscadd As System.Text.StringBuilder = New System.Text.StringBuilder
        sbSCseventscadd.Append(String.Format("<script type=""text/javascript"">"))
        sbSCseventscadd.Append(String.Format("s.events=""scAdd""""));", sbSCseventscadd.Append("</script>")))
        Literal1.Text = sbSCseventscadd.ToString
    End Sub
    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function
    Protected Overrides Sub SavePageStateToPersistenceMedium(viewState As Object)
        Session("_ViewState") = viewState
    End Sub

    Protected Overrides Sub OnPreInit(ByVal e As EventArgs)
        MyBase.OnPreInit(e)
        If Request.Browser.MSDomVersion.Major = 0 Then
            Response.Cache.SetNoStore()
        End If
    End Sub
End Class

