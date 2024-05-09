const $tbl_propProj = $("#tbl_PropProj");
const $tbl_location = $("#tbl-location-add");
const $tbl_unit = $("#tbl-unit-add");
const $modal_PropProj = $('#modal-PropProjModel');
const $modal_PropLoc = $('#modal-location');
const $modal_Unit = $('#modal-unit');
const $form = $("#PropProjModel_form");

var btn_delete_PropProjModel = $('#btn_delete_PropProjModel');
var btn_edit_PropProjModel = $('#btn_edit_PropProjModel');
var btn_add_PropProjModel = $('#btn_add_PropProjModel');
var btn_location_PropProjModel = $("#btn_location_PropProjModel");
var btn_projectrefresh = $('#btn_projectrefresh');
var btn_saveprojectlocation = $('#btn_saveprojectlocation');
var btn_unit_PropProjModel = $("#btn_unit_PropProjModel");
var btn_saveprojectUnit = $("#btn_saveprojectUnit");
$(async function () {
    let $companyDropdown, companyDropdown;

    $companyDropdown = $(`[name='PropProjModel.CompanyId']`).selectize({
        valueField: 'Id',
        labelField: 'Name',
        searchField: 'Name',
        preload: true,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + 'Company/GetDevelopers',
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

    companyDropdown = $companyDropdown[0].selectize;

    rebindValidator();

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
                    data: 'CompanyName',
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
    }

    tbl_propProj.on('select deselect draw', function () {
        var all = tbl_propProj.rows({ search: 'applied' }).count();
        var selectedRows = tbl_propProj.rows({ selected: true, search: 'applied' }).count();
        var id = tbl_propProj.rows({ selected: true }).data().pluck("Id").toArray().toString();

        $("#select-all-document").prop("checked", (all == selectedRows && all > 0));

        btn_add_PropProjModel.attr({
            "disabled": (selectedRows >= 1)
        });

        btn_edit_PropProjModel.attr({
            "disabled": !(selectedRows === 1),
            "data-id": id
        });

        btn_delete_PropProjModel.attr({
            "disabled": !(selectedRows >= 1),
            "data-id": id
        });

        btn_location_PropProjModel.attr({
            "disabled": !(selectedRows === 1),
            "data-id": id
        });

        btn_unit_PropProjModel.attr({
            "disabled": !(selectedRows === 1),
            "data-id": id
        });
    });

    //#region Project Table

    btn_add_PropProjModel.on('click', async function () {
        await openProjectModal();
    });
    btn_edit_PropProjModel.on('click', async function () {
        var id = tbl_propProj.rows('.selected').data().pluck('Id').toArray()[0];
        await openProjectModal(id);
    });

    btn_delete_PropProjModel.on('click', function () {
        var ids = tbl_propProj.rows('.selected').data().pluck('Id').toArray();
        // var name = tbl_propProj.rows('.selected').data().pluck('Name').toArray();
        var name = tbl_propProj.rows({ selected: true }).data().pluck("Name").toArray().toString();

        Swal.fire({
            title: 'Are you sure?',
            text: `The following Role/s will be deleted: ${name}`,
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

    btn_projectrefresh.on('click', function () {
        tbl_propProj.ajax.reload();
    });

    btn_location_PropProjModel.on('click', function () {
        let id = $(this).attr('data-id');
        openProjectLocationModal(id);
    });

    btn_unit_PropProjModel.on('click', function () {
        let id = $(this).attr('data-id');
        openUnitProjectModal(id);
    });

    //#endregion Project Table

    //#region Project Location Table

    if ($tbl_location) {
        var tbl_projectLocation = $("#tbl-location-add").DataTable({
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

        $("#tbl-location-add_filter, #tbl-location-add_length").hide();
    }

    tbl_projectLocation.on('select deselect draw', function () {
        var all = tbl_projectLocation.rows({ search: 'applied' }).count();
        let userSelectedRows = tbl_projectLocation.rows({ selected: true, search: 'applied' }).count();
        var id = tbl_projectLocation.rows({ selected: true }).data().pluck("Id").toArray().toString();

        $("#selectAllLocation").prop("checked", (all == userSelectedRows && all > 0));
    });

    $.fn.dataTable.ext.search.push(
        function (settings, searchData, index, rowData, counter) {
            var tableId = settings.nTable.id;

            if (tableId === "tbl-location-add") {
                var checked = $('input[id="filterLocation"]').is(':checked');

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

    $('input[id="filterLocation"]').on('change', function () {
        tbl_projectLocation.draw();
    });

    $("#selectAllLocation").on("change", function () {
        if ($(this).prop("checked")) {
            tbl_projectLocation.rows({ search: 'applied' }).select();
        } else {
            tbl_projectLocation.rows({ search: 'applied' }).deselect();
        }
    });

    btn_saveprojectlocation.on("click", function (e) {
        let selected = tbl_projectLocation.rows({ selected: true }).data().pluck("Id").toArray().toString();
        $("[name='PropertyLocationIds']").val(selected);

        let $form = $("#frm_projectLocation");
        let formData = $form.serialize();

        $form.on("submit", function (e) {
            e.preventDefault();

            if (!$(this).valid()) {
                messageBox("Fill all required fields!", "danger", true);
                return;
            }

            $.ajax({
                url: baseUrl + `PropertyProject/SaveProjectLocation`,
                method: "POST",
                data: formData,
                success: function (response) {
                    messageBox("Save project Successfuly!", "success");
                    $("#modal-location").modal("hide");
                    tbl_propProj.ajax.reload();
                },
                error: async function (jqXHR, textStatus, errorThrown) {
                    messageBox(jqXHR.responseText, "danger", true);
                }
            });
        });
    });

    //#endregion Project Location Table

    //#region Project Unit Table

    if ($tbl_unit) {
        var tbl_unit = $("#tbl-unit-add").DataTable({
            ajax: {
                url: '/PropertyUnit/GetAllPropertyUnit',
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

        $("#tbl-unit-add_filter, #tbl-unit-add_length").hide();
    }

    tbl_unit.on('select deselect draw', function () {
        var all = tbl_unit.rows({ search: 'applied' }).count();
        let userSelectedRows = tbl_unit.rows({ selected: true, search: 'applied' }).count();
        var id = tbl_unit.rows({ selected: true }).data().pluck("Id").toArray().toString();

        $("#selectAllUnit").prop("checked", (all == userSelectedRows && all > 0));
    });

    $.fn.dataTable.ext.search.push(
        function (settings, searchData, index, rowData, counter) {
            var tableId = settings.nTable.id;

            if (tableId === "tbl-unit-add") {
                var checked = $('input[id="filterUnit"]').is(':checked');

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

    $('input[id="filterUnit"]').on('change', function () {
        tbl_unit.draw();
    });

    $("#selectAllUnit").on("change", function () {
        if ($(this).prop("checked")) {
            tbl_unit.rows({ search: 'applied' }).select();
        } else {
            tbl_unit.rows({ search: 'applied' }).deselect();
        }
    });

    btn_saveprojectUnit.on("click", function (e) {
        let selected = tbl_unit.rows({ selected: true }).data().pluck("Id").toArray().toString();
        $("[name='PropertyUnitIds']").val(selected);

        let $form = $("#frm_projectUnit");
        let formData = $form.serialize();

        $form.on("submit", function (e) {
            e.preventDefault();

            if (!$(this).valid()) {
                messageBox("Fill all required fields!", "danger", true);
                return;
            }

            $.ajax({
                url: baseUrl + `PropertyProject/SaveProjectUnit`,
                method: "POST",
                data: formData,
                success: function (response) {
                    messageBox("Save unit Successfuly!", "success");
                    $("#modal-unit").modal("hide");
                    tbl_propProj.ajax.reload();
                },
                error: async function (jqXHR, textStatus, errorThrown) {
                    messageBox(jqXHR.responseText, "danger", true);
                }
            });
        });
    });
    //#endregion Project Unit Table
    function openProjectModal(id) {
        clearForm($form);

        if (id != 0) {
            $.ajax({
                url: `/PropertyProject/GetPropertyProjectById`,
                method: 'GET',
                data: {
                    id: id
                },
                success: function (propProject) {
                    $('[name="PropProjModel.Id"]').val(propProject ? propProject.Id : 0);
                    $('[name="PropProjModel.Name"]').val(propProject ? propProject.Name : "");
                    $('[name="PropProjModel.Description"]').val(propProject ? propProject.Description : "");
                    //$('[name="PropProjModel.CompanyId"]').data('selectize').setValue(propProject ? propProject.CompanyId : "");
                    companyDropdown.setValue(propProject.CompanyId);
                }
            });
        }

        $modal_PropProj.modal('show')
    }

    function openProjectLocationModal(id) {
        if (id != 0) {
            $.ajax({
                url: `/PropertyProject/GetPropertyProjectById`,
                method: 'GET',
                data: {
                    id: id
                },
                success: function (propProject) {
                    $('[name="PropProjLocModel.ProjectName"]').val(propProject ? propProject.Name : "");
                    $('[name="PropProjLocModel.ProjectId"]').val(propProject ? propProject.Id : 0);

                    $('[name="PropProjModel.Id"]').val(propProject ? propProject.Id : 0);

                    $('[name="PropProjModel.Name"]').val(propProject ? propProject.Name : "");
                    $('[name="PropProjModel.Description"]').val(propProject ? propProject.Description : "");
                    //$('[name="PropProjModel.CompanyId"]').data('selectize').setValue(propProject ? propProject.CompanyId : "");
                    companyDropdown.setValue(propProject.CompanyId);
                }
            });

            tbl_projectLocation.rows().deselect();

            $.ajax({
                url: baseUrl + "PropertyProject/GetPropertyLocationByProject",
                data: {
                    id: id
                },
                success: function (dt) {
                    console.log(dt)

                    let locationIds = [];
                    dt.forEach((item) => {
                        let id = `#${item.LocationId}`;
                        locationIds.push(id);
                    });

                    tbl_projectLocation.rows(locationIds).select();

                    $("#filterLocation").prop("checked", (locationIds.length > 0));
                    $("#filterLocation").trigger('change');
                }
            });
        }

        $modal_PropLoc.modal('show')
    }

    function openUnitProjectModal(id) {
        if (id != 0) {
            $.ajax({
                url: `/PropertyProject/GetPropertyProjectById`,
                method: 'GET',
                data: {
                    id: id
                },
                success: function (propProject) {
                    $('[name="PropProjModel.Name"]').val(propProject ? propProject.Name : "");
                    $('[name="PropUnitProjModel.ProjectId"]').val(propProject ? propProject.Id : 0);

                    $('[name="PropProjModel.Id"]').val(propProject ? propProject.Id : 0);

                    $('[name="PropProjModel.Name"]').val(propProject ? propProject.Name : "");
                    $('[name="PropProjModel.Description"]').val(propProject ? propProject.Description : "");
                    companyDropdown.setValue(propProject.CompanyId);
                }
            });

            tbl_unit.rows().deselect();

            $.ajax({
                url: baseUrl + "PropertyProject/GetPropertyUnitByProject",
                data: {
                    id: id
                },
                success: function (dt) {
                    console.log(dt)

                    let unitIds = [];
                    dt.forEach((item) => {
                        let id = `#${item.UnitId}`;
                        unitIds.push(id);
                    });

                    tbl_unit.rows(unitIds).select();

                    $("#filterUnit").prop("checked", (unitIds.length > 0));
                    $("#filterUnit").trigger('change');
                }
            });
        }

        $modal_Unit.modal('show');
    }
    function rebindValidator() {
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
    }
});