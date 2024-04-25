$(function () {

    const roleId = $("#txt_roleId").val();

    setFormViewOnly();
    rebindValidators();

    $("#btn_edit").on('click', function () {
        $("#btn_edit").addClass("active");

        $("#frm_hlf068 input, #frm_hlf068 select, #frm_hlf068 textarea").removeAttr("readonly");
        $("#frm_hlf068 input, #frm_hlf068 select, #frm_hlf068 textarea").removeClass("disabled");
        $('.calendarpicker, .timepicker, .present-calendar-picker').prop('disabled', false);
        $(`#frm_hlf068 input[type="checkbox"]`).removeAttr("disabled");
        $('input[type="radio"]').prop('disabled', false);

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

        initializeRadioBtnMisc();
    });

    $("#btn_pdf").on('click', function () {
        let applicationCode = $("#ApplicantsPersonalInformationModel_Code").val();
        let link = baseUrl + "Report/LatestHousingForm/" + applicationCode;

        window.open(link, '_blank');
    });

    function setFormViewOnly() {
        $("#frm_hlf068 input, #frm_hlf068 select, #frm_hlf068 textarea").attr("readonly", true);
        $("#frm_hlf068 input, #frm_hlf068 select, #frm_hlf068 textarea").addClass("disabled");
        $(`#frm_hlf068 input[type="checkbox"]`).attr("disabled", true);
        $('.calendarpicker, .timepicker, .present-calendar-picker').prop('disabled', true);
        $('input[type="radio"]').prop('disabled', true);

        $('#frm_hlf068').find('.selectized').each(function (i, e) {
            e.selectize.lock();
        });

        initializeRadioBtnMisc();
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

            // re-enable checkboxes on submission
            // Important: this snippet should come first before validation and FormData varialble
            $(`#frm_hlf068 input[type="checkbox"]`).removeAttr("disabled");

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
                        //beneficiary
                        if (roleId != 4) {
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
});