$(() => {
    "use strict";

    $('#rootwizard').bootstrapWizard({
        'onNext': function (tab, navigation, index) {
            var form = $($(tab).data("targetForm"));
            if (form) {
                form.addClass('was-validated');
                if (form[0].checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                    return false;
                }
            }
        }
    });

    $('.submit-form').on('click', (e) => {
        e.preventDefault();
        $('#form2').trigger('submit'); // Submit the form if the user clicks OK
    })


    $('#form2').on('submit', function (e) {
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
        $.ajax({
            type: 'POST',
            url: '/Applicants/SaveHLF068',
            data: combinedData,
            beforeSend: function () {
                loading("Saving Changes...");
            },
            success: async function (response) {
                messageBox("Successfully", "success", true);
                location.href = "/Applicants/HLF068?UserId=" + response;
                loader.close();
            },
            error: async function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                messageBox(jqXHR.responseText, "danger", true);
                loader.close();
            }
        });
    })
    $('.calendarpicker').flatpickr();
});