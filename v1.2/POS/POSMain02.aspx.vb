Imports System.Data.SqlClient
Imports System.Data
Imports System.Windows.Controls.Primitives
Partial Class POS_POSMain02
    Inherits System.Web.UI.Page
    Shared strsqlmain As String = "SELECT ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] WHERE (isnull([Description],'') <> '')"
    Shared ProductImageFolder As String = "../Images/POSImages/"
    Private Sub POSMain_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        'getpods(strsqlmain)
        'Dim dttest As DataTable = SumDTColumns
        Literal_OrderID.Text = "Order ID=" & Request.QueryString("TicketID")
    End Sub
    Private Sub getsettings()
        Dim strsql As String = "select configuration from tblPLU where"
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
    End Sub
    Private Sub getpods(strsql As String)
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
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
    'Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
    '        & "   where 1=1   And department ='d';"
    '    Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
    '    Literal1.Text = GetProductPod(dt)
    'End Sub
    'Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
    '    Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
    ' & "   where 1=1   and    department in ('L','W') ;"
    '    Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
    '    Literal1.Text = GetProductPod(dt)
    'End Sub
    'Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
    '    Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
    ' & "   where 1=1   and    department in ('r') ;"
    '    Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
    '    Literal1.Text = GetProductPod(dt)
    'End Sub
    'Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

    '    Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
    '            & "   where 1=1 and department not in ('r','d') and  department is not null;"
    '    Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
    '    Literal1.Text = GetProductPod(dt)
    'End Sub
    'Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
    '    Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
    '    & "   where 1=1 and  department is not null;"
    '    Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
    '    Literal1.Text = GetProductPod(dt)
    'End Sub

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

