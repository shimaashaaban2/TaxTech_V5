
//Submission Log
function OnGetAllReceiptsByFilterShowLoader() {
    showLoader('btn-EReceipt-number', 'btn-EReceiptNumber-loader');
    showLoader('Btn-EReceipt-UUID', 'Btn-EReceiptUUID-loader');
    showLoader('btn-ReceiptGetLogs', 'btn-ReceiptGetLogs-loader');
}
function OnGetCancelWithPagination(PageNo, PageSize) {
//alert();
    $("#PageNo").val(PageNo);
    $("#PageSize").val(PageSize);
    $("#btn-ReceiptGetLogs").submit();
}
function OnGetRefundWithPagination(PageNo, PageSize) {
  //  alert();
    $("#PageNo").val(PageNo);
    $("#PageSize").val(PageSize);
    $("#btn-ReceiptGetLogs").submit();
}


//Submission log
function OnGetAllReceiptsByFilterHideLoader(event) {
    hideLoader('btn-EReceipt-number', 'btn-EReceiptNumber-loader');
    hideLoader('Btn-EReceipt-UUID', 'Btn-EReceiptUUID-loader');
    hideLoader('btn-ReceiptGetLogs', 'btn-ReceiptGetLogs-loader');
  
    
    if (event.responseJSON.msg === 'success') {
        document.querySelector('#errMsg').innerHTML = "";
        document.querySelector('#EReceipt-Logs').innerHTML = event.responseJSON.view;
        $("#thebasic-datatable").DataTable({ "pageLength": 50 });
    }
    else {
        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
    }
   
}

//Billing 
function OnGetBillingLog(event) {
    hideLoader('btn-billingGetDate', 'btn-billingGetDate-loader');
    if (event.responseJSON.msg === 'success') {
        document.querySelector('#errMsg').innerHTML = " ";
        document.querySelector('#BillingLogs').innerHTML = event.responseJSON.view;

        $("#thebasic-datatable").DataTable({ "pageLength": 50 });
    }
    else {
        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
    }

}


function OnGetBillingValiationLog(event) {
    hideLoader('btn-BillingValidation', 'btn-BillingValidation-loader');
    if (event.responseJSON.msg === 'success') {
        document.querySelector('#errMsg').innerHTML = " ";
        document.querySelector('#billingValiationList').innerHTML = event.responseJSON.view;

        $("#thebasic-datatable").DataTable({ "pageLength": 50 });
    }
    else {
        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
    }

}

function OnGetBillingValidationDateRange(event) {
    hideLoader('btn-BillingValidationDate','btn-BillingValidationDate-loader');
    if (event.responseJSON.msg === 'success') {
        document.querySelector('#errMsg').innerHTML = " ";
        document.querySelector('#billingValiationList').innerHTML = event.responseJSON.view;

        $("#thebasic-datatable").DataTable({ "pageLength": 50 });
    }
    else {
        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
    }

}

//wincash
function OnGetWinCashLog(event) {
    hideLoader('Btn-GetWinCash', 'Btn-GetWinCash-loader');
    if (event.responseJSON.msg === 'success') {
        document.querySelector('#errMsg').innerHTML = " ";
        document.querySelector('#WincashLog').innerHTML = event.responseJSON.view;

        $("#thebasic-datatable").DataTable({ "pageLength": 50 });
    }
    else {
        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
    }

}

//Ereceipt List
function GetReceiptByNumber(event) {
    hideLoader('btn-Receipt-number', 'btn-ReceiptNumber-loader');
    if (event.responseJSON.msg === 'success') {
        document.querySelector('#errMsg').innerHTML = " ";
        document.querySelector('#Receipt-List').innerHTML = event.responseJSON.view;

        $("#thebasic-datatable").DataTable({ "pageLength": 50 });
    }
    else {
        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
    }
}

function OnGetReceiptListByFilterShowLoader() {
    showLoader('btn-Receipt-number', 'btn-ReceiptNumber-loader');
    showLoader('Btn-Receipt-UUID', 'Btn-ReceiptUUID-loader');
    showLoader('btn-ReceiptGetList', 'btn-ReceiptGetList-loader');
    showLoader('btnExportToExecl', 'btnExportToExecl-loader');

}
function OnGetReceiptListByFilterHideLoader(event) {
    hideLoader('btn-Receipt-number', 'btn-ReceiptNumber-loader');
    hideLoader('Btn-Receipt-UUID', 'Btn-ReceiptUUID-loader');
    hideLoader('btn-ReceiptGetList', 'btn-ReceiptGetList-loader');
    hideLoader('btnExportToExecl', 'btnExportToExecl-loader');
  
    if (event.responseJSON.msg === 'success') {
        document.querySelector('#errMsg').innerHTML = " ";
        document.querySelector('#Receipt-List').innerHTML = event.responseJSON.view;
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });

       
    }
    else {
        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
    }
}


// clear search Input
function ClearLogInputs()
{
   
    $("#EReceiptUUID").on("click", function () {
            $("#EReceiptNumber").val("");
        })
    $("#EReceiptNumber").on("click", function () {
        $("#EReceiptUUID").val("");
        })
    
    
    $("#btn-ReceiptGetLogs").on("click", function () {
        $('#EReceiptUUID').val("");
    })
    $("#btn-ReceiptGetLogs").on("click", function () {
         $('#EReceiptNumber').val("");
    })


}

function ClearEReceiptSearch() {
    $("#ReceiptUUID").on("click", function () {
        $("#ReceiptNumber").val("");
    })
    $("#ReceiptNumber").on("click", function () {
        $("#ReceiptUUID").val("");
    })

    $("#btn-ReceiptGetList").on("click", function () {
        $('#ReceiptNumber').val("");
    })
    $("#btn-ReceiptGetList").on("click", function () {
        $('#ReceiptUUID').val("");
    })

}

function ClearBillingValidation() {
    $("#From").on("click",function () {
        $('#EReceiptNumber').val("");
    })
    $("#btn-BillingValidationDate").on("click", function () {
        $('#EReceiptNumber').val("");
    })
}

//EReceipt Tracking
function EReceipttracking(event) {
    // alert($('#eReceiptnumber').val())
   
    var eReceiptnumber = $('#eReceiptnumber').val();
   
    hideLoader('btn-eReceiptnumber', 'btn-eReceiptnumber-loader');

    if (event.responseJSON.msg === 'success') {
        document.querySelector('#errMsg').innerHTML = " ";
        document.querySelector('#EReceiptTracking').innerHTML = event.responseJSON.view;
      
    }
   
        else {

        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
            document.querySelector('#EReceiptTracking').innerHTML = " ";


    }

  
}

/**
 * function getDocTotals(docId) {
    sendGetRequest(`/InvoiceLine/GetDocTotal?docId=${docId}`, function (e) {
        if (e.view) {
            $("#invoiceTotals").html(e.view);
        }
        else {
            $("#invoiceTotals").html(e);
        }
    });
}
 */

function ValidatedQuantity(id) {
    // Get the original quantity as a number
    let originalQ = parseFloat($('#lbl-quentity-' + id).text());
    // Get the new quantity as a number
    let newQ = parseFloat($('#txt-quentity-' + id).val());


    // Check if new quantity is a valid number
    if (isNaN(newQ)) {
        alert("Please enter a valid number.");
        $('#txt-quentity-' + id).val('0');
        return;
    }

    // Check if new quantity is a decimal
    if (newQ % 1 !== 0) {
        alert("The Quantity Must be an Integer.");
        $('#txt-quentity-' + id).val('0');
        return;
    }

    // Check if new quantity is negative
    if (newQ < 0) {
        alert("The Number Must be Positive");
        $('#txt-quentity-' + id).val('0');
        return;
    }

    // Check if new quantity is greater than the original quantity
    if (newQ > originalQ) {
        alert("New Quantity Must be Less than or Equal to the Original Quantity");
        $('#txt-quentity-' + id).val('0');
        return;
    }


 }
function onParialRefundComplete(event) {
    HideLoading(1);
    if (event.responseJSON) {
        if (event.responseJSON.msg === 'fail') {
            document.querySelector('#PopupMsg').innerHTML = event.responseJSON.view;
        }
        else {
            
            $("#btn-ReceiptGetLogs").submit();
            $("#default").modal('hide');
           
        }
    }
    else {
        document.querySelector('#PopupMsg').innerHTML = event.responseText;
    }
}




function RefundedReceipt(LogID) {
    $.blockUI({
        message: '<div class="spinner-border text-white" role="status"></div>',
        timeout: 1000,
        css: {
            backgroundColor: 'transparent',
            border: '0'
        },
        overlayCSS: {
            opacity: 0.5
        },
        onBlock: function () {
            sendPostRequest(`/EReceipt/RefundedReceipt?LogID=${LogID}`, null, function (event) {
                $.unblockUI();
        if (event.msg == 'success') {
            $("#btn-ReceiptGetLogs").submit();
            document.querySelector('#successMsg').innerHTML = event.view;
          //  scrollBy(0, -10000);
        }
        else {
            document.querySelector('#errMsg').innerHTML = event.view;
        }
    });
        }
    });
}

function ReSubmitReceipt(LogID) {
    $.blockUI({
        message: '<div class="spinner-border text-white" role="status"></div>',
        timeout: 1000,
        css: {
            backgroundColor: 'transparent',
            border: '0'
        },
        overlayCSS: {
            opacity: 0.5
        },
        onBlock: function () {
            sendPostRequest(`/EReceipt/ReSubmitReceipt?LogID=${LogID}`, null, function (event) {
                $.unblockUI();
                $("#btn-eReceiptnumber").submit();
                document.querySelector('#Msg').innerHTML = event.view;
            });
        }
    });
}

function OnGetAllCancelReceiptsHideLoader(event) {
    hideLoader('btn-Cancelnumber', 'btn-CancelNumber-loader');
    hideLoader('Btn-CancelUUID', 'Btn-CancelUUID-loader');
    hideLoader('btn-CancelGetLogs', 'btn-CancelGetLogs-loader');


    if (event.responseJSON.msg === 'success') {
        document.querySelector('#errMsg').innerHTML = " ";
        document.querySelector('#EReceipt-Logs').innerHTML = event.responseJSON.view;
        $("#thebasic-datatable").DataTable({ "pageLength": 50 });
    }
    else {
        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
    }

}
function selectAll(event) {
    let inputs = [...document.querySelectorAll('.custom-control-input')];
    let truthyInputs = inputs.filter(i => i.checked == true);
    const btnCancelAll = document.querySelector('#btnCancelAll');

    inputs.forEach((ele, i) => {
        if (i > 0) {
            ele.checked = event.target.checked;
        }
    });

    if (event.target.checked) {
        btnCancelAll.disabled = false; 
    }
    else {
        btnCancelAll.disabled = true; 
    }
}

function onItemSelect(event) {
    let inputs = [...document.querySelectorAll('.custom-control-input')];
    let truthyInputs = inputs.filter(i => i.checked == true);

    if (truthyInputs.length > 0) {
        // enable buttons
        btnCancelAll.disabled = false; 
    }
    else {
        btnCancelAll.disabled = true; 
    }
}

function changeRefundBulk() {
    $.blockUI({
        message: '<div class="spinner-border text-white" role="status"></div>',
        timeout: 1000,
        css: {
            backgroundColor: 'transparent',
            border: '0'
        },
        overlayCSS: {
            opacity: 0.5
        },
        onBlock: function () {
    Iscontinue = '0';
   
    //showLoader('btnCancelAll', 'btnCancelAll-loader');
    let inputs = [...document.querySelectorAll('.custom-control-input')];
    let truthyInputs = inputs.filter(i => i.checked == true);
    const allCheckBox = document.querySelector('#all-checkBox');
   
    var lengthIs = truthyInputs.length;
    let msg = '';
    let i = allCheckBox.checked ? 1 : 0;
    
        for (; i < truthyInputs.length; i++) {
            sendPostRequest(`/EReceipt/RefundedReceipt?LogID=${truthyInputs[i].dataset.id}`, null, function (event) {
                $.unblockUI();
                if (event.msg == 'success') {
                    document.querySelector('#successMsg').innerHTML = event.view;
                    $("#btn-ReceiptGetLogs").submit();
                }
                else {
                    document.querySelector('#errMsg').innerHTML = event.view;
                }
            });
          
            }

            //if (i >= lengthIs) {
            //    Iscontinue = '1';
            //}
            //if (Iscontinue === '1') { 
            //$("#btn-ReceiptGetLogs").submit();
            //}

        }
         
    });
  
}


function exportAllReceiptsToExecl() {

    const fromDate = document.querySelector('#From');
  

    const ReceiptNumber = document.querySelector('#ReceiptNumber');
    const ReceiptUUID = document.querySelector('#ReceiptUUID');
    const EReceiptSourceId = document.querySelector('#EReceiptSourceId');
    const EReceiptStatusID = document.querySelector('#EReceiptStatusId');
    const EReceiptStatus = document.querySelector('#EReceiptStatus');
    const EReceiptSource = document.querySelector('#EReceiptSource');
   

    document.querySelector('#btnExportToExecl').innerHTML = "loading";
    document.querySelector('#btnExportToExecl').disabled = true;
   
   
    var iframe = document.createElement('iframe');
    iframe.style.display = 'none';
    iframe.id = 'iframe-execl';
    if (ReceiptNumber.value) {
        iframe.src = url + `EReceipt/ExportReceiptsNumberToExecl?ReceiptNumber=${ReceiptNumber.value}`;
        document.body.appendChild(iframe);
    }
    else if (ReceiptUUID.value) {
        iframe.src = url + `EReceipt/ExportReceiptsUUIDToExecl?ReceiptUUID=${ReceiptUUID.value}`;
        document.body.appendChild(iframe);
    }
    else {
        
        iframe.src = url + `EReceipt/ExportAllReceiptsToExecl?From=${fromDate.value}&EReceiptSource=${EReceiptSource.value}&EReceiptStatus=${EReceiptStatus.value}&EReceiptSourceId=${EReceiptSourceId.checked}&EReceiptStatusId=${EReceiptStatusID.checked}`;
       
        document.body.appendChild(iframe);
    }
    
    document.querySelector('#btnExportToExecl').innerHTML = "Export To Execl";
    document.querySelector('#btnExportToExecl').disabled = false;

}

function exportFullReceiptsToExecl() {

    const fromDate = document.querySelector('#From');
    const ToDate = document.querySelector('#To');


    const EReceiptNumber = document.querySelector('#EReceiptNumber');
    const EReceiptUUID = document.querySelector('#EReceiptUUID');
    const EReceiptSourceId = document.querySelector('#EReceiptSourceId');
    const EReceiptStatusID = document.querySelector('#EReceiptStatusId');
    const EReceiptStatus = document.querySelector('#EReceiptStatus');
    const EReceiptSource = document.querySelector('#EReceiptSource');
    const SubmitStatusId = document.querySelector('#SubmitStatusId');
    const SubmitStatus = document.querySelector('#SubmitStatus');
    //alert(SubmitStatusId.value);
    // alert(SubmitStatus.value);

    document.querySelector('#btnFullExportToExecl').innerHTML = "loading";
    document.querySelector('#btnFullExportToExecl').disabled = true;


    var iframe = document.createElement('iframe');
    iframe.style.display = 'none';
    iframe.id = 'iframe-execl';
    if (EReceiptNumber.value) {
        iframe.src = url + `EReceipt/ExportLogReceiptsNumberToExecl?EReceiptNumber=${EReceiptNumber.value}`;
        document.body.appendChild(iframe);
    }
    else if (EReceiptUUID.value) {
        iframe.src = url + `EReceipt/ExportLogReceiptsUUIDToExecl?EReceiptUUID=${EReceiptUUID.value}`;
        document.body.appendChild(iframe);
    }
    else {

        iframe.src = url + `EReceipt/ExportLogReceiptsToExecl?From=${fromDate.value}&To=${ToDate.value}&EReceiptSource=${EReceiptSource.value}&EReceiptStatus=${EReceiptStatus.value}&EReceiptSourceId=${EReceiptSourceId.checked}&EReceiptStatusId=${EReceiptStatusID.checked}&SubmitStatusId=${SubmitStatusId.checked}&SubmitStatus=${SubmitStatus.value}`;

        document.body.appendChild(iframe);
    }

    document.querySelector('#btnFullExportToExecl').innerHTML = "Export To Execl";
    document.querySelector('#btnFullExportToExecl').disabled = false;

}
//TotalReports
function OnGetTotalReportsListByFilterShowLoader() {
    showLoader('btn-ReportGetList', 'btn-ReportGetList-loader');
}
function OnGetTotalReportsListByFilterHideLoader(event) {
    hideLoader('btn-ReportGetList', 'btn-ReportGetList-loader');
    const fromDate = document.querySelector('#From');
    const toDate = document.querySelector('#To');
    const lang = extractLangCookie();
    if (fromDate.value > toDate.value) {
        if (lang === 'ar') {
            document.querySelector('#errMsg').innerHTML = 'تاريخ البدء يجب ان يكون قبل تاريخ الانتهاء';
            alert('تاريخ البدء يجب ان يكون قبل تاريخ الانتهاء');
        }
        else {
            document.querySelector('#errMsg').innerHTML = 'Start Date must be before End Date';
            alert('Start Date must be before End Date');
        }
    }
    else {
        if (event.responseJSON.msg === 'success') {
            document.querySelector('#errMsg').innerHTML = " ";
            document.querySelector('#Report-List').innerHTML = event.responseJSON.view;
            $('#thebasic-datatable').DataTable({ "pageLength": 50 });
        }
        else {
            document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
        }
    }
}
function exportTotalReportsToExecl() {
    $.blockUI({
        message: '<div class="spinner-border text-white" role="status"></div>',
        timeout: 1000,
        css: {
            backgroundColor: 'transparent',
            border: '0'
        },
        overlayCSS: {
            opacity: 0.5
        },
        onBlock: function () {
    const fromDate = document.querySelector('#From');
    const toDate = document.querySelector('#To');
    const iframe = document.createElement('iframe');
    iframe.style.display = 'none';
    iframe.id = 'iframe-execl';
    iframe.src = url + `EReceipt/ExportTotalReports?fromDate=${fromDate.value}&toDate=${toDate.value}`;
            document.body.appendChild(iframe);
            $.unblockUI();
            
        }
    });
}

function ExportNotExsitItemsToExcel() {
    $.blockUI({
        message: '<div class="spinner-border text-white" role="status"></div>',
        timeout: 1000,
        css: {
            backgroundColor: 'transparent',
            border: '0'
        },
        overlayCSS: {
            opacity: 0.5
        },
        onBlock: function () {
            var iframe = document.createElement('iframe');
            iframe.style.display = 'none';
            iframe.id = 'iframe-execl';
            iframe.src = url + `EReceipt/ExportNotExsitItemsToExcel`;
            $.unblockUI();
            document.body.appendChild(iframe);
        }
    });
}