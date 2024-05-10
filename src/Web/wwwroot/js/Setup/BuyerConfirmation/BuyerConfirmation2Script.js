$(function () {
    $(".selectize").selectize({
        search: false
    });


    $('.calendarpicker').flatpickr({
        dateFormat: "m/d/Y",
    });

    rebindValidators();
    function rebindValidators() {
        let $form = $("#frm_bcf");
        let button = $("#btn_savebcf");

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
                    $("#bcf-overlay").removeClass('d-none');
                },
                success: function (response) {
                    // Success message handling
                    let recordId = $("input[name='BuyerConfirmation.Id']").val();

                    let type = (recordId == 0 ? "Added!" : "Updated!");
                    let successMessage = `Buyer Confirmation Successfully ${type}`;
                    messageBox(successMessage, "success", true);

                    //Redirect handling

                    var link = "Report/LatestBuyerConfirmationForm/" + response;

                    setTimeout(function () {
                        $("#beneficiary-overlay").addClass('d-none');
                        // Redirect to the specified location
                        window.location.href = baseUrl + link;
                    }, 2000);

                    // Reset button state
                    button.attr({ disabled: false });
                    button.html("<span class='mdi mdi-content-save-outline'></span> Save");
                },
                error: function (response) {
                    // Error message handling
                    messageBox(response.responseText, "danger");
                    $("#bcf-overlay").addClass('d-none');
                    button.html("<span class='mdi mdi-content-save-outline'></span> Save");
                    button.attr({ disabled: false });
                }
            });
        });
    }
});