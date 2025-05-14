<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSProductImport.aspx.vb" Inherits="POSProductImport" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>Import Products | SimplePOS</title>
<!-- #include virtual = "/POS/javascriptsinclude.html" -->
    <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />
    <link href="../Styles/pos.css" rel="stylesheet" />
</head>
<body class="skin-green fixed sidebar-mini">
       <form id="form1" runat="server">
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
<h1>Import Products</h1>
<ol class="breadcrumb">
<li><a href="https://spos.tecdiary.com/"><i class="fa fa-dashboard"></i> Home</a></li>
<li><a href="https://spos.tecdiary.com/products">Products</a></li><li class="active">Import Products</li> </ol>
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
<div class="well well-sm">
<a href="https://spos.tecdiary.com/uploads/csv/sample_products.csv" class="btn btn-info btn-sm pull-right"><i class="fa fa-download"></i> Download sample File</a>
<p><span class="text-info">The first line in downloaded csv file should remain as it is. Please do not change the order of columns.</span><br /><span class="text-success">The correct column order is (<b>Product Code, Product Name, Purchase Price, Product Tax, Product Price, Category Code</b>)</span> <span class="text-primary">&amp; you must follow this. If you are using any other language then English, please make sure the csv file is UTF-8 encoded and not saved with byte order mark (BOM)</span></p>
</div>
<form action="https://spos.tecdiary.com/products/import" enctype="multipart/form-data" method="post" accept-charset="utf-8">
<input type="hidden" name="spos_token" value="41b9555b0ce496326f24656c24dcc5b0" />
<div class="form-group">
<label for="csv_file">Upload File</label> <input type="file" name="userfile" id="csv_file">
<div class="inline-help">Please select .csv files (allowed file size 200KB)</div>
</div>
<div class="form-group">
<input type="submit" name="import" value="Import" class="btn btn-primary" />
</div>
</form>
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
    </form>
<script style="display: none !important;">!function(e,t,r,n,c,a,l){function i(t,r){return r=e.createElement('div'),r.innerHTML='<a href="'+t.replace(/"/g,'&quot;')+'"></a>',r.childNodes[0].getAttribute('href')}function o(e,t,r,n){for(r='',n='0x'+e.substr(t,2)|0,t+=2;t<e.length;t+=2)r+=String.fromCharCode('0x'+e.substr(t,2)^n);return i(r)}try{for(c=e.getElementsByTagName('a'),l='/cdn-cgi/l/email-protection#',n=0;n<c.length;n++)try{(t=(a=c[n]).href.indexOf(l))>-1&&(a.href='mailto:'+o(a.href,t+l.length))}catch(e){}for(c=e.querySelectorAll('.__cf_email__'),n=0;n<c.length;n++)try{(a=c[n]).parentNode.replaceChild(e.createTextNode(o(a.getAttribute('data-cfemail'),0)),a)}catch(e){}}catch(e){}}(document);</script><script type="text/javascript">
    var base_url = 'https://spos.tecdiary.com/';
    var site_url = 'https://spos.tecdiary.com/';
    var dateformat = 'D j M Y', timeformat = 'h:i A';
        var Settings = {"logo":"logo1.png","site_name":"SimplePOS","tel":"0105292122","dateformat":"D j M Y","timeformat":"h:i A","language":"english","theme":"default","mmode":"0","captcha":"0","currency_prefix":"USD","default_customer":"3","default_tax_rate":"5%","rows_per_page":"10","total_rows":"30","header":"<h2><strong>Simple POS<\/strong><\/h2>\r\n       My Shop Lot, Shopping Mall,<br>\r\n                                                                                              Post Code, City<br>","footer":"Thank you for your business!\r\n<br>","bsty":"3","display_kb":"0","default_category":"1","default_discount":"0","item_addition":"1","barcode_symbology":"","pro_limit":"10","decimals":"2","thousands_sep":",","decimals_sep":".","focus_add_item":"ALT+F1","add_customer":"ALT+F2","toggle_category_slider":"ALT+F10","cancel_sale":"ALT+F5","suspend_sale":"ALT+F6","print_order":"ALT+F11","print_bill":"ALT+F12","finalize_sale":"ALT+F8","today_sale":"Ctrl+F1","open_hold_bills":"Ctrl+F2","close_register":"ALT+F7","java_applet":"0","receipt_printer":"","pos_printers":"","cash_drawer_codes":"","char_per_line":"42","rounding":"1","pin_code":"abdbeb4d8dbe30df8430a8394b7218ef","purchase_code":null,"envato_username":null,"theme_style":"green","after_sale_page":null,"overselling":"1","multi_store":"1","qty_decimals":"2","symbol":null,"sac":"0","display_symbol":null,"remote_printing":"1","printer":null,"order_printers":null,"auto_print":"0","local_printers":null,"rtl":null,"print_img":null,"selected_language":"english"};
    $(window).load(function () {
        $('.mm_products').addClass('active');
        $('#products_import').addClass('active');
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
