$(function () {
    const baseUrl = $("#txtBaseUrl").val();
    $("#BaseUrl").val(baseUrl);

    rebindValidator();

    function rebindValidator() {
        let $form = $("#frm_request_resetcredentials");

        $form.submit(function (e) {
            e.preventDefault();

            if (!$form.valid()) { return; }

            let formData = $form.serialize();

            $.ajax({
                url: $form.attr("action"),
                method: $form.attr("method"),
                data: formData,

                success: function (response) {
                    messageBox("request Successfully Sent.", "success");
                },
                error: function (response) {
                    messageBox(response.responseText, "danger", true);
                }
            });
        });
    }

    function messageBox(message, type = "success", isToastr = false, isTimed = true) {
        var title = "";

        switch (type) {
            case "danger": title = 'Alert!'; break;
            case "info": title = 'Information'; break;
            case "warning": title = 'Warning!'; break;
            case "success": title = 'Success!'; break;
            default:
        }

        if (type == "danger") type = "error";

        if (isToastr) {
            if (typeof (message) === "object") {
                console.log(message);
                toastr.error(message, title);
            } else {
                if (type == "danger" || type == "error")
                    toastr.error(message, title);
                else if (type == "info")
                    toastr.info(message, title);
                else if (type == "success")
                    toastr.success(message, title);
                else
                    toastr.warning(message, title);
            }
        }
        else if (isTimed) {
            if (typeof (message) === "object") {
                console.log(message);
                Swal.fire({
                    icon: type,
                    title: title,
                    html: 'Error Occurred! - Please see error logs for more information',
                    timer: 2000,
                    timerProgressBar: true
                });
            } else
                Swal.fire({
                    icon: type,
                    title: title,
                    html: message,
                    timer: 2000,
                    timerProgressBar: true
                });
        }
        else {
            if (typeof (message) === "object") {
                console.log(message);
                Swal.fire(title, 'Error Occurred! - Please see error logs for more information', type);
            } else Swal.fire(title, message, type);
        }
    }
});