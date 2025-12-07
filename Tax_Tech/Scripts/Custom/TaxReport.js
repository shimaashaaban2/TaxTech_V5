function OnGetTaxTable(event) {
    hideLoader('btn-Get', 'btn-Get-loader');
    if (event.responseJSON.msg === 'success') {
        document.querySelector('#TaxMsg').innerHTML = " ";
        document.querySelector('#List').innerHTML = event.responseJSON.view;

        $("#thebasic-datatable").DataTable({ "pageLength": 50 });
    }
    else {
        document.querySelector('#TaxerrMsg').innerHTML = event.responseJSON.view;
    }
}