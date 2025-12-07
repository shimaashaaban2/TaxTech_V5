 function sendingFile(DocumentId, PhoneNumber) {

        $.ajax({
            url: '/SendingFile/GenerateInvoice',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                DocumentId: DocumentId,
                PhoneNumber: PhoneNumber
            }),
            success: function (data) {
                
                if (data.success) {
                    toastr.success(data.data?.Message);
                } else {
                    toastr.warning("Failed: " + data.data?.message);
                }
            },
            error: function (xhr, status, error) {
                toastr.error("Server error: " + xhr.responseText);
            }
        });

    }





