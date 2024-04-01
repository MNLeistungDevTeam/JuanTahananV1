$(async function () {
    "strict";
    const FileFormats = {
        1: ['.pdf'],
        2: '.docx',
        3: '.txt',
        4: '.xlsx',
        5: ['.jpg', '.jpeg','.JPEG','.Jpg'], // Mapping both .jpg and .jpeg to the same value
        6: '.png'
    };

    const $modal = $('#modal-file');
    const $form = $("#document_form");

    var applicationinfoCode = $("#txt_applicationCode").val() ? $("#txt_applicationCode").val() : null;

    var documentypeid = 0;
    var DocumentId = 0;

    var tbl_files = $("#tbl_files").DataTable({
        ajax: {
            url: `/Document/GetApplicantUploadedDocumentByDocumentType/`,
            method: 'GET',
            dataSrc: "",
            data: function (d) {
                d.documentTypeId = documentypeid,
                    d.applicantCode = applicationinfoCode
            }
        },
        columns: [
            {
                data: 'Name',
                orderable: true,
                className: 'align-middle text-center'
            },
            {
                data: 'Size',
                orderable: true,
                className: 'align-middle text-center',
            },
            {
                data: 'DateCreated',
                orderable: !0,
                className: 'align-middle text-center',
                render: function (data) {
                    if (data && data.trim() !== "") {
                        return moment(data).format('YYYY-MM-DD');
                    } else {
                        return "";
                    }
                }
            },
            {
                data: null,
                orderable: true,
                className: 'text-center',
                render: function () {
                    return `<button class="btn btn-primary btn-sm waves-effect ms-1 replace">Replace</button> <button class="btn btn-danger btn-sm waves-effect ms-1 delete">Delete</button>`;
                }
            }
        ],
        drawCallback: function () {
            $(".dataTables_paginate > .pagination").addClass("pagination-rounded"),
                $('li.paginate_button.page-item.active > a').addClass('waves-effect')
        },
        language: {
            "zeroRecords": "No Records Found....",
            loadingRecords: "Records loading...",
            emptyTable: `No Records Found....`,
            infoEmpty: "No entries to show",
            paginate: { previous: "<i class='mdi mdi-chevron-left'>", next: "<i class='mdi mdi-chevron-right'>" },
            info: `Showing Records _START_ to _END_ of _TOTAL_`
        },
        scrollY: '24rem',
        scrollX: true,
        order: [[1, "asc"]],
        pageLength: 10,
        searchHighlight: true,
        stateSave: false,
        processing: true
    });

    $('.upload').on('click', async function (e) {
        e.preventDefault();
        DocumentId = 0;
        var documentType = await GetDocumentType(documentypeid);
        alert(documentType.FileType)
        let fileFormats = FileFormats[documentType.FileType];

        if (Array.isArray(fileFormats)) {
            fileFormats = fileFormats.join(','); // Join array elements into a single string
        }

        $('#file-input').attr('accept', fileFormats);
        $('#file-input').trigger('click');
    });
    $('#tbl_files tbody').on('click', '.replace', function (e) {
        e.preventDefault();
        var rowData = tbl_files.row($(this).closest('tr')).data();
        DocumentId = rowData?.Id || 0;
        $('#file-input').trigger('click');
    });

    //$(document).on('click', '.delete', function (e) {
    //    e.preventDefault();
    //    var rowData = tbl_files.row(this).data();
    //    DocumentId = rowData?.Id || 0;
    //    DeleteFile(DocumentId);
    //});
    $('#tbl_files tbody').on('click', '.delete', function (e) {
        e.preventDefault();
        var rowData = tbl_files.row($(this).closest('tr')).data();
        Swal.fire({
            title: 'Are you sure?',
            text: `The following Document will be deleted`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showLoaderOnConfirm: true,
            preConfirm: (login) => {
                return fetch(`/document/DocumentDelete/`,
                    {
                        method: "DELETE",
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        body: `DocumentId=${rowData?.Id || 0}`
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
                messageBox("Document successfully deleted.", "success");
                tbl_applicationDocument.ajax.reload();
                tbl_verificationDocument.ajax.reload();
                tbl_files.ajax.reload();
            }
        })
    });

    $('#file-input').on('change', function () {
        var file = this.files[0];
        if (file) {
            upload(file, documentypeid, DocumentId);
        }
    });
    function upload(file, DocumentTypeId, DocumentId) {
        var formData = new FormData();
        formData.append('file', file);
        formData.append('ApplicationId', $('#applicationId').val());
        formData.append('DocumentTypeId', DocumentTypeId);
        formData.append('DocumentId', DocumentId);

        $.ajax({
            url: '/Document/DocumentUploadFile',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () {
                loading('Uploading...', true);
            },
            success: function (response) {
                messageBox('Uploaded Successfully', "success", true);

                loader.close();
                tbl_files.ajax.reload();
                tbl_verificationDocument.ajax.reload();
                tbl_applicationDocument.ajax.reload();

                //method removing iformfile
                var myfile = $('#file-input')[0];

                myfile.files[0];

                // remove filename
                $('#file-input').val('');
            },
            error: function (xhr, status, error) {
                messageBox(xhr.responseText, "danger", true);
                loader.close();
                tbl_document.ajax.reload();
                tbl_files.ajax.reload();
            }
        });
    }

    var tbl_applicationDocument = $("#tbl_application_document").DataTable({
        ajax: {
            url: baseUrl + "Document/GetApplicantApplicationDocument",
            data: function (d) {
                d.applicantCode = applicationinfoCode
            },

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
                class: "text-center",
                render: function (data, type, row) {
                    return data;
                }
            },
            {
                data: "DocumentTypeId",
                visible: false
            },

            {
                data: "TotalDocumentCount",
                class: "text-center align-middle"
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

    $('#tbl_application_document tbody').on('click', 'tr', function () {
        var rowData = tbl_verificationDocument.row(this).data();
        documentypeid = rowData.DocumentTypeId; // Assign the value of documentTypeId here
        tbl_files.ajax.reload();
        $modal.modal('show');
    });

    $("#tbl_application_document_filter, #tbl_application_document_length").hide();

    $('#txt_application_search').on('input', function () {
        tbl_applicationDocument.search(this.value).draw();
    });
    $("#btn_application_refresh").on("click", function () {
        // $("#btn_application_refresh").attr({ disabled: true });
        tbl_applicationDocument.ajax.reload(null, false);
    });

    var tbl_verificationDocument = $("#tbl_verification_document").DataTable({
        ajax: {
            url: baseUrl + "Document/GetEligibilityApplicationDocument",
            data: function (d) {
                d.applicantCode = applicationinfoCode
            },

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
                class: "text-center",
                render: function (data, type, row) {
                    return data;
                }
            },

            {
                data: "DocumentTypeId",
                visible: false
            },

            {
                data: "TotalDocumentCount",
                class: "text-center align-middle"
            },

        ],
        drawCallback: function () {
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

    $('#tbl_verification_document tbody').on('click', 'tr', function () {
        var rowData = tbl_verificationDocument.row(this).data();
        documentypeid = rowData.DocumentTypeId; // Assign the value of documentTypeId here
        console.log(rowData);
        console.log(documentypeid);
        tbl_files.ajax.reload();
        $modal.modal('show');
    });

    $("#tbl_verification_document_filter, #tbl_verification_document_length").hide();

    $('#txt_verification_search').on('input', function () {
        tbl_verificationDocument.search(this.value).draw();
    });
    $("#btn_verification_refresh").on("click", function () {
        // $("#btn_verification_refresh").attr({ disabled: true });
        tbl_verificationDocument.ajax.reload(null, false);
    });

    async function GetDocumentType(documentTypeId) {
        const response = $.ajax({
            url: baseUrl + "Document/GetDocumentTypeById/" + documentTypeId,
            method: "get",
            data: 'json'
        });

        return response;
    }
});