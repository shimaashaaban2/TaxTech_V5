

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

function showCurrencyAmount(event) {
	if (event.target.selectedOptions[0].value == 'EGP') {
		console.log(event.target.selectedOptions[0].value);
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