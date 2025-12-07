function getChangeMyPasswordPopup(UserID) {
    //getPopup("Popup/GetChangePasswordPopup?UserID=" + UserID);
    getPopup(`/Popup/GetChangePasswordPopup?UserID=${UserID}`);
}  
// Invoice Popups
function getFilterFormPopup() {
    getPopup(`/Popup/GetInvoiceFilterPopup`);
}
function getCancelInvoicePopup(JobID,DocID) {
    getPopup(`/Popup/GetCancelInvoicePopup?JobID=${JobID}&DocID=${DocID}`);
}
function getRejectDocumentPopup() {
    getPopup(`/Popup/GetRejectDocumentPopup`);
}

function getDocActionLog(id) {
    getPopup(`/Popup/GetInvoiceActionLog?docID=${id}`);
}

function getCreateInvoiceHead() {
    getPopup(`/Popup/GetCreateInvoiceHeadPopup`);
}
function GetCreateInvoiceLinePopup(id) {
    getPopup(`/Popup/GetCreateInvoiceLinePopup?id=${id}`);
}
 
function getRejectInvoicePopup(docID) {
    getPopup(`/Popup/GetRejectInvoicePopup?docID=${docID}`);
}

function getEditInvoiceLinePopup(lineID, itemId) {
    getPopup(`/Popup/GetEditInvoiceLinePopup?invoiceLineId=${lineID}&itemId=${itemId}`);
}

function getAddExtraDiscountPopup(docId) {
    getPopup(`/Popup/GetAddExtraDiscountPopup?docID=${docId}`);
}

function getGetInvoiceChangeStatusPopup(docId) {
    getPopup(`/Popup/GetInvoiceChangeStatusPopup?docID=${docId}`);
}

function getDocStatusPopup(uuid) {
    getPopup(`/Popup/GetDocumentStatus?uuid=${uuid}`);
}

function getCancelDocReasonPopup(invoiceId) {
    getPopup(`/Popup/GetCancelDocReasonPopup?invoiceId=${invoiceId}`);
}

function getCreateDebitHead() {
    getPopup(`/Popup/GetCreateDebitHeadPopup`);
}

function getCreateCreditHead() {
    getPopup(`/Popup/GetCreateCreditHeadPopup`);
}

function getCreateDebitLine(id) {
    getPopup(`/Popup/GetCreateDebitLinePopup?id=${id}`);
}

function getCreateCreditLine(id) {
    getPopup(`/Popup/GetCreateCreditLinePopup?id=${id}`);
}

function getEditCreditLinePopup(lineId, itemId) {
    getPopup(`/Popup/GetEditCreditLinePopup?id=${lineId}&itemID=${itemId}`);
}

function getEditDebitLinePopup(lineId, itemId) {
    getPopup(`/Popup/GetEditDebitLinePopup?id=${lineId}&itemID=${itemId}`);
}

function getCreditNotLinkedInvoicesPopup(creditID) {
    getPopup(`/Popup/GetLinkedInvoicesPopup?creditNoteID=${creditID}`);
}

function getDebitNoteLinkedInvoicesPopup(debitID) {
    getPopup(`/Popup/GetLinkedInvoicesDebitPopup?debitNoteID=${debitID}`);
}

function getFilterRecievedDocsPopup() {
    getPopup(`/Popup/GetRecievedDocFilterPopup`);
}

function getCreateVendorPopup() {
    $("#default").modal('hide');
    document.querySelector("#popupDev").innerHTML = '';
    getPopup(`/ConfigPopup/GetCreateVendorPopup`);
}

function getRejectBulkInvoicePopup() {
    getPopup(`/Popup/GetRejectBulkInvoicePopup`);
}

function getFilterDocByInternalIDPopup(docType) {
    getPopup(`/Popup/GetFilterByInternalIDPopup?docType=${docType}`);
}

function getGetInvoiceInternalRejectPopup(DocID) {
    getPopup(`/Popup/GetInvoiceInternalRejectPopup?DocID=${DocID}`);
}

function getGetInvoiceMOFRejectPopup(DocID) {
    getPopup(`/Popup/GetInvoiceMOFRejectPopup?DocID=${DocID}`);
}

function getGetInvoiceTrackSubmissionPopup(DocID) {
    getPopup(`/Popup/GetInvoiceTrackSubmissionPopup?DocID=${DocID}`);
}

// vendors
function getVendorsExemptedTaxes(vendorID) {
    getPopup(`Popup/GetVendorsExemptedTaxPopup?vendorID=${vendorID}`, function () {
        getAllIExemptedTaxesByVendorId(vendorID);
    });
}

function getCreateVendorPopup() {
    getPopup(`/Popup/GetCreateVendorPopup`);
}

function getUpdateVendorPopup(vendorID) {
    getPopup(`/Popup/GetUpdateVendorPopup?vendorID=${vendorID}`);
}

// items
function getCreateItemPopup() {
    getPopup(`/Popup/GetCreateItemPopup`);
}

function getUpdateItemPopup(id) {
    getPopup(`/Popup/GetItemsUpdatePopup?itemId=${id}`);
}

function getItemTaxesPopup(id) {
    getPopup(`/Popup/GetItemTaxesPopup?itemId=${id}`);
}
function getTaxesPopup(jobId, logType) {
    getPopup(`/Popup/GetJobsTrackerStatistics?jobId=${jobId}&logType=${logType}`, function () {
        $('#thebasic-datatable').DataTable({ "pageLength": 20 });
    });
}
function getMOFStatus(JobID, status) {
    getPopup(`/Popup/DocByMOFStatus?JobID=${JobID}&status=${status}`, function () {
        $('#thebasic-datatable').DataTable({ "pageLength": 20 });
    });
}
function getCreateEReceiptPopup() {
    getPopup(`/Popup/GetCreateEReceiptPopup`);
}
function getReboundPopup() {
    getPopup(`/Popup/GetReboundPopup`);
}
function getReceiptFilterPopup() {
    getPopup(`/Popup/GetReceiptFilterPopup`);
}
function GetEReceiptDetailsPopup(ID) {
    
    getPopup(`/Popup/GetEReceiptDetails?ID=${ID}`);
} 

function GetEReceiptQRPopup(EReceiptID) {

    getPopup(`/Popup/GetEReceiptQR?EReceiptID=${EReceiptID}`);
}
function EReceiptNumberDetails() {
    getPopup(`/Popup/EReceiptNumberDetails`);
}

function GetEReceiptToCancelPopup(LogID,EReceiptID) {
  //  alert(1);

    getPopup(`/Popup/GetReceiptLinesToCancel?LogID=${LogID}&EReceiptID=${EReceiptID}`);
}


function GetJobStopPopup() {
    getPopup(`/Popup/JobStop`);
}
function GetJobResumePopup() {
    getPopup(`/Popup/JobResume`);
 
}
function GetJobPriorityPopup() {
    getPopup(`/Popup/JobPriority`);
}
function GetJobResubmitPopup() {
    getPopup(`/Popup/JobResubmit`);
}