let currentPage = 1;
let pageSize = 50; // rows per page
let totalRecords = 0;
let totalPages = 0;
function DocumentSourceShowLoader(event) {
    showLoader('btn-DocSourceList', 'btn-DocSourceList-loader');
}
//function DocumentSourceList1(pageNo, pageSize) {
//    var from = $('#From').val();
//    var to = $('#To').val();
//   // var source = $('#source').val();
//    $("#PageNo").val(pageNo);
//    $("#PageSize").val(pageSize);
//    //var source=$(document).ready(function () {
//    //    $('#source').click(function () {
//    //        $('#combo :selected').text();
//    //    });
//    //});
//    hideLoader('btn-DocSourceList', 'btn-DocSourceList-loader');

//    sendGetRequest(`/DocumentSource/GetDocFilterBySource?From=${from}&To=${to}&source=${source}&pageNo=${pageNo}&pageSize=${pageSize}`, (e) => {

//        document.querySelector('#DocSource').innerHTML = e;

//        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
//    });

//}


function GetPageNo(pageNo) {
    localStorage.setItem("pageNo", pageNo);
    source = $("#source").val();
    var from = $('#From').val();
    var to = $('#To').val();
    
    var pageSize = $("#PageSize").val();
  //  alert(pageNo);
    totalPages = Math.ceil(totalRecords / pageSize);
    $.ajax({
        type: "GET",
        url: url + `DocumentSource/GetDocFilterBySource?From=${from}&To=${to}&source=${source}&pageNo=${pageNo}&pageSize=${pageSize}`,
        data: {},
        success: function (data) {

            $("#DocSource").html(data);
            // $('#thebasic-datatable').Table();
            $("#pageInfo").text(`Page ${currentPage} of ${totalPages}`);
        }
    });
}

function DocumentSourceList(pageNo) {
    //alert(pageNo)



    GetPageNo(pageNo)
    hideLoader('btn-DocSourceList', 'btn-DocSourceList-loader');
    $("#source").on("change",function () {
        
    });
    GetPageNo(pageNo)
}


//function getDocSourceWithPagination(PageNo, PageSize) {

//    $("#PageNo").val(PageNo);
//    $("#PageSize").val(PageSize);
//    $("#btn-DocSourceList").submit();
//}


//$(document).ready(function () {
   
//    // Load first page
//    DocumentSourceList(currentPage);

//    // Next page
//    $("#nextPage").on("click", function () {
        
//        if (currentPage < totalPages) {
//            currentPage++;
//            DocumentSourceList(currentPage);

//        }
//    });

//    // Previous page
//    $("#prevPage").on("click", function () {
//        if (currentPage > 1) {
//            currentPage--;
//            DocumentSourceList(currentPage);
//        }
//    });
//});
