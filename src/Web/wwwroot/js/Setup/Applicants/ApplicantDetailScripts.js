"use strict"

const CONST_MODULE = "Applicants Requests";
const CONST_MODULE_CODE = "APLCNTREQ";

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
    $('#btnApprove, #btnDefer, #btnReturn, #btnCancel').click(async function () {
        let id = $(this).attr("id");
        let statusType = id.replace("btn", "");

        await openModal(statusType);
    })

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

    $("#btn_save").on("click", function () {
        let action = $(this).attr('data-action');

        rebindValidator();

        //if (action == 'Approve') {
        //    Swal.fire({
        //        title: `Approve Application?`,
        //        text: `Are you sure you wish to proceed with approving this application? This will move the application to the next step`,
        //        icon: 'question',
        //        showCancelButton: true,
        //        confirmButtonColor: '#3085d6',
        //        cancelButtonColor: '#d33',
        //        confirmButtonText: 'Yes, approve it',
        //        cancelButtonText: "No, I'll recheck it",
        //        showLoaderOnConfirm: true,
        //        preConfirm: () => {
        //            $("#btn_save").submit();
        //            rebindValidator();
        //        },
        //        allowOutsideClick: () => !Swal.isLoading()
        //    }).then((result) => {
        //        if (result.isConfirmed) {
        //            messageBox("Approve Successfully.", "success");
        //        }
        //    })
        //}
        //else {
        //    Swal.fire({
        //        title: 'Defer Application?',
        //        text: `Are you sure you wish to defer this application? This will postpone this application, requiring a new submission from the beneficiary`,
        //        icon: 'question',
        //        showCancelButton: true,
        //        confirmButtonColor: '#3085d6',
        //        cancelButtonColor: '#d33',
        //        confirmButtonText: 'Yes, defer it',
        //        cancelButtonText: "No, I'll recheck it",
        //        showLoaderOnConfirm: true,
        //        preConfirm: () => {
        //            $("#btn_save").submit();
        //            rebindValidator();
        //        },
        //        allowOutsideClick: () => !Swal.isLoading()
        //    }).then((result) => {
        //        if (result.isConfirmed) {
        //            messageBox("Approve Successfully.", "success");
        //        }
        //    })
        //}
    });

    function rebindValidator() {
        let $form = $("#frm_approver_level");

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
                title: `Approve Application?`,
                text: `Are you sure you wish to proceed with approving this application? This will move the application to the next step`,
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, approve it',
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
                            return response.json();
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
        //$("[name='ApprovalLevel.Status']").val(0);
        //$("[name='ApprovalLevel.TransactionNo']").val("");
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