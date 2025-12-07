function createVendor() {
    ShowLoading(1);
    // Collect input values from your form
    var vendor = {
        VendorAPIType: $("#VendorTypeID").val(),
        PersonalID: $("#PersonalID").val(),
        VendorName: $("#VendorName").val(),
        CountryID: $("#CountryID").val(),
        CountryName: $("#CountryName").val(),
        IsActive: $("#IsActive").val(),
        Governate: $("#Governate").val(),
        RegionCity: $("#RegionCity").val(),
        Street: $("#Street").val(),
        BuildingNumber: $("#BuildingNumber").val(),
        PostalCode: $("#PostalCode").val(),
        Floor: $("#Floor").val(),
        Flat: $("#Flat").val(),
        Landmark: $("#Landmark").val(),
        AdditionalInformation: $("#AdditionalInformation").val(),
        PhoneNumber: $("#PhoneNumber").val(),
        ERPInternalID: $("#ERPInternalID").val(),
        EntityID: $("#EntityID").val(),
        ActionBy: $("#ActionBy").val(),
        VendorID: $("#VendorID").val(),
        EntityTitle: $("#EntityTitle").val(),
        IsTaxExempted: $("#IsTaxExempted").val(),
        TaxExmptedCount: $("#TaxExmptedCount").val(),
        TotalCount: $("#TotalCount").val()
        // Add more fields if your VendorViewModel contains others
    };

    $.ajax({
        url: '/VendorsV2/CreateVendorV2', // ✅ Adjust to your actual controller name if different
        type: 'POST',
        data: vendor,
        success: function (response) {

            if (response.success) {
                alert("✅ Vendor created successfully!");
                console.log("Response:", response.data);

                // Optionally clear form
                $("#vendorForm")[0].reset();
            } else {
                alert("⚠️ Failed to create vendor.");
                if (response.view) {
                    $("#resultContainer").html(response.view);
                }
            }
        },
        error: function (xhr, status, error) {
            console.error("❌ Error:", error);
            alert("An error occurred while creating the vendor.");
        }
    });
}
