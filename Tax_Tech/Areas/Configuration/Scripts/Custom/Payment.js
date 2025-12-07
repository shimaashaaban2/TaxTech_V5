window.addEventListener('load', function () {
    getPayments();
});

function getPayments() {
    sendGetRequest('/Payment/PaymentList', (e) => {
        document.querySelector('#list').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });
}