window.addEventListener('load', function () {
    loadReceivedDocs();
});

function loadReceivedDocs() {
    sendGetRequest(`/Invoice/ReceivedDocsList`, function (e) {
        document.querySelector('#list').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });
}