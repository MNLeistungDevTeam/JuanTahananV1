$(() => {
    "strict";
    const $tbl_portal = document.querySelector('#tbl_portal');
    var form = $('#portal_form');
    var modal = $('#portal_modal');
    if ($tbl_portal) {
        var tbl_portal = $("#tbl_portal").DataTable({
            ajax: {
                url: '/portal/getPortals',
                method: 'get',
                dataSrc: ""  
            },
            columns: [
                {
                    data: 'Id',
                    orderable: !1,
                    class: 'text-center ps-2 pe-2',
                    render: function (e, l, a, o) {
                        return (e = "display" === l ? '<input type="checkbox" class="form-check-input dt-checkboxes">' : e);
                    },
                    checkboxes: { selectRow: !0, selectAllRender: '<input type="checkbox" class="form-check-input dt-checkboxes ">' },
                },
                {
                    data: 'Description',
                    orderable: !0,
                    className: 'text-center',
                },
                {
                    data: 'ViewName',
                    orderable: !0,
                    className: 'text-center',
                    render: function (data) {
                        return `<span><span class="text-primary">${data}</span>.cshtml</span>`;
                    }
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
                emptyTable: "No Records Available",
                infoEmpty: "No entries to show",
                paginate: { previous: "<i class='mdi mdi-chevron-left'>", next: "<i class='mdi mdi-chevron-right'>" },
                info: `Showing Records _START_ to _END_ of _TOTAL_`
            },
            select: {
                style: "os"

            },
            scrollY: '24rem',
            scrollX: true,
            order: [[0, "asc"]],
            pageLength: 10,
            searchHighlight: true,
            stateSave: false,
            bLengthChange: false,
            dom: 'lrtip',
            processing: true
        });

        tbl_portal.on('select', () => {
            CheckRows(tbl_portal);
        })
        tbl_portal.on('deselect', () => {
            CheckRows(tbl_portal);
        })
    }
    $('.btn_add').on('click', function (e) {
        e.preventDefault();
        applyPortal();
    })
    $('.btn_edit').on('click', function (e) {
        e.preventDefault();
        var id = tbl_portal.rows('.selected').data().pluck('Id').toArray()[0];
        applyPortal(id);
    })
    $('.btn_delete').on('click', function (e) {
        e.preventDefault();
        var ids = tbl_portal.rows('.selected').data().pluck('Id').toArray();
        Swal.fire({
            title: 'Are you sure?',
            text: `The following Portal/s will be deleted: ${ids.length}`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showLoaderOnConfirm: true,
            preConfirm: (login) => {
                return fetch(`/portal/BatchDeletePortals/`,
                    {
                        method: "DELETE",
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
                messageBox("Portal(s) successfully deleted.", "success");
                tbl_portal.ajax.reload(null, false)
            }
        })
    })
    form.on('submit', function (e) {
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
                tbl_portal.ajax.reload();
                modal.modal('hide');
            },
            error: async function (jqXHR, textStatus, errorThrown) {

                button.html('Save').attr('disabled', false);
            }
        });
    })
    function applyPortal(id = 0) {
   
        clearForm(form)
        if (id != 0) {
            getPortal(id, function (data) {
                var items = convertToDictionaryArray(data);
                items.forEach(function (item) {
                    for (var key in item) {
                        var element = $(`[name="PortalModel.${key}"]`);
                        element.val(item[key]);
                    }
                })
            })
        }
        modal.modal('show');
    }

    function getPortal(id, callback) {
        $.ajax({
            url: '/portal/GetPortalById',
            method: 'POST',
            data: { id: id },
            traditional: true,
            success: callback
        });
    }
})