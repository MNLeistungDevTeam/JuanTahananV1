"use strict"

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

let approvalStatus = $('[name="ApplicantsPersonalInformationModel.ApprovalStatus"]').val();
let stageNo = $('#txt_stageNo').val();

let ApplicationId = $('#applicationId').val();
let DocumentTypeId = 0;

const FileFormats = {
    1: ['.pdf'],
    2: ['.docx'],
    3: ['.txt'],
    4: ['.xlsx'],
    5: ['image/*'], // Mapping both .jpg and .jpeg to the same value
};

$(async function () {
    let $btnSave = $('#btn_save');
    //loadApprovalData();
    //loadApproversData();
    loadApprovalData();

    await loadVerificationAttachments(CONST_APPLICANTCODE);
    await loadApplicationAttachments(CONST_APPLICANTCODE);

    $(document).ready(function () {
        // Add click event listener to the tab

        if (stageNo == 1) {
            //$('a[href="#tab4"]').on('click', function (event) {
            //    event.preventDefault(); // Prevent default click behavior
            //    messageBox('Navigation to this tab is disabled at this stage!', 'danger');
            //});

            // Optionally, you can also add a disabled look to the tab to indicate visually that it's not clickable
            $('a[href="#tab4"]').addClass('disabled'); // Add a disabled class to visual
        }
    });

    //#region Events
    $(document).on('click', '.upload-link', async function () {
        DocumentTypeId = $(this).data("document-type-id");
        const fileInput = $(`#fileInput_${DocumentTypeId}`);
        var documentType = await GetDocumentType(DocumentTypeId);

        let fileFormats = FileFormats[documentType.FileType];
        if (fileFormats === undefined || !Array.isArray(fileFormats)) {
            fileInput.prop('accept', '*/*');
        } else {
            let formated = fileFormats.join(',');
            fileInput.prop('accept', formated);
        }

        fileInput.trigger('click');
    });

    // Handling file input change event
    $(document).on('change', 'input[type=file]', function () {
        const file = this.files[0];
        const documentTypeId = $(this).attr("id").split("_")[1];
        if (file) {
            upload(file, documentTypeId, 0);
        }
    });

    $("#btnSubmitApplication, #btnWithdraw, #btnApprove, #btnDefer").on('click', async function (e) {
        e.preventDefault();
        let action = $(this).attr("data-value");

        openApprovalModal(action);
    });

    //#endregion Events

    //#region Function

    async function loadVerificationAttachments(applicantCode) {
        let verifAttach = await getVerificationDocuments(applicantCode);

        if (!verifAttach) { return; }

        //Append Verification Attachments
        const groupedItems = {};

        $("#div_verification").empty();

        // Group items by DocumentTypeName
        verifAttach.forEach(item => {
            const groupId = item.DocumentTypeId;
            const groupName = item.DocumentTypeName;

            if (!groupedItems[groupName]) {
                groupedItems[groupName] = [];
            }
            groupedItems[groupName].push(item);
        });

        // Append grouped items without subdocument or parent items
        for (const groupName in groupedItems) {
            if (groupedItems.hasOwnProperty(groupName)) {
                const groupItems = groupedItems[groupName];
                const firstItem = groupItems[0];

                let groupHtml = ``;

                groupItems.forEach((item, index) => {
                    const itemLink = item.DocumentLocation + item.DocumentName;
                    const uploadLinkClass = !item.DocumentName ? 'upload-link' : ''; // Add upload-link class conditionally
                    const isDisabled = !item.DocumentName ? 'disabled' : ''; // Add disabled attribute conditionally
                    const documentNumber = item.DocumentSequence ? `(${item.DocumentSequence})` : ''; // Append document number

                    if (item.HasParentId === 0 && item.HasSubdocument === 0) {
                        groupHtml += `<div class="col-md-4 mb-2"" id="${firstItem.DocumentTypeId}">
                        <h4 class="header-title text-muted">${groupName}</h4>
                        <div class="list-group">`;

                        groupHtml += `<div class="file-upload-wrapper">
                            <input type="file" id="fileInput_${item.DocumentTypeId}" style="display:none">
                            <a href="${item.DocumentName ? itemLink : 'javascript:void(0)'}" class="list-group-item list-group-item-action ${uploadLinkClass}" target="${item.DocumentName ? '_blank' : ''}" ${isDisabled} data-document-type-id="${item.DocumentTypeId}">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div>
                                        <i class="fe-file-text me-1"></i> ${item.DocumentName ? item.DocumentName + ' ' + documentNumber : 'Not Uploaded Yet'}
                                    </div>
                                </div>
                            </a>
                          </div>`;
                    }
                });

                groupHtml += `</div></div>`;
                $("#div_verification").append(groupHtml);
            }
        }

        // Append grouped items with parentId and subdocument
        for (const groupName in groupedItems) {
            if (groupedItems.hasOwnProperty(groupName)) {
                const groupItems = groupedItems[groupName];
                const firstItem = groupItems[0];

                let groupHtml = ``;

                groupItems.forEach((item, index) => {
                    const itemLink = item.DocumentLocation + item.DocumentName;
                    const uploadLinkClass = !item.DocumentName ? 'upload-link' : ''; // Add upload-link class conditionally
                    const isDisabled = !item.DocumentName ? 'disabled' : ''; // Add disabled attribute conditionally
                    const documentNumber = item.DocumentSequence ? `(${item.DocumentSequence})` : ''; // Append document number

                    if (item.HasSubdocument === 1) {
                        groupHtml += `<div class="col-md-12 mb-2" id="${firstItem.DocumentTypeId}">
                            <div class="nav-tabs nav-bordered">
                                <h4 class="header-title text-muted">${groupName}</h4>
                            </div>
                        <div>`;

                        groupItems.splice(index, 1);  //Removed the object
                    } else if (item.HasParentId === 1) {
                        groupHtml += `<div class="col-md-4 col-6 mb-2" id="${firstItem.DocumentTypeId}">
                        <h4 class="header-title text-muted ">${groupName}</h4>
                        <div class="list-group">`;

                        groupHtml += `<div class="file-upload-wrapper">
                            <input type="file" id="fileInput_${item.DocumentTypeId}" style="display:none">
                            <a href="${item.DocumentName ? itemLink : 'javascript:void(0)'}" class="list-group-item list-group-item-action ${uploadLinkClass}" target="${item.DocumentName ? '_blank' : ''}" ${isDisabled} data-document-type-id="${item.DocumentTypeId}">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div>
                                        <i class="fe-file-text me-1"></i> ${item.DocumentName ? item.DocumentName + ' ' + documentNumber : 'Not Uploaded Yet'}
                                    </div>
                                </div>
                            </a>
                          </div>`;
                    }
                });

                groupHtml += `</div></div>`;
                $("#div_verification").append(groupHtml);
            }
        }

        // Count all the uploaded files
        if (approvalStatus === '0') //Draft
        {
            var flag = allItemsHaveFiles(groupedItems);
            $("#btnSubmitApplication").prop('disabled', !(flag));
        }
    }

    async function loadApplicationAttachments(applicantCode) {
        let appAttach = await getApplicationDocuments(applicantCode);

        if (!appAttach) { return; }

        //Append Application Attachments
        const groupedItems = {};

        $("#div_application").empty();

        // Group items by DocumentTypeName
        await appAttach.forEach(item => {
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

                groupItems.forEach((item, index) => {
                    const itemLink = item.DocumentLocation + item.DocumentName;
                    const uploadLinkClass = !item.DocumentName ? 'upload-link' : ''; // Add upload-link class conditionally
                    const isDisabled = !item.DocumentName ? 'disabled' : ''; // Add disabled attribute conditionally
                    const documentNumber = item.DocumentSequence ? `(${item.DocumentSequence})` : ''; // Append document number

                    groupHtml += `<div class="file-upload-wrapper">
                            <input type="file" id="fileInput_${item.DocumentTypeId}" style="display:none">
                            <a href="${item.DocumentName ? itemLink : 'javascript:void(0)'}" class="list-group-item list-group-item-action ${uploadLinkClass}" target="${item.DocumentName ? '_blank' : ''}" ${isDisabled} data-document-type-id="${item.DocumentTypeId}">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div>
                                        <i class="fe-file-text me-1"></i> ${item.DocumentName ? item.DocumentName + ' ' + documentNumber : 'Not Uploaded Yet'}
                                    </div>
                                </div>
                            </a>
                          </div>`;
                });

                groupHtml += `</div></div>`;
                $("#div_application").append(groupHtml);
            }
        }

        // Count all the uploaded files
        if (approvalStatus === '4') //Pagibig Verified
        {
            var flag = allItemsHaveFiles(groupedItems);
            $("#btnSubmitApplication").prop('disabled', !(flag));
        }
    }

    //#region Approval

    function openApprovalModal(action) {
        let modalLabel = $("#approver-modalLabel");
        let transactionNo = $(`[name="ApplicantsPersonalInformationModel.Code"]`).val();
        let remarksInput = $('[name="ApprovalLevel.Remarks"]');
        let roleName = $("#txt_role_code").val();

        $btnSave.removeClass();
        $('.text-danger.validation-summary-errors').removeClass('validation-summary-errors').addClass('validation-summary-valid')
            .find('li').css('display', 'none');
        remarksInput.removeAttr("data-val-required").removeClass("input-validation-error").addClass("valid");

        if (action == 1) {      //submitted
            modalLabel.html('<span class="fe-send"></span> Submit Application');
            $btnSave.addClass("btn btn-primary").html('<span class="fe-send"></span> Submit')

            remarksInput.removeAttr("data-val-required").removeClass("input-validation-error").addClass("valid");
        }

        else if (action == 6) {   //post-submitted
            modalLabel.html('<span class="fe-send"></span> Submit Application');
            $btnSave.addClass("btn btn-primary").html('<span class="fe-send"></span> Submit')

            remarksInput.removeAttr("data-val-required").removeClass("input-validation-error").addClass("valid");
        }

        else if (action == 2) {        // deferred
            modalLabel.html('<span class="fe-x-circle"></span> Defer Application');
            $btnSave.addClass("btn btn-danger").html('<span class="fe-x-circle"></span> Defer')

            //remarksInput.attr("data-val-required", "true").attr("required", true).addClass("input-validation-error").addClass("invalid");
            remarksInput.attr("required", true);
        }

        else if (action == 5) {       // withdraw
            modalLabel.html('<span class="mdi mdi-book-cancel-outline"></span> Withdraw Application');
            $btnSave.addClass("btn btn-warning").html('<span class="mdi mdi-book-cancel-outline"></span> Confirm')

            remarksInput.removeAttr("data-val-required").removeClass("input-validation-error").addClass("valid");
        }

        else if (action == 10) {       // discontinued
            modalLabel.html('<span class="mdi mdi-book-cancel-outline"></span> Withdraw Application');
            $btnSave.addClass("btn btn-warning").html('<span class="mdi mdi-book-cancel-outline"></span> Confirm')

            remarksInput.removeAttr("data-val-required").removeClass("input-validation-error").addClass("valid");
        }

        else if (action == 9) {        // disapproved
            modalLabel.html('<span class="fe-x-circle"></span> Defer Application');
            $btnSave.addClass("btn btn-danger").html('<span class="fe-x-circle"></span> Defer')
            /*remarksInput.attr("data-val-required", "true").attr("required", true).addClass("input-validation-error").addClass("invalid");*/
            remarksInput.attr("required", true);
        }
        else if (action == 11) {
            modalLabel.html('<span class="fe-repeat"></span> Return for Revision');
            $btnSave.addClass("btn btn-warning").html('<span class="fe-repeat"></span> Return for Revision')
            //remarksInput.attr("data-val-required", "true").attr("required", true).addClass("input-validation-error").addClass("invalid");
            remarksInput.attr("required", true);
        }
        else {
            modalLabel.html('<span class="fe-check-circle"></span> Approve Application');
            $btnSave.addClass("btn btn-success").html('<span class="fe-check-circle"></span> Approve')
            remarksInput.removeAttr("data-val-required").attr("required", false).removeClass("input-validation-error").addClass("valid");
        }

        $("#author_txt").html(`Author: ${roleName}`);
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

            let approvalLevelStatus = $("#ApprovalLevel_Status").val();

            let action = "";
            let text = "";
            let confirmButtonText = "";

            if (approvalLevelStatus == 1) {
                action = "Submit";
                text = "Are you sure you wish to proceed with submitting this application?";
                confirmButtonText = "submit";
            } else if (approvalLevelStatus == 2) {
                action = "Defer";
                text = "Are you sure you wish to proceed with deferring this application?";
                confirmButtonText = "defer";
            } else if (approvalLevelStatus == 3 || approvalLevelStatus == 4) {
                action = "Approve";
                text = "Are you sure you wish to proceed with approving this application?";
                confirmButtonText = "approve";
            } else if (approvalLevelStatus == 5) {
                action = "Withdrawn";
                text = "Are you sure you wish to proceed with withdrawing this application?";
                confirmButtonText = "withdrawn";
            }

            else if (approvalLevelStatus == 6) {
                action = "Submit";
                text = "Are you sure you wish to proceed with submitting this application?";
                confirmButtonText = "submit";
            } else if (approvalLevelStatus == 9) {
                action = "Defer";
                text = "Are you sure you wish to proceed with deferring this application?";
                confirmButtonText = "defer";
            } else if (approvalLevelStatus == 7 || approvalLevelStatus == 8) {
                action = "Approve";
                text = "Are you sure you wish to proceed with approving this application?";
                confirmButtonText = "approve";
            } else if (approvalLevelStatus == 10) {
                action = "Withdrawn";
                text = "Are you sure you wish to proceed with withdrawing this application?";
                confirmButtonText = "withdrawn";
            }

            else if (approvalLevelStatus == 11) {
                action = "Resubmission";
                text = "Are you sure you wish to proceed with for-resubmission this application?";
                confirmButtonText = " for-resubmission";
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
                            $("#applicant-overlay").removeClass('d-none');
                            $btnSave.attr({ disabled: true });
                        },
                        success: function (response) {
                            $("#applicant-overlay").addClass('d-none');

                            messageBox("Successfully saved.", "success");

                            $btnSave.attr({ disabled: false });

                            $approverModal.modal("hide");

                            location.reload();

                            $("#btnSubmitApplication").prop('disabled', false);
                        },
                        error: function (response) {
                            // Error message handling
                            $btnSave.attr({ disabled: false });

                            messageBox(response.responseText, "danger", true);
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

    function upload(file, DocumentTypeId, DocumentId) {
        var formData = new FormData();
        formData.append('file', file);
        formData.append('ApplicationId', ApplicationId);
        formData.append('DocumentTypeId', DocumentTypeId);
        formData.append('DocumentId', DocumentId);

        $.ajax({
            url: '/Document/DocumentUploadFile',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () {
                loading('Uploading...', true);
            },
            success: async function (response) {
                messageBox('Uploaded Successfully', "success", true);
                //method removing iformfile
                var myfile = $('input[type=file]')[0];
                myfile.files[0];

                // remove filename
                $('input[type=file]').val('');

                loader.close();

                //Reload the tab
                await loadVerificationAttachments(CONST_APPLICANTCODE);
                await loadApplicationAttachments(CONST_APPLICANTCODE);
            },
            error: function (xhr, status, error) {
                messageBox(xhr.responseText, "danger", true);
                loader.close();
            }
        });
    }

    //Count for DocumentFile
    function allItemsHaveFiles(groupedItems) {
        for (const groupName in groupedItems) {
            if (groupedItems.hasOwnProperty(groupName)) {
                const groupItems = groupedItems[groupName];
                for (const item of groupItems) {
                    console.log(item.DocumentName);
                    if (!item.DocumentName) {
                        return false; // Return false if any item does not have a file attached
                    }
                }
            }
        }
        return true; // Return true if all items have files attached
    }

    //#endregion Function

    //#region Getters Function

    async function GetDocumentType(documentTypeId) {
        const response = $.ajax({
            url: baseUrl + "Document/GetDocumentTypeById/" + documentTypeId,
            method: "get",
            data: 'json'
        });

        return response;
    }

    async function getApprovalData(referenceId) {
        const response = await $.ajax({
            url: baseUrl + `Approval/GetByReferenceModuleCodeAsync/${referenceId}/${CONST_MODULE_CODE}`,
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    async function getVerificationDocuments(applicantCode) {
        const response = $.ajax({
            url: baseUrl + "Applicants/GetEligibilityVerificationDocuments",
            data: {
                applicantCode: applicantCode
            },
            method: 'get',
            dataType: 'json'
        });

        return response;
    }
    async function getApplicationDocuments(applicantCode) {
        const response = $.ajax({
            url: baseUrl + "Applicants/GetApplicationVerificationDocuments",
            data: {
                applicantCode: applicantCode
            },
            method: 'get',
            dataType: 'json'
        });

        return response;
    }

    //#endregion Getters Function
});