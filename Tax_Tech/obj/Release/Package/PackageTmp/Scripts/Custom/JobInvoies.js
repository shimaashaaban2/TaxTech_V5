
function selectAll(event) {
    let inputs = [...document.querySelectorAll('.custom-control-input')];
    let truthyInputs = inputs.filter(i => i.checked == true);
    const CancelSelectedInvoices = document.querySelector('#CancelSelectedInvoices'); 

    inputs.forEach((ele, i) => {
        if (i > 0) {
            ele.checked = event.target.checked;
        }
    });

    if (event.target.checked) {
        CancelSelectedInvoices.disabled = false; 
    }
    else {
        CancelSelectedInvoices.disabled = true; 
    }
}

function onItemSelect(event) {
    let inputs = [...document.querySelectorAll('.custom-control-input')];
    let truthyInputs = inputs.filter(i => i.checked == true);

    if (truthyInputs.length > 0) {
        // enable buttons
        CancelSelectedInvoices.disabled = false; 
    }
    else {
        CancelSelectedInvoices.disabled = true; 
    }
}
function CancelSelected() {
    let inputs = [...document.querySelectorAll('.custom-control-input')];
    let truthyInputs = inputs.filter(i => i.checked == true);
    const allCheckBox = document.querySelector('#all-checkBox');
    let msg = '';
    let i = allCheckBox.checked ? 1 : 0;
    let jobID = document.getElementById('jobvalue').value;
   
        for (; i < truthyInputs.length; i++) {
            sendPostRequest(`/Invoice/GetCancellManyInvoices?JobID=${jobID}&InvoiceID=${truthyInputs[i].dataset.id}`, null, function (e) {
                if (e.msg === 'success') {
                    getMasterReport(e.id);
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