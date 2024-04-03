﻿"use strict"

const CONST_MODULE = "Applicants Requests";
const CONST_MODULE_CODE = "APLCNTREQ";
const CONST_TRANSACTIONID = $("#ApplicantsPersonalInformationModel_Id").val();
const CONST_APPLICANTCODE = $("#txt_applicantCode").val();

const $btnApprove = $('#btnApprove');
const $btnDisapprove = $('#btnDisapprove');
const $btnReturn = $('#btnReturn');
const $btnCancel = $('#btnCancel');
const $approverModal = $('#approver-modal');
const $approverDiv = $('#div_approval');

$(function () {
    let $btnSave = $('#btn_save');

    //loadApprovalData();
    //loadApproversData();
    loadApprovalData();

    loadVerificationAttachments(CONST_APPLICANTCODE);
    loadApplicationAttachments(CONST_APPLICANTCODE);

    function loadVerificationAttachments(applicantCode) {
        $.ajax({
            url: baseUrl + "Applicants/GetEligibilityVerificationDocuments",
            data: {
                applicantCode: applicantCode
            },
            method: "GET",
            success: function (data) {
                appendVerificationAttachments(data);
            },
            error: function (xhr, status, error) {
                console.error(xhr, status, error);
            }
        });
    }

    function appendVerificationAttachments(items) {
        const groupedItems = {};

        // Group items by DocumentTypeName
        items.forEach(item => {
            const groupId = item.DocumentTypeId;
            const groupName = item.DocumentTypeName;

            if (!groupedItems[groupName]) {
                groupedItems[groupName] = [];
            }
            groupedItems[groupName].push(item);
        });

        // Append grouped items
        for (const groupName in groupedItems) {
            if (groupedItems.hasOwnProperty(groupName)) {
                const groupItems = groupedItems[groupName];
                const firstItem = groupItems[0];
                let groupHtml = `<div class="col-md-4 col-6 mb-2" id="${firstItem.DocumentTypeId}">
                                <h4 class="header-title text-muted">${groupName}</h4>
                                <div class="list-group">`;

                groupItems.forEach(item => {
                    const itemLink = item.DocumentLocation + item.DocumentName;
                    groupHtml += `<a href="${item.DocumentName ? itemLink : 'javascript:void(0)'}" class="list-group-item list-group-item-action" target="${item.DocumentName ? '_blank' : ''}" download="">
                                    <div class="d-flex justify-content-between align-items-center">
                                        <div>
                                            <i class="fe-file-text me-1"></i> ${item.DocumentName ? item.DocumentName : 'Not Uploaded Yet'}
                                        </div>
                                    </div>
                                </a>`;
                });

                groupHtml += `</div></div>`;
                $("#div_verification").append(groupHtml);
            }
        }
    }

    function loadApplicationAttachments(applicantCode) {
        $.ajax({
            url: baseUrl + "Applicants/GetApplicationVerificationDocuments",
            data: {
                applicantCode: applicantCode
            },
            method: "GET",
            success: function (data) {
                appendApplicationAttachments(data);
            },
            error: function (xhr, status, error) {
                console.error(xhr, status, error);
            }
        });
    }

    function appendApplicationAttachments(items) {
        const groupedItems = {};

        // Group items by DocumentTypeName
        items.forEach(item => {
            const groupId = item.DocumentTypeId;
            const groupName = item.DocumentTypeName;

            if (!groupedItems[groupName]) {
                groupedItems[groupName] = [];
            }
            groupedItems[groupName].push(item);
        });

        // Append grouped items
        for (const groupName in groupedItems) {
            if (groupedItems.hasOwnProperty(groupName)) {
                const groupItems = groupedItems[groupName];
                const firstItem = groupItems[0];
                let groupHtml = `<div class="col-md-4 col-6 mb-2" id="${firstItem.DocumentTypeId}">
                                <h4 class="header-title text-muted">${groupName}</h4>
                                <div class="list-group">`;

                groupItems.forEach(item => {
                    const itemLink = item.DocumentLocation + item.DocumentName;
                    groupHtml += `<a href="${item.DocumentName ? itemLink : 'javascript:void(0)'}" class="list-group-item list-group-item-action" target="${item.DocumentName ? '_blank' : ''}" download="">
                                    <div class="d-flex justify-content-between align-items-center">
                                        <div>
                                            <i class="fe-file-text me-1"></i> ${item.DocumentName ? item.DocumentName : 'Not Uploaded Yet'}
                                        </div>
                                    </div>
                                </a>`;
                });

                groupHtml += `</div></div>`;
                $("#div_application").append(groupHtml);
            }
        }
    }

    //#region Approval

    //$("#btn_save").on("click", function () {
    //    rebindValidator();
    //});

    $("#btnSubmitApplication, #btnWithdraw, #btnApprove, #btnDefer").on('click', async function () {
        let action = $(this).attr("data-value");

        await openApprovalModal(action)
    });

    async function openApprovalModal(action) {
        let modalLabel = $("#approver-modalLabel");
        let transactionNo = $(`[name="ApplicantsPersonalInformationModel.Code"]`).val();
        let remarksInput = $('[name="ApprovalLevel.Remarks"]');

        remarksInput.removeAttr("data-val-required").removeClass("input-validation-error").addClass("valid");
        $btnSave.removeClass()

        if (action == 1) {
            modalLabel.html('<span class="fe-send"></span> Submit Application');
            $btnSave.addClass("btn btn-primary").html('<span class="fe-send"></span> Submit')
        } else if (action == 5) {
            modalLabel.html('<span class="mdi mdi-book-cancel-outline"></span> Withdraw Application');
            $btnSave.addClass("btn btn-warning").html('<span class="mdi mdi-book-cancel-outline"></span> Confirm')
        } else if (action == 2) {
            modalLabel.html('<span class="fe-x-circle"></span> Defer Application');
            $btnSave.addClass("btn btn-danger").html('<span class="fe-x-circle"></span> Defer')
        } else {
            modalLabel.html('<span class="fe-check-circle"></span> Approve Application');
            $btnSave.addClass("btn btn-success").html('<span class="fe-check-circle"></span> Approve')
        }

        $("[name='ApprovalLevel.Status']").val(action);
        $("[name='ApprovalLevel.TransactionNo']").val(transactionNo);

        rebindValidator();
        $approverModal.modal("show");
    }
    async function loadApprovalData() {
        const requestorId = $(`[name="${CONST_MODULE}.CreatedById"]`).val();
        const currentUserId = $("#current_userId").val();

        let recordId = $("#ApplicantsPersonalInformationModel_Id").val();
        const approvalData = await getApprovalData(recordId);

        if (!approvalData) { return; }

        $("[name='ApprovalLevel.Id']").val(approvalData.ApprovalLevelId ?? 0);
        $("[name='ApprovalLevel.ApprovalStatusId']").val(approvalData.Id ?? 0);
        $("[name='ApprovalLevel.ModuleCode']").val(CONST_MODULE_CODE);
        $("[name='ApprovalLevel.TransactionId']").val(CONST_TRANSACTIONID);
    }
    async function getApprovalData(referenceId) {
        const response = await $.ajax({
            url: baseUrl + `Approval/GetByReferenceModuleCodeAsync/${referenceId}/${CONST_MODULE_CODE}`,
            method: "get",
            dataType: 'json'
        });

        return response;
    }
    function rebindValidator() {
        let $form = $("#frm_approver_level");

        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate($form.data("unobtrusiveValidation").options);
        $form.data("validator").settings.ignore = "";

        $form.submit(function (e) {
            e.preventDefault();

            if (!$form.valid()) { return; }

            let formData = $form.serialize();
            formData = formData.replace(/ApprovalLevel\./g, "");

            let approvalStatus = $("#ApprovalLevel_Status").val();

            let action = "";
            let text = "";
            let confirmButtonText = "";

            if (approvalStatus == 1) {
                action = "Submit";
                text = "Are you sure you wish to proceed with submitting this application?";
                confirmButtonText = "submit";
            } else if (approvalStatus == 2) {
                action = "Defer";
                text = "Are you sure you wish to proceed with deferring this application?";
                confirmButtonText = "defer";
            } else if (approvalStatus == 3 || approvalStatus == 4) {
                action = "Approve";
                text = "Are you sure you wish to proceed with approving this application?";
                confirmButtonText = "approve";
            } else if (approvalStatus == 5) {
                action = "Withdrawn";
                text = "Are you sure you wish to proceed with withdrawing this application?";
                confirmButtonText = "withdrawn";
            }


            // Use SweetAlert for confirmation
            Swal.fire({
                title: `${action} Application`,
                text: text,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: `Yes, ${confirmButtonText} it`,
                cancelButtonText: 'No, cancel'
            }).then((result) => {
                if (result.isConfirmed) {
                    // User confirmed, proceed with form submission
                    $.ajax({
                        url: $form.attr("action"),
                        method: $form.attr("method"),
                        data: formData,
                        beforeSend: function () {
                            $btnSave.attr({ disabled: true });
                        },
                        success: function (response) {
                            messageBox("Successfully saved.", "success");

                            $btnSave.attr({ disabled: false });

                            $approverModal.modal("hide");

                            location.reload();
                        },
                        error: function (response) {
                            // Error message handling
                            $btnSave.attr({ disabled: false });

                            messageBox(error.responseText, "danger", false, false);
                        }
                    });
                }
            });
        });
    }

    function resetForm() {
        $("[name='ApprovalLevel.Remarks']").val("");

        $btnSave.removeClass();
    }

    //#endregion
});