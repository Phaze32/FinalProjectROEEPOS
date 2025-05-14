<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POS_Welcome - Copy (3).aspx.vb" Inherits="POS_Welcome" %>


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
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />
    <link href="../Styles/pos.css" rel="stylesheet" />

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
    <script type="text/javascript">
    $(document).ready(function () {
    $.ajax({
        url: '/Handlers/SalesSummaryHandler.ashx', // Adjust the path to your handler
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Prepare data for the graph
            let labels = [];
            let ticketTotals = [];
            let piecesTotals = [];

            data.forEach(function (row) {
                // Format the date label as "YYYY-MM-DD"
                labels.push(`${row.year}-${row.month}-${row.day}`);
                ticketTotals.push(row.TicketTotal); // Ticket totals for line 1
                piecesTotals.push(row.PiecesTotal); // Pieces totals for line 2
            });

            // Create the graph using Chart.js
            var ctx = document.getElementById('salesChart').getContext('2d');
            new Chart(ctx, {
                type: 'line', // Line graph
                data: {
                    labels: labels, // Dates (x-axis)
                    datasets: [
                        {
                            label: 'Ticket Total',
                            data: ticketTotals, // Ticket totals (y-axis)
                            borderColor: 'rgba(75, 192, 192, 1)',
                            backgroundColor: 'rgba(75, 192, 192, 0.2)',
                            fill: true // Area under the line filled
                        },
                        {
                            label: 'Pieces Total',
                            data: piecesTotals, // Pieces totals (y-axis)
                            borderColor: 'rgba(153, 102, 255, 1)',
                            backgroundColor: 'rgba(153, 102, 255, 0.2)',
                            fill: true // Area under the line filled
                        }
                    ]
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'Sales Summary'
                    },
                    scales: {
                        x: {
                            title: {
                                display: true,
                                text: 'Date'
                            }
                        },
                        y: {
                            title: {
                                display: true,
                                text: 'Amount'
                            }
                        }
                    }
                }
            });
        },
        error: function (error) {
            console.error('Error fetching data:', error);
        }
    });
    });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            // AJAX call to fetch data from SalesSummaryTodayHandler
            $.ajax({
                url: '/Handlers/SalesSummaryTodayHandler.ashx', // Adjust the path to your handler
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    // Prepare rows for the table
                    let tableBody = $('#SalesSummaryTable tbody');
                    tableBody.empty(); // Clear any existing rows

                    data.forEach(function (row) {
                        // Append a new row to the table with TicketID stored in a data attribute
                        tableBody.append(`
                    <tr class="table-row" data-ticket-id="${row.TicketID}">
                        <td>${row.TicketID}</td>
                        <td>${formatDate(row.TicketDate)}</td>
                        <td>${formatDate(row.CollectDate)}</td>
                        <td>${row.SurName}</td>
                        <td>${row.qty}</td>
                        <td>${row.total.toFixed(2)}</td>
                    </tr>
                `);
                    });

                    // Attach click event listener to rows
                    $('.table-row').on('click', function () {
                        const ticketID = $(this).data('ticket-id');
                        alert("tablerow before modal show");
                        // Load the modal and pass TicketID
                        $('#tsModal').modal('show'); // Bootstrap method to trigger modal
                        alert("tablerow after modal show");
                        loadFormInvoiceModal(ticketID); // Function to handle loading modal content
                    });
                },
                error: function (error) {
                    console.error('Error fetching data from handler:', error);
                }
            });

            function formatDate(dateString) {
                const date = new Date(dateString);
                // Extract day, month, and year
                const day = String(date.getDate()).padStart(2, '0'); // Ensure two digits for day
                const month = String(date.getMonth() + 1).padStart(2, '0'); // Month is 0-indexed
                const year = String(date.getFullYear()).slice(-2); // Last two digits of the year
                return `${day}-${month}-${year}`;
            }

            function loadFormInvoiceModal(ticketID) {
                $.ajax({
                    url: `/Handlers/InvoiceDetailHandler.ashx?TicketID=${ticketID}`,
                    type: 'GET',
                    dataType: 'json',
                    success: function (data) {
                        console.log("Response received:", data);

                        // Populate invoice details
                        $("#invoiceNumber").text(data.data[0].TicketID);
                        $("#customerName").text(data.data[0].ForeName || "N/A");
                        $("#customerSurname").text(data.data[0].SurName || "N/A");

                        let TicketDate = new Date(data.data[0].TicketDate);
                        let formattedDate2 = TicketDate.toLocaleDateString("en-GB") + " " + TicketDate.toLocaleTimeString("en-GB", { hour: "2-digit", minute: "2-digit", hour12: false });

                        $("#TicketDate").text(formattedDate2 || "N/A");
                        //let collectDate = data.data[0].CollectDate;
                        //let formattedDate = new Date(collectDate).toLocaleString("en-GB", { hour: "2-digit", minute: "2-digit", hour12: false });

                        //$("#CollectDate").text(formattedDate || "N/A");
                        let collectDate = new Date(data.data[0].CollectDate);
                        let formattedDate = collectDate.toLocaleDateString("en-GB") + " " + collectDate.toLocaleTimeString("en-GB", { hour: "2-digit", minute: "2-digit", hour12: false });

                        $("#CollectDate").text(formattedDate || "N/A");



                       // $("#CollectDate").text(data.data[0].CollectDate || "N/A");

                        // Populate table
                        const modalBody = $('#tsModal tbody');
                        modalBody.empty();

                        data.data.forEach(function (item) {
                            modalBody.append(`
                    <tr>
                        <td>${item.PLUDescription}</td>
                        <td style="text-align:center;">${item.PLUPrice.toFixed(2)}</td>
                        <td style="text-align:center;">${item.Qty}</td>
                        <td style="text-align:center;">${(item.PLUPrice * item.Qty).toFixed(2)}</td>
                        <td style="width: 20px; text-align:center;">
                            <i class="fa fa-trash-o"></i>
                        </td>
                    </tr>
                `);
                        });

                        $('#tsModal').modal('show');
                    },
                    error: function (error) {
                        console.error('Error loading modal content:', error);
                    }
                });
            }

            // Save button functionality
            $("#saveInvoice").click(function () {
                console.log("Invoice saved!");
                $('#tsModal').modal('hide');
            });
        });
    </script>

</head>
<body class="skin-green fixed sidebar-mini sidebar-collapse">
<style>
    #SalesSummaryTable {
        font-size: smaller;
    }
</style>
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
 
<section class="content" style="margin-left:50px">
    <div class="row">
    <div class="col-xs-12">
    <div class="box box-success">
    <div class="box-header">
    <h3 class="box-title">Quick Links</h3>
    </div>
    <div class="box-body">
        <a class="btn btn-app" href="/POS/posmain02.aspx?TicketID=66">
        <i class="fa fa-th"></i> POS </a>
        <a class="btn btn-app" href="/POS/POSProductlist.aspx">
        <i class="fa fa-barcode"></i> Products </a>
        <a class="btn btn-app" href="/POS/possaleslist.aspx">
        <i class="fa fa-shopping-cart"></i> Sales </a>
        <a class="btn btn-app" href="/POS/POSOrderList.aspx">
        <i class="fa fa-bell-o"></i>In Process</a>
        <a class="btn btn-app" href="/POS/POScategoryList.aspx">
        <i class="fa fa-folder-open"></i> Categories </a>
        <a class="btn btn-app" href="/POS/posgiftcardlist.aspx">
        <i class="fa fa-credit-card"></i> Gift Card </a>
        <a class="btn btn-app" href="/POS/POSCustomerList.aspx">
        <i class="fa fa-users"></i> Customers </a>
        <a class="btn btn-app" href="/POS/POSSettings.aspx">
        <i class="fa fa-cogs"></i> Settings </a>
        <a class="btn btn-app" href="/POS/POSreports.aspx">        
        <i class="fa fa-bar-chart-o"></i> Reports </a>
        <a class="btn btn-app" href="/POS/POSUsersList.aspx">
        <i class="fa fa-users"></i> Users </a>
        <a class="btn btn-app" href="javascript:void(0)" id="backupButton" >
        <i class="fa fa-database" ></i> Backups </a>

     </div>
    </div>
    <div class="row">
        <div class="col-md-8">
        <div class="box box-primary">
            <div class="box-header" >
                <h3 class="box-title">Sales Chart</h3>
            </div>
            <div class="box-body" >
                  <canvas id="salesChart" style="width: 100%; height: 400px;"></canvas>

            </div>
        </div>
        </div>
        <div class="col-md-4">
            <div class="box box-primary">
                <div class="box-header">
                    <h3 class="box-title" >
                        New Orders
                    </h3>
                 </div>
                <div class="box-body">
                   <div class="table-responsive">
                        <table id="SalesSummaryTable" class="table table-bordered table-striped">
                            <thead>
                                <tr>
                                    <th>Ticket ID</th>
                                    <th>Ticket Date</th>
                                    <th>Collect Date</th>
                                    <th>SurName</th>
                                    <th>Qty</th>
                                    <th>Total</th>
                                </tr>
                            </thead>
                            <tbody>
                                <!-- Rows will be appended dynamically via JavaScript -->
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
    </div>
</section>
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
   <!-- #include virtual = "/POS/FormInvoiceModal.html" -->
   <!-- #include virtual = "/POS/FormDatabaseBackupModal.html" --> 
</body>
</html>
