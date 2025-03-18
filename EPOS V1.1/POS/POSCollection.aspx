<%@ Page Language="VB"   AutoEventWireup="false" EnableViewState="false" CodeFile="POSCollection.aspx.vb" Inherits="POS_POSPOSCollection"  Debug="true"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html>
<head runat="server">
  <meta runat="server" charset="UTF-8"  name="pos" content="Free Web tutorials">
  <meta name="keywords" content="HTML,CSS,XML,JavaScript">
    <title>POS | SimplePOS</title>
    <script type="text/javascript">  
        // for testing debuging
       // document.addEventListener('DOMContentLoaded', function () {
            // Your code here
       // });
       // function myFunc(f) {
            // f = "ddd";
            // alert("Test ok!" + "PLUid=" + f);
       //     get_product(f.id);
       // };
        function get_product(Product_ids) {
            //alert('hh')
            // alert('POSCustom.ln:242 Product_ids=' + Product_ids);
            $.ajax({
                type: "GET",
                url: "GetProduct.aspx",
                data: "act=addShipbill&Productid=" + Product_ids,
                success: function (msg) {
                   // alert('get_product.ln:22 msg=' + msg);
                    $("#divAddbill").html(msg);

                    //setTimeout(function(){$.unblockUI}, 50 );
                    // $.unblockUI();
                }
            });
            get_Totals("66");
        };
        function get_Totals(TicketID) {
            //alert('hh')
            // alert('POSCustom.ln:242 Product_ids=' + Product_ids);
            $.ajax({
                type: "GET",
                url: "GetTotals.aspx",
                data: "act=addShipbill&TicketID=" + TicketID,
                success: function (msg) {
                    $("#totaldiv").html(msg);

                    //setTimeout(function(){$.unblockUI}, 50 );
                    // $.unblockUI();
                }
            });
        };
        function FetchPOds(f,smid) {
            // f = "ddd";
            // alert("Test ok!!!" + "smid=" + smid);
            get_pods(f,smid);
            get_submenu(f);
            get_ButtonMenu(f);
        };
        function get_pods(depertmentList,smid) {
            //alert('hh')
            //alert('POSCustom.ln:242 Product_ids=' + Product_ids);


            $.ajax({
                type: "GET",
                url: "GetPODsAjax.aspx",
                data: "act=addShipbill&DepartmentList=" + depertmentList + "&smid=" + smid,
                success: function (msg) {
                    $("#PODsContainer").html(msg);

                    //setTimeout(function(){$.unblockUI}, 50 );
                    // $.unblockUI();
                }
            });
           
        };
        function changeQty(f) {
            // f = "ddd";
                       var res = f.id;
            res = res.replace("quantity_", "");
            res = res.replace("_", ",");
            var array = res.split(",");
            var ticket_id = array[0];
            var pid = array[1];
            var New_Quantty = f.value
            var Qdata = "act=update&Productid=" + pid + "&ticket_id=" + ticket_id + "&New_Quantty=" + New_Quantty
            //alert("Test ok!" + "val=" + New_Quantty + ", id=" + f.id + "\n" + "res=" + res + "\n Ticket=" + ticket_id + ", PID=" + pid + "\n Qdata=" + Qdata );
            ////UpdateQuantity;
            $.ajax({
                type: "GET",
                url: "UpdateTicketProductQty.aspx",
                data: Qdata,
                success: function (msg) {
                   // alert('changeQty.ln:101 msg=' + "\n" + '#########' + msg);
                    $("#divAddbill").html(msg);
                    //alert('After' + Qdata);
                   
                     //myFunc(0);
                }
                //,
                //error: function (xhr, text, error) {              
                    // If 40x or 50x; errors
                //    alert('Error: ' + error);
                //}
                    //setTimeout(function(){$.unblockUI}, 50 );
                    // $.unblockUI();
            });
           // endofprocess();
            get_Totals(ticket_id);    
        };
        function endofprocess() {
            alert('last' );
        };
        function process_payment(OrderId, gtotal, payby, count, note) {
            //alert('hh')
            alert('POSCustom02.aspx.process_payment.ln:115 OrderId=' + OrderId + 'gtotal=' + gtotal + ', count=' + count + ', payby=' + payby + ', note=' + note);
            var count = document.getElementById("count").innerHTML;
            var total = document.getElementById("total-payable").innerHTML;
            var payment_note = document.getElementById("payment_note").innerHTML;
            var amount = document.getElementById("total").innerHTML;
            
            $.ajax({
                url: "/Test_JQueryServerSide.aspx?TicketNo=" + OrderId + ",amount=" + amount + ',count=' + count + ',payby=' + payby + ',note=' + note + "", //  external server side URL 
                data: { message: "hello" },
                type: "POST",
                success: function (response) {
                    $("#popupContentReturn").text(response);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error:", textStatus, errorThrown);
                    // Handle errors appropriately (display error message)
                }
            });
            get_Totals("66"); /* triggers totals*/
        };
function get_submenu(Department_Code) {
        //alert('hh')
        //alert('get_submenu.js.ln:110 Department_Code=' + Department_Code);


        $.ajax({
            type: "GET",
            url: "GetSubmenu.aspx",
            data: "act=button&Department_Code=" + Department_Code ,
            success: function (msg) {
                $("#divSubMenu").html(msg);

                //setTimeout(function(){$.unblockUI}, 50 );
               
            }
        });
    };
function get_ButtonMenu(Department_Code) {
        //alert('hh')
        //alert('get_ButtonMenu.ln:128 Department_Code=' + Department_Code);


        $.ajax({
            type: "GET",
            url: "GetButtonMenu.aspx",
            data: "act=button&Department_Code=" + Department_Code ,
            success: function (msg) {
                $("#ButtonMenu").html(msg);

                //setTimeout(function(){$.unblockUI}, 50 );
               
            }
        });
        };

        function getTicketNo() {
            // Replace this placeholder with your actual ticket retrieval logic
            const urlParams = new URLSearchParams(window.location.search);
            const id = urlParams.get("TicketID");
            alert('function getTicketNo().ln 170 id=' + id + 'count=' + count + ',gtotal=' + gtotal);
            if (id) {
                // Potentially retrieve ticket number based on the ID
                const ticketNo = id;
                return ticketNo;
            } else {
                return "00";
            }
        };

        function getTicketNo2() {
            var id = getQueryStringValue('TicketID');
            //var language = getQueryStringValue('language');
            // document.getElementById("TicketNo").innerHTML = "<b>Id : </b>" + id 
            if (id = null) { id = "66"; };
            document.getElementById("orderid").value = getQueryStringValue('TicketID');
            id = getQueryStringValue('TicketID')
            return id;
        };
        function language() {
            var language = getQueryStringValue('language');
            // document.getElementById("TicketNo").innerHTML = "<b>Id : </b>" + id 
            return language;
        };

        function getQueryStringValue (parameter) {
            var currentPageURL = window.location.search.substring(1);
            var queryString = currentPageURL.split('&');
            var getParameter;
            var i;
            for (i = 0; i < queryString.length; i++) {
                getParameter = queryString[i].split('=');
                if (getParameter[0] === parameter) {
                    return getParameter[1] === undefined ? true : decodeURIComponent(getParameter[1]);
                }
            }
        };

        //document.getElementById("TicketNo").innerHTML = 'ggg';
        let TicketNo = document.getElementById("TicketNo");
            function welcomeFunction() {
                TicketNo.innerHTML = getTicketNo();

            };
    </script>
 
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

<script type="text/javascript" >
        $(document).ready(function () {
            $("#paymentZZ").click(function () {
                var count = document.getElementById("count").innerHTML;
                var total = document.getElementById("total-payable").innerHTML;
                var Ticketno = getTicketNo();
                alert('#paymentZZ.ln.246 ticketno=' + Ticketno +', count=' + count + ',gtotal=' + gtotal );
                //if (!Ticketno) { Ticketno = "zzzzz" }
                $("#TicketNo").innerHTML = Ticketno
                $("#orderid").innerHTML = Ticketno
            if (count <= 1) return bootbox.alert(lang.please_add_product + "..count:=" + count), !1;
            if (sid && (suspend = $("<span></span>"), suspend.html('<input type="hidden" name="delete_id" value="' + sid + '" />'),
                suspend.appendTo("#hidesuspend")),
                gtotal = formatDecimal(total - order_discount + order_tax), 0 != Settings.rounding)
                {
                round_total = roundNumber(gtotal, parseInt(Settings.rounding)); var t = formatDecimal(round_total - gtotal);
                $("#twt").text(formatMoney(round_total) + " (" + formatMoney(t) + ")"),
                $("#quick-payable").text(round_total)
                }
            else
                $("#twt").text(formatMoney(gtotal)),
                $("#quick-payable").text(gtotal);
                $("#item_count").text(an - 1 + " (" + (count ) + ")"),
                $("#order_quantity").val(count),
                $("#order_items").val(an - 1),
                $("#balance").text("0.00"),
                $("#orderid").text(Ticketno),
                $("#payModal").modal({ backdrop: "static" })
            });
            $("#submit-sale").click(function () {
                var count = document.getElementById("count").innerHTML;
                var gtotal = document.getElementById("total-payable").innerHTML;
                var payby = document.getElementById("paid_by").value;
                var note = document.getElementById("note").value;
                var mTicketID = document.getElementById("orderid").innerHTML;
                var OrderId = document.getElementById("orderid").innerHTML;
                if (mTicketID = null) { TicketID = 66 };
                alert('submit-sale ln 277 count=' + count + ',gtotal=' + gtotal + ',payby=' + payby + ',Note=' + note + ',mTicketID=zz' + mTicketID);
                mTicketID = document.getElementById("orderid").value;
                alert('submit-sale ln 278 TicketID=' + mTicketID);

                process_payment(OrderId);

            });
            $("#orderid").click(function () {
                var OrderId = document.getElementById("orderid").innerHTML;
                var count = document.getElementById("count").innerHTML;
                var gtotal = document.getElementById("total-payable").innerHTML;
                var payby = document.getElementById("paid_by").value;
                var note = document.getElementById("note").value;
                process_payment(OrderId, gtotal, payby, count, note);
            });
            $("#update-note").click(function (t) {
                var count = document.getElementById("count").innerHTML;
                var total = document.getElementById("total-payable").innerHTML;
                var payby = document.getElementById("paid_by").value;
                var note = document.getElementById("note").value;
                var TicketID = getQueryStringValue('TicketNo') 
                document.getElementById("TicketNo").innerHTML ;
                alert('update-note' + 'count=' + count + ',gtotal=' + gtotal + ',payby=' + payby + ',Note=' + note + ',TicketID=' + TicketID);

            });

        });

    //document.getElementById("TicketNo").innerHTML = 'OOOO';
</script>
</head>
<body class="skin-green sidebar-collapse sidebar-mini pos"   >
<form id="form2" runat="server">
                <input type="hidden" id="item_id" />
                <input type="hidden" id="row_id" />
                        <input type="hidden" name="gcname" value="Gift Card" id="gcname" />
                                    <input type="hidden" name="total_quantity" id="total_quantity" value="0" />
                                    <input type="hidden" name="total_items" id="total_items" value="0" />
                                    <input type="hidden" name="did" id="is_delete" value="0" />
                                    <input type="hidden" name="count" id="total_item" value="" />
                                    <input type="hidden" name="order_discount" id="discount_val" value="" />
                                    <input type="hidden" name="order_tax" id="tax_val" value="" />
                                    <input type="hidden" name="customer" id="customer" value="3" />
                                        <input type="hidden" name="payment_note" id="payment_note_val" value="" />
                                        <input type="hidden" name="cc_cvv2" id="cc_cvv2_val" value="" />
                                        <input type="hidden" name="cc_type" id="cc_type_val" value="" />
                                        <input type="hidden" name="cc_year" id="cc_year_val" value="" />
                                        <input type="hidden" name="cc_month" id="cc_month_val" value="" />
                                        <input type="hidden" name="cheque_no" id="cheque_no_val" value="" />
                                        <input type="hidden" name="cc_holder" id="cc_holder_val" value="" />
                                        <input type="hidden" name="paying_gift_card_no" id="paying_gift_card_no_val" value="" />
                                        <input type="hidden" name="cc_no" id="cc_no_val" value="" />
                                        <input type="hidden" name="paid_by" id="paid_by_val" value="cash" />
                                        <input type="hidden" name="balance_amount" id="balance_val" value="" />
                                        <input type="hidden" name="amount" id="amount_val" value="" />
                                    <input type="hidden" name="spos_note" value="" id="spos_note">
                                <input type="hidden" id="TicketID" name="TicketID" value="007" />
                                <input type="hidden" name="spos_token" value="1479a37b6443485bdaaf75fb256d1484" />
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
            </div>
        </div>
        <aside class="control-sidebar control-sidebar-dark" id="categories-list">
            <div class="tab-content sb">
                <div class="tab-pane active sb" id="control-sidebar-home-tab">
                    <div id="filter-categories-con">
                    <input type="text" autocomplete="off" data-list=".control-sidebar-menu" name="filter-categories" id="filter-categories" class="form-control sb col-xs-12 kb-text" placeholder="Type to filter categories" style="margin-bottom: 20px;">
                    </div>
                    <div class="clearfix sb"></div>
                    <div id="category-sidebar-menu">
                        <ul class="control-sidebar-menu">
                            <li>
                                <a href="#" class="category active" id="1">
                                <div class="menu-icon">
                                    <img src="../images/posimages/icons/no_image.png" alt="" class="img-thumbnail img-responsive">
                                </div>
                                <div class="menu-info"><h4 class="control-sidebar-subheading">
                                    G01</h4><p>General</p>
                                </div>
                                </a>
                            </li> 
                        </ul>
                    </div>
                </div>
            </div>
        </aside>
        <div class="control-sidebar-bg sb"></div>
    </div>
    <div id="order_tbl" style="display:none;"><span id="order_span"></span>
        <table id="order-table" class="prT table table-striped table-condensed" style="width:100%;margin-bottom:0;"></table>
    </div>
    <div id="bill_tbl" style="display:none;">
        <span id="bill_span"></span>
        <table id="bill-table" width="100%" class="prT table table-striped table-condensed" style="width:100%;margin-bottom:0;"></table>
        <table id="bill-total-table" width="100%" class="prT table table-striped table-condensed" style="width:100%;margin-bottom:0;"></table>
    </div>
    <div style="width:500px;background:#FFF;display:block">
        <div id="order-data" style="display:none;" class="text-center">
            <h1>SimplePOS</h1>
            <h2>Order</h2>
            <div id="preo" class="text-left"></div>
        </div>
        <div id="bill-data" style="display:none;" class="text-center">
            <h1>SimplePOS</h1>
            <h2>Bill</h2>
            <div id="preb" class="text-left"></div>
        </div>
    </div>

</form>
</body>
</html>


