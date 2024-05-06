const $tbl_propProj = $("#tbl_PropProj");
const $tbl_location = $("#tbl-location-add");
const $modal_PropProj = $('#modal-PropProjModel');
const $modal_PropLoc = $('#modal-location');
const $form = $("#PropProjModel_form");

$(async function () {
    let $companyIdDropdown, companyIdDropdown;

    $companyIdDropdown = $(`[name='PropProjModel.CompanyId']`).selectize({
        valueField: 'Id',
        labelField: 'Name',
        searchField: 'Name',
        preload: true,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + 'Company/GetCompanies',
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
                    escape(item.Name) +
                    "</div>"
                );
            },
            option: function (item, escape) {
                return ("<div class='py-1 px-2'>" +
                    escape(item.Name) +
                    "</div>"
                );
            }
        },
    });

    companyIdDropdown = $companyIdDropdown[0].selectize;

    if ($tbl_propProj) {
        var tbl_propProj = $("#tbl_PropProj").DataTable({
            ajax: {
                url: '/PropertyProject/GetAllPropertyProject',
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

    tbl_propProj.on('select deselect draw', function () {
        var all = tbl_propProj.rows({ search: 'applied' }).count();
        var selectedRows = tbl_propProj.rows({ selected: true, search: 'applied' }).count();
        var id = tbl_propProj.rows({ selected: true }).data().pluck("Id").toArray().toString();

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

        btn_location_PropProjModel.attr({
            "disabled": !(selectedRows >= 1),
        });
    });

    if ($tbl_location) {
        var location_add = $("#tbl-location-add").DataTable({
            ajax: {
                url: '/PropertyLocation/GetAllPropertyLocation',
                method: 'GET',
                dataSrc: ""
            },
            columns: [
                {
                    data: 'Id',
                },
                {
                    data: 'Name',
                }
            ],
            columnDefs: [
                {
                    targets: [0],
                    orderable: false,
                    className: "select-checkbox",
                    render: function () {
                        return "";
                    }
                }

            ],
            select: {
                style: 'multi',
                selector: 'tr'
            },
            drawCallback: function () {
                $(".dataTables_paginate > .pagination").addClass("pagination-rounded");
            },
            rowId: "Id",
            processing: true,
            scrollX: true,
            scrollY: '35vh',
            scrollCollapse: true,
            searchHighlight: true,
            info: false,
            paging: false,
            order: [[1, "asc"]]
        });
    }

    $.fn.dataTable.ext.search.push(
        function (settings, searchData, index, rowData, counter) {
            var tableId = settings.nTable.id;

            if (tableId === "location_add") {
                var checked = $('input[id="filterUser"]').is(':checked');

                if (checked) {
                    var api = new $.fn.dataTable.Api(settings);
                    var node = api.row(index).node();

                    if ($(node).hasClass('selected')) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }

            return true;
        }
    );

    $("#tbl-location-add_filter").hide();
    var btn_location_PropProjModel = $('#btn_location_PropProjModel').on('click', async function (e) {
        e.preventDefault();
        applyLocation();
    });

    var btn_add_PropProjModel = $('#btn_add_PropProjModel').on('click', async function (e) {
        e.preventDefault();
        await applyPropProj();
    });
    var btn_edit_PropProjModel = $('#btn_edit_PropProjModel').on('click', async function (e) {
        e.preventDefault();
        var id = tbl_propProj.rows('.selected').data().pluck('Id').toArray()[0];
        await applyPropProj(id);
    });
    var btn_delete_PropProjModel = $('#btn_delete_PropProjModel').on('click', function (e) {
        e.preventDefault();
        var ids = tbl_propProj.rows('.selected').data().pluck('Id').toArray();
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
                return fetch(`/PropertyProject/DeletePropertyProject/`,
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
                messageBox("Project(s) successfully deleted.", "success");
                tbl_propProj.ajax.reload(null, false)
            }
        })
    })

    async function applyPropProj(id) {
        clearForm($form);

        console.log(id);

        let propProject = await getPropProject(id);

        $('[name="PropProjModel.Id"]').val(propProject ? propProject.Id : 0);
        $('[name="PropProjModel.Name"]').val(propProject ? propProject.Name : "");
        $('[name="PropProjModel.Description"]').val(propProject ? propProject.Description : "");
        $('[name="PropProjModel.CompanyId"]').data('selectize').setValue(propProject ? propProject.CompanyId : "");

        $modal_PropProj.modal('show')
    }

    async function applyLocation() {
        $modal_PropLoc.modal('show')
    }

    async function getPropProject(id) {
        const response = $.ajax({
            url: `/PropertyProject/GetPropertyProjectById`,
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

        var propUnitId = $('[name="PropProjModel.Id"]').val();

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
                successMessage = `Project Successfully ${propUnitId == 0 ? 'Added' : 'Updated'}!`;
                messageBox(successMessage, "success", true);
                button.html('Save').attr('disabled', false);
                tbl_propProj.ajax.reload();
                $modal_PropProj.modal('hide');
                clearForm($form);
            },
            error: async function (jqXHR, textStatus, errorThrown) {
                button.html('Save').attr('disabled', false);
                messageBox(jqXHR.responseText, "danger", true);
            }
        });
    })
});