$(() => {
    "use strict";
    $('select').selectize();

    $('#LoanParticularsInformation_ExistingChecker').on('change', function (e) {
        e.preventDefault();
        if ($(this).prop('checked')) {
            $('[name="LoanParticularsInformation.ExistingHousingApplicationNumber"]').prop('disabled', false);
        } else {
            $('[name="LoanParticularsInformation.ExistingHousingApplicationNumber"]').val('');
            $('[name="LoanParticularsInformation.ExistingHousingApplicationNumber"]').prop('disabled', true);
        }
    })
    $('[name="CollateralInformation.ExistingReasonChecker"]').on('change', function (e) {
        e.preventDefault();
        if ($(this).prop('checked')) {
            $('[name="CollateralInformation.CollateralReason"]').prop('disabled', false);
        } else {
            $('[name="CollateralInformation.CollateralReason"]').val('');
            $('[name="CollateralInformation.CollateralReason"]').prop('disabled', true);
        }
    })
})