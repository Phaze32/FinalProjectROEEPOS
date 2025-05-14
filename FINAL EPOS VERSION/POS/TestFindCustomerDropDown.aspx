<%@ Page Language="VB" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Text" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<script runat="server">
    <System.Web.Script.Services.ScriptMethod(), System.Web.Services.WebMethod()>
    Shared Function SearchCustomers(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim SearchTextList As List(Of String) = New List(Of String)
        Try

            Dim sqlstr1 As String = ""
            Dim sqlstr2 As String = ""
            'change searchfield to chose what to search
            Dim serachfield As String = "Surname"
            Dim serachfield2 As String = "CustomerAccNo"

            '' Use the Search query based on the search table
            sqlstr1 = "Select ProductImage,Description,round(price,2) price,PLUID FROM [tblPLU] " _
     & " where isnull(Description,'') <> '' and  (Description Like '%' + Replace(Replace(@SearchText, '  ', ' '), ' ', '%')  + '%') ;"
            sqlstr2 = "Select [CustomerAccNo],[Title],[Forename],[Surname],[Company],[AddressName],[AddressStreet],[AddressTown],[AddressCounty][Postcode],[NothingByPost],[Telephone] " _
                & "FROM [DST].[dbo].[tblCustomer] where isnull(Forename,'') <> '' " _
                & " and ([Surname] Like '%' + Replace(Replace(@SearchText, '  ', ' '), ' ', '%')  + '%');"

            sqlstr1 = "Select [CustomerAccNo],[Forename],[Surname],[Telephone],(replace(trim([Forename]),'-','') +','+[Surname] + [CustomerAccNo] ) as name  From [DST].[dbo].[tblCustomer] " _
        & "Where isnull(CustomerAccNo,'') <> '' and ([Surname] Like '%' + Replace(Replace(@SearchText, '  ', ' '), ' ', '%')  + '%')"

            Dim conn As SqlConnection = New SqlConnection
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("DSTConnectionString").ConnectionString
            Dim cmd As SqlCommand = New SqlCommand
            prefixText = Replace(Replace(prefixText, "  ", " "), " ", "%")
            cmd.CommandText = sqlstr1
            cmd.Parameters.AddWithValue("@SearchText", prefixText)
            cmd.Connection = conn
            conn.Open()


            Dim sdr As SqlDataReader = cmd.ExecuteReader
            While sdr.Read
                SearchTextList.Add(sdr(serachfield).ToString & "--" & sdr(serachfield2).ToString)
            End While
            conn.Close()
        Catch ex As Exception
        End Try
        Return SearchTextList
    End Function

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
<div class="form-group" style="margin-bottom:5px;">

<asp:TextBox type="text" name="codeq" id="txtSearch"  class="form-control" style="z-index:2000!important;" placeholder="Search name of service"  runat="server" ></asp:TextBox>
<cc1:autocompleteextender ServiceMethod="SearchCustomers"
    MinimumPrefixLength="2"
    CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
    TargetControlID="txtSearch"
    ID="AutoCompleteExtender1" runat="server" >
</cc1:autocompleteextender>
</div>
        </div>
    </form>
</body>
</html>
