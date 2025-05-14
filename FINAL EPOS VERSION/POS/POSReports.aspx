<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSReports.aspx.vb" Inherits="POSReports" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Reports | SimplePOS</title>
    <link rel="shortcut icon" href="../Images/POSImages/icons/icon.png" />
    <link href="../Styles/pos.css" rel="stylesheet" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.11.3/css/jquery.dataTables.min.css" />
    <script src="https://cdn.datatables.net/1.11.3/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <style>
        .data-mode-select {
            margin: 15px auto;
            padding: 8px 12px;
            font-size: 15px;
            display: block;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-family: Arial, sans-serif;
        }

        .gridview-table {
            width: 100%;
            border-collapse: collapse;
            font-family: Arial, sans-serif;
            margin-top: 20px;
        }

        .gridview-table th,
        .gridview-table td {
            padding: 10px;
            border: 1px solid #ddd;
        }

        .gridview-table th {
            background-color: #4CAF50;
            color: white;
            text-align: center;
        }

        .gridview-table tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        .gridview-table tr:hover {
            background-color: #eee;
        }

        .right-align {
            text-align: right;
        }

        .center-align {
            text-align: center;
        }

        #dataChart {
            width: 90% !important;
            max-width: 1000px;
            height: 400px !important;
            margin: 0 auto;
            display: block;
            background-color: white;
            border: 1px solid #ddd;
        }

        #reportTitle {
            text-align: center;
            margin-top: 15px;
            font-size: 24px;
            font-weight: bold;
            font-family: Arial, sans-serif;
        }
    </style>
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

            <div class="content-wrapper">
                <section class="content-header">
                    <h1 id="reportTitle">Sales Report - By Year</h1>
                    <ol class="breadcrumb">
                        <li><a href="/localhost/"><i class="fa fa-dashboard"></i> Home</a></li>
                        <li class="active">Reports</li>
                    </ol>
                </section>

                <section class="content">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box-body">

                                <!-- Chart -->
                                <canvas id="dataChart"></canvas>

                                <!-- Dropdown for selection -->
                                <select id="dataModeSelect" class="data-mode-select">
                                    <option value="year" selected>By Year</option>
                                    <option value="quarter">By Quarter</option>
                                    <option value="month">By Month</option>
                                    <option value="day">By Day</option>
                                </select>

                                <!-- Table -->
                                <div class="table-responsive">
                                    <table id="gridviewTable" class="gridview-table display">
                                        <thead>
                                            <tr>
                                                <th>Year</th>
                                                <th>Total Ticket Sales</th>
                                                <th>Total Pieces Sold</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <!-- Filled by JS -->
                                        </tbody>
                                    </table>
                                </div>

                            </div>
                        </div>
                    </div>
                </section>

                <footer class="main-footer">
                    <div class="pull-right hidden-xs">Version <strong>4.0.13</strong></div>
                    &copy; 2025 SimplePOS. All rights reserved.
                </footer>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        $(document).ready(function () {
            let dataChart;

            const modeTitles = {
                year: "By Year",
                quarter: "By Quarter",
                month: "By Month",
                day: "By Day"
            };

            fetchData('year');

            $('#dataModeSelect').change(function () {
                const selectedMode = $(this).val();
                $('#reportTitle').text(`Sales Report - ${modeTitles[selectedMode]}`);
                fetchData(selectedMode);
            });

            function fetchData(mode) {
                $.ajax({
                    url: `http://localhost:8069/handlers/SalesSummaryByPeriodHandler.ashx?mode=${mode}`,
                    type: 'GET',
                    dataType: 'json',
                    success: function (data) {
                        populateTable(data, mode);
                        generateChart(data, mode);
                    },
                    error: function (err) {
                        console.error('Data fetch error:', err);
                    }
                });
            }

            function populateTable(data, mode) {
                const tableHead = $('#gridviewTable thead');
                const tableBody = $('#gridviewTable tbody');

                // Clear old content
                tableHead.empty();
                tableBody.empty();

                // Build table headers dynamically
                let headerRow = '<tr><th class="center-align">Year</th>';

                if (mode === 'quarter') {
                    headerRow += '<th class="center-align">Quarter</th>';
                } else if (mode === 'month') {
                    headerRow += '<th class="center-align">Month</th>';
                } else if (mode === 'day') {
                    headerRow += '<th class="center-align">Month</th>';
                    headerRow += '<th class="center-align">Day</th>';
                }

                headerRow += `
        <th class="right-align">Total Ticket Sales</th>
        <th class="right-align">Total Pieces Sold</th>
    </tr>`;

                tableHead.append(headerRow);

                // Populate table body rows
                data.forEach(row => {
                    let newRow = `<tr class="table-row" data-ticket-id="${row.TicketID}"><td class="center-align">${row.Year}</td>`;

                    if (mode === "quarter") newRow += `<td class="center-align">Q${row.Quarter}</td>`;
                    if (mode === "month") newRow += `<td class="center-align">${row.Month}</td>`;
                    if (mode === "day") {
                        newRow += `<td class="center-align">${row.Month}</td>`;
                        newRow += `<td class="center-align">${row.Day}</td>`;
                    }

                    newRow += `<td class="right-align">${Math.round(row.TotalTicketSales)}</td>`;
                    newRow += `<td class="right-align">${Math.round(row.TotalPiecesSold)}</td>`;
                    newRow += `</tr>`;

                    tableBody.append(newRow);
                });

                // Attach click event listener to rows
                $('.table-row').on('click', function () {
                    const ticketID = $(this).data('ticket-id');
                    alert(`Ticket ID: ${ticketID}`);
                });
            }


            function generateChart(data, mode) {
                const labels = [];
                const ticketSales = [];
                const piecesSold = [];

                data.forEach(row => {
                    let label = row.Year;
                    if (mode === 'quarter') label += ` Q${row.Quarter}`;
                    else if (mode === 'month') label += `-${row.Month}`;
                    else if (mode === 'day') label += `-${row.Month}-${row.Day}`;

                    labels.push(label);
                    ticketSales.push(Math.round(row.TotalTicketSales));
                    piecesSold.push(Math.round(row.TotalPiecesSold));
                });

                if (dataChart) dataChart.destroy();

                setTimeout(() => {
                    const ctx = document.getElementById('dataChart').getContext('2d');
                    dataChart = new Chart(ctx, {
                        type: 'bar',
                        data: {
                            labels: labels,
                            datasets: [
                                {
                                    label: 'Total Ticket Sales',
                                    data: ticketSales,
                                    backgroundColor: 'rgba(75, 192, 192, 0.7)',
                                    borderColor: 'rgba(75, 192, 192, 1)',
                                    borderWidth: 1
                                },
                                {
                                    label: 'Total Pieces Sold',
                                    data: piecesSold,
                                    backgroundColor: 'rgba(153, 102, 255, 0.7)',
                                    borderColor: 'rgba(153, 102, 255, 1)',
                                    borderWidth: 1
                                }
                            ]
                        },
                        options: {
                            responsive: true,
                            maintainAspectRatio: false,
                            plugins: {
                                legend: {
                                    position: 'top',
                                    labels: {
                                        font: {
                                            size: 24,
                                            weight: 'bold',
                                        },
                                        color: '#000',
                                        padding: 20
                                    }
                                },
                                tooltip: {
                                    enabled: true, // Ensure tooltips are enabled
                                    callbacks: {
                                        label: function (tooltipItem) {
                                            // Display value with a custom format
                                            return `${tooltipItem.dataset.label}: ${tooltipItem.raw}`;
                                        }
                                    },
                                    bodyFont: {
                                        size: 20,
                                        weight: 'normal',
                                    },
                                    titleFont: {
                                        size: 20,
                                        weight: 'bold',
                                    },
                                    titleColor: '#000',
                                    bodyColor: '#000'
                                }
                            },
                            layout: {
                                padding: {
                                    top: 30,
                                    bottom: 30,
                                    left: 10,
                                    right: 10
                                }
                            },
                            scales: {
                                x: {
                                    title: {
                                        display: true,
                                        text: 'Period',
                                        font: {
                                            size: 24,
                                            weight: 'bold'
                                        },
                                        color: '#000'
                                    },
                                    ticks: {
                                        font: {
                                            size: 18,
                                            weight: 'bold'
                                        },
                                        color: '#000',
                                        maxRotation: 45,
                                        minRotation: 0,
                                        autoSkip: true
                                    }
                                },
                                y: {
                                    title: {
                                        display: true,
                                        text: 'Amount',
                                        font: {
                                            size: 24,
                                            weight: 'bold'
                                        },
                                        color: '#000'
                                    },
                                    ticks: {
                                        font: {
                                            size: 18,
                                            weight: 'bold'
                                        },
                                        color: '#000',
                                        beginAtZero: true
                                    }
                                }
                            }
                        }
                    });
                }, 100);
            }


        });
    </script>
</body>
</html>
