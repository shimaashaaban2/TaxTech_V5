window.addEventListener('load', function () {
    EReceiptSourceList();
});

function EReceiptSourceList() {
    sendGetRequest(`/EReceiptSource/GetEReceiptSource`, (e) => {
        document.querySelector('#ERectSource').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });
}
function EReceiptSourceShowLoader(event) {
    showLoader('btn-ERectSourceList', 'btn-ERectSourceList-loader');
}

//function EReceiptSourceList(pageNo) {
//    //alert(pageNo)
//    GetPageNo(pageNo)
//    hideLoader('btn-DocSourceList', 'btn-DocSourceList-loader');
//    $("#source").on("change", function () {

//    });
//    GetPageNo(pageNo)
//}


function GetErectPageNo(pageNo) {
 
    localStorage.setItem("pageNo", pageNo);
    source = $("#source").val();
    var from = $('#From').val();
    var to = $('#To').val();

    var pageSize = $("#PageSize").val();

    $.ajax({
        type: "GET",
        url: url + `EReceiptSource/GetEReceiptSource?FromDate=${from}&ToDate=${to}&source=${source}&PageNumber=${pageNo}&pageSize=${pageSize}`,
        data: {},
        success: function (data) {
            $("#ERectSource").html(data);
            alert(data)

            // FIX: Remove this line or replace with DataTable initialization if needed
            // $('#thebasic-datatable').Table(); // THIS LINE CAUSES THE ERROR

            $("#pageInfo").text(`Page ${pageNo}`);
        }
    });
}