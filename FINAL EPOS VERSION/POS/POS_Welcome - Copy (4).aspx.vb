Imports System.Data
Partial Class POS_Welcome
    Inherits System.Web.UI.Page
    Public pin_code As String = ""
    Public TELL As String = "00"
    Public prod_disp_limit As String = "10"
    Public currency_prefix As String = "£"
    Public default_discount As String = "0"
    Public tax_rate As String = "0"
    Public rows_per_page As String = "20"
    Public logo_file As String = ""
    Public display_product As String = "Name"
    Public site_name As String = "site_name"

    Private Sub POS_Welcome_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub POS_Welcome_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Dim dt As DataTable = getsettindata()
        site_name = DTClass.SearchDT(dt, "Value", "Name", "site_name")
        pin_code = DTClass.SearchDT(dt, "Value", "Name", "pin_code")
        TELL = DTClass.SearchDT(dt, "Value", "Name", "tel")
        prod_disp_limit = DTClass.SearchDT(dt, "Value", "Name", "prod_disp_limit")
        currency_prefix = DTClass.SearchDT(dt, "Value", "Name", "currency_prefix")
        default_discount = DTClass.SearchDT(dt, "Value", "Name", "default_discount")
        tax_rate = DTClass.SearchDT(dt, "Value", "Name", "tax_rate")
        'rows_per_page = DTClass.SearchDT(dt, "Value", "Name", "rows_per_page")
        logo_file = DTClass.SearchDT(dt, "Value", "Name", "logo_file")
        display_product = DTClass.SearchDT(dt, "Value", "Name", "display_product")
        'DDL_rows_per_page.SelectedValue = rows_per_page
        'Literal1.Text = DTClass.ColumnsinDataTable(dt) & "pin_code=" & pin_code & "rows_per_page:" & rows_per_page
    End Sub

    Function getsettindata(Optional storid As String = "64") As DataTable
        Dim sqlstr As String = "SELECT [Id],[Name],[Value],[StoreId],[DateTimeStamp] FROM [DST].[dbo].[tblConfiguration] where [StoreId] = " & storid & " ;"
        Dim setdata As DataTable = SQLFunctions.GetDataTableFromSqlstr(sqlstr, "DSTConnectionString")
        Return setdata
        ' use ajax and getdata from another page
    End Function

End Class
