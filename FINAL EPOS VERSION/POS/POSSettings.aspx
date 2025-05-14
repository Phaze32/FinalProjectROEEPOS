<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSSettings.aspx.vb" Inherits="Styles_POSSettings" %>


<!DOCTYPE html>
<html>
<head>
<meta content="HTML,CSS,XML,JavaScript" charset="UTF-8">
<title>Settings | SimplePOS</title>
<script src="/cdn-cgi/apps/head/Bx0hUCX-YaUCcleOh3fM_NqlPrk.js"></script>
<%--    <link rel="shortcut icon" href="https://spos.tecdiary.com/themes/default/assets/images/icon.png" />--%> 
<meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
<%-- <link href="https://spos.tecdiary.com/themes/default/assets/dist/css/styles.css" rel="stylesheet" type="text/css" />--%>
  <script type="text/javascript" src="../Scripts/jQuery-2.1.4.min.js"></script>
    <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />
    <link href="../Styles/pos.css" rel="stylesheet" />

<%-- <script src="https://spos.tecdiary.com/themes/default/assets/plugins/jQuery/jQuery-2.1.4.min.js"></script>--%>
 <script type="text/javascript" >
        $(document).ready(function () {
            $("#update_settings").click(function () {
                var site_name = document.getElementById("site_name").value;
                var currency_prefix = document.getElementById("currency_prefix").value;
                var pin_code = document.getElementById("pin_code").value;
                var pin_code = document.getElementById("pin_code").value;
                var prod_disp_limit = document.getElementById("prod_disp_limit").value;
                var mydata ='site_name=' + site_name + '&currency_prefix=' + currency_prefix + '&pin_code=' + pin_code + '&prod_disp_limit='+ prod_disp_limit
                //alert(mydata);
                UpdateSettings(mydata);
            });
        });
</script>
    
    <script type="text/javascript">
        function myFunc(f) {
            // f = "ddd";
            // alert("Test ok!" + "PLUid=" + f);
            get_product(f);
        };
        function UpdateSettings(mydata) {
            //alert('hh')
            // alert('POSCustom.ln:242 Product_ids=' + Product_ids);
            $.ajax({
                type: "GET",
                url: "UpdateSettings.aspx",
                data: "act=?UpdateSettings&" + mydata,
                success: function (msg) {
                    alert('POSSettings.UpdateSettings.ln:47 msg=' + msg);
                    $("#System_Message").html(msg);

                    //setTimeout(function(){$.unblockUI}, 50 );
                    // $.unblockUI();
                }
            });
            //alert(mydata);
        };
</script>
</head>
<body class="skin-green fixed sidebar-mini sidebar-collapse">
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
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        <section class="content-header">
        <h1>Settings</h1>
        <ol class="breadcrumb">
            <li><a href="/">
            <i class="fa fa-dashboard"></i> Home</a></li>
            <li class="active">Settings</li> 
        </ol>
        </section>
               <div class="col-lg-12 alerts">
            <div id="custom-alerts" style="display:none;">
                <div class="alert alert-dismissable">
                <div class="custom-msg">testing return message</div>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <section class="content">
            <div class="row">
            <div class="col-xs-12">
            <div class="box box-primary">
            <div class="box-header">
                <h3 class="box-title">Please update the information below</h3>
                <div  id="System_Message">Testing</div>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <form action="https://spos.tecdiary.com/settings" class="validation" enctype="multipart/form-data" method="post" accept-charset="utf-8">
                <input type="hidden" name="spos_token" value="e7a6d8bfe584c5d9d82240ccc2f816e5" />
                <div class="row">
                <div class="col-md-6">
                <div class="form-group">
                <label for="site_name">Site name</label> 
                <input type="text" name="site_name" value="<%=site_name %>" class="form-control" id="site_name" required="required" />
                </div>
                <div class="form-group">
                <label for="tel">Tel</label> <input type="text" name="tel" value="<%=tell %>"" class="form-control" id="tel" required="required" />
                </div>
                <div class="form-group">
                <label for="language">Language</label> 
                <select name="language" class="form-control tip select2" id="language" required="required" style="width:100%;">
                <option value="arabic">Arabic</option>
                <option value="english" selected="selected">English</option>
                </select>
                </div>
                <div class="form-group">
                <label for="theme">Theme</label> 
                <select name="theme" class="form-control tip select2" id="theme" required="required" style="width:100%;">
                <option value="default" selected="selected">Default</option>
                </select>
                </div>
                <div class="form-group">
                <label for="theme_style">Theme Style</label> 
                <select name="theme_style" class="form-control tip select2" id="theme_style" required="required" style="width:100%;">
                <option value="black">Black</option>
                <option value="black-light">Black Light</option>
                <option value="blue">Blue</option>
                <option value="blue-light">Blue Light</option>
                <option value="green" selected="selected">Green</option>
                <option value="green-light">Green Light</option>
                <option value="purple">Purple</option>
                <option value="purple-light">Purple Light</option>
                <option value="red">Red</option>
                <option value="red-light">Red Light</option>
                <option value="yellow">Yellow</option>
                <option value="yellow-light">Yellow Light</option>
                </select>
                </div>
                <div class="form-group">
                <label for="overselling">Overselling</label> 
                <select name="overselling" class="form-control select2" id="overselling" required="required" style="width:100%;">
                <option value="0">Disable</option>
                <option value="1" selected="selected">Enable</option>
                </select>
                </div>
                <div class="form-group">
                <label for="multi_store">Multiple Stores</label> <select name="multi_store" class="form-control select2" id="multi_store" required="required" style="width:100%;">
                <option value="0">Disable</option>
                <option value="1" selected="selected">Enable</option>
                </select>
                </div>
                <div class="form-group">
                <label for="currency_code">Currency code</label> 
                    <input type="text" id="currency_prefix" name="currency_prefix" value="<%=currency_prefix %>" class="form-control" id="currency_code" required="required" />
                </div>
                <div class="form-group">
                <label for="auto_print">Auto Print Receipt</label> <select name="auto_print" class="form-control select2" id="auto_print" required="required" style="width:100%;">
                <option value="0" selected="selected">Disable</option>
                <option value="1">Enable</option>
                </select>
                </div>
                <div class="form-group">
                <label for="after_sale_page">After Sale Page</label> 
                <select name="after_sale_page" class="form-control select2" id="after_sale_page" required="required" style="width:100%;">
                <option value="0">Receipt</option>
                <option value="1">POS</option>
                </select>
                </div>
                <div class="form-group">
                <label for="default_discount">Default Discount</label> 
                <input type="text" name="default_discount" value="<%=default_discount %>" class="form-control" id="default_discount" required="required" />
                </div>
                <div class="form-group">
                <label for="default_tax_rate">Default Order Tax</label> 
                <input type="text" name="tax_rate" value="<%=tax_rate %>" class="form-control" id="default_tax_rate" required="required" />
                </div>
                <div class="form-group">
                <label for="rows_per_page">Row per page</label> 
                <asp:DropDownList ID="DDL_rows_per_page" name="rows_per_page" class="form-control select2" runat="server">
                    <asp:ListItem Value="10">10</asp:ListItem>
                    <asp:ListItem Value="25">25</asp:ListItem>
                    <asp:ListItem Value="50">50</asp:ListItem>
                    <asp:ListItem Value="100">100</asp:ListItem>
                </asp:DropDownList>
                </div>
                </div>
                <div class="col-md-6">
                <div class="form-group">
                <label for="pin_code">Pin Code</label> 
                <input type="password" name="pin_code" value="<%=pin_code %>" class="form-control" pattern="[0-9]{4,8}" id="pin_code" />
                </div>
                <div class="form-group">
                <label for="rounding">Rounding</label> <select name="rounding" class="form-control select2" id="rounding" required="required" style="width:100%;">
                <option value="0">Disable</option>
                <option value="1" selected="selected">Round to nearest 0.05</option>
                <option value="2">Round to nearest 0.50</option>
                <option value="3">Round to nearest number (Integer)</option>
                <option value="4">Round to next number (Integer)</option>
                </select>
                </div>
                <div class="form-group">
                <label for="display_product">Display Product</label> 
                <select name="display_product" class="form-control select2" id="display_product" style="width:100%;" required="required">
                <option value="1">Name</option>
                <option value="2">Photo</option>
                <option value="3" selected="selected">Both</option>
                </select>
                </div>
                <div class="form-group">
                <label for="pro_limit">Product Display Limit</label> 
                <input type="text" name="<%=prod_disp_limit %>"" value="10" class="form-control" id="prod_disp_limit" required="required" />
                </div>
                <div class="form-group">
                <label for="display_kb">Display Keyboard</label> <select name="display_kb" class="form-control select2" id="display_kb" style="width:100%;" required="required">
                <option value="1">Yes</option>
                <option value="0" selected="selected">No</option>
                </select>
                </div>
                <div class="form-group">
                <label for="item_addition">Item Addition</label> <select name="item_addition" id="item_addition" class="form-control tip select2" required="required" style="width:100%;">
                <option value="0">Add new item</option>
                <option value="1" selected="selected">Increase the quantity of item is exists</option>
                </select>
                </div>
                <div class="form-group">
                <label for="default_category">Default Category</label> <select name="default_category" class="form-control select2" style="width:100%;" id="default_category">
                <option value="0">Select Default Category</option>
                <option value="1" selected="selected">General</option>
                </select>
                </div>
                <div class="form-group">
                <label for="default_customer">Default Customer</label> <select name="default_customer" class="form-control select2" style="width:100%;" id="default_customer" required="required">
                <option value="1">Walk-in Client</option>
                </select>
                </div>
                <div class="form-group">
                <div class="form-group">
                <label for="dateformat">Date Format</label> <a href="http://php.net/manual/en/function.date.php" target="_blank"><i class="fa fa-external-link"></i></a>
                <input type="text" name="dateformat" value="D j M Y" class="form-control tip" id="dateformat" required="required" />
                </div>
                </div>
                <div class="form-group">
                <label for="timeformat">Time Format</label> <input type="text" name="timeformat" value="h:i A" class="form-control tip" id="timeformat" required="required" />
                </div>
                <div class="form-group">
                <label for="default_email">Default email</label> <input type="text" name="default_email" value="noreply@spos.tecdiary.my" class="form-control tip" id="default_email" required="required" />
                </div>
                <div class="form-group">
                <label for="rtl">RTL Support</label> <select name="rtl" class="form-control select2" id="rtl">
                <option value="0">Disable</option>
                <option value="1">Enable</option>
                </select>
                </div>
                <div class="form-group">
                <label for="protocol">Email protocol</label> <div class="controls">
                <select name="protocol" class="form-control tip select2" id="protocol" style="width:100%;" required="required">
                <option value="mail" selected="selected">PHP Mail Function</option>
                <option value="sendmail">Send Mail</option>
                <option value="smtp">SMTP</option>
                </select>
                </div>
                </div>
                </div>
                <div class="clearfix"></div>
                <div class="row" id="sendmail_config" style="display: none;">
                <div class="col-md-12">
                <div class="col-md-6">
                <div class="form-group">
                <label for="mailpath">Mailpath</label> <div class="controls"> <input type="text" name="mailpath" value="" class="form-control tip" id="mailpath" />
                </div>
                </div>
                </div>
                </div>
                </div>
                <div class="clearfix"></div>
                <div class="row" id="smtp_config" style="display: none;">
                <div class="col-md-12">
                <div class="col-md-6">
                <div class="form-group">
                <label for="smtp_host">SMTP Host</label> <div class="controls"> <input type="text" name="smtp_host" value="pop.gmail.com" class="form-control tip" id="smtp_host" />
                </div>
                </div>
                </div>
                <div class="col-md-6">
                <div class="form-group">
                <label for="smtp_user">SMTP User</label> <div class="controls"> <input type="text" name="smtp_user" value="noreply@spos.tecdiary.my" class="form-control tip" id="smtp_user" />
                </div>
                </div>
                </div>
                <div class="col-md-6">
                <div class="form-group">
                <label for="smtp_pass">SMTP Password</label> <div class="controls"> <input type="password" name="smtp_pass" value="" class="form-control tip" id="smtp_pass" />
                </div>
                </div>
                </div>
                <div class="col-md-6">
                <div class="form-group">
                <label for="smtp_port">SMTP Port</label>  <div class="controls"> <input type="text" name="smtp_port" value="25" class="form-control tip" id="smtp_port" />
                </div>
                </div>
                </div>
                <div class="col-md-6">
                <div class="form-group">
                <label for="smtp_crypto">SMTP Crypto</label> <select name="smtp_crypto" class="form-control tip select2" id="smtp_crypto" style="width:100%;">
                <option value="" selected="selected">None</option>
                <option value="tls">TLS</option>
                <option value="ssl">SSL</option>
                </select>
                </div>
                </div>
                </div>
                </div>
                </div>
                <div class="row">
                <div class="col-lg-12">
                <div class="well well-sm">
                <div class="col-md-4">
                <div class="form-group">
                <label class="control-label" for="decimals">Decimals</label>
                <div class="controls"> <select name="decimals" class="form-control tip select2" id="decimals" style="width:100%;" required="required">
                <option value="0">Disable</option>
                <option value="1">1</option>
                <option value="2" selected="selected">2</option>
                </select>
                </div>
                </div>
                </div>
                <div class="col-md-4">
                <div class="form-group">
                <label class="control-label" for="qty_decimals">Quantity Decimals</label>
                <div class="controls"> <select name="qty_decimals" class="form-control tip select2" id="qty_decimals" style="width:100%;" required="required">
                <option value="0">Disable</option>
                <option value="1">1</option>
                <option value="2" selected="selected">2</option>
                </select>
                </div>
                </div>
                </div>
                <div class="col-md-4">
                <div class="form-group">
                <label for="sac">South Asian countries - currency formate</label> <select name="sac" class="form-control tip select2" id="sac" required="required">
                <option value="0" selected="selected">Disable</option>
                <option value="1">Enable</option>
                </select>
                </div>
                </div>
                <div class="clearfix"></div>
                <div class="nsac">
                <div class="col-md-4">
                <div class="form-group">
                <label class="control-label" for="decimals_sep">Decimals Separator</label>
                <div class="controls"> <select name="decimals_sep" class="form-control tip select2" id="decimals_sep" style="width:100%;" required="required">
                <option value="." selected="selected">Dot (.)</option>
                <option value=",">Comma (,)</option>
                </select>
                </div>
                </div>
                </div>
                <div class="col-md-4">
                <div class="form-group">
                <label class="control-label" for="thousands_sep">Thousands Separator</label>
                <div class="controls"> <select name="thousands_sep" class="form-control tip select2" id="thousands_sep" style="width:100%;" required="required">
                <option value=".">Dot (.)</option>
                <option value="," selected="selected">Comma (,)</option>
                <option value="0">Space</option>
                </select>
                </div>
                </div>
                </div>
                </div>
                <div class="col-md-4">
                <div class="form-group">
                <label for="display_symbol">Display Currency Symbol</label> <select name="display_symbol" class="form-control select2" id="display_symbol" style="width:100%;" required="required">
                <option value="0">Disable</option>
                <option value="1">Before</option>
                <option value="2">After</option>
                </select>
                </div>
                </div>
                <div class="col-md-4">
                <div class="form-group">
                <label for="symbol">Currency Symbol</label> <input type="text" name="symbol" value="" class="form-control" id="symbol" style="width:100%;" />
                </div>
                </div>
                <div class="clearfix"></div>
                </div>
                </div>
                </div>
                <div class="row">
                <div class="col-lg-12">
                <div class="well well-sm">
                <div class="col-md-6">
                <div class="form-group">
                <label for="stripe">Stripe</label> <select name="stripe" class="form-control select2" id="stripe" required="required">
                <option value="0">Disable</option>
                <option value="1" selected="selected">Enable</option>
                </select>
                </div>
                </div>
                <div class="clearfix"></div>
                <div id="stripe_con">
                <div class="col-md-6 col-sm-6">
                <div class="form-group">
                <label for="stripe_secret_key">Stripe Secret Key</label> <input type="text" name="stripe_secret_key" value="sk_test_jHf4cEzAYtgcXvgWPCsQAn50" class="form-control tip" id="stripe_secret_key" />
                </div>
                </div>
                <div class="col-md-6 col-sm-6">
                <div class="form-group">
                <label for="stripe_publishable_key">Stripe Publishable Key</label> <input type="text" name="stripe_publishable_key" value="pk_test_beat8SWPORb0OVdF2H77A7uG" class="form-control tip" id="stripe_publishable_key" />
                </div>
                </div>
                <div class="clearfix"></div>
                </div>
                </div>
                </div>
                </div>
                <div class="row">
                <div class="col-lg-12">
                <div class="well well-sm">
                <p>Please set your shortcuts as you like, you can use F1 - F2 or any other key combinations with Crtl, Alt and Shift.</p>
                <div class="col-md-4 col-sm-4">
                <div class="form-group">
                <label for="focus_add_item">Focus add/search item input</label> <input type="text" name="focus_add_item" value="ALT+F1" class="form-control tip" id="focus_add_item" />
                </div>
                </div>
                <div class="col-md-4 col-sm-4">
                <div class="form-group">
                <label for="add_customer">Add Customer</label> <input type="text" name="add_customer" value="ALT+F2" class="form-control tip" id="add_customer" />
                </div>
                </div>
                <div class="col-md-4 col-sm-4">
                <div class="form-group">
                <label for="toggle_category_slider">Toggle Category Slider</label> <input type="text" name="toggle_category_slider" value="ALT+F10" class="form-control tip" id="toggle_category_slider" />
                </div>
                </div>
                <div class="col-md-4 col-sm-4">
                <div class="form-group">
                <label for="cancel_sale">Cancel Sale</label> <input type="text" name="cancel_sale" value="ALT+F5" class="form-control tip" id="cancel_sale" />
                </div>
                </div>
                <div class="col-md-4 col-sm-4">
                <div class="form-group">
                <label for="suspend_sale">Suspend Sale</label> <input type="text" name="suspend_sale" value="ALT+F6" class="form-control tip" id="suspend_sale" />
                </div>
                </div>
                <div class="col-md-4 col-sm-4">
                <div class="form-group">
                <label for="print_order">Print Order</label> <input type="text" name="print_order" value="ALT+F11" class="form-control tip" id="print_order" />
                </div>
                </div>
                <div class="col-md-4 col-sm-4">
                <div class="form-group">
                <label for="print_bill">Print Bill</label> <input type="text" name="print_bill" value="ALT+F12" class="form-control tip" id="print_bill" />
                </div>
                </div>
                <div class="col-md-4 col-sm-4">
                <div class="form-group">
                <label for="finalize_sale">Finalize Sale</label> <input type="text" name="finalize_sale" value="ALT+F8" class="form-control tip" id="finalize_sale" />
                </div>
                </div>
                <div class="col-md-4 col-sm-4">
                <div class="form-group">
                <label for="today_sale">Today's Sale</label> <input type="text" name="today_sale" value="Ctrl+F1" class="form-control tip" id="today_sale" />
                </div>
                </div>
                <div class="col-md-4 col-sm-4">
                <div class="form-group">
                <label for="open_hold_bills">Opened Bills</label> <input type="text" name="open_hold_bills" value="Ctrl+F2" class="form-control tip" id="open_hold_bills" />
                </div>
                </div>
                <div class="col-md-4 col-sm-4">
                <div class="form-group">
                <label for="close_register">Close Register</label> <input type="text" name="close_register" value="ALT+F7" class="form-control tip" id="close_register" />
                </div>
                </div>
                <div class="clearfix"></div>
                </div>
                </div>
                </div>
                <div class="row">
                <div class="col-md-12">
                <div class="form-group">
                <label for="logo">Login Logo</label> 
                <input type="file" name="userfile" id="logo" value="<%=logo_file %>"">
                </div>
                </div>
                </div>
                <div class="row">
                <div class="col-md-6">
                <div class="form-group">
                <label for="remote_printing">Printing</label> <select name="remote_printing" class="form-control select2" id="remote_printing" style="width:100%;">
                <option value="0">PHP Server (only for Localhost/Desktop)</option>
                <option value="1" selected="selected">Web Browser</option>
                <option value="2">PHP POS Print Server</option>
                </select>
                <span class="help-block">On local installation <strong>PHP Server</strong> will be the best choice and for live server, you can install <strong>PHP Pos Print Server</strong> locally.</span>
                <span class="help-block ppp">You even can purchase <a href="http://tecdiary.com/products/php-pos-print-server-windows-installer" target="_blank">PHP POS Print Server (Windows Installer)</a>.</span>
                <span class="help-block">On demo, you can test web printing only.</span>
                </div>
                </div>
                </div>
                <div class="clearfix"></div>
                <div class="row">
                <div class="col-md-12">
                <div class="well well-sm printers">
                <div class="ppp">
                <div class="col-md-6">
                <div class="form-group">
                <label for="local_printers">Use Local Printers</label> <select name="local_printers" class="form-control select2" id="local_printers" required="required">
                <option value="1">Yes</option>
                <option value="0">No</option>
                </select>
                </div>
                </div>
                </div>
                <div class="lp">
                <div class="col-md-6">
                <div class="form-group">
                <label for="receipt_printer">Receipt Printer</label> <strong>*</strong>
                <select name="receipt_printer" class="form-control select2" id="receipt_printer" style="width:100%;">
                </select>
                </div>
                </div>
                <div class="col-md-6">
                <div class="form-group">
                <label for="order_printers">Order Printers</label> <strong>*</strong>
                <select name="order_printers[]" multiple class="form-control select2" id="order_printers" style="width:100%;">
                </select>
                </div>
                </div>
                <div class="col-md-6">
                <div class="form-group">
                <label for="cash_drawer_codes">Cash Drawer Code</label> <input type="text" name="cash_drawer_codes" value="" class="form-control" id="cash_drawer_codes" placeholder="\x1C" />
                </div>
                </div>
                </div>
                <div class="">
                <div class="col-md-6">
                <div class="form-group">
                <label for="print_img">Send print as</label> <select name="print_img" class="form-control select2" id="print_img" required="required">
                <option value="0">Text</option>
                <option value="1">Image</option>
                </select>
                </div>
                </div>
                </div>
                <div class="clearfix"></div>
                </div>
                <div class="clearfix"></div>
                </div>
                </div>
                <input type="submit" id="update_settings" name="update_settings" value="Update Settings" class="btn btn-primary"  />
                </form> 
                </div>
                <div class="clearfix"></div>
            </div>
            </div>
            </div>
            </div>
        </section>

        <script src="https://ajax.cloudflare.com/cdn-cgi/scripts/78d64697/cloudflare-static/email-decode.min.js"></script>
        <script type="text/javascript">
                $(document).ready(function () {
                $("#order_printers").select2().select2('val', );
                if ($('#protocol').val() == 'smtp') {
                $('#smtp_config').slideDown();
                } else if ($('#protocol').val() == 'sendmail') {
                $('#sendmail_config').slideDown();
                }
                $('#protocol').change(function () {
                if ($(this).val() == 'smtp') {
                $('#sendmail_config').slideUp();
                $('#smtp_config').slideDown();
                } else if ($(this).val() == 'sendmail') {
                $('#smtp_config').slideUp();
                $('#sendmail_config').slideDown();
                } else {
                $('#smtp_config').slideUp();
                $('#sendmail_config').slideUp();
                }
                });
                if ($('#stripe').val() == 0) {
                $('#stripe_con').slideUp();
                } else {
                $('#stripe_con').slideDown();
                }
                $('#stripe').change(function () {
                if ($(this).val() == 0) {
                $('#stripe_con').slideUp();
                } else {
                $('#stripe_con').slideDown();
                }
                });
                if ($('#remote_printing').val() == 1) {
                $('.printers').slideUp();
                $('.ppp').slideUp();
                } else if ($('#remote_printing').val() == 0) {
                $('.printers').slideDown();
                $('.ppp').slideUp();
                $('.lp').slideDown();
                } else {
                $('.printers').slideDown();
                $('.ppp').slideDown();
                if ($('#local_printers').val() == 1) {
                $('.lp').slideUp();
                } else {
                $('.lp').slideDown();
                }
                }
                $('#remote_printing').change(function () {
                if ($(this).val() == 1) {
                $('.printers').slideUp();
                $('.ppp').slideUp();
                } else if ($(this).val() == 0) {
                $('.printers').slideDown();
                $('.ppp').slideUp();
                $('.lp').slideDown();
                } else {
                $('.printers').slideDown();
                $('.ppp').slideDown();
                if ($('#local_printers').val() == 1) {
                $('.lp').slideUp();
                } else {
                $('.lp').slideDown();
                }
                }
                });
                $('#local_printers').change(function () {
                if ($(this).val() == 1) {
                $('.lp').slideUp();
                } else {
                $('.lp').slideDown();
                }
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
    var Settings = { "logo": "logo1.png", "site_name": "SimplePOS", "tel": "0105292122", "dateformat": "D j M Y", "timeformat": "h:i A", "language": "english", "theme": "default", "mmode": "0", "captcha": "0", "currency_prefix": "USD", "default_customer": "3", "default_tax_rate": "5%", "rows_per_page": "10", "total_rows": "30", "header": "<h2><strong>Simple POS<\/strong><\/h2>\r\n       My Shop Lot, Shopping Mall,<br>\r\n                                                                                              Post Code, City<br>", "footer": "Thank you for your business!\r\n<br>", "bsty": "3", "display_kb": "0", "default_category": "1", "default_discount": "0", "item_addition": "1", "barcode_symbology": "", "pro_limit": "10", "decimals": "2", "thousands_sep": ",", "decimals_sep": ".", "focus_add_item": "ALT+F1", "add_customer": "ALT+F2", "toggle_category_slider": "ALT+F10", "cancel_sale": "ALT+F5", "suspend_sale": "ALT+F6", "print_order": "ALT+F11", "print_bill": "ALT+F12", "finalize_sale": "ALT+F8", "today_sale": "Ctrl+F1", "open_hold_bills": "Ctrl+F2", "close_register": "ALT+F7", "java_applet": "0", "receipt_printer": "", "pos_printers": "", "cash_drawer_codes": "", "char_per_line": "42", "rounding": "1", "pin_code": "abdbeb4d8dbe30df8430a8394b7218ef", "purchase_code": null, "envato_username": null, "theme_style": "green", "after_sale_page": null, "overselling": "1", "multi_store": "1", "qty_decimals": "2", "symbol": null, "sac": "0", "display_symbol": null, "remote_printing": "1", "printer": null, "order_printers": null, "auto_print": "0", "local_printers": null, "rtl": null, "print_img": null, "selected_language": "english" };
    $(window).load(function () {
        $('.mm_settings').addClass('active');
        $('#settings_index').addClass('active');
    });
    var lang = new Array();
    lang['code_error'] = 'Code Error';
    lang['r_u_sure'] = '<strong>Are you sure?</strong>';
    lang['register_open_alert'] = 'Register is open, are you sure to sign out?';
    lang['code_error'] = 'Code Error';
    lang['r_u_sure'] = '<strong>Are you sure?</strong>';
    lang['no_match_found'] = 'No match found';
</script>
<script src="/scripts/libraries.min.js" type="text/javascript"></script>
<script src="/scripts/scripts.min.js" type="text/javascript"></script>
<!--<script src="/scripts/spos_ad.min.js"  type="text/javascript"></!--script>-->
</body>






</html>
