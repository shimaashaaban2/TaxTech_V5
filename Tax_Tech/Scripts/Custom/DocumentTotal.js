function filterByDate() {
    const fromDate = document.querySelector('#fromDate');
    const toDate = document.querySelector('#toDate');
    const docTypeID = document.querySelector('#DocumentTypeID');
    const lang = extractLangCookie();

    if (!fromDate.value) {
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

    if (!toDate.value) {
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

    if (!docTypeID.value) {
        if (lang === 'ar') {
            Swal.fire({
                title: 'خطأ',
                text: "الرجاء اختيار نوع المستند",
                icon: 'error',
                showCancelButton: false,
                confirmButtonColor: '#7367f0'
            });
        }
        else {
            Swal.fire({
                title: 'Error',
                text: "Please Select Document Type",
                icon: 'error',
                showCancelButton: false,
                confirmButtonColor: '#7367f0'
            });
        }
        return;
    }

    ShowLoading(2);
    sendGetRequest(`/Reports/FilterDocumentCountByDate?fromDate=${fromDate.value}&toDate=${toDate.value}&docTypeID=${docTypeID.value}`, function (e) {
        document.querySelector('#list').innerHTML = e;
        HideLoading(2);
    });
}

function exportDocCountsToExecl() {
    const fromDate = document.querySelector('#fromDate');
    const toDate = document.querySelector('#toDate');
    const docTypeID = document.querySelector('#DocumentTypeID');
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

    if (!docTypeID.value) {
        if (lang === 'ar') {
            alert('الرجاء تحديد نوع المستند');
        }
        else {
            alert('Please Specify Document Type');
        }
        return;
    }
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
    iframe.src = url + `Reports/ExportDocCountReport?fromDate=${fromDate.value}&toDate=${toDate.value}&docTypeID=${docTypeID.value}`;
            document.body.appendChild(iframe);

        }
    });
}