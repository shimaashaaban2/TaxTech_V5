
function GetInvoiceLines(id) {
    sendGetRequest(`/Invoice/GetInvoiceLines?invoiceId=${id}`, (e) => {
        document.querySelector('#invoiceLinesContainer').innerHTML = e;
        $("[rel=tooltip]").tooltip({ html: true });
    });
}

function getVendorsById(event) {
    sendGetRequest(`/Invoice/GetVendorById?vendorId=${event.target.selectedOptions[0].value}`, (e) => {
        if (e.msg == 'success') {
            document.querySelector('#vendorContainerData').innerHTML = `
                <p class="card-text mb-25">${e.data.BuildingNumber || "-"} ${e.data.Street || "-"}, ${e.data.RegionCity || "-"}</p>
                <p class="card-text mb-25">${e.data.CountryName || "-"} ${e.data.PostalCode || "-"}</p>
                <p class="card-text mb-25">Reg. Info: ${e.data.VendorRegInfo || "-"}</p>
                <p class="card-text mb-0">Vendor type: ${e.data.VendorTypeList || "-"}</p>
            `;
        }
        else {
            // in case of error
            document.querySelector('#vendorContainerData').innerHTML = e;
        }
    });
}

function getPaymentsById(event) {
    sendGetRequest(`/Invoice/GetPaymentById?paymentID=${event.target.selectedOptions[0].value}`, (e) => {
            if (e.msg == 'success') {
                document.querySelector('#paymentDataContainer').innerHTML = `
                    <tr>
                        <td class="pr-1">Bank name:</td>
                        <td>${e.data.BankName || "-"}</td>
                    </tr>
                    <tr>
                        <td class="pr-1">IBAN:</td>
                        <td>${e.data.BankAccountIBAN || "-"}</td>
                    </tr>
                    <tr>
                        <td class="pr-1">SWIFT code:</td>
                        <td>${e.data.SwiftCode || "-"}</td>
                    </tr>
                `;
            }
            else {
                // in case of error
                document.querySelector('#paymentDataContainer').innerHTML = e;
            }
    })
}