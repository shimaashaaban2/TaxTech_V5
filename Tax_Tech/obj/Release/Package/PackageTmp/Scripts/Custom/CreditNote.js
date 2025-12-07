

function getAllCreditNotes(pageNo, pageSize) {
    sendGetRequest(`/CreditNote/List?pageNo=${pageNo}&pageSize=${pageSize}`, function (e) {
        document.querySelector('#list').innerHTML = e.view;
        //document.querySelector('#exportType').innerHTML = e.exportType;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
    });
}

function getCreditNotes() {
    sendGetRequest(`/CreditNote/List`, function (e) {
        document.querySelector('#list').innerHTML = e.view;
        //document.querySelector('#exportType').innerHTML = e.exportType;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
    });
}

function getInvoicesOf(status) {
    sendGetRequest(`/Invoice/List?status=${status}`, (e) => {
        document.querySelector('#list').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 100 });
        //To Show Tooltip in List
        $("[rel=tooltip]").tooltip({ html: true });
    });
}

function onFilterComplete(event) {
    HideLoading(1);
    if (event.responseJSON) {
        if (event.responseJSON.msg === 'fail') {
            document.querySelector('#PopupMsg').innerHTML = event.responseJSON.view;
        }
        else {
            document.querySelector('#list').innerHTML = event.responseJSON.view;
            document.querySelector('#exportType').innerHTML = event.responseJSON.exportType;
            $("#default").modal('hide');
            $('#thebasic-datatable').DataTable({ "pageLength": 10 });
        }
    }
    else {
        document.querySelector('#PopupMsg').innerHTML = event.responseText;
    }
}

const input = document.getElementById('autocomplete-input');
input.addEventListener('input', function () {
    const userInput = input.value;

    if (userInput.length >= 3) {
        fetch(`/Invoice/GetVendorsList?VendorName=${userInput}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ input: userInput })
        })
            .then(response => response.json())
            .then(data => {
                data = JSON.parse(data);
                var a = document.createElement("div");
                a.setAttribute("class", "autocomplete-items");
                this.parentNode.appendChild(a);
                for (i = 0; i < data.length; i++) {
                    var b = document.createElement("div");
                    //b.innerHTML = "<strong>" + data[i].substr(0, userInput.length) + "</strong>";
                    b.innerHTML += data[i].substr(userInput.length);
                    b.innerHTML += "<input type='hidden' value='" + data[i] + "'>";
                    b.addEventListener("click", function (e) {
                        input.value = this.getElementsByTagName("input")[0].value;
                        closeAllLists();
                    });
                    a.appendChild(b);
                };
            })
            .catch(error => console.error('Error fetching autocomplete suggestions:', error));
    }
    else {
        input.innerHTML = '';
    }
});
document.addEventListener("click", function (e) {
    closeAllLists(e.target);
});
function closeAllLists(elmnt) {
    var x = document.getElementsByClassName("autocomplete-items");
    for (var i = 0; i < x.length; i++) {
        if (elmnt != x[i] && elmnt != input) {
            x[i].parentNode.removeChild(x[i]);
        }
    }
}