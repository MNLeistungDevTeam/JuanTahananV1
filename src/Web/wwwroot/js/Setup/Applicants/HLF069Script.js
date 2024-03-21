﻿const applicantInfoIdVal = $(`[name='ApplicantsPersonalInformationModel.Id']`).val();
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

            //currentForm.addClass('was-validated');

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

    //#region Selectize

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

    //#endregion

    $('#LoanParticularsInformationModel_DesiredLoanAmount').on('input', function () {
        var inputValue = $(this).val().toString();
        var numericValue = parseInt(inputValue, 10);

        // Check if both the length exceeds 7 characters and the numeric value exceeds 7 million
        if (inputValue.length > 7 || numericValue > 7000000) {
            //alert("Input value exceeds both 7 characters and 7 million!");

            messageBox("Desired Loan Amount exceeds up to 7000000!", "danger", true);

            $('#LoanParticularsInformationModel_DesiredLoanAmount').trigger('invalid');

            $(this).val(0);

            // Adjust the input value accordingly
            inputValue = inputValue.substring(0, 7); // Truncate input to 7 characters
            $(this).val(0); // Set the input value
        }
    });

    $('#LoanParticularsInformationModel_DesiredLoanTermYears').on('input', function () {
        var inputValue = $(this).val().toString();
        var numericValue = parseInt(inputValue, 10);

        if (numericValue > 30 || inputValue.length > 2) {
            //alert("Input value exceeds 30!");

            messageBox("Desired Loan Term Years exceeds 30!", "danger", true);

            $('#LoanParticularsInformationModel_DesiredLoanTermYears').trigger('invalid');

            $(this).val(0);
        }
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

    //#endregion

    //#region Collateral
    $("#CollateralInformationModel_ExistingReasonChecker").on("change", function (event) {
        // Check if checkbox is checked
        if ($(this).is(":checked")) {
            $("#CollateralInformationModel_CollateralReason").prop('disabled', false);
        } else {
            $("#CollateralInformationModel_CollateralReason").prop('disabled', true);
            $("#CollateralInformationModel_CollateralReason").val('');
        }
    });

    $('#CollateralInformationModel_TctOctCctNumber').on('input', function () {
        var inputValue = $(this).val().toString();

        //maximum 25 characters
        if (inputValue.length > 25) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 25));

            messageBox("TctOctCctNumber must not exceed to 25 characters", "danger", true);

            $('#CollateralInformationModel_TctOctCctNumber').trigger('invalid');
        }
    });

    $('#CollateralInformationModel_TaxDeclrationNumber').on('input', function () {
        var inputValue = $(this).val().toString();
        //maximum 25 characters
        if (inputValue.length > 25) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 25));
            messageBox("Tax Declaration Number must not exceed to 25 characters", "danger", true);

            $('#CollateralInformationModel_TaxDeclrationNumber').trigger('invalid');
        }
    });

    $('#CollateralInformationModel_LotUnitNumber').on('input', function () {
        var inputValue = $(this).val().toString();
        //maximum 25 characters
        if (inputValue.length > 25) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 25));

            messageBox("Lot Unit Number must not exceed to 25 characters", "danger", true);

            $('#CollateralInformationModel_LotUnitNumber').trigger('invalid');
        }
    });

    $('#CollateralInformationModel_BlockBuildingNumber').on('input', function () {
        var inputValue = $(this).val().toString();
        //maximum 25 characters
        if (inputValue.length > 25) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 25));
            messageBox("Block Building Number must not exceed to 25 characters", "danger", true);

            $('#CollateralInformationModel_BlockBuildingNumber').trigger('invalid');
        }
    });

    $('#CollateralInformationModel_LandArea').on('input', function () {
        var inputValue = $(this).val().toString();
        //maximum 10 characters
        if (inputValue.length > 10) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 10));

            messageBox("Land/Floor Area must not exceed to 9999999999 sqm", "danger", true);

            $('#CollateralInformationModel_LandArea').trigger('invalid');
        }
    });

    $('#CollateralInformationModel_HouseAge').on('input', function () {
        var inputValue = $(this).val().toString();
        //maximum 3 characters
        if (inputValue.length > 3) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 3));

            messageBox("House age must not exceed to 1000 years", "danger", true);

            $('#CollateralInformationModel_LandArea').trigger('invalid');
        }
    });

    $('#CollateralInformationModel_NumberOfStoreys').on('input', function () {
        var inputValue = $(this).val().toString();
        //maximum 3 characters
        if (inputValue.length > 3) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 3));

            messageBox("Existing Number of storey must not exceed to 999", "danger", true);

            $('#CollateralInformationModel_NumberOfStoreys').trigger('invalid');
        }
    });

    $('#CollateralInformationModel_ProposedNoOfStoreys').on('input', function () {
        var inputValue = $(this).val().toString();
        //maximum 3 characters
        if (inputValue.length > 3) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 3));

            messageBox("Propose Number of storey must not exceed to 999", "danger", true);

            $('#CollateralInformationModel_ProposedNoOfStoreys').trigger('invalid');
        }
    });

    $('#CollateralInformationModel_ExistingTotalFloorArea').on('input', function () {
        var inputValue = $(this).val().toString();
        //maximum 10 characters
        if (inputValue.length > 10) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 10));

            messageBox("Existing TotalFloorArea must not exceed to 9999999999 sqm", "danger", true);

            $('#CollateralInformationModel_ExistingTotalFloorArea').trigger('invalid');
        }
    });

    $('#CollateralInformationModel_ProposedTotalFloorArea').on('input', function () {
        var inputValue = $(this).val().toString();
        //maximum 10 characters
        if (inputValue.length > 10) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 10));

            messageBox("Propose TotalFloorArea must not exceed to 9999999999 sqm", "danger", true);

            $('#CollateralInformationModel_ProposedTotalFloorArea').trigger('invalid');
        }
    });

    //#region Selectize

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

    //#endregion

    //#region Form2Page
    var sourcePagibigFundVal = $(`[name='Form2PageModel.SourcePagibigFundId']`).attr('data-value');

    var sourePagibigFundDropdown, $sourePagibigFundDropdown;

    $sourePagibigFundDropdown = $(`[name='Form2PageModel.SourcePagibigFundId']`).selectize({
        valueField: 'Id',
        labelField: 'Name',
        searchField: 'Name',
        preload: true,
        search: false,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + 'Applicants/GetAllSourcePagibigFund',
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
                    escape(item.Name) +
                    "</div>"
                );
            },
            option: function (item, escape) {
                return ("<div class='py-1 px-2'>" +
                    escape(item.Name) +
                    "</div>"
                );
            }
        },
    });

    sourePagibigFundDropdown = $sourePagibigFundDropdown[0].selectize;

    sourePagibigFundDropdown.on('load', function (options) {
        sourePagibigFundDropdown.setValue(sourcePagibigFundVal || '');
        //resourceCounter("purposeofloan");
        sourePagibigFundDropdown.off('load');
    });

    //#endregion




    //#region Barrower



    $('#BarrowersInformationModel_HomeNumber').on('input', function () {
        var inputValue = $(this).val().toString();
        //maximum 25 characters
        if (inputValue.length > 25) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 25));

            messageBox("HomeNumber must not exceed to 25 characters", "danger", true);

            $('#BarrowersInformationModel_HomeNumber').trigger('invalid');
        }
    });



    $('#BarrowersInformationModel_MobileNumber').on('input', function () {
        var inputValue = $(this).val().toString();

        //maximum 25 characters
        if (inputValue.length > 25) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 25));

            messageBox("Mobile Number must not exceed to 25 characters", "danger", true);

            $('#BarrowersInformationModel_MobileNumber').trigger('invalid');
        }
    });



    $('#BarrowersInformationModel_YearsofStay').on('input', function () {
        var inputValue = $(this).val().toString();

        //maximum 25 characters
        if (inputValue.length > 3) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 3));

            messageBox("Years of Stay must not exceed to 3 characters", "danger", true);

            $('#BarrowersInformationModel_YearsofStay').trigger('invalid');
        }
    });



    $('#ApplicantsPersonalInformationModel_HousingAccountNumber').on('input', function () {
        var inputValue = $(this).val().toString();

        //maximum 25 characters
        if (inputValue.length > 12) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 12));

            messageBox("Housing Account Number must not exceed to 12 characters", "danger", true);

            $('#ApplicantsPersonalInformationModel_HousingAccountNumber').trigger('invalid');
        }
    });




    $('#BarrowersInformationModel_SSSNumber').on('input', function () {
        var inputValue = $(this).val().toString();

        //maximum 25 characters
        if (inputValue.length > 25) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 25));

            messageBox("SSS Number must not exceed to 25 characters", "danger", true);

            $('#BarrowersInformationModel_SSSNumber').trigger('invalid');
        }
    });




    $('#BarrowersInformationModel_TinNumber').on('input', function () {
        var inputValue = $(this).val().toString();

        //maximum 25 characters
        if (inputValue.length > 25) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 25));

            messageBox("TIN Number must not exceed to 25 characters", "danger", true);

            $('#BarrowersInformationModel_TinNumber').trigger('invalid');
        }
    });




    $('#ApplicantsPersonalInformationModel_PagibigNumber').on('input', function () {
        var inputValue = $(this).val().toString();

        //maximum 25 characters
        if (inputValue.length > 12) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 12));

            messageBox("PagIBIG Number must not exceed to 12 characters", "danger", true);

            $('#ApplicantsPersonalInformationModel_PagibigNumber').trigger('invalid');
        }
    });


    $('#BarrowersInformationModel_BusinessDirectLineNumber').on('input', function () {
        var inputValue = $(this).val().toString();

        //maximum 25 characters
        if (inputValue.length > 25) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 25));

            messageBox("Business Direct Line must not exceed to 25 characters", "danger", true);

            $('#BarrowersInformationModel_BusinessDirectLineNumber').trigger('invalid');
        }
    });




    $('#BarrowersInformationModel_BusinessTruckLineNumber').on('input', function () {
        var inputValue = $(this).val().toString();

        //maximum 25 characters
        if (inputValue.length > 25) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 25));

            messageBox("Business TruckLine Number must not exceed to 25 characters", "danger", true);

            $('#BarrowersInformationModel_BusinessTruckLineNumber').trigger('invalid');
        }
    });















    //#endregion


    function validateForm(form) {
        var isValid = true;

        // Your validation logic here
        // For example, check if required fields are filled
        form.find(':input[required]').each(function () {
            if (!$(this).val()) {
                $(this).addClass('is-invalid');
                $(this).removeClass('was-validated');
                isValid = false;
            } else {
                $(this).removeClass('is-invalid');
                $(this).addClass('was-validated');
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
                    $("#beneficiary-overlay").addClass('d-none');
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