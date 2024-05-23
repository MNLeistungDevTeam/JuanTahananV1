$(async function () {
    const $tbl_propUnit = $("#tbl_PropUnit");
    const $modal = $('#modal-PropUnitModel');
    const $form = $("#PropUnitModel_form");



    $('#fileInputTrigger').on('click', function () {
        $('#PropUnitModel_ProfileImageFile').click();
    });

    $('#PropUnitModel_ProfileImageFile').on('change', function () {
        let container = $('#imagePreview');

        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                container.attr("src", e.target.result);
                container.hide();
                container.fadeIn(650);
            }
            reader.readAsDataURL(this.files[0]);
        }
    });


    if ($tbl_propUnit) {
        var tbl_propUnit = $("#tbl_PropUnit").DataTable({
            ajax: {
                url: '/PropertyUnit/GetAllPropertyUnit',
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
                    data: 'Description',
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
            order: [[4, "asc"]],
            pageLength: 10,
            searchHighlight: true,
            stateSave: false,
            bLengthChange: false,
            dom: 'lrtip',
            processing: true
        });
    }

    tbl_propUnit.on('select deselect draw', function () {
        var all = tbl_propUnit.rows({ search: 'applied' }).count();
        var selectedRows = tbl_propUnit.rows({ selected: true, search: 'applied' }).count();
        var id = tbl_propUnit.rows({ selected: true }).data().pluck("Id").toArray().toString();

        // Tick select-all based on row count;
        $("#select-all-document").prop("checked", (all == selectedRows && all > 0));

        btn_add_PropUnitModel.attr({
            "disabled": (selectedRows >= 1),
        });

        btn_edit_PropUnitModel.attr({
            "disabled": !(selectedRows === 1),
        });

        btn_delete_PropUnitModel.attr({
            "disabled": !(selectedRows >= 1),
        });
    });

        var btn_add_PropUnitModel = $('#btn_add_PropUnitModel').on('click', async function (e) {
            e.preventDefault();
            await applyPropUnit();
        });
        var btn_edit_PropUnitModel = $('#btn_edit_PropUnitModel').on('click', async function (e) {
            e.preventDefault();
            var id = tbl_propUnit.rows('.selected').data().pluck('Id').toArray()[0];
            await applyPropUnit(id);
        });
        var btn_delete_PropUnitModel = $('#btn_delete_PropUnitModel').on('click', function (e) {
            e.preventDefault();
            var ids = tbl_propUnit.rows('.selected').data().pluck('Id').toArray();
            var names = tbl_propUnit.rows('.selected').data().pluck('Name').toArray();
            Swal.fire({
                title: 'Are you sure?',
                text: `The following Role/s will be deleted: ${names}`,
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Confirm',
                showLoaderOnConfirm: true,
                preConfirm: (login) => {
                    return fetch(`/PropertyUnit/DeletePropertyUnit/`,
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
                    messageBox("Unit(s) successfully deleted.", "success");
                    tbl_propUnit.ajax.reload(null, false)
                }
            })
        })

    async function applyPropUnit(id) {
        clearForm($form);

        console.log(id);

        let propUnit = await getProperyUnit(id);

        $('[name="PropUnitModel.Id"]').val(propUnit ? propUnit.Id : 0);
        $('[name="PropUnitModel.Name"]').val(propUnit ? propUnit.Name : "");
        $('[name="PropUnitModel.Description"]').val(propUnit ? propUnit.Description : "");

        $modal.modal('show')
    }

    async function getProperyUnit(id) {
        const response = $.ajax({
            url: `/PropertyUnit/GetPropertyUnit`,
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

        var propUnitId = $('[name="PropUnitModel.Id"]').val();

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
                successMessage = `Unit Successfully ${propUnitId == 0 ? 'Added' : 'Updated'}!`;
                messageBox(successMessage, "success", true);
                button.html('Save').attr('disabled', false);
                tbl_propUnit.ajax.reload();
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