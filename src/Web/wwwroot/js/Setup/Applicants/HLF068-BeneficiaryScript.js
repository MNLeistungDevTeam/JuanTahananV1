const applicantInfoIdVal = $(`[name='ApplicantsPersonalInformationModel.Id']`).val();
const roleName = $("#txt_role_name").val();
const roleId = $("#txt_roleId").val();

$(function () {
    var telNoArray = [];
    var itiFlag = false;
 
    //#region Initialization

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

    //assessPresentPermanentCheckbox();

    assessCheckbox(
        $(`[name="BarrowersInformationModel.PresentAddressIsPermanentAddress"]`),
        $(`input[name^="BarrowersInformationModel.Present"][type="text"]`)
    );

    rebindValidators();

    //#endregion

    $('.codeInputMask').on('input', function (e) {
    });

    $('#ApplicantsPersonalInformationModel_EncodedPartialStatus').on('change', function () {
        // Your onchange event handling logic here
        // For example, you can retrieve the selected value and perform actions based on it
        var selectedValue = $(this).val();
        $("#ApplicantsPersonalInformationModel_EncodedStatus").val(selectedValue);
    });

    //encodedStatusdropDown.on('load', function (options) {
    //});

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

    $('.radio-pcRadio input[type="radio"]').on('change', function () {
        let $inputField = $("[name='Form2PageModel.PendingCase']");

        if ($("#pcRadioBtn1").is(":checked")) {
            $inputField.prop('disabled', false).prop('required', true);
        } else {
            $inputField.prop('disabled', true).prop('required', false);
            $inputField.val(null);
        }
    });

    $(".radio-pdRbtn input[type='radio']").on('change', function () {
        let $inputField = $("[name='Form2PageModel.PastDue']");

        if ($("#pdRbtn1").is(":checked")) {
            $inputField.prop('disabled', false).prop('required', true);
        } else {
            $inputField.prop('disabled', true).prop('required', false);
            $inputField.val(null);
        }
    });

    $(".radio-bcRbtn input[type='radio']").on('change', function () {
        let $inputField = $("[name='Form2PageModel.BouncingChecks']");

        if ($("#bcRbtn1").is(":checked")) {
            $inputField.prop('disabled', false).prop('required', true);
        } else {
            $inputField.prop('disabled', true).prop('required', false);
            $inputField.val(null);
        }
    });

    $(".radio-maRbtn input[type='radio']").on('change', function () {
        let $inputField = $("[name='Form2PageModel.MedicalAdvice']");

        if ($("#maRbtn1").is(":checked")) {
            $inputField.prop('disabled', false).prop('required', true);
        } else {
            $inputField.prop('disabled', true).prop('required', false);
            $inputField.val(null);
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

            if (!isValid) {
                // If validation fails, prevent navigation to the next step
                return false;
            } else {
                // Hide the current form
                currentForm.addClass('fade').prop('hidden', true);

                // Show the previous form
                prevForm.removeClass('fade').prop('hidden', false);
            }

            // If current form is "form2", return without proceeding to next step
            if (currentFormName == "form2") {
                $("#form2").removeClass('fade').prop('hidden', false);
                return;
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

            //$("#form3").addClass('was-validated').prop('hidden', true);

            // Hide the current form
            currentForm.addClass('fade').prop('hidden', true);

            // Show the next form
            nextForm.removeClass('fade').prop('hidden', false);

            // Always return true to allow navigation to the previous step
            return true;
        }
    });

    $(function () {
        initializeRadioBtnMisc();

        $('input[name="customRadio1"]').on('change', function () {
            // Get the value of the selected radio button
            encodedStatusdropDown.unlock();

            var selectedOptionText = $('input[name="customRadio1"]:checked').attr('data-name');

            if (selectedOptionText === "Application Completion") {
                $("#ApplicantsPersonalInformationModel_EncodedStage").val(2);

                encodedStatusdropDown.clearOptions();
                // Developer   //LGU
                if (roleId == 5 || roleId == 2) {
                    $("#div_stageapprovalsettings").removeClass('d-none');

                    var optionsToAdd = [
                        { value: '4', text: 'Pagibig Verified' },
                        { value: '6', text: 'Post Submitted' },
                        { value: '7', text: 'Developer Approved' },
                    ];

                    encodedStatusdropDown.addOption(optionsToAdd);
                } else {
                    encodedStatusdropDown.clearOptions();

                    var optionsToAdd = [
                        { value: '4', text: 'Pagibig Verified' },
                        { value: '6', text: 'Post Submitted' },
                        { value: '8', text: 'Pagibig Approved' }
                    ];

                    encodedStatusdropDown.addOption(optionsToAdd);
                }
            } else if (selectedOptionText === "Credit Verification") {
                $("#ApplicantsPersonalInformationModel_EncodedStage").val(1);

                encodedStatusdropDown.clearOptions();

                // Developer   //LGU
                if (roleId == 5 || roleId == 2) {
                    $("#div_stageapprovalsettings").removeClass('d-none');

                    var optionsToAdd = [
                        { value: '0', text: 'Application In Draft' },
                        { value: '1', text: 'Submitted' },
                        { value: '3', text: 'Developer Verified' }
                    ];

                    encodedStatusdropDown.addOption(optionsToAdd);
                } else {
                    encodedStatusdropDown.clearOptions();

                    var optionsToAdd = [
                        { value: '0', text: 'Application In Draft' },
                        { value: '1', text: 'Submitted' },
                        { value: '4', text: 'Pagibig Verified' },
                    ];

                    encodedStatusdropDown.addOption(optionsToAdd);
                }
            }
        });
    });

    //#region Methods

    //$(document).ready(function () {
    //    loadloanParticularInformation(applicantInfoIdVal);
    //    loadSpouseInformation(applicantInfoIdVal);
    //    loadBorrowerInformation(applicantInfoIdVal);
    //    loadCollateralInformation(applicantInfoIdVal);
    //    loadForm2PageInformation(applicantInfoIdVal);
    //    initializeRadioBtnMisc();
    //});

    function loadloanParticularInformation(id) {
        $.ajax({
            url: baseUrl + "Applicants/GetLoanParticularsByApplicantInfoData/" + id,
            method: 'Get',
            success: function (response) {
                //$(`select[name='LoanParticularsInformationModel.PurposeOfLoanId']`).data('selectize').setValue(response.PurposeOfLoanId);

                //purposeOfLoanDropdown.setValue(response.PurposeOfLoanId);

                //$(`[name='LoanParticularsInformationModel.ExistingHousingApplicationNumber']`).val(response.ExistingHousingApplicationNumber);
                //$(`[name='LoanParticularsInformationModel.ExistingChecker']`).prop("checked", response.ExistingChecker);
                //$(`[name='LoanParticularsInformationModel.DesiredLoanAmount']`).val(response.DesiredLoanAmount);
                //$(`[name='LoanParticularsInformationModel.DesiredLoanTermYears']`).val(response.DesiredLoanTermYears);
                //$(`[name='LoanParticularsInformationModel.RepricingPeriod']`).val(response.RepricingPeriod);

                //modeofPaymentDropdown.setValue(response.ModeOfPaymentId);

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

        // Prevent form submission when "Enter" key is pressed
        $form.on("keydown", function (e) {
            if (e.key === "Enter") {
                e.preventDefault();
            }
        });

        $form.on("submit", function (e) {
            e.preventDefault();
            let formData = new FormData(e.target);

            if ($(this).valid() == false) {
                messageBox("Please fill out all required fields!", "danger", true);
                return;
            }

            //console.log(telNoArray.filter(iti => iti.a.hasAttribute('required') || iti.a.value));

            if (!(telNoArray.filter(iti => iti.a.hasAttribute('required') || iti.a.value).every(iti => iti.isValidNumberPrecise()))) {
                let arrayOfInvalidTels = telNoArray.filter(iti => (iti.a.hasAttribute('required') || iti.a.value) && !iti.isValidNumberPrecise());
                //let listOfInvalids = ``;

                for (var index in arrayOfInvalidTels) {
                    let itiElement = arrayOfInvalidTels[index];
                    $(`span[name="${itiElement.a.name}.Error"]`).html(intlTelErrors[itiElement.getValidationError()]);
                }

                //console.log(arrayOfInvalidTels);

                messageBox("Some contact numbers entered are invalid, please double check or re-enter them!", "danger", true);
                return;
            }

            for (var index in telNoArray) {
                let itiElement = telNoArray[index];
                formData.set(itiElement.a.name, itiElement.getNumber());
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
                    // Success message handling
                    let recordId = $("input[name='User.Id']").val();
                    console.log(recordId);
                    let type = (recordId == 0 ? "Added!" : "Updated!");
                    let successMessage = `Beneficiary Successfully ${type}`;
                    messageBox(successMessage, "success", true);

                    //Redirect handling

                    if (applicantInfoIdVal == 0) {
                        setTimeout(function () {
                            $("#beneficiary-overlay").addClass('d-none');
                            window.location.href = "/Applicants/HLF068/" + response;
                        }, 2000);
                    }

                    else {
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

        // Set checked status for PendingCase radio buttons
        $("#pcRadioBtn1").prop("checked", !!pendingCaseValue);

        $("[name='Form2PageModel.PendingCase']").prop("disabled", !pendingCaseValue);

        // Set checked status for PastDue radio buttons
        $("#pdRbtn1").prop("checked", !!pastDueValue);

        $("[name='Form2PageModel.PastDue']").prop("disabled", !pastDueValue);

        // Set checked status for BouncingChecks radio buttons
        $("#bcRbtn1").prop("checked", !!bouncingChecksValue);

        $("[name='Form2PageModel.BouncingChecks']").prop("disabled", !bouncingChecksValue);

        // Set checked status for MedicalAdvice radio buttons
        $("#maRbtn1").prop("checked", !!medicalAdviceValue);

        $("[name='Form2PageModel.MedicalAdvice']").prop("disabled", !medicalAdviceValue);

        if (applicantInfoIdVal != 0) {
            $("#pcRadioBtn2").prop("checked", !pendingCaseValue);

            $("#pdRbtn2").prop("checked", !pastDueValue);

            $("#bcRbtn2").prop("checked", !bouncingChecksValue);

            $("#maRbtn2").prop("checked", !medicalAdviceValue);
        }
    }
    //#endregion
});