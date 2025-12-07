

function getInvoices(id) {
    sendGetRequest(`/Invoice/List?status=${id}`, (e) => {
        if (e.target.readyState == 4 && e.target.status == 200) {
            document.querySelector('#InvoiceContainer').innerHTML = e.target.response;
            $('#thebasic-datatable').DataTable();
            //$('[data-toggle="tooltip"]').tooltip();
            $("[rel=tooltip]").tooltip({html: true });
        }
    });
}


