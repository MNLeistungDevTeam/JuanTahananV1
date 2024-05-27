"use strict"

var inputLock = false;

$(function () {
    const { pdfjsLib } = globalThis;
    const debounceError = debounce((callback) => { callback(); }, 5000);

    const CONST_MODULE = "BuyerConfirmation  Requests";
    const CONST_MODULE_CODE = "BCF-APLRQST";
    const CONST_TRANSACTIONID = $("#BuyerConfirmation_Id").val();
    const buyerConfirmationCode = $("#BuyerConfirmationModel_Code").val();
    const approvalStatus = $("#BuyerConfirmationModel_ApprovalStatus").val();

    var currentStep = 0;
    //document.ready
    $(function () {
        initializeLeftDecimalInputMask(".decimalInputMask", 2);
        initializePdfJs();
        updateProgressBar();

        initializeInputMasks();
        initializeCustomValidators();

        loadApprovalData();

        //#region Initialization

        // add blue border on focus
        $(document).on("focus", "#BuyerConfirmationModel_SellingPrice", function () {
            $(this).addClass("focused");
            $(this).addClass("border-primary border-3 focused");
        });

        // remove blue border when focus is lost
        $(document).on("blur", "#BuyerConfirmationModel_SellingPrice", function () {
            $(this).removeClass("focused");
            $(this).removeClass("border-primary border-3 focused");
        });

        // add blue border on focus
        $(document).on("focus", "#BuyerConfirmationModel_MonthlyAmortization", function () {
            $(this).addClass("focused");
            $(this).addClass("border-primary border-2 focused");
        });

        // remove blue border when focus is lost
        $(document).on("blur", "#BuyerConfirmationModel_MonthlyAmortization", function () {
            $(this).removeClass("focused");
            $(this).removeClass("border-primary border-2 focused");
        });

        if (buyerConfirmationCode !== localStorage.getItem("BuyerConfirmationModel_Code")) {
            localStorage.removeItem("BuyerConfirmationModel_Code");
            localStorage.removeItem("BuyerConfirmationModel_SellingPrice");
            localStorage.removeItem("BuyerConfirmationModel_MonthlyAmortization")
        }

        //Load data from local storage
        if (localStorage.getItem("BuyerConfirmationModel_Code")) {
            $("#BuyerConfirmationModel_Code").val(localStorage.getItem("BuyerConfirmationModel_Code"));
        }

        if (localStorage.getItem("BuyerConfirmationModel_SellingPrice")) {
            $("#BuyerConfirmationModel_SellingPrice").val(localStorage.getItem("BuyerConfirmationModel_SellingPrice"));
        }

        if (localStorage.getItem("BuyerConfirmationModel_MonthlyAmortization")) {
            $("#BuyerConfirmationModel_MonthlyAmortization").val(localStorage.getItem("BuyerConfirmationModel_MonthlyAmortization"));
        }

        if (approvalStatus === '3') {
            $("#bcfSetAmount").click();
            $("#bcfSetAmount").text("Change Pricing");
            updateChangePricingButton();
        }

        PricingFieldInputChecker();
        updateChangePricingButton();

        //#endregion
    });

    //#region Events
    $(`[id="bcfSetAmount"]`).on('click', function (e) {
        // lock two inputs to readonly
        e.preventDefault();

        inputLock = !inputLock;
        $(`[name="BuyerConfirmationModel.SellingPrice"], [name="BuyerConfirmationModel.MonthlyAmortization"]`).attr('readonly', inputLock);

        $(this).html(inputLock ? "Change Pricing" : "Apply Pricing");

        $(this).addClass(inputLock ? "btn-outline-info" : "btn-info");
        $(this).removeClass(!inputLock ? "btn-outline-info" : "btn-info");

        $(`[id="bcfPreviewBtn"]`).addClass(!inputLock ? "btn-outline-info" : "btn-info");
        $(`[id="bcfPreviewBtn"]`).removeClass(inputLock ? "btn-outline-info" : "btn-info");

        localStorage.setItem("BuyerConfirmationModel_Code", buyerConfirmationCode);
        localStorage.setItem("BuyerConfirmationModel_SellingPrice", $("#BuyerConfirmationModel_SellingPrice").val());
        localStorage.setItem("BuyerConfirmationModel_MonthlyAmortization", $("#BuyerConfirmationModel_MonthlyAmortization").val());
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

    $("input[id*='BuyerConfirmationModel'][required]").each(function () {
        $(this).on('change input', updateChangePricingButton);
    });
    //#endregion Events

    //#region Methods
    function PricingFieldInputChecker() {
        var sellingPriceValue = $("#BuyerConfirmationModel_SellingPrice").val();
        var amortizationValue = $("#BuyerConfirmationModel_MonthlyAmortization").val();

        let fieldsAreNotEmpty = sellingPriceValue !== "" && amortizationValue !== "";

        if (fieldsAreNotEmpty) {
            $("#bcfSetAmount").prop('disabled', false);
        } else {
            $("#bcfSetAmount").prop('disabled', true);
        }

        return fieldsAreNotEmpty;
    }

    function initializePdfJs() {
        pdfjsLib.GlobalWorkerOptions.workerSrc = baseUrl + 'lib/pdfjs-dist/build/pdf.worker.mjs';
    }

    function initializeCustomValidators() {
        $(`[name="BuyerConfirmationModel.MonthlyAmortization"], [name="BuyerConfirmationModel.SellingPrice"]`).on('input', function (e) {
            if (!PricingFieldInputChecker()) {
                return;
            }

            let sellingPrice = $(`[name="BuyerConfirmationModel.SellingPrice"]`).val().replace(/,/g, '');
            let monthlyAmort = $(`[name="BuyerConfirmationModel.MonthlyAmortization"]`).val().replace(/,/g, '');

            sellingPrice = parseFloat(sellingPrice);
            monthlyAmort = parseFloat(monthlyAmort);

            if (monthlyAmort > sellingPrice) {
                // Error: Monthly Amortization should not be higher than the selling price

                //$(this).val(sellPrcVal);
                $(`[id="BuyerConfirmationModel_SellingPrice-error-custom"]`).html(`Selling Price Amount should not be lower than the amount of Monthly Amortization`);
                $(`[id="BuyerConfirmationModel_MonthlyAmortization-error-custom"]`).html(`Monthly Amortization should not be higher than the selling price`);
            }
            else {
                $(`[id="BuyerConfirmationModel_SellingPrice-error-custom"]`).html(``);
                $(`[id="BuyerConfirmationModel_MonthlyAmortization-error-custom"]`).html(``);
            }

            $(`[id="bcfSetAmount"]`).attr('disabled', monthlyAmort > sellingPrice);
        });
    }

    function initializeInputMasks() {
        initializeLeftDecimalInputMask(".decimalInputMask5", 2);
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

            if (!$form.valid()) {
                messageBox("Please apply the fields.", "danger", true);
                return;
            }

            if (!inputLock) {
                messageBox("Please apply the pricing details first before proceeding.", "warning", true);
                return;
            }

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
                            //Clear local storage data
                            localStorage.removeItem("BuyerConfirmationModel_Code");
                            localStorage.removeItem("BuyerConfirmationModel_SellingPrice");
                            localStorage.removeItem("BuyerConfirmationModel_MonthlyAmortization")

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

    function updateChangePricingButton() {
        // Check if the selling price input field is not readonly
        if (!$("#BuyerConfirmationModel_SellingPrice").prop("readonly")) {
            // Check if all required fields with BuyerConfirmationModel in their IDs have values
            let allFilled = true;
            $("input[id*='BuyerConfirmationModel'][required]").each(function () {
                if ($(this).val().trim() === '') {
                    allFilled = false;
                    return false; // Exit the each loop early
                }
            });

            // Change the button label based on whether all fields are filled
            if (allFilled) {
                $("#bcfSetAmount").text("Apply pricing");
            } else {
                $("#bcfSetAmount").text("Set pricing");
            }
        }
    }

    //#endregion
});