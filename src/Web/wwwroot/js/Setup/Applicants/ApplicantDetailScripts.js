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
    $('#btnApprove, #btnDefer, #btnReturn, #btnCancel').click(async function () {
        let id = $(this).attr("id");
        let statusType = id.replace("btn", "");

        await openModal(statusType);
    })

    $("#btn_save").on("click", function () {
        let action = $(this).attr('data-action');

        rebindValidator();
    });

    async function openModal(statusType) {
        let modalLabel = $("#approver-modalLabel");
        /*        let recordId = $(`[name="${CONST_MODULE}.Id"]`).val();*/
        //applicationCode
        let transactionNo = $(`[name="ApplicantsPersonalInformationModel.Code"]`).val();

        let status;
        let remarksInput = $('[name="ApprovalLevel.Remarks"]');
        remarksInput.removeAttr("data-val-required").removeClass("input-validation-error").addClass("valid");

        resetForm();

        if (statusType == "Approve") {
            modalLabel.html('<span class="fe-thumbs-up"></span> Approve Transaction');
            $btnSave.addClass("btn btn-primary").html('<span class="fe-thumbs-up"></span> Approve')
            $btnSave.attr('data-action', 'Approve');

            status = 2;
        } else if (statusType == "Defer") {
            modalLabel.html('<span class="fe-thumbs-down"></span> Defer Transaction');
            $btnSave.addClass("btn btn-danger").html('<span class="fe-thumbs-down"></span> Defer')
            $btnSave.attr('data-action', 'Defer');

            status = 3;
            remarksInput.attr({ "data-val-required": "Remarks is required.", "data-val": true });
        } else if (statusType == "Return") {
            modalLabel.html('<span class="fe-corner-down-left"></span> Return Transaction');
            $btnSave.addClass("btn btn-warning").html('<span class="fe-corner-down-left"></span> Return')
            $btnSave.attr('data-action', 'Return');

            status = 4;
            remarksInput.attr({ "data-val-required": "Remarks is required.", "data-val": true });
        } else if (statusType == "Cancel") {
            modalLabel.html('<span class="fe-slash"></span> Cancel Transaction');
            $btnSave.addClass("btn btn-danger").html('<span class="fe-slash"></span> Cancel')
            $btnSave.attr('data-action', 'Cancel');

            status = 6;
            remarksInput.attr({ "data-val-required": "Remarks is required.", "data-val": true });
        }

        //$("[name='ApprovalLevel.Id']").val(id)
        //$("[name='ApprovalLevel.ApprovalStatusId]'").val(approvalStatusID);
        $("[name='ApprovalLevel.Status']").val(status);
        $("[name='ApprovalLevel.TransactionNo']").val(transactionNo);

        // rebindValidator();
        $approverModal.modal("show");
    }

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
        let rowstoAppend = `

                                                <div class="col-md-4 col-6 mb-2"  id="${item.DocumentTypeId}">
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
                                                </div>

        `;

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
        let rowstoAppend = `

                                                <div class="col-md-4 col-6 mb-2"  id="${item.DocumentTypeId}">
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
                                                </div>

        `;

        $("#div_verification").append(rowstoAppend);
    }

    function rebindValidator() {
        let $form = $("#frm_approver_level");

        let action = $("#btn_save").attr('data-action');

        var actionHead = "";

        var titleText = "";

        var confirmbuttonText = "";

        if (action == "Approve") {
            actionHead = 'Approve';
            titleText = 'Are you sure you wish to proceed with approving this application? This will move the application to the next step';
            confirmbuttonText = '   Yes, approve it';
        }
        else {
            actionHead = 'Defer';
            titleText = 'Are you sure you wish to defer this application? This will postpone this application, requiring a new submission from the beneficiary';
            confirmbuttonText = 'Yes, defer it';
        }

        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate($form.data("unobtrusiveValidation").options);
        $form.data("validator").settings.ignore = "";

        $form.submit(function (e) {
            e.preventDefault();
            console.log('form submitted');
            if (!$form.valid()) { return; }

            Swal.fire({
                title: `${actionHead} Application?`,
                text: `${titleText}`,
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: `${confirmbuttonText}`,
                cancelButtonText: "No, I'll recheck it",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    // Using fetch to submit form data
                    return fetch($form.attr("action"), {
                        method: $form.attr("method"),
                        body: new FormData($form[0]),
                    })
                        .then(response => {
                            if (!response.ok) {
                                throw new Error(response.statusText);
                            }
                            return response.ok;
                        })
                        .then(data => {
                            // Handle the response data here if needed
                            messageBox("Successfully saved.", "success");
                            $approverModal.modal("hide");
                            location.reload();
                            return data; // Return data to be used by Swal
                        })
                        .catch(error => {
                            messageBox(error.message, "danger", false, false);
                            throw error; // Throw error to show Swal validation message
                        });
                },
                allowOutsideClick: () => !Swal.isLoading()
            }).then((result) => {
                if (result.isConfirmed) {
                    // Handle success if needed
                }
            });
        });
    }

    function resetForm() {
        $("[name='ApprovalLevel.Remarks']").val("");

        $btnSave.removeClass();
    }

    async function loadApprovalData() {
        const requestorId = $(`[name="${CONST_MODULE}.CreatedById"]`).val();
        const currentUserId = $("#current_userId").val();

        let recordId = $("#ApplicantsPersonalInformationModel_Id").val();
        const approvalData = await getApprovalData(recordId);

        //$btnApprove.attr({ hidden: true });
        //$btnDisapprove.attr({ hidden: true });
        //$btnReturn.attr({ hidden: true });
        //$btnCancel.attr({ hidden: true });

        if (!approvalData) { return; }

        console.log(approvalData)

        $("[name='ApprovalLevel.Id']").val(approvalData.ApprovalLevelId ?? 0);
        $("[name='ApprovalLevel.ApprovalStatusId']").val(approvalData.Id ?? 0);
        $("[name='ApprovalLevel.ModuleCode']").val(CONST_MODULE_CODE);
        $("[name='ApprovalLevel.TransactionId']").val(CONST_TRANSACTIONID);

        //let statusArray = [1, 4]
        //let isVisibleApprovalButton = (currentUserId == approvalData.CurrentApproverUserId && statusArray.includes(approvalData.Status));

        //$btnApprove.attr({ hidden: !isVisibleApprovalButton });
        //$btnDisapprove.attr({ hidden: !isVisibleApprovalButton });
        //$btnReturn.attr({ hidden: !(isVisibleApprovalButton && approvalData.Status != 4 && approvalData.ModuleDescription != "Canvass Sheet") });
        //$btnCancel.attr({ hidden: !(currentUserId == requestorId && statusArray.includes(approvalData.Status)) });
    }
    async function getApprovalData(referenceId) {
        const response = await $.ajax({
            url: baseUrl + `Approval/GetByReferenceModuleCodeAsync/${referenceId}/${CONST_MODULE_CODE}`,

            method: "get",
            dataType: 'json'
        });

        return response;
    }
});