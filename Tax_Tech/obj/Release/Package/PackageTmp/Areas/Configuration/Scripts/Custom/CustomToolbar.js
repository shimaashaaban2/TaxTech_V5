function ShowFilterForm(div, text,id) {
    $.ajax({
        type: "POST",
        url: url + "CustomToolbar/ShowFilterForm", // Controller/View
        data: { formId:id},
        success: function (data) {
            $("#frm").html('');   
            $('#list').html('');
            $("#frm").html(data);      
            ShowToFilterDiv(div, text);
            console.log("Filter worked successfully");
        }
    });
}


function ShowToFilterDiv(div, text) {
    $("#" + div).removeClass("hidden");
    $("#filterTxt").text(text);

}

 