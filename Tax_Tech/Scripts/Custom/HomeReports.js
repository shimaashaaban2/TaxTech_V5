window.addEventListener("load", function (e) {
    try {
        //getFailedImportedDocsCount();
        //getDocsTotals();
    } catch (e) {

    }

    try {
        getRunningAndClosedCounts();
    } catch (e) {

    }

    getRunningJobsRecord();
});

//function getRejectedDocsReport() {
//    sendGetRequest(`Reports/GetRejectedDocsJson`, function (e) {
//        const ctx = document.getElementById('rejetedDocsChart').getContext('2d');
//        if (e.msg == 'fail') {
//            ctx.font = "15px Arial";
//            ctx.fillStyle = "red";
//            ctx.fillText('Failed to Load Chart Data', 20, 20);
//            return;
//        }
//        var myChart = new Chart(ctx, {
//            type: 'horizontalBar',
//            options: {
//                elements: {
//                    rectangle: {
//                        borderWidth: 2,
//                        borderSkipped: 'right'
//                    }
//                },
//                tooltips: {
//                    shadowOffsetX: 1,
//                    shadowOffsetY: 1,
//                    shadowBlur: 8,
//                    //shadowColor: tooltipShadow,
//                    backgroundColor: window.colors.solid.white,
//                    titleFontColor: window.colors.solid.black,
//                    bodyFontColor: window.colors.solid.black
//                },
//                responsive: true,
//                maintainAspectRatio: true,
//                responsiveAnimationDuration: 500,
//                legend: {
//                    display: false
//                },
//                scales: {
//                    xAxes: [
//                        {
//                            display: true,
//                            gridLines: {
//                                zeroLineColor: 'rgba(200, 200, 200, 0.2)',
//                                borderColor: 'transparent',
//                                color: 'rgba(200, 200, 200, 0.2)'
//                            },
//                            scaleLabel: {
//                                display: true
//                            },
//                            ticks: {
//                                min: 0,
//                                fontColor: ''
//                            }
//                        }
//                    ],
//                    yAxes: [
//                        {
//                            display: true,
//                            barThickness: 15,
//                            gridLines: {
//                                display: false
//                            },
//                            scaleLabel: {
//                                display: true
//                            },
//                            ticks: {
//                                fontColor: '#6e6b7b'
//                            }
//                        }
//                    ]
//                }
//            },
//            data: {
//                labels: [...e.data].map(i => i.MOFRejectStatus),
//                datasets: [
//                    {
//                        data: [...e.data].map(i => i.DocCount),
//                        backgroundColor: '#f00',
//                        borderColor: 'transparent'
//                    }
//                ]
//            }
//        });
//    });
//}

function getRejectedDocsCount() {
    // id: MOFRejectedCount
    sendGetRequest(`Reports/GetRejectedDocsJson`, function (e) {
        if (e.msg != 'fail') {
            document.querySelector('#MOFRejectedCount').innerHTML = [...e.data].length;
        }
    });
}

function getFailedImportedDocsCount() {
    sendGetRequest(`Reports/FailedImportedDocsListJson`, function (e) {
        if (e.msg != 'fail') {
            document.querySelector('#ImportFailedCount').innerHTML = [...e.data].length;
        }
    });
}

function getInvoiceTotals() {
    showLoader('invoiceTotalBtn', 'invoiceTotals-btn-loader');
    sendGetRequest(`Home/GetInvoiceTotals`, function (res) {
        hideLoader('invoiceTotalBtn', 'invoiceTotals-btn-loader');

        document.querySelector('#invoiceTotalsContainer').innerHTML = res;
    });
}

function getCreditNotesTotals() {
    showLoader('creditNoteBtn', 'creditNotesTotals-btn-loader');
    sendGetRequest(`Home/GetCreditTotals`, function (res) {
        hideLoader('creditNoteBtn', 'creditNotesTotals-btn-loader');

        document.querySelector('#creditNotesTotalsContainer').innerHTML = res;
    });
}

function getDebitNotesTotals() {
    showLoader('debitNotesTotals-btn', 'debitNotesTotals-btn-loader');
    sendGetRequest(`Home/GetDebitTotals`, function (res) {
        hideLoader('debitNotesTotals-btn', 'debitNotesTotals-btn-loader');

        document.querySelector('#debitNotesTotalsContainer').innerHTML = res;
    });
}

function getMOFTotals() {
    showLoader('mofLoad', 'mofTotals-btn-loader');
    sendGetRequest(`Home/GetRejectedTotals`, function (res) {
        hideLoader('mofLoad', 'mofTotals-btn-loader');

        document.querySelector('#mofStatusTotalsContainer').innerHTML = res;
    });
}

function getRunningAndClosedCounts() {
    sendPostRequest(`Home/GetStatusStatistics`, null, function (res) {
        if (res.msg === 'fail') {
            document.querySelector('#main-area-err').innerHTML = res.view;
        }
        else {
            document.querySelector('#runningJobsContainer').innerHTML = res.CountOfRunning;
            document.querySelector('#closedJobsContainer').innerHTML = res.CountOfClosed;
        }
    });
}

function getRunningJobsRecord() {
    sendGetRequest(`Home/GetImportStatistics`, function (res) {
        document.querySelector('#runningJobReportContainer').innerHTML = res;
    });
}

function getJopTotalPopup(id) {
    alert(id)
    getPopup(`Configuration/ConfigPopup/GetJopTotalPopup?itemId=${id}`);
}

function getCloseJob(id) {
    sendPostRequest(`Home/GetCloseJop?itemId=${id}`, null, function (res) {
        if (res.msg === 'fail') {
            document.querySelector('#main-area-err').innerHTML = res.view;
        }
        else {
            success(res)
        }
    });
}