$(function () {
    //ALL VARIABLES
    var tblEmailSetup;
    var tblEmailSetup_tbody = "#tbl_emailSetup tbody";
    let $form = $("#frmEmailSetup");
    //ALL INITIALIZATION
    rebindValidator()
    initializeDecimalInputMask()
    $("#btnReset").trigger("click")
    //ALL EVENTS

    tblEmailSetup = $("#tbl_emailSetup").DataTable({
        ajax: {
            url: `${baseUrl}EmailSetup/GetList`,
            dataSrc: "",
        },
        language: {
            emptyTable: "No Email Setup record found!",
            processing: "<div class='text-center'><span class='spinner-border spinner-border-sm'></span> Loading...</div>",
            previous: "<i class='mdi mdi-chevron-left'>",
            next: "<i class='mdi mdi-chevron-right'>"
        },
        columns: [
            {
                data: "Email",
                className: "text-center",
            },
            {
                data: "Password",
                className: "text-center",
            },
            {
                data: "Host",
                className: "text-center",
            },
            {
                data: "DisplayName",
                className: "text-center",
            },
            {
                data: "Port",
                className: "text-center",
            },
            {
                data: "CompanyName",
                className: "text-center",
            },

        ],
        drawCallback: function () {
            $(".dataTables_paginate > .pagination").addClass("pagination-rounded")
            $('[data-toggle="tooltip"]').tooltip();
        },
        processing: true,
        select: {
            style: 'single',
            info: false
        },
        pageLength: 10,
        ordering: true,
        order: [0, "desc"],
        filter: true,
        paging: true,
        scrollX: true,
        searchHighlight: true
    });

    $("#tbl_emailSetup_filter, #tbl_emailSetup_length").hide();
    $(".txtSearch").on('keyup, input', function () {
        tblEmailSetup.search(this.value).draw();
    })
    tblEmailSetup.on('select deselect draw', function () {
        var all = tblEmailSetup.rows({ search: 'applied' }).count();
        var selectedRows = tblEmailSetup.rows({ selected: true, search: 'applied' }).count();
        var id = tblEmailSetup.rows({ selected: true }).data().pluck("Id").toArray().toString();
        var email = tblEmailSetup.rows({ selected: true }).data().pluck("Email").toArray().toString();
        var host = tblEmailSetup.rows({ selected: true }).data().pluck("Host").toArray().toString();
        var port = tblEmailSetup.rows({ selected: true }).data().pluck("Port").toArray().toString();
        var dpname = tblEmailSetup.rows({ selected: true }).data().pluck("DisplayName").toArray().toString();
        var password = tblEmailSetup.rows({ selected: true }).data().pluck("Password").toArray().toString();
        var isdefault = tblEmailSetup.rows({ selected: true }).data().pluck("IsDefault").toArray().toString();

        $("#btn_edit").attr({
            "disabled": !(selectedRows === 1),
            "data-id": id,
            "data-email": email,
            "data-pass": password,
            "data-host": host,
            "data-port": port,
            "data-isdefault": isdefault == "true" ? true : false,
            "data-displayName": dpname
        });
        $("#btn_delete").attr({
            "disabled": !(selectedRows === 1),
            "data-id": id,
        });

        //$("#IsDisabled").val(isactive);
        //isactive == "true" ? $('#IsDisabled').prop('checked', true) : $('#IsDisabled').prop('checked', false);

        $('.field-validation-error').hide();
    });
    $('#IsDefault').on('change', function () {
        $(this).val($(this).prop('checked') ? true : false);
    });
    $('#btnReset').click(function () {
        $('#headerTitle').html('<span class="fe-plus"></span>&nbsp; Add Email Setup');
        $('#btnSave').html('<i class="fas fa-save"></i> Save');
        $('.odd').removeClass('selected')
        $('.even').removeClass('selected')
        tblEmailSetup.rows({ selected: true }).deselect();
        resetForm();
        $("#Id").attr('value', 0);
    })
    $('#btn_edit').click(function () {
        var id = $(this).attr("data-id").trim();
        var email = $(this).attr("data-email").trim();
        var pass = $(this).attr("data-pass").trim();
        var host = $(this).attr("data-host").trim();
        var port = $(this).attr("data-port").trim();
        var isdefault = $(this).attr("data-isdefault").trim();
        var dpname = $(this).attr("data-displayName").trim();
        $('#headerTitle').html('<span class="fe-edit"></span>&nbsp; Edit Email Setup');
        $('#btnSave').html('<i class="fe-edit"></i> Update');
        $("#Id").val(id);
        $("#Email").val(email);
        $("#Password").val(pass);
        $("#Host").val(host);
        $("#Port").val(port);
        $("#DisplayName").val(dpname);
        $("#IsDefault").prop("checked", isdefault);
        resetForm(id);
    })
    $('#btn_refresh').click(function () {
        tblEmailSetup.ajax.reload();
        $("#btnReset").trigger('click');
    })

    $.contextMenu({
        selector: tblEmailSetup_tbody,
        animation: { duration: 200, show: 'fadeIn', hide: 'fadeOut' },
        items: {
            "refresh": {
                name: "Refresh",
                icon: "fas fa-redo",
                accesskey: 'r',
                callback: function () {
                    tblEmailSetup.ajax.reload();
                }
            },

            "edit": {
                name: "Edit",
                icon: "fas fa-edit",
                accesskey: 'e',
                callback: function () {
                    $("#btn_edit").click();
                },
                disabled: function () {
                    return this.data('Edit-Disabled');
                }
            },
        }
    })
    $(tblEmailSetup_tbody).on('contextmenu', 'tr', function () {
        var selectedRows = tblEmailSetup.rows({ selected: true, search: 'applied' }).count();

        if (selectedRows == 1) {
            tblEmailSetup.rows().deselect();
            tblEmailSetup.rows(this).select();
        } else {
            tblEmailSetup.rows(this).select();
        }
        selectedRows = tblEmailSetup.rows({ selected: true, search: 'applied' }).count();

        let $trigger = $(tblEmailSetup_tbody);

        $trigger.data('Edit-Disabled', !(selectedRows === 1));
    });
    //ALL FUNCTIONS
    function rebindValidator() {
        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate("unobtrusiveValidation");

        $form.submit(function (e) {
            e.preventDefault();
            $form.data("validator").settings.ignore = ".hidden";
            let formData = new FormData(e.target);
            let recordId = $("#Id").val();
            let button = $("#btnSave");

            if ($form.valid() == false) {
                messageBox("Please fill out all required fields!", "danger", true);
                return;
            }

            $.ajax({
                url: $form.attr("action"),
                method: $form.attr("method"),
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function () {
                    button.html("<span class='spinner-border spinner-border-sm'></span> Saving...");
                    button.attr({ disabled: true });
                },
                success: function (response) {
                    let type = `Successfully ${recordId == 0 ? "Saved!" : "Updated!"}`;
                    messageBox(type, "success");
                    $("#btnReset").click()
                    button.attr({ disabled: false });
                    button.html("<i class='fas fa-save'></i> Save");
                    tblEmailSetup.ajax.reload(null, false);
                },
                error: function (response) {
                    messageBox(response.responseText, "danger");
                    button.html("<i class='fas fa-save'></i> Save");
                    button.attr({ disabled: false });
                }
            });
        });
    }

    function resetForm(id = 0) {
        var $validator = $form.validate();
        var $errors = $form.find(".field-validation-error span");
        $errors.each(function () { $validator.settings.success($(this)); })
        $validator.resetForm();
        if (id == 0) {
            $("#frmEmailSetup .form-control, #frmEmailSetup textarea").val('');
            $("#Id").val(0);
            $("#CompanyId").val(0);
        }

        //$('form').find('.selectized').each(function (index, element) { element.selectize && element.selectize.clear() })

        //$(`[name='Attachment.listFile']`).fileinput('clear');
    }

    function initializeDecimalInputMask(classname = `[name="Port"]`, digits = 0, limiter = "9999", isallownegative = false) {
        let placeholder = "0";

        $(classname).inputmask({
            alias: 'decimal',
            rightAlign: false,
            groupSeparator: '',
            digits: digits, // Set digits to 0 to disallow decimals
            allowMinus: isallownegative,
            autoGroup: true,
            placeholder: placeholder,
            max: Number(limiter.replace(/[^-?0-9\.]+/g, ""))
        });
    }
})