<%@ Page Language="C#" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="HtmlAgilityPack" %>
<%@ Import Namespace="System.Dynamic" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Data" %>

<!DOCTYPE html>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        // test use
        var  queryString =  Request.QueryString.ToString();

        // Generate the SQL query
        var tableName = "[dbo].[tblCustomer]";
        var ConnectionString = "DSTConnectionString";
        var sqlquery = ConvertQueryStringToInsert(queryString, tableName);
        // test use end
        //sqlquery = "select top(10) * from " + tableName + ";";
        DataSet ds = ExecuteQuery(sqlquery, ConnectionString);
        //var columnNameString =   SQLFunctionsCS.GetSQLTableColumnNamesAsCsv(tableName) ;
        string columnNamesCsv = sqlquery; // DTClass.GetTableNames(ds) + DTClass.GetTableNamesInDataset(ds); //GetColumnNamesAsCsv( ConnectionString, tableName); ;
        testresponse.Text =  "!!>>>>>>>" + columnNamesCsv + "<<<<<<<<!!" ;

        //testresponse.Text = ConvertStringToHtml(modalHtml) ;

        // Set the current time
        currentTime.Text = DateTime.Now.ToString("HH:mm:ss");
        var qsticketid = Request.QueryString["ticketID"];
        // Set the ticket ID (from query string or default to 66)
        if (Request.QueryString["ticketID"] != null)
        {
            ticketID.Text = sqlquery + "&ticketID=" + qsticketid + "&queryString=" + queryString  ;
        }
        else
        {
            ticketID.Text = "66" + "queryString=" +">" + queryString + "sqlquery=" + sqlquery ;
        }
    }
    public static string ConvertQueryStringToInsert(string queryString, string tableName, string excludekey = "act")
    {
        var queryParameters = HttpUtility.ParseQueryString(queryString);
        var columns = new List<string>();
        var values = new List<string>();

        foreach (string key in queryParameters)
        {
        if (!key.Equals(excludekey, StringComparison.OrdinalIgnoreCase) && !key.Equals("_"))
            {
            columns.Add("[" + key + "]");
            values.Add("'" + queryParameters[key] + "'");
            }        
        }

        string columnsAndParameters = string.Join(", ", columns);
        string valuesString = string.Join(", ", values);

        string query = "INSERT INTO [" + tableName + "] (" + columnsAndParameters + ") VALUES (" + valuesString + ")";
        return query;
    }

    public static string GetColumnNamesAsCsv(string connectionString, string tableName)
    {
        var columnNames = new List<string>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TableName", tableName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columnNames.Add(reader["COLUMN_NAME"].ToString());
                    }
                }
            }
        }

        return string.Join(",", columnNames);
    }

    public static DataSet ExecuteQuery(string query, string connectionStringName = "DTSconnectionstring")
    {
        // Retrieve the connection string from the web.config file
        string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataSet dataSet = new DataSet();
                    try
                    {
                        connection.Open();
                        adapter.Fill(dataSet);
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                    return dataSet;
                }
            }
        }
    }

    //************************


    string modalHtml = @"<div class=""modal"" data-easein=""flipYIn"" id=""customerModal"" tabindex=""-1"" role=""dialog"" aria-labelledby=""cModalLabel"" aria-hidden=""true""> 
    <div class=""modal-dialog"">
        <div class=""modal-content"">
            <div class=""modal-header modal-primary"">
                <button type=""button"" class=""close"" data-dismiss=""modal"" aria-hidden=""true""><i class=""fa fa-times""></i></button>
                <h4 class=""modal-title"" id=""cModalLabel"">
                    Add Customer
                     </h4>
            </div>
            <form action=""Bond"" id=""customer-form"" method=""post"" accept-charset=""utf-8"">
                <input type=""hidden"" name=""spos_token"" value=""1479a37b6443485bdaaf75fb256d1484"" />
                <div class=""modal-body"">
                    <div id=""c-alert"" class=""alert alert-danger"" style=""display:none;""></div>
                    <div class=""row"">
                        <div class=""col-xs-12"">
                            <div class=""form-group"">
                                <label class=""control-label"" for=""code"">
                                    First Name
                                </label>
                                <input type=""text"" name=""name"" value="""" class=""form-control input-sm kb-text"" id=""cname"" />
                                <label class=""control-label"" for=""code"">
                                    Sur Name####
                                </label>
                                <input type=""text"" name=""sur_name"" value="""" class=""form-control input-sm kb-text"" id=""sname"" />
                            </div>
                        </div>
                    </div>
                    <div class=""row"">
                        <div class=""col-xs-6"">
                            <div class=""form-group"">
                                <label class=""control-label"" for=""cemail"">
                                    Email Address
                                </label>
                                <input type=""text"" name=""email"" value="""" class=""form-control input-sm kb-text"" id=""cemail"" />
                            </div>
                        </div>
                        <div class=""col-xs-6"">
                            <div class=""form-group"">
                                <label class=""control-label"" for=""phone"">
                                    Phone
                                </label>
                                <input type=""text"" name=""phone"" value="""" class=""form-control input-sm kb-pad"" id=""cphone"" />
                            </div>
                        </div>
                    </div>
                    <div class=""row"">
                        <div class=""col-xs-6"">
                            <div class=""form-group"">
                                <label class=""control-label"" for=""cf1""> 
                                    Address 1
                                </label>
                                <input type=""text"" name=""cf1_placeholder_Name"" value="""" class=""form-control input-sm kb-text"" id=""cf1"" />
                            </div>
                        </div>
                        <div class=""col-xs-6"">
                            <div class=""form-group"">
                                <label class=""control-label"" for=""cf2"">
                                    Address 2
                                </label>
                                <input type=""text"" name=""cf2"" value="""" class=""form-control input-sm kb-text"" id=""cf2"" />
                            </div>
                        </div>
                    </div>
                    <div class=""row"">
                        <div class=""col-xs-6"">
                            <div class=""form-group"">
                                <label class=""control-label"" for=""postcode"">
                                    Post Code
                                </label>
                                <input type=""text"" name=""postcode_placeholder_Name"" value="""" class=""form-control input-sm kb-text"" id=""postcode_placeholder_id"" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class=""modal-footer"" style=""margin-top:0;"">
                    <button type=""button"" class=""btn btn-default pull-left"" data-dismiss=""modal""> Close </button>
                    <button type=""button"" class=""btn btn-primary"" id=""add_customerQ"" onclick=""get_customer(this)""> Add Customer</button>
                </div>
                <div id=""result""> result </div>
            </form>
        </div>
    </div>
</div>";

    //   ********


</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Literal EnableViewState="false" ID="currentTime" runat="server" Text="Label"></asp:Literal> <br/>
            <asp:Literal EnableViewState="false" ID="ticketID" runat="server" Text="Label"></asp:Literal>  <br/>
            <asp:Literal EnableViewState="false" ID="testresponse" runat="server" Text="Label"></asp:Literal>
     <! -- form begins -->




            <!-- -->
  <div class="modal" data-easein="flipYIn" id="customerModal" tabindex="-1" role="dialog" aria-labelledby="cModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header modal-primary">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><i class="fa fa-times"></i></button>
                <h4 class="modal-title" id="cModalLabel">
                    Add Customer
                </h4>
            </div>
            <!-- Add Customer Form-->
            <form action="get_customer(this);" id="customer-form" method="post" accept-charset="utf-8">
                <input type="hidden" name="spos_token" value="1479a37b6443485bdaaf75fb256d1484" />
                <div class="modal-body">
                    <div id="c-alert" class="alert alert-danger" style="display:none;"></div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="code">
                                    First Name
                                </label>
                                <input type="text" name="name" value="" class="form-control input-sm kb-text" id="cname" />
                                <label class="control-label" for="code">
                                    Sur Name
                                </label>
                                <input type="text" name="sur_name" value="" class="form-control input-sm kb-text" id="sname" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-6">
                            <div class="form-group">
                                <label class="control-label" for="cemail">
                                    Email Address
                                </label>
                                <input type="text" name="email" value="" class="form-control input-sm kb-text" id="cemail" />
                            </div>
                        </div>
                        <div class="col-xs-6">
                            <div class="form-group">
                                <label class="control-label" for="phone">
                                    Phone
                                </label>
                                <input type="text" name="phone" value="" class="form-control input-sm kb-pad" id="cphone" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-6">
                            <div class="form-group">
                                <label class="control-label" for="cf1">
                                    Address 1
                                </label>
                                <input type="text" name="cf1" value="" class="form-control input-sm kb-text" id="cf1" />
                            </div>
                        </div>
                        <div class="col-xs-6">
                            <div class="form-group">
                                <label class="control-label" for="cf2">
                                    Address 2
                                </label>
                                <input type="text" name="cf2" value="" class="form-control input-sm kb-text" id="cf2" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-6">
                            <div class="form-group">
                                <label class="control-label" for="postcode">
                                    Post Code
                                </label>
                                <input type="text" name="postcode_placeholder_Name" value="" class="form-control input-sm kb-text" id="postcode_placeholder_id" />
                            </div>
                        </div>

                    </div>
                </div>
                <div class="modal-footer" style="margin-top:0;">
                    <button type="button" class="btn btn-default pull-left" data-dismiss="modal"> Close </button>
                    <button type="button" class="btn btn-primary" id="add_customerQ" onclick="get_customer(this)"> Add Customer</button>
                </div>
                <div id="result"> result </div>
            </form>
        </div>
    </div>
</div>
    <!-- eof form -->

        </div>
    </form>
</body>
</html>