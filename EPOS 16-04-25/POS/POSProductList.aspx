<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSProductList.aspx.vb" Inherits="POSProductList" %>

<%-- This is the main HTML document that shows the product list in SimplePOS --%>

<!DOCTYPE html>
<html>
<head>
    <%-- Set up page metadata and include required styles and scripts --%>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" charset="UTF-8">
    <title>Products | SimplePOS</title>
    <script type="text/javascript" src="../Scripts/jQuery-2.1.4.min.js"></script>

    <%-- Optional scripts and favicon --%>
    <%-- <script src="/cdn-cgi/apps/head/Bx0hUCX-YaUCcleOh3fM_NqlPrk.js"></script>--%>
    <%--<link rel="shortcut icon" href="/Images/POSImages/icons/icon.png" />--%>

    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />
    <link href="../Styles/pos.css" rel="stylesheet" />
</head>

<body class="skin-green fixed sidebar-mini">

<form id="form1" runat="server">

    <%-- ScriptManager enables JavaScript/Ajax features in ASP.NET --%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" ViewStateMode="Disabled"></asp:ScriptManager>  

    <div id="main-wrapper">
        <div class="wrapper rtl rtl-inv">
            <%-- Page Header with logo and nav bar --%>
            <header class="main-header">
                <a href="/pos/POS_welcome.aspx" class="logo">
                    <span class="logo-mini">POS</span>
                    <span class="logo-lg">Simple<b>POS</b></span>
                </a>
                <%-- Top navigation bar --%>
                <!-- #include virtual = "/POS/NavBarTop.html" -->
            </header>

            <%-- Left side menu --%>
            <!-- #include virtual = "/POS/leftmenu.html" -->

            <div class="content-wrapper">

                <%-- Page title and breadcrumbs --%>
                <section class="content-header">
                    <h1>Products</h1>
                    <ol class="breadcrumb">
                        <li><a href="/POS/POS_Welcome.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
                        <li class="active">Products</li>
                    </ol>
                </section>

                <%-- Alert area (initially hidden) --%>
                <div class="col-lg-12 alerts">
                    <div id="custom-alerts" style="display:none;">
                        <div class="alert alert-dismissable">
                            <div class="custom-msg"></div>
                        </div>
                    </div>
                </div>

                <div class="clearfix"></div>

                <%-- JavaScript: handles opening of product modal for editing, from grid view edit button --%>
                <script>
                    //following function opens up modal pop up to give detail of the product to edit and save
                    function openProductRecordModal(event) {
                        event.preventDefault();
                        const button = event.currentTarget;
                        const pluid = button.getAttribute('data-pluid');
                        console.log('Opening modal for PLUID:', pluid);
                        alert('Opening modal for PLUID:' + pluid);
                        //$('#ProductModal').modal('show'); 
                        //following function opens up modal
                        fetchProductData(pluid);
                    }
                </script>

         

                <%-- Table column styling --%>
                <style type="text/css">
                    .table td:first-child { padding: 1px; }
                    .table td:nth-child(6), .table td:nth-child(7), .table td:nth-child(8) { text-align: center; }
                    .table td:nth-child(9), .table td:nth-child(10) { text-align: right; }
                </style>

                <%-- Main content area showing products table --%>
                <section class="content">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header">
                                    <div class="dropdown pull-right">
                                        <button class="btn btn-primary" id="dLabel" type="button" data-toggle="dropdown">
                                            SimplePOS (POS) <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu" aria-labelledby="dLabel"></ul>
                                    </div>
                                    <h3 class="box-title">Please use the table below to navigate or filter the results.</h3>
                                </div>

                                <div class="box-body">
                                    <div class="table-responsive">

                                        <%-- Search bar to be used in future development --%>
                                        <asp:Table runat="server">
                                            <asp:TableFooterRow>
                                                <asp:TableCell ColumnSpan="12" CssClass="p0">
                                                    <input type="text" class="form-control b0" name="search_table" id="search_table" placeholder="Type & hit enter to search the table" style="width:100%;">
                                                </asp:TableCell>
                                            </asp:TableFooterRow>
                                        </asp:Table>

                                        <%-- GridView with product data --%>
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="PLUID" 
                                            DataSourceID="SqlDataSource1" AllowPaging="True" AllowSorting="True" PageSize="50">

                                            <%-- Define table columns --%>
                                            <Columns>
                                                <%-- Product image column --%>
                                                <asp:TemplateField HeaderText="Image">
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image1" Width="50px" Height="50px" runat="server" ImageUrl='<%# "/POS/images/POSImages/" & Eval("ProductImage")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- Other columns --%>
                                                <asp:BoundField DataField="PLUID" HeaderText="PLUID" ReadOnly="True" SortExpression="PLUID" />
                                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                                                <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                                                <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
                                                <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                                                <asp:BoundField DataField="QtySoldYTD" HeaderText="QtySoldYTD" SortExpression="QtySoldYTD" />
                                                <asp:BoundField DataField="ValueSoldYTD" HeaderText="ValueSoldYTD" SortExpression="ValueSoldYTD" />
                                                <asp:BoundField DataField="QtySold" HeaderText="QtySold" SortExpression="QtySold" />
                                                <asp:BoundField DataField="ValueSold" HeaderText="ValueSold" SortExpression="ValueSold" />
                                                <asp:BoundField DataField="Barcode" HeaderText="Barcode" SortExpression="Barcode" />
                                                <asp:BoundField DataField="CurrentStockQty" HeaderText="CurrentStockQty" SortExpression="CurrentStockQty" />
                                                <asp:BoundField DataField="EditedBy" HeaderText="EditedBy" SortExpression="EditedBy" />

                                                <%-- Edit button --%>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <button style="background-color: green; color: white; border: none; padding: 5px 10px; cursor: pointer; border-radius: 5px;"
                                                            onclick="openProductRecordModal(event); return false;" data-pluid='<%# Eval("PLUID") %>'>
                                                            Edit
                                                        </button>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- ASP.NET Select button --%>
                                                <asp:CommandField ShowSelectButton="True" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <%-- Modal for image display --%>
                                    <div class="modal fade" id="picModal" tabindex="-1" role="dialog">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal"><i class="fa fa-times"></i></button>
                                                    <button type="button" class="close mr10" onclick="window.print();"><i class="fa fa-print"></i></button>
                                                    <h4 class="modal-title" id="myModalLabel">title</h4>
                                                </div>
                                                <div class="modal-body text-center">
                                                    <img id="product_image" src="" alt="" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="clearfix"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>

            <%-- Page footer --%>
            <footer class="main-footer">
                <div class="pull-right hidden-xs">Version <strong>4.0.13</strong></div>
                Copyright &copy; 2025 SimplePOS. All rights reserved.
            </footer>
        </div>

        <%-- Extra modals and loading spinner --%>
        <div class="modal" data-easein="flipYIn" id="posModal"></div>
        <div class="modal" data-easein="flipYIn" id="myModal"></div>
        <div id="ajaxCall"><i class="fa fa-spinner fa-pulse"></i></div>
    </div>

    <%-- JavaScript config and UI setup --%>
    <script type="text/javascript">
        var base_url = '/pos/';
        var site_url = '/pos/';
        var Settings = {
            "site_name": "SimplePOS", "dateformat": "D j M Y", "timeformat": "h:i A", "language": "english",
            "theme": "default", "currency_prefix": "USD", "rows_per_page": "10"
        };

        $(window).load(function () {
            $('.mm_products').addClass('active');
            $('#products_index').addClass('active');
        });

        var lang = [];
        lang['code_error'] = 'Code Error';
        lang['r_u_sure'] = '<strong>Are you sure?</strong>';
        lang['no_match_found'] = 'No match found';
    </script>

    <%-- External JS includes --%>
    <script src="../scripts/libraries.min.js" type="text/javascript"></script>
    <script src="../scripts/scripts.min.js" type="text/javascript"></script>
    <script src="../scripts/pos.min.js" type="text/javascript"></script>

    <%-- SQL Data source for product GridView --%>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:DSTConnectionString %>" 
        SelectCommand="SELECT isnull([ProductImage],'CoverImage.png') ProductImage,[PLUID], [Description],  [Type], [Price], [Department], [QtySold], [QtySoldYTD], [ValueSoldYTD], [ValueSold], [Barcode], [CurrentStockQty], [EditedBy], [Category] FROM [tblPLU] where Description <> '' ">
    </asp:SqlDataSource>

    <%-- Modal form for editing products --%>
    <!-- #include virtual = "/POS/FormProductRecordModal.html" -->

</form>
</body>
</html>
