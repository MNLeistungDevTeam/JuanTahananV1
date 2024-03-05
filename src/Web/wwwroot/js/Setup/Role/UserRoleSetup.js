$(() => {
    "strict";
    const $tbl_userrole = document.querySelector('#tbl_user_role');
    var $modal = $('#modal-user-role');
    var $form = $('#user_role_form');
    var usersDropdown, $usersDropdown;
    $('select').selectize();
    $usersDropdown = $('#UserRole_UsersId').selectize({
        plugins: ["remove_button"],
        delimiter: ",",
        valueField: 'value',
        labelField: 'text',
        searchField: ['text','position'],
        selectOnTab: true,
        create: false,
        persist: true,
        preload: true,
        render: {
            item: function (item, escape) {
                return ("<div class='text-truncate' style='max-width:90%;'>" +
                    escape(item.text) + " <span class='text-muted'>(" + escape(item.position) + ")</span>" +
                    "</div>"
                );
            },
            option: function (item, escape) {
                return ("<div class='py-1 px-2'>" +
                    escape(item.text) + " <span class='text-muted'>(" + escape(item.position) + ")</span>" +
                    "</div>"
                );
            }
        }
    })
    usersDropdown = $usersDropdown[0].selectize;

    if ($tbl_userrole) {
        var tbl_userrole = $("#tbl_user_role").DataTable({
            ajax: {
                url: '/Role/GetUserRoles',
                method: 'GET',
                dataSrc: "",
                data: {
                    roleId: $('[name="UserRole.RoleId"]').val()
                }
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
                    data: null,
                    orderable: true,
                    className: 'ps-2',
                    render: function (data) {
                        return `
                                    <div class="d-flex align-items-center">
                                        <img src="${data.ProfilePicture == null ? '/images/user/default.png' : "/images/user/" + data.ProfilePicture}" class="rounded-circle avatar-sm img-thumbnail me-3" alt="profile">
                                        <div>
                                            <div>${data.Name}</div>
                                            <a href="${data.Email}" class="text-decoration-none">${data.Email}</a>
                                        </div>
                                    </div>
                                `;
                    }
                },
                {
                    data: 'Position',
                    orderable: !0,
                    className: 'align-middle text-center',
                    render: function (data) {
                        return `
                            <div class="d-flex justify-content-center align-items-center h-100">
                                ${data}
                            </div>
                        `;
                    }
                },
                {
                    data: 'JoinedDate',
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
                    data: 'StatusOnline',
                    orderable: !0,
                    className: 'align-middle text-center',
                    render: function (data) {
                        return `
                        <span class="badge fs-6 border bg-secondary">${data}</span>
                        `;
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

        tbl_userrole.on('select', () => {
            CheckRows(tbl_userrole);
        })
        tbl_userrole.on('deselect', () => {
            CheckRows(tbl_userrole);
        })
    }
    var btn_add_user_role = $('#btn_add_user_role').on('click', function () {
        applyUsers();
    })
    var btn_delete_user_role = $('#btn_delete_user_role').on('click', function (e) {
        e.preventDefault();
        var ids = tbl_userrole.rows('.selected').data().pluck('Id').toArray();
        Swal.fire({
            title: 'Are you sure?',
            text: `The following User/s will be removed on this role: ${ids.length}`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showLoaderOnConfirm: true,
            preConfirm: (login) => {
                return fetch(`/Role/RemoveFromRole/`,
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
                messageBox("User(s) successfully removed.", "success");
                tbl_userrole.ajax.reload(null, false)
            }
        })
    })
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
                tbl_userrole.ajax.reload();
                $modal.modal('hide');
                messageBox(`Module Successfully ${data}`, "success", true);
            },
            error: async function (jqXHR, textStatus, errorThrown) {
                button.html('Save').attr('disabled', false);
                messageBox(jqXHR.responseText, "danger", true);
            }
        });
    })
    function applyUsers() {
        clearForm($form, ['[name="UserRole.RoleId"]']);
        $('[name="UserRole.Id"]').val(0);
        var id = $('[name="UserRole.RoleId"]').val();
        fetchUsers()
        $modal.modal('show');
    }
    function fetchUsers() {
        usersDropdown.setValue(-1, false);
        usersDropdown.clearOptions();
        usersDropdown.load(function (callback) {
            $.ajax({
                url: '/Role/GetUsers',
                type: 'get',
                dataType: 'json',
                data: {
                    roleId: $('[name="UserRole.RoleId"]').val()
                },
                success: function (data) {
                    callback(data);
                },
                error: function () {
                    callback();
                }
            });
        })
    }
})