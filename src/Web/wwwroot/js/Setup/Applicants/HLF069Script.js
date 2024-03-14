const applicantInfoIdVal = $(`[name='ApplicantsPersonalInformationModel.Id']`).val();
$(function () {
    $(".selectize").selectize();
    $('.calendarpicker').flatpickr();

    //#region Loan Particulars

    var purposeOfLoanDropdown, $purposeOfLoanDropdown;
    var modeofPaymentDropdown, $modeofPaymentDropdown;

    var purposeofloanVal = $(`[name='LoanParticularsInformationModel.PurposeOfLoanId']`).attr('data-value');
    var modeofpaymentVal = $(`[name='LoanParticularsInformationModel.ModeOfPaymentId']`).attr('data-value');

    $purposeOfLoanDropdown = $(`[name='LoanParticularsInformationModel.PurposeOfLoanId']`).selectize({
        valueField: 'Id',
        labelField: 'Description',
        searchField: 'Description',
        preload: true,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + 'Applicants/GetPurposeOfLoan',
                success: function (results) {
                    try {
                        callback(results);
                    } catch (e) {
                        callback();
                    }
                },
                error: function () {
                    callback();
                }
            });
        },

        render: {
            item: function (item, escape) {
                return ("<div>" +
                    escape(item.Description) +
                    "</div>"
                );
            },
            option: function (item, escape) {
                return ("<div class='py-1 px-2'>" +
                    escape(item.Description) +
                    "</div>"
                );
            }
        },
    });

    purposeOfLoanDropdown = $purposeOfLoanDropdown[0].selectize;

    purposeOfLoanDropdown.on('load', function (options) {
        purposeOfLoanDropdown.setValue(purposeofloanVal || '');
        //resourceCounter("purposeofloan");
        purposeOfLoanDropdown.off('load');
    });

    $modeofPaymentDropdown = $(`[name='LoanParticularsInformationModel.ModeOfPaymentId']`).selectize({
        valueField: 'Id',
        labelField: 'Description',
        searchField: 'Description',
        preload: true,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + 'Applicants/GetModeOfPayment',
                success: function (results) {
                    try {
                        callback(results);
                    } catch (e) {
                        callback();
                    }
                },
                error: function () {
                    callback();
                }
            });
        },

        render: {
            item: function (item, escape) {
                return ("<div>" +
                    escape(item.Description) +
                    "</div>"
                );
            },
            option: function (item, escape) {
                return ("<div class='py-1 px-2'>" +
                    escape(item.Description) +
                    "</div>"
                );
            }
        },
    });

    modeofPaymentDropdown = $modeofPaymentDropdown[0].selectize;

    modeofPaymentDropdown.on('load', function (options) {
        modeofPaymentDropdown.setValue(modeofpaymentVal || '');
        //resourceCounter("modeofpayment");
        modeofPaymentDropdown.off('load');
    });

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

    //#endregion

    //#region Collateral
    var propertyTypeVal = $(`[name='CollateralInformationModel.PropertyTypeId']`).attr('data-value');
    var propertyTypeDropdown, $propertyTypeDropdown;

    $propertyTypeDropdown = $(`[name='CollateralInformationModel.PropertyTypeId']`).selectize({
        valueField: 'Id',
        labelField: 'Description',
        searchField: 'Description',
        preload: true,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + 'Applicants/GetPropertyType',
                success: function (results) {
                    try {
                        callback(results);
                    } catch (e) {
                        callback();
                    }
                },
                error: function () {
                    callback();
                }
            });
        },

        render: {
            item: function (item, escape) {
                return ("<div>" +
                    escape(item.Description) +
                    "</div>"
                );
            },
            option: function (item, escape) {
                return ("<div class='py-1 px-2'>" +
                    escape(item.Description) +
                    "</div>"
                );
            }
        },
    });

    propertyTypeDropdown = $propertyTypeDropdown[0].selectize;

    propertyTypeDropdown.on('load', function (options) {
        propertyTypeDropdown.setValue(propertyTypeVal || '');
        //resourceCounter("propertyType");
        propertyTypeDropdown.off('load');
    });

    //#endregion

    var form1 = $("#loanparticulars");
    var form2 = $("#collateraldata");

    $('#rootwizard').bootstrapWizard({
        'onNext': function (tab, navigation, index, e) {
            var currentForm = $($(tab).data("targetForm"));
            var currentFormName = currentForm.attr("id");

            if (currentForm) {
                if (currentFormName == "loanparticulars") {
                    form1.validate();
                    currentForm.addClass('was-validated');

                    if (!form1.valid()) {
                        e.preventDefault();
                        e.stopPropagation();
                        return false;
                    }
                } else if (currentFormName == "collateraldata") {
                    form2.validate();
                    currentForm.addClass('was-validated');

                    if (!form2.valid()) {
                        e.preventDefault();
                        e.stopPropagation();
                        return false;
                    }
                }
            }
        }
    });

    $('.submit-form').on('click', (e) => {
        e.preventDefault();
        $('#form2').trigger('submit'); // Submit the form if the user clicks OK
    })

    $('#form2').on('submit', async function (e) {
        e.preventDefault();
        let $loanparticulars = $('#loanparticulars');
        let $collateraldata = $('#collateraldata');
        let $spousedata = $('#spousedata');
        if (!$(this).valid() || !$loanparticulars.valid() || !$collateraldata.valid() || !$spousedata.valid()) {
            return;
        }
        let loanparticulars = $loanparticulars.serializeArray();
        let collateraldata = $collateraldata.serializeArray();
        let spousedata = $spousedata.serializeArray();
        let form2 = $(this).serializeArray();
        let combinedData = {};
        $(loanparticulars.concat(collateraldata, spousedata, form2)).each(function (index, obj) {
            combinedData[obj.name] = obj.value;
        });

        // Use await before the AJAX call
        try {
            await $.ajax({
                type: 'POST',
                url: '/Applicants/SaveHLF068',
                data: combinedData,
                beforeSend: function () {
                    loading("Saving Changes...");
                },
                success: async function (response) {
                    messageBox("Successfully", "success", true);

                    if (applicantInfoIdVal == 0) {
                        loader.close();
                        setTimeout(function () {
                            window.location.href = "/Applicants/HLF068/" + response;
                        }, 2000);
                    }
                    else {
                        loader.close();

                        setTimeout(function () {
                            // Redirect to the specified location
                            window.location.href = "/Applicants/ApplicantRequests";
                        }, 2000); // 2000 milliseconds = 2 seconds
                    }
                },
                error: async function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText);
                    messageBox(jqXHR.responseText, "danger", true);
                    loader.close();
                }
            });
        } catch (error) {
            // Handle any errors from the AJAX request
            console.log(error);
            // Optionally display an error message
            messageBox("An error occurred during the submission.", "danger", true);
        }
    });

    $(document).ready(function () {
        loadloanParticularInformation(applicantInfoIdVal);
        loadSpouseInformation(applicantInfoIdVal);
        loadBorrowerInformation(applicantInfoIdVal);
        loadCollateralInformation(applicantInfoIdVal);
        loadForm2PageInformation(applicantInfoIdVal);
    });

    function loadloanParticularInformation(id) {
        $.ajax({
            url: baseUrl + "Applicants/GetLoanParticularsByApplicantInfoData/" + id,
            method: 'Get',
            success: function (response) {

                $(`select[name='LoanParticularsInformationModel.PurposeOfLoanId']`).data('selectize').setValue(response.PurposeOfLoanId);

                purposeOfLoanDropdown.setValue(response.PurposeOfLoanId);

                $(`[name='LoanParticularsInformationModel.ExistingHousingApplicationNumber']`).val(response.ExistingHousingApplicationNumber);
                $(`[name='LoanParticularsInformationModel.ExistingChecker']`).prop("checked", response.ExistingChecker);
                $(`[name='LoanParticularsInformationModel.DesiredLoanAmount']`).val(response.DesiredLoanAmount);
                $(`[name='LoanParticularsInformationModel.DesiredLoanTermYears']`).val(response.DesiredLoanTermYears);
                $(`[name='LoanParticularsInformationModel.RepricingPeriod']`).val(response.RepricingPeriod);

                modeofPaymentDropdown.setValue(response.ModeOfPaymentId);

                //CollateralInformationModel.Province
                //CollateralInformationModel.Municipality
                //CollateralInformationModel.Street
                //CollateralInformationModel.DeveloperName
                //CollateralInformationModel.PropertyTypeId
                //CollateralInformationModel.TctOctCctNumber
                //CollateralInformationModel.TaxDeclrationNumber
                //CollateralInformationModel.LotUnitNumber
                //CollateralInformationModel.BlockBuildingNumber
                //CollateralInformationModel.IsMortgage
                //CollateralInformationModel.LandArea
                //CollateralInformationModel.HouseAge
                //CollateralInformationModel.ExistingReasonChecker
                //CollateralInformationModel.CollateralReason
                //CollateralInformationModel.CollateralReason
                //CollateralInformationModel.NumberOfStoreys
                //CollateralInformationModel.ProposedNoOfStoreys
                //CollateralInformationModel.ExistingTotalFloorArea
                //CollateralInformationModel.ProposedTotalFloorArea
            },
            error: function () {
            }
        });
    }

    function loadSpouseInformation(id) {
        $.ajax({
            url: baseUrl + "Applicants/GetSpouseByApplicantInfoData/" + id,
            method: 'Get',
            success: function (response) {

            },
            error: function () {
            }
        });
    }

    function loadBorrowerInformation(applicantId) {
        $.ajax({
            url: baseUrl + "Applicants/GetBarrowerByApplicantInfoData/" + applicantId,
            method: 'Get',
            success: function () {
            },
            error: function () {
            }
        });
    }

    function loadForm2PageInformation(applicantId) {
        $.ajax({
            url: baseUrl + "Applicants/GetForm2ByApplicantInfoData/" + applicantId,
            method: 'Get',
            success: function () {
            },
            error: function () {
            }
        });
    }

    function loadCollateralInformation(applicantId) {
        $.ajax({
            url: baseUrl + "Applicants/GetCollateralByApplicantInfoData/" + applicantId,
            method: 'Get',
            success: function () {
            },
            error: function () {
            }
        });
    }
});