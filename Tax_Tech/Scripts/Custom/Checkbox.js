function UnSelectOption(CID, id) {
    let CheckVal = document.getElementById(CID).checked;
    if (CheckVal) {
        document.getElementById(id).selectedIndex = 0;
    }
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
}

function UncheckBox(CID) {
    let inputs = document.getElementById(CID);
    inputs.checked = false;
}