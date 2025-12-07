


function getCloseJobV2(SessionID, JobStatus, Createdby) {
    var jobQueue = {
        SessionId: SessionID,
        JobStatus: JobStatus,
        CreatedBy: Createdby
    };

    $.ajax({
        url: '/JobQueueV2/ChangeJobStatus',
        type: 'POST',
        data: JSON.stringify(jobQueue),
        contentType: 'application/json; charset=utf-8',

        beforeSend: function () {
            // ✅ Show BlockUI before sending the request
            $.blockUI({
                message: '<div class="d-flex justify-content-center align-items-center"><p class="mr-50 mb-0">Please Wait...</p> <div class="spinner-grow spinner-grow-sm text-white" role="status"></div> </div>',
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: 0.6,
                    color: '#fff'
                }
            });
        },

        success: function (data) {
            console.log(data);
            console.log();
            if (data.success) {
                $("#headingCollapse-" + SessionID)
                    .closest(".card")
                    .fadeOut(500, function () {
                        $(this).remove();
                    });

                toastr.success(data.data);
            } else {
                toastr.warning("Failed: " + data.data);
            }
        },

        error: function (xhr, status, error) {
            console.error("❌ Response Headers:", xhr.getAllResponseHeaders());
            toastr.error("Server error: " + xhr.responseText);

        },

        complete: function () {
            // ✅ Always unblock UI (success or error)
            $.unblockUI();
        }
    });
}
