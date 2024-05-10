"use strict"

$(function () {
    const { pdfjsLib } = globalThis;

    var currentStep = 0;
    var inputLock = false;

    initializePdfJs();
    updateProgressBar();

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

    function initializePdfJs() {
        pdfjsLib.GlobalWorkerOptions.workerSrc = baseUrl + 'lib/pdfjs-dist/build/pdf.worker.mjs';
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
                    loadBcfPreview();
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

    }
});