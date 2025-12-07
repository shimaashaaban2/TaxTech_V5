

function getRejectedDocs() {
    sendGetRequest(`/Reports/DocumentsRejectedList`, function (e) {
        document.querySelector('#list').innerHTML = e;
    });
}

function filterByDate() {
    const startDate = document.querySelector('#fromDate');
    const endDate = document.querySelector('#toDate');
    const lang = extractLangCookie();

    if (!startDate.value) {
        if (lang === 'ar') {
            Swal.fire({
                title: 'Error',
                text: 'Please Select From Date',
                icon: 'error',
                showCancelButton: false,
                confirmButtonColor: '#7367f0'
            });
        }
        else {
            Swal.fire({
                title: 'خطأ',
                text: 'الرجاء اختيار تاريخ البدء',
                icon: 'error',
                showCancelButton: false,
                confirmButtonColor: '#7367f0'
            });
        }
        return;
    }

    if (!endDate.value) {
        if (lang === 'ar') {
            Swal.fire({
                title: 'خطأ',
                text: "الرجاء اختيار تاريخ الانتهاء",
                icon: 'error',
                showCancelButton: false,
                confirmButtonColor: '#7367f0'
            });
        }
        else {
            Swal.fire({
                title: 'Error',
                text: "Please Select To Date",
                icon: 'error',
                showCancelButton: false,
                confirmButtonColor: '#7367f0'
            });
        }
        return;
    }

    ShowLoading(2);
    sendGetRequest(`Reports/FilterRejectedDocsByDate?fromDate=${startDate.value}&toDate=${endDate.value}`, function (e) {
        document.querySelector('#list').innerHTML = e;
        HideLoading(2);
    });
}

function exportRejectedDocsToExecl() {
    const fromDate = document.querySelector('#fromDate');
    const toDate = document.querySelector('#toDate');
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

    const iframe = document.createElement('iframe');
    iframe.style.display = 'none';
    iframe.id = 'iframe-execl';
    iframe.src = url + `Reports/ExportDocsRejectedReport?fromDate=${fromDate.value}&toDate=${toDate.value}`;
    document.body.appendChild(iframe);
}