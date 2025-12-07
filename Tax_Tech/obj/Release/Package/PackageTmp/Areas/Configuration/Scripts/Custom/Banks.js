window.addEventListener('load', function () {
    getAllBanks();
});

function getAllBanks() {
    sendGetRequest('Banks/BankList', (e) => {
        document.querySelector("#list").innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });
}

function changeBankFieldStatus(status, bankId) {
    sendPostRequest(`Banks/ChangeBankStatus?status=${status}&bankId=${bankId}`, null, (e) => {
        if (e.msg == 'success') {
            document.querySelector("#list").innerHTML = e.view;
        }
        else if (e.view) {
            document.querySelector("#main-err").innerHTML = e.view;
        }
        else {
            document.querySelector("#main-err").innerHTML = e;
        }
    });
}

function toggleInlineEdit(tdId, spanId, inputId) {
    const span = document.querySelector(`#${spanId}`);
    const input = document.querySelector(`#${inputId}`);

    if (input.style.display === 'none') {
        span.style.display = 'none';
        input.value = span.innerText;
        input.style.display = 'block';
        document.querySelector(`#${tdId}`).style.display = "block";
    }
    else {
        span.style.display = 'block';
        input.style.display = 'none';
        document.querySelector(`#${tdId}`).style.display = "none";
    }
}

function saveData(bankId, tdId, spanId, inputId) {

    const formData = new FormData();
    formData.append('BankName', document.querySelector(`#${inputId}`).value);
    formData.append('BankID', bankId);

    sendPostRequest('/Banks/UpdateBank', formData, (e) => {
        if (e.msg == 'success') {
            document.querySelector('#list').innerHTML = e.view;
            $('#thebasic-datatable').DataTable({ "pageLength": 50 });
        }
        else if (e.view) {
            document.querySelector("#main-err").innerHTML = e.view;
        }
        else {
            document.querySelector("#main-err").innerHTML = e;
        }
        HideLoading(0);
    });
}
