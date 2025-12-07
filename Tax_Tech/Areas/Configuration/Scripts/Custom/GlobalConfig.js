
window.addEventListener('load', function () {
    getConfigList();
});

function getConfigList() {
    sendGetRequest(`/GlobalConfig/List`, function (e) {
        document.querySelector("#list").innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 10 });
    });
}