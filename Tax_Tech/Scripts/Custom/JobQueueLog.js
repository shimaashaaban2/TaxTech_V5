

function getJobQueueLogDetails(jobId) {
    sendGetRequest(`JobQueue/JobQueueLogSummaryList?jobId=${jobId}`, function (res) {
        document.querySelector('#list').innerHTML = res;
    });
}