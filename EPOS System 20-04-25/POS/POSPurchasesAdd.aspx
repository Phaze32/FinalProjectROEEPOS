<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSPurchasesAdd.aspx.vb" Inherits="POSPurchasesAdd" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>Add Purchase | SimplePOS</title>
    <script type="text/javascript" src="../Scripts/jQuery-2.1.4.min.js"></script>
 <%--   <script type="text/typescript" language="javascript" src="../Scripts/pos003.js"></script>
   <script type="text/typescript" language="javascript" src="../Scripts/POS002.js"></script>
     <script type="text/typescript" language="javascript" src="../Scripts/POSCustom.js"></script>--%>
    <script src="../scripts/email-decode.min.js" type="text/javascript"></script>
    <script src="../Scripts/POSMain.js" type="text/javascript"></script>
 <%-- --%>  
    <script src="../scripts/libraries.min.js" type="text/javascript" ></script>
    <script src="../scripts/scripts.min.js" type="text/javascript"></script>
    <script src="../scripts/pos.min.js" type="text/javascript"></script>

    <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />
    <link href="../Styles/pos.css" rel="stylesheet" />
</head>
<body class="skin-green fixed sidebar-mini">
<form id="form2" runat="server">
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
<h1>Add Purchase</h1>
<ol class="breadcrumb">
<li><a href="https://spos.tecdiary.com/"><i class="fa fa-dashboard"></i> Home</a></li>
<li><a href="https://spos.tecdiary.com/purchases">Purchases</a></li><li class="active">Add Purchase</li> </ol>
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
<div class="col-lg-12">
<form action="https://spos.tecdiary.com/purchases/add" class="validation" enctype="multipart/form-data" method="post" accept-charset="utf-8">
<input type="hidden" name="spos_token" value="cc252b3bd5f5b7491a9516e691066bef" />
<div class="row">
<div class="col-md-6">
<div class="form-group">
<label for="date">Date</label> <input type="text" name="date" value="2017-10-16 01:43" class="form-control tip" id="date" required="required" />
</div>
</div>
<div class="col-md-6">
<div class="form-group">
<label for="reference">Reference</label> <input type="text" name="reference" value="" class="form-control tip" id="reference" />
</div>
</div>
</div>
<div class="form-group">
<input type="text" placeholder="Search product by code or name, you can scan barcode too" id="add_item" class="form-control">
</div>
<div class="row">
<div class="col-md-12">
<div class="table-responsive">
<table id="poTable" class="table table-striped table-bordered">
<thead>
<tr class="active">
<th>Product</th>
<th class="col-xs-2">Quantity</th>
<th class="col-xs-2">Unit Cost</th>
<th class="col-xs-2">Subtotal</th>
<th style="width:25px;"><i class="fa fa-trash-o"></i></th>
</tr>
</thead>
<tbody>
<tr>
<td colspan="5">Add product by searching in above fields</td>
</tr>
</tbody>
<tfoot>
<tr class="active">
<th>Total</th>
<th class="col-xs-2"></th>
<th class="col-xs-2"></th>
<th class="col-xs-2 text-right"><span id="gtotal">0.00</span></th>
<th style="width:25px;"></th>
</tr>
</tfoot>
</table>
</div>
</div>
</div>
<div class="row">
<div class="col-md-6">
<div class="form-group">
<label for="supplier">Supplier</label> <select name="supplier" class="form-control select2 tip" id="supplier" required="required" style="width:100%;">
<option value="" selected="selected">Select Supplier</option>
<option value="1">Test Supplier</option>
</select>
</div>
</div>
<div class="col-md-6">
<div class="form-group">
<label for="received">Received</label> <select name="received" class="form-control select2 tip" id="received" required="required" style="width:100%;">
<option value="1">Received</option>
<option value="0">Not received yet</option>
</select>
</div>
</div>
</div>
<div class="form-group">
<label for="attachment">Attachment</label> <input type="file" name="userfile" class="form-control tip" id="attachment">
</div>
<div class="form-group">
<label for="note">Note</label> <textarea name="note" cols="40" rows="10" class="form-control redactor" id="note"></textarea>
</div>
<div class="form-group">
<input type="submit" name="add_purchase" value="Add Purchase" class="btn btn-primary" />
<button type="button" id="reset" class="btn btn-danger">Reset</button>
</div>
</form> </div>
<div class="clearfix"></div>
</div>
</div>
</div>
</div>
</section>
<script style="display: none !important;">!function(e,t,r,n,c,a,l){function i(t,r){return r=e.createElement('div'),r.innerHTML='<a href="'+t.replace(/"/g,'&quot;')+'"></a>',r.childNodes[0].getAttribute('href')}function o(e,t,r,n){for(r='',n='0x'+e.substr(t,2)|0,t+=2;t<e.length;t+=2)r+=String.fromCharCode('0x'+e.substr(t,2)^n);return i(r)}try{for(c=e.getElementsByTagName('a'),l='/cdn-cgi/l/email-protection#',n=0;n<c.length;n++)try{(t=(a=c[n]).href.indexOf(l))>-1&&(a.href='mailto:'+o(a.href,t+l.length))}catch(e){}for(c=e.querySelectorAll('.__cf_email__'),n=0;n<c.length;n++)try{(a=c[n]).parentNode.replaceChild(e.createTextNode(o(a.getAttribute('data-cfemail'),0)),a)}catch(e){}}catch(e){}}(document);</script><script type="text/javascript">
    var spoitems = {};
    if (localStorage.getItem('remove_spo')) {
        if (localStorage.getItem('spoitems')) {
            localStorage.removeItem('spoitems');
        }
        localStorage.removeItem('remove_spo');
    }
</script>
<script src="https://spos.tecdiary.com/themes/default/assets/dist/js/purchases.min.js" type="text/javascript"></script>
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
    var base_url = 'https://spos.tecdiary.com/';
    var site_url = 'https://spos.tecdiary.com/';
    var dateformat = 'D j M Y', timeformat = 'h:i A';
        var Settings = {"logo":"logo1.png","site_name":"SimplePOS","tel":"0105292122","dateformat":"D j M Y","timeformat":"h:i A","language":"english","theme":"default","mmode":"0","captcha":"0","currency_prefix":"USD","default_customer":"3","default_tax_rate":"5%","rows_per_page":"10","total_rows":"30","header":"<h2><strong>Simple POS<\/strong><\/h2>\r\n       My Shop Lot, Shopping Mall,<br>\r\n                                                                                              Post Code, City<br>","footer":"Thank you for your business!\r\n<br>","bsty":"3","display_kb":"0","default_category":"1","default_discount":"0","item_addition":"1","barcode_symbology":"","pro_limit":"10","decimals":"2","thousands_sep":",","decimals_sep":".","focus_add_item":"ALT+F1","add_customer":"ALT+F2","toggle_category_slider":"ALT+F10","cancel_sale":"ALT+F5","suspend_sale":"ALT+F6","print_order":"ALT+F11","print_bill":"ALT+F12","finalize_sale":"ALT+F8","today_sale":"Ctrl+F1","open_hold_bills":"Ctrl+F2","close_register":"ALT+F7","java_applet":"0","receipt_printer":"","pos_printers":"","cash_drawer_codes":"","char_per_line":"42","rounding":"1","pin_code":"abdbeb4d8dbe30df8430a8394b7218ef","purchase_code":null,"envato_username":null,"theme_style":"green","after_sale_page":null,"overselling":"1","multi_store":"1","qty_decimals":"2","symbol":null,"sac":"0","display_symbol":null,"remote_printing":"1","printer":null,"order_printers":null,"auto_print":"0","local_printers":null,"rtl":null,"print_img":null,"selected_language":"english"};
    $(window).load(function () {
        $('.mm_purchases').addClass('active');
        $('#purchases_add').addClass('active');
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

