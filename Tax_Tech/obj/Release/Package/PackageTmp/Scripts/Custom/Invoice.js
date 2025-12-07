//window.addEventListener('load', function () {
//    getMasterReport()
//});

//needed in view as form
function getInvoicesOf(status, pageNo, pageSize) {
    sendGetRequest(`/Invoice/List?status=${status}&pageNo=${pageNo}&pageSize=${pageSize}`, (e) => {
        document.querySelector('#list').innerHTML = e.view;
        document.querySelector('#exportType').innerHTML = e.exportType;
        $('#thebasic-datatable').DataTable({
            "pageLength": 100
        });
        //To Show Tooltip in List
        $("[rel=tooltip]").tooltip({ html: true });
    });
}

function getPendingInvoices(pageNo, pageSize) {
    sendGetRequest(`/Invoice/PendingList?pageNo=${pageNo}&pageSize=${pageSize}`, (e) => {
        document.querySelector('#list').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
        //To Show Tooltip in List
        $("[rel=tooltip]").tooltip({ html: true });
    });
}

function getRejectedInvoices(status) {
    sendGetRequest(`/Invoice/RejectedList?status=${status}`, (e) => {
        document.querySelector('#list').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
    });
}

function getDocRejectedInvoices(status, DocID) {
    sendGetRequest(`/Invoice/DocumentRejectedList?status=${status}&DocID=${DocID}`, (e) => {
        document.querySelector('#DocList').innerHTML = e;
        $('#thebasic-datatable2').DataTable({ "pageLength": 100 });
    });
}

function changeInvoiceStatus(docId, status, msg) {
    sendPostRequest(`/Invoice/ChangeInvoiceStatus?docId=${docId}&status=${status}&msg=${msg}`, null, function (e) {
        if (e.msg == 'success') {
            document.querySelector('#list').innerHTML = e.view;
            $('#thebasic-datatable').DataTable({ "pageLength": 100 });
        }
        else {
            document.querySelector('#errMsg').innerHTML = e.view;
        }
    });
}


function getAllInvoicesList(pageNo, pageSize) {
    sendGetRequest(`/Invoice/AllInvoicesList?pageNo=${pageNo}&pageSize=${pageSize}`, function (e) {
        document.querySelector('#list').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
        $("[rel=tooltip]").tooltip({ html: true });
    });
}

function getCreditNotes() {
    sendGetRequest(`/CreditNote/List`, function (e) {
        document.querySelector('#list').innerHTML = e.view;
        document.querySelector('#exportType').innerHTML = e.exportType;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
    });
}

function getDebitNotes() {
    sendGetRequest(`/DebitNote/List`, function (e) {
        document.querySelector('#list').innerHTML = e.view;
        document.querySelector('#exportType').innerHTML = e.exportType;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
    });
}

function getInvoices(pageNo, pageSize) {
    sendGetRequest(`/Invoice/GetInvoices?pageNo=${pageNo}&pageSize=${pageSize}`, function (e) {
        document.querySelector('#list').innerHTML = e.view;
        document.querySelector('#exportType').innerHTML = e.exportType;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
    });
}

function onFilterComplete(event) {
    HideLoading(1);
    if (event.responseJSON) {
        if (event.responseJSON.msg === 'fail') {
            document.querySelector('#PopupMsg').innerHTML = event.responseJSON.view;
        }
        else {
            document.querySelector('#list').innerHTML = event.responseJSON.view;
            document.querySelector('#exportType').innerHTML = event.responseJSON.exportType;

            $("#default").modal('hide');
            $('#thebasic-datatable').DataTable({ "pageLength": 50 });
        }
    }
    else {
        document.querySelector('#PopupMsg').innerHTML = event.responseText;
    }
}

function OnGetAllDocsByFilterList(event) {
    hideLoader('btn-internal', 'btn-internal-loader');
    hideLoader('btn-UUID', 'btn-UUID-loader');
    hideLoader('btn-Get', 'btn-Get-loader');
    hideLoader('btnExportToExecl', 'btnExportToExecl-loader');

    if (event.responseJSON.msg === 'success') {
        document.querySelector('#errMsg').innerHTML = " ";
        document.querySelector('#list').innerHTML = event.responseJSON.view;

        $("#thebasic-datatable").DataTable({ "pageLength": 50 });
    }
    else {
        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
    }
}

function OnGetSearchDocsByFilterList(event) {
    hideLoader('btn-Docinternal', 'btn-Docinternal-loader');
    hideLoader('btn-DocUUID', 'btn-DocUUID-loader');
    hideLoader('btn-DocGet', 'btn-DocGet-loader');
    hideLoader('btnDocExportToExecl', 'btnDocExportToExecl-loader');

    if (event.responseJSON.msg === 'success') {
        document.querySelector('#errMsg').innerHTML = " ";
        document.querySelector('#list').innerHTML = event.responseJSON.view;

        $("#thebasic-datatable").DataTable({ "pageLength": 50 });
    }
    else {
        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
    }
}
function onCancellItem(event) {
    hideLoader('btn-Cancel', 'btn-Cancel-loader');
    if (event.responseJSON) {
        document.querySelector('#PopupMsg').innerHTML = event.responseJSON.view;
        if (event.responseJSON.msg === 'success') {
            getMasterReport(event.responseJSON.id);
            $("#default").modal('hide');
        }
    }
}

function OnGetAllDocsByFilterLoader() {
    showLoader('btn-internal', 'btn-internal-loader');
    showLoader('btn-UUID', 'btn-UUID-loader');
    showLoader('btn-Get', 'btn-Get-loader');
    showLoader('btnExportToExecl', 'btnExportToExecl-loader');

}
function OnGetSearchDocsByFilterLoader() {
    showLoader('btn-Docinternal', 'btn-Docinternal-loader');
    showLoader('btn-DocUUID', 'btn-DocUUID-loader');
    showLoader('btn-DocGet', 'btn-DocGet-loader');
    showLoader('btnDocExportToExecl', 'btnDocExportToExecl-loader');

}

function exportAllDocToExecl() {

    const fromDate = document.querySelector('#DateFrom');
    const toDate = document.querySelector('#DateTo');

    const DocumentType = document.querySelector('#DocumentType');
    const DocumentCheckBox = document.querySelector('#DocumentCheckBox');
    const ProccessStatusID = document.querySelector('#ProccessStatusID');
    const ProccessStatusOption = document.querySelector('#ProccessStatusOption');
    const InputType = document.querySelector('#InputType');
    const ItemOption = document.querySelector('#ItemOption');
    const AccountID = document.querySelector('#AccountID');
    const accountOption = document.querySelector('#accountOption');


    const lang = extractLangCookie();

    if (!fromDate.value || !toDate.value) {
        if (lang === 'ar') {
            alert('الرجاء تحديد تاريخ من و الي');
        }
        else {
            alert('Please Specify Date Range');
        }
        return;
    }

    if (fromDate.value > toDate.value) {
        if (lang === 'ar') {
            alert('تاريخ البدء يجب ان يكون قبل تاريخ الانتهاء');
        }
        else {
            alert('Start Date must be before End Date');
        }
        return;
    }

    if (DocumentCheckBox.checked === false && !DocumentType.value) {
        if (lang === 'ar') {
            alert('أختر نوع الفاتوره');
        }
        else {
            alert('Choose document type');
        }
        return;
    }
    if (ProccessStatusOption.checked === false && !ProccessStatusID.value) {
        if (lang === 'ar') {
            alert('أختر حاله الفاتوره');
        }
        else {
            alert('Choose document Proccess');
        }
        return;
    }
    if (ItemOption.checked === false && !InputType.value) {
        if (lang === 'ar') {
            alert('أختر نوع العملية');
        }
        else {
            alert('Choose Input Type');
        }
        return;
    }
    if (accountOption.checked === false && !AccountID.value) {
        if (lang === 'ar') {
            alert('أختر العميل');
        }
        else {
            alert('Choose Vendor');
        }
        return;
    }

    document.querySelector('#btnExportToExecl').innerHTML = "loading";
    document.querySelector('#btnExportToExecl').disabled = true;
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
    const iframe = document.createElement('iframe');
    iframe.style.display = 'none';
    iframe.id = 'iframe-execl';
    iframe.src = url + `Invoice/ExportAllDocsToExecl?fromDate=${fromDate.value}&toDate=${toDate.value}&DocumentType=${DocumentType.value}&ProccessStatusID=${ProccessStatusID.value}
    &InputType=${InputType.value}&AccountID=${AccountID.value}&ItemOption=${ItemOption.checked}&accountOption=${accountOption.checked}&ProccessStatusOption=${ProccessStatusOption.checked}
    &DocumentCheckBox=${DocumentCheckBox.checked}`;

    document.body.appendChild(iframe);
    document.querySelector('#btnExportToExecl').innerHTML = "Export To Execl";
    document.querySelector('#btnExportToExecl').disabled = false;
        }
    });
}
function getMasterReport(id) {
    sendPostRequest(`Invoice/GetMasterReportList?JobID=${id}`, null, function (res) {
        document.querySelector('#list').innerHTML = res;
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });
}
function getMasterReportPaging(pageNo, pageSize) {
    const fromDate = document.querySelector('#DateFrom');
    const toDate = document.querySelector('#DateTo');
    const DocumentType = document.querySelector('#DocumentType');
    const DocumentCheckBox = document.querySelector('#DocumentCheckBox');
    const ProccessStatusID = document.querySelector('#ProccessStatusID');
    const ProccessStatusOption = document.querySelector('#ProccessStatusOption');
    const InputType = document.querySelector('#InputType');
    const ItemOption = document.querySelector('#ItemOption');
    const AccountID = document.querySelector('#AccountID');
    const accountOption = document.querySelector('#accountOption');


    var form = new FormData();
    form.append("DateFrom", fromDate.value);
    form.append("DateTo", toDate.value);
    form.append("DocumentType", DocumentType.value);
    form.append("ProccessStatusID", ProccessStatusID.value);
    form.append("InputType", InputType.value);
    form.append("AccountID", AccountID.value);
    form.append("ItemOption", ItemOption.checked);
    form.append("accountOption", accountOption.checked);
    form.append("DocumentCheckBox", DocumentCheckBox.checked);
    form.append("ProccessStatusOption", ProccessStatusOption.checked);
    form.append("pageNo", pageNo);
    form.append("pageSize", pageSize);
    form.append("ReturnPartial", 1);

    sendPostRequest(`Invoice/AllDocList`, form, function (res) {
        document.querySelector('#list').innerHTML = res;
        $('#thebasic-datatable').DataTable({ "pageLength": pageSize });
    });


}


const input = document.getElementById('autocomplete-input');
input.addEventListener('input', function () {
    const userInput = input.value;

    if (userInput.length >= 3) {
        fetch(url+`/Invoice/GetVendorsList?VendorName=${userInput}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ input: userInput })
        })
            .then(response => response.json())
            .then(data => {
                data = JSON.parse(data);
                var a = document.createElement("div");
                a.setAttribute("class", "autocomplete-items");
                this.parentNode.appendChild(a);
                for (i = 0; i < data.length; i++) {
                    var b = document.createElement("div");
                    //b.innerHTML = "<strong>" + data[i].substr(0, userInput.length) + "</strong>";
                    b.innerHTML += data[i].substr(userInput.length);
                    b.innerHTML += "<input type='hidden' value='" + data[i] + "'>";
                    b.addEventListener("click", function (e) {
                        input.value = this.getElementsByTagName("input")[0].value;
                        closeAllLists();
                    });
                    a.appendChild(b);
                };
            })
            .catch(error => console.error('Error fetching autocomplete suggestions:', error));
    }
    else {
        input.innerHTML = '';
    }
});
document.addEventListener("click", function (e) {
    closeAllLists(e.target);
});
function closeAllLists(elmnt) {
    var x = document.getElementsByClassName("autocomplete-items");
    for (var i = 0; i < x.length; i++) {
        if (elmnt != x[i] && elmnt != input) {
            x[i].parentNode.removeChild(x[i]);
        }
    }
}
//function cancellation() {
//    prompt("Cancell Reason", "");
//}


function ExportToExcel(status) {
    const button = document.querySelector('#InternalRejectToExcel');

    button.innerHTML = "loading";
    button.disabled = true;

    // Send an AJAX request to the server
    fetch(url + `Invoice/ExportToExecl?${status}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json'
        }
    })
        .then(response => {
            if (response.ok) {
                // Handle successful export (file download)
                return response.blob();
            } else {
                // If the response is an error (e.g., 401/403), handle the error
                return response.json().then(errorData => {
                    throw new Error(errorData.error || 'An error occurred while exporting');
                });
            }
        })
        .then(blob => {
            // Get the current date in YYYY-MM-DD format
            const currentDate = new Date().toISOString().split('T')[0];

            // Create a download link and trigger the download with dynamic file name
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            a.download = `ExportItems_${currentDate}.xlsx`; // File name with current date
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);

            // Re-enable the button after download
            button.innerHTML = "Export To Excel";
            button.disabled = false;
        })
        .catch(error => {
            // Handle the error (e.g., show a message to the user)
            alert("Error: " + error.message);

            // Re-enable the button after the error
            button.innerHTML = "Export To Excel";
            button.disabled = false;
        });
}

