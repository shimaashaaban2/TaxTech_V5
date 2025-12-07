function loadDocs() {
    showLoader('btn-load', 'btn-load-loader');
    const fromDate = document.querySelector('#fromDate');
    const toDate = document.querySelector('#toDate');

    loadRecentDocs(fromDate.value, toDate.value);
}
function loadRecentDocs(fromDate, toDate) {
    sendGetRequest(`/Invoice/GetRecentDocsList?fromDate=${fromDate}&toDate=${toDate}`, function (e) {
        hideLoader('btn-load', 'btn-load-loader');

        if (e.msg == 'success') {
            document.querySelector('#list').innerHTML = e.view;
            recentDocs = e.data;
            $("#thebasic-datatable").DataTable({ "pageLength": 10 });
        }
        else {
            document.querySelector('#errMsg').innerHTML = e.view;
        }
    });
}

function insertDocs(event) {
    event.target.disabled = true;
    let checkedInputs = [...document.querySelectorAll('.custom-control-input')].filter(i => i.checked);

    if (recentDocs) {
        let selectedDocs = [];
        for (let i = 0; i < recentDocs.Result.length; i++) {
            for (let j = 0; j < checkedInputs.length; j++) {
                if (recentDocs.Result[i].UUID == checkedInputs[j].dataset.uuid) {
                    selectedDocs.push(recentDocs.Result[i]);
                }
            }
        }

        const formData = new FormData();
        formData.append('docsStr', JSON.stringify(selectedDocs));

        sendPostRequest(`/Invoice/InsertReceivedDocument`, formData, function (e) {
            if (e.msg == 'fail') {
                document.querySelector('#errMsg').innerHTML = e.view;
            }
            else {
                document.querySelector('#errMsg').innerHTML = e.view;
            }
            event.target.disabled = false;
        });
    }
}

function rejectDocs(event) {
    ShowLoading(1);
    let checkedInputs = [...document.querySelectorAll('.custom-control-input')].filter(i => i.checked);
    const reason = document.querySelector('#Reason');

    if (recentDocs) {
        let selectedDocs = checkedInputs.map(i => i.dataset.docId);

        const formData = new FormData();
        formData.append('docsStr', JSON.stringify(selectedDocs));

        sendPostRequest(`/Invoice/RejectRecievedDocs?reason=${reason.value}`, formData, function (e) {
            HideLoading(1);
            if (e.msg == 'fail') {
                document.querySelector('#PopupMsg').innerHTML = e.view;
            }
            else {
                $("#default").modal('hide');
                document.querySelector('#errMsg').innerHTML = e.view;
            }
        });
    }
}

// bulk action functionality
function selectAll(event) {
    let inputs = [...document.querySelectorAll('.custom-control-input')];
    let truthyInputs = inputs.filter(i => i.checked == true);
    const btnInsertAll = document.querySelector('#btnRejectAll');

    inputs.forEach((ele, i) => {
        if (i > 0) {
            ele.checked = event.target.checked;
        }
    });

    if (event.target.checked && inputs.length > 1) {
        btnInsertAll.disabled = false;
    }
    else {
        btnInsertAll.disabled = true;
    }
}

function onItemSelect(event) {
    const btnInsertAll = document.querySelector('#btnRejectAll');
    let inputs = [...document.querySelectorAll('.custom-control-input')];
    let truthyInputs = inputs.filter(i => i.checked == true);

    if (truthyInputs.length > 0) {
        // enable buttons
        btnInsertAll.disabled = false;
    }
    else {
        btnInsertAll.disabled = true;
    }
}
function exportToExecl() {

    const fromDate = document.querySelector('#From');
    const toDate = document.querySelector('#To');

    const mDocTypeID = document.querySelector('#mDocTypeID');
    const mOStatus = document.querySelector('#mOStatus');
    const accountName = document.querySelector('#accountName');
    const docTypeOption = document.querySelector('#docTypeOption');
    const mOFStatusOption = document.querySelector('#mOFStatusOption');
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

    if (docTypeOption.checked === false && !mDocTypeID.value) {
        if (lang === 'ar') {
            alert('أختر نوع الفاتوره');
        }
        else {
            alert('Choose document type');
        }
        return;
    }
    if (mOFStatusOption.checked === false && !mOStatus.value) {
        if (lang === 'ar') {
            alert('أختر حاله الفاتوره');
        }
        else {
            alert('Choose document Status');
        }
        return;
    }

    if (accountOption.checked === false && !accountName.value) {
        if (lang === 'ar') {
            alert('أختر العميل');
        }
        else {
            alert('Choose Vendor');
        }
        return;
    }


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
    iframe.src = url + `Invoice/ExportRecievedDocsToExecl?fromDate=${fromDate.value}&toDate=${toDate.value}&mOStatus=${mOStatus.value}&accountName=${accountName.value}
    &mDocTypeID=${mDocTypeID.value}&docTypeOption=${docTypeOption.checked}&mOFStatusOption=${mOFStatusOption.checked}&accountOption=${accountOption.checked}`;

    document.body.appendChild(iframe);

        document.querySelector('#btnExportToExecl').disabled = false;
    }
    });
}

//function exportToExecl() {
//    const fromDate = document.querySelector('#fromDate');
//    const toDate = document.querySelector('#toDate');
//    const lang = extractLangCookie();

//    if (!fromDate.value || !toDate.value) {
//        if (lang === 'ar') {
//            alert('الرجاء تحديد تاريخ من و الي');
//        }
//        else {
//            alert('Please Specify Date Range');
//        }
//        return;
//    }

//    if (fromDate.value > toDate.value) {
//        if (lang === 'ar') {
//            alert('تاريخ البدء يجب ان يكون قبل تاريخ الانتهاء');
//        }
//        else {
//            alert('Start Date must be before End Date');
//        }
//        return;
//    }

//    document.querySelector('#btnExportToExecl').disabled = true;

//    const iframe = document.createElement('iframe');
//    iframe.style.display = 'none';
//    iframe.id = 'iframe-execl';
//    iframe.src = url + `Invoice/ExportRecievedDocsToExecl?fromDate=${fromDate.value}&toDate=${toDate.value}`;
//    document.body.appendChild(iframe);

//    document.querySelector('#btnExportToExecl').disabled = false;
//}

function onRejectActionComplete(event) {
    HideLoading(1);
    if (event.responseJSON) {
        document.querySelector('#PopupMsg').innerHTML = event.responseJSON.view;
    }
    else {
        document.querySelector('#PopupMsg').innerHTML = event.responseText;
    }
}
function OnGetRecentDocsByFilterList(event) {
    hideLoader('btn-load', 'btn-load-loader');

    if (event.responseJSON.msg == 'success') {
        document.getElementById("errMsg").innerHTML = " ";

        document.querySelector('#list').innerHTML = event.responseJSON.view;
       
        $("#thebasic-datatable").DataTable({ "pageLength": 50 });
    }
    else {
        document.querySelector('#errMsg').innerHTML = event.responseJSON.view;
    }
}

function loadDocs() {
    showLoader('btn-load', 'btn-load-loader');
    const fromDate = document.querySelector('#fromDate');
    const toDate = document.querySelector('#toDate');

    loadRecentDocs(fromDate.value, toDate.value);
}
 