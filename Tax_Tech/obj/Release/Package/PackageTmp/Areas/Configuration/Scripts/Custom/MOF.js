//window.addEventListener('load', function () {
//    if ($('#Hlog').val()) {
//        getMOFByDate();

//    }
//});

function PostMOFData(card) {
    Entityid = $("#EntityId").val();
    ClientID = $("#ClientID").val();
    ClientSecret = $("#ClientSecret").val();
    DefaultActivityCode = $("#DefaultActivityCode").val();
    DocumentVersion = $("#DocumentVersion").val();
    EnviromentBool = null;
    let formData = new FormData();
    formData.append('card', card);
    formData.append('Entityid', Entityid);
    formData.append('ClientID', ClientID);
    formData.append('ClientSecret', ClientSecret);
    formData.append('DefaultActivityCode', DefaultActivityCode);
    formData.append('DocumentVersion', DocumentVersion);
    formData.append('EnviromentBool', EnviromentBool);

    sendPostRequest(`MOFConfig/PostMOFData`, formData, (e) => {
        if (e.msg == 'msg') {
            $("#msg").html(e.view);
        }

    });
}
function OnchangeRadio(val, card) {
    Entityid = $("#EntityId").val();
    ClientID = $("#ClientID").val();
    ClientSecret = $("#ClientSecret").val();
    DefaultActivityCode = $("#DefaultActivityCode").val();
    DocumentVersion = $("#DocumentVersion").val();
    if (val == 1) {
        EnviromentBool = "1";
    }
    else {
        EnviromentBool = "0";
    }

    let formData = new FormData();
    formData.append('card', card);
    formData.append('Entityid', Entityid);
    formData.append('ClientID', ClientID);
    formData.append('ClientSecret', ClientSecret);
    formData.append('DefaultActivityCode', DefaultActivityCode);
    formData.append('DocumentVersion', DocumentVersion);
    formData.append('EnviromentBool', EnviromentBool);

    sendPostRequest(`MOFConfig/PostMOFData`, formData, (e) => {
        if (e.msg == 'msg') {
            $("#msg").html(e.view);
        }

    });
    //sendPostRequest(`MOFConfig/PostMOFData?card=${card}&Entityid=${Entityid}
    //                &ClientID=${ClientID}&ClientSecret=${ClientSecret}&DefaultActivityCode=${DefaultActivityCode}
    //                &DocumentVersion=${DocumentVersion}&EnviromentBool=${EnviromentBool}`, null, (e) => {
    //        if (e.msg == 'msg') {
    //            $("#msg").html(e.view);
    //        }

    //    });
}
function showMOFFormResult(event) {

    // HideLoading(1);
    if (event.responseJSON.msg == "msg") {
        $('#msg').html(event.responseJSON.view);

    } else
        if (event.responseJSON.msg == "result") {
            $('#ContainerDiv').html(event.responseJSON.view);
        }
}
function getMOFByDate() {
    const from = $('#fromDate').val();
    const to = $('#toDate').val();

    if (!from) {
        Swal.fire({
            title: 'Error',
            text: "Please Select From Date",
            icon: 'error',
            showCancelButton: false,
            confirmButtonColor: '#7367f0'
        });
        return;
    }

    if (!to) {
        Swal.fire({
            title: 'Error',
            text: "Please Select To Date",
            icon: 'error',
            showCancelButton: false,
            confirmButtonColor: '#7367f0'
        });
        return;
    }


    ShowLoading(1);
    sendGetRequest(`/Configuration/Logs/getMOFByDate?from=${from}&to=${to}`, function (e) {
        $('#list').html(e);
        HideLoading(1);
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });
}
function getMOFByInternalID() {
    const InternalID = $('#InternalIDVal').val(); 
    //alert(InternalID);
    if (!InternalID) {
        Swal.fire({
            title: 'Error',
            text: "Enter Internal ID",
            icon: 'error',
            showCancelButton: false,
            confirmButtonColor: '#7367f0'
        });
        return;
    }
 
    showLoader('subBtn-3', 'loadBtn-3');
    sendGetRequest(`/Configuration/Logs/getMOFLogByInternalID?InternalID=${InternalID}`, function (e) {
        $('#list').html(e);
        hideLoader('subBtn-3', 'loadBtn-3');
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });
}

function onEnterPressHandler(event) {
    if (event.keyCode === 13) {
        getMOFByInternalID();
    }
}

function submitForm() {
    $("#subBtn-1").click();
}