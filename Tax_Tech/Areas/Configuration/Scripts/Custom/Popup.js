 
function getCreateAccountPopup() {
    getPopup("Configuration/Popup/GetCreateAccountPopup");
 

}

function getEditAccountPopup(UserID ) {
    getPopup(`Configuration/Popup/GetEditAccountPopup?UserID=${UserID}` );

   
}

function getChangeMyPasswordPopup(UserID) {
    getPopup(`Configuration/Popup/GetChangeMyPasswordPopup?UserID=${UserID}`);
    
}
function getResetPasswordPopup(UserID) {
    getPopup(`Configuration/Popup/GetResetPasswordPopup?UserID=${UserID}`);

}
// Vendors Popups
function getVendorsExemptedTaxes(vendorID) {
    getPopup(`Configuration/ConfigPopup/GetVendorsExemptedTaxPopup?vendorID=${vendorID}`, function () {
        getAllIExemptedTaxesByVendorId(vendorID);
    });
}

function getUpdateVendorPopup(id) {
    getPopup(`Configuration/ConfigPopup/GetUpdateVendorPopup?vendorID=${id}`);
}

function getCreateVendorPopup() {
    getPopup(`Configuration/ConfigPopup/GetCreateVendorPopup`);
}

// Branches Popups
function getCreateBranchModal() {
    getPopup(`Configuration/ConfigPopup/GetCreateBranchPopup`);
}

function getUpdateBranchModal(id) {
    getPopup(`Configuration/ConfigPopup/GetUpdateBranchPopup?branchId=${id}`);
}

// Items Popups
function getCreateItemPopup() {
    getPopup(`Configuration/ConfigPopup/GetCreateItemPopup`);
}

function getUpdateItemPopup(id) {
    getPopup(`Configuration/ConfigPopup/GetItemsUpdatePopup?itemId=${id}`);
}

function getTaxesPopup(id) {
    getPopup(`Configuration/ConfigPopup/GetItemTaxesPopup?itemId=${id}`);
}


// Banks Popups
function getInsertBankPopup() {
    getPopup(`Configuration/ConfigPopup/GetInsertBankPopup`);
}

// Payment Popups
function getCreatePaymentModal() {
    getPopup(`Configuration/ConfigPopup/GetCreatePaymentPopup`);
}

function getUpdateModal(id) {
    getPopup(`Configuration/ConfigPopup/GetUpdatePaymentPopup?paymentId=${id}`);
}

// Mail Config Popups

function getEditMailConfigModal(id) {
    getPopup(`Configuration/ConfigPopup/GetMailConfigEditPopup?id=${id}`);
}

// Global Config Popups
function getEditGlobalConfigModal(id) {
    getPopup(`Configuration/ConfigPopup/GetGlobalConfigEditPopup?id=${id}`);
}

function getItemTaxesPopup(id) {
    getPopup(`/Popup/GetItemTaxesPopup?itemId=${id}`);
}

function getPriceAfterTaxPopup(id) {
    getPopup(`/Popup/GetPriceAfterTaxPopup?itemId=${id}`, function () {
        getTaxPriceHistory(id);
    });
}
function GetGroupItemsPopup(id) {

    getPopup(`/Popup/GetGroupItemsPopup?id=${id}`);
}