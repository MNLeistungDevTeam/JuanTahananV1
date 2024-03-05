$(() => {
    "strict";
    const $tbl_module = document.querySelector('#tbl_module');
    const $modal = $('#modal-module');
    const $form = $("#module_form");

    var actionDropdown, $actionDropdown;
    var controllerDropdown, $controllerDropdown;
    var parentmoduleDropdown, $parentmoduleDropdown;
    var modulestatusDropdown, $modulestatusDropdown;

    rebindValidators();

    $('select').selectize();
    $controllerDropdown = $('#Module_Controller').selectize({
        valueField: 'value',
        labelField: 'text',
        searchField: ['text'],
        selectOnTab: true,
        preload: true
    })
    $actionDropdown = $('#Module_Action').selectize({
        valueField: 'value',
        labelField: 'text',
        searchField: ['text'],
        selectOnTab: true,
        preload: true
    })
    $modulestatusDropdown = $('#Module_ModuleStatusId').selectize({
        valueField: 'value',
        labelField: 'text',
        searchField: ['text'],
        selectOnTab: true,
        preload: true
    })
    $parentmoduleDropdown = $('#Module_ParentModuleId').selectize({
        valueField: 'value',
        labelField: 'text',
        searchField: ['text'],
        selectOnTab: true,
        preload: true,
        render: {
            option: function (item, escape) {
                return ("<div class='py-1 px-2'>" +
                    escape(item.Description) +
                    "</div>"
                );
            }
        }
    })
    parentmoduleDropdown = $parentmoduleDropdown[0].selectize;
    controllerDropdown = $controllerDropdown[0].selectize;
    actionDropdown = $actionDropdown[0].selectize;
    modulestatusDropdown = $modulestatusDropdown[0].selectize;
    if ($tbl_module) {
        var tbl_module = $("#tbl_module").DataTable({
            ajax: {
                url: '/Module/GetAllModules',
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
                    data: null,
                    orderable: !0,
                    className: 'text-center',
                    render: function (data) {
                        return `<span class="badge m-0 bg-${data.StatusColor} p-1">${data.StatusName}</span>`;
                    }
                },
                {
                    data: 'Code',
                    orderable: !0,
                    className: 'text-center'
                },
                {
                    data: 'Ordinal',
                    orderable: !0,
                    className: 'text-center'
                },
                {
                    data: 'Description',
                    orderable: !0,
                    className: 'text-center'
                },
                {
                    data: 'BreadName',
                    orderable: !0,
                    className: 'text-center'
                },
                {
                    data: null,
                    orderable: !1,
                    className: 'text-center',
                    render: function (data) {
                        return data.Controller !== '' ? `${data.Controller}/${data.Action}` : '';
                    }
                },
                {
                    data: 'ParentModuleName',
                    orderable: !0,
                    className: 'text-center'
                },
                {
                    data: 'Icon',
                    orderable: !1,
                    className: 'text-center',
                    render: function (data) {
                        return data != '' || data != null ? `<i class="${data}"></i>` : ''
                    }
                },
                {
                    data: 'IsBreaded',
                    orderable: !0,
                    className: 'text-center',
                    render: function (data) {
                        return data ? `<i class="fas fa-check">` : '<i class="mdi mdi-close">'
                    }
                },
                {
                    data: 'IsVisible',
                    orderable: !0,
                    className: 'text-center',
                    render: function (data) {
                        return data ? `<i class="fas fa-eye">` : '<i class="fas fas fa-eye-slash">'
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

        tbl_module.on('select', () => {
            CheckRows(tbl_module);
        })
        tbl_module.on('deselect', () => {
            CheckRows(tbl_module);
        })
    }

    var btn_edit_module = $('#btn_edit_module').on('click', function (e) {
        e.preventDefault();
        var id = tbl_module.rows('.selected').data().pluck('Id').toArray()[0];
        applyModuleType(id);
    })
    var btn_add_module = $('#btn_add_module').on('click', function (e) {
        e.preventDefault();
        applyModuleType();
    })
    var btn_delete_module = $('#btn_delete_module').on('click', function (e) {
        e.preventDefault();
        var ids = tbl_module.rows('.selected').data().pluck('Id').toArray();
        Swal.fire({
            title: 'Are you sure?',
            text: `The following Module/s will be deleted: ${ids.length}`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showLoaderOnConfirm: true,
            preConfirm: (login) => {
                return fetch(`/Module/DeleteModules/`,
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
                messageBox("Module(s) successfully deleted.", "success");
                tbl_module.ajax.reload(null, false)
            }
        })
    })

    function rebindValidators() {
        let $form = $('#module_form');
        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate($form.data("unobtrusiveValidation").options);
        $form.data("validator").settings.ignore = "";

        $form.on('submit', function (e) {
            e.preventDefault();
            var button = $(this).find('button[type="submit"]');
            var formData = new FormData(e.target);

            if (!$(this).valid()) {
                messageBox("Please fill up all required fields.", "error", true);
                return;
            }

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
                success: function (data) {
                    button.html('Save').attr('disabled', false);
                    tbl_module.ajax.reload();
                    $modal.modal('hide');
                    messageBox(`Module Successfully ${data}`, "success", true);
                },
                error: async function (jqXHR, textStatus, errorThrown) {
                    button.html('Save').attr('disabled', false);
                    messageBox(jqXHR.responseText, "danger", true);
                }
            });
        })

    }
    
    function applyModuleType(id = 0) {
        clearForm($form);
        $(`[name="Module.IsVisible"]`).prop('checked', true);
        $(`[name="Module.IsBreaded"]`).prop('checked', true);
        $(`[name="Module.IsVisible"]`).val(true);
        $(`[name="Module.IsBreaded"]`).val(true);
        $('[name="Module.Id"]').val(0);
        $('[name="Module.Ordinal"]').val(1);
        if (id != 0) {
            fetchModule(id, function (callback) {
                $(`[name="Module.IsVisible"]`).prop('checked', callback.IsVisible);
                $(`[name="Module.IsBreaded"]`).prop('checked', callback.IsBreaded);
                $(`[name="Module.IsVisible"]`).val(callback.IsVisible);
                $(`[name="Module.IsBreaded"]`).val(callback.IsBreaded);
                $('[name="Module.Ordinal"]').val(callback.Ordinal);
                $('[name="Module.Description"]').val(callback.Description);
                $('[name="Module.Code"]').val(callback.Code);
                $('[name="Module.BreadName"]').val(callback.BreadName);
                $('[name="Module.Ordinal"]').val(callback.Ordinal);
                $('[name="Module.Icon"]').val(callback.Icon);
                $('[name="Module.Id"]').val(callback.Id);
                fetchController(callback.Controller, callback.Action);
                fetchModuleStatus(callback.ModuleStatusId);
                fetchParentModule(callback.Id, callback.ParentModuleId);
            })
        } else {
            fetchParentModule();
            fetchController();
            fetchModuleStatus();
        }
        $modal.modal('show');
    }

    function fetchModule(id, callback) {
        $.ajax({
            url: `/Module/GetModuleById`,
            method: 'GET',
            data: {
                id: id
            },
            success: callback
        })
    }
    function fetchController(controller = '', action = '') {
        controllerDropdown.setValue(-1, false);
        controllerDropdown.clearOptions();
        controllerDropdown.load(function (callback) {
            $.ajax({
                url: '/api/Collection/GetControllers',
                type: 'get',
                datatype: 'json',
                success: function (data) {
                    controllerDropdown.addOption(data);
                    controllerDropdown.setValue(controller);
                    callback(data);
                },
                error: function () {
                    callback();
                }
            });
        })
        controllerDropdown.on('change', function (data) {
            fetchActionByControllerName(data, action);
        })
    }
    function fetchParentModule(id = 0, parentId = 0) {
        parentmoduleDropdown.setValue(-1, false);
        parentmoduleDropdown.clearOptions();
        parentmoduleDropdown.load(function (callback) {
            $.ajax({
                url: `/Module/GetParentModule/${id}`,
                type: 'get',
                datatype: 'json',
                success: function (data) {
                    parentmoduleDropdown.addOption(data);
                    parentmoduleDropdown.setValue(parentId);
                    callback(data);
                },
                error: function () {
                    callback();
                }
            });
        })
    }
    function fetchModuleStatus(status = 0) {
        modulestatusDropdown.setValue(-1, false);
        modulestatusDropdown.clearOptions();
        modulestatusDropdown.load(function (callback) {
            $.ajax({
                url: `/Module/GetModuleStatus`,
                type: 'get',
                datatype: 'json',
                success: function (data) {
                    modulestatusDropdown.addOption(data);
                    modulestatusDropdown.setValue(status);
                    callback(data);
                },
                error: function () {
                    callback();
                }
            });
        })
    }
    function fetchActionByControllerName(value, action) {
        actionDropdown.setValue(-1, false);
        actionDropdown.clearOptions();
        actionDropdown.load(function (callback) {
            $.ajax({
                url: `/api/collection/GetActionsForController/${value}`,
                type: 'get',
                datatype: 'json',
                success: function (data) {
                    actionDropdown.addOption(data);
                    actionDropdown.setValue(action);
                    callback(data);
                },
                error: function () {
                    callback();
                }
            });
        })
    }
})