function ScrollTop() {

    $('html, body').animate({ scrollTop: 0 }, 800);
    //return false;
}
//function ShowProgress() {
//    $(".btn-submit").addClass("hidden");
//    $(".btn-load").removeClass("hidden");

//}
//function HideProgress() {
//    $(".btn-submit").removeClass("hidden");
//    $(".btn-load").addClass("hidden");

//}


//function ShowProgressByID(submit, load) {
//    $("#" + submit).addClass("hidden");
//    $("#" + load).removeClass("hidden");

//}
//function HideProgressByID(submit, load) {
//    $("#" + submit).removeClass("hidden");
//    $("#" + load).addClass("hidden");

//}

//function ShowHideByCheck(id, Div) {
//    if ($(id).prop("checked") == true) {
//        $("#" + Div).removeClass("hide");
//    }
//    else if ($(id).prop("checked") == false) {
//        $("#" + Div).addClass("hide");
//    }

//}
//function DismissPopup() {
//    $("#default").modal('hide');

//    setTimeout(function () { $('#popupDev').html(''); }, 1000);
//}

//function DismissPopupOf(id) {
//    $(id).modal('hide');

//    setTimeout(function () { $('#popupDev').html(''); }, 500);
//}


//function sendGetRequest(pathUrl, onSuccess) {
//    ShowProgress();
//    let xhr = new XMLHttpRequest();

//    xhr.onerror = function (e) {
//        if (e.message) {
//            alert(e.message);
//        }
//        else {
//            alert("Cannot connect to the server");
//        }
//        HideProgress();
//    }

//    xhr.open("GET", url + `${pathUrl}`, true);

//    xhr.onreadystatechange = onSuccess;

//    xhr.send();
//}

//function sendPostRequest(pathUrl, body, onSuccess) {
//    ShowProgress();
//    let xhr = new XMLHttpRequest();

//    xhr.onerror = function (e) {
//        if (e.message) {
//            alert(e.message);
//        }
//        else {
//            alert("Cannot connect to the server");
//        }
//        HideProgress();
//    }

//    xhr.open("POST", url + `${pathUrl}`, true);

//    xhr.onreadystatechange = onSuccess;

//    xhr.send(body);
//}