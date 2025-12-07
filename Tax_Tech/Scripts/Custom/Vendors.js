window.addEventListener('load', function () {
    getCountOfAll();
});

function getVendors() {
    sendGetRequest(`/Vendors/List`, (e) => {
        document.querySelector('#list').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
    });
}
function getCountOfAll() {
    sendGetRequest(`/Vendors/CountOfAll`, (e) => {
        document.querySelector('#count').innerHTML = e; 
    });
}

function getVendorsByFilter(pageNo, pageSize) {
    filter = $("#FilterBy").val();
    const lang = extractLangCookie();

    if (filter.length < 3) {
        if (lang === 'ar')
            alert("الرجاء ادخال 3 حروف علي الاقل");
        else
            alert("Please Enter at Least 3 Characters");
        return;
    }
   
    if (filter) {
        showLoader('btnFilterCustomer', 'btnFilterCustomerLoader');
        sendGetRequest(`/Vendors/ListByFilter?filter=${filter}&pageNo=${pageNo}&pageSize=${pageSize}`, (e) => {
            hideLoader('btnFilterCustomer', 'btnFilterCustomerLoader');
            document.querySelector('#list').innerHTML = e;
            $('#thebasic-datatable').DataTable({ "pageLength": 100 });
        });
    }
    else {
        if (lang === 'ar')
            alert('الرجاء ادخال كلمة البحث');
        else
            alert('Please Enter Text to Search');
    }
}
function getVendorsByFilterAfterAction() {
    Filter = $("#FilterBy").val();
    if (Filter) {
        sendGetRequest(`/Vendors/ListByFilter?Filter=${Filter}`, (e) => {
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
    getVendorsByFilter();

}
function GetRegInfo() {
   // alert($("#VendorAPIType").val())
    if ($("#VendorAPIType").val()==3) {
        $("#PersonalID").val('0');
    }
}
function showAfterPopupActionResult(event) {
    //getVendorsByFilter();
    getVendorsByFilterAfterAction();
    if (event.responseJSON.msg == "fail") {
        $('#PopupMsg').html(event.responseJSON.view);
        HideLoading(1);
    }
    else {

        DismissPopup();

    }

}

function onKeyDownSearchText(event) {
    if (event.keyCode === 13) {
        document.querySelector('#btnFilterCustomer').click();
    }
}

function getAllIExemptedTaxesByVendorId(vendorId) {
    sendGetRequest(`/Vendors/GetAllIExemptedTaxesByVendorId?vendorId=${vendorId}`, function (res) {
        document.querySelector('#itemsListContainer').innerHTML = res;
    });
}

function onAddTaxtoVendor(event) {
    HideLoading(1);
    if (event.responseJSON) {
        if (event.responseJSON.msg === 'fail') {
            document.querySelector('#PopupMsg').innerHTML = event.responseJSON.view;
        }
        else {
            document.querySelector('#itemsListContainer').innerHTML = event.responseJSON.view;
        }
    }
    else {
        document.querySelector('#PopupMsg').innerHTML = event.responseText;
    }
}

function onRemoveTaxResult(event) {
    if (event.responseJSON) {
        if (event.responseJSON.msg === 'fail') {
            document.querySelector('#PopupMsg').innerHTML = event.responseJSON.view;
        }
        else {
            document.querySelector('#itemsListContainer').innerHTML = event.responseJSON.view;
        }
    }
    else {
        document.querySelector('#PopupMsg').innerHTML = event.responseText;
    }
}
function ChangeVendorStatus(id, status) {
    if (confirm("Are you sure you want to Chnage Vendor Status ?")) {
        $.blockUI({
            message: `<div class="d-flex justify-content-center align-items-center"><p class="mr-50 mb-0">Please Wait...</p> <div class="spinner-grow spinner-grow-sm text-white" role="status"></div> </div>`,
            css: {
                backgroundColor: 'transparent',
                color: '#fff',
                border: '0'
            },
            overlayCSS: {
                opacity: 0.5
            },
            onBlock: function () {
                sendPostRequest(`Vendors/ChangeStatus?vendorID=${id}&status=${status}`, null, function (res) {
                    document.querySelector('#err-msg').innerHTML = res.view;
                    showAfterActionResult();
                    $.unblockUI();
                });
            },
        });
    }

}

function ExportVendorsToExcelV1() {
    // Disable the button and change its text while exporting
    const button = document.querySelector('#VendorsExcel');
    button.innerHTML = "loading";
    button.disabled = true;

    // Send an AJAX request to the server
    fetch(url + `Vendors/ExportToExecl`, {
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
            a.download = `ExportVendors_${currentDate}.xlsx`; // File name with current date
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

function ExportVendorsToExcelV2() {
    // Disable the button and change its text while exporting
    const button = document.querySelector('#VendorsExcel');
    button.innerHTML = "loading";
    button.disabled = true;
    // Send an AJAX request to the server
    fetch(url + `Vendors/ExportToExecl`, {
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
            a.download = `ExportVendors_${currentDate}.xlsx`; // File name with current date
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

function ExportVendorsToExcel() {
    // Disable the button and change its text while exporting
    const button = document.querySelector('#VendorsExcel');
    button.innerHTML = "loading";
    button.disabled = true;
    // Send an AJAX request to the server
    fetch(url + `Vendors/ExportToExeclByEntity`, {
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
            a.download = `ExportVendors_${currentDate}.xlsx`; // File name with current date
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