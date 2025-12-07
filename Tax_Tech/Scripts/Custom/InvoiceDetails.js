function getPaymentDetails() {
    document.querySelector('#paymentDetailsContainer').innerHTML = `
        <table>
            <tbody>
                <tr>
                    <td class="pr-1">Total Due:</td>
                    <td><strong>$12,110.55</strong></td>
                </tr>
                <tr>
                    <td class="pr-1">Bank name:</td>
                    <td>American Bank</td>
                </tr>
                <tr>
                    <td class="pr-1">Country:</td>
                    <td>United States</td>
                </tr>
                <tr>
                    <td class="pr-1">IBAN:</td>
                    <td>ETD95476213874685</td>
                </tr>
                <tr>
                    <td class="pr-1">SWIFT code:</td>
                    <td>BR91905</td>
                </tr>
            </tbody>
        </table>
    `;
}



function getInvoiceLineList(id) {

    $.ajax({
        type: "POST",
        url: url + '/InvoiceLine/GetInvoiceLineList',
        data: { id: id },
        success: function (data) {
            //alert(data);
            $("#list").html(data);
        }
    });

}


//function getItemById(event) {
//    const inItemId = event.target;

//    if (event.keyCode == 13) {
//        if (inItemId.value) {
//            inItemId.disabled = true;
//            document.querySelector('#myModalLabel1').innerHTML = "Add Item to Invoice - Loading...";
//            sendGetRequest(`/Invoice/GetItemById?itemId=${inItemId.value}`, (e) => {
//                if (e.target.readyState == 4 && e.target.status == 200) {
//                    document.querySelector('#addItemModalItemsContainer').innerHTML = e.target.response;
//                }
//                inItemId.disabled = false;
//                document.querySelector('#myModalLabel1').innerHTML = "Add Item to Invoice";
//            });
//        }
//    }
//}

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

function calculateTotalPrice(event) {

}

//function getItemsByName(event) {
//    let itemName = event.target.value;
//    if (itemName) {
//        if (event.keyCode === 13) {
//            $.ajax({
//                url: url + `/InvoiceLine/GetItemByName?name=${itemName}`,
//                error: function (e) {
//                    if (e.message) {
//                        alert(e.message);
//                    }
//                    else {
//                        alert("Cannot connect to the server");
//                    }
//                },
//                success: function (e) {
//                    document.querySelector("#autoComplete").innerHTML = '';
//                    document.querySelector("#autoComplete").style.display = 'block';
//                    if (e.values.length == 0) {
//                        document.querySelector("#autoComplete").innerHTML += `
//                            <li style="margin: 10px;cursor: pointer;">No Items Found</li>
//                        `;
//                    }
//                    [...e.values].forEach(ele => {
//                        document.querySelector("#autoComplete").innerHTML += `
//                            <li class="auto-complete-item" style="margin-left: -20px;padding: 10px;cursor: pointer;" onclick="chooseItemName(event)">${ele}</li>
//                        `;
//                    });
//                }
//            });
//        }
//    }
//}

function chooseItemName(event) {
    document.querySelector('#ItemName').value = event.target.innerText;
}

function showExchangeRate(event) {
    //if (event.target.selectedOptions[0].value == 1) {
    //    $('#ExchangeRate').val('1');
    //    $('#ExchangeRate').prop("disabled", true);
    //}
    //else {

    //    $('#ExchangeRate').prop("disabled", true);
    //}
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
        //emptyAllCreateInvoiceFields();
        $('.select2').each(function () {
            var $this = $(this);
            $this.wrap('<div class="position-relative"></div>');
            $this.select2({
                // the following code is used to disable x-scrollbar when click in select input and
                // take 100% width in responsive also
                dropdownAutoWidth: true,
                width: '100%',
                dropdownParent: $this.parent()
            });
        });
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