$(function () {
    let frmLogin = $("#frmLogin");
    let submitButton = $('button[type="submit"]');



    frmLogin.on('submit',function (e) {
        e.preventDefault();

        if (!$(this).valid())
            return;

        $.ajax({
            url: $(this).attr('action') + '?returnUrl=' + $("#returnUrl").html(),
            method: $(this).attr('method'),
            data: $(this).serialize(),
            beforeSend: function () {
                $("#frm_login :input").attr({ disabled: true });

                submitButton.html('<span class="spinner-border spinner-border-sm"></span> Loading...');
                submitButton.attr({ disabled: true });
            },
            success: function () {
                submitButton.html('<span class="spinner-border spinner-border-sm"></span> Signing In...');
                var logoff = "/Account/LogOff";
                if ($("#returnUrl").html() == "" || $("[name='ReturnUrl']").val() == logoff) {
                    window.location.href = "/";
                } else {
                    window.location.href = $("[name='ReturnUrl']").val();
                }

                location.reload();

                submitButton.html('<span class="spinner-border spinner-border-sm"></span> Redirecting');
                submitButton.attr({ disabled: true });
            },
            error: function (response) {
                $("#Password").val("");
                alertMessage(response.responseText, 'danger');

                $("#frm_login :input").attr({ disabled: false });
                submitButton.attr({ disabled: false });
                submitButton.html('Log In');
            }
        });

    });

    function alertMessage(message, type, container = "#message") {
        $(container).fadeOut();
        var title = "", alert = "";

        switch (type) {
            case "danger":
                title = '<i class="icon fas fa-ban"></i> Alert!';
                break;
            case "info":
                title = '<i class="icon fas fa-info"></i> Info!';
                break;
            case "warning":
                title = '<i class="icon fas fa-exclamation-triangle"></i> Warning!';
                break;
            case "success":
                title = '<i class="icon fas fa-check"></i> Sucess!';
                break;
            default:
        }

        if (typeof (message) == "object") {
            alert = `<div class="alert alert-${type} alert-dismissible col"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button><h5>${title}</h5><div>Please see error logs for more information</div></div>`;
            alert = `<div class="alert alert-${type}" role="alert"><i class="mdi mdi-block-helper me-2"></i> Please see error logs for more information</div>`
        } else {
            alert = `<div class="alert alert-${type} alert-dismissible col"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button><h5>${title}</h5><div>${message}</div></div>`;
            alert = `<div class="alert alert-${type}" role="alert"><i class="mdi mdi-block-helper me-2"></i> ${message}</div>`
        }

        $(container).fadeIn(10, function () {
            $(container).html(alert);
        });
    }
});