$(function () {
    let encodedStatusdropDown = $("#ApplicantsPersonalInformationModel_EncodedStatus")[0].selectize;
    const encodedStageVal = $("#ApplicantsPersonalInformationModel_EncodedStage").val();
    const roleId = $("#txt_roleId").val();

    rebindValidators();
    encodedStatus(encodedStageVal);

    $("#ApplicantsPersonalInformationModel_EncodedStatus")[0].selectize.lock();

    $('input[name="customRadio1"]').on('change', function () {
        // Get the value of the selected radio button

        encodedStatusdropDown.unlock();

        var selectedOptionText = $('input[name="customRadio1"]:checked').attr('data-name');

        if (selectedOptionText === "Application Completion") {
            $("#ApplicantsPersonalInformationModel_EncodedStage").val(2);

            encodedStatusdropDown.clearOptions();
                     //developer         //lgu
            if (roleId ==  5 || roleId == 2) {
                $("#div_stageapprovalsettings").removeClass('d-none');

                var optionsToAdd = [

                    { value: '4', text: 'Pagibig Verified' },
                    { value: '6', text: 'Post Submitted' },
                    { value: '7', text: 'Developer Approved' },

                ];

                encodedStatusdropDown.addOption(optionsToAdd);
            }

            else {
                encodedStatusdropDown.clearOptions();

                var optionsToAdd = [
                    { value: '4', text: 'Pagibig Verified' },
                    { value: '6', text: 'Post Submitted' },
                    { value: '8', text: 'Pagibig Approved' }

                ];

                encodedStatusdropDown.addOption(optionsToAdd);
            }
        }

        else if (selectedOptionText === "Credit Verification") {
            $("#ApplicantsPersonalInformationModel_EncodedStage").val(1);

            encodedStatusdropDown.clearOptions();
               //Developer             //lgu
            if (roleId ==  5 || roleId ==  2) {
                $("#div_stageapprovalsettings").removeClass('d-none');

                var optionsToAdd = [
                    { value: '0', text: 'Application In Draft' },
                    { value: '1', text: 'Submitted' },
                    { value: '3', text: 'Developer Verified' }
                ];

                encodedStatusdropDown.addOption(optionsToAdd);
            }

            else {
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

                    let encodedStageVal = $("#ApplicantsPersonalInformationModel_EncodedStatus").val();

                    //Redirect handling

                    if (applicantInfoIdVal == 0) {
                        //not applicationindraft
                        if (encodedStageVal > 1) {
                            setTimeout(function () {
                                $("#beneficiary-overlay").addClass('d-none');
                                window.location.href = "/Applicants/Details/" + response;
                            }, 2000);
                        }
                        else {
                            setTimeout(function () {
                                $("#beneficiary-overlay").addClass('d-none');
                                window.location.href = "/Applicants/HLF068/" + response;
                            }, 2000);
                        }
                    }

                    else {
                        var link = "Applicants/Beneficiary";

                           //beneficiary
                        if (roleId !=  4) {
                            link = "Applicants/ApplicantRequests";
                        }
                        setTimeout(function () {
                            $("#beneficiary-overlay").addClass('d-none');
                            // Redirect to the specified location
                            window.location.href = baseUrl + link;
                        }, 2000);
                    }

                    // Reset button state
                    button.attr({ disabled: false });
                    button.html("<span class='mdi mdi-content-save-outline'></span> Submit");
                },
                error: function (response) {
                    // Error message handling
                    messageBox(response.responseText, "danger");
                    $("#beneficiary-overlay").addClass('d-none');
                    button.html("<span class='mdi mdi-content-save-outline'></span> Submit");
                    button.attr({ disabled: false });
                }
            });
        });
    }

    function encodedStatus(encodedStage) {
        if (encodedStageVal == 1) {
            $('input[name="customRadio1"][data-name="Application Completion"]').prop('checked', true).prop("disabled", true);

            $('#customRadio3').prop('checked', true);
        }

        else if (encodedStageVal === 2) {
            $('input[name="customRadio1"][data-name="Credit Verification"]').prop('checked', true).prop("disabled", true);;

            $('#customRadio5').click();
        }
    }
});