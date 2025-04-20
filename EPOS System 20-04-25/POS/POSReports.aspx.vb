Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports eBay.Service.Core.Soap
Imports System.Web.UI.WebControls

Partial Class POSReports
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'CreateButtons()

        End If
    End Sub

End Class
