$(function () {
    //All Variables
    let $form = $("#frmEmail");

    //All Initialization
    //$("#btn_edit").attr('disabled', true);
    $("#btn_send").attr('disabled', true);

    fieldValidation();
    emailValidators();

    var tblEmail = $("#tblEmailLogs").DataTable({
        ajax: {
            url: "/EmailLog/GetEmailList",
            dataSrc: ""
        },
        language: {
            emptyTable: "No Email records found!",
            processing: "<div class='text-center'><span class='spinner-border spinner-border-sm'></span> Loading...</div>",
            previous: "<i class='mdi mdi-chevron-left'>",
            next: "<i class='mdi mdi-chevron-right'>"
        },
        columns: [
            {
                data: "Id",
                class: "text-center",
                width: "5%",
                title: "Id"
            },
            {
                data: "Status",
                class: "text-center",
                width: "5%",
                render: function (data, type, row) {
                    return data;
                }
            },
            {
                data: "ReferenceNo",
                class: "text-center",
                width: "10%",
                render: function (data, type, row) {
                    return data;
                }
            },
            {
                data: "ReceiverName",
                class: "text-center",
                width: "35%",
                render: function (data, type, row) {
                    return data;
                }
            },
            {
                data: "Description",
                class: "text-center text-truncate",
                width: "35%",
                render: function (data, type, row) {
                    if (data.length > 80) {
                        return `<span title="${data}">${data.substring(0, 50)}...</span>`;
                    }
                    else {
                        return `${data}`;
                    }
                }
            },
            {
                data: "SenderName",
                class: "text-center",
                width: "10%",
                render: function (data, type, row) {
                    return data;
                }
            },

        ],
        drawCallback: function () {
            $(".dataTables_paginate > .pagination").addClass("pagination-rounded")
            $('[data-toggle="tooltip"]').tooltip();
        },
        processing: true,
        //select: {
        //    style: 'single',
        //    info: false
        //},
        pageLength: 10,
        select: 'multi',
        ordering: true,
        order: [0, "desc"],
        saveState: true,
        filter: true,
        searching: true,
        paging: true,
        scrollX: true,
        searchHighlight: true
    });

    tblEmail.on('select deselect draw', function () {
        var all = tblEmail.rows({ search: 'applied' }).count();
        var selectedRows = tblEmail.rows({ selected: true, search: 'applied' }).count();
        var id = tblEmail.rows({ selected: true }).data().pluck("Id").toArray().toString();
        var refid = tblEmail.rows({ selected: true }).data().pluck("ReferenceId").toArray().toString();
        var vendorId = tblEmail.rows({ selected: true }).data().pluck("ReceiverId").toArray().toString();
        var refno = tblEmail.rows({ selected: true }).data().pluck("ReferenceNo").toArray().toString();
        var creator = tblEmail.rows({ selected: true }).data().pluck("SenderId").toArray().toString();
        var status = tblEmail.rows({ selected: true }).data().pluck("MailStatus").toArray().toString();

        //$("#btn_edit").attr({
        //    "disabled": !(selectedRows === 1),
        //    "data-code": refno,
        //    "data-id": id
        //});
        var rowsOnPage = tblEmail.rows({ page: 'current', selected: true, search: 'applied' }).indexes().toArray();
        var allRows = tblEmail.rows({ selected: true, search: 'applied' }).indexes().toArray();
        if (rowsOnPage.length != allRows.length) {
            tblEmail.rows().deselect();
        }

        $("#btn_send").attr({
            "disabled": !(selectedRows === 1),
            "data-code": refno,
            "data-vendor": vendorId,
            "data-id": id
        });

        //$("#btnSave").attr('disabled', (selectedRows === 1));

        if (selectedRows === 1) {
            GetById(id);
            manipulateFields(true, true);
            dynamicTitle("Update Email Log", "Update");
        }
        else if (selectedRows > 1) {
            $("#btn_send").attr({
                "disabled": false,
                "data-code": refno,
                "data-id": id
            });
            manipulateFields(true, false)
        }
        else {
            manipulateFields(false);
            clearForm();
            dynamicTitle("Add Email Log", "Save");
            $("#Status").val(0);
        }
    });

    $("#tblEmailLogs_filter, #tblEmailLogs_length").hide();

    //All Events

    $(document).on('keyup input', "#txtsearch", function () {
        tblEmail.search(this.value).draw();
    })
    $(document).on('click', "#btn_refresh", function () {
        $("#txtsearch").val("").trigger('input');
        tblEmail.ajax.reload();
    })
    $(document).on('click', "#btnReset", function () {
        tblEmail.rows({ selected: true }).deselect();
    })
    $(document).on('click', "#btn_send", function () {
        var refnos = $(this).attr('data-code');
        var vendors = $(this).attr('data-vendor');

        $("#ReferenceNos").val(refnos);
        $("#ReferenceIds").val(vendors);

        $("#frmResend").submit();
    })
    //$("#btn_edit").on('click', function () {
    //    var categoryid = $(this).attr('data-id');

    //    GetById(categoryid);
    //    manipulateFields(false);
    //    dynamicTitle("Edit Budget Group Category", "Update");
    //})

    //All Functions
    function GetById(id) {
        $.ajax({
            url: "/EmailLog/GetEmailById/",
            data: {
                Id: id,
            },
            beforeSend: function () {
                $(".overlay").attr('hidden', false);
            },
            success: function (logs) {
                var newdate = convertDate(logs.Date, "YYYY-MM-DD");
                $("#Id").val(logs.Id);
                $("#Description").val(logs.Description);
                $("#ReferenceNo").val(logs.ReferenceNo);
                $("#ReferenceId").val(logs.ReferenceId);
                $("#ReceiverId").val(logs.ReceiverId);
                $("#Status").val(logs.Status);
                $("#SenderId").val(logs.SenderId);
                $("#Date").val(newdate);
              
            },
            complete: function () {
                $(".overlay").attr('hidden', true);
            },
            error: function () {
            }
        })
    }

    function clearForm() {
        $("#ReferenceNo").val("");
        $("#Description").val("");
        $("#ReceiverName").val("");
        $("#ReferenceId").val(0);
        $("#Id").val(0);
        
    }

    function manipulateFields(view = true, Isupdate = true) {
        $("#Description").attr('readonly', Isupdate == true ? false : true);
        $("#btnSave").attr('disabled', Isupdate == true ? false : true);
        $("#ReferenceNo").attr('readonly', view == true ? true : false);
        $("#ReceiverName").attr('readonly', view == true ? true : false);
        $("#ReferenceId").attr('readonly', view == true ? true : false);

         
    }

    function dynamicTitle(title = "", btnName = "") {
        $("#cardTitle").html(`<i class="fe-${btnName == "Update" ? "edit" : "plus"}"></i>&nbsp; ${title}`);
        $("#btnSave").html(`<i class="fe-${btnName == "Update" ? "edit" : "save"}"></i>&nbsp; ${btnName}`);
    }

    function fieldValidation() {
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
                toasterBox("Please fill out all required fields!", "danger", true);
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
                    button.html(`<span class='spinner-border spinner-border-sm'></span> ${recordId == 0 ? "Saving!" : "Updating!"}...`);
                    button.attr({ disabled: true });
                },
                success: function (response) {
                    let type = `Successfully ${recordId == 0 ? "Saved!" : "Updated!"}`;
                    toasterBox(type, "success");
                    $("#btnReset").click()
                    button.attr({ disabled: false });
                    button.html("<i class='fa fe-save'></i> Save");
                    tblEmail.ajax.reload(null, false);
                },
                error: function (response) {
                    toasterBox(response.responseText, "danger");
                    button.html("<i class='fa fe-save'></i> Save");
                    button.attr({ disabled: false });
                }
            });
        });
    }

    function emailValidators() {
        var $form = $("#frmResend");
        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate("unobtrusiveValidation");

        $form.submit(function (e) {
            e.preventDefault();
            $form.data("validator").settings.ignore = ".hidden";
            var formData = new FormData(e.target);
            var btn = $('#btn_send');
            var getId = $('#ReferenceNos').val();
            var flag = parseInt($("#flag").val());
            var message = flag === 0 ? 'saved' : 'updated';

            if ($form.valid()) {
                $.ajax({
                    url: $(this).attr("action"),
                    method: $(this).attr("method"),
                    data: formData,
                    cache: false,
                    contentType: false,
                    processData: false,
                    beforeSend: function () {
                        if (getId != "") {
                            if (!(btn.prop('disabled'))) {
                                btn.attr({ disabled: true }).html("<span class='spinner-border spinner-border-sm'></span> Sending...");
                            }
                        }
                    },
                    success: function (response) {
                        if (response == "success") {
                            setTimeout(function () {
                                toasterBox(`Successfully Sent`, 'success');
                                btn.attr({ disabled: false }).html("<i class='fe-send'></i>&nbsp; Resend");
                            }, 1000)
                        }
                        setTimeout(function () {
                            tblEmail.ajax.reload();
                        }, 1000)
                    },
                    error: function (error) {
                        toasterBox(error.responseText, "warning");
                        btn.attr({ disabled: false }).html("<i class='fe-send'></i>&nbsp; Resend");
                    }
                });
            }
            else {
                toasterBox("Please fill out all required fields!", "danger");
            }
        });
    }
});