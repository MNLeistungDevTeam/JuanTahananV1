"use strict"

$(function () {
    const { pdfjsLib } = globalThis;
    const debounceError = debounce((callback) => { callback(); }, 5000);

    const CONST_MODULE = "BuyerConfirmation  Requests";
    const CONST_MODULE_CODE = "BCF-APLRQST";
    const CONST_TRANSACTIONID = $("#BuyerConfirmation_Id").val();

    var currentStep = 0;
    var inputLock = false;

    //document.ready
    $(function () {
        initializeLeftDecimalInputMask(".decimalInputMask", 2);
        initializePdfJs();
        updateProgressBar();

        initializeInputMasks();
        initializeCustomValidators();

        loadApprovalData();

        //#region Initialization

        // Add blue border on focus
        $(document).on("focus", "#BuyerConfirmationModel_SellingPrice", function () {
            $(this).addClass("focused");
        });

        // Remove blue border when focus is lost
        $(document).on("blur", "#BuyerConfirmationModel_SellingPrice", function () {
            $(this).removeClass("focused");
        });

        //#endregion
    });



    //#region Events
    $(`[id="bcfSetAmount"]`).on('click', function (e) {
        // lock two inputs to readonly
        e.preventDefault();

        inputLock = !inputLock;
        $(`[name="BuyerConfirmationModel.SellingPrice"], [name="BuyerConfirmationModel.MonthlyAmortization"]`).attr('readonly', inputLock);

        $(this).html(inputLock ? "Change pricing" : "Apply pricing");

        $(this).addClass(inputLock ? "btn-outline-info" : "btn-info");
        $(this).removeClass(!inputLock ? "btn-outline-info" : "btn-info");

        $(`[id="bcfPreviewBtn"]`).addClass(!inputLock ? "btn-outline-info" : "btn-info");
        $(`[id="bcfPreviewBtn"]`).removeClass(inputLock ? "btn-outline-info" : "btn-info");
    });

    $(`[id="bcfPreviewBtn"]`).on('click', function (e) {
        // navigate to second step
        e.preventDefault();
        updateProgressBar("previewBcf");

        loadBcfPreview();

        $(`#previewBcf`).attr('hidden', false);
        $(`#inputBcf`).attr('hidden', true);
    });

    $(`[id="bcfBackBtn"]`).on('click', function (e) {
        // navigate back to first step
        e.preventDefault();
        updateProgressBar("inputBcf");

        $(`#previewBcf`).attr('hidden', true);
        $(`#inputBcf`).attr('hidden', false);
    });


    $("#btnApprove, #btnReturn").on('click', function (e) {
        e.preventDefault();
        let action = $(this).attr("data-value");

        openApprovalModal(action);
    });

    //#endregion

    //#region Methods
    function PricingFieldInputChecker() {
        var sellingPriceValue = $("#BuyerConfirmationModel_SellingPrice").val();
        var amortizationValue = $("#BuyerConfirmationModel_MonthlyAmortization").val();

        if (sellingPriceValue !== "" && amortizationValue !== "") {
            $("#bcfSetAmount").prop('disabled', false);
        } else {
            $("#bcfSetAmount").prop('disabled', true);
        }
    }

    function initializePdfJs() {
        pdfjsLib.GlobalWorkerOptions.workerSrc = baseUrl + 'lib/pdfjs-dist/build/pdf.worker.mjs';
    }

    function initializeCustomValidators() {
        $(`[name="BuyerConfirmationModel.SellingPrice"]`).on('input', function (e) {
            let currentVal = $(this).val().replace(/,/g, '');
            let amortVal = $(`[name="BuyerConfirmationModel.MonthlyAmortization"]`).val().replace(/,/g, '');
            console.log(currentVal);
            console.log(amortVal);

            if (amortVal > currentVal) {
                // Error: Selling Price Amount should not be lower than the amount of monthly Amortization

                //$(this).val(amortVal);
                $(`[id="BuyerConfirmationModel_SellingPrice-error-custom"]`).html(`Selling Price Amount should not be lower than the amount of Monthly Amortization`);
            }
            else {
                $(`[id="BuyerConfirmationModel_SellingPrice-error-custom"]`).html(``);

                PricingFieldInputChecker();
            }
        });

        $(`[name="BuyerConfirmationModel.MonthlyAmortization"]`).on('input', function (e) {
            let currentVal = $(this).val().replace(/,/g, '');
            let sellPrcVal = $(`[name="BuyerConfirmationModel.SellingPrice"]`).val().replace(/,/g, '');

            if (currentVal > sellPrcVal) {
                // Error: Monthly Amortization should not be higher than the selling price

                //$(this).val(sellPrcVal);
                $(`[id="BuyerConfirmationModel_MonthlyAmortization-error-custom"]`).html(`Monthly Amortization should not be higher than the selling price`);
            }
            else {
                $(`[id="BuyerConfirmationModel_MonthlyAmortization-error-custom"]`).html(``);
                PricingFieldInputChecker();
            }
        });
    }

    function initializeInputMasks() {
        $('[name="BuyerConfirmationModel.SellingPrice"]').inputmask({
            regex: "^[0-9]+$"
        });

        $('[name="BuyerConfirmationModel.MonthlyAmortization"]').inputmask({
            regex: "^[0-9]+$"
        });
    }

    function updateProgressBar(targetForm = "inputBcf") {
        var steps = $(".progressbar .progress-step");

        let stepIndex = [
            {
                formId: ["inputBcf"],
                progress: 0,
                callback: function () {
                    $(`[id="bcfPreview"]`).html("");
                }
            },
            {
                formId: ["previewBcf"],
                progress: 1,
                callback: function () {
                   // loadBcfPreview();
                }
            }
        ];

        let stepObj = stepIndex.find(a => a.formId.includes(targetForm));
        currentStep = stepIndex.find(a => a.formId.includes(targetForm)).progress;

        steps.each((index, step) => {
            if (index === currentStep) {
                step.classList.remove("completed");
                step.classList.add("current");
            }
            else if (index < currentStep) {
                step.classList.add("completed");
            }
            else {
                step.classList.remove("current");
                step.classList.remove("completed");
            }
        });

        const allCurrentClasses = document.querySelectorAll(".progressbar .completed");

        let width = ((allCurrentClasses.length / (steps.length - 1)) * 99) + allCurrentClasses.length;

        if (width > 99) width = 99;

        $(`.progressbar #progress`).css('width', `${width}%`);

        stepObj.callback();
    }

    function loadBcfPreview() {
        var formData = new FormData(document.querySelector(`#frm_bcf`));

        $.ajax({
            method: 'POST',
            url: '/Report/LatestBCFB64ForDeveloper',
            data: formData, // Convert to JSON string
            contentType: 'application/json', // Set content type to JSON,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function () {
                //$("#beneficiary-overlay").removeClass('d-none');
            },
            success: function (response) {
                // Redirect to another URL based on the response
                //window.location.href = '/Report/LatestHousingForm2';

                // Handle success response
                //console.log(response);
                // Do something with the response, like displaying a success message

                console.log(response)


                var responseData = atob(response);

                $(`[id="bcfPreview"]`).html("");
                var loadingTask = pdfjsLib.getDocument({ data: responseData });

                loadingTask.promise.then(function (pdf) {
                    console.log('PDF loaded');

                    let pages = pdf._pdfInfo.numPages;

                    for (let i = 1; i <= pages; i++) {
                        pdf.getPage(i).then(page => {
                            console.log(page);

                            let pdfCanvas = document.createElement("canvas");
                            let context = pdfCanvas.getContext("2d");
                            let pageViewPort = page.getViewport({ scale: 1.75 });
                            console.log(pageViewPort);

                            pdfCanvas.width = pageViewPort.width;
                            pdfCanvas.height = pageViewPort.height;

                            $(`[id="bcfPreview"]`).append(pdfCanvas);

                            page.render({
                                canvasContext: context,
                                viewport: pageViewPort
                            });
                        }).catch(error => {
                            console.error(error);
                        })
                    }
                }).catch(function (reason) {
                    // PDF loading error
                    console.error(reason);
                });
            },
            error: function (xhr, status, error) {
                // Handle error
                console.error(xhr.responseText);
                // Show error message to the user
            },
            complete: function () {
                //setTimeout(function () {
                //    $("#beneficiary-overlay").addClass('d-none');
                //}, 2000); // 2000 milliseconds = 2 seconds
            }
        });
    }

    async function loadApprovalData() {
        const requestorId = $(`[name="${CONST_MODULE}.CreatedById"]`).val();
        const currentUserId = $("#current_userId").val();

        let recordId = $("#BuyerConfirmationModel_Id").val();
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

        console.log(response);
        return response;
    }

    function openApprovalModal(action) {
        let $approverModal = $('#approver-modal');
        let modalLabel = $("#approver-modalLabel");
        let transactionNo = $(`[name="BuyerConfirmationModel.Code"]`).val();
        let remarksInput = $('[name="ApprovalLevel.Remarks"]');
        let roleName = $("#txt_role_code").val();
        let $btnSave = $("#btn_save");

        $btnSave.removeClass();
        $('.text-danger.validation-summary-errors').removeClass('validation-summary-errors').addClass('validation-summary-valid')
            .find('li').css('display', 'none');
        remarksInput.removeAttr("data-val-required").removeClass("input-validation-error").addClass("valid");

        if (action == 1) {      //submitted
            modalLabel.html('<span class="fe-send"></span> Submit Application');
            $btnSave.addClass("btn btn-primary").html('<span class="fe-send"></span> Submit')

            remarksInput.removeAttr("data-val-required").removeClass("input-validation-error").addClass("valid");
        }

        else if (action == 11) {
            modalLabel.html('<span class="fe-repeat"></span> Return for Revision BCF');
            $btnSave.addClass("btn btn-warning").html('<span class="fe-repeat"></span> Return for Revision')
            //remarksInput.attr("data-val-required", "true").attr("required", true).addClass("input-validation-error").addClass("invalid");
            remarksInput.attr("required", true);
        }
        else {
            modalLabel.html('<span class="fe-check-circle"></span> Approve BCF');
            $btnSave.addClass("btn btn-success").html('<span class="fe-check-circle"></span> Approve')
            remarksInput.removeAttr("data-val-required").attr("required", false).removeClass("input-validation-error").addClass("valid");
        }

        $("#author_txt").html(`Author: ${roleName}`);
        $("[name='ApprovalLevel.Status']").val(action);
        $("[name='ApprovalLevel.TransactionNo']").val(transactionNo);

        rebindValidator();
        $approverModal.modal("show");
    }

    function rebindValidator() {
        let $form = $("#frm_approver_level");
        let $approverModal = $('#approver-modal');

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

            if (approvalLevelStatus == 3 || approvalLevelStatus == 4) {
                action = "Approve";
                text = "Are you sure you wish to proceed with approving this buyer confirmation application?";
                confirmButtonText = "save and approve";
            }

            else if (approvalLevelStatus == 11) {
                action = "Resubmission";
                text = "Are you sure you wish to proceed with for-resubmission this buyer confirmation application?";
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
                            $("#beneficiary-overlay").removeClass('d-none');

                            // $("#btnApprove").attr({ disabled: true });
                        },
                        success: function (response) {

                            //$("#beneficiary-overlay").addClass('d-none');

                            //messageBox("Successfully saved.", "success");

                            updateBCF(approvalLevelStatus);
                            $approverModal.modal("hide");
                        },
                        error: function (response) {
                            // Error message handling
                            messageBox(response.responseText, "danger", true);
                        },
                        complete: function (xhr, status) {
                            $("#btnApprove").attr({ disabled: false });
                        }
                    });
                }
            });
        });
    }

    function updateBCF(approvalLevelStatus) {
        var formData = new FormData(document.querySelector(`#frm_bcf`));

        let messageArray = [
            {
                approvalLevels: [3, 4], // Approved
                message: "BCF has been saved and approved, and ready for printing"
            },
            {
                approvalLevels: [11], // Resubmission
                message: "BCF has been marked for resubmission"
            }
        ];

        let selectedMessage = messageArray.find(m => m.approvalLevels.includes(Number(approvalLevelStatus))).message;

        $.ajax({
            method: 'POST',
            url: '/BuyerConfirmation/UpdateBCF',
            data: formData,
            contentType: 'application/json',
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                messageBox(selectedMessage, "success");
            },
            error: function (xhr, statusText) {
                messageBox(xhr.responseText, "error");
            },
            complete: function () {
                setTimeout(function () {
                    location.reload();
                }, 2000);
            }
        });
    }

    //#endregion
});