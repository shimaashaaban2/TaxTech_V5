function getCreditNoteLines(docId) {
    sendGetRequest(`/CreditNote/CreditLines?docId=${docId}`, function (e) {
        document.querySelector('#invoiceLinesContainer').innerHTML = e;
    });
}
function SendInvoiceToSubmit(id) {
    ShowLoading(10);
    // alert();
    sendPostRequest(`/CreditNote/SendInvoiceToSubmit?DocumentId=${id}`, null, (e) => {
        // alert(e.UUID);
        document.querySelector("#msg").innerHTML = e.view;
        if (e.msg == 'success') {
           // document.querySelector('#uuid').innerHTML = e.UUID;
        }
    });
    HideLoading(10);
}
function onRejectActionComplete(event) {
    HideLoading(1);
    if (event.responseJSON) {
        document.querySelector('#PopupMsg').innerHTML = event.responseJSON.view;
    }
    else {
        document.querySelector('#PopupMsg').innerHTML = event.responseText;
    }
}