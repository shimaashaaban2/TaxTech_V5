function uploadFile(jobType, errMsg) {
    const el = document.querySelector('#ExeclFile');
    var TemplateID = $("#TemplateID").val();
    if (el.files.length > 0) {
        let formData = new FormData();
        formData.append('execlFile', el.files[0], el.files[0].name);
        formData.append('jobType', jobType);
        formData.append('TemplateID', TemplateID);

        sendPostRequest(`/Invoice/UploadExecl`, formData, (e) => {
            if (e.msg == 'success') {
                document.querySelector('#lastStep').innerHTML = e.view;
                let steper = new Stepper(document.querySelector(".horizontal-wizard-example"));
                steper.next();
                el.value = "";
            }
            else if (e.msg == 'errors' || e.msg == 'failed') {
                document.querySelector('#ErrMsg1').innerHTML = e.view;
            }
            HideLoading(4);
        }, 4);
    }
    else {
        Swal.fire({
            title: errMsg,
            text: '',
            icon: 'error',
            showCancelButton: false,
            confirmButtonColor: '#7367f0'
        });
    }
}

function selectTemplate(event) {
    const val = event.target.selectedOptions[0].value;
    const btn = document.querySelector('#btnDownloadTemplate');

    switch (val) {
        case "1":
            btn.href = url + 'Content/files/execl templates/Template1.xlsx';
            break;
        case "2":
            btn.href = url + 'Content/files/execl templates/Template2.xlsx';
            break;
        case "3":
            btn.href = url + 'Content/files/execl templates/Template3.xlsx';
            break;
    }
}