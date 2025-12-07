window.addEventListener('load', function () {
    loadImportStatuses();
});

function loadImportStatuses() {
    sendGetRequest(`/Reports/FailedImportedDocsList`, function (e) {
        document.querySelector('#list').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
    });
}

function exportFailedImportedDocsToExecl() {
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
    iframe.src = url + `Reports/ExportFailedImportedDocsReport`;
            document.body.appendChild(iframe);
        }
    });
}