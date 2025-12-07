
function GetInvoiceLines(id) {
    sendPostRequest(`/Invoice/GetInvoiceLines?invoiceId=${id}`, (e) => {
            document.querySelector('#invoiceLinesContainer').innerHTML = e;
            $("[rel=tooltip]").tooltip({ html: true });
    });
}


function SendInvoiceToSubmit(id) {
    ShowLoading(10);
   // alert();
    sendPostRequest(`/Invoice/SendInvoiceToSubmit?DocumentId=${id}`, null, (e) => {
       // alert(e.UUID);
        document.querySelector("#msg").innerHTML = e.view;
        if (e.msg == 'success') {
            //document.querySelector('#uuid').innerHTML = e.UUID;      
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