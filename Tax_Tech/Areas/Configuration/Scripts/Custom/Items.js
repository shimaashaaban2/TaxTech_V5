window.addEventListener('load', function () {
    getCountOfAll();
});

function getItems() {
    sendGetRequest(`/Configuration/Items/List`, (e) => {
        document.querySelector('#list').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
    });
}

function onAddTaxtoItem(event) {
    HideLoading(1);
    if (event.responseJSON) {
        document.querySelector('#itemsListContainer').innerHTML = event.responseJSON.view;
    }
    else {
        document.querySelector('#PopupMsg').innerHTML = event.responseText;
    }
}

function onChangeStatusResult(event) {
    if (event.responseJSON) {
        $("#itemsListContainer").html(event.responseJSON.view);
    }
    else {
        $("#PopupMsg").html(event.responseText);
    }
}

function getTaxSubTypes(event) {
    let taxId = event.target.selectedOptions[0].value;

    sendGetRequest(`/Configuration/Items/GetTaxSubTypes?taxId=${taxId}`, function (e) {
        $("#taxSubTypesContainer").html(e);
    });
}


function getCountOfAll() {
    sendGetRequest(`/Configuration/Items/CountOfAll`, (e) => {
        document.querySelector('#count').innerHTML = e;
    });
}
function getItemsByFilter(pageNo, pageSize) {
    filter = $("#FilterBy").val();
    const lang = extractLangCookie();

    if (filter.length < 3) {
        if(lang === 'ar')
            alert("الرجاء ادخال 3 حروف علي الاقل");
        else
            alert("Please Enter at Least 3 Characters");
        return;
    }

    showLoader('filterItem-btn', 'filterItem-btn-loader');
    sendGetRequest(`/Configuration/Items/ListByFilter?filter=${filter}&pageNo=${pageNo}&pageSize=${pageSize}`, (e) => {
        hideLoader('filterItem-btn', 'filterItem-btn-loader');

        document.querySelector('#list').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
    });
}

function getItemsByFilterAfterAction() {
    showLoader('filterItem-btn', 'filterItem-btn-loader');

    Filter = $("#FilterBy").val();

    sendGetRequest(`/Configuration/Items/ListByFilter?Filter=${Filter}`, (e) => {
        hideLoader('filterItem-btn', 'filterItem-btn-loader');

        document.querySelector('#list').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
    });
  
}
function PopupResult(event) {
    getCountOfAll();
    if (event.responseJSON.msg == "fail") {
        $('#PopupMsg').html(event.responseJSON.view);
        HideLoading(1);
    }
    else {//if (event.responseJSON.msg == "success")

        $('#list').html(event.responseJSON.view);
        DismissPopup();
        try {
            $('#thebasic-datatable').DataTable({ "pageLength": 100 });
        } catch (e) {

        }
    }

}

function onAddItem(event) {
    if (event.responseJSON.msg == "fail") {
        $('#PopupMsg').html(event.responseJSON.view);
        HideLoading(1);
    }
    else {
        getItemsByFilterAfterAction();
        DismissPopup();
    }
}

function showAfterActionResult() {
    getItemsByFilter();

}

function showAfterPopupActionResult(event) {
    $('#PopupMsg').html(event.responseJSON.view);
    HideLoading(1);

    if (event.responseJSON.msg == "success") {
        getTaxPriceHistory(event.responseJSON.itemId);
    }
    
}
function getTaxPriceHistory(id) {
    $.ajax({
        type: "GET",
        url: url + `Configuration/Items/GetPriceAfterTaxPopup?itemId=${id}`,
        processData: false,
        contentType: false,
        success: function (result) {
            $('#priceHistoryList').html(result);
            $('#thebasic-datatable1').DataTable({ "pageLength": 50 });
        },
        error: function (result) {
        }
    });
}
function getAllItems() {
    showLoader('getall-btn', 'getall-btn-loader');
    sendGetRequest(`/Configuration/Items/GetAllItems`, function (res) {
        hideLoader('getall-btn', 'getall-btn-loader');

        document.querySelector('#list').innerHTML = res;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
    });
}

function onKeyDownSearchText(event) {
    if (event.keyCode === 13) {
        document.querySelector('#filterItem-btn').click();
    }
}


//function getItemsGroup(event) {
//    let taxId = event.target.selectedOptions[0].value;

//    sendGetRequest(`/Configuration/Items/?taxId=${taxId}`, function (e) {
//        $("#").html(e);
//    });
//}
function onAddItemGroup(event) {
    HideLoading(1);
    if (event.responseJSON) {
        document.querySelector('#ItemGroupList').innerHTML = event.responseJSON.view;
    }
    else {
        document.querySelector('#PopupMsg').innerHTML = event.responseText;
    }
}

function onRemoveItemGroup(event) {
    if (event.responseJSON) {
        if (event.responseJSON.msg === 'fail') {
            document.querySelector('#PopupMsg').innerHTML = event.responseJSON.view;
        }
        else {
            document.querySelector('#ItemGroupList').innerHTML = event.responseJSON.view;
        }
    }
    else {
        document.querySelector('#PopupMsg').innerHTML = event.responseText;
    }
}

function ExportItemsToExcel() {
    const button = document.querySelector('#ItemsToExcel');
    button.innerHTML = "loading";
    button.disabled = true;

    // Send an AJAX request to the server
    fetch(url + `Items/ExportToExecl`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json'
        }
    })
        .then(response => {
            if (response.ok) {
                // Handle successful export (file download)
                return response.blob();
            } else {
                // If the response is an error (e.g., 401/403), handle the error
                return response.json().then(errorData => {
                    throw new Error(errorData.error || 'An error occurred while exporting');
                });
            }
        })
        .then(blob => {
            // Get the current date in YYYY-MM-DD format
            const currentDate = new Date().toISOString().split('T')[0];

            // Create a download link and trigger the download with dynamic file name
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            a.download = `ExportItems_${currentDate}.xlsx`; // File name with current date
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);

            // Re-enable the button after download
            button.innerHTML = "Export To Excel";
            button.disabled = false;
        })
        .catch(error => {
            // Handle the error (e.g., show a message to the user)
            alert("Error: " + error.message);

            // Re-enable the button after the error
            button.innerHTML = "Export To Excel";
            button.disabled = false;
        });
}