function loadDocs(pageNo, pageSize) {
    showLoader('btn-load', 'btn-load-loader');
    const fromDate = document.querySelector('#fromDate');
    const toDate = document.querySelector('#toDate');

    loadSearchDocs(fromDate.value, toDate.value, pageNo, pageSize);
}

function loadSearchDocs(fromDate, toDate, pageNo, pageSize) {
    sendGetRequest(`/Invoice/GetSearchDocsList?fromDate=${fromDate}&toDate=${toDate}&pageNo=${pageNo}&pageSize=${pageSize}`, function (e) {
        hideLoader('btn-load', 'btn-load-loader');

        if (e.msg == 'success') {
            document.querySelector('#list').innerHTML = e.view;
            SearchDocs = e.data;
            $("#thebasic-datatable").DataTable({ "pageLength": 10 });
        }
        else {
            document.querySelector('#errMsg').innerHTML = e.view;
        }
    });
}

 

function exportToExeclSearch() {
 
    const fromDate = document.querySelector('#From');
    const toDate = document.querySelector('#To');
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

    document.querySelector('#btnDocExportToExecl').disabled = true;
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
    iframe.src = url + `Invoice/ExportSearchDocsToExecl?fromDate=${fromDate.value}&toDate=${toDate.value}`;
    document.body.appendChild(iframe);

            document.querySelector('#btnDocExportToExecl').disabled = false;
        }
    });
}

 