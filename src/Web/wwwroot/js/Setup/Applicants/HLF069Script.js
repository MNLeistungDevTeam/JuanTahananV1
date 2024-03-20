const applicantInfoIdVal = $(`[name='ApplicantsPersonalInformationModel.Id']`).val();
$(function () {
    $(".selectize").selectize();
    $('.calendarpicker').flatpickr();

    rebindValidators();

    $('#rootwizard').bootstrapWizard({
        onNext: function (tab, navigation, index, e) {
            console.log("Next button clicked");

            var currentForm = $($(tab).data("target-div"));
            var currentFormName = currentForm.attr("id");

            // Find the current tab pane
            var currentTabPane = $('.tab-pane').eq(index);

            // Hide the previous form (loanparticulars) and remove 'fade' class
            var prevForm = currentTabPane;
            console.log("Current form ID: " + currentFormName);

            // Add the "was-validated" class to the current form
            currentForm.addClass('was-validated');

            // Validate the current form
            var isValid = validateForm(currentForm);

            // If current form is "form2", return without proceeding to next step
            if (currentFormName == "form2") {
                return;
            }

            if (!isValid) {
                // If validation fails, prevent navigation to the next step
                return false;
            } else {
                // Hide the current form
                currentForm.addClass('fade').prop('hidden', true);

                // Show the previous form
                prevForm.removeClass('fade').prop('hidden', false);
            }
        },
        onPrevious: function (tab, navigation, index) {
            console.log("Previous button clicked");

            var currentForm = $($(tab).data("target-div"));
            var currentFormName = currentForm.attr("id");

            // Find the current tab pane
            var currentTabPane = $('.tab-pane').eq(index);

            // Hide the current form (collateral) and remove 'fade' class
            var nextForm = currentTabPane;
            console.log("Current form ID: " + currentFormName);

            // Hide the current form
            currentForm.addClass('fade').prop('hidden', true);

            // Show the next form
            nextForm.removeClass('fade').prop('hidden', false);

            // Always return true to allow navigation to the previous step
            return true;
        }
    });

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
        search: false,
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
        search: false,
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










    $("#LoanParticularsInformationModel_ExistingChecker").on("change", function (event) {
        // Check if checkbox is checked
        if ($(this).is(":checked")) {

            $("#LoanParticularsInformationModel_ExistingHousingApplicationNumber").prop('disabled', false);

        } else {
            $("#LoanParticularsInformationModel_ExistingHousingApplicationNumber").prop('disabled', true);
            $("#LoanParticularsInformationModel_ExistingHousingApplicationNumber").val('');

        }
    });



    $("#CollateralInformationModel_ExistingReasonChecker").on("change", function (event) {
        // Check if checkbox is checked
        if ($(this).is(":checked")) {

            $("#CollateralInformationModel_CollateralReason").prop('disabled', false);

        } else {
            $("#CollateralInformationModel_CollateralReason").prop('disabled', true);
            $("#CollateralInformationModel_CollateralReason").val('');

        }
    });










    function validateForm(form) {
        var isValid = true;

        // Your validation logic here
        // For example, check if required fields are filled
        form.find(':input[required]').each(function () {
            if (!$(this).val()) {
                $(this).addClass('is-invalid');
                isValid = false;
            } else {
                $(this).removeClass('is-invalid');
            }
        });

        return isValid;
    }

    function rebindValidators() {
        let $form = $("#frm_hlf068");
        let button = $("#btn_savehlf068");

        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate($form.data("unobtrusiveValidation").options);
        $form.data("validator").settings.ignore = "";

        $form.on("submit", function (e) {
            e.preventDefault();
            let formData = new FormData(e.target);

            if ($(this).valid() == false) {
                messageBox("Please fill out all required fields!", "danger", true);

                return;
            }

            $.ajax({
                url: $(this).attr("action"),
                method: $(this).attr("method"),
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function () {
                    button.html("<span class='spinner-border spinner-border-sm'></span> Saving...");
                    button.attr({ disabled: true });

                    $("#beneficiary-overlay").removeClass('d-none');

                },
                success: function (response) {
                    let recordId = $("input[name='User.Id']").val();
                    console.log(recordId);
                    let type = (recordId == 0 ? "Added!" : "Updated!");
                    let successMessage = `Beneficiary Successfully ${type}`;

                    messageBox(successMessage, "success", true);

                    if (applicantInfoIdVal == 0) {
                        setTimeout(function () {

                            $("#beneficiary-overlay").addClass('d-none');

                            window.location.href = "/Applicants/HLF068/" + response;
                        }, 2000);
                    }
                    else {
                        setTimeout(function () {
                            $("#beneficiary-overlay").addClass('d-none');
                            // Redirect to the specified location
                            window.location.href = "/Applicants/ApplicantRequests";
                        }, 2000); // 2000 milliseconds = 2 seconds
                    }

                    button.attr({ disabled: false });
                    button.html("<span class='mdi mdi-content-save-outline'></span> Submit");

                    userModal.modal('hide');
                },
                error: function (response) {
                    messageBox(response.responseText, "danger");
                    button.html("<span class='mdi mdi-content-save-outline'></span> Submit");
                    button.attr({ disabled: false });
                }
            });
        });
    }

    //$('#form2').on('submit', async function (e) {
    //    e.preventDefault();
    //    let $loanparticulars = $('#loanparticulars');
    //    let $collateraldata = $('#collateraldata');
    //    let $spousedata = $('#spousedata');
    //    if (!$(this).valid() || !$loanparticulars.valid() || !$collateraldata.valid() || !$spousedata.valid()) {
    //        return;
    //    }
    //    let loanparticulars = $loanparticulars.serializeArray();
    //    let collateraldata = $collateraldata.serializeArray();
    //    let spousedata = $spousedata.serializeArray();
    //    let form2 = $(this).serializeArray();
    //    let combinedData = {};
    //    $(loanparticulars.concat(collateraldata, spousedata, form2)).each(function (index, obj) {
    //        combinedData[obj.name] = obj.value;
    //    });

    //    // Use await before the AJAX call
    //    try {
    //        await $.ajax({
    //            type: 'POST',
    //            url: '/Applicants/SaveHLF068',
    //            data: combinedData,
    //            beforeSend: function () {
    //                loading("Saving Changes...");
    //            },
    //            success: async function (response) {
    //                messageBox("Successfully", "success", true);

    //                if (applicantInfoIdVal == 0) {
    //                    loader.close();
    //                    setTimeout(function () {
    //                        window.location.href = "/Applicants/HLF068/" + response;
    //                    }, 2000);
    //                }
    //                else {
    //                    loader.close();

    //                    setTimeout(function () {
    //                        // Redirect to the specified location
    //                        window.location.href = "/Applicants/ApplicantRequests";
    //                    }, 2000); // 2000 milliseconds = 2 seconds
    //                }
    //            },
    //            error: async function (jqXHR, textStatus, errorThrown) {
    //                console.log(jqXHR.responseText);
    //                messageBox(jqXHR.responseText, "danger", true);
    //                loader.close();
    //            }
    //        });
    //    } catch (error) {
    //        // Handle any errors from the AJAX request
    //        console.log(error);
    //        // Optionally display an error message
    //        messageBox("An error occurred during the submission.", "danger", true);
    //    }
    //});

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