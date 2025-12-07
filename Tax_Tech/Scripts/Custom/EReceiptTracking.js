window.addEventListener('load', function () {
    JobsList();
});
function JobsList() {
    sendGetRequest(`/EReceiptTracking/EreceiptJobsList`, (e) => {
        document.querySelector('#EReceiptJobTracking').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 50, "order": [[0, 'dasc']] });
    });

}

function EReceiptJobIDListShowLoader(event) {
    showLoader('btn-RctJobID', 'btn-RctJobID-loader');
}



function EReceiptJobDateListShowLoader(event) {

    showLoader('btn-RctJobDateList', 'btn-RctJobDateList-loader');
}
function EReceiptJobsListByJobID() {

    //Entityid = $("#EntityId").val();
    var jobID = $("#RctJobId").val();
    //var startDate = $("#startDate").val();
    //var endDate = $("#endDate").val();
    //// alert(JobId);
    hideLoader('btn-RctJobID', 'btn-RctJobID-loader');

    sendGetRequest(`/EReceiptTracking/JobTrackingByJobID?jobID=${jobID}`, (e) => {

        document.querySelector('#EReceiptJobTracking').innerHTML = e;

        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });

}
function EReceiptJobsListByJobDate() {
    var startDate = $("#startDate").val();
    var endDate = $("#endDate").val();

    hideLoader('btn-RctJobDateList', 'btn-RctJobDateList-loader');
    sendGetRequest(`/EReceiptTracking/JobTrackingByJobDate?startDate=${startDate}&endDate=${endDate}`, (e) => {
        document.querySelector('#EReceiptJobTracking').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });

}


let currentPage = 1;
let pageSize = 50; // rows per page
let totalRecords = 0;
let totalPages = 0;
function pageNoList(jobId, pageNo, logType) {
    const pageSize = 50;
    sendGetRequest(`/EReceiptJobTracking/EReceiptFailedJobLogList?jobId=${jobId}&logType=${logType}&pageNo=${pageNo}&pageSize=${pageSize}`)

}


function EReceiptexportToExcel() {
    const fromDate = document.querySelector('#startDate');
    const toDate = document.querySelector('#endDate');
    const JobId = document.querySelector('#RctJobId');
    var iframe = document.createElement('iframe');
    iframe.style.display = 'none';
    iframe.id = 'iframe-execl';
    if (JobId.value) {
        iframe.src = url + `EReceiptTracking/ExportJobByIdToExcel?jobID=${JobId.value}`;
        document.body.appendChild(iframe);
    }
    //else if (fromDate.value && toDate.value) {
    //    iframe.src = url + `EReceipt/ExportReceiptsUUIDToExecl?ReceiptUUID=${ReceiptUUID.value}`;
    //    document.body.appendChild(iframe);
    //}
    else {

        iframe.src = url + `EReceiptTracking/ExportEReceiptJobListToExcel`;

        document.body.appendChild(iframe);
    }

    document.querySelector('#btnExportToExecl').innerHTML = "Export To Execl";
    document.querySelector('#btnExportToExecl').disabled = false;
}