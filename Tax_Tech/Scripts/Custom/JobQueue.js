window.addEventListener('load', function () {
    getAllJobs();
});

function getAllJobs() {
    sendGetRequest(`/JobQueue/List`, (e) => {
        document.querySelector('#JobQueueContainer').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 50, "order": [[0, 'dasc']] });
    });
    //id: JobQueueContainer
}

function filterBy(event, filterType) {
    const dropDown = document.querySelector('#filterDropDown');
    dropDown.innerText = event.target.innerText;
    switch (filterType) {
        case '1':
            // jobtype
            sendGetRequest(`/JobQueue/GetJobTypes`, function (e) {
                const container = document.querySelector('#filterContainer');
                container.style.display = 'block';
                container.innerHTML = e;
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
            break;
        case '2':
            // status
            break;
    }
}

function filterJobsByJobType(jobType) {
    sendGetRequest(`/JobQueue/FilterByJobType?jobType=${jobType}`, function (e) {
        document.querySelector('#JobQueueContainer').innerHTML = e;
        $('#thebasic-datatable').DataTable({ "pageLength": 50 });
    });
}