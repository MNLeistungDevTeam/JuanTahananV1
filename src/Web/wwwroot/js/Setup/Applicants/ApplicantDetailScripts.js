"use strict"

const CONST_MODULE = "Applicants Requests";
const CONST_MODULE_CODE = "APLCNTREQ";
const CONST_TRANSACTIONID = $("#ApplicantsPersonalInformationModel_Id").val();
const CONST_TRANSACTIONCODE = $("#ApplicantsPersonalInformationModel_Code").val();

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

    loadVerificationAttachments(CONST_TRANSACTIONCODE);

    function loadVerificationAttachments(applicantCode) {
        $.ajax({
            url: baseUrl + "Applicants/GetEligibilityVerificationDocuments/" + applicantCode,
            method: "Get",
            success: function (data) {
                for (let item of data) {
                    appendVerificationAttachments(item);
                }
            }
        });
    }

    function appendVerificationAttachments(item) {
        let itemLink = item.DocumentLocation + item.DocumentName;
        let rowstoAppend = `<div class="col-md-4 col-6 mb-2"  id="${item.DocumentTypeId}">
                                <h4 class="header-title text-muted">${item.DocumentTypeName}</h4>

                                <div class="list-group">
                                    <a href="${item.DocumentName == null ? 'javascript:void(0)' : itemLink}" class="list-group-item list-group-item-action" target="${item.DocumentName == null ? '' : '_blank'}" download="">
                                        <div class="d-flex justify-content-between align-items-center">
                                            <div>
                                                <i class="fe-file-text me-1"></i>  ${item.DocumentName == null ? 'Not Upload Yet' : item.DocumentName}
                                            </div>
                                        </div>
                                    </a>
                                </div>
                            </div>`;

        $("#div_verification").append(rowstoAppend);
    }

    function loadApplicationAttachments(applicantCode) {
        $.ajax({
            url: baseUrl + "Applicants/GetEligibilityVerificationDocuments/" + applicantCode,
            method: "Get",
            success: function (data) {
                for (let item of data) {
                    appendVerificationAttachments(item);
                }
            }
        });
    }

    function appendApplicationAttachments(item) {
        let itemLink = item.DocumentLocation + item.DocumentName;
        let rowstoAppend = `<div class="col-md-4 col-6 mb-2"  id="${item.DocumentTypeId}">
                                <h4 class="header-title text-muted">${item.DocumentTypeName}</h4>

                                <div class="list-group">
                                    <a href="${item.DocumentName == null ? 'javascript:void(0)' : itemLink}" class="list-group-item list-group-item-action" target="${item.DocumentName == null ? '' : '_blank'}" download="">
                                        <div class="d-flex justify-content-between align-items-center">
                                            <div>
                                                <i class="fe-file-text me-1"></i>  ${item.DocumentName == null ? 'Not Upload Yet' : item.DocumentName}
                                            </div>
                                        </div>
                                    </a>
                                </div>
                            </div>`;

        $("#div_verification").append(rowstoAppend);
    }

    //#region Approval
    

    //$("#btn_save").on("click", function () {
    //    rebindValidator();
    //});

    $("#btnSubmitApplication, #btnWithdraw, #btnApprove, #btnDefer").on('click', async  function () {
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

            $.ajax({
                url: $form.attr("action"),
                method: $form.attr("method"),
                data: formData,
                beforeSend: function () {
                    $btnSave.attr({ disabled: true });
                    //$overlay.attr({ hidden: false });
                },
                success: function (response) {
                    messageBox("Successfully saved.", "success");

                    $btnSave.attr({ disabled: false });
                    //$overlay.attr({ hidden: true });

                    $approverModal.modal("hide");

                    location.reload();
                },
                error: function (error) {
                    //$overlay.attr({ hidden: true });
                    $btnSave.attr({ disabled: false });

                    messageBox(error.responseText, "danger", false, false);
                }
            })
        })
    }
    function resetForm() {
        $("[name='ApprovalLevel.Remarks']").val("");

        $btnSave.removeClass();
    }

    //#endregion
});