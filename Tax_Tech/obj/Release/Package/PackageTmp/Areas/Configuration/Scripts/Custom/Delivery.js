
function getCreateDeliveryModal() {
    sendGetRequest(`/ConfigPopup/GetCreateDeliveryPopup`, (e) => {
        if (e.target.readyState == 4 && e.target.status == 200) {
            document.querySelector('#popupDev').innerHTML = e.target.response;
            $('#AddDeliveryModal').modal('show');
        }
        HideProgress();
    });
}