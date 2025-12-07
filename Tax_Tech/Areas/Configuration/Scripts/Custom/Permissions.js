

function filterByUserId() {
    let userId = document.querySelector('#UserID').value;
    document.querySelector('#list').innerHTML = `
        <div class="my-3 d-flex justify-content-center">
            <span class="my-2 spinner-border"></span>
        </div>
    `;

    sendGetRequest(`/Configuration/Permissions/GetPermissionsOfUser?userId=${userId}`, function (res) {
        document.querySelector('#list').innerHTML = res;
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });
}

function addPermissionToUser(event) {
    $.blockUI({
        message: '<div class="spinner-border text-white" role="status"></div>',
        timeout: 1000,
        css: {
            backgroundColor: 'transparent',
            border: '0'
        },
        overlayCSS: {
            opacity: 0.5
        },
        onBlock: function () {
            let userId = document.querySelector('#UserID').value;
            let actionId = event.target.dataset.actionId;

            sendPostRequest(`/Configuration/Permissions/AddActionToUser?actionId=${actionId}&userId=${userId}`, null, function (res) {
                $.unblockUI();
                document.querySelector('#main-err-area').innerHTML = res.view;
                scrollBy(0, -10000);
                filterByUserId();
            });
        }
    });
}

function removePermissionFromUser(event) {
    $.blockUI({
        message: '<div class="spinner-border text-white" role="status"></div>',
        timeout: 1000,
        css: {
            backgroundColor: 'transparent',
            border: '0'
        },
        overlayCSS: {
            opacity: 0.5
        },
        onBlock: function () {
            let userId = document.querySelector('#UserID').value;
            let actionId = event.target.dataset.actionId;

            sendPostRequest(`/Configuration/Permissions/RemoveActionFromUser?actionId=${actionId}&userId=${userId}`, null, function (res) {
                $.unblockUI();
                document.querySelector('#main-err-area').innerHTML = res.view;
                scrollBy(0, -10000);
                filterByUserId();
            });
        }
    });
}