<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSUsersList - Copy.aspx.vb" Inherits="POSUsersList" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>Users | SimplePOS</title>
<!-- #include virtual = "/POS/javascriptsinclude.html" -->
    <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />
    <link href="../Styles/pos.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.11.3/css/jquery.dataTables.min.css" />
    <script src="https://cdn.datatables.net/1.11.3/js/jquery.dataTables.min.js"></script>
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
        $(document).ready(function () {
            alert('AJAX call triggered!');

            $.ajax({
                url: '/Handlers/UsersHandlerNew.ashx',
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
                            '<td>' + item.userID + '</td>' +
                            '<td>' + item.ForeName + '</td>' +
                            '<td>' + item.SurName + '</td>' +
                            '<td>' + item.address + '</td>' +
                            '<td>' + item.PostCode + '</td>' +
                            '<td>' + item.City + '</td>' +
                            '<td>' + item.PhoneNumber + '</td>' +
                            '<td>' + item.Email + '</td>' +
                            '<td>' + item.Login + '</td>' +
                            '<td>' + item.Password + '</td>' +
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
<h1>Users</h1>
<ol class="breadcrumb">
<li><a href="/localhost/"><i class="fa fa-dashboard"></i> Home</a></li>
<li class="active">Users</li> </ol>
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
<table id="catData" class="table table-striped table-bordered table-condensed table-hover" style="margin-bottom:5px;">
    <thead>
        <tr class="active">
            <th style="max-width:50px;">userID</th>
            <th style="max-width:50px;">ForeName</th>
            <th style="max-width:30px;">SurName</th>
            <th style="max-width:150px;">address</th>
            <th style="max-width:30px;">PostCode</th>
            <th style="max-width:50px;">City</th>
            <th style="max-width:30px;">PhoneNumber</th>
            <th style="max-width:630px;">Email</th>
            <th style="width:30px;">Login</th>
            <th style="width:30px;">Password</th>
        </tr>
    </thead>
    <tbody>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="9" class="p0">
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
Copyright &copy; 2017 SimplePOS. All rights reserved.
</footer>
</div>
<div class="modal" data-easein="flipYIn" id="posModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true"></div>
<div class="modal" data-easein="flipYIn" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"></div>
<div id="ajaxCall"><i class="fa fa-spinner fa-pulse"></i></div>
        </form>




</body>
</html>
