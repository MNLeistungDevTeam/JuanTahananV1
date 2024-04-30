$(function () {
    const roleId = $("#txt_roleId").val();


    rebindValidators();

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