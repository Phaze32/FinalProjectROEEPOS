<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSSalesList - Copy.aspx.vb" Inherits="POSSalesList" %>

<!DOCTYPE html>
<html>

<head>
     <style>
        /* Style the GridView table */
        .gridview-table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
            font-family: Arial, sans-serif;
        }

        .gridview-table th, .gridview-table td {
            padding: 10px;
            border: 1px solid #ddd;
            text-align: left;
        }

        .gridview-table th {
            background-color: #4CAF50;
            color: white;
            text-align: center;
        }

        .gridview-table tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        .gridview-table tr:hover {
            background-color: #ddd;
        }

        .gridview-table .right-align {
            text-align: right;
        }

        .gridview-table .center-align {
            text-align: center;
        }
    </style>

    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" charset="UTF-8">
    <title>Sales | SimplePOS</title>
    <script type="text/javascript" src="../Scripts/jQuery-2.1.4.min.js"></script>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />
    <link href="../Styles/pos.css" rel="stylesheet" />
</head>
<body class="skin-green fixed sidebar-mini">
<form id="form1" runat="server">
 <asp:ScriptManager ID="ScriptManager1" runat="server"  ViewStateMode="Disabled" ></asp:ScriptManager>
 <div id="main-wrapper">
        <div class="wrapper rtl rtl-inv">
            <header class="main-header">
                <a href="https://localhost/" class="logo">
                <span class="logo-mini">POS</span>
                <span class="logo-lg">Simple<b>POS</b></span>
                </a>
                <!-- #include virtual = "/POS/NavBarTop.html" -->
            </header>
            <!-- #include virtual = "/POS/leftmenu.html" -->
                <%--######--%>

<div class="content-wrapper">
<section class="content-header">
<h1>Sales</h1>
<ol class="breadcrumb">
<li><a href="https://spos.tecdiary.com/"><i class="fa fa-dashboard"></i> Home</a></li>
<li class="active">Sales</li> </ol>
</section>
<div class="col-lg-12 alerts">
<div id="custom-alerts" style="display:none;">
<div class="alert alert-dismissable">
<div class="custom-msg"></div>
</div>
</div>
</div>
<div class="clearfix"></div>
<script style="display: none !important;">!
    function (e, t, r, n, c, a, l) {
        function i(t, r) { return r = e.createElement('div'), r.innerHTML = '<a href="' + t.replace(/"/g, '&quot;') + '"></a>', r.childNodes[0].getAttribute('href') } function o(e, t, r, n) { for (r = '', n = '0x' + e.substr(t, 2) | 0, t += 2; t < e.length; t += 2)r += String.fromCharCode('0x' + e.substr(t, 2) ^ n); return i(r) } try { for (c = e.getElementsByTagName('a'), l = '/cdn-cgi/l/email-protection#', n = 0; n < c.length; n++)try { (t = (a = c[n]).href.indexOf(l)) > -1 && (a.href = 'mailto:' + o(a.href, t + l.length)) } catch (e) { } for (c = e.querySelectorAll('.__cf_email__'), n = 0; n < c.length; n++)try { (a = c[n]).parentNode.replaceChild(e.createTextNode(o(a.getAttribute('data-cfemail'), 0)), a) } catch (e) { } } catch (e) { }
    }(document);</script>
    <script type="text/javascript">
        $(document).ready(function () {

            function ptype(x) {
                if (x == 'standard') {
                    return 'Standard';
                } else if (x == 'combo') {
                    return 'Combo';
                } else if (x == 'service') {
                    return 'Service';
                } else {
                    return x;
                }
            }

            function image(n) {
                if (n !== null) {
                    return '<div style="width:32px; margin: 0 auto;"><a href="https://spos.tecdiary.com/uploads/' + n + '" class="open-image"><img src="https://spos.tecdiary.com/uploads/thumbs/' + n + '" alt="" class="img-responsive"></a></div>';
                }
                return '';
            }

            function method(n) {
                return (n == 0) ? '<span class="label label-primary">Inclusive</span>' : '<span class="label label-warning">Exclusive</span>';
            }

            var table = $('#prTables').DataTable({

                // 'ajax': 'https://spos.tecdiary.com/products/get_products/1',
                'ajax': {
                    url: 'https://spos.tecdiary.com/products/get_products/1', type: 'POST', "data": function (d) {
                        d.spos_token = "41b9555b0ce496326f24656c24dcc5b0";
                    }
                },
                "buttons": [
                    { extend: 'copyHtml5', 'footer': false, exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] } },
                    { extend: 'excelHtml5', 'footer': false, exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] } },
                    { extend: 'csvHtml5', 'footer': false, exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] } },
                    {
                        extend: 'pdfHtml5', orientation: 'landscape', pageSize: 'A4', 'footer': false,
                        exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] }
                    },
                    { extend: 'colvis', text: 'Columns' },
                ],
                "columns": [
                    { "data": "pid", "visible": false },
                    { "data": "image", "searchable": false, "orderable": false, "render": image },
                    { "data": "code" },
                    { "data": "pname" },
                    { "data": "type", "render": ptype },
                    { "data": "cname" },
                    { "data": "quantity" },
                    { "data": "tax" },
                    { "data": "tax_method", "render": method },
                    { "data": "cost", "searchable": false },
                    { "data": "price", "searchable": false },
                    { "data": "Actions", "searchable": false, "orderable": false }
                ]

            });

            // $('#prTables tfoot th:not(:last-child, :nth-last-child(2), :nth-last-child(3))').each(function () {
            //     var title = $(this).text();
            //     $(this).html( '<input type="text" class="text_filter" placeholder="'+title+'" />' );
            // });

            $('#search_table').on('keyup change', function (e) {
                var code = (e.keyCode ? e.keyCode : e.which);
                if (((code == 13 && table.search() !== this.value) || (table.search() !== '' && this.value === ''))) {
                    table.search(this.value).draw();
                }
            });

            table.columns().every(function () {
                var self = this;
                $('input', this.footer()).on('keyup change', function (e) {
                    var code = (e.keyCode ? e.keyCode : e.which);
                    if (((code == 13 && self.search() !== this.value) || (self.search() !== '' && this.value === ''))) {
                        self.search(this.value).draw();
                    }
                });
                $('select', this.footer()).on('change', function (e) {
                    self.search(this.value).draw();
                });
            });

        });
</script>
    <script type="text/javascript">
    $(document).ready(function () {
        $('#prTables').on('click', '.image', function () {
            var a_href = $(this).attr('href');
            var code = $(this).attr('id');
            $('#myModalLabel').text(code);
            $('#product_image').attr('src', a_href);
            $('#picModal').modal();
            return false;
        });
        $('#prTables').on('click', '.barcode', function () {
            var a_href = $(this).attr('href');
            var code = $(this).attr('id');
            $('#myModalLabel').text(code);
            $('#product_image').attr('src', a_href);
            $('#picModal').modal();
            return false;
        });
        $('#prTables').on('click', '.open-image', function () {
            var a_href = $(this).attr('href');
            var code = $(this).closest('tr').find('.image').attr('id');
            $('#myModalLabel').text(code);
            $('#product_image').attr('src', a_href);
            $('#picModal').modal();
            return false;
        });
    });
</script>
<style type="text/css">
    .table td:first-child { padding: 1px; }
    .table td:nth-child(6), .table td:nth-child(7), .table td:nth-child(8) { text-align: center; }
    .table td:nth-child(9), .table td:nth-child(10) { text-align: right; }
</style>
<section class="content">
<div class="row">
<div class="col-xs-12">
<div class="box box-primary">
<div class="box-header">
<div class="dropdown pull-right">
<button class="btn btn-primary" id="dLabel" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
SimplePOS (POS) <span class="caret"></span>
</button>
<ul class="dropdown-menu" aria-labelledby="dLabel">
</ul>
</div>
<h3 class="box-title">Please use the table below to navigate or filter the results.</h3>
</div>
<div class="box-body">
<div class="table-responsive">
    <table>
        <tr><td><asp:Button ID="Button1" runat="server" Text="Summary" CommandName="MakeSummary"  ></asp:Button>
           <asp:Button ID="Button4" runat="server" Text="Detail" CommandName="MakeSummary"  ></asp:Button></td></tr>
        <tr><td><asp:Button ID="Button2" runat="server" Text="Sale"></asp:Button>
            <asp:Button ID="Button3" runat="server" Text="Customer"></asp:Button>
            <asp:Button ID="Button5" runat="server" Text="Product"></asp:Button>
            </td></tr>

    </table>

    <asp:Table runat="server">
        <asp:TableFooterRow>
<asp:TableCell   ColumnSpan="12" CssClass="p0">

    <input type="text" class="form-control b0" name="search_table" id="search_table" placeholder="Type & hit enter to search the table" style="width:100%;"></td>
</asp:TableCell>
</asp:TableFooterRow>
    </asp:Table>

<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSource1" AllowPaging="True" AllowSorting="True" PageSize="100"
    CssClass="gridview-table">
    <Columns>
       <asp:BoundField DataField="TicketID" HeaderText="TicketID" SortExpression="TicketID" />
        <asp:BoundField DataField="Surname" HeaderText="Surname" SortExpression="Surname" />
        <asp:BoundField DataField="CustomerAccNo" HeaderText="CustomerAccNo" SortExpression="CustomerAccNo" />
        <asp:BoundField DataField="year" HeaderText="year" SortExpression="year" />
        <asp:BoundField DataField="month" HeaderText="month" SortExpression="month" />
        <asp:BoundField DataField="day" HeaderText="day" SortExpression="day" />
        <asp:BoundField DataField="TicketTotal" HeaderText="TicketTotal" SortExpression="TicketTotal">
            <ItemStyle CssClass="right-align" />
        </asp:BoundField>
        <asp:BoundField DataField="PiecesTotal" HeaderText="PiecesTotal" SortExpression="PiecesTotal">
            <ItemStyle CssClass="center-align" />
        </asp:BoundField>
 
    </Columns>
</asp:GridView>
    

</div>
<div class="modal fade" id="picModal" tabindex="-1" role="dialog" aria-labelledby="picModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><i class="fa fa-times"></i></button>
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
            
<footer class="main-footer">
<div class="pull-right hidden-xs">
Version <strong>4.0.13</strong>
</div>
Copyright &copy; 2017 SimplePOS. All rights reserved.
</footer>
</div>
<div class="modal" data-easein="flipYIn" id="posModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true"></div>
<div class="modal" data-easein="flipYIn" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"></div>
<div id="ajaxCall"><i class="fa fa-spinner fa-pulse"></i></div>
</div>
<script type="text/javascript">
    var base_url = 'https://spos.tecdiary.com/';
    var site_url = 'https://spos.tecdiary.com/';
    var dateformat = 'D j M Y', timeformat = 'h:i A';
    var Settings = { "logo": "logo1.png", "site_name": "SimplePOS", "tel": "0105292122", "dateformat": "D j M Y", "timeformat": "h:i A", "language": "english", "theme": "default", "mmode": "0", "captcha": "0", "currency_prefix": "USD", "default_customer": "3", "default_tax_rate": "5%", "rows_per_page": "10", "total_rows": "30", "header": "<h2><strong>Simple POS<\/strong><\/h2>\r\n       My Shop Lot, Shopping Mall,<br>\r\n                                                                                              Post Code, City<br>", "footer": "Thank you for your business!\r\n<br>", "bsty": "3", "display_kb": "0", "default_category": "1", "default_discount": "0", "item_addition": "1", "barcode_symbology": "", "pro_limit": "10", "decimals": "2", "thousands_sep": ",", "decimals_sep": ".", "focus_add_item": "ALT+F1", "add_customer": "ALT+F2", "toggle_category_slider": "ALT+F10", "cancel_sale": "ALT+F5", "suspend_sale": "ALT+F6", "print_order": "ALT+F11", "print_bill": "ALT+F12", "finalize_sale": "ALT+F8", "today_sale": "Ctrl+F1", "open_hold_bills": "Ctrl+F2", "close_register": "ALT+F7", "java_applet": "0", "receipt_printer": "", "pos_printers": "", "cash_drawer_codes": "", "char_per_line": "42", "rounding": "1", "pin_code": "abdbeb4d8dbe30df8430a8394b7218ef", "purchase_code": null, "envato_username": null, "theme_style": "green", "after_sale_page": null, "overselling": "1", "multi_store": "1", "qty_decimals": "2", "symbol": null, "sac": "0", "display_symbol": null, "remote_printing": "1", "printer": null, "order_printers": null, "auto_print": "0", "local_printers": null, "rtl": null, "print_img": null, "selected_language": "english" };
    $(window).load(function () {
        $('.mm_products').addClass('active');
        $('#products_index').addClass('active');
    });
    var lang = new Array();
    lang['code_error'] = 'Code Error';
    lang['r_u_sure'] = '<strong>Are you sure?</strong>';
    lang['register_open_alert'] = 'Register is open, are you sure to sign out?';
    lang['code_error'] = 'Code Error';
    lang['r_u_sure'] = '<strong>Are you sure?</strong>';
    lang['no_match_found'] = 'No match found';
</script>
<%-- <script src="https://spos.tecdiary.com/themes/default/assets/dist/js/libraries.min.js" type="text/javascript"></script>--%>
<%-- <script src="https://spos.tecdiary.com/themes/default/assets/dist/js/scripts.min.js" type="text/javascript"></script>--%>
<%-- <script src="https://spos.tecdiary.com/themes/default/assets/dist/js/spos_ad.min.js"></script> --%>
    <script src="../scripts/libraries.min.js" type="text/javascript" ></script>
    <script src="../scripts/scripts.min.js" type="text/javascript"></script>
    <script src="../scripts/pos.min.js" type="text/javascript"></script>
         <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:DSTConnectionString %>" 
            SelectCommand="SELECT YEAR(TicketDate) AS year,  MONTH(TicketDate) AS month, 
            DAY(TicketDate) AS day,SUM(CONVERT(float, TicketTotal)) AS TicketTotal, SUM(CONVERT(int, PiecesTotal)) AS PiecesTotal, 
            TicketID,dbo.tblTicket.CustomerAccNo,tc.Surname,tc.Forename
        FROM dbo.tblTicket
        LEFT JOIN  tblCustomer tc ON dbo.tblTicket.CustomerAccNo = tc.CustomerAccNo
        GROUP BY YEAR(TicketDate),MONTH(TicketDate), DAY(TicketDate), 
            TicketID,dbo.tblTicket.CustomerAccNo, tc.Surname,tc.Forename
        ORDER BY TicketID DESC;">

        </asp:SqlDataSource>
</form>


</body>
</html>




