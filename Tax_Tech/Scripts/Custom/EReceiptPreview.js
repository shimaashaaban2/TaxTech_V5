


//function GetEReceiptLines(id) {
//    sendPostRequest(`/EReceipt/GetReceiptLines?EReceiptID=${id}`, function (data) {
//        document.querySelector('#EReceiptLines').innerHTML = data;
//        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
//        $("[rel=tooltip]").tooltip({ html: true });
//    });
//}
//function GetInvoiceLines(id) {
//    sendPostRequest(`/Invoice/GetInvoiceLines?invoiceId=${id}`, (e) => {
//        document.querySelector('#invoiceLinesContainer').innerHTML = e;
//        $("[rel=tooltip]").tooltip({ html: true });
//    });
//}
//function getReceiptTotals(id) {
//    sendPostRequest(`/EReceipt/GetEReceiptTotals?EReceiptID=${id}`, function (data) {

//        $("#EReceiptTotals").html(data.view);



//    });
//}

function GetEReceiptLines(Id) {
   //// alert(Id);
   // sendPostRequest(`/EReceipt/GetLines`, function (data) {
   //     alert(1);
   //     //document.querySelector('#EReceiptLines1').innerHTML = data;
        
   // });
    sendGetRequest(`/EReceipt/GetReceiptLines?EReceiptID=${Id}`, (e) => {
       // alert(1);
        document.querySelector('#EReceiptLines1').innerHTML = e;
        //$('#thebasic-datatable').DataTable({ "pageLength": 50 });
        $("[rel=tooltip]").tooltip({ html: true });
    });

    //sendPostRequest(`/EReceipt/GetReceiptLines?EReceiptID=${Id}`, function (res) {

    //    alert(res.view);
    //   // document.getElementById("EReceiptLines").innerHTML = res;

    //   // $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    //   //// $("[rel=tooltip]").tooltip({ html: true });
    //});
}