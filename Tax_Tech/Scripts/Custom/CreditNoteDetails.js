function showCurrencyAmount(event) {
    if (event.target.selectedOptions[0].value == 'EGP') {
        document.querySelector("#amountEGPContainer").style.display = 'block';
        document.querySelector("#amountSoldContainer").style.display = 'none';
    }
    else {
        console.log(event.target.selectedOptions[0].value);
        document.querySelector("#amountEGPContainer").style.display = 'none';
        document.querySelector("#amountSoldContainer").style.display = 'block';
    }
}

function calculateSoldTotal() {
    let quantity = document.querySelector('#Quantity').value;
    let isPercentage = document.querySelector('#percentageDiscount').checked;
    let exchangeRate = document.querySelector('#ExchangeRate').value;
    let amount = document.querySelector('#Amount').value;
    let discount = document.querySelector('#Discount').value;
    let itemSerial = document.querySelector('#ItemID').selectedOptions[0].value;

    if (amount && quantity && exchangeRate) {
        document.querySelector('#SoldTotal').value = amount * quantity * exchangeRate;
        //if (discount) {
        if (isPercentage) {
            let discountValue = parseFloat(discount) * parseFloat(document.querySelector('#SoldTotal').value) / 100;
            document.querySelector('#ItemTotal').value = amount * quantity * exchangeRate - discountValue;
        }
        else {
            document.querySelector('#ItemTotal').value = amount * quantity * exchangeRate - discount;
        }
        //}

        // calculating tax total
        if (itemSerial) {
            sendGetRequest(`/InvoiceLine/GetItemTax?itemId=${itemSerial}&itemNet=${document.querySelector('#ItemTotal').value}`, function (e) {
                if (e.msg.toString().includes('fail')) {
                    document.querySelector('#PopupMsg').innerHTML = e.view;
                }
                else {
                    document.querySelector('#Tax').value = e.data;
                    calculateNetTotal();
                }
            });
        }
    }
}

function calculateNetTotal() {
    let ItemTotal = document.querySelector('#ItemTotal').value;
    let Tax = document.querySelector('#Tax').value;
    var NetTotal = parseFloat(ItemTotal) + parseFloat(Tax);
    if (ItemTotal && Tax) {
        $('#NetTotal').val(NetTotal);
    }
}

function calculateTotal() {
    let isPercentage = document.querySelector('#percentageDiscount').checked;
    let discount = document.querySelector('#Discount').value;
    let soldTotal = document.querySelector('#SoldTotal').value;
    let itemSerial = document.querySelector('#ItemID').selectedOptions[0].value;
    if (discount && soldTotal) {
        document.querySelector('#percentageDiscount').value = isPercentage;
        if (isPercentage) {
            let discountValue = discount * parseFloat(soldTotal) / 100;
            $('#ItemTotal').val(parseFloat(soldTotal) - discountValue);
        }
        else {
            let discountValue = parseFloat(soldTotal) - parseFloat(discount);
            $('#ItemTotal').val(discountValue);
        }

        if (itemSerial) {
            sendGetRequest(`/InvoiceLine/GetItemTax?itemId=${itemSerial}&itemNet=${document.querySelector('#ItemTotal').value}`, function (e) {
                if (e.msg.toString().includes('fail')) {
                    document.querySelector('#PopupMsg').innerHTML = e.view;
                }
                else {
                    document.querySelector('#Tax').value = e.data;
                    calculateNetTotal();
                }
            });
        }
    }
}

function emptyAllCreateInvoiceFields() {
    document.querySelector('#Discount').value = "0";
    document.querySelector('#SoldTotal').value = "0.00";
    document.querySelector('#Quantity').value = "";
    document.querySelector('#ExchangeRate').value = "1";
    document.querySelector('#Currency').value = "EGP";
    document.querySelector('#Amount').value = "";
    document.querySelector('#SoldTotal').value = "0.00";
    document.querySelector('#ItemTotal').value = "0.00";
    document.querySelector('#Tax').value = "0.00";
    document.querySelector('#NetTotal').value = "0.00";
}

function GetItemsByID(id, TargetId) {
    ItemID = $("#" + id).val();

    getDDL('InvoiceLine/GetItemListByID?id=' + ItemID + "&partialCode=" + TargetId, TargetId);
    sendGetRequest('InvoiceLine/GetItemsTaxList?id=' + ItemID, (e) => {
        $("#ItemTaxList").html('');
        $("#ItemTaxList").html(e);
       // emptyAllCreateInvoiceFields();
    });
}

function getDocTotals(docId) {
    sendGetRequest(`/InvoiceLine/GetDocTotal?docId=${docId}`, function (e) {
        if (e.view) {
            $("#invoiceTotals").html(e.view);
        }
        else {
            $("#invoiceTotals").html(e);
        }
    });
}

function getCreditNoteLines(docId) {
    sendGetRequest(`/CreditNote/CreditLinesWithActions?docId=${docId}`, function (e) {
        document.querySelector('#list').innerHTML = e;
    });
}

function createTblRow(rowData) {
    const tr = document.createElement('tr');
    const td = document.createElement('td');
    td.innerText = rowData.InternalID;
    td.style.width = '25% !important';
    const td2 = document.createElement('td');
    td2.innerText = rowData.TotalAmount;
    const td3 = document.createElement('td');
    td3.innerHTML = `<a href="javascript:void(0)" onclick='removeLinkedInvoice(event, ${rowData.DocumentID})'>X</a>`;

    tr.appendChild(td);
    tr.appendChild(td2);
    tr.appendChild(td3);
    return tr;
}

let linkedInvoices = [];

function bind(data) {
    const total = document.querySelector('#totalLinkedInvoicesAmount');
    let internalID = document.querySelector('#CreditNotInternalID');

    [...data].forEach(row => {
        if (linkedInvoices.filter(i => i.DocumentID == row.DocumentID).length == 0) {
            linkedInvoices.push(row);
            const tr = createTblRow(row);
            linkedInvoicesContainer.appendChild(tr);
            // calcuating the total
            total.innerText = parseFloat(total.innerText) + row.TotalAmount;
        }
        else {
            swal.fire({
                text: `Document of Internal ID ${internalID.value} is Already Added`,
                icon: "error"
            });
        }
    });
}

function colorTotalIfLarger() {
    try {
        const linkedInvoicesTotalEl = document.querySelector('#totalLinkedInvoicesAmount');
        const creditNoteTotalEl = document.querySelector("#CredittotalAmount");
        let linkedInvoicesTotal = parseFloat(linkedInvoicesTotalEl.innerText);
        let creditNoteTotal = parseFloat(creditNoteTotalEl.innerText);

        if (linkedInvoicesTotal > creditNoteTotal) {
            linkedInvoicesTotalEl.style.color = "red";
            return false;
        }
        else {
            linkedInvoicesTotalEl.style.color = "#6e6b7b";
            return true;
        }
    } catch (e) {

    }
}

function getDocByInternalID(event) {
    event.target.disabled = true;
    let internalID = document.querySelector('#CreditNotInternalID');

    if (!internalID.value) {
        swal.fire({
            text: "Please Enter Document Internal ID",
            icon: "error",
        });
        event.target.disabled = false;
        return;
    }

    sendPostRequest(`CreditNote/GetInvoiceInfo?internalID=${internalID.value}`, null, function (res) {
        event.target.disabled = false;
        if (res.msg == 'success') {

            if ([...res.data].length == 0) {
                document.querySelector('#PopupMsg').innerHTML = '<div class="alert alert-danger">Internal ID Not Found<button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button></div>';
                return;
            }

            const linkedInvoicesContainer = document.querySelector('#linkedInvoicesContainer');

            if (linkedInvoicesContainer.innerHTML.toString().toLowerCase().includes('no data') || linkedInvoicesContainer.innerHTML.toString().toLowerCase().includes('لا يوجد بيانات')) {
                linkedInvoicesContainer.innerHTML = '';
            }
            // binding the data to the datatable
            bind(res.data);
            document.querySelector('#PopupMsg').innerHTML = '';
            colorTotalIfLarger();
        }
        else {
            document.querySelector('#PopupMsg').innerHTML = res.view;
        }
        internalID.value = '';
    });
}

function removeLinkedInvoice(event, docId) {
    // remove the item from the list
    let objToRemove = linkedInvoices.filter(i => i.DocumentID == docId);
    let objIndex = linkedInvoices.indexOf(objToRemove[0]);
    linkedInvoices.splice(objIndex, 1);

    // remove the item from the view
    event.target.parentElement.parentElement.remove();

    // decreamenting the total amount
    const total = document.querySelector('#totalLinkedInvoicesAmount');
    total.innerText = parseFloat(total.innerText) - parseFloat(objToRemove[0].TotalAmount);
    colorTotalIfLarger();
}

function submitLinkedInvoices(event, creditNoteID) {
    event.target.disabled = true;
    let docsIDs = linkedInvoices.map(i => i.DocumentID);

    let formData = new FormData();
    formData.append('docsStr', docsIDs.join(','));
    const total = document.querySelector('#totalLinkedInvoicesAmount');
    const creditTotal = document.querySelector('#CredittotalAmount');

    if (parseFloat(total.innerText) > parseFloat(creditTotal.innerText)) {
        swal.fire({
            text: "Linked Invoices Total must be less than Document Total",
            icon: "error",
        });
        event.target.disabled = false;
        return;
    }

    sendPostRequest(`/CreditNote/UpdateLinkedInvoices?creditNoteID=${creditNoteID}&total=${total.innerText}&creditTotal=${creditTotal.innerText}`, formData, function (e) {
        document.querySelector('#PopupMsg').innerHTML = e.view;
        event.target.disabled = false;
    });
}

function getLinkedInvoices(creditNoteId) {
    sendPostRequest(`/CreditNote/GetLinkedInvoices?docId=${creditNoteId}`, null, function (e) {
        if (e.msg == 'success') {
            document.querySelector('#list2').innerHTML = e.view;
            linkedInvoices = [];
            [...e.data].forEach(i => {
                linkedInvoices.push(i);
            });
        }
        else {
            document.querySelector('#PopupMsg').innerHTML = e.view;
        }
    });
}