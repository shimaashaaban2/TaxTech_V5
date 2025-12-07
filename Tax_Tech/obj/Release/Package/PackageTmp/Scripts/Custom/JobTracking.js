
window.addEventListener('load', function () {
    JobsList();
});
function JobsList() {
    sendGetRequest(`/JobTracking/JobsList`, (e) => {
        document.querySelector('#JobTracking').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 50, "order": [[0, 'dasc']] });
    });
    
}
function JobIDListShowLoader(event) {
    showLoader('btn-JobID', 'btn-JobID-loader');
}
//function JobTrackingListHideLoader(event) {
//    hideLoader('btn-JobID', 'btn-JobID-loader');
//    hideLoader('btn-JobDateList', 'btn-JobDateList-loader');
//    if (event.responseJSON.msg === 'success') {
//        document.querySelector('#errMsg').innerHTML = " ";
//        document.querySelector('#job-list').innerHTML = event.responseJSON.view;
//        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
//    }
//    else {
//        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
//    }
//}

function JobDateListShowLoader(event) {
    
    showLoader('btn-JobList', 'btn-JobDateList-loader');
}
function JobsListByJobID() {
   
    //Entityid = $("#EntityId").val();
    var JobId = $("#JobId").val();
   // alert(JobId);
    hideLoader('btn-JobID', 'btn-JobID-loader');

    sendGetRequest(`/JobTracking/JobTrackingByJobID?JobId=${JobId}`, (e) => {
       
        document.querySelector('#JobTracking').innerHTML = e;
        
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });

}
function JobsListByJobDate() {
    var from = $("#From").val();
    var to = $("#To").val();
    
    hideLoader('btn-JobDateList', 'btn-JobDateList-loader');
    sendGetRequest(`/JobTracking/JobTrackingByJobDate?From=${from}&To=${to}`, (e) => {
        document.querySelector('#JobTracking').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });

}

let currentPage = 1;
let pageSize = 50; // rows per page
let totalRecords = 0;
let totalPages = 0;

//function FailedJobLogList(JobId, pageNo, logType, pageSize) {
//    var pageSize = $("#PageSize").val();
//    totalPages = Math.ceil(totalRecords / pageSize);

//    sendGetRequest(`/JobTracking/FailedJobLogList?jobId=${JobId}&logType=${logType}&pageNo=${pageNo}&pageSize=${pageSize}`, () => {
       
//        $("#pageInfo").text(`Page ${currentPage} of ${totalPages}`);
           
//        });
//}
function pageNoList(jobId, pageNo, logType) {
    const pageSize = 50;
    sendGetRequest(`/JobTracking/FailedJobLogList?jobId=${jobId}&logType=${logType}&pageNo=${pageNo}&pageSize=${pageSize}`)
  
}


function exportToExcel() {
     const fromDate = document.querySelector('#From');
     const toDate = document.querySelector('#To');
    const JobId = document.querySelector('#JobId');
    var iframe = document.createElement('iframe');
    iframe.style.display = 'none';
    iframe.id = 'iframe-execl';
    if (JobId.value) {
        iframe.src = url + `JobTracking/ExportJobByIdToExcel?JobId=${JobId.value}`;
        document.body.appendChild(iframe);
    }
    //else if (fromDate.value && toDate.value) {
    //    iframe.src = url + `EReceipt/ExportReceiptsUUIDToExecl?ReceiptUUID=${ReceiptUUID.value}`;
    //    document.body.appendChild(iframe);
    //}
    else {

    iframe.src = url + `JobTracking/ExportJobListToExcel`;

        document.body.appendChild(iframe);
    }

    document.querySelector('#btnExportToExecl').innerHTML = "Export To Execl";
    document.querySelector('#btnExportToExecl').disabled = false;
}