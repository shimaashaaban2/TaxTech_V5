 
function getSubLogByDate() {
    const from = $('#Sub-log-fromDate').val();
    const to = $('#Sub-log-toDate').val();
    const status = $('#status').val();
    const lang = extractLangCookie();

    if (!from) {
        if (lang === 'ar') {
            Swal.fire({
                title: 'خطأ',
                text: "الرجاء اختيار تاريخ البدء",
                icon: 'error',
                showCancelButton: false,
                confirmButtonColor: '#7367f0'
            });
        }
        else {
            Swal.fire({
                title: 'Error',
                text: "Please Select From Date",
                icon: 'error',
                showCancelButton: false,
                confirmButtonColor: '#7367f0'
            });
        }
        return;
    }

    if (!to) {
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

    showLoader('subBtn-5', 'loadBtn-5');
    sendGetRequest(`/Submission/getSubmissionLogByDate?from=${from}&to=${to}&Status=${status}`, function (e) {
        $('#list').html(e);
        hideLoader('subBtn-5', 'loadBtn-5');
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });
}

function getSubLogByInternalID() {
    const InternalID = $('#Sub-log-InternalIDVal').val();
    const lang = extractLangCookie();

    if (!InternalID) {
        if (lang === 'ar') {
            Swal.fire({
                title: 'خطأ',
                text: "الرجاء ادخال الكود الداخلي",
                icon: 'error',
                showCancelButton: false,
                confirmButtonColor: '#7367f0'
            });
        }
        else {
            Swal.fire({
                title: 'Error',
                text: "Enter Internal ID",
                icon: 'error',
                showCancelButton: false,
                confirmButtonColor: '#7367f0'
            });
        }
        return;
    }

    showLoader('subBtn-4', 'loadBtn-4');
    sendGetRequest(`/Submission/getSubmissionLogByInternalID?InternalID=${InternalID}`, function (e) {
        $('#list').html(e);
        hideLoader('subBtn-4', 'loadBtn-4');
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });
}

function onEnterPressHandler(event) {
    if (event.keyCode === 13) {
        getSubLogByInternalID();
    }
}