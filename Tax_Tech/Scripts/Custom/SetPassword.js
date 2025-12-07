function OnChangePassword(event) {
    HideLoading(1);

    if (event.responseJSON.msg === 'success') {
        if (event.responseJSON.redirecturl) {
            //window.location.href = result.redirecturl;
            location.replace(event.responseJSON.redirecturl);
        }
    }
    else {
        document.querySelector('#msg').innerHTML = event.responseJSON.view;
    }
}