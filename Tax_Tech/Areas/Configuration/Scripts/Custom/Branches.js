window.addEventListener('load', function () {
    getBranches();
});

function getBranches() {
    sendGetRequest(`/Branches/List`, (e) => {
        document.querySelector('#list').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });
}

function changeStatus(id, status) {
    sendPostRequest(`/Branches/ChangeStatus?branchId=${id}&status=${status}`, null, (e) => {
        if (e.msg == 'success') {
            document.querySelector('#BranchesContainer').innerHTML = e.view;
            $('#thebasic-datatable').DataTable({ "pageLength": 50 });
        }
        else {
            document.querySelector('#main-err').innerHTML = e;
        }
    });
}
function GetRegInfo() {
    // alert($("#VendorAPIType").val())
    if ($("#BranchAPIType").val() == 3) {
        $("#PersonalID").val('0');
    }
}

function ExportBranchesToExcel() {
    const button = document.querySelector('#BranchesToExcel');
    button.innerHTML = "loading";
    button.disabled = true;

    // Send an AJAX request to the server
    fetch(url + `Branches/ExportToExecl`, {
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
            a.download = `ExportBranches_${currentDate}.xlsx`; // File name with current date
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