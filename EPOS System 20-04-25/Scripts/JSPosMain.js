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
function FetchPOds(f, smid) {
    // f = "ddd";
    // alert("Test ok!!!" + "smid=" + smid);
    get_pods(f, smid);
    get_submenu(f);
    get_ButtonMenu(f);
};
function get_pods(depertmentList, smid) {
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
    alert('last');
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
        data: "act=button&Department_Code=" + Department_Code,
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
        data: "act=button&Department_Code=" + Department_Code,
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

function getQueryStringValue(parameter) {
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

$(document).ready(function () {
    var count = $("#count").html()

    $("#paymentZZ").click(function () {
        var count = document.getElementById("count").innerHTML;
        var total = document.getElementById("total-payable").innerHTML;
        var Ticketno = getTicketNo();
        alert('#paymentZZ.ln.246 ticketno=' + Ticketno + ', count=' + count + ',gtotal=' + gtotal);
        //if (!Ticketno) { Ticketno = "zzzzz" }
        $("#TicketNo").innerHTML = Ticketno
        $("#orderid").innerHTML = Ticketno
        if (count <= 1) return bootbox.alert(lang.please_add_product + "..count:=" + count), !1;
        if (sid && (suspend = $("<span></span>"), suspend.html('<input type="hidden" name="delete_id" value="' + sid + '" />'),
            suspend.appendTo("#hidesuspend")),
            gtotal = formatDecimal(total - order_discount + order_tax), 0 != Settings.rounding) {
            round_total = roundNumber(gtotal, parseInt(Settings.rounding)); var t = formatDecimal(round_total - gtotal);
            $("#twt").text(formatMoney(round_total) + " (" + formatMoney(t) + ")"),
                $("#quick-payable").text(round_total)
        }
        else
            $("#twt").text(formatMoney(gtotal)),
                $("#quick-payable").text(gtotal);
        $("#item_count").text(an - 1 + " (" + (count) + ")"),
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
        document.getElementById("TicketNo").innerHTML;
        alert('update-note' + 'count=' + count + ',gtotal=' + gtotal + ',payby=' + payby + ',Note=' + note + ',TicketID=' + TicketID);

    });

    $("#print_order").click(function (t) {
        $('#orderModal').modal({ backdrop: 'static' }); // displays the modal orderModal
        t.preventDefault();
        var count = $("#count").html()
        var TicketID = 13869
        var rv = ""
        var url = "http://localhost:8069/POS/TestInvoicePopup.html?customer=Mandy%20Colman&ticketID=66&discount=5.00&product0=Trousers&price0=4.95&quantity0=3&total0=14.85&product1=Evening%20Dress&price1=15.00&quantity1=1&total1=15.00&product2=Shirt&price2=2.60&quantity2=1&total2=2.60&DateReceived=02/12/2025%2000:00:01&DateReceived=27/12/2025%2000:00:01";

        if (count <= 1) {
            bootbox.alert(lang.please_add_product + '\n##JPOSMAIN.JS#print_order#ln282##count=' + count);
        } else {
            bootbox.alert("save order procedure begins" + '\n##JPOSMAIN.#print_order#ln284##count=' + count)
            checkCollectDate();
            rv = saveOrder(TicketID)
            rv = document.getElementById('responseStorage').value
            alert("print_order.saveOrder.rv=" + rv)
                // fetchInvoiceData(rv)
            setTimeout(function () {
                $("#order-data").hide();
            }, 500);
            processInvoice(url);
        }
        bootbox.alert(rv);
       
        return rv;
    });
    $("#print_order22").click(function (t) {
        t.preventDefault();
        var count = $("#count").html()
        if (count <= 1) {
            bootbox.alert(lang.please_add_product + '##JPOSMAIN.JS##' + count);
        } else {
            bootbox.alert("save order procedure begins" + '##JPOSMAIN.#print_order##count=' + count)
            if (Settings.remote_printing == 0) {
                $("#order-data").show();

                if (Settings.print_img == 1) {
                    $("#preo").html('<pre style="background:#FFF;font-size:20px;margin:0;border:0;">' + order_data.info + order_data.items + "</pre>");
                    html2canvas($("#order-data"), {
                        onrendered: function (canvas) {
                            var imgData = canvas.toDataURL().split(",")[1];
                            $.post(base_url + "pos/receipt_img", { img: imgData, spos_token: csrf_hash });
                        }
                    });
                } else {
                    var serializedData = $("#pos-sale-form").serialize();
                    $.post(base_url + "pos/p/order", serializedData);
                }

            } else {
                printOrder(order_data);
            }

            setTimeout(function () {
                $("#order-data").hide();
            }, 500);
        }

        return false;
    });
    // Initialize the datepicker
    $("#datepicker").datepicker({
        dateFormat: "dd/mm/yy", // Set the date format to British DD/MM/YYYY
        onSelect: function (dateText) {
            // Set the selected date in the hidden input field
            $("#selected_date").val(dateText);
            console.log("Selected date: " + dateText);
            // Close the dialog after a date is selected
            $("#datepicker-dialog").dialog("close");
        }
    });

    // Initialize the dialog
    $("#datepicker-dialog").dialog({
        autoOpen: false, // Don't open the dialog automatically
        modal: true, // Make it a modal dialog
        title: "Select a Date"
    });

    // Function to open the datepicker dialog
    function openDatePickerDialog() {
        $("#datepicker-dialog").dialog("open");
    }

    // Main function to handle date picker logic
    function handleDatePicker() {
        openDatePickerDialog();
    }

    // Bind the handleDatePicker function to the button click event
    $("#open_datepicker_button").click(function () {
        handleDatePicker();
    });

    // Display the selected date as an alert when the button is clicked
    $("#display_selected_date_button").click(function () {
        var selectedDate = $("#selected_date").val();
        alert("Selected date: " + selectedDate);
    });

    
    $("#add_discount").click(function () {
        alert("pos003.js#add_discount");
        var dval = $('#discount_val').val();
        $('#get_ds').val(dval);
        $('#dsModal').modal({ backdrop: 'static' });
        return false;
    });
    $('#dsModal').on('shown.bs.modal', function () {
        $('#get_ds').focusToEnd();
    });
});
function handleDatePicker() {
    openDatePickerDialog();
}
function openDatePickerDialog() {
    $("#datepicker-dialog").dialog("open");
}

function checkCollectDate() {
    var collectDateText = $("#CollectDate").html();
    var isValidDate = !isNaN(Date.parse(collectDateText));

    if (!isValidDate) {
        handleDatePicker();
    }
}

function saveOrder(TicketID) {
    //alert("ln399.JSPosMain.saveOrder.TicketID=" + TicketID);
    // Ensure TicketID is valid
    if (typeof TicketID !== 'number' || !Number.isInteger(TicketID)) {
        console.error("Invalid TicketID: must be an integer.");
        return;
    }
    // Retrieve the customer ID from the hidden input field
    const customerID = document.getElementById('customer_id_Selected').value;
    const CustomerAccNo = document.getElementById('customer_CustomerAccNo').value;
    // Ensure customerID is not empty
    if (!customerID) {
        console.error("Invalid customerID: must not be empty.");
        alert("Failed to save order. Customer ID is missing.");
        return;
    }

    // Log saving order
    console.log("Saving order with TicketID:", TicketID, "and CustomerID:", customerID);
    //alert("Saving order...TicketID=" + TicketID);

    // Example AJAX call to save the order
    $.ajax({
        url: '/Handlers/SaveOrder.ashx',
        type: 'GET',
        data: {
            TicketID: TicketID,      // Send TicketID as data to server
            CustomerID: customerID,  // Send CustomerID as data to server
            CustomerAccNo: CustomerAccNo  // Send CustomerID as data to server
        },
        success: function (response) {
            console.log("Order saved successfully!", response);
            alert("Order saved successfully!!!!" + response);
            const domElement = document.getElementById('responseStorage');
            domElement.value = convertToNumber(response); // Save response as a Number

        },
        error: function (xhr, status, error) {
            console.error("Failed to save order:", status, error);
            alert("Failed to save order. Please try again.");
        }
    });
}

function processInvoice(url) {
    const params = new URL(url).searchParams;
    alerrt("JSPosMain.processInvoice ln 440")
    // Extract general invoice details
    const customer = params.get("customer") || "N/A";
    const ticketID = params.get("ticketID") || "N/A";
    const discount = parseFloat(params.get("discount")) || 0.00;
    const dateReceived = params.getAll("DateReceived").join(", "); // Handle multiple DateReceived values
    const detePromised = params.get("DetePromised") || "N/A";

    // Extract product details
    const products = [];
    let index = 0;

    while (params.has(`product${index}`)) {
        products.push({
            product: params.get(`product${index}`),
            price: parseFloat(params.get(`price${index}`)),
            quantity: parseInt(params.get(`quantity${index}`)),
            total: parseFloat(params.get(`total${index}`))
        });
        index++;
    }

    // Calculate totals
    let totalQty = 0;
    let grandTotal = 0;

    products.forEach((item) => {
        totalQty += item.quantity;
        grandTotal += item.total;
    });

    grandTotal -= discount;

    // Create the HTML structure for the invoice
    const invoiceHTML = `
        <div style="padding: 20px; font-family: Arial, sans-serif; border: 1px solid #ccc; border-radius: 10px; max-width: 600px; margin: auto; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);">
            <h2 style="text-align: center;">Peaches and Clean Invoice</h2>
            <p><strong>Invoice #:</strong> ${ticketID}</p>
            <p><strong>Customer:</strong> ${customer}</p>
            <p><strong>Received Dates:</strong> ${dateReceived}</p>
            <p><strong>Promised Date:</strong> ${detePromised}</p>
            <table style="width: 100%; border-collapse: collapse; margin-top: 20px;">
                <thead>
                    <tr>
                        <th style="border: 1px solid #ddd; padding: 8px; background-color: #f4f4f4;">#</th>
                        <th style="border: 1px solid #ddd; padding: 8px; background-color: #f4f4f4;">Product</th>
                        <th style="border: 1px solid #ddd; padding: 8px; background-color: #f4f4f4;">Price (£)</th>
                        <th style="border: 1px solid #ddd; padding: 8px; background-color: #f4f4f4;">Qty</th>
                        <th style="border: 1px solid #ddd; padding: 8px; background-color: #f4f4f4;">Total (£)</th>
                    </tr>
                </thead>
                <tbody>
                    ${products
            .map((item, idx) => `
                            <tr>
                                <td style="border: 1px solid #ddd; padding: 8px; text-align: center;">${idx + 1}</td>
                                <td style="border: 1px solid #ddd; padding: 8px;">${item.product}</td>
                                <td style="border: 1px solid #ddd; padding: 8px; text-align: center;">£${item.price.toFixed(2)}</td>
                                <td style="border: 1px solid #ddd; padding: 8px; text-align: center;">${item.quantity}</td>
                                <td style="border: 1px solid #ddd; padding: 8px; text-align: right;">£${item.total.toFixed(2)}</td>
                            </tr>
                        `)
            .join("")}
                    <tr>
                        <td colspan="4" style="border: 1px solid #ddd; padding: 8px; text-align: right;"><strong>Total Pieces:</strong></td>
                        <td style="border: 1px solid #ddd; padding: 8px; text-align: right;"><strong>${totalQty}</strong></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="border: 1px solid #ddd; padding: 8px; text-align: right;"><strong>Discount:</strong></td>
                        <td style="border: 1px solid #ddd; padding: 8px; text-align: right;"><strong>£${discount.toFixed(2)}</strong></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="border: 1px solid #ddd; padding: 8px; text-align: right;"><strong>Grand Total:</strong></td>
                        <td style="border: 1px solid #ddd; padding: 8px; text-align: right;"><strong>£${grandTotal.toFixed(2)}</strong></td>
                    </tr>
                </tbody>
            </table>
        </div>
    `;

    // Insert the HTML into a specific element on your page (e.g., a container)
    const invoiceContainer = document.getElementById("invoiceContainer");
    if (invoiceContainer) {
        invoiceContainer.innerHTML = invoiceHTML;
    } else {
        alert("Error: No container found to display the invoice.");
    }
}
function convertToNumber(input) {
    // Remove all commas and convert the string to a number
    return Number(input.replace(/,/g, ''));
}

// Example Usage
//const url = "http://localhost:8069/POS/TestInvoicePopup.html?customer=Mandy%20Colman&ticketID=66&discount=5.00&product0=Trousers&price0=4.95&quantity0=3&total0=14.85&product1=Evening%20Dress&price1=15.00&quantity1=1&total1=15.00&product2=Shirt&price2=2.60&quantity2=1&total2=2.60&DateReceived=02/12/2025%2000:00:01&DateReceived=27/12/2025%2000:00:01";



//document.getElementById("TicketNo").innerHTML = 'OOOO';