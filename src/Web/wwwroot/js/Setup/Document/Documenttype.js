$(() => {
    "strict";
    const $tbl_document = document.querySelector('#tbl_document');
    const $modal = $('#modal-document');
    const $form = $("#document_form");

    if ($tbl_document) {
        var tbl_document = $("#tbl_document").DataTable({
            ajax: {
                url: '/document/GetAllDocumentTypes',
                method: 'GET',
                dataSrc: ""
            },
            columns: [
                {
                    data: 'Id',
                    orderable: !1,
                    class: 'align-middle text-center ps-2 pe-2',
                    render: function (e, l, a, o) {
                        return (e = "display" === l ? '<input type="checkbox" class="form-check-input dt-checkboxes">' : e);
                    },
                    checkboxes: { selectRow: !0, selectAllRender: '<input type="checkbox" class="form-check-input dt-checkboxes">' },
                },
                {
                    data: 'Description',
                    orderable: true,
                    className: 'w-100'
                },
                {
                    data: 'TotalDocumentCount',
                    orderable: !0,
                    className: 'align-middle text-center'
                },
                {
                    data: 'CreatedBy',
                    orderable: !0,
                    className: 'text-center',
                },
                {
                    data: 'DateCreated',
                    orderable: !0,
                    className: 'text-center',
                    render: function (data) {
                        if (data && data.trim() !== "") {
                            return moment(data).format('YYYY-MM-DD');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: 'ModifiedBy',
                    orderable: !0,
                    className: 'text-center',
                },
                {
                    data: 'DateModified',
                    orderable: !0,
                    className: 'text-center',
                    render: function (data) {
                        if (data && data.trim() !== "") {
                            return moment(data).format('YYYY-MM-DD');
                        } else {
                            return "";
                        }
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
            select: {
                style: "os"

            },
            scrollY: '24rem',
            scrollX: true,
            order: [[4, "asc"]],
            pageLength: 10,
            searchHighlight: true,
            stateSave: false,
            bLengthChange: false,
            dom: 'lrtip',
            processing: true
        });

        tbl_document.on('select', () => {
            CheckRows(tbl_document);
        })
        tbl_document.on('deselect', () => {
            CheckRows(tbl_document);
        })
        tbl_document.on('draw', () => {
            CheckRows(tbl_document);
        })
        function CheckRows(element) {
            var row2minimum = element.rows({ selected: true }).count();
            var currentElement = $(`#${$(element.table().node()).attr('id')}`);
            var id = tbl_document.rows('.selected').data().pluck('TotalDocumentCount').toArray();
            var sum = id.reduce(function (total, currentValue) {
                return total + currentValue;
            }, 0);

            while (true) {
                if (currentElement.hasClass('card-body') || currentElement.hasClass('tab-pane') || currentElement.hasClass('modal-content')) {
                    let btn_delete = currentElement.find('.btn_delete');
                    let btn_edit = currentElement.find('.btn_edit');
                    if (row2minimum > 1 && sum == 0) {
                        btn_delete.attr('disabled', false);
                        btn_edit.attr('disabled', true);
                    } else if (row2minimum === 1 && sum == 0) {
                        btn_delete.attr('disabled', false);
                        btn_edit.attr('disabled', false);
                    } else {
                        btn_delete.attr('disabled', true);
                        btn_edit.attr('disabled', true);
                    }
                    break;
                }
                currentElement = currentElement.parent();
            }

        }
    }
    var btn_delete_document = $('#btn_delete_document').on('click', function (e) {
        e.preventDefault();
        var ids = tbl_document.rows('.selected').data().pluck('Id').toArray();
        Swal.fire({
            title: 'Are you sure?',
            text: `The following Document type/s will be deleted: ${ids.length}`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showLoaderOnConfirm: true,
            preConfirm: (login) => {
                return fetch(`/Document/DeleteDocuments/`,
                    {
                        method: "delete",
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        body: `ids=${ids}`
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
                messageBox("Document type(s) successfully deleted.", "success");
                tbl_document.ajax.reload(null, false)
            }
        })
    })
    var btn_edit_document = $('#btn_edit_document').on('click', function (e) {
        e.preventDefault();
        var id = tbl_document.rows('.selected').data().pluck('Id').toArray()[0];
        applyDocument(id);
    });
    var btn_add_document = $('#btn_add_document').on('click', function (e) {
        e.preventDefault();
        applyDocument();
    });
    $form.on('submit', function (e) {
        e.preventDefault();
        var button = $(this).find('button[type="submit"]');
        if (!$(this).valid())
            return;
        $.ajax({
            url: $(this).attr('action'),
            method: $(this).attr('method'),
            data: $(this).serialize(),
            beforeSend: function () {
                button.html(`<i class="mdi mdi-spin mdi-loading"></i> Saving..`).attr('disabled', true);
            },
            success: function (data) {
                button.html('Save').attr('disabled', false);
                tbl_document.ajax.reload();
                $modal.modal('hide');
                messageBox(`Document type Successfully ${data}`, "success", true);
            },
            error: async function (jqXHR, textStatus, errorThrown) {
                button.html('Save').attr('disabled', false);
                messageBox(jqXHR.responseText, "danger", true);
            }
        });
    })
    function applyDocument(id = 0) {
        clearForm($form);
        $('[name="Document.Id"]').val(0);
        if (id != 0) {
            fetchDocument(id, function (callback) {
                $('[name="Document.Id"]').val(callback.Id);
                $('[name="Document.Description"]').val(callback.Description);
            })
        }
        $modal.modal('show');
    }
    function fetchDocument(id, callback) {
        $.ajax({
            url: `/Document/GetDocumentById`,
            method: 'GET',
            data: {
                id: id
            },
            success: callback
        })
    }
})