"use strict"

const userId = $("[name='User.Id']").val();
const frmUser = $("#frm_user");
const frmChangePass = $("#frm_change_pass");
const defaultProfile = $("#Layout-defaultProfile").html();
const defaultSignature = $("#Layout-defaultSignature").html();
var $userRoleDropdown, userRoleDropdown;

$(function () {
    $("#User_Gender").selectize();

    $('#btnRefreshTransaction').on('click', function () {
        tbltransaction.column(7).search('').draw();
        tbltransaction.ajax.reload();
    });
    $userRoleDropdown = $("[name='User.UserRoleId']").selectize({
        valueField: 'Id',
        labelField: 'UserName',
        searchField: ['UserName', 'Name'],
        selectOnTab: true,
        preload: true,
        persist: false,
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

    userRoleDropdown.on("load", function () {
        loadUserInfo();
        userRoleDropdown.off("load");
    });

    $("#User_ProfilePictureFile").change(function () {
        let container = $("#imagePreview");
        readUrl(this, container);
    });
    $("#User_SignatureFile").change(function () {
        let container = $("#imagePreview2");
        readUrl(this, container);
    });

    $("#btn_save_user").click(function () {
        $("#frm_user").submit();
    });
    $('#btnRefreshBudget').on('click', function () {
        tbl_budget.ajax.reload();
    });
    frmChangePass.on("submit", function (e) {
        e.preventDefault();

        var formData = $(this)
            .serialize()
            .replaceAll("ChangePassword.", "");
        let button = $("#btn_change_pass");

        if ($(this).valid() == false) {
            messageBox("Please fill out all required fields!", "danger", true);
            return;
        }

        $.ajax({
            url: frmChangePass.attr("action"),
            method: frmChangePass.attr("method"),
            data: formData,
            success: function (response) {
                messageBox("Change Password successfull", "success", true);
                resetChangePasswordForm();
                button.attr({ disabled: false });
                button.html("<span class='mdi mdi-content-save-outline'></span> Save");
            },
            error: function (response) {
                messageBox(response.responseText, "danger");
                button.html("<span class='mdi mdi-content-save-outline'></span> Save");
                button.attr({ disabled: false });
            }
        });
    });

    frmUser.on("submit", function (e) {
        e.preventDefault();
        let formData = new FormData(e.target);
        let button = $("#btn_save_user");

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
                let successMessage = `User Successfully Updated`;

                messageBox(successMessage, "success", true);

                loadUserInfo();
                button.attr({ disabled: false });
                button.html("<span class='mdi mdi-content-save-outline'></span> Save");
            },
            error: function (response) {
                messageBox(response.responseText, "danger");
                button.html("<span class='mdi mdi-content-save-outline'></span> Save");
                button.attr({ disabled: false });
            }
        });
    });
});

function resetUserForm() {
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

    loadImageOnPreview(defaultProfile, defaultProfile, '#imagePreview');
    loadImageOnPreview(defaultSignature, defaultSignature, '#imagePreview2');
}

function resetChangePasswordForm() {
    $("input[name='ChangePassword.CurrentPassword']").val("");
    $("input[name='ChangePassword.NewPassword']").val("");
    $("input[name='ChangePassword.ConfirmPassword']").val("");
}

function loadUserInfo() {
    let actualPicture = "", actualSignature = "";

    $.ajax({
        url: baseUrl + "User/GetUser/" + userId,
        beforeSend: function () {
            resetUserForm();
        },
        success: function (response) {
            console.log(response);
            let UserPicture = response.ProfilePicture;
            let UserSignature = response.Signature;

            $("input[name='ChangePassword.UserId']").val(response.Id);
            $("input[name='ChangePassword.Username']").val(response.UserName);
            $("input[name='User.Id']").val(response.Id);
            $("input[name='User.Prefix']").val(response.Prefix);
            $("input[name='User.LastName']").val(response.LastName);
            $("input[name='User.FirstName']").val(response.FirstName);
            $("input[name='User.MiddleName']").val(response.MiddleName);
            $("input[name='User.Suffix']").val(response.Suffix);
            $("input[name='User.UserName']").val(response.UserName);

            $("input[name='User.Password']").val(response.Password);
            $("input[name='User.Email']").val(response.Email);
            $("input[name='User.Position']").val(response.Position);
            $("select[name='User.Gender']").data('selectize').setValue(response.Gender);

            $("[name='User.IsDisabled']").prop("checked", response.IsDisabled);

            userRoleDropdown.unlock();
            userRoleDropdown.setValue(response.UserRoleId);
            userRoleDropdown.lock();

            if (UserPicture === "" || UserPicture === null) actualPicture = defaultProfile;
            else actualPicture = UserPicture;

            if (UserSignature === "" || UserSignature === null) actualSignature = defaultSignature;
            else actualSignature = UserSignature;

            loadImageOnPreview(actualPicture, defaultProfile, '#imagePreview');
            loadImageOnPreview(actualSignature, defaultSignature, '#imagePreview2');
        }
    });
}