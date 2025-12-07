window.addEventListener('load', function () {
    selectAllDropdowns();
});

function selectAllDropdowns() {
    const dropDown = $('.select2');

    if (dropDown.length > 0) {
        dropDown.each((i, e) => {
            if (e.options) {
                if (e.options.length == 2) {
                    e.value = e.options[1].value;
                }
            }
        });
    }
}

function getLangCookie() {
    if (document.cookie.split(';').filter(val => val.includes('lang')).length == 0)
        return null;
    return document.cookie.split(';').filter(val => val.includes('lang'))[0];
}

function ScrollTop() {
 
    $('html, body').animate({ scrollTop: 0 }, 800);
    //return false;
}

function ScrollTopHalf() {
    window.scrollTo(0, document.body.scrollHeight / 2);
}

function success(result) {
   // console.log("success");
    if (result.redirecturl) {
        //window.location.href = result.redirecturl;
        location.replace(result.redirecturl);
    }
}
function ShowLoading(id) {
    $("#subBtn-" + id).addClass("hidden");
    $("#loadBtn-" + id).removeClass("hidden");
}
function HideLoading(id) {

    $("#subBtn-" + id).removeClass("hidden");
    $("#loadBtn-" + id).addClass("hidden");
}

function showFormResult(event) {
    HideLoading(2);
    if (event.responseJSON) {
        document.querySelector('#result-msg').innerHTML = event.responseJSON.view;
    }
    else {
        document.querySelector('#result-msg').innerHTML = event.responseText;
    }
}


function showPopupMsgResult(event) {
    if (event.responseJSON.msg == "fail") {
        $('#PopupMsg').html(event.responseJSON.view);
        HideLoading(1);
    }
    else {
        DismissPopup();
        $("#msg").html(event.responseJSON.view);
        HideLoading(1);
    }
}
 
function showPopUpResult(event) {
    if (event.responseJSON.msg == "fail") {
        $('#PopupMsg').html(event.responseJSON.view);
        HideLoading(1);
    }
    else {//if (event.responseJSON.msg == "success")

        $('#list').html(event.responseJSON.view);
        try {
            document.querySelector('#exportType').innerHTML = event.responseJSON.exportType;
        } catch (e) {

        }
      

        DismissPopup();
        try {
            $('#thebasic-datatable').DataTable({ "pageLength": 50 });
        } catch (e) {
            
        }
    }
     
}

function showUpdateMailConfigPopupResult(event) {
    if (event.responseJSON.msg == "fail") {
        $('#PopupMsg').html(event.responseJSON.view);
        HideLoading(1);
    }
    else {
        DismissPopup();
        location.reload();
    }
}

function showDeleteResult(event) {
    if (event.responseJSON) {
        $('#list').html(event.responseJSON.view);
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    }
    else {
        $("#err-msg").html(event.responseText);
    }
}

function showInvoiceLineResult(event) {
    if (event.responseJSON) {
        if (event.responseJSON.msg == 'success') {
            $("#list").html(event.responseJSON.view);
            $("#invoiceTotals").html(event.responseJSON.totals);
            DismissPopup();
        }
        else {
            // in case of failure
            $("#PopupMsg").html(event.responseJSON.view);
            HideLoading(1);
        }
    }
    else {
        $('#list').html(event.responseText);
    }
}

function showUpdateTotalResult(event) {
    if (event.responseJSON) {
        if (event.responseJSON.msg == 'success') {
            $("#invoiceTotals").html(event.responseJSON.view);
            DismissPopup();
        }
        else {
            // in case of failure
            $("#PopupMsg").html(event.responseJSON.view);
        }
    }
    else {
        $("#err-msg").html(event.responseText);
    }
    HideLoading(1);
}
 
function DismissPopup() {
    $("#default").modal('hide');

    setTimeout(function () { $('#popupDev').html(''); }, 1000);
}

function sendGetRequest(pathUrl, onSuccess) {
    $.ajax({
        url: url + pathUrl,
        method: 'GET',
        success: onSuccess,
        cache: false,
        async: true,
        error: function (e) {
            //if (e.statusText) {
            //    Swal.fire({
            //        title: 'Error',
            //        text: e.statusText,
            //        icon: 'error',
            //        showCancelButton: false,
            //        confirmButtonColor: '#7367f0'
            //    });
            //}
            //else {
            //    Swal.fire({
            //        title: 'Error',
            //        text: "Cannot connect to the server",
            //        icon: 'error',
            //        showCancelButton: false,
            //        confirmButtonColor: '#7367f0'
            //    });
            //}
        }
    });
}

function onCreateInvoiceHeadSubmit(event) {
    if (event.responseJSON) {
        if (event.responseJSON.msg == 'fail') {
            document.querySelector('#PopupMsg').innerHTML = event.responseJSON.view;
        }
        else {
            location.href = event.responseJSON.redirecturl;
        }
    }
    else {
        document.querySelector('#PopupMsg').innerHTML = event.responseText;
    }
}

function sendPostRequest(pathUrl, body, onSuccess, btnID) {
    ShowLoading(btnID);
    $.ajax({
        url: url + pathUrl,
        method: 'POST',
        success: onSuccess,
        cache: false,
        async: true,
        data: body,
        processData: false,
        contentType: false,
        error: function (e) {
            //if (e.statusText) {
            //    Swal.fire({
            //        title: 'Error',
            //        text: e.statusText,
            //        icon: 'error',
            //        showCancelButton: false,
            //        confirmButtonColor: '#7367f0'
            //    });
            //}
            //else {
            //    Swal.fire({
            //        title: 'Error',
            //        text: "Cannot connect to the server",
            //        icon: 'error',
            //        showCancelButton: false,
            //        confirmButtonColor: '#7367f0'
            //    });
            //}
        }
    });
}

function getPopup(pathUrl, onComplete = null) {
    ShowLoading(1);
    $.ajax({
        url: url + pathUrl,
        method: 'GET',
        cache: false,
        async: true,
        error: function (e) {
            if (e.message) {
                alert(e.message);
            }
            else {
                alert("Cannot connect to the server");
            }
            HideLoading(1);
        },
        success: function (result) {
            $('#popupDev').html(result);
            $("#default").modal('show');
            HideLoading(1);
            //selectAllDropdowns();

            if (onComplete != null) {
                onComplete();
            }
        }
    });
}

function getDDL(pathUrl,TargetId) {
    //ShowLoading(1);
    $.ajax({
        url: url + pathUrl,
        method: 'GET',
        cache: false,
        async: true,
        error: function (e) {
            if (e.message) {
                alert(e.message);
            }
            else {
                alert("Cannot connect to the server");
            }
           // HideLoading(1);
        },
        success: function (result) {
            $('#' + TargetId).html('');
            $('#' + TargetId).html(result);
             //HideLoading(1);
        }
    });
}

function showAlert(title, msg) {
    Swal.fire({
        title,
        text: msg,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire(
                'Deleted!',
                'Deleted Successfully.',
                'success'
            )
        }
        });
}

function changeInvoiceStatusResult(event) {
    if (event.responseJSON) {
        if (event.responseJSON.msg == 'success') {
            const statusTitleEle = document.querySelector('#StatusTitleContainer');
            const list = document.querySelector('#list');

            if (statusTitleEle) {
                statusTitleEle.value = event.responseJSON.data.ProcessStatusTitle
            }
            else if(list && location.href.toLocaleLowerCase().includes('invoice/index')){
                document.querySelector('#list').innerHTML = event.responseJSON.view;
                $('#thebasic-datatable').DataTable({ "pageLength": 50, paging: false});
                document.querySelector(event.responseJSON.invoiceId).scrollIntoView();
            }
            DismissPopup();
        }
        else {
            $("#PopupMsg").html(event.responseJSON.view);
        }
    }
    else {
        $("#PopupMsg").html(event.responseText);
    }
    HideLoading(1);
}


function onCreateInvoiceHeadSubmit(event) {
    HideLoading(1);
    if (event.responseJSON.msg == 'fail') {
        document.querySelector('#PopupMsg').innerHTML = event.responseJSON.view;
    }
    else {
        location.href = event.responseJSON.redirecturl;
    }
}

function showLoader(btnId, btnLoaderId) {
    try {
        document.querySelector(`#${btnId}`).style.display = 'none';
        document.querySelector(`#${btnLoaderId}`).style.display = 'block';
    }
    catch (e) {}
}

function hideLoader(btnId, btnLoaderId) {
    try {
        document.querySelector(`#${btnLoaderId}`).style.display = 'none';
        document.querySelector(`#${btnId}`).style.display = 'block';
    }
    catch (e) {

    }
}