<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POScategoryList - Copy.aspx.vb" Inherits="POScategoryList" %>


<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>Categories | SimplePOS</title>
<!-- #include virtual = "/POS/javascriptsinclude.html" -->
    <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />
    <link href="../Styles/pos.css" rel="stylesheet" />
</head>
<body class="skin-green fixed sidebar-mini sidebar-collapse">
     <form id="form1" runat="server">
<div class="wrapper rtl rtl-inv">
 <header class="main-header">
                <a href="/POS/pos_welcome.aspx" class="logo">
                <span class="logo-mini">POS</span>
                <span class="logo-lg">Simple<b>POS</b></span>
                </a>
                <!-- #include virtual = "/POS/NavBarTop.html" -->
 </header>
            <!-- #include virtual = "/POS/leftmenu.html" -->
                <%--######--%>
<div class="content-wrapper">
<section class="content-header">
<h1>Categories</h1>
<ol class="breadcrumb">
<li><a href="/POS/pos_welcome.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
<li class="active">Categories</li> </ol>
</section>
<div class="col-lg-12 alerts">
<div id="custom-alerts" style="display:none;">
<div class="alert alert-dismissable">
<div class="custom-msg"></div>
</div>
</div>
</div>
<div class="clearfix"></div>
<script type="text/javascript">
    $(document).ready(function () {
        var table = $('#catData').DataTable({
            ajax: {
                url: '/handlers/CategoryHandler.ashx',
                type: 'POST',
                dataType: 'json',
                dataSrc: '', // your JSON is a flat array, so this is correct
                error: function (xhr, status, error) {
                    console.error('Error fetching data:', error);
                    console.error('Response:', xhr.responseText);
                    alert('Failed to load data from the server.');
                }
            },
            columns: [
                { data: "Rec_id", title: "Rec_id", visible: true },
                { data: "Department", title: "Department", searchable: false, orderable: false },
                { data: "DeparmentName", title: "Department Name", searchable: true, orderable: true },
                {
                    data: "ShopID",
                    title: "ShopID",
                    render: function (data) {
                        return data ? data : 'N/A';
                    }
                },
                {
                    data: "DeparmentName",
                    title: "Repeated Name",
                    searchable: false,
                    orderable: false
                }
            ],
            dom: 'Bfrtip',
            buttons: [
                { extend: 'copyHtml5', exportOptions: { columns: [0, 1, 2, 3] } },
                { extend: 'excelHtml5', exportOptions: { columns: [0, 1, 2, 3] } },
                { extend: 'csvHtml5', exportOptions: { columns: [0, 1, 2, 3] } },
                {
                    extend: 'pdfHtml5',
                    orientation: 'landscape',
                    pageSize: 'A4',
                    exportOptions: { columns: [0, 1, 2, 3] }
                },
                { extend: 'colvis', text: 'Columns' }
            ],
            language: {
                emptyTable: "No data available in the table"
            }
        });

        // Search input behavior
        $('#search_table').on('keyup change', function (e) {
            var code = e.keyCode ? e.keyCode : e.which;
            if ((code === 13 && table.search() !== this.value) || (table.search() !== '' && this.value === '')) {
                table.search(this.value).draw();
            }
        });
    });
</script>

<script>
    $(document).ready(function () {
        $('#catData').on('click', '.image', function () {
            var a_href = $(this).attr('href');
            var code = $(this).attr('id');
            $('#myModalLabel').text(code);
            $('#product_image').attr('src', a_href);
            $('#picModal').modal();
            return false;
        });
        $('#catData').on('click', '.open-image', function () {
            var a_href = $(this).attr('href');
            var code = $(this).closest('tr').find('.image').attr('id');
            $('#myModalLabel').text(code);
            $('#product_image').attr('src', a_href);
            $('#picModal').modal();
            return false;
        });
    });
</script>
<section class="content">
<div class="row">
<div class="col-xs-12">
<div class="box box-primary">
<div class="box-header">
<h3 class="box-title">Please use the table below to navigate or filter the results.</h3>
</div>
<div class="box-body">
<div class="table-responsive">
<table id="catData" class="table table-striped table-bordered table-condensed table-hover" style="margin-bottom:5px;">
    <thead>
        <tr class="active">
            <th style="max-width:30px;">Rec_id</th>
            <th style="max-width:30px;">Department</th>
            <th>DeparmentName</th>
            <th>ShopID</th>
              <th style="width:30px;">Action</th>
        </tr>
    </thead>
    <tbody>
        <tr>
        <td colspan="5" class="dataTables_empty">Leading data from server</td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
        <td colspan="5" class="p0"><input type="text" class="form-control b0" name="search_table" id="search_table" placeholder="Type & hit enter to search the table" style="width:100%;"></td>
        </tr>
    </tfoot>
</table>
</div>
<div class="clearfix"></div>
</div>
</div>
</div>
</div>
</section>
<div class="modal fade" id="picModal" tabindex="-1" role="dialog" aria-labelledby="picModalLabel" aria-hidden="true">
<div class="modal-dialog">
<div class="modal-content">
<div class="modal-header">
<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
<h4 class="modal-title" id="myModalLabel">Modal title</h4>
</div>
<div class="modal-body text-center">
<img id="product_image" src="" alt="" />
</div>
</div>
</div>
</div>
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
        </form>

<script type="text/javascript">
    var base_url = '/POS/';
    var site_url = 'https:///POS/.com/';    // needs name from the 
    var dateformat = 'D j M Y', timeformat = 'h:i A';
    var Settings = { "logo": "logo1.png", "site_name": "SimplePOS", "tel": "0105292122", "dateformat": "D j M Y", "timeformat": "h:i A", "language": "english", "theme": "default", "mmode": "0", "captcha": "0", "currency_prefix": "USD", "default_customer": "3", "default_tax_rate": "5%", "rows_per_page": "10", "total_rows": "30", "header": "<h2><strong>Simple POS<\/strong><\/h2>\r\n       My Shop Lot, Shopping Mall,<br>\r\n                                                                                              Post Code, City<br>", "footer": "Thank you for your business!\r\n<br>", "bsty": "3", "display_kb": "0", "default_category": "1", "default_discount": "0", "item_addition": "1", "barcode_symbology": "", "pro_limit": "10", "decimals": "2", "thousands_sep": ",", "decimals_sep": ".", "focus_add_item": "ALT+F1", "add_customer": "ALT+F2", "toggle_category_slider": "ALT+F10", "cancel_sale": "ALT+F5", "suspend_sale": "ALT+F6", "print_order": "ALT+F11", "print_bill": "ALT+F12", "finalize_sale": "ALT+F8", "today_sale": "Ctrl+F1", "open_hold_bills": "Ctrl+F2", "close_register": "ALT+F7", "java_applet": "0", "receipt_printer": "", "pos_printers": "", "cash_drawer_codes": "", "char_per_line": "42", "rounding": "1", "pin_code": "abdbeb4d8dbe30df8430a8394b7218ef", "purchase_code": null, "envato_username": null, "theme_style": "green", "after_sale_page": null, "overselling": "1", "multi_store": "1", "qty_decimals": "2", "symbol": null, "sac": "0", "display_symbol": null, "remote_printing": "1", "printer": null, "order_printers": null, "auto_print": "0", "local_printers": null, "rtl": null, "print_img": null, "selected_language": "english" };
    $(window).load(function () {
        $('.mm_categories').addClass('active');
        $('#categories_index').addClass('active');
    });
    var lang = new Array();
    lang['code_error'] = 'Code Error';
    lang['r_u_sure'] = '<strong>Are you sure?</strong>';
    lang['register_open_alert'] = 'Register is open, are you sure to sign out?';
    lang['code_error'] = 'Code Error';
    lang['r_u_sure'] = '<strong>Are you sure?</strong>';
    lang['no_match_found'] = 'No match found';
</script>
 <script src="../scripts/libraries.min.js" type="text/javascript" ></script>
<script src="../scripts/scripts.min.js" type="text/javascript"></script>
<script src="../scripts/pos.min.js" type="text/javascript"></script>
</body>
</html>


