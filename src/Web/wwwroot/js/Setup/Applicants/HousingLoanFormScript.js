﻿const applicantInfoIdVal = $(`[name='ApplicantsPersonalInformationModel.Id']`).val();
const applicantInfoApprovalStatus = $(`[name='ApplicantsPersonalInformationModel.ApprovalStatus']`).val();
const roleName = $("#txt_role_name").val();
const roleId = $("#txt_roleId").val();

const buyerconfirmationCode = $("#BuyerConfirmationModel_Code").val();
const buyerconfirmationAppStatus = $("#BuyerConfirmationModel_ApprovalStatus").val();
const hasBcf = $("#BarrowersInformationModel_IsBcfCreated").val();

$(async function () {
    var telNoArray = [];
    var itiFlag = false;

    var editableFlag = false;

    const { pdfjsLib } = globalThis;

    var telNoArray = [];
    var currentStep = 0;

    //#region Initialization

    $("#btn_savehlf068").prop('disabled', true);

    console.log(hasBcf);
    //$("#btn_edit").prop('disabled', !(applicantInfoApprovalStatus == '0' || applicantInfoApprovalStatus == '11'));

    $(".selectize").selectize({
        search: false
    });

    $('.calendarpicker').flatpickr({
        dateFormat: "m/d/Y",
    });

    $('.present-calendar-picker').flatpickr({
        dateFormat: "m/d/Y",
        maxDate: moment().format("MM/DD/YYYY")
    });

    $(".timepicker").flatpickr({
        enableTime: true,
        noCalendar: true,
        dateFormat: "H:i",
        time_24hr: true
    });

    $('.pagibigInputMask').inputmask({
        mask: "9999-9999-9999",
        placeholder: 'X',
        //clearIncomplete: true
    });

    $('.tinInputMask').inputmask({
        mask: "999-999-999[-9999]",
        placeholder: "X",
        //clearIncomplete: true
    });

    //// Disable 'e', '+', retain '-'
    //$('.codeInputMask').inputmask({ regex: "^[A-Z0-9-]*$" }); // zip code

    $('.codeInputMask').inputmask({
        mask: "*{1,10}",
        regex: "^[A-Za-df-zA-DF-Z0-9]*-?[A-Za-df-zA-DF-Z0-9]*$",
        definitions: {
            '*': {
                validator: function (chrs, buffer, pos, strict, opts) {
                    // Disallow '-' if it's the first character
                    if (pos === 0 && chrs === '-') {
                        return false;
                    }
                    else if (pos > 0 && chrs === '-') {
                        return !(buffer.buffer.includes('-'));
                    }

                    return true;
                }
            }
        }
    });

    initializeLeftDecimalInputMask(".decimalInputMask5", 2);

    initializeLoanCreditDate();

    //initializeIntlTelInput();
    initializeBasicTelInput();    // Disable 'e', retain '-', '+'

    initializePdfJs();

    //assessPresentPermanentCheckbox();

    assessCheckbox(
        $(`[name="BarrowersInformationModel.PresentAddressIsPermanentAddress"]`),
        $(`input[name^="BarrowersInformationModel.Present"][type="text"]`)
    );

    rebindValidators();

    adjustEditCard();
    progressCheck();

    //#endregion

    // Function to handle beforeunload prompt

    window.onbeforeunload = function (event) {
        if (editableFlag) {
            event.returnValue = "You have unsaved changes. Are you sure you want to leave this page?";
            return event.returnValue;

            //Swal.fire({
            //    title: 'Leave edit mode?',
            //    text: 'You have unsaved changes. If you leave now then all of your edits progress will be lost.',
            //    icon: 'warning',
            //    showCancelButton: true,
            //    confirmButtonColor: '#3085d6',
            //    cancelButtonColor: '#d33',
            //    confirmButtonText: 'Discard changes',
            //    cancelButtonText: 'Continue editing'
            //}).then((result) => {
            //    if (result.isConfirmed) {
            //        editableFlag = false; // Resetting the flag
            //    }
            //});
        }
    };

    //$('.mobileNumInputMask').inputmask({ mask: "9999-999-9999" });

    // Disable 'e', retain '-', '+'
    //$(`[name="BarrowersInformationModel.HomeNumber"]`).inputmask({ regex: `^[0-9+-]*$` /*, mask: `(+9{1,}) 9{1,}`*/ });
    //$(`[name="BarrowersInformationModel.MobileNumber"]`).inputmask({ regex: `^[0-9+-]*$` /*, mask: `(+9{1,}) 9{1,}`*/ });
    //$(`[name="BarrowersInformationModel.BusinessDirectLineNumber"]`).inputmask({ regex: `^[0-9+-]*$` /*, mask: `(+9{1,}) 9{1,}`*/ });
    //$(`[name="BarrowersInformationModel.BusinessTruckLineNumber"]`).inputmask({ regex: `^[0-9+-]*$` /*, mask: `(+9{1,}) 9{1,}`*/ });
    //$(`[name="SpouseModel.BusinessTelNo"]`).inputmask({ regex: `^[0-9+-]*$` /*, mask: `(+9{1,}) 9{1,}`*/ });
    //$(`[name^="Form2PageModel.TradeTellNo"]`).inputmask({ regex: `^[0-9+-]*$` });
    //$(`[name^="Form2PageModel.CharacterTellNo"]`).inputmask({ regex: `^[0-9+-]*$` });

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

    //#region Loan Particulars

    //#region Selectize

    var purposeofloanVal = $(`[name='LoanParticularsInformationModel.PurposeOfLoanId']`).attr('data-value');
    var modeofpaymentVal = $(`[name='LoanParticularsInformationModel.ModeOfPaymentId']`).attr('data-value');

    var purposeOfLoanDropdown, $purposeOfLoanDropdown;
    var modeofPaymentDropdown, $modeofPaymentDropdown;
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
        create: false,
        readOnly: true,
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

    $('#LoanParticularsInformationModel_DesiredLoanAmount').on('keydown', function (e) {
        // Reject inputs 'e', '+', '-'
        let rejectCodes = ['e', 'E', '-', '-', '+'];
        //console.log(e.key);
        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
        }
    });

    $('#LoanParticularsInformationModel_DesiredLoanAmount').on('input', function (e) {
        //// Get the raw value without the input mask
        //var rawValue = $(this).inputmask('unmaskedvalue');

        //// Convert the raw value to a numeric format
        //var numericValue = parseFloat(rawValue);

        //// Check if the length exceeds 7 characters
        //if (rawValue.length > 7 || numericValue > 7000000) {
        //    messageBox("Desired Loan Amount cannot exceed 7,000,000!", "danger", true);
        //    $(this).addClass("input-validation-error is-invalid");
        //    $(this).val(0); // Reset the input value to 0
        //}

        // Get the raw value without the input mask
        var rawValue = $(this).inputmask('unmaskedvalue');

        // Convert the raw value to a numeric format
        var numericValue = parseFloat(rawValue);

        // Check if the numeric value exceeds 7 million
        if (numericValue > 7000000) {
            messageBox("Desired Loan Amount cannot exceed 7,000,000!", "danger", true);
            $(this).addClass("input-validation-error is-invalid");
            $(this).val(0); // Reset the input value to 0
        }

        // Check if the length exceeds 7 characters
        if (rawValue.length > 7) {
            // Truncate input to 7 characters
            var truncatedValue = rawValue.substring(0, 7);
            $(this).val(truncatedValue); // Set the input value
        }
    });

    $('#LoanParticularsInformationModel_DesiredLoanTermYears').on('keydown', function (e) {
        // Reject inputs 'e', '+', '-'
        //let rejectCodes = ['KeyE', 'NumpadAdd', 'NumpadSubtract'];
        let rejectCodes = ['e', 'E', '-', '-', '+'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
        }
    });

    $('#LoanParticularsInformationModel_DesiredLoanTermYears').on('input', function (e) {
        //var inputValue = $(this).val().toString();
        //var numericValue = parseInt(inputValue, 10);

        //if (numericValue > 30 || inputValue.length > 2) {
        //    //alert("Input value exceeds 30!");

        //    messageBox("Desired Loan Term Years exceeds 30!", "danger", true);

        //    $('#LoanParticularsInformationModel_DesiredLoanTermYears').trigger('invalid');

        //    $(this).val(0);
        //}

        // Get the raw value without the input mask
        var rawValue = $(this).val().toString();

        // Convert the raw value to a numeric format
        var numericValue = parseInt(rawValue, 10);

        // Check if the numeric value exceeds 7 million
        if (numericValue > 30) {
            messageBox("Desired Loan Terms exceeds 30!", "danger", true);
            $(this).trigger('invalid');
            $(this).val(30);
        }

        // Check if the length exceeds 7 characters
        if (rawValue.length > 2) {
            // Truncate input to 7 characters
            var truncatedValue = rawValue.substring(0, 2);
            $(this).val(truncatedValue); // Set the input value
        }
    });

    //#region Set Selectize to readonly
    $('#LoanParticularsInformationModel_PurposeOfLoanId-selectized').prop('readonly', true);
    $('#LoanParticularsInformationModel_ModeOfPaymentId-selectized').prop('readonly', true);
    //#endregion

    $('#LoanParticularsInformationModel_ExistingChecker').on('change', function (e) {
        e.preventDefault();
        if ($(this).prop('checked')) {
            $('[name="LoanParticularsInformationModel.ExistingHousingApplicationNumber"]').prop('disabled', false);
            $('[name="LoanParticularsInformationModel.ExistingHousingApplicationNumber"]').attr('required', true);
        } else {
            $('[name="LoanParticularsInformationModel.ExistingHousingApplicationNumber"]').val('');
            $('[name="LoanParticularsInformationModel.ExistingHousingApplicationNumber"]').prop('disabled', true);
            $('[name="LoanParticularsInformationModel.ExistingHousingApplicationNumber"]').removeAttr('required');
        }
    });

    //#endregion

    //#endregion

    //#region Collateral

    // Disable 'e', '+', and '-' - not yet

    $(`#CollateralInformationModel_TctOctCctNumber`).inputmask({ regex: `^\\d(?:-?\\d+)*$` });
    $(`[name="CollateralInformationModel.TaxDeclrationNumber"]`).inputmask({ regex: `^\\d(?:-?\\d+)*$` });

    $("#CollateralInformationModel_ExistingReasonChecker").on("change", function (event) {
        // Check if checkbox is checked
        if ($(this).is(":checked")) {
            $("#CollateralInformationModel_CollateralReason").prop('disabled', false);
            $('[name="LCollateralInformationModel.CollateralReason"]').attr('required', true);
        } else {
            $("#CollateralInformationModel_CollateralReason").prop('disabled', true);
            $("#CollateralInformationModel_CollateralReason").val('');
            $('[name="CollateralInformationModel.CollateralReason"]').removeAttr('required');
        }
    });

    $('#CollateralInformationModel_TctOctCctNumber').on('keydown', function (e) {
        // Reject inputs 'e', '-'
        //let rejectCodes = ['KeyE', 'NumpadAdd'];
        let rejectCodes = ['e', 'E', '+'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
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

    $('#CollateralInformationModel_TaxDeclrationNumber').on('keydown', function (e) {
        // Reject inputs 'e', '-'
        //let rejectCodes = ['KeyE', 'NumpadAdd'];
        let rejectCodes = ['e', 'E', '+'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
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

    $('#CollateralInformationModel_LandArea').on('keydown', function (e) {
        // Reject inputs 'e', '-', '+'
        //let rejectCodes = ['KeyE', 'NumpadAdd', 'NumpadSubtract'];
        let rejectCodes = ['e', 'E', '-', '-', '+'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
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
    var sourePagibigFundDropdown, $sourePagibigFundDropdown;

    $propertyTypeDropdown = $(`[name='CollateralInformationModel.PropertyTypeId']`).selectize({
        valueField: 'Id',
        labelField: 'Description',
        searchField: 'Description',
        placeholder: "Select Property Type...",
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

    // Disable 'e', '+', and '-' - not yet
    $(`[name^="Form2PageModel.AccountNumber"]`).inputmask({ regex: "^[A-Z0-9-]*$" });

    var sourcePagibigFundVal = $("Form2PageModel.SourcePagibigFundId").attr('data-value');

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

    // Set values for Form2PageModel_DateOpened fields
    setDateValue('#Form2PageModel_DateOpened1');
    setDateValue('#Form2PageModel_DateOpened2');
    setDateValue('#Form2PageModel_DateOpened3');

    // Set values for Form2PageModel_CardExpiration fields
    setDateValue('#Form2PageModel_CardExpiration1');
    setDateValue('#Form2PageModel_CardExpiration2');
    setDateValue('#Form2PageModel_CardExpiration3');

    // Set values for Form2PageModel_DateFullyPaid fields
    setDateValue('#Form2PageModel_DateFullyPaid1');
    setDateValue('#Form2PageModel_DateFullyPaid2');
    setDateValue('#Form2PageModel_DateFullyPaid3');

    // Set values for Form2PageModel_DateObtained fields
    setDateValue('#Form2PageModel_DateObtained1');
    setDateValue('#Form2PageModel_DateObtained2');
    setDateValue('#Form2PageModel_DateObtained3');

    setDateValue('#Form2PageModel_MaturityDateTime1');
    setDateValue('#Form2PageModel_MaturityDateTime2');
    setDateValue('#Form2PageModel_MaturityDateTime3');

    //Miscellaneous Rbtn Events

    $('.radio-pcRadio input[type="radio"]').on('change', function () {
        let $inputField = $("[name='Form2PageModel.PendingCase']");

        if ($("#pcRadioBtn1").is(":checked")) {
            $inputField.prop('disabled', false).prop('required', true);
        } else {
            $inputField.prop('disabled', true).prop('required', false).val(null);
        }
    });

    $(".radio-pdRbtn input[type='radio']").on('change', function () {
        let $inputField = $("[name='Form2PageModel.PastDue']");

        if ($("#pdRbtn1").is(":checked")) {
            $inputField.prop('disabled', false).prop('required', true);
        } else {
            $inputField.prop('disabled', true).prop('required', false).val(null);
        }
    });

    $(".radio-bcRbtn input[type='radio']").on('change', function () {
        let $inputField = $("[name='Form2PageModel.BouncingChecks']");

        if ($("#bcRbtn1").is(":checked")) {
            $inputField.prop('disabled', false).prop('required', true);
        } else {
            $inputField.prop('disabled', true).prop('required', false).val(null);
        }
    });

    $(".radio-maRbtn input[type='radio']").on('change', function () {
        let $inputField = $("[name='Form2PageModel.MedicalAdvice']");

        if ($("#maRbtn1").is(":checked")) {
            $inputField.prop('disabled', false).prop('required', true);
        } else {
            $inputField.prop('disabled', true).prop('required', false).val(null);
        }
    });

    //#endregion

    //#region Barrower

    // Set value for BarrowersInformationModel_BirthDate
    setDateValue('#BarrowersInformationModel_BirthDate');

    $('[name="BarrowersInformationModel.SSSNumber"]').inputmask({
        mask: "99-99999[9][9]-99",
        placeholder: 'X',
        //clearIncomplete: true
    });

    $('[name="BarrowersInformationModel.HomeOwnerShip"]').on('change', function () {
        if ($(this).val() == 'Rented') {
            $('#rentalForm').show();
            $('[name="BarrowersInformationModel.MonthlyRent"]').attr('required', true);
        } else {
            $('#rentalForm').hide();
            $('[name="BarrowersInformationModel.MonthlyRent"]').removeAttr('required').val(null);
        }
    });

    $('#BarrowersInformationModel_BirthDate').on('change', function () {
        var birthdate = moment($(this).val());
        var today = moment();
        var age = today.diff(birthdate, 'years');

        console.log("Age: " + age);

        // Check if age is 21 or older
        if (age < 21) {
            console.log("User is NOT 21 or older");
            $(`[id="BarrowersInformationModel.BirthDate_RequiredAge"]`).fadeIn(0);
            $(this).val('');
        }
        else {
            $(`[id="BarrowersInformationModel.BirthDate_RequiredAge"]`).fadeOut(0);
        }
    });

    var homeOwnershipVal = $("#BarrowersInformationModel_HomeOwnerShip").attr('data-value');
    if (homeOwnershipVal == 'Rented') {
        // Adding d-none class if condition is true
        $('#rentalForm').removeClass('d-none');
    }

    $('[name="BarrowersInformationModel.HomeOwnerShip"]').on('change', function () {
        if ($(this).val() == 'Rented') {
            $('#rentalForm').removeClass('d-none');
            $('[name="BarrowersInformationModel.MonthlyRent"]').attr('required', true);
        } else {
            $('#rentalForm').addClass('d-none');
            $('[name="BarrowersInformationModel.MonthlyRent"]').removeAttr('required').val(null);
        }
    });

    $('#BarrowersInformationModel_HomeNumber').on('keydown', function (e) {
        // Reject inputs
        /*let rejectCodes = ['KeyE'];*/
        let rejectCodes = ['e', 'E'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
        }
    });

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

    $('#BarrowersInformationModel_MobileNumber').on('keydown', function (e) {
        // Reject inputs
        //let rejectCodes = ['KeyE'];
        let rejectCodes = ['e', 'E'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
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

    $('#BarrowersInformationModel_PermanentZipCode').on('keydown', function (e) {
        //var keyCode = e.keyCode || e.which;

        //let regexZip = /^[A-Z0-9-]*$/;
        //let key = e.key;
        //// Check if the key is a character or hyphen
        ////var isCharacterOrHyphen = (e.keyCode >= 65 && e.keyCode <= 90) ||  // A-Z
        ////keyCode == 45; // hyphen

        //if (!regexZip.test(key)) {
        //    console.log(e.key);
        //    e.preventDefault();
        //}
    });

    $(`[name^="BarrowersInformationModel.Permanent"]`).on('input', debounce(function () {
        if ($(`[name="BarrowersInformationModel.PresentAddressIsPermanentAddress"]`).prop('checked')) {
            $(`[name="BarrowersInformationModel.PresentUnitName"]`).val($(`[name="BarrowersInformationModel.PermanentUnitName"]`).val());
            $(`[name="BarrowersInformationModel.PresentBuildingName"]`).val($(`[name="BarrowersInformationModel.PermanentBuildingName"]`).val());
            $(`[name="BarrowersInformationModel.PresentLotName"]`).val($(`[name="BarrowersInformationModel.PermanentLotName"]`).val());
            $(`[name="BarrowersInformationModel.PresentStreetName"]`).val($(`[name="BarrowersInformationModel.PermanentStreetName"]`).val());
            $(`[name="BarrowersInformationModel.PresentSubdivisionName"]`).val($(`[name="BarrowersInformationModel.PermanentSubdivisionName"]`).val());
            $(`[name="BarrowersInformationModel.PresentBaranggayName"]`).val($(`[name="BarrowersInformationModel.PermanentBaranggayName"]`).val());
            $(`[name="BarrowersInformationModel.PresentMunicipalityName"]`).val($(`[name="BarrowersInformationModel.PermanentMunicipalityName"]`).val());
            $(`[name="BarrowersInformationModel.PresentProvinceName"]`).val($(`[name="BarrowersInformationModel.PermanentProvinceName"]`).val());
            $(`[name="BarrowersInformationModel.PresentZipCode"]`).val($(`[name="BarrowersInformationModel.PermanentZipCode"]`).val());
        }
    }, 2000))

    $(`[name="BarrowersInformationModel.PresentAddressIsPermanentAddress"]`).on('change', function (e) {
        if ($(this).prop('checked')) {
            $(`[name="BarrowersInformationModel.PresentUnitName"]`).val($(`[name="BarrowersInformationModel.PermanentUnitName"]`).val());
            $(`[name="BarrowersInformationModel.PresentBuildingName"]`).val($(`[name="BarrowersInformationModel.PermanentBuildingName"]`).val());
            $(`[name="BarrowersInformationModel.PresentLotName"]`).val($(`[name="BarrowersInformationModel.PermanentLotName"]`).val());
            $(`[name="BarrowersInformationModel.PresentStreetName"]`).val($(`[name="BarrowersInformationModel.PermanentStreetName"]`).val());
            $(`[name="BarrowersInformationModel.PresentSubdivisionName"]`).val($(`[name="BarrowersInformationModel.PermanentSubdivisionName"]`).val());
            $(`[name="BarrowersInformationModel.PresentBaranggayName"]`).val($(`[name="BarrowersInformationModel.PermanentBaranggayName"]`).val());
            $(`[name="BarrowersInformationModel.PresentMunicipalityName"]`).val($(`[name="BarrowersInformationModel.PermanentMunicipalityName"]`).val());
            $(`[name="BarrowersInformationModel.PresentProvinceName"]`).val($(`[name="BarrowersInformationModel.PermanentProvinceName"]`).val());
            $(`[name="BarrowersInformationModel.PresentZipCode"]`).val($(`[name="BarrowersInformationModel.PermanentZipCode"]`).val());

            $(`input[name^="BarrowersInformationModel.Present"][type="text"]`).prop('readonly', true);

            return;
        }
        else {
            $(`input[name^="BarrowersInformationModel.Present"][type="text"]`).val("");
            $(`input[name^="BarrowersInformationModel.Present"][type="text"]`).prop('readonly', false);
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

    $('#BarrowersInformationModel_SSSNumber').on('keydown', function (e) {
        // Reject inputs
        //let rejectCodes = ['KeyE', 'NumpadAdd'];
        let rejectCodes = ['e', 'E', '+'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
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

    $('#BarrowersInformationModel_TinNumber').on('keydown', function (e) {
        // Reject inputs
        //let rejectCodes = ['KeyE', 'NumpadAdd'];
        let rejectCodes = ['e', 'E', '+'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
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

    $('#BarrowersInformationModel_YearsEmployment').on('keydown', function (e) {
        // Reject inputs
        //let rejectCodes = ['KeyE', 'NumpadAdd'];
        let rejectCodes = ['e', 'E', '+'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
        }
    });

    $('#BarrowersInformationModel_NumberOfDependent').on('keydown', function (e) {
        // Get the key code of the pressed key
        var keyCode = e.keyCode || e.which;

        // Allow numbers and specific keys like backspace, delete, arrows, etc.
        if ((keyCode < 48 || keyCode > 57) && // Not a number key
            keyCode != 8 && keyCode != 9 && keyCode != 13 && // Backspace, Tab, Enter
            keyCode != 37 && keyCode != 39 && // Left and right arrows
            keyCode != 46) { // Delete
            e.preventDefault(); // Prevent the character from being entered
        }
    });

    $('#BarrowersInformationModel_DirectLineNumber').on('keydown', function (e) {
        // Get the key code of the pressed key
        var keyCode = e.keyCode || e.which;

        // Allow numbers and specific keys like backspace, delete, arrows, etc.
        if ((keyCode < 48 || keyCode > 57) && // Not a number key
            keyCode != 8 && keyCode != 9 && keyCode != 13 && // Backspace, Tab, Enter
            keyCode != 37 && keyCode != 39 && // Left and right arrows
            keyCode != 46) { // Delete
            e.preventDefault(); // Prevent the character from being entered
        }
    });

    //$('#ApplicantsPersonalInformationModel_PagibigNumber').on('input', function () {
    //    var inputValue = $(this).val().toString();

    //    //maximum 25 characters
    //    if (inputValue.length > 12) {
    //        //alert("Input value exceeds 7 characters!");
    //        $(this).val(inputValue.substring(0, 12));

    //        messageBox("PagIBIG Number must not exceed to 12 characters", "danger", true);

    //        $('#ApplicantsPersonalInformationModel_PagibigNumber').trigger('invalid');
    //    }
    //});

    $('#BarrowersInformationModel_BusinessDirectLineNumber').on('keydown', function (e) {
        // Reject inputs
        /*let rejectCodes = ['KeyE'];*/
        let rejectCodes = ['e', 'E'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
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

    $('#BarrowersInformationModel_BusinessTruckLineNumber').on('keydown', function (e) {
        // Reject inputs
        //let rejectCodes = ['KeyE'];
        let rejectCodes = ['e', 'E'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
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

    $('#BarrowersInformationModel_MaritalStatus').on('change', function () {
        var value = $(this).val();

        $('#SpouseModel_FirstName').prop('required', false);
        $('#SpouseModel_LastName').prop('required', false);
        $('#SpouseModel_Citizenship').prop('required', false);
        $('#SpouseModel_BirthDate').prop('required', false);
        $('#SpouseModel_PagibigMidNumber').prop('required', false);
        $('#SpouseModel_SpouseEmploymentBaranggayName').prop('required', false);
        $('#SpouseModel_SpouseEmploymentMunicipalityName').prop('required', false);
        $('#SpouseModel_SpouseEmploymentProvinceName').prop('required', false);
        $('#SpouseModel_SpouseEmploymentZipCode').prop('required', false);

        if (value !== 'Married') {
            return;
        }
        else {
            $('#SpouseModel_FirstName').prop('required', true);
            $('#SpouseModel_LastName').prop('required', true);
            $('#SpouseModel_Citizenship').prop('required', true);
            $('#SpouseModel_BirthDate').prop('required', true);
            //$('#SpouseModel_SpouseEmploymentBaranggayName').prop('required', true);
            //$('#SpouseModel_SpouseEmploymentMunicipalityName').prop('required', true);
            //$('#SpouseModel_SpouseEmploymentProvinceName').prop('required', true);
            //$('#SpouseModel_SpouseEmploymentZipCode').prop('required', true);
        }
    });

    //#region Set Selectize to readonly
    $(`#BarrowersInformationModel_Sex-selectized`).prop('readonly', true);
    $('#BarrowersInformationModel_MaritalStatus-selectized').prop('readonly', true);
    $('#BarrowersInformationModel_HomeOwnerShip-selectized').prop('readonly', true);
    $('#BarrowersInformationModel_OccupationStatus-selectized').prop('readonly', true);
    $('#BarrowersInformationModel_PreparedMailingAddress-selectized').prop('readonly', true);
    //#endregion

    //#endregion

    //#region Spouse

    // Set value for SpouseModel_BirthDate
    setDateValue('#SpouseModel_BirthDate');

    $('#SpouseModel_PagibigMidNumber').on('keydown', function (e) {
        // Reject inputs
        //let rejectCodes = ['KeyE', 'NumpadAdd'];
        let rejectCodes = ['e', 'E', '+'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
        }
    });

    $('#SpouseModel_TinNumber').on('keydown', function (e) {
        // Reject inputs
        //let rejectCodes = ['KeyE', 'NumpadAdd'];
        let rejectCodes = ['e', 'E', '+'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
        }
    });

    $('#SpouseModel_BusinessTelNo').on('keydown', function (e) {
        // Reject inputs
        //let rejectCodes = ['KeyE'];
        let rejectCodes = ['e', 'E'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
        }
    });

    $(`[id^="Form2PageModel_TradeTellNo"]`).on('keydown', function (e) {
        // Reject inputs 'e', '-', '+'
        //let rejectCodes = ['KeyE', 'NumpadAdd', 'NumpadSubtract'];
        let rejectCodes = ['e', 'E'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
        }
    });

    $(`[id^="Form2PageModel_CharacterTellNo"]`).on('keydown', function (e) {
        // Reject inputs 'e', '-', '+'
        //let rejectCodes = ['KeyE', 'NumpadAdd', 'NumpadSubtract'];
        let rejectCodes = ['e', 'E'];

        if ($.inArray(e.key, rejectCodes) > -1) {
            e.preventDefault(); // Prevent the character from being entered
        }
    });

    //#region Set Selectize to readonly
    $('#SpouseModel_OccupationStatus-selectized').prop('readonly', true);
    //#endregion Set Selectize to readonly

    //#endregion

    //$(`[id="form2"] .next button`).on('click', function (e) {
    //    if (hasBcf === "True") {
    //        loadHlafPreview();
    //    }

    //    else {
    //        loadBcfPreview();
    //    }
    //});

    //$(`[id="previewBcf"] .next button`).on('click', function (e) {
    //    loadHlafPreview();
    //});

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

            currentForm.addClass('was-validated');

            // Validate the current form
            var isValid = validateForm(currentForm);

            if (editableFlag) {
                console.log("editableFlag is true");

                currentForm.addClass('was-validated');
                var isValid = validateForm(currentForm);

                if (currentFormName == "bcfdata") {
                    bcfToHLafConnectedFieldMap();
                }

                if (!isValid) {
                    // If validation fails, prevent navigation to the next step
                    console.log("validation fail");
                    return false;
                } else {
                    console.log('fade executed');
                    // Hide the current form
                    currentForm.addClass('fade').prop('hidden', true);

                    // Show the previous form
                    prevForm.removeClass('fade').prop('hidden', false);

                    if (currentFormName == "bcfdata") {
                        bcfToHLafConnectedFieldMap();
                    }
                }
            } else {
                console.log('fade executed');
                // Hide the current form
                currentForm.addClass('fade').prop('hidden', true);

                // Show the previous form
                prevForm.removeClass('fade').prop('hidden', false);
            }

            // If current form is "form2", return without proceeding to next step
            if (currentFormName == "previewHlaf") {
                $("#previewHlaf").removeClass('fade').prop('hidden', false);
                return;
            }

            //Will auto scroll to Top of the view
            if (currentFormName === "bcfdata") {
                $("html, body").animate({ scrollTop: 0 }, "fast");
            }

            if (currentFormName == "form2" && isValid) {
                if (hasBcf === "True") {
                    loadHlafPreview();
                } else {
                    loadBcfPreview();
                }
            }

            if (currentFormName == "previewBcf") {
                loadHlafPreview();
            }

            progressCheck(prevForm.attr('id'));
        },
        onPrevious: function (tab, navigation, index) {
            console.log("Previous button clicked");

            var currentForm = $($(tab).data("target-div"));
            var currentFormName = currentForm.attr("id");

            if (currentFormName == "loanparticulars") {
                HLafTobcfConnectedFieldMap();
            }

            // Find the current tab pane
            var currentTabPane = $('.tab-pane').eq(index);

            // Hide the current form (collateral) and remove 'fade' class
            var nextForm = currentTabPane;
            console.log("Current form ID: " + currentFormName);

            //$("#form3").addClass('was-validated').prop('hidden', true);

            // Hide the current form
            currentForm.addClass('fade').prop('hidden', true);

            // Show the next form
            nextForm.removeClass('fade').prop('hidden', false);

            progressCheck(nextForm.attr('id'));

            // Always return true to allow navigation to the previous step
            return true;
        }
    });

    //#region Events
    $("#btn_edit").on('click', async function () {
        if (editableFlag) {
            preConfirmationSwal();
            return;
        }

        editableFlag = true;

        $(this).addClass("active");

        $('#confirmCard').prop("hidden", false);
        //$("#frm_hlf068 .selectize").each(function () {
        //    var selectize = $(this)[0].selectize;
        //    selectize.lock();
        //});
        $("#frm_hlf068 input, #frm_hlf068 select, #frm_hlf068 textarea").removeAttr("readonly");
        $("#frm_hlf068 input, #frm_hlf068 select, #frm_hlf068 textarea").removeClass("disabled");
        $(`#frm_hlf068 input[type="checkbox"]`).removeAttr("disabled");

        $('.calendarpicker, .timepicker, .present-calendar-picker').prop('disabled', false);
        $('input[type="radio"]').prop('disabled', false);

        //reinitialize miscellaneous radio buttons
        initializeRadioBtnMisc();

        if ($(`[name="BarrowersInformationModel.PresentAddressIsPermanentAddress"]`).prop('checked')) {
            $(`input[name^="BarrowersInformationModel.Present"][type="text"]`).prop('readonly', true);
        }

        $.each($('.calendarpicker, .timepicker, .present-calendar-picker'), function (i, elem) {
            elem._flatpickr.set("allowInput", true);
            elem._flatpickr.set("clickOpens", true);
            //elem._flatpickr.set("wrap", true);
        });

        $('#frm_hlf068').find('.selectized').each(function (i, e) {
            e.selectize.unlock();
        })

        $("#btn_savehlf068").prop('disabled', false);

        $(this).removeClass("text-info").addClass("text-danger").html(`<i class="mdi mdi-pencil-outline text-info"></i> Leave Edit Mode`);

        if (buyerconfirmationAppStatus === '3') {
            DisableBuyerConfirmationFields();

            $("#BuyerConfirmationModel_ApprovalStatus").attr('disabled', false);
        }

        //Load the possible data on others forms
        HLafTobcfConnectedFieldMap();
        bcfToHLafConnectedFieldMap();

        messageBox("Editing Mode", "info", true);
    });

    $("#btn_discardChang").on("click", () => preConfirmationSwal());

    $("#btn_hlafpdf").on('click', function () {
        let applicationCode = $("#ApplicantsPersonalInformationModel_Code").val();
        let link = baseUrl + "Report/LatestHousingForm/" + applicationCode;

        window.open(link, '_blank');
    });

    $("#btn_bcfpdf").on('click', function () {
        let link = baseUrl + "Report/LatestBuyerConfirmationForm/" + buyerconfirmationCode;

        // If buyerconfirmationCode is not null or empty, open the link in a new tab
        window.open(link, '_blank');
    });

    $("#btn_saveChang").on('click', function () {
        editableFlag = false;
        $("#frm_hlf068").trigger('submit');
    });

    $('#BuyerConfirmationModel_MaritalStatus').on('change', function () {
        var value = $(this).val();

        if (value === 'Married') {
            $('#BuyerConfirmationModel_SpouseFirstName').prop('required', true);
            $('#BuyerConfirmationModel_SpouseLastName').prop('required', true);
        }
        else {
            $('#BuyerConfirmationModel_SpouseFirstName').prop('required', false);
            $('#BuyerConfirmationModel_SpouseLastName').prop('required', false);
        }
    });

    //#endregion

    $(function () {
        $("#frm_hlf068 input, #frm_hlf068 select, #frm_hlf068 textarea").attr("readonly", true).addClass("disabled");

        $(`#frm_hlf068 input[type="checkbox"]`).attr("disabled", true);
        $('.calendarpicker, .timepicker, .present-calendar-picker').prop('disabled', true);
        $('input[type="radio"]').prop('disabled', true);

        $('#frm_hlf068').find('.selectized').each(function (i, e) {
            e.selectize.lock();
        });
        initializeRadioBtnMisc();

        if (hasBcf == "False") {
            $(`.radio-incomeSrcRbtn [name="incomeSrcRbtn"]`).on(`change`, function (e) {


                $(`[id="bcf-incomeFields"]`).attr({
                    hidden: $(this).attr('id') === 'isRbtn2',
                });

                $(`[id="bcf-incomeFields"] input[type="text"]`).attr({
                    required: $(this).attr('id') === 'isRbtn1',
                });

                if ($(this).attr('id') === 'isRbtn2') {
                    //$(`[id="bcf-incomeFields"] input[type="text"]`).val('');

                    $("#BuyerConfirmationModel_AdditionalSourceIncome").val(null);
                    $("#BuyerConfirmationModel_AverageMonthlyAdditionalIncome").val(0);
                }

                $(`[name="BuyerConfirmationModel.IsOtherSourceOfIncome"]`).attr('value', $(this).attr('id') === 'isRbtn1');
            });

            $(`.radio-pagibigRbtn input[name="pagibigRbtn"]`).on(`change`, function (e) {
                $(`[id="bcf-pagIbigNumField"]`).attr({
                    hidden: $(this).attr('id') === 'pagibigRbtn2',
                });

                $(`[id="bcf-pagIbigNumField"] [id="BuyerConfirmationModel_PagibigNumber"]`).attr({
                    required: $(this).attr('id') === 'pagibigRbtn1',
                });

                //if ($(this).attr('id') === 'pagibigRbtn2') {
                //    $(`[id="bcf-pagIbigNumField"] input[type="text"]`).val("");
                //}

                $(`[name="BuyerConfirmationModel.IsPagibigMember"]`).attr('value', $(this).attr('id') === 'pagibigRbtn1');
            });

            $(`.radio-availedLoanRbtn input[name="availedLoanRbtn"]`).on(`change`, function (e) {
                $(`[name="BuyerConfirmationModel.IsPagibigAvailedLoan"]`).attr('value', $(this).attr('id') === 'availedLoanRbtn1');
            });

            $(`.radio-cbwrRbtn input[name="coBorrowerRbtn"]`).on(`change`, function (e) {
                $(`[name="BuyerConfirmationModel.IsPagibigCoBorrower"]`).attr('value', $(this).attr('id') === 'cbwrRbtn1');
            });

            $(`.radio-prpRbtn input[name="projectPropRbtn"]`).on(`change`, function (e) {
                $(`[name="BuyerConfirmationModel.IsPursueProjectProponent"]`).attr('value', $(this).attr('id') === 'prpRbtn1');
            });

            $(`.radio-itcRbtn input[name="informedTermsRbtn"]`).on(`change`, function (e) {
                $(`[name="BuyerConfirmationModel.IsInformedTermsConditions"]`).attr('value', $(this).attr('id') === 'itcRbtn1');
            });

            //Load the possible data on others forms
            //HLafTobcfConnectedFieldMap();
            //bcfToHLafConnectedFieldMap();
        }
    });

    //#region Methods

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

        //form.find('input[type="radio"][required]').each(function () {
        //    let hasClass = $(this).hasClass('valid');

        //    console.log(roleId);
        //    if (roleId === '3') {
        //        $(this).prop('required', false);
        //        return;
        //    }

        //    if (!hasClass) {
        //        $(this).addClass('is-invalid');
        //        $(this).removeClass('valid');

        //        isValid = false;
        //    } else {
        //        $(this).addClass('valid');
        //        $(this).removeClass('is-invalid');
        //    }
        //});

        return isValid;
    }

    function rebindValidators() {
        $("#BuyerConfirmationModel_HouseUnitModel").attr("disabled", false);

        let $form = $("#frm_hlf068");
        let button = $("#btn_savehlf068");

        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate($form.data("unobtrusiveValidation").options);
        $form.data("validator").settings.ignore = "";

        // Prevent form submission when "Enter" key is pressed
        $form.on("keydown", function (e) {
            if (e.key === "Enter") {
                e.preventDefault();
            }
        });

        $form.on("submit", function (e) {
            e.preventDefault();

            // re-enable checkboxes on submission
            // Important: this snippet should come first before validation and FormData varialble
            $(`#frm_hlf068 input[type="checkbox"]`).removeAttr("disabled").prop('disabled', false);
            $("#frm_hlf068 input, #frm_hlf068 select, #frm_hlf068 textarea").removeAttr("readonly");
            $('.calendarpicker, .timepicker, .present-calendar-picker').prop('disabled', false);
            $('input[type="radio"]').prop('disabled', false);

            if (!$(this).valid()) {
                messageBox("Please fill out all required fields!", "danger", true);
                return;
            }

            let formData = new FormData(e.target);

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
                    $("#BuyerConfirmationModel_HouseUnitModel").attr("disabled", true);

                    // Success message handling
                    let recordId = $("input[name='User.Id']").val();
                    console.log(recordId);
                    let type = (recordId == 0 ? "Added!" : "Updated!");
                    let successMessage = `Application Successfully ${type}`;
                    messageBox(successMessage, "success", true);

                    // Redirect handling
                    if (applicantInfoIdVal == 0) {
                        setTimeout(function () {
                            $("#beneficiary-overlay").addClass('d-none');
                            window.location.href = "/Applicants/HLF068/" + response;
                        }, 2000);
                    } else {
                        var link = "Applicants/Beneficiary";

                        setTimeout(function () {
                            $("#beneficiary-overlay").addClass('d-none');
                            // Redirect to the specified location
                            window.location.href = baseUrl + link;
                        }, 2000);
                    }
                    // Reset button state
                    button.attr({ disabled: false });
                    button.html("<span class='mdi mdi-content-save-outline'></span> Save");

                    editableFlag = false;
                },
                error: function (response) {
                    // Error message handling
                    messageBox(response.responseText, "danger");
                    $("#beneficiary-overlay").addClass('d-none');
                    button.html("<span class='mdi mdi-content-save-outline'></span> Save");
                    button.attr({ disabled: false });
                }
            });

            // Use SweetAlert for confirmation
            //Swal.fire({
            //    title: 'Are you sure?',
            //    text: "You are about to submit the form. Proceed?",
            //    icon: 'warning',
            //    showCancelButton: true,
            //    confirmButtonColor: '#3085d6',
            //    cancelButtonColor: '#d33',
            //    confirmButtonText: 'Yes, submit',
            //    cancelButtonText: 'No, cancel'
            //}).then((result) => {
            //    if (result.isConfirmed) {
            //        // User confirmed, proceed with form submission
            //        $.ajax({
            //            url: $(this).attr("action"),
            //            method: $(this).attr("method"),
            //            data: formData,
            //            cache: false,
            //            contentType: false,
            //            processData: false,
            //            beforeSend: function () {
            //                button.html("<span class='spinner-border spinner-border-sm'></span> Saving...");
            //                button.attr({ disabled: true });
            //                $("#beneficiary-overlay").removeClass('d-none');
            //            },
            //            success: function (response) {
            //                // Success message handling
            //                let recordId = $("input[name='User.Id']").val();
            //                console.log(recordId);
            //                let type = (recordId == 0 ? "Added!" : "Updated!");
            //                let successMessage = `Beneficiary Successfully ${type}`;
            //                messageBox(successMessage, "success", true);

            //                // Redirect handling
            //                if (applicantInfoIdVal == 0) {
            //                    setTimeout(function () {
            //                        $("#beneficiary-overlay").addClass('d-none');
            //                        window.location.href = "/Applicants/HLF068/" + response;
            //                    }, 2000);
            //                } else {
            //                    var link = "Applicants/Beneficiary";
            //                    if (roleName != 'Beneficiary') {
            //                        link = "Applicants/ApplicantRequests";
            //                    }
            //                    setTimeout(function () {
            //                        $("#beneficiary-overlay").addClass('d-none');
            //                        // Redirect to the specified location
            //                        window.location.href = baseUrl + link;
            //                    }, 2000);
            //                }
            //                // Reset button state
            //                button.attr({ disabled: false });
            //                button.html("<span class='mdi mdi-content-save-outline'></span> Submit");
            //            },
            //            error: function (response) {
            //                // Error message handling
            //                messageBox(response.responseText, "danger");
            //                $("#beneficiary-overlay").addClass('d-none');
            //                button.html("<span class='mdi mdi-content-save-outline'></span> Submit");
            //                button.attr({ disabled: false });
            //            }
            //        });
            //    }
            //});
        });
    }

    function setDateValue(selector) {
        let dataValue = $(selector).attr('data-value');
        if (dataValue && dataValue.trim() !== '') {
            $(selector).val(moment(dataValue).format("MM/DD/YYYY"));
        }
    }

    function lengthValidator() {
        var isValid = true;
        var elements = [
            { name: 'ApplicantsPersonalInformationModel.PagibigNumber', requiredLength: 12, message: "Mobile number" },
            //{ name: 'Company.TelNo', requiredLength: 8, message: "Telephone number" },
            //{ name: 'Company.FaxNo', requiredLength: 10, message: "Fax number" },
            //{ name: 'Company.Tin', requiredLength: 13, message: "TIN number" },
            //{ name: 'Company.RepresentativeTin', requiredLength: 13, message: "Company Representative TIN number" }
        ];

        elements.forEach(function (element) {
            var inputValue = $("[name='" + element.name + "']").inputmask('unmaskedvalue'); // Get the unmasked value
            var alphanumericLength = inputValue.replace(/[^a-zA-Z0-9]/g, '').length; // Count only alphanumeric characters
            if (alphanumericLength !== element.requiredLength) {
                $("[data-valmsg-for='" + element.name + "']").text(element.message + " length should be " + element.requiredLength + " characters.");
                isValid = false;
            } else {
                $("[data-valmsg-for='" + element.name + "']").text("");
                isValid = isValid != false ? true : false;
            }
        });

        return isValid;
    }

    function initializeLoanCreditDate() {
        const dateFormat = "Y-m-d";
        var currentDate = moment().format("YYYY-MM-DD");

        $('[id^="Form2PageModel_DateObtained"]').flatpickr({
            dateFormat: dateFormat,
            maxDate: currentDate,
            onChange: function (selectedDates, dateStr, instance) {
                let fullyPaidId = instance.input.id.replace("DateObtained", "DateFullyPaid");

                if (dateStr === '') {
                    $(`#${fullyPaidId}`).val("");
                    return;
                }

                $(`#${fullyPaidId}`).flatpickr({
                    dateFormat: dateFormat,
                    minDate: dateStr,
                    onChange: function (selectedDatesArr, dateString, instance1) {
                        let obtainedId = instance1.input.id.replace("DateFullyPaid", "DateObtained");
                        let obtVal = $(`#${obtainedId}`).val();
                        console.log(obtVal);
                        if (obtVal === '') {
                            $(`#${instance1.input.id}`).val("");
                            return;
                        }
                    }
                });
            }
        });

        $('[id^="Form2PageModel_DateFullyPaid"]').flatpickr({
            dateFormat: dateFormat,
            minDate: currentDate,
            maxDate: currentDate,
            onChange: function (selectedDates, dateStr, instance) {
                let obtainedId = instance.input.id.replace("DateFullyPaid", "DateObtained");
                let obtVal = $(`#${obtainedId}`).val();
                console.log(obtVal);
                if (obtVal === '') {
                    $(`#${instance.input.id}`).val("");
                    return;
                }
            }
        });
    }

    function initializeIntlTelInput() {
        //var homeNum = intlTelInput(document.getElementById(`BarrowersInformationModel_HomeNumber`), intlTelConfig);
        //var mobileNum = intlTelInput(document.getElementById(`BarrowersInformationModel_MobileNumber`), intlTelConfig);
        //var businessDirectLineNum = intlTelInput(document.getElementById(`BarrowersInformationModel_BusinessDirectLineNumber`), intlTelConfig);
        //var businessTruckLineNum = intlTelInput(document.getElementById(`BarrowersInformationModel_BusinessTruckLineNumber`), intlTelConfig);
        //var businessTelNum = intlTelInput(document.getElementById(`SpouseModel_BusinessTelNo`), intlTelConfig);

        telNoArray.push(intlTelInput(document.getElementsByName(`BarrowersInformationModel.HomeNumber`)[0], intlTelConfig));
        telNoArray.push(intlTelInput(document.getElementsByName(`BarrowersInformationModel.MobileNumber`)[0], intlTelConfig));
        telNoArray.push(intlTelInput(document.getElementsByName(`BarrowersInformationModel.BusinessDirectLineNumber`)[0], intlTelConfig));
        telNoArray.push(intlTelInput(document.getElementsByName(`BarrowersInformationModel.BusinessTruckLineNumber`)[0], intlTelConfig));
        telNoArray.push(intlTelInput(document.getElementsByName(`SpouseModel.BusinessTelNo`)[0], intlTelConfig));

        $.each($(`input[name^="Form2PageModel.TradeTellNo"]`), function (i, element) {
            let elem = intlTelInput(element, intlTelConfig);
            telNoArray.push(elem);
        });

        $.each($(`input[name^="Form2PageModel.CharacterTellNo"]`), function (i, element) {
            let elem = intlTelInput(element, intlTelConfig);
            telNoArray.push(elem);
        });

        console.log(telNoArray);

        // apply validation
        for (var index in telNoArray) {
            let itiElement = telNoArray[index];

            $(`[name="${itiElement.a.name}"]`).on('input', function () {
                let id = $(this).attr('id');
                let itiInstance = window.intlTelInputGlobals.getInstance(document.getElementById(id));

                console.log(itiInstance.isValidNumberPrecise());

                if (!itiInstance.isValidNumberPrecise() && (itiInstance.a.hasAttribute('required') || itiInstance.a.value)) {
                    console.log(itiInstance.getValidationError());
                    $(`span[name="${itiInstance.a.name}.Error"]`).html(intlTelErrors[itiInstance.getValidationError()]);
                }
                else {
                    $(`span[name="${itiInstance.a.name}.Error"]`).html("");
                }
            });
        }

        //itiFlag = true;
    }

    function initializeBasicTelInput() {
        $(`[name="BarrowersInformationModel.HomeNumber"]`).inputmask({ regex: `^[0-9+-]*$` /*, mask: `(+9{1,}) 9{1,}`*/ });
        $(`[name="BarrowersInformationModel.MobileNumber"]`).inputmask({ regex: `^[0-9+-]*$` /*, mask: `(+9{1,}) 9{1,}`*/ });
        $(`[name="BarrowersInformationModel.BusinessDirectLineNumber"]`).inputmask({ regex: `^[0-9+-]*$` /*, mask: `(+9{1,}) 9{1,}`*/ });
        $(`[name="BarrowersInformationModel.BusinessTruckLineNumber"]`).inputmask({ regex: `^[0-9+-]*$` /*, mask: `(+9{1,}) 9{1,}`*/ });
        $(`[name="SpouseModel.BusinessTelNo"]`).inputmask({ regex: `^[0-9+-]*$` /*, mask: `(+9{1,}) 9{1,}`*/ });
        $(`[name^="Form2PageModel.TradeTellNo"]`).inputmask({ regex: `^[0-9+-]*$` });
        $(`[name^="Form2PageModel.CharacterTellNo"]`).inputmask({ regex: `^[0-9+-]*$` });
    }

    function initializeRadioBtnMisc() {
        let pendingCaseValue = $("[name='Form2PageModel.PendingCase']").val();
        let pastDueValue = $("[name='Form2PageModel.PastDue']").val();
        let bouncingChecksValue = $("[name='Form2PageModel.BouncingChecks']").val();
        let medicalAdviceValue = $("[name='Form2PageModel.MedicalAdvice']").val();

        let pagibigAvailedLoan = $("[name='BuyerConfirmationModel.IsPagibigAvailedLoan']").val();

        let coborrower = $("[name='BuyerConfirmationModel.IsPagibigCoBorrower']").val();

        let projectProponent = $("[name='BuyerConfirmationModel.IsPursueProjectProponent']").val();
        let termConditions = $("[name='BuyerConfirmationModel.IsInformedTermsConditions']").val();

        //let isPagibigMember = $("[name='BuyerConfirmationModel.IsPagibigMember']").val();
        let isOtherSourceIncome = $("[name='BuyerConfirmationModel.IsOtherSourceOfIncome']").val();

        let bcfPagibigNumber = $("#BuyerConfirmationModel_PagibigNumber").val();;
        let bcfAdditionalSourceIncome = $("#BuyerConfirmationModel_AdditionalSourceIncome").val();
        let bcfAverageMonthlyAddIncome = $("#BuyerConfirmationModel_AverageMonthlyAdditionalIncome").val();

        // If pagibigAvailedLoan has a value of "1", set the radio button as checked
        //if (pagibigAvailedLoan === "True") {
        //    $("#availedLoanRbtn1").prop("checked", true);
        //} else {
        //    $("#availedLoanRbtn2").prop("checked", true);
        //}

        // If pagibigAvailedLoan has a value of "1", set the radio button as checked
        //if (coborrower === "True") {
        //    $("#cbwrRbtn1").prop("checked", true);
        //} else {
        //    $("#cbwrRbtn2").prop("checked", true);
        //}

        // If pagibigAvailedLoan has a value of "1", set the radio button as checked
        //if (projectProponent === "True") {
        //    $("#prpRbtn1").prop("checked", true);
        //} else {
        //    $("#prpRbtn2").prop("checked", true);
        //}

        // If pagibigAvailedLoan has a value of "1", set the radio button as checked
        //if (termConditions === "True") {
        //    $("#itcRbtn1").prop("checked", true);
        //} else {
        //    $("#itcRbtn2").prop("checked", true);
        //}

        // If pagibigAvailedLoan has a value of "1", set the radio button as checked
        //if (isPagibigMember === "True") {
        //    $("#pagibigRbtn1").prop("checked", true);
        //} else {
        //    $("#pagibigRbtn2").prop("checked", true);
        //}

        //if (isOtherSourceIncome === "True") {
        //    $("#isRbtn1").prop("checked", true);
        //} else {
        //    $("#isRbtn2").prop("checked", true);
        //}

        // Set checked status for PendingCase radio buttons
        $("#pcRadioBtn1").prop("checked", !!pendingCaseValue);
        $("#pcRadioBtn2").prop("checked", !pendingCaseValue);

        if (applicantInfoIdVal !== '0') {
            // Set checked status for PendingCase radio buttons
            $("#pcRadioBtn1").prop("checked", !!pendingCaseValue);
            $("#pcRadioBtn2").prop("checked", !pendingCaseValue);

            // Set checked status for PastDue radio buttons
            $("#pdRbtn1").prop("checked", !!pastDueValue);
            $("#pdRbtn2").prop("checked", !pastDueValue);

            // Set checked status for BouncingChecks radio buttons
            $("#bcRbtn1").prop("checked", !!bouncingChecksValue);
            $("#bcRbtn2").prop("checked", !bouncingChecksValue);

            // Set checked status for MedicalAdvice radio buttons
            $("#maRbtn1").prop("checked", !!medicalAdviceValue);
            $("#maRbtn2").prop("checked", !medicalAdviceValue);

            // Set checked status for BCF Additional income radio buttons
            $("#isRbtn1").prop("checked", !!bcfAdditionalSourceIncome);
            $("#isRbtn2").prop("checked", !bcfAdditionalSourceIncome);

            // Set checked status for BCF Pagibig Number radio buttons
            $("#pagibigRbtn1").prop("checked", !!bcfPagibigNumber);
            $("#pagibigRbtn2").prop("checked", !bcfPagibigNumber);

            // Set checked status for BCF availed laon radio buttons
            $("#availedLoanRbtn1").prop("checked", !pagibigAvailedLoan);
            $("#availedLoanRbtn2").prop("checked", pagibigAvailedLoan);

            // Set checked status for BCF co-borrower radio buttons
            $("#cbwrRbtn1").prop("checked", !coborrower);
            $("#cbwrRbtn2").prop("checked", coborrower);

            // Set checked status for BCF co-borrower radio buttons
            $("#prpRbtn1").prop("checked", !projectProponent);
            $("#prpRbtn2").prop("checked", projectProponent);

            // Set checked status for BCF term in condition radio buttons
            $("#itcRbtn1").prop("checked", !termConditions);
            $("#itcRbtn2").prop("checked", termConditions);
        }

        // Set miscellanous input to disable
        $("[name='Form2PageModel.PendingCase']").prop("disabled", !pendingCaseValue);
        $("[name='Form2PageModel.PastDue']").prop("disabled", !pastDueValue);
        $("[name='Form2PageModel.BouncingChecks']").prop("disabled", !bouncingChecksValue);
        $("[name='Form2PageModel.MedicalAdvice']").prop("disabled", !medicalAdviceValue);

        //BCF Particulars
        $("#bcf-incomeFields").prop("hidden", !bcfAdditionalSourceIncome);
        $("#bcf-pagIbigNumField").prop("hidden", !bcfPagibigNumber);
    }

    function bcfToHLafConnectedFieldMap() {
        if (hasBcf == "True" || buyerconfirmationAppStatus == 3) {
            return;
        }

        var lastName = $("#BuyerConfirmationModel_LastName").val();
        var FirstName = $("#BuyerConfirmationModel_FirstName").val();
        var MiddleName = $("#BuyerConfirmationModel_MiddleName").val();
        var Suffix = $("#BuyerConfirmationModel_Suffix").val();
        var BirthDate = $("#BuyerConfirmationModel_BirthDate").val();

        var MaritalStatus = $('#BuyerConfirmationModel_MaritalStatus')[0].selectize.getValue();

        $("#BarrowersInformationModel_LastName").val(lastName);
        $("#BarrowersInformationModel_FirstName").val(FirstName);
        $("#BarrowersInformationModel_MiddleName").val(MiddleName);
        $("#BarrowersInformationModel_Suffix").val(Suffix);
        $("#BarrowersInformationModel_BirthDate").val(BirthDate);
        $('#BarrowersInformationModel_MaritalStatus')[0].selectize.setValue(MaritalStatus);

        var homeNumber = $("#BuyerConfirmationModel_HomeNumber").val();
        var mobileNumber = $("#BuyerConfirmationModel_MobileNumber").val();
        var email = $("#BuyerConfirmationModel_Email").val();

        $("#BarrowersInformationModel_HomeNumber").val(homeNumber);
        $("#BarrowersInformationModel_MobileNumber").val(mobileNumber);

        $("#BarrowersInformationModel_Email").val(email);

        // Accessing values of all fields in the model
        var presentUnitName = $("#BuyerConfirmationModel_PresentUnitName").val();
        var presentBuildingName = $("#BuyerConfirmationModel_PresentBuildingName").val();
        var presentLotName = $("#BuyerConfirmationModel_PresentLotName").val();
        var presentStreetName = $("#BuyerConfirmationModel_PresentStreetName").val();
        var presentSubdivisionName = $("#BuyerConfirmationModel_PresentSubdivisionName").val();
        var presentBaranggayName = $("#BuyerConfirmationModel_PresentBaranggayName").val();
        var presentMunicipalityName = $("#BuyerConfirmationModel_PresentMunicipalityName").val();
        var presentProvinceName = $("#BuyerConfirmationModel_PresentProvinceName").val();
        var presentZipCode = $("#BuyerConfirmationModel_PresentZipCode").val();

        // Update properties of the BarrowersInformationModel directly
        $("#BarrowersInformationModel_PresentUnitName").val(presentUnitName);
        $("#BarrowersInformationModel_PresentBuildingName").val(presentBuildingName);
        $("#BarrowersInformationModel_PresentLotName").val(presentLotName);
        $("#BarrowersInformationModel_PresentStreetName").val(presentStreetName);
        $("#BarrowersInformationModel_PresentSubdivisionName").val(presentSubdivisionName);
        $("#BarrowersInformationModel_PresentBaranggayName").val(presentBaranggayName);
        $("#BarrowersInformationModel_PresentMunicipalityName").val(presentMunicipalityName);
        $("#BarrowersInformationModel_PresentProvinceName").val(presentProvinceName);
        $("#BarrowersInformationModel_PresentZipCode").val(presentZipCode);

        var employername = $("#BuyerConfirmationModel_EmployerName").val();

        $("#BarrowersInformationModel_EmployerName").val(employername);

        var spouseLastName = $('#BuyerConfirmationModel_SpouseLastName').val();
        var spouseFirstName = $('#BuyerConfirmationModel_SpouseFirstName').val();
        var spouseExtensionName = $('#BuyerConfirmationModel_SpouseSuffix').val();
        var spouseMiddleName = $('#BuyerConfirmationModel_SpouseMiddleName').val();

        $('#SpouseModel_LastName').val(spouseLastName);
        $('#SpouseModel_FirstName').val(spouseFirstName);
        $('#SpouseModel_Suffix').val(spouseExtensionName);
        $('#SpouseModel_MiddleName').val(spouseMiddleName);

        var spouseEmploymentUnit = $('#BuyerConfirmationModel_SpouseCompanyUnitName').val();
        var spouseCompanyBuilding = $('#BuyerConfirmationModel_SpouseCompanyBuildingName').val();
        var spouseCompanyLot = $('#BuyerConfirmationModel_SpouseCompanyLotName').val();
        var spouseCompanyStreet = $('#BuyerConfirmationModel_SpouseCompanyStreetName').val();
        var spouseCompanySubdivision = $('#BuyerConfirmationModel_SpouseCompanySubdivisionName').val();
        var spouseCompanyBaranggay = $('#BuyerConfirmationModel_SpouseCompanyBaranggayName').val();
        var spouseCompanyMunicipality = $('#BuyerConfirmationModel_SpouseCompanyMunicipalityName').val();
        var spouseCompanyProvince = $('#BuyerConfirmationModel_SpouseCompanyProvinceName').val();
        var spouseCompanyZipcode = $('#BuyerConfirmationModel_SpouseCompanyZipCode').val();

        $('#SpouseModel_SpouseEmploymentUnitName').val(spouseEmploymentUnit);
        $('#SpouseModel_SpouseEmploymentBuildingName').val(spouseCompanyBuilding);
        $('#SpouseModel_SpouseEmploymentLotName').val(spouseCompanyLot);
        $('#SpouseModel_SpouseEmploymentStreetName').val(spouseCompanyStreet);
        $('#SpouseModel_SpouseEmploymentSubdivisionName').val(spouseCompanySubdivision);
        $('#SpouseModel_SpouseEmploymentBaranggayName').val(spouseCompanyBaranggay);
        $('#SpouseModel_SpouseEmploymentMunicipalityName').val(spouseCompanyMunicipality);
        $('#SpouseModel_SpouseEmploymentProvinceName').val(spouseCompanyProvince);
        $('#SpouseModel_SpouseEmploymentZipCode').val(spouseCompanyZipcode);

        var companyUnit = $('#BuyerConfirmationModel_CompanyUnitName').val();
        var companyBldgName = $('#BuyerConfirmationModel_CompanyBuildingName').val();
        var companyLotNo = $('#BuyerConfirmationModel_CompanyLotName').val();
        var companySreetName = $('#BuyerConfirmationModel_CompanyStreetName').val();
        var companySubd = $('#BuyerConfirmationModel_CompanySubdivisionName').val();
        var companyBrgy = $('#BuyerConfirmationModel_CompanyBaranggayName').val();
        var companyMuni = $('#BuyerConfirmationModel_CompanyMunicipalityName').val();
        var companyProv = $('#BuyerConfirmationModel_CompanyProvinceName').val();
        var companyZipcode = $('#BuyerConfirmationModel_CompanyZipCode').val();

        $('#BarrowersInformationModel_BusinessUnitName').val(companyUnit);
        $('#BarrowersInformationModel_BusinessBuildingName').val(companyBldgName);
        $('#BarrowersInformationModel_BusinessLotName').val(companyLotNo);
        $('#BarrowersInformationModel_BusinessStreetName').val(companySreetName);
        $('#BarrowersInformationModel_BusinessSubdivisionName').val(companySubd);
        $('#BarrowersInformationModel_BusinessBaranggayName').val(companyBrgy);
        $('#BarrowersInformationModel_BusinessMunicipalityName').val(companyMuni);
        $('#BarrowersInformationModel_BusinessProvinceName').val(companyProv);
        $('#BarrowersInformationModel_BusinessZipCode').val(companyZipcode);

        var pagibigNo = $('#BuyerConfirmationModel_PagibigNumber').val();
        $("#ApplicantsPersonalInformationModel_PagibigNumber").val(pagibigNo);
    }

    function HLafTobcfConnectedFieldMap() {
        if (hasBcf == "True" || buyerconfirmationAppStatus == 3) {
            return;
        }

        var lastName = $("#BarrowersInformationModel_LastName").val();
        var FirstName = $("#BarrowersInformationModel_FirstName").val();
        var MiddleName = $("#BarrowersInformationModel_MiddleName").val();
        var Suffix = $("#BarrowersInformationModel_Suffix").val();
        var BirthDate = $("#BarrowersInformationModel_BirthDate").val();

        var MaritalStatus = $('#BarrowersInformationModel_MaritalStatus')[0].selectize.getValue();

        $("#BuyerConfirmationModel_LastName").val(lastName);
        $("#BuyerConfirmationModel_FirstName").val(FirstName);
        $("#BuyerConfirmationModel_MiddleName").val(MiddleName);
        $("#BuyerConfirmationModel_Suffix").val(Suffix);
        $("#BuyerConfirmationModel_BirthDate").val(BirthDate);
        $('#BuyerConfirmationModel_MaritalStatus').data("selectize").setValue(MaritalStatus);

        var homeNumber = $("#BarrowersInformationModel_HomeNumber").val();
        var mobileNumber = $("#BarrowersInformationModel_MobileNumber").val();
        var email = $("#BarrowersInformationModel_Email").val();

        $("#BuyerConfirmationModel_HomeNumber").val(homeNumber);
        $("#BuyerConfirmationModel_MobileNumber").val(mobileNumber);
        $("#BuyerConfirmationModel_Email").val(email);

        // Accessing values of all fields in the model
        var presentUnitName = $("#BarrowersInformationModel_PresentUnitName").val();
        var presentBuildingName = $("#BarrowersInformationModel_PresentBuildingName").val();
        var presentLotName = $("#BarrowersInformationModel_PresentLotName").val();
        var presentStreetName = $("#BarrowersInformationModel_PresentStreetName").val();
        var presentSubdivisionName = $("#BarrowersInformationModel_PresentSubdivisionName").val();
        var presentBaranggayName = $("#BarrowersInformationModel_PresentBaranggayName").val();
        var presentMunicipalityName = $("#BarrowersInformationModel_PresentMunicipalityName").val();
        var presentProvinceName = $("#BarrowersInformationModel_PresentProvinceName").val();
        var presentZipCode = $("#BarrowersInformationModel_PresentZipCode").val();

        // Update properties of the BarrowersInformationModel directly
        $("#BuyerConfirmationModel_PresentUnitName").val(presentUnitName);
        $("#BuyerConfirmationModel_PresentBuildingName").val(presentBuildingName);
        $("#BuyerConfirmationModel_PresentLotName").val(presentLotName);
        $("#BuyerConfirmationModel_PresentStreetName").val(presentStreetName);
        $("#BuyerConfirmationModel_PresentSubdivisionName").val(presentSubdivisionName);
        $("#BuyerConfirmationModel_PresentBaranggayName").val(presentBaranggayName);
        $("#BuyerConfirmationModel_PresentMunicipalityName").val(presentMunicipalityName);
        $("#BuyerConfirmationModel_PresentProvinceName").val(presentProvinceName);
        $("#BuyerConfirmationModel_PresentZipCode").val(presentZipCode);

        var employername = $("#BarrowersInformationModel_EmployerName").val();

        $("#BuyerConfirmationModel_EmployerName").val(employername);

        var spouseLastName = $('#SpouseModel_LastName').val();
        var spouseFirstName = $('#SpouseModel_FirstName').val();
        var spouseExtensionName = $('#SpouseModel_Suffix').val();
        var spouseMiddleName = $('#SpouseModel_MiddleName').val();

        $('#BuyerConfirmationModel_SpouseLastName').val(spouseLastName);
        $('#BuyerConfirmationModel_SpouseFirstName').val(spouseFirstName);
        $('#BuyerConfirmationModel_SpouseSuffix').val(spouseExtensionName);
        $('#BuyerConfirmationModel_SpouseMiddleName').val(spouseMiddleName);

        var spouseEmploymentUnit = $('#SpouseModel_SpouseEmploymentUnitName').val();
        var spouseCompanyBuilding = $('#SpouseModel_SpouseEmploymentBuildingName').val();
        var spouseCompanyLot = $('#SpouseModel_SpouseEmploymentLotName').val();
        var spouseCompanyStreet = $('#SpouseModel_SpouseEmploymentStreetName').val();
        var spouseCompanySubdivision = $('#SpouseModel_SpouseEmploymentSubdivisionName').val();
        var spouseCompanyBaranggay = $('#SpouseModel_SpouseEmploymentBaranggayName').val();
        var spouseCompanyMunicipality = $('#SpouseModel_SpouseEmploymentMunicipalityName').val();
        var spouseCompanyProvince = $('#SpouseModel_SpouseEmploymentProvinceName').val();
        var spouseCompanyZipcode = $('#SpouseModel_SpouseEmploymentZipCode').val();

        $('#BuyerConfirmationModel_SpouseCompanyUnitName').val(spouseEmploymentUnit);
        $('#BuyerConfirmationModel_SpouseCompanyBuildingName').val(spouseCompanyBuilding);
        $('#BuyerConfirmationModel_SpouseCompanyLotName').val(spouseCompanyLot);
        $('#BuyerConfirmationModel_SpouseCompanyStreetName').val(spouseCompanyStreet);
        $('#BuyerConfirmationModel_SpouseCompanySubdivisionName').val(spouseCompanySubdivision);
        $('#BuyerConfirmationModel_SpouseCompanyBaranggayName').val(spouseCompanyBaranggay);
        $('#BuyerConfirmationModel_SpouseCompanyMunicipalityName').val(spouseCompanyMunicipality);
        $('#BuyerConfirmationModel_SpouseCompanyProvinceName').val(spouseCompanyProvince);
        $('#BuyerConfirmationModel_SpouseCompanyZipCode').val(spouseCompanyZipcode);

        var companyUnit = $('#BarrowersInformationModel_BusinessUnitName').val();
        var companyBldgName = $('#BarrowersInformationModel_BusinessBuildingName').val();
        var companyLotNo = $('#BarrowersInformationModel_BusinessLotName').val();
        var companySreetName = $('#BarrowersInformationModel_BusinessStreetName').val();
        var companySubd = $('#BarrowersInformationModel_BusinessSubdivisionName').val();
        var companyBrgy = $('#BarrowersInformationModel_BusinessBaranggayName').val();
        var companyMuni = $('#BarrowersInformationModel_BusinessMunicipalityName').val();
        var companyProv = $('#BarrowersInformationModel_BusinessProvinceName').val();
        var companyZipcode = $('#BarrowersInformationModel_BusinessZipCode').val();

        $('#BuyerConfirmationModel_CompanyUnitName').val(companyUnit);
        $('#BuyerConfirmationModel_CompanyBuildingName').val(companyBldgName);
        $('#BuyerConfirmationModel_CompanyLotName').val(companyLotNo);
        $('#BuyerConfirmationModel_CompanyStreetName').val(companySreetName);
        $('#BuyerConfirmationModel_CompanySubdivisionName').val(companySubd);
        $('#BuyerConfirmationModel_CompanyBaranggayName').val(companyBrgy);
        $('#BuyerConfirmationModel_CompanyMunicipalityName').val(companyMuni);
        $('#BuyerConfirmationModel_CompanyProvinceName').val(companyProv);
        $('#BuyerConfirmationModel_CompanyZipCode').val(companyZipcode);

        var pagibigNo = $('#ApplicantsPersonalInformationModel_PagibigNumber').val();

        if (pagibigNo) {
            $("#pagibigRbtn1").prop("checked", true);
            $("#bcf-pagIbigNumField").prop("hidden", false);
            $("#BuyerConfirmationModel_PagibigNumber").val(pagibigNo);
        }
    }

    function progressCheck(targetForm = "bcfdata") {
        console.log("execute");
        if (targetForm === "bcfdata" && $(`#bcfdata`).length === 0) {
            targetForm = "loanparticulars";
        }

        var steps = $(".progressbar .progress-step");

        let stepIndexArray = {
            2: [
                {
                    formId: ["loanparticulars", "collateraldata", "spousedata", "form2"],
                    progress: 0
                },
                {
                    formId: ["previewHlaf"],
                    progress: 1
                }
            ],
            4: [
                {
                    formId: ["bcfdata"],
                    progress: 0
                },
                {
                    formId: ["loanparticulars", "collateraldata", "spousedata", "form2"],
                    progress: 1
                },
                {
                    formId: ["previewBcf"],
                    progress: 2
                },
                {
                    formId: ["previewHlaf"],
                    progress: 3
                }
            ],
        };

        let stepIndex = stepIndexArray[steps.length];

        currentStep = stepIndex.find(a => a.formId.includes(targetForm)).progress;

        steps.each((index, step) => {
            if (index === currentStep) {
                step.classList.remove("completed");
                step.classList.add("current");
            }
            else if (index < currentStep) {
                step.classList.add("completed");
            }
            else {
                step.classList.remove("current");
                step.classList.remove("completed");
            }
        });

        const allCurrentClasses = document.querySelectorAll(".progressbar .completed");

        let width = ((allCurrentClasses.length / (steps.length - 1)) * 99) + allCurrentClasses.length;

        if (width > 99) width = 99;

        $(`.progressbar #progress`).css('width', `${width}%`);
    }

    function initializePdfJs() {
        pdfjsLib.GlobalWorkerOptions.workerSrc = baseUrl + 'lib/pdfjs-dist/build/pdf.worker.mjs';
    }

    function loadBcfPreview() {
        //$("#BuyerConfirmationModel_HouseUnitModel").attr("disabled", false);
        //$("#BuyerConfirmationModel_BirthDate").attr("disabled", false);

        //$("#BuyerConfirmationModel_SellingPrice").attr("disabled", false);
        //$("#BuyerConfirmationModel_MonthlyAmortization").attr("disabled", false);


        //enable bcf field even approve temporarily to mapped in pdf
        enableBuyerConfirmationFields();

        var formData = new FormData(document.querySelector(`#frm_hlf068`));

        $.ajax({
            method: 'POST',
            url: '/Report/LatestBCFB64',
            data: formData, // Convert to JSON string
            contentType: 'application/json', // Set content type to JSON,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function () {
                $("#beneficiary-overlay").removeClass('d-none');
            },
            success: function (response) {

                
                $("#BuyerConfirmationModel_HouseUnitModel").attr("disabled", true);
                $("#BuyerConfirmationModel_BirthDate").attr("disabled", true);

                $("#BuyerConfirmationModel_SellingPrice").attr("disabled", true);
                $("#BuyerConfirmationModel_MonthlyAmortization").attr("disabled", true);

                // Redirect to another URL based on the response
                //window.location.href = '/Report/LatestHousingForm2';

                // Handle success response
                //console.log(response);
                // Do something with the response, like displaying a success message

                $(`[id="bcfPreview"]`).html("");
                var loadingTask = pdfjsLib.getDocument({ data: atob(response) });

                loadingTask.promise.then(function (pdf) {
                    console.log('PDF loaded');

                    let pages = pdf._pdfInfo.numPages;

                    for (let i = 1; i <= pages; i++) {
                        pdf.getPage(i).then(page => {
                            console.log(page);

                            let pdfCanvas = document.createElement("canvas");
                            let context = pdfCanvas.getContext("2d");
                            let pageViewPort = page.getViewport({ scale: 1.75 });
                            console.log(pageViewPort);

                            pdfCanvas.width = pageViewPort.width;
                            pdfCanvas.height = pageViewPort.height;

                            $(`[id="bcfPreview"]`).append(pdfCanvas);

                            page.render({
                                canvasContext: context,
                                viewport: pageViewPort
                            });
                        }).catch(error => {
                            console.error(error);
                        })
                    }
                }).catch(function (reason) {
                    // PDF loading error
                    console.error(reason);
                });
            },
            error: function (xhr, status, error) {
                // Handle error
                console.error(xhr.responseText);
                // Show error message to the user
            },
            complete: function () {
                setTimeout(function () {
                    $("#beneficiary-overlay").addClass('d-none');
                }, 2000); // 2000 milliseconds = 2 seconds
            }
        });
    }

    function loadHlafPreview() {
        $("#BarrowersInformationModel_BirthDate").attr('disabled', false);

        var formData = new FormData(document.querySelector(`#frm_hlf068`));
        // var formData = form1.serialize();

        $.ajax({
            method: 'POST',
            url: '/Report/LatestHousingFormB64',
            data: formData, // Convert to JSON string
            contentType: 'application/json', // Set content type to JSON,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function () {
                $("#beneficiary-overlay").removeClass('d-none');
            },
            success: function (response) {
                $("#BarrowersInformationModel_BirthDate").attr('disabled', true);
                // Redirect to another URL based on the response
                //window.location.href = '/Report/LatestHousingForm2';

                // Handle success response
                //console.log(response);
                // Do something with the response, like displaying a success message

                $(`[id="hlafPreview"]`).html("");
                var loadingTask = pdfjsLib.getDocument({ data: atob(response) });

                loadingTask.promise.then(function (pdf) {
                    console.log('PDF loaded');

                    let pages = pdf._pdfInfo.numPages;

                    for (let i = 1; i <= pages; i++) {
                        pdf.getPage(i).then(page => {
                            console.log(page);

                            let pdfCanvas = document.createElement("canvas");
                            let context = pdfCanvas.getContext("2d");
                            let pageViewPort = page.getViewport({ scale: 1.75 });
                            console.log(pageViewPort);

                            pdfCanvas.width = pageViewPort.width;
                            pdfCanvas.height = pageViewPort.height;

                            $(`[id="hlafPreview"]`).append(pdfCanvas);

                            page.render({
                                canvasContext: context,
                                viewport: pageViewPort
                            });
                        }).catch(error => {
                            console.error(error);
                        })
                    }

                    //  $("#beneficiary-overlay").addClass('d-none');
                }).catch(function (reason) {
                    // PDF loading error
                    console.error(reason);
                });
            },

            complete: function () {
                setTimeout(function () {
                    $("#beneficiary-overlay").addClass('d-none');
                }, 2000); // 2000 milliseconds = 2 seconds
            },

            error: function (xhr, status, error) {
                // Handle error
                console.error(xhr.responseText);
                // Show error message to the user
            }
        });
    }

    //#endregion

    function preConfirmationSwal() {
        // Use SweetAlert for confirmation
        Swal.fire({
            title: `<h1 class="text-warning p-0"><b>Leave edit mode?</b></h1>`,
            text: `You have unsaved changes. If you leave now then all of your edit progress will be lost.`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: `Continue editing`,
            cancelButtonText: 'Discard changes',
            reverseButtons: true,
            allowOutsideClick: false,
            allowEscapeKey: false,
            allowEnterKey: false
        }).then(async (result) => {
            if (result.isDismissed) {
                // Discard Changes

                await retreiveHLAFandBCF();

                $("#btn_edit").removeClass("active");

                $("#confirmCard").prop('hidden', true);

                $("#frm_hlf068 input, #frm_hlf068 select, #frm_hlf068 textarea").attr("readonly", true).addClass("disabled");

                $(`#frm_hlf068 input[type="checkbox"]`).attr("disabled", true);
                $('.calendarpicker, .timepicker, .present-calendar-picker').prop('disabled', true);
                $('input[type="radio"]').prop('disabled', true);

                $('#frm_hlf068').find('.selectized').each(function (i, e) {
                    e.selectize.lock();
                });

                $("#btn_edit").removeClass("text-danger").addClass("text-info").html(`<i class="mdi mdi-pencil-outline text-info"></i> Edit`);

                editableFlag = false;
            }
        });
    }

    async function retreiveHLAFandBCF() {
        //Loan Particulars Information Model
        let particulars = await getLoanParticularInformation(applicantInfoIdVal);

        //$("#LoanParticularsInformationModel_Id").val(particulars.Id);
        $("#LoanParticularsInformationModel_PurposeOfLoanId").val(particulars ? particulars.PurposeOfLoanId : null);
        //$("#LoanParticularsInformationModel_ApplicantsPersonalInformationId").val(particulars.ApplicantsPersonalInformationId);
        $("#LoanParticularsInformationModel_RepricingPeriod").val(particulars ? particulars.RepricingPeriod : null);
        $("#LoanParticularsInformationModel_DesiredLoanTermYears").val(particulars ? particulars.DesiredLoanTermYears : null);
        $("#LoanParticularsInformationModel_ModeOfPaymentId").data('selectize').setValue(particulars ? particulars.ModeOfPaymentId : null);
        $("#LoanParticularsInformationModel_DesiredLoanAmount").val(particulars ? particulars.DesiredLoanAmount : null);
        $("#LoanParticularsInformationModel_ExistingHousingApplicationNumber").val(particulars ? particulars.ExistingHousingApplicationNumber : null);

        let spouse = await getSpouseInformation(applicantInfoIdVal)

        //$("#SpouseModel_Id").val(spouse ? spouse.Id : 0);
        //$("#SpouseModel_ApplicantsPersonalInformationId").val(spouse ? spouse.ApplicantsPersonalInformationId : null);

        $("#SpouseModel_IsSpouseAddressAbroad").prop('checked', spouse ? spouse.IsSpouseAddressAbroad : false);
        $("#SpouseModel_SpouseEmploymentUnitName").val(spouse ? spouse.SpouseEmploymentUnitName : '');
        $("#SpouseModel_SpouseEmploymentBuildingName").val(spouse ? spouse.SpouseEmploymentBuildingName : '');
        $("#SpouseModel_SpouseEmploymentLotName").val(spouse ? spouse.SpouseEmploymentLotName : '');
        $("#SpouseModel_SpouseEmploymentStreetName").val(spouse ? spouse.SpouseEmploymentStreetName : '');
        $("#SpouseModel_SpouseEmploymentSubdivisionName").val(spouse ? spouse.SpouseEmploymentSubdivisionName : '');
        $("#SpouseModel_SpouseEmploymentBaranggayName").val(spouse ? spouse.SpouseEmploymentBaranggayName : '');
        $("#SpouseModel_SpouseEmploymentMunicipalityName").val(spouse ? spouse.SpouseEmploymentMunicipalityName : '');
        $("#SpouseModel_SpouseEmploymentProvinceName").val(spouse ? spouse.SpouseEmploymentProvinceName : '');
        $("#SpouseModel_SpouseEmploymentZipCode").val(spouse ? spouse.SpouseEmploymentZipCode : '');
        $("#SpouseModel_PreparedMailingAddress").val(spouse ? spouse.PreparedMailingAddress : '');
        $("#SpouseModel_PreferredTimeToContact").val(spouse ? spouse.PreferredTimeToContact : '');
        $("#SpouseModel_LastName").val(spouse ? spouse.LastName : '');
        $("#SpouseModel_FirstName").val(spouse ? spouse.FirstName : '');
        $("#SpouseModel_MiddleName").val(spouse ? spouse.MiddleName : '');
        $("#SpouseModel_Suffix").val(spouse ? spouse.Suffix : '');
        $("#SpouseModel_PagibigMidNumber").val(spouse ? spouse.PagibigMidNumber : '');
        $("#SpouseModel_TinNumber").val(spouse ? spouse.TinNumber : '');
        $("#SpouseModel_Citizenship").val(spouse ? spouse.Citizenship : '');

        $("#SpouseModel_BirthDate").val(spouse ? spouse.BirthDate : '');
        setDateValue("#SpouseModel_BirthDate");

        $("#SpouseModel_BusinessNumber").val(spouse ? spouse.BusinessNumber : '');
        $("#SpouseModel_DeletedById").val(spouse ? spouse.DeletedById : null);
        $("#SpouseModel_IndustryId").data('selectize').setValue(spouse ? spouse.IndustryId : null);
        $("#SpouseModel_BusinessName").val(spouse ? spouse.BusinessName : '');
        $("#SpouseModel_OccupationStatus").data('selectize').setValue(spouse ? spouse.OccupationStatus : null);
        $("#SpouseModel_YearsInEmployment").val(spouse ? spouse.YearsInEmployment : null);
        $("#SpouseModel_EmploymentPosition").val(spouse ? spouse.EmploymentPosition : '');
        $("#SpouseModel_BusinessTelNo").val(spouse ? spouse.BusinessTelNo : '');

        let borrower = await getBorrowerInformation(applicantInfoIdVal);

        //$("#BarrowersInformationModel_Id").val(borrower.Id);
        //$("#BarrowersInformationModel_ApplicantsPersonalInformationId").val(borrower.ApplicantsPersonalInformationId);

        $("#BarrowersInformationModel_LastName").val(borrower.LastName);
        $("#BarrowersInformationModel_FirstName").val(borrower.FirstName);
        $("#BarrowersInformationModel_MiddleName").val(borrower.MiddleName);
        $("#BarrowersInformationModel_Suffix").val(borrower.Suffix);
        $("#BarrowersInformationModel_Citizenship").val(borrower.Citizenship);

        $("#BarrowersInformationModel_BirthDate").val(borrower.BirthDate);
        setDateValue('#BarrowersInformationModel_BirthDate');

        $("#BarrowersInformationModel_Sex").data('selectize').setValue(borrower ? borrower.Sex : null);
        $("#BarrowersInformationModel_MaritalStatus").data('selectize').setValue(borrower ? borrower.MaritalStatus : null);
        $("#BarrowersInformationModel_HomeNumber").val(borrower.HomeNumber);
        $("#BarrowersInformationModel_MobileNumber").val(borrower.MobileNumber);
        $("#BarrowersInformationModel_Email").val(borrower.Email);
        $("#BarrowersInformationModel_PresentUnitName").val(borrower.PresentUnitName);
        $("#BarrowersInformationModel_PresentBuildingName").val(borrower.PresentBuildingName);
        $("#BarrowersInformationModel_PresentLotName").val(borrower.PresentLotName);
        $("#BarrowersInformationModel_PresentStreetName").val(borrower.PresentStreetName);
        $("#BarrowersInformationModel_PresentSubdivisionName").val(borrower.PresentSubdivisionName);
        $("#BarrowersInformationModel_PresentBaranggayName").val(borrower.PresentBaranggayName);
        $("#BarrowersInformationModel_PresentMunicipalityName").val(borrower.PresentMunicipalityName);
        $("#BarrowersInformationModel_PresentProvinceName").val(borrower.PresentProvinceName);
        $("#BarrowersInformationModel_PresentZipCode").val(borrower.PresentZipCode);
        $("#BarrowersInformationModel_PermanentUnitName").val(borrower.PermanentUnitName);
        $("#BarrowersInformationModel_PermanentBuildingName").val(borrower.PermanentBuildingName);
        $("#BarrowersInformationModel_PermanentLotName").val(borrower.PermanentLotName);
        $("#BarrowersInformationModel_PermanentStreetName").val(borrower.PermanentStreetName);
        $("#BarrowersInformationModel_PermanentSubdivisionName").val(borrower.PermanentSubdivisionName);
        $("#BarrowersInformationModel_PermanentBaranggayName").val(borrower.PermanentBaranggayName);
        $("#BarrowersInformationModel_PermanentMunicipalityName").val(borrower.PermanentMunicipalityName);
        $("#BarrowersInformationModel_PermanentProvinceName").val(borrower.PermanentProvinceName);
        $("#BarrowersInformationModel_PermanentZipCode").val(borrower.PermanentZipCode);
        $("#BarrowersInformationModel_HomeOwnerShip").data('selectize').setValue(borrower ? borrower.HomeOwnerShip : null);
        $("#BarrowersInformationModel_MonthlyRent").val(borrower.MonthlyRent);
        $("#BarrowersInformationModel_YearsofStay").val(borrower.YearsofStay);
        $("#BarrowersInformationModel_SSSNumber").val(borrower.SSSNumber);
        $("#BarrowersInformationModel_TinNumber").val(borrower.TinNumber);
        $("#BarrowersInformationModel_OccupationStatus").data('selectize').setValue(borrower ? borrower.OccupationStatus : null);
        $("#BarrowersInformationModel_EmployerName").val(borrower.EmployerName);
        $("#BarrowersInformationModel_IndustryName").data('selectize').setValue(borrower ? borrower.IndustryName : null);
        $("#BarrowersInformationModel_PositionName").val(borrower.PositionName);
        $("#BarrowersInformationModel_DepartmentName").val(borrower.DepartmentName);
        $("#BarrowersInformationModel_YearsEmployment").val(borrower.YearsEmployment);
        $("#BarrowersInformationModel_NumberOfDependent").val(borrower.NumberOfDependent);
        $("#BarrowersInformationModel_BusinessUnitName").val(borrower.BusinessUnitName);
        $("#BarrowersInformationModel_BusinessBuildingName").val(borrower.BusinessBuildingName);
        $("#BarrowersInformationModel_BusinessLotName").val(borrower.BusinessLotName);
        $("#BarrowersInformationModel_BusinessStreetName").val(borrower.BusinessStreetName);
        $("#BarrowersInformationModel_BusinessSubdivisionName").val(borrower.BusinessSubdivisionName);
        $("#BarrowersInformationModel_BusinessBaranggayName").val(borrower.BusinessBaranggayName);
        $("#BarrowersInformationModel_BusinessMunicipalityName").val(borrower.BusinessMunicipalityName);
        $("#BarrowersInformationModel_BusinessProvinceName").val(borrower.BusinessProvinceName);
        $("#BarrowersInformationModel_BusinessZipCode").val(borrower.BusinessZipCode);
        $("#BarrowersInformationModel_BusinessCountry").val(borrower.BusinessCountry);
        $("#BarrowersInformationModel_BusinessContactNumber").val(borrower.BusinessContactNumber);
        $("#BarrowersInformationModel_BusinessDirectLineNumber").val(borrower.BusinessDirectLineNumber);
        $("#BarrowersInformationModel_BusinessTruckLineNumber").val(borrower.BusinessTruckLineNumber);
        $("#BarrowersInformationModel_BusinessEmail").val(borrower.BusinessEmail);
        $("#BarrowersInformationModel_PreparedMailingAddress").data('selectize').setValue(borrower ? borrower.PreparedMailingAddress : null);
        $("#BarrowersInformationModel_PreferredTimeToContact").val(borrower.PreferredTimeToContact);
        $("#BarrowersInformationModel_IsPermanentAddressAbroad").prop('checked', borrower.IsPermanentAddressAbroad);
        $("#BarrowersInformationModel_IsPresentAddressAbroad").prop('checked', borrower.IsPresentAddressAbroad);
        $("#BarrowersInformationModel_PresentAddressIsPermanentAddress").prop('checked', borrower.PresentAddressIsPermanentAddress);
        $("#BarrowersInformationModel_IsBusinessAddressAbroad").prop('checked', borrower.IsBusinessAddressAbroad);
        $("#BarrowersInformationModel_PropertyDeveloperName").val(borrower.PropertyDeveloperName);
        $("#BarrowersInformationModel_PropertyLocation").val(borrower.PropertyLocation);
        $("#BarrowersInformationModel_PropertyUnitLevelName").val(borrower.PropertyUnitLevelName);

        let form2Page = await getForm2PageInformation(applicantInfoIdVal);

        //$("#Form2PageModel_Id").val(form2Page.Id);
        //$("#Form2PageModel_ApplicantsPersonalInformationId").val(form2Page.ApplicantsPersonalInformationId);

        $("#Form2PageModel_Bank1").val(form2Page.Bank1 ? form2Page.Bank1 : null);
        $("#Form2PageModel_Bank2").val(form2Page.Bank2 ? form2Page.Bank2 : null);
        $("#Form2PageModel_Bank3").val(form2Page.Bank3 ? form2Page.Bank3 : null);

        $("#Form2PageModel_BranchAddress1").val(form2Page.BranchAddress1 ? form2Page.BranchAddress1 : null);
        $("#Form2PageModel_BranchAddress2").val(form2Page.BranchAddress2 ? form2Page.BranchAddress2 : null);
        $("#Form2PageModel_BranchAddress3").val(form2Page.BranchAddress3 ? form2Page.BranchAddress3 : null);

        $("#Form2PageModel_TypeOfAccount1").val(form2Page.TypeOfAccount1 ? form2Page.TypeOfAccount1 : null);
        $("#Form2PageModel_TypeOfAccount2").val(form2Page.TypeOfAccount2 ? form2Page.TypeOfAccount2 : null);
        $("#Form2PageModel_TypeOfAccount3").val(form2Page.TypeOfAccount3 ? form2Page.TypeOfAccount3 : null);

        $("#Form2PageModel_AccountNumber1").val(form2Page.AccountNumber1 ? form2Page.AccountNumber1 : null);
        $("#Form2PageModel_AccountNumber2").val(form2Page.AccountNumber2 ? form2Page.AccountNumber2 : null);
        $("#Form2PageModel_AccountNumber3").val(form2Page.AccountNumber3 ? form2Page.AccountNumber3 : null);

        $("#Form2PageModel_DateOpened1").val(form2Page.DateOpened1 ? form2Page.DateOpened1 : null);
        $("#Form2PageModel_DateOpened2").val(form2Page.DateOpened2 ? form2Page.DateOpened2 : null);
        $("#Form2PageModel_DateOpened3").val(form2Page.DateOpened3 ? form2Page.DateOpened3 : null);

        $("#Form2PageModel_AverageBalance1").val(form2Page.AverageBalance1 ? form2Page.AverageBalance1 : null);
        $("#Form2PageModel_AverageBalance2").val(form2Page.AverageBalance2 ? form2Page.AverageBalance2 : null);
        $("#Form2PageModel_AverageBalance3").val(form2Page.AverageBalance3 ? form2Page.AverageBalance3 : null);

        $("#Form2PageModel_IssuerName1").val(form2Page.IssuerName1 ? form2Page.IssuerName1 : null);
        $("#Form2PageModel_IssuerName2").val(form2Page.IssuerName2 ? form2Page.IssuerName2 : null);
        $("#Form2PageModel_IssuerName3").val(form2Page.IssuerName3 ? form2Page.IssuerName3 : null);

        $("#Form2PageModel_CardType1").val(form2Page.CardType1 ? form2Page.CardType1 : null);
        $("#Form2PageModel_CardType2").val(form2Page.CardType2 ? form2Page.CardType2 : null);
        $("#Form2PageModel_CardType3").val(form2Page.CardType3 ? form2Page.CardType3 : null);

        $("#Form2PageModel_CardExpiration1").val(form2Page.CardExpiration1 ? form2Page.CardExpiration1 : null);
        $("#Form2PageModel_CardExpiration2").val(form2Page.CardExpiration2 ? form2Page.CardExpiration2 : null);
        $("#Form2PageModel_CardExpiration3").val(form2Page.CardExpiration3 ? form2Page.CardExpiration3 : null);

        $("#Form2PageModel_CreditLimit1").val(form2Page.CreditLimit1 ? form2Page.CreditLimit1 : null);
        $("#Form2PageModel_CreditLimit2").val(form2Page.CreditLimit2 ? form2Page.CreditLimit2 : null);
        $("#Form2PageModel_CreditLimit3").val(form2Page.CreditLimit3 ? form2Page.CreditLimit3 : null);

        $("#Form2PageModel_Location1").val(form2Page.Location1 ? form2Page.Location1 : null);
        $("#Form2PageModel_Location2").val(form2Page.Location2 ? form2Page.Location2 : null);
        $("#Form2PageModel_Location3").val(form2Page.Location3 ? form2Page.Location3 : null);

        $("#Form2PageModel_TypeOfProperty1").val(form2Page.TypeOfProperty1 ? form2Page.TypeOfProperty1 : null);
        $("#Form2PageModel_TypeOfProperty2").val(form2Page.TypeOfProperty2 ? form2Page.TypeOfProperty2 : null);
        $("#Form2PageModel_TypeOfProperty3").val(form2Page.TypeOfProperty3 ? form2Page.TypeOfProperty3 : null);

        $("#Form2PageModel_AquisitionCost1").val(form2Page.AquisitionCost1 ? form2Page.AquisitionCost1 : null);
        $("#Form2PageModel_AquisitionCost2").val(form2Page.AquisitionCost2 ? form2Page.AquisitionCost2 : null);
        $("#Form2PageModel_AquisitionCost3").val(form2Page.AquisitionCost3 ? form2Page.AquisitionCost3 : null);

        $("#Form2PageModel_MarketValue1").val(form2Page.MarketValue1 ? form2Page.MarketValue1 : null);
        $("#Form2PageModel_MarketValue2").val(form2Page.MarketValue2 ? form2Page.MarketValue2 : null);
        $("#Form2PageModel_MarketValue3").val(form2Page.MarketValue3 ? form2Page.MarketValue3 : null);

        $("#Form2PageModel_MortgageBalance1").val(form2Page.MortgageBalance1 ? form2Page.MortgageBalance1 : null);
        $("#Form2PageModel_MortgageBalance2").val(form2Page.MortgageBalance2 ? form2Page.MortgageBalance2 : null);
        $("#Form2PageModel_MortgageBalance3").val(form2Page.MortgageBalance3 ? form2Page.MortgageBalance3 : null);

        $("#Form2PageModel_RentalIncome1").val(form2Page.RentalIncome1 ? form2Page.RentalIncome1 : null);
        $("#Form2PageModel_RentalIncome2").val(form2Page.RentalIncome2 ? form2Page.RentalIncome2 : null);
        $("#Form2PageModel_RentalIncome3").val(form2Page.RentalIncome3 ? form2Page.RentalIncome3 : null);

        $("#Form2PageModel_CreditorAndAddress1").val(form2Page.CreditorAndAddress1 ? form2Page.CreditorAndAddress1 : null);
        $("#Form2PageModel_CreditorAndAddress2").val(form2Page.CreditorAndAddress2 ? form2Page.CreditorAndAddress2 : null);
        $("#Form2PageModel_CreditorAndAddress3").val(form2Page.CreditorAndAddress3 ? form2Page.CreditorAndAddress3 : null);

        $("#Form2PageModel_Security1").val(form2Page.Security1 ? form2Page.Security1 : null);
        $("#Form2PageModel_Security2").val(form2Page.Security2 ? form2Page.Security2 : null);
        $("#Form2PageModel_Security3").val(form2Page.Security3 ? form2Page.Security3 : null);

        $("#Form2PageModel_Type1").val(form2Page.Type1 ? form2Page.Type1 : null);
        $("#Form2PageModel_Type2").val(form2Page.Type2 ? form2Page.Type2 : null);
        $("#Form2PageModel_Type3").val(form2Page.Type3 ? form2Page.Type3 : null);

        $("#Form2PageModel_AmountBalance1").val(form2Page.AmountBalance1 ? form2Page.AmountBalance1 : null);
        $("#Form2PageModel_AmountBalance2").val(form2Page.AmountBalance2 ? form2Page.AmountBalance2 : null);
        $("#Form2PageModel_AmountBalance3").val(form2Page.AmountBalance3 ? form2Page.AmountBalance3 : null);

        $("#Form2PageModel_MaturityDateTime1").val(form2Page.MaturityDateTime1 ? form2Page.MaturityDateTime1 : null);
        $("#Form2PageModel_MaturityDateTime2").val(form2Page.MaturityDateTime2 ? form2Page.MaturityDateTime2 : null);
        $("#Form2PageModel_MaturityDateTime3").val(form2Page.MaturityDateTime3 ? form2Page.MaturityDateTime3 : null);

        $("#Form2PageModel_Amortization1").val(form2Page.Amortization1 ? form2Page.Amortization1 : null);
        $("#Form2PageModel_Amortization2").val(form2Page.Amortization2 ? form2Page.Amortization2 : null);
        $("#Form2PageModel_Amortization3").val(form2Page.Amortization3 ? form2Page.Amortization3 : null);

        $("#Form2PageModel_PendingCase").val(form2Page.PendingCase ? form2Page.PendingCase : null);
        $("#Form2PageModel_PastDue").val(form2Page.PastDue ? form2Page.PastDue : null);
        $("#Form2PageModel_BouncingChecks").val(form2Page.BouncingChecks ? form2Page.BouncingChecks : null);
        $("#Form2PageModel_MedicalAdvice").val(form2Page.MedicalAdvice ? form2Page.MedicalAdvice : null);

        $("#Form2PageModel_BankFinancial1").val(form2Page.BankFinancial1 ? form2Page.BankFinancial1 : null);
        $("#Form2PageModel_BankFinancial2").val(form2Page.BankFinancial2 ? form2Page.BankFinancial2 : null);
        $("#Form2PageModel_BankFinancial3").val(form2Page.BankFinancial3 ? form2Page.BankFinancial3 : null);

        $("#Form2PageModel_Address1").val(form2Page.Address1 ? form2Page.Address1 : null);
        $("#Form2PageModel_Address2").val(form2Page.Address2 ? form2Page.Address2 : null);
        $("#Form2PageModel_Address3").val(form2Page.Address3 ? form2Page.Address3 : null);

        $("#Form2PageModel_Purpose1").val(form2Page.Purpose1 ? form2Page.Purpose1 : null);
        $("#Form2PageModel_Purpose2").val(form2Page.Purpose2 ? form2Page.Purpose2 : null);
        $("#Form2PageModel_Purpose3").val(form2Page.Purpose3 ? form2Page.Purpose3 : null);

        $("#Form2PageModel_LoanSecurity1").val(form2Page.LoanSecurity1 ? form2Page.LoanSecurity1 : null);
        $("#Form2PageModel_LoanSecurity2").val(form2Page.LoanSecurity2 ? form2Page.LoanSecurity2 : null);
        $("#Form2PageModel_LoanSecurity3").val(form2Page.LoanSecurity3 ? form2Page.LoanSecurity3 : null);

        $("#Form2PageModel_HighestAmount1").val(form2Page.HighestAmount1 ? form2Page.HighestAmount1 : null);
        $("#Form2PageModel_HighestAmount2").val(form2Page.HighestAmount2 ? form2Page.HighestAmount2 : null);
        $("#Form2PageModel_HighestAmount3").val(form2Page.HighestAmount3 ? form2Page.HighestAmount3 : null);

        $("#Form2PageModel_PresentBalance1").val(form2Page.PresentBalance1 ? form2Page.PresentBalance1 : null);
        $("#Form2PageModel_PresentBalance2").val(form2Page.PresentBalance2 ? form2Page.PresentBalance2 : null);
        $("#Form2PageModel_PresentBalance3").val(form2Page.PresentBalance3 ? form2Page.PresentBalance3 : null);

        $("#Form2PageModel_DateObtained1").val(form2Page.DateObtained1 ? form2Page.DateObtained1 : null);
        $("#Form2PageModel_DateObtained2").val(form2Page.DateObtained2 ? form2Page.DateObtained2 : null);
        $("#Form2PageModel_DateObtained3").val(form2Page.DateObtained3 ? form2Page.DateObtained3 : null);

        $("#Form2PageModel_DateFullyPaid1").val(form2Page.DateFullyPaid1 ? form2Page.DateFullyPaid1 : null);
        $("#Form2PageModel_DateFullyPaid2").val(form2Page.DateFullyPaid2 ? form2Page.DateFullyPaid2 : null);
        $("#Form2PageModel_DateFullyPaid3").val(form2Page.DateFullyPaid3 ? form2Page.DateFullyPaid3 : null);

        $("#Form2PageModel_NameSupplier1").val(form2Page.NameSupplier1 ? form2Page.NameSupplier1 : null);
        $("#Form2PageModel_NameSupplier2").val(form2Page.NameSupplier2 ? form2Page.NameSupplier2 : null);
        $("#Form2PageModel_NameSupplier3").val(form2Page.NameSupplier3 ? form2Page.NameSupplier3 : null);

        $("#Form2PageModel_TradeAddress1").val(form2Page.TradeAddress1 ? form2Page.TradeAddress1 : null);
        $("#Form2PageModel_TradeAddress2").val(form2Page.TradeAddress2 ? form2Page.TradeAddress2 : null);
        $("#Form2PageModel_TradeAddress3").val(form2Page.TradeAddress3 ? form2Page.TradeAddress3 : null);

        $("#Form2PageModel_TradeTellNo1").val(form2Page.TradeTellNo1 ? form2Page.TradeTellNo1 : null);
        $("#Form2PageModel_TradeTellNo2").val(form2Page.TradeTellNo2 ? form2Page.TradeTellNo2 : null);
        $("#Form2PageModel_TradeTellNo3").val(form2Page.TradeTellNo3 ? form2Page.TradeTellNo3 : null);

        $("#Form2PageModel_CharacterNameSupplier1").val(form2Page.CharacterNameSupplier1 ? form2Page.CharacterNameSupplier1 : null);
        $("#Form2PageModel_CharacterNameSupplier2").val(form2Page.CharacterNameSupplier2 ? form2Page.CharacterNameSupplier2 : null);
        $("#Form2PageModel_CharacterNameSupplier3").val(form2Page.CharacterNameSupplier3 ? form2Page.CharacterNameSupplier3 : null);

        $("#Form2PageModel_CharacterAddress1").val(form2Page.CharacterAddress1 ? form2Page.CharacterAddress1 : null);
        $("#Form2PageModel_CharacterAddress2").val(form2Page.CharacterAddress2 ? form2Page.CharacterAddress2 : null);
        $("#Form2PageModel_CharacterAddress3").val(form2Page.CharacterAddress3 ? form2Page.CharacterAddress3 : null);

        $("#Form2PageModel_CharacterTellNo1").val(form2Page.CharacterTellNo1 ? form2Page.CharacterTellNo1 : null);
        $("#Form2PageModel_CharacterTellNo2").val(form2Page.CharacterTellNo2 ? form2Page.CharacterTellNo2 : null);
        $("#Form2PageModel_CharacterTellNo3").val(form2Page.CharacterTellNo3 ? form2Page.CharacterTellNo3 : null);

        $("#Form2PageModel_FirstName").val(form2Page.FirstName ? form2Page.FirstName : null);
        $("#Form2PageModel_MiddleName").val(form2Page.MiddleName ? form2Page.MiddleName : null);
        $("#Form2PageModel_Suffix").val(form2Page.Suffix ? form2Page.Suffix : null);
        $("#Form2PageModel_LastName").val(form2Page.LastName ? form2Page.LastName : null);
        $("#Form2PageModel_PagibigNumber").val(form2Page.PagibigNumber ? form2Page.PagibigNumber : null);
        $("#Form2PageModel_TinNumber").val(form2Page.TinNumber ? form2Page.TinNumber : null);
        $("#Form2PageModel_ContactNumber").val(form2Page.ContactNumber ? form2Page.ContactNumber : null);
        $("#Form2PageModel_Email").val(form2Page.Email ? form2Page.Email : null);
        $("#Form2PageModel_SourcePagibigFundId").val(form2Page.SourcePagibigFundId ? form2Page.SourcePagibigFundId : null);
        $("#Form2PageModel_DateCreated").val(form2Page.DateCreated ? form2Page.DateCreated : null);
        $("#Form2PageModel_CreatedById").val(form2Page.CreatedById ? form2Page.CreatedById : null);
        $("#Form2PageModel_DateModified").val(form2Page.DateModified ? form2Page.DateModified : null);
        $("#Form2PageModel_ModifiedById").val(form2Page.ModifiedById ? form2Page.ModifiedById : null);
        $("#Form2PageModel_DateDeleted").val(form2Page.DateDeleted ? form2Page.DateDeleted : null);
        $("#Form2PageModel_DeletedById").val(form2Page.DeletedById ? form2Page.DeletedById : null);
        $("#Form2PageModel_Agreement").val(form2Page.Agreement ? form2Page.Agreement : null);
        $("#Form2PageModel_SellersUnitName").val(form2Page.SellersUnitName ? form2Page.SellersUnitName : null);
        $("#Form2PageModel_SellersBuildingName").val(form2Page.SellersBuildingName ? form2Page.SellersBuildingName : null);
        $("#Form2PageModel_SellersLotName").val(form2Page.SellersLotName ? form2Page.SellersLotName : null);
        $("#Form2PageModel_SellersStreetName").val(form2Page.SellersStreetName ? form2Page.SellersStreetName : null);
        $("#Form2PageModel_SellersSubdivisionName").val(form2Page.SellersSubdivisionName ? form2Page.SellersSubdivisionName : null);
        $("#Form2PageModel_SellersBaranggayName").val(form2Page.SellersBaranggayName ? form2Page.SellersBaranggayName : null);
        $("#Form2PageModel_SellersMunicipalityName").val(form2Page.SellersMunicipalityName ? form2Page.SellersMunicipalityName : null);
        $("#Form2PageModel_SellersProvinceName").val(form2Page.SellersProvinceName ? form2Page.SellersProvinceName : null);
        $("#Form2PageModel_SellersZipCode").val(form2Page.SellersZipCode ? form2Page.SellersZipCode : null);

        let collateral = await getCollateralInformation(applicantInfoIdVal);

        //$("#CollateralInformationModel_Id").val(collateral ? collateral.Id : 0);
        //$("#CollateralInformationModel_ApplicantsPersonalInformationId").val(collateral ? collateral.ApplicantsPersonalInformationId : null);

        $("#CollateralInformationModel_Province").val(collateral ? collateral.Province : '');
        $("#CollateralInformationModel_Municipality").val(collateral ? collateral.Municipality : '');
        $("#CollateralInformationModel_Street").val(collateral ? collateral.Street : '');
        $("#CollateralInformationModel_DeveloperName").val(collateral ? collateral.DeveloperName : '');
        $("#CollateralInformationModel_PropertyTypeId").data('selectize').setValue(collateral ? collateral.PropertyTypeId : null);
        $("#CollateralInformationModel_TctOctCctNumber").val(collateral ? collateral.TctOctCctNumber : '');
        $("#CollateralInformationModel_TaxDeclrationNumber").val(collateral ? collateral.TaxDeclarationNumber : '');
        $("#CollateralInformationModel_LotUnitNumber").val(collateral ? collateral.LotUnitNumber : null);
        $("#CollateralInformationModel_BlockBuildingNumber").val(collateral ? collateral.BlockBuildingNumber : null);
        $("#CollateralInformationModel_IsMortgage").prop("checked", collateral ? collateral.IsMortgage : false);
        $("#CollateralInformationModel_CollateralReason").val(collateral ? collateral.CollateralReason : '');
        $("#CollateralInformationModel_LandArea").val(collateral ? collateral.LandArea : null);
        $("#CollateralInformationModel_HouseAge").val(collateral ? collateral.HouseAge : null);
        $("#CollateralInformationModel_NumberOfStoreys").val(collateral ? collateral.NumberOfStoreys : null);
        $("#CollateralInformationModel_ProposedNoOfStoreys").val(collateral ? collateral.ProposedNoOfStoreys : null);
        $("#CollateralInformationModel_ExistingTotalFloorArea").val(collateral ? collateral.ExistingTotalFloorArea : null);
        $("#CollateralInformationModel_ProposedTotalFloorArea").val(collateral ? collateral.ProposedTotalFloorArea : null);
        $("#CollateralInformationModel_PagibigMidNumber").val(collateral ? collateral.PagibigMidNumber : null);
        $("#CollateralInformationModel_HanNumber").val(collateral ? collateral.HanNumber : null);

        let buyerConfimarion = await getBuyerConfirmation(buyerconfirmationCode);

        //$("#BuyerConfirmationModel_Id").val(buyerConfimarion ? buyerConfimarion.Id : null);
        //$("#BuyerConfirmationModel_ApplicantsPersonalInformationId").val(buyerConfimarion ? buyerConfimarion.ApplicantsPersonalInformationId : null);

        $("#BuyerConfirmationModel_UserId").val(buyerConfimarion ? buyerConfimarion.UserId : null);
        $("#BuyerConfirmationModel_PagibigNumber").val(buyerConfimarion ? buyerConfimarion.PagibigNumber : null);
        $("#BuyerConfirmationModel_Code").val(buyerConfimarion ? buyerConfimarion.Code : null);
        $("#BuyerConfirmationModel_ProjectProponentName").val(buyerConfimarion ? buyerConfimarion.ProjectProponentName : null);
        $("#BuyerConfirmationModel_JuridicalPersonalityId").val(buyerConfimarion ? buyerConfimarion.JuridicalPersonalityId : null);
        $("#BuyerConfirmationModel_LastName").val(buyerConfimarion ? buyerConfimarion.LastName : null);
        $("#BuyerConfirmationModel_FirstName").val(buyerConfimarion ? buyerConfimarion.FirstName : null);
        $("#BuyerConfirmationModel_MiddleName").val(buyerConfimarion ? buyerConfimarion.MiddleName : null);
        $("#BuyerConfirmationModel_Suffix").val(buyerConfimarion ? buyerConfimarion.Suffix : null);
        $("#BuyerConfirmationModel_BirthDate").val(buyerConfimarion ? buyerConfimarion.BirthDate : null);
        $("#BuyerConfirmationModel_MothersMaidenName").val(buyerConfimarion ? buyerConfimarion.MothersMaidenName : null);
        $("#BuyerConfirmationModel_MaritalStatus").val(buyerConfimarion ? buyerConfimarion.MaritalStatus : null);
        $("#BuyerConfirmationModel_OccupationStatus").val(buyerConfimarion ? buyerConfimarion.OccupationStatus : null);
        $("#BuyerConfirmationModel_PresentUnitName").val(buyerConfimarion ? buyerConfimarion.PresentUnitName : null);
        $("#BuyerConfirmationModel_PresentBuildingName").val(buyerConfimarion ? buyerConfimarion.PresentBuildingName : null);
        $("#BuyerConfirmationModel_PresentLotName").val(buyerConfimarion ? buyerConfimarion.PresentLotName : null);
        $("#BuyerConfirmationModel_PresentStreetName").val(buyerConfimarion ? buyerConfimarion.PresentStreetName : null);
        $("#BuyerConfirmationModel_PresentSubdivisionName").val(buyerConfimarion ? buyerConfimarion.PresentSubdivisionName : null);
        $("#BuyerConfirmationModel_PresentBaranggayName").val(buyerConfimarion ? buyerConfimarion.PresentBaranggayName : null);
        $("#BuyerConfirmationModel_PresentMunicipalityName").val(buyerConfimarion ? buyerConfimarion.PresentMunicipalityName : null);
        $("#BuyerConfirmationModel_PresentProvinceName").val(buyerConfimarion ? buyerConfimarion.PresentProvinceName : null);
        $("#BuyerConfirmationModel_PresentZipCode").val(buyerConfimarion ? buyerConfimarion.PresentZipCode : null);
        $("#BuyerConfirmationModel_HomeNumber").val(buyerConfimarion ? buyerConfimarion.HomeNumber : null);
        $("#BuyerConfirmationModel_MobileNumber").val(buyerConfimarion ? buyerConfimarion.MobileNumber : null);
        $("#BuyerConfirmationModel_BusinessTelNo").val(buyerConfimarion ? buyerConfimarion.BusinessTelNo : null);
        $("#BuyerConfirmationModel_Email").val(buyerConfimarion ? buyerConfimarion.Email : null);
        $("#BuyerConfirmationModel_CompanyEmployerName").val(buyerConfimarion ? buyerConfimarion.CompanyEmployerName : null);
        $("#BuyerConfirmationModel_CompanyUnitName").val(buyerConfimarion ? buyerConfimarion.CompanyUnitName : null);
        $("#BuyerConfirmationModel_CompanyBuildingName").val(buyerConfimarion ? buyerConfimarion.CompanyBuildingName : null);
        $("#BuyerConfirmationModel_CompanyLotName").val(buyerConfimarion ? buyerConfimarion.CompanyLotName : null);
        $("#BuyerConfirmationModel_CompanyStreetName").val(buyerConfimarion ? buyerConfimarion.CompanyStreetName : null);
        $("#BuyerConfirmationModel_CompanySubdivisionName").val(buyerConfimarion ? buyerConfimarion.CompanySubdivisionName : null);
        $("#BuyerConfirmationModel_CompanyBaranggayName").val(buyerConfimarion ? buyerConfimarion.CompanyBaranggayName : null);
        $("#BuyerConfirmationModel_CompanyMunicipalityName").val(buyerConfimarion ? buyerConfimarion.CompanyMunicipalityName : null);
        $("#BuyerConfirmationModel_CompanyProvinceName").val(buyerConfimarion ? buyerConfimarion.CompanyProvinceName : null);
        $("#BuyerConfirmationModel_CompanyZipCode").val(buyerConfimarion ? buyerConfimarion.CompanyZipCode : null);
        $("#BuyerConfirmationModel_SpouseLastName").val(buyerConfimarion ? buyerConfimarion.SpouseLastName : null);
        $("#BuyerConfirmationModel_SpouseFirstName").val(buyerConfimarion ? buyerConfimarion.SpouseFirstName : null);
        $("#BuyerConfirmationModel_SpouseMiddleName").val(buyerConfimarion ? buyerConfimarion.SpouseMiddleName : null);
        $("#BuyerConfirmationModel_SpouseSuffix").val(buyerConfimarion ? buyerConfimarion.SpouseSuffix : null);
        $("#BuyerConfirmationModel_SpouseCompanyEmployerName").val(buyerConfimarion ? buyerConfimarion.SpouseCompanyEmployerName : null);
        $("#BuyerConfirmationModel_SpouseCompanyUnitName").val(buyerConfimarion ? buyerConfimarion.SpouseCompanyUnitName : null);
        $("#BuyerConfirmationModel_SpouseCompanyBuildingName").val(buyerConfimarion ? buyerConfimarion.SpouseCompanyBuildingName : null);
        $("#BuyerConfirmationModel_SpouseCompanyLotName").val(buyerConfimarion ? buyerConfimarion.SpouseCompanyLotName : null);
        $("#BuyerConfirmationModel_SpouseCompanyStreetName").val(buyerConfimarion ? buyerConfimarion.SpouseCompanyStreetName : null);
        $("#BuyerConfirmationModel_SpouseCompanySubdivisionName").val(buyerConfimarion ? buyerConfimarion.SpouseCompanySubdivisionName : null);
        $("#BuyerConfirmationModel_SpouseCompanyBaranggayName").val(buyerConfimarion ? buyerConfimarion.SpouseCompanyBaranggayName : null);
        $("#BuyerConfirmationModel_SpouseCompanyMunicipalityName").val(buyerConfimarion ? buyerConfimarion.SpouseCompanyMunicipalityName : null);
        $("#BuyerConfirmationModel_SpouseCompanyProvinceName").val(buyerConfimarion ? buyerConfimarion.SpouseCompanyProvinceName : null);
        $("#BuyerConfirmationModel_SpouseCompanyZipCode").val(buyerConfimarion ? buyerConfimarion.SpouseCompanyZipCode : null);
        $("#BuyerConfirmationModel_MonthlySalary").val(buyerConfimarion ? buyerConfimarion.MonthlySalary : null);
        $("#BuyerConfirmationModel_SpouseMonthlySalary").val(buyerConfimarion ? buyerConfimarion.SpouseMonthlySalary : null);
        $("#BuyerConfirmationModel_IsOtherSourceOfIncome").prop('checked', buyerConfimarion ? buyerConfimarion.IsOtherSourceOfIncome : false);
        $("#BuyerConfirmationModel_AdditionalSourceIncome").val(buyerConfimarion ? buyerConfimarion.AdditionalSourceIncome : null);
        $("#BuyerConfirmationModel_AverageMonthlyAdditionalIncome").val(buyerConfimarion ? buyerConfimarion.AverageMonthlyAdditionalIncome : null);
        $("#BuyerConfirmationModel_IsPagibigMember").prop('checked', buyerConfimarion ? buyerConfimarion.IsPagibigMember : false);
        $("#BuyerConfirmationModel_IsPagibigAvailedLoan").prop('checked', buyerConfimarion ? buyerConfimarion.IsPagibigAvailedLoan : false);
        $("#BuyerConfirmationModel_IsPagibigCoBorrower").prop('checked', buyerConfimarion ? buyerConfimarion.IsPagibigCoBorrower : false);
        $("#BuyerConfirmationModel_IsPursueProjectProponent").prop('checked', buyerConfimarion ? buyerConfimarion.IsPursueProjectProponent : false);
        $("#BuyerConfirmationModel_IsInformedTermsConditions").prop('checked', buyerConfimarion ? buyerConfimarion.IsInformedTermsConditions : false);
        $("#BuyerConfirmationModel_HouseUnitModel").val(buyerConfimarion ? buyerConfimarion.HouseUnitModel : null);
        $("#BuyerConfirmationModel_SellingPrice").val(buyerConfimarion ? buyerConfimarion.SellingPrice : null);
        $("#BuyerConfirmationModel_MonthlyAmortization").val(buyerConfimarion ? buyerConfimarion.MonthlyAmortization : null);

        initializeRadioBtnMisc();

        //$("#frm_hlf068").clearValidation();
    }

    function adjustEditCard() {
        if (!$(`[id="div_approvebcfNote"]`).hasClass('d-none')) {
            $(`[id="confirmCard"]`).css('top', '6em');
        }
        //else {
        //    $(`[id="confirmCard"]`).css('top', '0.5em');
        //}
    }

    //#region Getters Functions

    async function getBuyerConfirmation(code) {
        const response = await $.ajax({
            url: baseUrl + "BuyerConfirmation/GetBCFapplicationByCode/",
            data: { code: code },
            method: 'Get',
            dataType: 'json'
        });

        return response;
    }

    async function getLoanParticularInformation(id) {
        const response = await $.ajax({
            url: baseUrl + "Applicants/GetLoanParticularsByApplicantInfoData/" + id,
            method: 'Get',
            dataType: 'json'
        });

        return response;
    }

    async function getSpouseInformation(id) {
        const response = await $.ajax({
            url: baseUrl + "Applicants/GetSpouseByApplicantInfoData/" + id,
            method: 'Get',
            dataType: 'json'
        });

        return response;
    }

    async function getBorrowerInformation(applicantId) {
        const response = await $.ajax({
            url: baseUrl + "Applicants/GetBarrowerByApplicantInfoData/" + applicantId,
            method: 'Get',
            dataType: 'json'
        });

        return response;
    }

    async function getForm2PageInformation(applicantId) {
        const response = await $.ajax({
            url: baseUrl + "Applicants/GetForm2ByApplicantInfoData/" + applicantId,
            method: 'Get',
            dataType: 'json'
        });

        return response;
    }

    async function getCollateralInformation(applicantId) {
        const response = await $.ajax({
            url: baseUrl + "Applicants/GetCollateralByApplicantInfoData/" + applicantId,
            method: 'Get',
            dataType: 'json'
        });

        return response;
    }

    //#endregion Getters Funcions

    function DisableBuyerConfirmationFields() {
        $('[id^="BuyerConfirmationModel_"]').each(function () {
            $(this).prop('disabled', true);
        });

        $("#BuyerConfirmationModel_JuridicalPersonalityId")[0].selectize.lock();
        $("#BuyerConfirmationModel_OccupationStatus")[0].selectize.lock();
        $("#BuyerConfirmationModel_MaritalStatus")[0].selectize.lock();

        $('[name^="incomeSrcRbtn"]').each(function () {
            $(this).prop('disabled', true);
        });

        $('[name^="pagibigRbtn"]').each(function () {
            $(this).prop('disabled', true);
        });

        $('[name^="informedTermsRbtn"]').each(function () {
            $(this).prop('disabled', true);
        });

        $('[name^="availedLoanRbtn"]').each(function () {
            $(this).prop('disabled', true);
        });

        $('[name^="coBorrowerRbtn"]').each(function () {
            $(this).prop('disabled', true);
        });

        $('[name^="projectPropRbtn"]').each(function () {
            $(this).prop('disabled', true);
        });
    }

    function enableBuyerConfirmationFields() {
        $('[id^="BuyerConfirmationModel_"]').each(function () {
            $(this).prop('disabled', false);
        });

        $("#BuyerConfirmationModel_JuridicalPersonalityId")[0].selectize.unlock();
        $("#BuyerConfirmationModel_OccupationStatus")[0].selectize.unlock();
        $("#BuyerConfirmationModel_MaritalStatus")[0].selectize.unlock();

        $('[name^="incomeSrcRbtn"]').each(function () {
            $(this).prop('disabled', false);
        });

        $('[name^="pagibigRbtn"]').each(function () {
            $(this).prop('disabled', false);
        });

        $('[name^="informedTermsRbtn"]').each(function () {
            $(this).prop('disabled', false);
        });

        $('[name^="availedLoanRbtn"]').each(function () {
            $(this).prop('disabled', false);
        });

        $('[name^="coBorrowerRbtn"]').each(function () {
            $(this).prop('disabled', false);
        });

        $('[name^="projectPropRbtn"]').each(function () {
            $(this).prop('disabled', false);
        });
    }
});