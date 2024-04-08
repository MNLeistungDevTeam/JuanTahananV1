$(async function () {
    "strict";
    const $tbl_role = document.querySelector('#tbl_role');
    const $modal = $('#modal-role');
    const $form = $("#role_form");
    const $role_add = $('#tbl-role-add');

    if ($tbl_role) {
        var tbl_role = $("#tbl_role").DataTable({
            ajax: {
                url: '/Role/GetAllRoles',
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
                    data: 'TotalModulesCount',
                    orderable: !0,
                    className: 'text-center'
                },
                {
                    data: 'TotalUserCount',
                    orderable: !0,
                    className: 'text-center'
                },
                {
                    data: 'IsDisabled',
                    orderable: !0,
                    className: 'text-center',
                    render: function (data) {
                        return data ? `<i class="fas fa-check">` : '<i class="mdi mdi-close">'
                    }
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

        tbl_role.on('select', () => {
            customCheckRows(tbl_role);
        })

        tbl_role.on('deselect', () => {
            customCheckRows(tbl_role);
        })

        tbl_role.on('draw', () => {
            customCheckRows(tbl_role);
        });

    }
    function customCheckRows(element) {
        var row2minimum = element.rows({ selected: true }).count();
        var id = tbl_role.rows('.selected').data().pluck('IsLocked').toArray()[0];
        var currentElement = $(`#${$(element.table().node()).attr('id')}`);
        while (true) {
            if (currentElement.hasClass('card-body') || currentElement.hasClass('tab-pane') || currentElement.hasClass('modal-content')) {
                let btn_add = currentElement.find('.btn_add');
                let btn_delete = currentElement.find('.btn_delete');
                let btn_edit = currentElement.find('.btn_edit');
                let btn_view = currentElement.find('.btn_view');
                if (row2minimum > 1) {
                    btn_delete.attr('disabled', id >= 1 ? true : false);
                    btn_view.attr('disabled', true)
                    btn_edit.attr('disabled', true);
                    btn_add.attr('disabled', true);
                } else if (row2minimum === 1) {
                    btn_delete.attr('disabled', id === 1 ? true : false);
                    btn_edit.attr('disabled', false);
                    btn_view.attr('disabled', false)
                    btn_add.attr('disabled', true);
                } else {
                    btn_delete.attr('disabled', true);
                    btn_view.attr('disabled', true)
                    btn_edit.attr('disabled', true);
                    btn_add.attr('disabled', false);
                }
                break;
            }
            currentElement = currentElement.parent();
        }
    }
    if ($role_add) {
        var role_add = $("#tbl-role-add").DataTable({
            ajax: {
                url: '/Role/GetRoleSetupAccess',
                method: 'GET',
                data: function () {
                    return {
                        id: $('[name="Role.Id"]').val()
                    }
                },
                dataSrc: ""
            },
            columns: [
                {
                    data: null,
                    orderable: !0,
                    className: 'fw-semibold',
                    render: function (data) {
                        return `
                        <input type="hidden" id="RoleAccess_RoleId[${data.Index}]" name="RoleAccess[${data.Index}].RoleId" value="${data.RoleId}">
                        <input type="hidden" id="RoleAccess_ModuleId[${data.Index}]" data-val="true" data-val-required="this field is required." name="RoleAccess[${data.Index}].ModuleId" value="${data.ModuleId}">
                        <input type="hidden" id="RoleAccess_Id[${data.Index}]" data-val="true" data-val-required="The Id field is required." name="RoleAccess[${data.Index}].Id" value="${data.Id}">
                        ${data.ModuleName}
                        `;
                    }
                },
                {
                    data: null,
                    orderable: !0,
                    className: 'text-center text-start',
                    render: function (data) {
                        return `
                            <div class="">
                                <input type="checkbox" class="form-check-input" id="RoleAccess_CanRead[${data.Index}]" data-val="true" data-val-required="The CanRead field is required." name="RoleAccess[${data.Index}].CanRead" value="${data.CanRead}" ${data.CanRead ? 'checked' : ''}>
                                <label class="form-input-label" for="RoleAccess_CanRead[${data.Index}]"></label>
                            </div>
                        `;
                    }
                },
                {
                    data: null,
                    orderable: !0,
                    className: 'text-center',
                    render: function (data) {
                        return `
                            <div class="">
                                <input type="checkbox" class="form-check-input" id="RoleAccess_CanCreate[${data.Index}]" data-val="true" data-val-required="The CanCreate field is required." name="RoleAccess[${data.Index}].CanCreate" value="${data.CanCreate}" ${data.CanCreate ? 'checked' : ''}>
                                <label class="form-input-label" for="RoleAccess_CanCreate[${data.Index}]"></label>
                            </div>
                        `;
                    }
                },
                {
                    data: null,
                    orderable: !0,
                    className: 'text-center',
                    render: function (data) {
                        return `
                            <div class="">
                                <input type="checkbox" class="form-check-input" id="RoleAccess_CanModify[${data.Index}]" data-val="true" data-val-required="The CanModify field is required." name="RoleAccess[${data.Index}].CanModify" value="${data.CanModify}" ${data.CanModify ? 'checked' : ''}>
                                <label class="form-input-label" for="RoleAccess_CanModify[${data.Index}]"></label>
                            </div>
                        `;
                    }
                },
                {
                    data: null,
                    orderable: !0,
                    className: 'text-center',
                    render: function (data) {
                        return `
                            <div class="">
                                <input type="checkbox" class="form-check-input" id="RoleAccess_CanDelete[${data.Index}]" data-val="true" data-val-required="The CanDelete field is required." name="RoleAccess[${data.Index}].CanDelete" value="${data.CanDelete}" ${data.CanDelete ? 'checked' : ''}>
                                <label class="form-input-label" for="RoleAccess_CanDelete[${data.Index}]"></label>
                            </div>
                        `;
                    }
                },
                {
                    data: null,
                    orderable: !0,
                    className: 'text-center',
                    render: function (data) {
                        return `
                            <div class="">
                                <input type="checkbox" class="form-check-input" id="RoleAccess_FullAccess[${data.Index}]" data-val="true" data-val-required="The FullAccess field is required." name="RoleAccess[${data.Index}].FullAccess" value="${data.FullAccess}" ${data.FullAccess ? 'checked' : ''}>
                                <label class="form-input-label" for="RoleAccess_FullAccess[${data.Index}]"></label>
                            </div>
                        `;
                    }
                }
            ],
            drawCallback: function () {
                $(".dataTables_paginate > .pagination").addClass("pagination-rounded");
                $('li.paginate_button.page-item.active > a').addClass('waves-effect');

                // Check if admin access is checked
                if ($("[name='Role.AdminAccess']").prop("checked")) {
                    // Check all checkboxes in the table
                    $("[id^='RoleAccess_FullAccess'], [id^='RoleAccess_CanCreate'], [id^='RoleAccess_CanModify'], [id^='RoleAccess_CanDelete'], [id^='RoleAccess_CanRead']").prop("checked", true);
                }
                checkIfRoleAdminAccess();
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
            order: [[4, "asc"]],
            searchHighlight: true,
            stateSave: false,
            bLengthChange: false,
            dom: 'lrtip',
            processing: true,
            paging: false // Disable paging
        });
    }
    var btn_add_role = $('#btn_add_role').on('click', function (e) {
        e.preventDefault();
        applyRole();
    });
    var btn_edit_role = $('#btn_edit_role').on('click', function (e) {
        e.preventDefault();
        var id = tbl_role.rows('.selected').data().pluck('Id').toArray()[0];
        applyRole(id);
    });
    var btn_view_role = $('#btn_view_role').on('click', function (e) {
        e.preventDefault();
        var code = tbl_role.rows('.selected').data().pluck('Name').toArray()[0];
        location.href = "/Role/UserRole/" + code;
    });
    var btn_delete_role = $('#btn_delete_role').on('click', function (e) {
        e.preventDefault();
        var ids = tbl_role.rows('.selected').data().pluck('Id').toArray();
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
                return fetch(`/Role/DeleteRoles/`,
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
                messageBox("Role(s) successfully deleted.", "success");
                tbl_role.ajax.reload(null, false)
            }
        })
    })
    function applyRole(id = 0) {
        clearForm($form);
        $('[name="Role.Id"]').val(0);
        $('[name="Role.IsDisabled"]').prop('checked', false);
        $('[name="Role.IsDisabled"]').val(false);
        $("[name='Role.AdminAccess']").prop("checked", false);
        $("[name='Role.AdminAccess']").val(false);

        if (id != 0) {
            fetchRole(id, function (callback) {
                $('[name="Role.Id"]').val(callback.Id);
                $('[name="Role.Name"]').val(callback.Name);
                $('[name="Role.Description"]').val(callback.Description);
                $('[name="Role.IsDisabled"]').prop('checked', callback.IsDisabled);
                $('[name="Role.IsDisabled"]').val(callback.IsDisabled);
                role_add.ajax.reload();
                checkIfRoleAdminAccess();
            })
        } else {
            role_add.ajax.reload();
        }
        $modal.modal('show')
    }

    function fetchRole(id, callback) {
        $.ajax({
            url: `/Role/GetRoleById`,
            method: 'GET',
            data: {
                id: id
            },
            success: callback
        })
    }

    $('#Role_IsDisabled').on('change', function () {
        var isChecked = $(this).is(':checked');
        $('#role_form').find('input[name="Role.IsDisabled"]').val(isChecked);
    });

    $form.on('submit', async function (e) {
        e.preventDefault();
        var button = $(this).find('button[type="submit"]');
        var formData = $(this).serialize();

        if (!$(this).valid())
            return;

        var valResult = await customValidationForm();
        if (!valResult)
            return;

        // Log serialized form data
        console.log("Form Data:", formData);

        var roleId = $('[name="Role.Id"]').val();

        $.ajax({
            url: $(this).attr('action'),
            method: $(this).attr('method'),
            data: formData,
            beforeSend: function () {
                button.html(`<i class="mdi mdi-spin mdi-loading"></i> Saving..`).attr('disabled', true);
            },
            success: function (response) {
                let successMessage = "";
                successMessage = `Role Successfully ${roleId == 0 ? 'Added' : 'Updated'}!`;
                messageBox(successMessage, "success", true);
                button.html('Save').attr('disabled', false);
                tbl_role.ajax.reload();
                $modal.modal('hide');
                clearForm($form);
            },
            error: async function (jqXHR, textStatus, errorThrown) {
                button.html('Save').attr('disabled', false);
                messageBox(jqXHR.responseText, "danger", true);
            }
        });
    })

    function customValidationForm() {
        var id = $('[name="Role.Id"]').val();
        var codeName = $('[name="Role.Name"]').val().trim();

        if (id !== '0') {
            return Promise.resolve(true); // Early resolve for existing role
        }

        return new Promise((resolve, reject) => {
            $.ajax({
                url: baseUrl + "Role/GetAllRoles",
                method: "GET",
                dataType: 'json',
                success: function (response) {
                    const roleNames = response.map(role => role.Name.toLowerCase().trim());
                    if (!roleNames.includes(codeName.toLowerCase())) {
                        resolve(true); // Validation successful
                    } else {
                        messageBox("Code must be unique!", "danger", true);
                        reject(new Error("Duplicate code name"));
                    }
                },
                error: function (jqXHR, status, error) {
                    messageBox(jqXHR.responseText, "danger", true);
                    reject(new Error("Error fetching roles"));
                }
            });
        });
    }

    $(document).on('change', `[id^='RoleAccess_CanCreate['], [id^='RoleAccess_CanModify['], [id^='RoleAccess_CanDelete['], [id^='RoleAccess_CanRead[']`, function () {
        let Id = $(this).prop("id");
        let IdNum = getIdNum(Id);
        const canCreate = $(`input[id='RoleAccess_CanCreate[${IdNum}]']`).prop("checked");
        const canModify = $(`input[id='RoleAccess_CanModify[${IdNum}]']`).prop("checked");
        const canDelete = $(`input[id='RoleAccess_CanDelete[${IdNum}]']`).prop("checked");
        const canRead = $(`input[id='RoleAccess_CanRead[${IdNum}]']`).prop("checked");
        $(`input[id='RoleAccess_FullAccess[${IdNum}]']`).prop("checked", canCreate && canModify && canDelete && canRead);
        checkIfRoleAdminAccess();
    });

    // Event listener for admin access checkbox change
    $(document).on('change', '[name="Role.AdminAccess"]', function () {
        let isChecked = $(this).prop("checked");

        // Check all checkboxes in the table if admin access is checked
        $("[id^='RoleAccess_FullAccess'], [id^='RoleAccess_CanCreate'], [id^='RoleAccess_CanModify'], [id^='RoleAccess_CanDelete'], [id^='RoleAccess_CanRead']").prop("checked", isChecked);
        $("[id^='RoleAccess_FullAccess'], [id^='RoleAccess_CanCreate'], [id^='RoleAccess_CanModify'], [id^='RoleAccess_CanDelete'], [id^='RoleAccess_CanRead']").val(isChecked);
    });
    $(document).on('change', "[id^='RoleAccess_FullAccess[']", function () {
        let Id = $(this).prop("id");
        let IdNum = getIdNum(Id);
        let checked = $(this).prop("checked");

        $(`[id^='RoleAccess_CanCreate[${IdNum}]'], [id^='RoleAccess_CanModify[${IdNum}]'], [id^='RoleAccess_CanDelete[${IdNum}]'], [id^='RoleAccess_CanRead[${IdNum}]']`).prop("checked", checked);
        $(`[id^='RoleAccess_CanCreate[${IdNum}]'], [id^='RoleAccess_CanModify[${IdNum}]'], [id^='RoleAccess_CanDelete[${IdNum}]'], [id^='RoleAccess_CanRead[${IdNum}]']`).val(checked);
        checkIfRoleAdminAccess();
    });

    function checkIfRoleAdminAccess() {
        var adminAccessCheckbox = $("[name='Role.AdminAccess']");
        var allCheckboxes = $("[id^='RoleAccess_FullAccess'], [id^='RoleAccess_CanCreate'], [id^='RoleAccess_CanModify'], [id^='RoleAccess_CanDelete'], [id^='RoleAccess_CanRead']");

        // Check if all checkboxes are checked
        var allChecked = true;
        allCheckboxes.each(function () {
            if (!$(this).prop("checked")) {
                allChecked = false;
                return false; // Exit the loop early if any checkbox is not checked
            }
        });

        // Update the state of the "Admin Access" checkbox
        adminAccessCheckbox.prop("checked", allChecked);
        adminAccessCheckbox.val(allChecked);
        // Update the value attribute if needed
    }

    function getIdNum(id) {
        let start = id.indexOf("[");
        let end = id.indexOf("]");
        return id.substring(start + 1, end);
    }
})