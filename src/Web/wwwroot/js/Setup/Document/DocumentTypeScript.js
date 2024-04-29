"use strict";

const $modal = $('#modal-document');
const $form = $("#document_form");
var tbl_verificationDocument;
var tbl_applicationDocument;
var tbl_Document;

const parentDocumentVal = $(`[name='DocumentType.ParentId']`).attr('data-value');
$(function () {
    rebindValidators();

    $(".custom_selectize").selectize();

    var $parentDocumentdropDown, parentDocumentdropDown;

    $parentDocumentdropDown = $(`[name='DocumentType.ParentId']`).selectize({
        valueField: 'Id',
        labelField: 'Description',
        searchField: 'Description',
        preload: true,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + 'Document/GetAllParentDocuments',
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

    parentDocumentdropDown = $parentDocumentdropDown[0].selectize;





    tbl_Document = $("#tbl_document").DataTable({
        ajax: {
            url: baseUrl + "Document/GetAllDocumentType",

            dataSrc: ''
        },
        language: {
            processing: "<div class='text-center'><span class='spinner-border spinner-border-sm'></span> Loading...</div>",
            previous: "<i class='mdi mdi-chevron-left'>",
            next: "<i class='mdi mdi-chevron-right'>"
        },
        columns: [
            {
                data: "Id"
            },
            {
                data: "Code",
                class: "text-left align-middle",
                render: function (data, type, row) {
                    return data;
                }
            },
            {
                data: "Description",
                class: "text-left align-middle",
                render: function (data, type, row) {
                    return data;
                }
            },
            {
                data: "VerificationTypeDescription",
                class: "text-left align-middle",
                render: function (data, type, row) {
                    return data;
                }
            },

            {
                data: "FileFormat",
                class: "text-left align-middle",
                render: function (data, type, row) {
                    return data
                }
            },

            {
                data: "ParentDocumentName",
                class: "text-left align-middle",
                render: function (data, type, row) {
                    return data
                }
            },

            {
                data: "CreatedBy",
                class: "text-left align-middle"
            },
            {
                data: "DateCreated",
                class: "text-center align-middle",
                render: function (data, type, row) {
                    return convertDate(data, 'MM/DD/YYYY');
                }
            },
            {
                data: "ModifiedBy",
                class: "text-left align-middle"
            },
            {
                data: "DateModified",
                class: "text-center align-middle",
                render: function (data, type, row) {
                    return convertDate(data, 'MM/DD/YYYY');
                }
            },
        ],
        drawCallback: function () {
            //let api = this.api();
            //var info = api.page.info();
            //if (info.pages != 0 && (api.page() > 0 && api.rows({ page: 'current' }).count() === 0)) {
            //    api.page('first').state.save();
            //    //window.location.reload();
            //    $("#btnRefresh").attr({ disabled: true });
            //    tbl_transaction.ajax.reload();
            //}

            //var rowCount = api.rows().data().toArray().length

            $(".dataTables_paginate > .pagination").addClass("pagination-rounded");
            $("#btnRefresh").attr({ disabled: false });
        },
        initComplete: function () {
            //resourceCounter();
        },
        columnDefs: [
            {
                targets: 0,
                orderable: false,
                className: "select-checkbox",
                data: null,
                render: function () {
                    return "";
                }
            }
        ],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        select: true,
        order: [[2, 'desc']],
        pageLength: 20,
        rowId: "Id",
        processing: true,
        scrollX: true,
        scrollY: "25vh",
        scrollCollapse: true
    });

    $("#tbl_document_filter, #tbl_document_length").hide();
    tbl_Document.on('select deselect draw', function () {
        var all = tbl_Document.rows({ search: 'applied' }).count();
        var selectedRows = tbl_Document.rows({ selected: true, search: 'applied' }).count();
        var id = tbl_Document.rows({ selected: true }).data().pluck("Id").toArray().toString();

        var documentTypeName = tbl_Document.rows({ selected: true }).data().pluck("Description").toArray().toString();
        var documentTypeCode = tbl_Document.rows({ selected: true }).data().pluck("Code").toArray().toString();
        var verificationType = tbl_Document.rows({ selected: true }).data().pluck("VerificationType").toArray().toString();
        var verificationDocumentId = tbl_Document.rows({ selected: true }).data().pluck("DocumentVerificationId").toArray().toString();
        var fileType = tbl_Document.rows({ selected: true }).data().pluck("FileType").toArray().toString();
        var fileParentId = tbl_Document.rows({ selected: true }).data().pluck("ParentId").toArray().toString();

        // Tick select-all based on row count;
        $("#select-all-document").prop("checked", (all == selectedRows && all > 0));

        $("#btn_add").attr({
            "disabled": (selectedRows >= 1),
        });

        $("#btn_edit").attr({
            "disabled": !(selectedRows === 1),
            "data-documenttype": documentTypeName,
            "data-verification-type": verificationType,
            "data-verificationdocument-id": verificationDocumentId,
            "data-id": id,
            "data-code": documentTypeCode,
            "data-fileType": fileType,
            "data-parentId": fileParentId
        });

        $("#btn_delete").attr({
            "disabled": !(selectedRows >= 1),
        });
    });

    $('#txt_document_search').on('input', function () {
        tbl_Document.search(this.value).draw();
    });
    $("#btn_document_refresh").on("click", function () {
        /* $("#btn_document_refresh").attr({ disabled: true });*/
        tbl_Document.ajax.reload(null, false);
    });
    $("#select-all-document").on("change", function () {
        if ($(this).prop("checked")) {
            tbl_Document.rows({ search: 'applied' }).select();
        } else {
            tbl_Document.rows({ search: 'applied' }).deselect();
        }
    });

    tbl_verificationDocument = $("#tbl_verification_document").DataTable({
        ajax: {
            url: baseUrl + "Document/GetAllEligibilityDocumentSetup",

            dataSrc: ''
        },
        language: {
            processing: "<div class='text-center'><span class='spinner-border spinner-border-sm'></span> Loading...</div>",
            previous: "<i class='mdi mdi-chevron-left'>",
            next: "<i class='mdi mdi-chevron-right'>"
        },
        columns: [
            {
                data: "Id"
            },
            {
                data: "DocumentTypeDescription",
                class: "text-left align-middle",
                render: function (data, type, row) {
                    return data;
                }
            },
            {
                data: "DocumentTypeParentDescription",
                class: "text-left align-middle",
                render: function (data, type, row) {
                    return data;
                }
            },

            {
                data: "CreatedBy",
                class: "text-left align-middle"
            },
            {
                data: "DateCreated",
                class: "text-center align-middle",
                render: function (data, type, row) {
                    return convertDate(data, 'MM/DD/YYYY');
                }
            },
            {
                data: "ModifiedBy",
                class: "text-left align-middle"
            },
            {
                data: "DateModified",
                class: "text-center align-middle",
                render: function (data, type, row) {
                    return convertDate(data, 'MM/DD/YYYY');
                }
            },
        ],
        drawCallback: function () {
            //let api = this.api();
            //var info = api.page.info();
            //if (info.pages != 0 && (api.page() > 0 && api.rows({ page: 'current' }).count() === 0)) {
            //    api.page('first').state.save();
            //    //window.location.reload();
            //    $("#btnRefresh").attr({ disabled: true });
            //    tbl_transaction.ajax.reload();
            //}

            //var rowCount = api.rows().data().toArray().length

            $(".dataTables_paginate > .pagination").addClass("pagination-rounded");
            $("#btnRefresh").attr({ disabled: false });
        },
        initComplete: function () {
            //resourceCounter();
        },
        columnDefs: [
            {
                targets: 0,
                orderable: false,
                className: "select-checkbox",
                visible: false,
                data: null,
                render: function () {
                    return "";
                }
            }
        ],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        select: true,
        order: [[2, 'desc']],
        pageLength: 20,
        rowId: "Id",
        processing: true,
        scrollX: true,
        scrollY: "25vh",
        scrollCollapse: true
    });
    $("#tbl_verification_document_filter, #tbl_verification_document_length").hide();
    tbl_verificationDocument.on('select deselect draw', function () {
        var all = tbl_verificationDocument.rows({ search: 'applied' }).count();
        var selectedRows = tbl_verificationDocument.rows({ selected: true, search: 'applied' }).count();
        var id = tbl_verificationDocument.rows({ selected: true }).data().pluck("Id").toArray().toString();
        var transactionNo = tbl_verificationDocument.rows({ selected: true }).data().pluck("TransactionNo").toArray().toString();
        var status = tbl_verificationDocument.rows({ selected: true }).data().pluck("AppStatus2").toArray().toString();

        // Tick select-all based on row count;
        $("#select-all-verification").prop("checked", (all == selectedRows && all > 0));

        //var statusToExlcude = ["0", "1", "4"];
        //$("#btnEdit").attr({
        //    "disabled": !(selectedRows === 1 && (statusToExlcude.includes(status))),
        //    "data-url": baseUrl + "Budget/Edit/" + transactionNo
        //});

        //$("#btnDetail").attr({
        //    "disabled": !(selectedRows === 1),
        //    "data-url": baseUrl + "Budget/Details/" + transactionNo
        //});
    });

    $('#txt_verification_search').on('input', function () {
        tbl_verificationDocument.search(this.value).draw();
    });
    $("#btn_verification_refresh").on("click", function () {
        // $("#btn_verification_refresh").attr({ disabled: true });
        tbl_verificationDocument.ajax.reload(null, false);
    });
    $("#select-all-verification").on("change", function () {
        if ($(this).prop("checked")) {
            tbl_verificationDocument.rows({ search: 'applied' }).select();
        } else {
            tbl_verificationDocument.rows({ search: 'applied' }).deselect();
        }
    });

    tbl_applicationDocument = $("#tbl_application_document").DataTable({
        ajax: {
            url: baseUrl + "Document/GetAllApplicationDocumentSetup",

            dataSrc: ''
        },
        language: {
            processing: "<div class='text-center'><span class='spinner-border spinner-border-sm'></span> Loading...</div>",
            previous: "<i class='mdi mdi-chevron-left'>",
            next: "<i class='mdi mdi-chevron-right'>"
        },
        columns: [
            {
                data: "Id"
            },
            {
                data: "DocumentTypeDescription",
                class: "text-left align-middle",
                render: function (data, type, row) {
                    return data;
                }
            },

            {
                data: "CreatedBy",
                class: "text-left align-middle"
            },
            {
                data: "DateCreated",
                class: "text-center align-middle",
                render: function (data, type, row) {
                    return convertDate(data, 'MM/DD/YYYY');
                }
            },
            {
                data: "ModifiedBy",
                class: "text-left  align-middle"
            },
            {
                data: "DateModified",
                class: "text-center align-middle",
                render: function (data, type, row) {
                    return convertDate(data, 'MM/DD/YYYY');
                }
            },
        ],
        drawCallback: function () {
            //let api = this.api();
            //var info = api.page.info();
            //if (info.pages != 0 && (api.page() > 0 && api.rows({ page: 'current' }).count() === 0)) {
            //    api.page('first').state.save();
            //    //window.location.reload();
            //    $("#btnRefresh").attr({ disabled: true });
            //    tbl_transaction.ajax.reload();
            //}

            //var rowCount = api.rows().data().toArray().length

            $(".dataTables_paginate > .pagination").addClass("pagination-rounded");
            $("#btnRefresh").attr({ disabled: false });
        },
        initComplete: function () {
            //resourceCounter();
        },
        columnDefs: [
            {
                targets: 0,
                orderable: false,
                className: "select-checkbox",
                visible: false,
                data: null,
                render: function () {
                    return "";
                }
            }
        ],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        select: true,
        order: [[2, 'desc']],
        pageLength: 20,
        rowId: "Id",
        processing: true,
        scrollX: true,
        scrollY: "25vh",
        scrollCollapse: true
    });
    $("#tbl_application_document_filter, #tbl_application_document_length").hide();
    tbl_applicationDocument.on('select deselect draw', function () {
        var all = tbl_applicationDocument.rows({ search: 'applied' }).count();
        var selectedRows = tbl_applicationDocument.rows({ selected: true, search: 'applied' }).count();
        var id = tbl_applicationDocument.rows({ selected: true }).data().pluck("Id").toArray().toString();
        var transactionNo = tbl_applicationDocument.rows({ selected: true }).data().pluck("TransactionNo").toArray().toString();
        var status = tbl_applicationDocument.rows({ selected: true }).data().pluck("AppStatus2").toArray().toString();

        // Tick select-all based on row count;
        $("#select-all-application").prop("checked", (all == selectedRows && all > 0));

        //var statusToExlcude = ["0", "1", "4"];
        //$("#btnEdit").attr({
        //    "disabled": !(selectedRows === 1 && (statusToExlcude.includes(status))),
        //    "data-url": baseUrl + "Budget/Edit/" + transactionNo
        //});

        //$("#btnDetail").attr({
        //    "disabled": !(selectedRows === 1),
        //    "data-url": baseUrl + "Budget/Details/" + transactionNo
        //});
    });

    $('#txt_application_search').on('input', function () {
        tbl_applicationDocument.search(this.value).draw();
    });
    $("#btn_application_refresh").on("click", function () {
        // $("#btn_application_refresh").attr({ disabled: true });
        tbl_applicationDocument.ajax.reload(null, false);
    });
    $("#select-all-application").on("change", function () {
        if ($(this).prop("checked")) {
            tbl_applicationDocument.rows({ search: 'applied' }).select();
        } else {
            tbl_applicationDocument.rows({ search: 'applied' }).deselect();
        }
    });

    function rebindValidators() {
        let $form = $("#document_form");
        let button = $("#btn_save");

        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate($form.data("unobtrusiveValidation").options);
        $form.data("validator").settings.ignore = "";

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
                },
                success: function (response) {
                    let recordId = $("input[name='DocumentType.Id']").val();
                    console.log(recordId);
                    let type = (recordId == 0 ? "Added!" : "Updated!");
                    let successMessage = `Document Successfully ${type}`;

                    messageBox(successMessage, "success", true);

                    button.attr({ disabled: false });
                    button.html("<span class='mdi mdi-content-save-outline'></span> Save");
                    tbl_Document.ajax.reload();
                    tbl_verificationDocument.ajax.reload();
                    tbl_applicationDocument.ajax.reload();

                    $("#modal-document").modal('hide');

                    tbl_Document.rows().deselect();
                    resetForm();
                },
                error: function (response) {
                    messageBox(response.responseText, "danger");
                    //  $("#beneficiary-overlay").addClass('d-none');
                    button.html("<span class='mdi mdi-content-save-outline'></span> Save");
                    button.attr({ disabled: false });
                }
            });
        });
    }

    $("#btn_add").on('click', function () {
        $("#modal-document").modal('show');
        resetForm();
    });

    $("#btn_edit").on('click', function () {
        $("#modal-document").modal('show');

        let id = $(this).attr('data-id');

        let documentType = $(this).attr('data-documenttype');
        let verificationType = $(this).attr('data-verification-type');
        let verificationdocumentId = $(this).attr('data-verificationdocument-id');
        let documentTypecode = $(this).attr('data-code');
        let documentFileType = $(this).attr('data-fileType');
        let documentParentId = $(this).attr('data-parentId');

        parentDocumentdropDown.setValue(documentParentId || '');

        if (verificationdocumentId == "") {
            verificationdocumentId = 0;
        }

        if (verificationType == "Eligibility Attachment") {
            verificationType = 1;
        }

        else if (verificationType == "Application Attachment") {
            verificationType = 2;
        }

        $("#DocumentType_Id").val(id);
        $("#DocumentVerification_Id").val(verificationdocumentId);
        $("#DocumentType_Description").val(documentType);
        $("#DocumentType_Code").val(documentTypecode);

        $("#DocumentVerification_Type").data('selectize').setValue(verificationType);
        $("#DocumentType_FileType").data('selectize').setValue(documentFileType);
    });

    $("#btn_delete").on("click", function (e) {
        var documentIds = tbl_Document.rows({ selected: true }).data().pluck("Id").toArray().toString();

        Swal.fire({
            title: 'Are you sure?',
            text: `These Record/s Will Be Deleted Permanently`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showLoaderOnConfirm: true,
            preConfirm: (login) => {
                return fetch(`${baseUrl}Document/DeleteDocumentType/`,
                    {
                        method: "DELETE",
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        body: `ids=${documentIds}`
                    })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error(response.statusText)
                        }
                        return response;
                    })
                    .catch(error => {
                        Swal.showValidationMessage(
                            `Request failed: ${error}`
                        )
                    })
            },
            allowOutsideClick: () => !Swal.isLoading()
        }).then((result) => {
            if (result.isConfirmed) {
                messageBox("Record(s) successfully deleted.", "success");
                tbl_Document.ajax.reload(null, false)
                tbl_verificationDocument.ajax.reload(null, false);
                tbl_applicationDocument.ajax.reload(null, false);
            }
        })
    });

    function resetForm() {
        $('[name="DocumentType.Id"]').val(0);
        $('[name="DocumentVerification.Id"]').val(0);
        $('[name="DocumentType.Description"]').val("");
        $('[name="DocumentType.Code"]').val("");
        $('[name="DocumentVerification.Type"]').data('selectize').setValue("");
        $('[name="DocumentType.FileType"]').data('selectize').setValue("");
    }
});