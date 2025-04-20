<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSadd_expense.aspx.vb" Inherits="POSadd_expense" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>Add Expense | SimplePOS</title>
<%-- <script src="/cdn-cgi/apps/head/Bx0hUCX-YaUCcleOh3fM_NqlPrk.js"></script>--%>
    <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />

<meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
<%-- <link href="https://spos.tecdiary.com/themes/default/assets/dist/css/styles.css" rel="stylesheet" type="text/css" />--%>
<link href="../Styles/pos.css" rel="stylesheet" />
<script src="https://spos.tecdiary.com/themes/default/assets/plugins/jQuery/jQuery-2.1.4.min.js"></script>
</head>
<body class="skin-green fixed sidebar-mini">
        <form id="form1" runat="server">
        <div>
<div class="wrapper rtl rtl-inv">
<header class="main-header">
<a href="https://spos.tecdiary.com/" class="logo">
<span class="logo-mini">POS</span>
<span class="logo-lg">Simple<b>POS</b></span>
</a>
<!-- #include virtual = "/POS/NavBarTop.html" -->
</header>
<!-- #include virtual = "/POS/leftmenu.html" -->
<div class="content-wrapper">
<section class="content-header">
<h1>Add Expense</h1>
<ol class="breadcrumb">
<li><a href="https://spos.tecdiary.com/"><i class="fa fa-dashboard"></i> Home</a></li>
<li><a href="https://spos.tecdiary.com/purchases">Purchases</a></li><li><a href="https://spos.tecdiary.com/purchases/expenses">Expenses</a></li><li class="active">Add Expense</li> </ol>
</section>
<div class="col-lg-12 alerts">
<div id="custom-alerts" style="display:none;">
<div class="alert alert-dismissable">
<div class="custom-msg"></div>
</div>
</div>
</div>
<div class="clearfix"></div>
<section class="content">
<div class="row">
<div class="col-xs-12">
<div class="box box-primary">
<div class="box-header">
<h3 class="box-title">Please fill in the information below</h3>
</div>
<div class="box-body">
<div class="col-md-6">
<form action="https://spos.tecdiary.com/purchases/add_expense" enctype="multipart/form-data" method="post" accept-charset="utf-8">
<input type="hidden" name="spos_token" value="74efb7592498f4e9c50ded586fe15ef7" />
 <div class="form-group">
<label for="date">Date</label> <input type="text" name="date" value="" class="form-control datetimepicker" id="date" required="required" />
</div>
<div class="form-group">
<label for="reference">Reference</label> <input type="text" name="reference" value="" class="form-control tip" id="reference" />
</div>
<div class="form-group">
<label for="amount">Amount</label> <input name="amount" type="text" id="amount" value="" class="pa form-control kb-pad amount" required="required" />
</div>
<div class="form-group">
<label for="attachment">Attachment</label> <input id="attachment" type="file" name="userfile" data-show-upload="false" data-show-preview="false" class="form-control file">
</div>
<div class="form-group">
<label for="note">Note</label> <textarea name="note" cols="40" rows="10" class="form-control redactor" id="note"></textarea>
</div>
<div class="form-group">
<input type="submit" name="add_expense" value="Add Expense" class="btn btn-primary" />
</div>
</div>
</form> </div>
<div class="clearfix"></div>
</div>
</div>
</div>
</section>
<script src="/cdn-cgi/scripts/78d64697/cloudflare-static/email-decode.min.js"></script><script src="https://spos.tecdiary.com/themes/default/assets/plugins/bootstrap-datetimepicker/js/moment.min.js" type="text/javascript"></script>
<script src="https://spos.tecdiary.com/themes/default/assets/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $('.datetimepicker').datetimepicker({
            format: 'YYYY-MM-DD HH:mm'
        });
    });
</script>
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
    </form>
<script type="text/javascript">
    var base_url = 'https://spos.tecdiary.com/';
    var site_url = 'https://spos.tecdiary.com/';
    var dateformat = 'D j M Y', timeformat = 'h:i A';
        var Settings = {"logo":"logo1.png","site_name":"SimplePOS","tel":"0105292122","dateformat":"D j M Y","timeformat":"h:i A","language":"english","theme":"default","mmode":"0","captcha":"0","currency_prefix":"USD","default_customer":"3","default_tax_rate":"5%","rows_per_page":"10","total_rows":"30","header":"<h2><strong>Simple POS<\/strong><\/h2>\r\n       My Shop Lot, Shopping Mall,<br>\r\n                                                                                              Post Code, City<br>","footer":"Thank you for your business!\r\n<br>","bsty":"3","display_kb":"0","default_category":"1","default_discount":"0","item_addition":"1","barcode_symbology":"","pro_limit":"10","decimals":"2","thousands_sep":",","decimals_sep":".","focus_add_item":"ALT+F1","add_customer":"ALT+F2","toggle_category_slider":"ALT+F10","cancel_sale":"ALT+F5","suspend_sale":"ALT+F6","print_order":"ALT+F11","print_bill":"ALT+F12","finalize_sale":"ALT+F8","today_sale":"Ctrl+F1","open_hold_bills":"Ctrl+F2","close_register":"ALT+F7","java_applet":"0","receipt_printer":"","pos_printers":"","cash_drawer_codes":"","char_per_line":"42","rounding":"1","pin_code":"abdbeb4d8dbe30df8430a8394b7218ef","purchase_code":null,"envato_username":null,"theme_style":"green","after_sale_page":null,"overselling":"1","multi_store":"1","qty_decimals":"2","symbol":null,"sac":"0","display_symbol":null,"remote_printing":"1","printer":null,"order_printers":null,"auto_print":"0","local_printers":null,"rtl":null,"print_img":null,"selected_language":"english"};
    $(window).load(function () {
        $('.mm_purchases').addClass('active');
        $('#purchases_add_expense').addClass('active');
    });
    var lang = new Array();
    lang['code_error'] = 'Code Error';
    lang['r_u_sure'] = '<strong>Are you sure?</strong>';
    lang['register_open_alert'] = 'Register is open, are you sure to sign out?';
    lang['code_error'] = 'Code Error';
    lang['r_u_sure'] = '<strong>Are you sure?</strong>';
    lang['no_match_found'] = 'No match found';
</script>
<script src="https://spos.tecdiary.com/themes/default/assets/dist/js/libraries.min.js" type="text/javascript"></script>
<script src="https://spos.tecdiary.com/themes/default/assets/dist/js/scripts.min.js" type="text/javascript"></script>
<script src="https://spos.tecdiary.com/themes/default/assets/dist/js/spos_ad.min.js"></script></body>
</html>



