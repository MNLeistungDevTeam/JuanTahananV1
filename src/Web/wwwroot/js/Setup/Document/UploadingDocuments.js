$(function () {
    "strict";
    const $tbl_document = document.querySelector('#tbl_document');
    const $tbl_files = document.querySelector('#tbl_files');
    const $modal = $('#modal-file');
    const $form = $("#document_form");

    var applicationinfoCode = $("#txt_applicationCode").val() ? $("#txt_applicationCode").val() : null;

    var documentypeid = 0;
    var DocumentId = 0;

    var tbl_files; // Declare tbl_files outside the scope

    if ($tbl_document) {
        var tbl_document = $("#tbl_document").DataTable({
            ajax: {
                url: '/Document/GetDocumentsByApplicant/' + applicationinfoCode,
                method: 'GET',
                dataSrc: "",
            },
            columns: [
                {
                    data: 'Description',
                    orderable: true,
                    className: 'w-100',
                },
                {
                    data: 'TotalDocumentCount',
                    orderable: true,
                    className: 'text-center',
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
            order: [[1, "desc"]],
            pageLength: 10,
            searchHighlight: true,
            stateSave: false,
            bLengthChange: false,
            dom: 'lrtip',
            processing: true
        });

        $('#tbl_document tbody').on('click', 'tr', function () {
            var rowData = tbl_document.row(this).data();
            documentypeid = rowData.Id;
            tbl_files.ajax.reload();
            $modal.modal('show');
        });
    }

    if ($tbl_files) {
        tbl_files = $("#tbl_files").DataTable({
            ajax: {
                url: '/Document/GetApplicantUploadedDocuments/' + applicationinfoCode,
                method: 'GET',
                dataSrc: "",
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
    }

    $('.upload').on('click', function (e) {
        e.preventDefault();
        DocumentId = 0;

        $('#file-input').trigger('click');
    });

    $('#tbl_files tbody').on('click', '.replace', function (e) {
        e.preventDefault();
        var rowData = tbl_files.row($(this).closest('tr')).data();
        DocumentId = rowData?.Id || 0;
        $('#file-input').trigger('click');
    });

    $(document).on('click', '.delete', function (e) {
        e.preventDefault();
        var rowData = tbl_document.row(this).data();
        DocumentId = rowData?.Id || 0;
        DeleteFile(DocumentId);
    });
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
                tbl_document.ajax.reload();
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
                tbl_document.ajax.reload();
                tbl_files.ajax.reload();
            },
            error: function (xhr, status, error) {
                messageBox(xhr.responseText, "danger", true);
                loader.close();
                tbl_document.ajax.reload();
                tbl_files.ajax.reload();
            }
        });
    }
});