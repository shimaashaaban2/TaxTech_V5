window.addEventListener('load', function () {
    getAllAccountsByFilter();
    getAllAccounts();

   // $('#thebasic-datatable').DataTable({ "pageLength": 50 });     
});

function getAllAccounts() {

    $.ajax({
        type: "POST",
        url: url + '/Configuration/Accounts/AccountsList',
        data: {},
        success: function (data) { 
            $("#list").html(data);
            $('#thebasic-datatable').DataTable({ "pageLength": 50 });
        }
    });
 
}

function getAllAccountsByFilter() {
    $("#UserFilterSelect").change(function () {
        OpID = $("#UserFilterSelect").val();
         
        $.ajax({
            type: "GET",
            url: url + `/Configuration/Accounts/UsersFilter?OptionID=${OpID}`,
            data: {},
            success: function (data) {
                $("#list").html(data);
                $('#thebasic-datatable').DataTable({ "pageLength": 50 });
            }
        });
    });

}




  //$(document).ready(function () {
  //      $("select#selection").change(function () {
  //          let id = $(this).children("option:selected").val();
  //          console.log(id);
  //          $.ajax({
  //              type: "GET",
  //              url: url + `/Configuration/Accounts/UsersFilter?OptionID=${id}`,
  //              data: {},
  //              success: function (data) {
  //                  $("#list").html(data);
  //                  $('#thebasic-datatable').DataTable({ "pageLength": 50 });
  //              }
  //          });
  //      });
       
  //  });


function ReadURL(input, Prev) {


    if (input.files && input.files[0]) {

        var reader = new FileReader();
        reader.onload = function (e) {
            $('#' + Prev).attr('src', e.target.result); 
        }

        reader.readAsDataURL(input.files[0]);

    }
}
function EditAccount() {
    ShowLoading(1)
    var form = new FormData();

    var GoToUpload = true;
    form.append("UserID", $("#UserID").val());
    form.append("DisplayName", $("#DisplayName").val());
    form.append("UserMail", $("#UserMail").val());
    form.append("RoleID", $("#RoleID").val());
   

    var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp', 'tiff'];
    var ProfileImg = document.getElementsByName('ProfileImage')[0]; 

     
    if (ProfileImg.files.length > 0) {
        if ($.inArray(ProfileImg.files[0].name.split('.').pop().toLowerCase(), fileExtension) == -1) {
            alert('Only jpeg, jpg, png, gif, bmp, tiff  Allowed');
            HideProgress(5);
            GoToUpload = false;
        }
        else {
            form.append("ProfileImage", ProfileImg.files[0]);
        }
    }



    if (GoToUpload) {
        $.ajax({
            type: "POST",
            url: url + "/Configuration/Accounts/Edit",
            data: form,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.msg == "fail") {
                    $('#PopupMsg').html(data.view);
                    HideLoading(1);

                }
                else {
                    $('#list').html(data.view);
                    DismissPopup();
                }

            },
            complete: function (event) {
                
            }

        });

    }
}