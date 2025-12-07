window.addEventListener('load', function () {
    getCountOfAll();
});

function getItems() {
    sendGetRequest(`/Items/List`, (e) => {
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

    sendGetRequest(`/Items/GetTaxSubTypes?taxId=${taxId}`, function (e) {
        $("#taxSubTypesContainer").html(e);
    });
}


function getCountOfAll() {
    sendGetRequest(`/Items/CountOfAll`, (e) => {
        document.querySelector('#count').innerHTML = e;
    });
}
function getItemsByFilter(pageNo, pageSize) {
    filter = $("#FilterBy").val();
    const lang = extractLangCookie();

    if (filter.length < 3) {
        if (lang === 'ar')
            alert('الرجاء ادخال 3 حروف علي الاقل');
        else
            alert('Please Enter at Least 3 Characters or More');
        return;
    }

    if (filter) {
        showLoader('btnFilterItems', 'btnFilterItemsLoader');
        sendGetRequest(`/Items/ListByFilter?filter=${filter}&pageNo=${pageNo}&pageSize=${pageSize}`, (e) => {
            hideLoader('btnFilterItems', 'btnFilterItemsLoader');
            document.querySelector('#list').innerHTML = e;
            $('#thebasic-datatable').DataTable({ "pageLength": 100 });
        });
    }
    else {
        if(lang === 'ar')
            alert('الرجاء ادخال كلمة البحث');
        else
            alert('Please Enter Text to Search');
    }
}

function onKeyDownFreeText(event) {
    if (event.keyCode === 13) {
        document.querySelector('#btnFilterItems').click();
    }
}

function getItemsByFilterAfterAction() {
    Filter = $("#FilterBy").val();
    if (Filter) {
        sendGetRequest(`/Items/ListByFilter?Filter=${Filter}`, (e) => {
            document.querySelector('#list').innerHTML = e;
            $('#thebasic-datatable').DataTable({ "pageLength": 100 });
        });
    }
  
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

function showAfterActionResult() {
    getItemsByFilter();
}

function showAfterPopupActionResult(event) {
    //getItemsByFilter();
    getItemsByFilterAfterAction();
    if (event.responseJSON.msg == "fail") {
        $('#PopupMsg').html(event.responseJSON.view);
        HideLoading(1);
    }
    else {
        DismissPopup();  
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





//function ExportItemsToExcel2() {
//    const button = document.querySelector('#ItemsToExcel22');
//    button.innerHTML = "loading";
//    button.disabled = true;

//    // Send an AJAX request to the server
//    fetch(url + `Items/ExportToExecl_Old`, {
//        method: 'GET',
//        headers: {
//            'Accept': 'application/json'
//        }
//    })
//        .then(response => {
//            if (response.ok) {
//                // Handle successful export (file download)
//                return response.blob();
//            } else {
//                // If the response is an error (e.g., 401/403), handle the error
//                return response.json().then(errorData => {
//                    throw new Error(errorData.error || 'An error occurred while exporting');
//                });
//            }
//        })
//        .then(blob => {
//            // Get the current date in YYYY-MM-DD format
//            const currentDate = new Date().toISOString().split('T')[0];

//            // Create a download link and trigger the download with dynamic file name
//            const url = window.URL.createObjectURL(blob);
//            const a = document.createElement('a');
//            a.style.display = 'none';
//            a.href = url;
//            a.download = `ExportItems_${currentDate}.xlsx`; // File name with current date
//            document.body.appendChild(a);
//            a.click();
//            window.URL.revokeObjectURL(url);

//            // Re-enable the button after download
//            button.innerHTML = "Export To Excel";
//            button.disabled = false;
//        })
//        .catch(error => {
//            // Handle the error (e.g., show a message to the user)
//            alert("Error: " + error.message);

//            // Re-enable the button after the error
//            button.innerHTML = "Export To Excel";
//            button.disabled = false;
//        });
//}