<%@ Page Language="VB"   AutoEventWireup="false" EnableViewState="false" CodeFile="POSMain02 - Copy (2).aspx.vb" Inherits="POS_POSMain02"  Debug="true"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html>
<head runat="server">
  <meta runat="server" charset="UTF-8"  name="pos" content="Free Web tutorials">
  <meta name="keywords" content="HTML,CSS,XML,JavaScript">
    <title>POS | SimplePOS</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <script src="../scripts/JSPOSMain.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jQuery-2.1.4.min.js"></script>
    <script src="../scripts/email-decode.min.js" type="text/javascript"></script>
    <script src="../Scripts/POSMain.js" type="text/javascript"></script>
    <script src="../scripts/libraries.min.js" type="text/javascript" ></script>
    <script src="../scripts/scripts.min.js" type="text/javascript"></script>
    <script src="../scripts/pos.min.js" type="text/javascript"></script>
    <!-- Include jQuery UI CSS and JS for the Datepicker and Dialog -->
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <!-- dropdown repalted -->

    <!-- Include Select2 CSS and JS -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        function fetchData(searchData) {
            $.ajax({
                url: 'AddEditCustomer.aspx',
                dataType: 'json',
                data: { act: 'searchcustomers', query: searchData },
                success: function (data) {
                    populateDropdown(data);
                },
                error: function (xhr, status, error) {
                    console.error('Failed to fetch data: ', status, error);
                }
            });
        }

        function populateDropdown(data) {
            var dropdown = $('#customer-dropdown');
            dropdown.empty(); // Clear existing options
            dropdown.append('<option selected="true" disabled>Choose Customer</option>');
            dropdown.prop('selectedIndex', 0);

            $.each(data, function (key, entry) {
                dropdown.append($('<option></option>').attr('value', entry.id).text(entry.name));
            });
        }

        $("#customer-dropdown").select2({
            placeholder: "Select or type customer name",
            allowClear: true,  // Allow clear option
            minimumInputLength: 3,  // Trigger AJAX call after typing 3 letters
            ajax: {
                url: 'AddEditCustomer.aspx',
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        act: 'searchcustomers',
                        query: params.term  // search term
                    };
                },
                processResults: function (data) {
                    return {
                        results: $.map(data, function (item) {
                            return {
                                id: item.id,
                                text: item.name
                            };
                        })
                    };
                },
                cache: true
            },
            templateResult: function (customer) {
                return customer.name || customer.text;
            },
            templateSelection: function (customer) {
                return customer.name || customer.text;
            }
        }).on('select2:select', function (e) {
            var selectedId = e.params.data.id;
            $('#customer_id').val(selectedId); // Update the text box with the selected option ID
        });
    });
</script>   
    <style>
        .select2-container {
            width: 100% !important;
        }
        .select2-selection__clear {
            right: 24px;  /* Adjust this value to move the 'x' to the left */
        }
    </style>
     <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />
    <link href="../Styles/pos.css" rel="stylesheet" />

<style type="text/css">
        .auto-style1 {
            text-align: right;
            width: 89px;
        }
    .auto-style2 {
        width: 267px;
        height: 189px;
        }
     .select2-selection__clear {
            right: 15px;  /* Adjust this value to move the 'x' to the left */
        }
    </style>
</head>
<body class="skin-green sidebar-collapse sidebar-mini pos"   >
<form id="form2" runat="server">
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
                <div class="col-lg-12 alerts"></div>
               
                <table style="width:100%;" class="layout-table">
                    <tr>
                    <td style="width: 460px;">
                        <%-- Basket Begins --%>
                        <div id="pos">
                            <form action="pos/posmain02.aspx" id="pos-sale-form" method="post" accept-charset="utf-8">
                                <input type="hidden" name="spos_token" value="1479a37b6443485bdaaf75fb256d1484" />
                                <input type="hidden" id="TicketID" name="TicketID" value="007" />
                                <div class="well well-sm" id="leftdiv">
                                    <div id="lefttop" style="margin-bottom:5px;">
                                        <div class="form-group" style="margin-bottom:5px;">
                                            <div class="input-group">
                                               <select name="customer_id2" id="spos_customer" data-placeholder="Select Customer" required="required" class="form-control select2" style="width:100%;position:absolute;">
                                                    <option value="1">Walk-in Client</option>
                                                    <option value="2">Compay  Client</option>
                                                </select>
                                                  <div class="input-group-addon no-print" style="padding: 2px 5px;">
                                                    <a href="#" id="add-customer" class="external" data-toggle="modal" data-target="#myModal"><i class="fa fa-2x fa-plus-circle" id="addIcon"></i></a>
                                                </div>
                                            </div>
                                            <div style="clear:both;"></div>
                                        </div>
                                        <div class="form-group" style="margin-bottom:5px;">
                                            <!--<asp:TextBox EnableViewState="false" name="hold_ref" value="" id="hold_ref" class="form-control kb-text" placeholder="Reference Note" OnTextChanged="hold_ref_TextChanged" runat="server"></asp:TextBox>-->
                                                <select name="customer_id" id="customer-dropdown" data-placeholder="Select or type customer name" required="required" class="form-control select2" style="width:100%;position:absolute;">
                                                    <!-- Options will be populated here -->
                                                </select>
                                           <input type="hidden" id="customer_id" name="customer_id" />
                                        </div>
                                        <div class="form-group" style="margin-bottom:5px;">
                                            <asp:TextBox  EnableViewState="false" type="text" name="codeq" id="txtSearch"  class="form-control" style="z-index:2000!important;" placeholder="Search name of service"  runat="server" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                                            <cc1:autocompleteextender ServiceMethod="SearchCustomers"
                                                MinimumPrefixLength="2"
                                                CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                                TargetControlID="txtSearch"
                                                ID="AutoCompleteExtender1" runat="server" >
                                            </cc1:autocompleteextender>
                                        </div>
                                    </div>
                                    <div id="printhead" class="print">
                                        <h2><strong>Simple POS</strong></h2>
                                        My Shop Lot, Shopping Mall,<br>
                                        Post Code, City<br> <p>Date: Wed 1 Nov 2017</p>
                                    </div>
                                    <%--Basket Begins --%>
                                    <div id="print" class="fixed-table-container">
                                        <div id="list-table-div">
                                            <div class="fixed-table-header">
                                                <table class="table table-striped table-condensed table-hover list-table" style="margin:0;">
                                                    <thead>
                                                        <tr class="success">
                                                            <th>Product</th>
                                                            <th style="width: 15%;text-align:center;">Price</th>
                                                            <th style="width: 15%;text-align:center;">Qty</th>
                                                            <th style="width: 20%;text-align:center;">Total</th>
                                                            <th style="width: 20px;" class="satu"><i class="fa fa-trash-o"></i></th>
                                                        </tr>
                                                    </thead>
                                                </table>
                                                <!--Basket-->
                                                    <asp:Panel id="panel1" runat="server"  EnableViewState="false" >
                                                    <!--<div id="divAddbill" style="z-index:20000;overflow-y: scroll;height:510px;"></div> -->
                                                     <div id="divAddbill" style="z-index:20000; overflow-y: scroll; max-height: 510px; height: 100vh;"></div>

                                                    </asp:Panel>
                                               <!--Basket ends-->
                                            </div>

                                            <% If 1 = 2 Then  %>
                                            <table id="posTable" class="table table-striped table-condensed table-hover list-table" style="margin:0px;" data-height="100">
                                                <thead>
                                                    <tr class="success">
                                                        <th>Product</th>
                                                        <th style="width: 15%;text-align:center;">Price</th>
                                                        <th style="width: 15%;text-align:center;">Qty</th>
                                                        <th style="width: 20%;text-align:center;">Subtotal</th>
                                                        <th style="width: 20px;" class="satu"><i class="fa fa-trash-o"></i></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                            <% End If %>
                                        </div>
                                        <div style="clear:both;"></div>
                                        <div id="totaldiv">
                                            <!--totals under the basket -->
                                            <table id="totaltbl" class="table table-condensed totals" style="margin-bottom:10px;margin-top:40px">
                                                <tbody>
                                                    <tr class="info">
                                                        <td width="25%">Total Items</td>
                                                        <td class="text-right" style="padding-right:10px;"><span id="count">0</span></td>
                                                        <td width="25%">Total</td>
                                                        <td class="auto-style1" colspan="2"><span id="total">0</span></td>
                                                        </tr>
                                                        <tr class="info">
                                                        <td width="25%"><a href="#" id="add_discount">Discount</a></td>
                                                        <td class="text-right" style="padding-right:10px;"><span id="ds_con">0</span></td>
                                                        <td width="25%"><a href="#" id="add_tax">Order Tax</a></td>
                                                        <td class="auto-style1"><span id="ts_con">0</span></td>
                                                        </tr>
                                                        <tr class="success">
                                                         <td width="8%" >Collection on</td>
                                                         <td width="30%" >
                                                           <input type="text" data-placeholder="Give date" id="selected_date" name="selected_date" size="17" onclick="handleDatePicker()" />

                                                         </td>
                                                        <td width="30%"  style="font-weight:bold;">
                                                        Total Payable 
                                                        <a role="button" data-toggle="modal" data-target="#noteModal">
                                                            <i class="fa fa-comment"></i>
                                                        </a>
                                                        </td>
                                                        <td width="20%" class="text-right"  style="font-weight:bold;"><span id="total-payable">0</span></td>
                                                      </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="botbuttons" class="col-xs-12 text-center">
                                        <div class="row">
                                            <div class="col-xs-4" style="padding: 0;">
                                                <div class="btn-group-vertical btn-block">
                                                <button type="button" class="btn btn-warning btn-block btn-flat" id="suspend">Hold</button>
                                                <button type="button" class="btn btn-danger btn-block btn-flat" id="reset">Cancel</button>
                                                </div>
                                            </div>
                                            <div class="col-xs-4" style="padding: 0 5px;">
                                                <div class="btn-group-vertical btn-block">
                                                <button type="button" class="btn bg-purple btn-block btn-flat" id="print_order">Print Order</button>
                                                <button type="button" class="btn bg-navy btn-block btn-flat" id="print_bill">Print Bill</button>
                                                </div>
                                            </div>
                                            <div class="col-xs-4" style="padding: 0;">
                                                <button type="button" class="btn btn-success btn-block btn-flat" id="paymentZZ" style="height:67px;">Payment..</button>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <span id="hidesuspend"></span>
                                    <input type="hidden" name="spos_note" value="" id="spos_note">
                                    <div id="payment-con">
                                        <input type="hidden" name="amount" id="amount_val" value="" />
                                        <input type="hidden" name="balance_amount" id="balance_val" value="" />
                                        <input type="hidden" name="paid_by" id="paid_by_val" value="cash" />
                                        <input type="hidden" name="cc_no" id="cc_no_val" value="" />
                                        <input type="hidden" name="paying_gift_card_no" id="paying_gift_card_no_val" value="" />
                                        <input type="hidden" name="cc_holder" id="cc_holder_val" value="" />
                                        <input type="hidden" name="cheque_no" id="cheque_no_val" value="" />
                                        <input type="hidden" name="cc_month" id="cc_month_val" value="" />
                                        <input type="hidden" name="cc_year" id="cc_year_val" value="" />
                                        <input type="hidden" name="cc_type" id="cc_type_val" value="" />
                                        <input type="hidden" name="cc_cvv2" id="cc_cvv2_val" value="" />
                                        <%--<input type="hidden" name="balance" id="balance_val" value="" />--%>
                                        <input type="hidden" name="payment_note" id="payment_note_val" value="" />
                                    </div>
                                    <input type="hidden" name="customer" id="customer" value="3" />
                                    <input type="hidden" name="order_tax" id="tax_val" value="" />
                                    <input type="hidden" name="order_discount" id="discount_val" value="" />
                                    <input type="hidden" name="count" id="total_item" value="" />
                                    <input type="hidden" name="did" id="is_delete" value="0" />
                                    <%--<input type="hidden" name="eid" id="is_delete" value="0" />--%>
                                    <input type="hidden" name="total_items" id="total_items" value="0" />
                                    <input type="hidden" name="total_quantity" id="total_quantity" value="0" />
                                    <input type="submit" id="submit" value="Submit Sale" style="display: none;" />
                                </div>
                            </form> 
                        </div>
                        <%-- Basket Ends --%>
                    </td>
                    <td>
                        <div class="contents" id="right-col">
                            <div id="item-list">
                                <div class="items">
                                    <asp:UpdatePanel  EnableViewState="false" ID="upPan" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div id ="ButtonMenu">
                                            <asp:Button   EnableViewState="false" ID="btn_FetchPods" runat="server" Text="Dryclean"  Height="70" Width="70"  OnClientClick="FetchPOds('d','0'); return false;" />
                                            <asp:Button ID="btn_FetchPods0" runat="server" EnableViewState="false" Height="70" OnClientClick="FetchPOds('l,w','0'); return false;" Text="Laundry" Width="70" />
                                            <asp:Button ID="btn_FetchPods1" runat="server" EnableViewState="false" Height="70" OnClientClick="FetchPOds('r','0'); return false;" Text="Repair" Width="70" />
                                            <asp:Button ID="btn_FetchPods2" runat="server" EnableViewState="false" Height="70" OnClientClick="FetchPOds('H,S','0'); return false;" Text="Others" Width="70" />
                                            <asp:Button ID="Button6" runat="server" EnableViewState="false" Height="70" OnClientClick="FetchPOds('D,H,L,R,S,W','0'); return false;" Text="ALL" Width="70" />
                                           </div> 
                             <div id= "divSubMenu" class="" style="background-color:aqua;" >
                                 <div id="TicketNo" onload="welcomeFunction();">
                    <asp:Literal ID="Literal_OrderID" runat="server"></asp:Literal></div>
                            </div>
                                            <br>
                                            <div id="PODsContainer" style="float:left"><asp:Literal ID="Literal1" EnableViewState="false" runat="server" Visible="true"></asp:Literal></div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            
                            <div class="product-nav">

                            </div>
                        </div>
                    </td>
                    </tr>
                </table>
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
    <div id="ajaxCall">
        <i class="fa fa-spinner fa-pulse"></i>
    </div>
    <div class="modal" data-easein="flipYIn" id="gcModal" tabindex="-1" role="dialog" aria-labelledby="mModalLabel" aria-hidden="true">
        <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            <i class="fa fa-times"></i></button>
                        <h4 class="modal-title" id="myModalLabel">Sell Gift Card</h4>
                    </div>
                    <div class="modal-body">
                        <p>Please fill in the information below</p>
                        <div class="alert alert-danger gcerror-con" style="display: none;">
                        <button data-dismiss="alert" class="close" type="button">×</button>
                        <span id="gcerror"></span>
                        </div>
                        <div class="form-group">
                            <label for="gccard_no">Card No</label> *
                            <div class="input-group">
                            <input type="text" name="gccard_no" value="" class="form-control" id="gccard_no" />
                            <div class="input-group-addon" style="padding-left: 10px; padding-right: 10px;"><a href="#" id="genNo"><i class="fa fa-cogs"></i></a></div>
                            </div>
                        </div>
                        <input type="hidden" name="gcname" value="Gift Card" id="gcname" />
                        <div class="form-group">
                            <label for="gcvalue">Value</label> *
                            <input type="text" name="gcvalue" value="" class="form-control" id="gcvalue" />
                        </div>
                        <div class="form-group">
                            <label for="gcprice">Price</label> *
                            <input type="text" name="gcprice" value="" class="form-control" id="gcprice" />
                        </div>
                        <div class="form-group">
                            <label for="gcexpiry">Expiry Date</label> <input type="text" name="gcexpiry" value="" class="form-control" id="gcexpiry" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                        <button type="button" id="addGiftCard" class="btn btn-primary">Sell Gift Card</button>
                    </div>
                </div>
        </div>
    </div>
    <div class="modal" data-easein="flipYIn" id="dsModal" tabindex="-1" role="dialog" aria-labelledby="dsModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><i class="fa fa-times"></i></button>
                    <h4 class="modal-title" id="dsModalLabel">Discount (5 or 5%)</h4>
                </div>
                <div class="modal-body">
                    <input type='text' class='form-control input-sm kb-pad' id='get_ds' onClick='this.select();' value=''>
                    <label class="checkbox" for="apply_to_order">
                    <input type="radio" name="apply_to" value="order" id="apply_to_order" checked="checked" />
                    Apply to order total </label>
                    <label class="checkbox" for="apply_to_products">
                    <input type="radio" name="apply_to" value="products" id="apply_to_products" />
                    Apply to all order items </label>
                    </div>
                    <div class="modal-footer">
                    <button type="button" class="btn btn-default btn-sm pull-left" data-dismiss="modal">Close</button>
                    <button type="button" id="updateDiscount" class="btn btn-primary btn-sm">Update</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal" data-easein="flipYIn" id="tsModal" tabindex="-1" role="dialog" aria-labelledby="tsModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><i class="fa fa-times"></i></button>
                    <h4 class="modal-title" id="tsModalLabel">Tax (5 or 5%)</h4>
                </div>
                <div class="modal-body">
                    <input type='text' class='form-control input-sm kb-pad' id='get_ts' onClick='this.select();' value=''>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default btn-sm pull-left" data-dismiss="modal">Close</button>
                    <button type="button" id="updateTax" class="btn btn-primary btn-sm">Update</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal" data-easein="flipYIn" id="noteModal" tabindex="-1" role="dialog" aria-labelledby="noteModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><i class="fa fa-times"></i></button>
                    <h4 class="modal-title" id="noteModalLabel">Note</h4>
                </div>
                <div class="modal-body">
                        <textarea name="snote" id="snote" class="pa form-control kb-text"></textarea>
                    </div>
                    <div class="modal-footer">
                    <button type="button" class="btn btn-default btn-sm pull-left" data-dismiss="modal">Close</button>
                    <button type="button" id="update-note" class="btn btn-primary btn-sm">Update</button>
                    </div>
            </div>
        </div>
    </div>
    <div class="modal" data-easein="flipYIn" id="proModal" tabindex="-1" role="dialog" aria-labelledby="proModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
            <div class="modal-header modal-primary">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><i class="fa fa-times"></i></button>
                <h4 class="modal-title" id="proModalLabel">
                Payment </h4>
            </div>
            <div class="modal-body">
                <table class="table table-bordered table-striped">
                    <tr>
                        <th style="width:25%;">Net Price</th>
                        <th style="width:25%;"><span id="net_price"></span></th>
                        <th style="width:25%;">Product Tax</th>
                        <th style="width:25%;"><span id="pro_tax"></span> <span id="pro_tax_method"></span></th>
                    </tr>
                </table>
                <input type="hidden" id="row_id" />
                <input type="hidden" id="item_id" />
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label for="nPrice">Unit Price</label> <input type="text" class="form-control input-sm kb-pad" id="nPrice" onClick="this.select();" placeholder="New Price">
                        </div>
                        <div class="form-group">
                            <label for="nDiscount">Discount</label> <input type="text" class="form-control input-sm kb-pad" id="nDiscount" onClick="this.select();" placeholder="Discount">
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label for="nQuantity">Quantity</label> <input type="text" class="form-control input-sm kb-pad" id="nQuantity" onClick="this.select();" placeholder="Current Quantity">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <label for="nComment">Comment</label> <textarea class="form-control kb-text" id="nComment"></textarea>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                <button class="btn btn-success" id="editItem">Update</button>
            </div>
            </div>
        </div>
    </div>
    <div class="modal" data-easein="flipYIn" id="susModal" tabindex="-1" role="dialog" aria-labelledby="susModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><i class="fa fa-times"></i></button>
                    <h4 class="modal-title" id="susModalLabel">Suspend Sale</h4>
                </div>
                <div class="modal-body">
                    <p>Type Reference Note</p>
                    <div class="form-group">
                        <label for="reference_note">Reference Note</label> <input type="text" name="reference_note" value="" class="form-control kb-text" id="reference_note" />
                    </div>
                </div>
                <div class="modal-footer">
                <button type="button" class="btn btn-default pull-left" data-dismiss="modal"> Close </button>
                <button type="button" id="suspend_sale" class="btn btn-primary">Submit</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal" data-easein="flipYIn" id="saleModal" tabindex="-1" role="dialog" aria-labelledby="saleModalLabel" aria-hidden="true">
              <-- #include virtual = "/POS/FormSaleModal.html" -->
             <-- Note: above form has been made redundent-->
    </div>
    <div class="modal" data-easein="flipYIn" id="opModal" tabindex="-1" role="dialog" aria-labelledby="opModalLabel" aria-hidden="true"></div>
   
     <!-- #include virtual = "/POS/FormpayModal.html" -->
     <!-- #include virtual = "/POS/FormCustomerModal.html" -->
    
    <div class="modal" data-easein="flipYIn" id="posModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true"></div>
    <div class="modal" data-easein="flipYIn" id="posModal2" tabindex="-1" role="dialog" aria-labelledby="myModalLabel2" aria-hidden="true"></div>

</form>
</body>
</html>


