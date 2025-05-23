﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSGiftCardList - Copy.aspx.vb" Inherits="POSGiftCardList" %>

<!DOCTYPE html>
<html>
<head>
<meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" charset="UTF-8">
<title>Gift Cards | SimplePOS</title>
<script type="text/javascript" src="../Scripts/jQuery-2.1.4.min.js"></script>
<%-- <script src="/cdn-cgi/apps/head/Bx0hUCX-YaUCcleOh3fM_NqlPrk.js"></script>--%>
<meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
   <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />
    <link href="../Styles/pos.css" rel="stylesheet" />
</head>
<body class="skin-green fixed sidebar-mini sidebar-collapse">
       <form id="form1" runat="server">
 <asp:ScriptManager ID="ScriptManager1" runat="server"  ViewStateMode="Disabled" ></asp:ScriptManager>
 <div id="main-wrapper">
        <div class="wrapper rtl rtl-inv">
            <header class="main-header">
                <a href="/pos/POS_welcome.aspx" class="logo">
                <span class="logo-mini">POS</span>
                <span class="logo-lg">Simple<b>POS</b></span>
                </a>
                <!-- #include virtual = "/POS/NavBarTop.html" -->
            </header>
            <!-- #include virtual = "/POS/leftmenu.html" -->
                <%--######--%>

<div class="content-wrapper">
<section class="content-header">
<h1>Gift Cards</h1>
<ol class="breadcrumb">
<li><a href="/POS/POS_Welcome.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
<li class="active">Gift Cards</li> </ol>
</section>
<div class="col-lg-12 alerts">
<div id="custom-alerts" style="display:none;">
<div class="alert alert-dismissable">
<div class="custom-msg" id="custom-msg"></div>
</div>
</div>
</div>
<div class="clearfix"></div>


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

<h3 class="box-title">Please use the table below to navigate or filter the results.</h3>
</div>
<div class="box-body">
<div class="table-responsive">

<asp:Table runat="server">
    <asp:TableFooterRow>
        <asp:TableCell   ColumnSpan="12" CssClass="p0">
            <input type="text" class="form-control b0" name="search_table" id="search_table" placeholder="Type & hit enter to search the table" style="width:100%;">
        </asp:TableCell>
    </asp:TableFooterRow>
</asp:Table>

    </div>

<div class="clearfix" id="cat_id"></div>
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
Copyright &copy; 2025 SimplePOS. All rights reserved.
</footer>
</div>
<div id="ajaxCall"><i class="fa fa-spinner fa-pulse"></i></div>
</div>
<script type="text/javascript">
    var base_url = '/pos/';
    var site_url = '/pos/';
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
    <script src="../scripts/libraries.min.js" type="text/javascript" ></script>
    <script src="../scripts/scripts.min.js" type="text/javascript"></script>
    <script src="../scripts/pos.min.js" type="text/javascript"></script>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:DSTConnectionString %>" 
    SelectCommand="SELECT isnull([ProductImage],'CoverImage.png') ProductImage,[PLUID], [Description],  [Type], [Price], [Department], [QtySold], [QtySoldYTD], [ValueSoldYTD], [ValueSold], [Barcode], [CurrentStockQty], [EditedBy], [Category] FROM [tblPLU] where Description <> '' ">
</asp:SqlDataSource>
  <!-- #include virtual = "/POS/FormGiftCardModal.html" -->
</form>


</body>
</html>




