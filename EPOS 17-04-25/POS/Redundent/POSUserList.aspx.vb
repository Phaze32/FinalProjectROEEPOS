Imports System.Data.SqlClient
Imports System.Data
Imports System.Windows.Controls.Primitives
Partial Class POS_PosUserList
    Inherits System.Web.UI.Page
    Shared strsqlmain As String = "SELECT ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] WHERE (isnull([Description],'') <> '')"
    Shared ProductImageFolder As String = "../Images/POSImages/"
    Private Sub POSMain_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        'getpods(strsqlmain)
        'Dim dttest As DataTable = SumDTColumns
        'Literal_OrderID.Text = "Order ID=" & Request.QueryString("TicketID")
    End Sub
    Private Sub getsettings()
        Dim strsql As String = "select configuration from tblPLU where"
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
    End Sub
    Private Sub getpods(strsql As String)
        Dim dt As DataTable = SQLFunctions.GetDataTableFromSqlstr(strsql, "DSTConnectionString")
        Response.Write(dt.Rows.Count())
        'Literal1.Text = GetProductPod(dt)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Dim strsql As String = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
        '& "   where 1=1 and department not in ('r','d') and  department is not null;"

        ' getpods(strsql)

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

