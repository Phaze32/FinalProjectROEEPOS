<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestAjaxASHX.aspx.cs" Inherits="POS_TestAjaxASHX" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.11.3/css/jquery.dataTables.min.css" />
    <script src="https://cdn.datatables.net/1.11.3/js/jquery.dataTables.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="catData" class="table table-striped table-bordered table-condensed table-hover" style="margin-bottom:5px;">
                <thead>
                    <tr class="active">
                        <th style="max-width:50px;">CustomerID</th>
                        <th style="max-width:50px;">Name</th>
                        <th style="max-width:30px;">SurName</th>
                        <th style="max-width:150px;">address</th>
                        <th style="max-width:30px;">PostCode</th>
                        <th style="max-width:50px;">City</th>
                        <th style="max-width:30px;">PhoneNumber</th>
                        <th style="max-width:630px;">Email</th>
                        <th style="width:30px;">Status</th>
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
    </form>

    <script charset="utf-8">
        $(document).ready(function () {
            alert('AJAX call triggered!');

            $.ajax({
                url: '/Handlers/CustomerHandler.ashx',
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
                            '<td>' + item.CustomerID + '</td>' +
                            '<td>' + item.Name + '</td>' +
                            '<td>' + item.SurName + '</td>' +
                            '<td>' + item.address + '</td>' +
                            '<td>' + item.PostCode + '</td>' +
                            '<td>' + item.City + '</td>' +
                            '<td>' + item.PhoneNumber + '</td>' +
                            '<td>' + item.Email + '</td>' +
                            '<td>' + item.Status + '</td>' +
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
</body>
</html>
