const role_div = $("#role_list_div");
const tbl_user_tbody = "#tbl_user tbody";
const changePassModal = $("#change-pass-modal");
const frmChangePass = $("#frm_change_pass");
const currentUserId = $("#txt_userId").val();
const userDeveloperId = $("#txt_developerId").val();

var predefinedRoles = [];
var tbl_user;
var userModal = $("#user-modal");
var defaultProfile = "/images/user/default.png";
$(function () {
    //#region Initialization

    var $userRoleDropdown, userRoleDropdown;
    var $developerDropdown, developerDropdown;
    const dateFrom = moment().startOf("month").format("YYYY-MM-DD");
    const dateTo = moment().endOf("month").format("YYYY-MM-DD");

    /*    const onlineUserCount = new signalR.HubConnectionBuilder().withUrl(baseUrl + "onlineUserHub").build();*/

    $(".selectize").selectize();
    rebindValidators();

    loadUsers();

    //#endregion

    //#region Selectize

    $userRoleDropdown = $("[name='User.UserRoleId']").selectize({
        valueField: 'Id',
        labelField: 'UserName',
        searchField: ['UserName', 'Name'],
        selectOnTab: true,
        preload: true,
        persist: false,
        search: false,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + "Role/GetAllRoles",
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
                    "</div>");
            },
            option: function (item, escape) {
                return (
                    "<div class='py-1 px-2'>" +
                    escape(item.Name) +
                    "</div>"
                );
            }
        }
    });

    userRoleDropdown = $userRoleDropdown[0].selectize;

    $developerDropdown = $("[name='User.PropertyDeveloperId']").selectize({
        valueField: 'Id',
        labelField: 'Name',
        searchField: ['Name'],
        selectOnTab: true,
        preload: true,
        persist: false,
        search: false,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + "Company/GetDevelopers",
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
                    "</div>");
            },
            option: function (item, escape) {
                return (
                    "<div class='py-1 px-2'>" +
                    escape(item.Name) +
                    "</div>"
                );
            }
        }
    });

    developerDropdown = $developerDropdown[0].selectize;


    
    //#endregion

    //#region Events

    $('#txtSearch').on('input', function () {
        var value = $(this).val().toLowerCase();
        $("#div-user-container #div-user-cards .col-xl-3").filter(function () {
            var data = ($(this).find('.card-body h4').text().toLowerCase().indexOf(value) > -1);
            $(this).toggle(data);

            //if (!data) {
            //    $("#noSearch").show();
            //}
            //else {
            //    $("#noSearch").hide();
            //}
        });
    });

    $("#btnRefresh").on("click", async function () {
        await loadUsers();
    });
    $("#btnAdd").on("click", async function () {
        await openUserModal();
    });

    $(document).on("click", ".js-edit", async function () {
        let id = $(this).attr("data-id");
        await openUserModal(id);
    });

    $(document).on("click", ".js-change-pass", function () {
        let userId = $(this).attr("data-id");
        let userName = $(this).attr("data-name");

        openChangePassModal(userId, userName);
    });

    $(document).on("click", ".js-unlock", function () {
        let ids = $(this).attr("data-id");
        let name = $(this).attr("data-name");

        Swal.fire({
            title: 'Are you sure?',
            text: `The following user/s will be unlocked: ${name}`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showLoaderOnConfirm: true,
            preConfirm: (login) => {
                return fetch(`${baseUrl}User/UnlockUser/`,
                    {
                        method: "POST",
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        body: `userId=${ids}`
                    })
                    .then(response => {
                        if (!response.ok) {
                            return response.text().then(errorMessage => {
                                throw new Error(errorMessage);
                            });
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
                messageBox("User(s) successfully unlocked.", "success");
                tbl_user.ajax.reload(null, false)
            }
        })
    });

    $("#User_ProfilePictureFile").on("change", function () {
        let container = $("#imagePreview");
        readUrl(this, container);
    });
    $("#User_SignatureFile").on("change", function () {
        let container = $("#imagePreview2");
        readUrl(this, container);
    });
    $("#btn_add_role").on("click", function () {
        OpenRoleModal();
    });
    $("#btn_edit_role").on("click", function () {
        let id = $(this).attr("data-id");
        OpenRoleModal(id);
    });
    $("#btn_add_user_approver").on("click", function () {
        addApprover();
        rebindValidators();
    });
    $(document).on("click", ".btn_remove_approver", function (e) {
        e.preventDefault();
        $(this).closest(".approver_row").remove();
        fixElementSequence(".approver_row");
        rebindValidators();
    });

    $(document).on("change", "[Id^='UserApprover_ApproverId_']", function () {
        //getApproverInputs();
    });

    $("#change-pass-modal").on("shown.bs.modal", function () {
        frmChangePass.clearValidation();
    });

    frmChangePass.on("submit", function (e) {
        e.preventDefault();

        var formData = $(this).serialize().replaceAll("ChangePassword.", "");
        let button = $("#btn_change_pass");
        let changepassModal = $("#change-pass-modal");
        if ($(this).valid() == false) {
            messageBox("Please fill out all required fields!", "danger", true);
            return;
        }

        $.ajax({
            url: frmChangePass.attr("action"),
            method: frmChangePass.attr("method"),
            data: formData,
            success: function (response) {
                changepassModal.modal("hide");
                toastr.success("Change Password successfully");
                tbl_user.ajax.reload(null, false);
                button.attr({ disabled: false });
                button.html("<span class='mdi mdi-content-save-outline'></span> Save Changes");
            },
            error: function (response) {
                messageBox(response.responseText, "danger");
                button.html("<span class='mdi mdi-content-save-outline'></span> Save Changes");
                button.attr({ disabled: false });
            }
        });
    });

    //#endregion

    //#region Methods

    function resetUserForm() {
        let userModalLabel = $("#user-modalLabel");
        userModalLabel.html('<span class="fe-plus"></span> Add User');
        let DefaultProfile = $("#Layout-defaultProfile").html();
        let DefaultSignature = $("#Layout-defaultSignature").html();

        $("input[name='User.Id']").val(0);
        $("input[name='User.LastName']").val("");
        $("input[name='User.FirstName']").val("");
        $("input[name='User.MiddleName']").val("");
        $("input[name='User.UserName']").val("");
        $("input[name='User.Password']").val("");
        $("input[name='User.Email']").val("");
        $("input[name='User.Position']").val("");
        $("input[name='User.UserPicture']").val("");
        $("input[name='User.UserSignature']").val("");
        //$("select[name='User.RoleId']").val("").trigger("change");
        $("select[name='User.Gender']").data('selectize').setValue("");
        $("input[name='User.IsEnabled']").prop("checked", true);

        userRoleDropdown.setValue('');
        userRoleDropdown.unlock();

        $("#tbl_user_approver tbody").empty();

        loadImageOnPreview(DefaultProfile, DefaultProfile, '#imagePreview');
        loadImageOnPreview(DefaultSignature, DefaultSignature, '#imagePreview2');
    }

    async function openUserModal(id = 0) {
        let userModal = $("#user-modal");
        let userModalLabel = $("#user-modalLabel");
        let DefaultProfile = $("#Layout-defaultProfile").html();
        let DefaultSignature = $("#Layout-defaultSignature").html();
        let ActualPicture = "", ActualSignature = "";
        let userModalOverlay = $("#user-modal-overlay");

        //method removing iformfile
        var profilePictureFile = $('#User_ProfilePictureFile')[0];

        profilePictureFile.files[0];

        // remove filename
        $('#User_ProfilePictureFile').val('');

        // reset and clearvalidation
        resetUserForm();
        $("#frm_user").clearValidation();

        if (id == 0) {
            //$("#div_userproject").hide();
            //$("#user-approver-tab").show();
            // $("#userrole_div").attr('hidden', true);
            $("#div_userpassword").attr('hidden', false);

            if (userDeveloperId != 0) {

                developerDropdown.setValue(userDeveloperId);
                developerDropdown.lock();
            }

        }

        else {
            /*$("#div_userproject").show();*/
            $//("#user-approver-tab").show();
            //$("#userrole_div").attr('hidden', false);
            $("#div_userpassword").attr('hidden', true);
            userModalLabel.html('<span class="fe-edit"></span> Update User');
            userModalOverlay.attr({ hidden: false });

            var userInfo = await getUserInfo(id);
            //var userProjects = await getUserProjects(id);
            //var userApprovers = await getUserApprover(id);

            UserId = userInfo.Id;
            let UserPicture = userInfo.ProfilePicture;

            let UserSignature = userInfo.Signature;

            $("input[name='User.Id']").val(userInfo.Id);
            $("input[name='User.Prefix']").val(userInfo.Prefix);
            $("input[name='User.LastName']").val(userInfo.LastName);
            $("input[name='User.FirstName']").val(userInfo.FirstName);
            $("input[name='User.MiddleName']").val(userInfo.MiddleName);
            $("input[name='User.Suffix']").val(userInfo.Suffix);
            $("input[name='User.UserName']").val(userInfo.UserName);
            $("input[name='User.Password']").val(userInfo.Password);
            $("input[name='User.Email']").val(userInfo.Email);
            $("input[name='User.Position']").val(userInfo.Position);
            $("select[name='User.Gender']").data('selectize').setValue(userInfo.Gender);
            $("[name='User.IsDisabled']").prop("checked", userInfo.IsDisabled);

            userRoleDropdown.unlock();
            userRoleDropdown.setValue(userInfo.UserRoleId);

            developerDropdown.unlock();
            developerDropdown.setValue(userInfo.PropertyDeveloperId);


            if (userDeveloperId != 0) {

                developerDropdown.setValue(userDeveloperId);
                developerDropdown.lock();
            }

            //userRoleDropdown.lock();

            if (UserPicture === "" || UserPicture === null) ActualPicture = DefaultProfile;
            else ActualPicture = UserPicture;

            if (UserSignature === "" || UserSignature === null) ActualSignature = DefaultSignature;
            else ActualSignature = UserSignature;

            loadImageOnPreview(ActualPicture, DefaultProfile, '#imagePreview');
            loadImageOnPreview(ActualSignature, DefaultSignature, '#imagePreview2');

            //for (const userApprover of userApprovers) {
            //    addApprover(userApprover);
            //}

            userModalOverlay.attr({ hidden: true });
        }

        userModal.modal("show");
    }

    function addApprover(itemObj = []) {
        let count = $(".approver_row").length;
        let approverIdDropdown, $approverIdDropdown;

        let elementToAdd = `<tr class="approver_row">
                                <th class="align-middle">
                                    <input type="hidden" id="UserApprover_Id_[${count}]" data-val="true" data-val-required="The Id field is required." name="UserApprover[${count}].Id" value="${itemObj.Id || "0"}">
                                    <span class="ui-widget-header ui-sortable-handle bg-transparent border-0 fs-3" style="cursor: move;display: inline-block;">
                                        <i class="fas fa-grip-vertical"></i>
                                    </span>
                                </th>
                                <th class="align-middle">
                                    <button class="btn btn-soft-danger btn-sm btn_remove_approver" type="button">
                                        <span class="fa fa-times"></span>
                                    </button>
                                </th>
                                <th>
                                    <div class="form-group form-group-sm">
                                        <select class="form-control form-control-sm" id="UserApprover_ApproverId_[${count}]" data-value="${itemObj.ApproverId || ""}" data-val="true" data-val-required="The Approver field is required." name="UserApprover[${count}].ApproverId">
                                            <option value="">Select Approver...</option>
                                        </select>
                                        <span class="text-danger field-validation-valid" data-valmsg-for="UserApprover[${count}].ApproverId" data-valmsg-replace="true"></span>
                                    </div>
                                </th>
                            </tr>`;

        $("#tbl_user_approver tbody").append(elementToAdd);

        $approverIdDropdown = $(`[id="UserApprover_ApproverId_[${count}]"]`).selectize({
            valueField: 'Id',
            labelField: 'UserName',
            searchField: ['UserName'],
            preload: true,
            load: function (query, callback) {
                $.ajax({
                    url: baseUrl + "User/GetUsers",
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
                        escape(item.UserName) + " <span class='text-muted'>(" + escape(item.Name) + ")</span>" +
                        "</div>"
                    );
                },
                option: function (item, escape) {
                    return ("<div class='py-1 px-2'>" +
                        escape(item.UserName) + " <span class='text-muted'>(" + escape(item.Name) + ")</span>" +
                        "</div>"
                    );
                }
            },
            persist: false
        });

        approverIdDropdown = $approverIdDropdown[0].selectize;

        approverIdDropdown.on('load', function () {
            approverIdDropdown.setValue(itemObj.ApproverId || "");
            approverIdDropdown.off('load');
        });

        refreshApproverList(approverIdDropdown);
    }

    function rebindValidators() {
        let $form = $("#frm_user");
        let button = $("#btn_save_user");

        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate($form.data("unobtrusiveValidation").options);
        $form.data("validator").settings.ignore = "";

        $form.on("submit", function (e) {
            e.preventDefault();
            let formData = new FormData(e.target);

            if ($(this).valid() == false) {
                messageBox("Please fill out all required fields!", "danger", true);

                return;
            }

            //if (customValidateForm() == false) return;

            $.ajax({
                url: $(this).attr("action"),
                method: $(this).attr("method"),
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function () {
                    button.html("<span class='spinner-border spinner-border-sm'></span> Saving...");
                    button.attr({ disabled: true });
                },
                success: function (response) {
                    let recordId = $("input[name='User.Id']").val();
                    console.log(recordId);
                    let type = (recordId == 0 ? "Added!" : "Updated!");
                    let successMessage = `User Successfully ${type}`;
                    messageBox(successMessage, "success", true);

                    if (recordId == currentUserId) {
                        updateUserProfile();
                    }

                    loadUsers();
                    button.attr({ disabled: false });
                    button.html("<span class='mdi mdi-content-save-outline'></span> Save");

                    userModal.modal('hide');
                },
                error: function (response) {
                    messageBox(response.responseText, "danger");
                    button.html("<span class='mdi mdi-content-save-outline'></span> Save");
                    button.attr({ disabled: false });
                }
            });
        });
    }

    function refreshApproverList(approverSelectize) {
        let userIdArray = [];

        $(".approver_row").each(function () {
            let user_approver_id = $(this).find("[Id^='UserApprover_ApproverId_']").val();
            let user_approver_input = $(this).find("[Id^='UserApprover_ApproverId_']");
            let user_approver_selectize = user_approver_input[0].selectize;

            if (user_approver_id != '') {
                userIdArray.push(user_approver_id);
            }
        });

        for (var item of userIdArray) {
            approverSelectize.removeOption(item);
        }
    }

    function addUserCard(item) {
        let userProfileToUse = isNullOrWhitespace(item.ProfilePicture) || !checkFileExist(item.ProfilePicture) ? defaultProfile : item.ProfilePicture;
        let userStatus = "";
        let timePassed = moment(new Date(item.LastOnlineTime)).fromNow();

        let unlockButton = "";

        //if (item.LockStatus != "Unlocked")
        unlockButton = `<a href="javascript:void(0);" class="dropdown-item js-unlock ${item.CompanyId == null ? 'disabled' : ''}" data-id="${item.Id}" data-name="${item.Name}"
            ${item.LockStatus == "Unlocked" ? "hidden" : ""}>Unlock</a>`

        if (convertDate(item.LastOnlineTime) == "") {
            timePassed = "";
        }

        if (item.IsOnline == true && item.IsDisabled == false) {
            userStatus = `<span class="badge bg-success">Online</span>`;
        } else if (item.IsOnline == true && item.IsDisabled == true) {
            userStatus = `<span class="badge fs-6 border bg-success">Online</span>`;
            userStatus += `<span class="badge fs-6 bg-danger">Account disabled</span>`;
        } else {
            userStatus += item.IsDisabled ? `<em class=" text-danger">Account disabled</em>` : `<span class="text-secondary">Offline ${timePassed}</span>`;
        }

        if (item.LockStatus == 'Locked')
            userStatus += `<span class="badge fs-6 text-danger border border-danger">Locked ${item.LockedDuration} minute/s</span>`;

        var userCard = `<div class="col-xl-3 col-md-6">
                            <div class="card text-center shadow">
                                <div class="card-body">
                                    <div class="card-widgets">
                                        <div class="dropdown text-end">
                                            <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                                                <i class="mdi mdi-dots-vertical"></i>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-end">
                                                <!-- item-->
                                                <a href="javascript:void(0);" class="dropdown-item js-edit" data-id="${item.Id}" data-name="${item.Name}">Edit</a>
                                                <!-- item-->
                                                ${unlockButton}
                                            </div>
                                        </div>
                                    </div>

                                    <img src="${userProfileToUse}" class="rounded-circle avatar-lg img-thumbnail bg-transparent" alt="profile-image">

                                    <div class="mt-3">
                                        <h4 class="mb-1">${item.Name}</h4>
                                        <p class="mb-2">${item.Position} | ${userStatus}</p>
                                        <p class="mb-2"> <a href="mailto:${item.Email}" class="text-pink">${item.Email}</a> </p>
                                    </div>
                                    <ul class="social-list list-inline mt-3 mb-0">

                                    </ul>
                                    <div class="row mt-3 g-3">
                                        <div class="col-6">
                                            <a href="javascript:void(0);" class="btn btn-sm btn-light w-100 js-change-pass" data-id="${item.Id}" data-name="${item.UserName}">Change Password</a>
                                        </div>
                                        <div class="col-6">
                                            <a href="javascript:void(0);" class="btn btn-sm btn-light w-100 btn-activity" data-id="${item.Id}" data-name="${item.Name}">View Activity</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>`;

        $("#div-user-cards").append(userCard);
    }

    function openChangePassModal(userId, userName) {
        $("[name='ChangePassword.UserId']").val(userId);
        $("[name='ChangePassword.Username']").val(userName);

        resetChangePassForm();
        changePassModal.modal("show");
    }

    async function loadUsers() {
        const users = await $.ajax({
            url: baseUrl + "User/GetUsersByCompany",
            method: "get",
            dataType: 'json'
        });

        $("#div-user-cards").empty();
        for (let user of users) {
            addUserCard(user);
        }
    }

    async function getUserInfo(userId) {
        const response = await $.ajax({
            url: baseUrl + "User/GetUser/" + userId,
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    async function getUserApprover(userId) {
        const response = await $.ajax({
            url: baseUrl + "User/GetUserApprover?userId=" + userId,
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    async function getUserProjects(userId) {
        const response = await $.ajax({
            url: baseUrl + "User/GetUserProjects/" + userId,
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    function resetChangePassForm() {
        $("#ChangePassword_CurrentPassword").val('');
        $("#ChangePassword_NewPassword").val('');
        $("#ChangePassword_ConfirmPassword").val('');
    }

    //#endregion Methods
});