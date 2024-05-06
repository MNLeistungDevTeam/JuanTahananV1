const $modal = $('#modal-PropLoc');
const $form = $("#propLocation_form");

$(async function () {
    var tbl_propLoc = $("#tbl_propLoc").DataTable({
        ajax: {
            url: '/PropertyLocation/GetAllPropertyLocation',
            method: 'GET',
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
                data: 'Name',
                orderable: !0,
                className: 'text-center'
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
        order: [[2, "asc"]],
        pageLength: 10,
        searchHighlight: true,
        stateSave: false,
        bLengthChange: false,
        dom: 'lrtip',
        processing: true
    });

    tbl_propLoc.on('select deselect draw', function () {
        var all = tbl_propLoc.rows({ search: 'applied' }).count();
        var selectedRows = tbl_propLoc.rows({ selected: true, search: 'applied' }).count();
        var id = tbl_propLoc.rows({ selected: true }).data().pluck("Id").toArray().toString();

        // Tick select-all based on row count;
        $("#select-all-document").prop("checked", (all == selectedRows && all > 0));

        btn_add_PropProjModel.attr({
            "disabled": (selectedRows >= 1),
        });

        btn_edit_PropProjModel.attr({
            "disabled": !(selectedRows === 1),
        });

        btn_delete_PropProjModel.attr({
            "disabled": !(selectedRows >= 1),
        });
    });

    var btn_add_PropProjModel = $('#btn_add_PropLocModel').on('click', async function (e) {
        e.preventDefault();
        await applyPropProj();
    });
    var btn_edit_PropProjModel = $('#btn_edit_PropLocModel').on('click', async function (e) {
        e.preventDefault();
        var id = tbl_propLoc.rows('.selected').data().pluck('Id').toArray()[0];
        await applyPropProj(id);
    });
    var btn_delete_PropProjModel = $('#btn_delete_PropLocModel').on('click', function (e) {
        e.preventDefault();
        var ids = tbl_propLoc.rows('.selected').data().pluck('Id').toArray();
        Swal.fire({
            title: 'Are you sure?',
            text: `The following Role/s will be deleted: ${ids.length}`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showLoaderOnConfirm: true,
            preConfirm: (login) => {
                return fetch(`/PropertyLocation/DeletePropertyLocation/`,
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
                messageBox("Location(s) successfully deleted.", "success");
                tbl_propLoc.ajax.reload(null, false)
            }
        })
    })

    async function applyPropProj(id) {
        clearForm($form);

        let propLocation = await getPropLocation(id);

        $('[name="PropLocModel.Id"]').val(propLocation ? propLocation.Id : 0);
        $('[name="PropLocModel.Name"]').val(propLocation ? propLocation.Name : "");

        $modal.modal('show')
    }

    async function getPropLocation(id) {
        const response = $.ajax({
            url: `/PropertyLocation/GetPropertyLocationById`,
            method: 'GET',
            data: {
                id: id
            }
        });

        return response;
    }

    $form.on('submit', async function (e) {
        e.preventDefault();
        var button = $(this).find('button[type="submit"]');
        var formData = new FormData(e.target);

        if (!$(this).valid())
            return;

        // Log serialized form data
        console.log("Form Data:", formData);

        var propUnitId = $('[name="PropLocModel.Id"]').val();

        $.ajax({
            url: $(this).attr('action'),
            method: $(this).attr('method'),
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function () {
                button.html(`<i class="mdi mdi-spin mdi-loading"></i> Saving..`).attr('disabled', true);
            },
            success: function (response) {
                let successMessage = "";
                successMessage = `Location Successfully ${propUnitId == 0 ? 'Added' : 'Updated'}!`;
                messageBox(successMessage, "success", true);
                button.html('Save').attr('disabled', false);
                tbl_propLoc.ajax.reload();
                $modal.modal('hide');
                clearForm($form);
            },
            error: async function (jqXHR, textStatus, errorThrown) {
                button.html('Save').attr('disabled', false);
                messageBox(jqXHR.responseText, "danger", true);
            }
        });
    })
});