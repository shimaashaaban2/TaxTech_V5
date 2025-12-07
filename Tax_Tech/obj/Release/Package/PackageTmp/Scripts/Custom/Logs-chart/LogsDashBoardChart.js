window.addEventListener('load', function () {
    GetDashboard();
  
});
 
function GetDashboard() {
    GetTotalReceived();
    submitted();
    getStatus();
     GetCompareStatus2();
}

function GetCompareStatus2() {
   
     
    var from = document.getElementById("From").value;
    var to = document.getElementById("To").value;
    
    let chartDataSource = [];
    
    $.getJSON(url + `LogsDashBoard/CompareStatus2?From=${from}&To=${to}`, {}, function (data) {

        for (var i = 0; i <= data.length - 1; i++) { 
            chartDataSource.push({ Source: "WinCash", Status: data[i].Status, Values: data[i].Wincash});
            chartDataSource.push({ Source: "Billing", Status: data[i].Status, Values: data[i].Billing});
        }
    })
        .always(function (data) {
            CompareStatusChart(chartDataSource);
        });

}

 

function submitted() {
    showLoader('btn-logDashboard', 'btnlogDashboard-loader');
    var from = document.getElementById("From").value;
    var to = document.getElementById("To").value;
    let chartDataSource = [];

    $.getJSON(url + `LogsDashBoard/GetDashBoard2?From=${from}&To=${to}`, {}, function (data) {
        hideLoader('btn-logDashboard', 'btnlogDashboard-loader');
        for (var i = 0; i <= data.length - 1; i++) {
            chartDataSource.push({ Source: data[i].Source, Value: data[i].CountOfEReceipt });
        }
    })
        .always(function (data) {
            submittedChart(chartDataSource);
        });

    

}
function getStatus() {
    showLoader('btn-logDashboard', 'btnlogDashboard-loader');
    var from = document.getElementById("From").value;
    var to = document.getElementById("To").value;
    let chartDataSource = [];

    $.getJSON(url + `LogsDashBoard/GetStatusChart2?From=${from}&To=${to}`, {}, function (data) {
        hideLoader('btn-logDashboard', 'btnlogDashboard-loader');
        for (var i = 0; i <= data.length - 1; i++) {
            chartDataSource.push({ Status: data[i].EReceiptStatus, Value: data[i].CountOfEReceipt });
        }
    })
        .always(function (data) {
            StatusChart(chartDataSource);
        });



}
function getStatusChart() {
    var from = document.getElementById("From").value;
    var to = document.getElementById("To").value;
    showLoader('btn-logDashboard', 'btnlogDashboard-loader');
    $.ajax({
        type: "POST",
        url: url + `/LogsDashBoard/GetStatusChart?From=${from}&To=${to}`,
        success: function (data) {
            hideLoader('btn-logDashboard', 'btnlogDashboard-loader');
            var _data = data;
            var ChartLabels = _data[0];
            var chartData = _data[1];
            var BarColors = ["#28dac6", "#2c9aff", "red", "#ffe800"];

            new Chart("StatusChart", {
                type: "horizontalBar",
                data: {
                    labels: ChartLabels,
                    datasets: [{
                        backgroundColor: BarColors,
                        data: chartData
                    }]
                },
                options: {
                    elements: {
                        rectangle: {
                            borderWidth: 2,
                            borderSkipped: 'bottom'
                        }
                    },
                    responsive: true,
                   /* maintainAspectRatio: false,*/
                    /*responsiveAnimationDuration: 500,*/
                    legend: {
                        display: false
                    },

                    legend: { display: false },
                    title: {
                        display: true,

                    },

                    scales: {
                        xAxes: [{
                            display: true,
                           /* barThickness: 50,*/
                            ticks: {
                                min: 0,
                                max: 5,
                                stepSize: 1,
                                //beginAtZero: true    
                            },
                            gridLines: {
                                display: true,
                            },
                            scaleLabel: {
                                display: false
                            },
                        }],
                        yAxes: [{
                            display: true,
                            barThickness: 50,
                            ticks: {
                                //min: 1,
                                //max: 5,
                                stepSize: 1,
                                beginAtZero: true    
                            },

                        }]
                    }
                }
            });
        }
    });
   
}
function GetCompareStatus() {
    var from = document.getElementById("From").value;
    var to = document.getElementById("To").value;
    showLoader('btn-logDashboard', 'btnlogDashboard-loader');
    $.ajax({
        type: "POST",
        url: url + `/LogsDashBoard/CompareStatus?From=${from}&To=${to}`,
        success: function (data) {
            hideLoader('btn-logDashboard', 'btnlogDashboard-loader');
            var _data = data;
            var ChartLabels = _data[0];
            var chartData = _data[1];
            var chartData2 = _data[2];
            if (ChartLabels[1] == "" && ChartLabels[0] == "") {
                ChartLabels[1] = "Success";
                ChartLabels[0] = "Error";
            }

            new Chart(document.getElementById('SystemIntegrationSummary'), {
                type: 'bar',
                data: {
                    labels: ['Wincash','Billing','Manule'],
                    datasets: [{
                        label: ChartLabels[1],
                        data: chartData,
                        backgroundColor: 'rgba(00, 180, 00, 0.9)',
                        borderColor: 'rgb(00, 200, 00)',
                        borderWidth: 1
                    },
                    {
                        label: ChartLabels[0],
                        data: chartData2,
                        backgroundColor: 'rgba(167, 00, 00, 0.9)',
                        borderColor: 'rgb(167, 00, 00)',

                    }
                    ]
                },
                options: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            usePointStyle: true
                        }
                    },

                    scales: {
                        xAxes: [{
                            barThickness: 50,
                        }],

                        yAxes: [{
                            barThickness: 1,
                            ticks: {
                                min: 0,
                                max: 10,
                                stepSize: 1
                            },
                            scaleLabel: {
                                display: true,
                                /*labelString: 'Percent (%)'*/
                            }
                        }],

                    }
                }
            });
        }

    

    });
}
function GetTotalReceived() {
    var from = document.getElementById("From").value;
    var to = document.getElementById("To").value;
    showLoader('btn-logDashboard', 'btnlogDashboard-loader');
     


    sendPostRequest(`/LogsDashBoard/TotalReceived?From=${from}&To=${to}`, null, (e) => {
        hideLoader('btn-logDashboard', 'btnlogDashboard-loader');


        $("#totalReceived").html(e.view);
        if (feather) {
            feather.replace({
                width: 14,
                height: 14
            });
        }
    });

}

function CompareStatusChart(chartDataSource) {

    $("#SystemIntegrationSummary").dxChart({
        dataSource: chartDataSource,
        commonSeriesSettings: {
            argumentField: 'Source',
            type: 'bar',
            valueField: 'Values',
            hoverMode: 'onlyPoint',
            label: {
                visible: true, // Ensures labels are visible
                format: 'fixedPoint', // Optional: format the label value
                precision: 2 // Optional: specify number of decimal places
            }
        },
        seriesTemplate: {
            nameField: 'Status'
        }
    });

}
function submittedChart(chartDataSource) {

    $("#SubmittedChart").dxChart({
        dataSource: chartDataSource,
        series: [{
            name: "Source", // Add this line to name your series
            argumentField: "Source",
            valueField: "Value", 
            type: "bar",
            color: "#40826d",
            label: {
                visible: true, // Ensures labels are visible
                format: 'fixedPoint', // Optional: format the label value
                precision: 2 // Optional: specify number of decimal places
            }
        }],
        argumentAxis: {
            //title: "Submitted"
        },
        valueAxis: {
            title: "Value"
        },
        //title: "Submitted"
    });
}
function StatusChart(chartDataSource) {

    $("#StatusChart").dxChart({
        dataSource: chartDataSource,
        series: [{
            name: "Status", // Add this line to name your series
            argumentField: "Status",
            valueField: "Value",
            type: "bar",
            color: "#0047ab",
            label: {
                visible: true, // Ensures labels are visible
                format: 'fixedPoint', // Optional: format the label value
                precision: 2 // Optional: specify number of decimal places
            }
        }],
        argumentAxis: {
            //title: "Submitted"
        },
        valueAxis: {
            title: "Value"
        },
        //tooltip: {
        //    enabled: true,
        //    customizeTooltip: function (arg) {
        //        return {
        //            text: arg.argument + ": " + arg.valueText
        //        };
        //    }
        //},
        
        //title: "Submitted"
    });
}
function ResetResult() {
   
    sendPostRequest(`EReceipt/ResetResult`, null, (e) => {
        document.querySelector("#msg").innerHTML = e.view;
        if (e.msg == 'success') {
            GetDashboard();
        }
       
    });

}

