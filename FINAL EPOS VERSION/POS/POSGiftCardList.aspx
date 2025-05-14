<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSGiftCardList.aspx.vb" Inherits="POSGiftCardList" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>Gift Cards | SimplePOS</title>
<!-- #include virtual = "/POS/javascriptsinclude.html" -->
    <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />
    <link href="../Styles/pos.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.11.3/css/jquery.dataTables.min.css" />
    <script src="https://cdn.datatables.net/1.11.3/js/jquery.dataTables.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
    
</head>
<body class="skin-green fixed sidebar-mini sidebar-collapse">
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
    <script charset="utf-8">

        function openFormGiftCardModal(event) {
            // Retrieve the button that triggered the modal
            const button = event.currentTarget;
            alert("openFormGiftCardModal.triggerd");
            // Get the data-userid value
            const GiftCardID = button.getAttribute('data-GiftCardID');

            // Alert to show the retrieved GiftCardID
            alert("GiftCardID: " + GiftCardID);

            // fethcing data for the selected user ID for the modal
            fetchData(GiftCardID);

            // Set the userId value in the hidden input field
            document.getElementById('editGiftCardID').value = GiftCardID;

            // Set the userId as a data attribute of the modal
            document.getElementById("getdata").setAttribute("onclick", "fetchData('" + GiftCardID + "')");
            alert("document.getElementById(getdata)" + GiftCardID);
            // Optionally, you can set the userId as a data attribute of the modal
            //document.getElementById('FormGiftCardModal').setAttribute('data-GiftCardID', GiftCardID);
            //document.getElementById('FormGiftCardModal').setAttribute('data-GiftCardID', GiftCardID);

            // Show the modal
            $('#gcModal').modal('show');
            alert("past FormGiftCardModal=" + GiftCardID);
        }

        $(document).ready(function () {
            alert('AJAX call triggered!');
            // Attach a click event to the "Edit" button

            function openFormGiftCardModal(event) {
                event.preventDefault();

                // Extract the userID using the data attribute from the clicked button
                // Using vanilla JavaScript:
                const GiftCardID = event.currentTarget.getAttribute('data-GiftCardID');

                // Alternatively, using jQuery:
                // const userID = $(event.currentTarget).data('userid');

                // Alert to confirm the userID and modal trigger
                alert("openFormGiftcardModal triggered for GiftCardID: " + GiftCardID + "!");

                // gets data for the modal and populate it before showiing
                fetchData(GiftCardID);

                // Trigger the modal with the given options
                $('#FormGiftcardModal').modal({ backdrop: "static" });

                // Optionally, if you want to fetch the user data using the extracted userID, you can call:
                // fetchUserData(userID);
            }


            $.ajax({
                url: '/Handlers/GiftcardHandler.ashx',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    alert('AJAX call was successful! Data: ' + JSON.stringify(data));
                    console.log(data);

                    // Manually populate the table with the JSON response
                    var tableBody = $('#catData tbody');
                    tableBody.empty(); // Clear any existing rows

                    $.each(data.data, function (index, item) {
                        var row = '<tr>' +
                            '<td>' + item.GiftCardID + '</td>' +
                            '<td>' + item.CardNumber + '</td>' +
                            '<td>' + (item.CardName || '') + '</td>' +
                            '<td>' + item.Balance.toFixed(2) + '</td>' +
                            '<td>' + (item.DiscountPercentage ? item.DiscountPercentage.toFixed(2) + '%' : '0%') + '</td>' +
                            '<td>' + (item.IsActive ? 'Yes' : 'No') + '</td>' +
                            '<td>' + item.CreatedDate.split('T')[0] + '</td>' +
                            '<td>' + item.ExpiryDate.split('T')[0] + '</td>' +
                            '<td>'+
                            '<button type="button" class="btn btn-sm btn-primary edit-btn" onclick="openFormGiftCardModal(event)" data-GiftCardID="' + item.GiftCardID + '" >' +
                            'Edit</button>' +
                            '</td>' +

                            '</tr>';
                        tableBody.append(row);
                    });

                    // Initialize DataTable
                    $('#catData').DataTable();

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert('AJAX call failed! ' + textStatus);
                    console.error('Error fetching customer data:', textStatus, errorThrown);
                    console.log('Response Text:', jqXHR.responseText);
                }
            });


        });

    </script>
     
<div class="content-wrapper">
<section class="content-header">
<h1>Gift Cards</h1>
<ol class="breadcrumb">
<li><a href="/localhost/"><i class="fa fa-dashboard"></i> Home</a></li>
<li class="active">GiftCard</li> </ol>
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
<h3 class="box-title">Please use the table below to navigate or filter the results.</h3>
</div>
<div class="box-body">
<div class="table-responsive">
  <input type="hidden" name="spos_token" value="74efb7592498f4e9c50ded586fe15ef7" />
<table id="catData" class="table table-striped table-bordered table-condensed table-hover" style="margin-bottom:5px;">
    <thead>
    <tr class="active">
        <th>GiftCardID</th>
        <th>Card Number</th>
        <th>Card Name</th>
        <th>Balance</th>
        <th>Discount (%)</th>
        <th>Is Active</th>
        <th>Created Date</th>
        <th>Expiry Date</th>
        <th>Action</th>
    </tr>
</thead>
    <tbody>
    </tbody>
    <tfoot>
    <tr>
        <td colspan="8" class="p0">
            <input type="text" class="form-control b0" name="search_table" id="search_table" placeholder="Type & hit enter to search the table" style="width:100%;"/>
        </td>
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
Copyright &copy; 2025 SimplePOS. All rights reserved.
</footer>
</div>
<div class="modal" data-easein="flipYIn" id="posModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true"></div>
<div class="modal" data-easein="flipYIn" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"></div>
<div id="ajaxCall"><i class="fa fa-spinner fa-pulse"></i></div>
  <!-- #include virtual = "/POS/FormGiftCardModal.html" -->
  </form>
</body>
</html>



