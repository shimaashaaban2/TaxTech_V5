
function selectAll(event) {
    let inputs = [...document.querySelectorAll('.custom-control-input')];
    let truthyInputs = inputs.filter(i => i.checked == true);
    const btnApproveAll = document.querySelector('#btnApproveAll');
    const btnRejectAll = document.querySelector('#btnRejectAll');

    inputs.forEach((ele, i) => {
        if (i > 0) {
            ele.checked = event.target.checked;
        }
    });

    if (event.target.checked) {
        btnApproveAll.disabled = false;
        btnRejectAll.disabled = false;
    }
    else {
        btnApproveAll.disabled = true;
        btnRejectAll.disabled = true;
    }
}

function onItemSelect(event) {
    let inputs = [...document.querySelectorAll('.custom-control-input')];
    let truthyInputs = inputs.filter(i => i.checked == true);

    if (truthyInputs.length > 0) {
        // enable buttons
        btnApproveAll.disabled = false;
        btnRejectAll.disabled = false;
    }
    else {
        btnApproveAll.disabled = true;
        btnRejectAll.disabled = true;
    }
}

function changeStatusBulk(opType) {
    let inputs = [...document.querySelectorAll('.custom-control-input')];
    let truthyInputs = inputs.filter(i => i.checked == true);
    const allCheckBox = document.querySelector('#all-checkBox');
    let msg = '';
    let i = allCheckBox.checked ? 1 : 0;

    if (opType === 'approve') {
        console.log('approve');
        for (; i < truthyInputs.length; i++) {
            sendPostRequest(`/Invoice/ChangeInvoiceStatus?docId=${truthyInputs[i].dataset.id}&status=${2}&msg=${msg}`, null, function (e) {
                if (e.msg == 'success') {
                    document.querySelector('#list').innerHTML = e.view;
                    $('#thebasic-datatable').DataTable({ "pageLength": 100 });
                    document.querySelector('#errMsg').innerHTML = `
                        <div class="alert alert-success alert-dismissable">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                            <i class="glyphicon glyphicon-check"></i> Status Changed Successfully
                        </div>
                    `;
                }
                else {
                    document.querySelector('#errMsg').innerHTML = e.view;
                }
            });
        }
    }
    else if (opType === 'rej') {
        console.log('rej');
        ShowLoading('1');
        msg = document.querySelector('#rejectMsg').value || '';
        for (; i < truthyInputs.length; i++) {
            sendPostRequest(`/Invoice/ChangeInvoiceStatus?docId=${truthyInputs[i].dataset.id}&status=${8}&msg=${msg}`, null, function (e) {
                HideLoading('1');
                $('#default').modal('hide');
                if (e.msg == 'success') {
                    document.querySelector('#list').innerHTML = e.view;
                    $('#thebasic-datatable').DataTable({ "pageLength": 100 });
                    document.querySelector('#errMsg').innerHTML = `
                        <div class="alert alert-success alert-dismissable">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                            <i class="glyphicon glyphicon-check"></i> Status Changed Successfully
                        </div>
                    `;
                }
                else {
                    document.querySelector('#errMsg').innerHTML = e.view;
                }
            });
        }
    }
}

function ApproveAll() {
    
    sendPostRequest(`/Invoice/ApproveAll`, null, function (e) {
        if (e.msg == 'success') {
            document.querySelector('#list').innerHTML = e.view;
            $('#thebasic-datatable').DataTable({ "pageLength": 100 });
            document.querySelector('#errMsg').innerHTML = `
                        <div class="alert alert-success alert-dismissable">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                            <i class="glyphicon glyphicon-check"></i> Status Changed Successfully
                        </div>
                    `;
        }
        else {
            document.querySelector('#errMsg').innerHTML = e.view;
        }
    });
}
