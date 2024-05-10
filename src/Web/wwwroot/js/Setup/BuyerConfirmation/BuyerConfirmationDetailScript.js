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
        $(`[name="SellingPrice"], [name="MonthlyAmortization"]`).attr('readonly', inputLock);

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
        var form1 = $("#frm_bcf");
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

                $(`[id="bcfPreview"]`).html("");
                var loadingTask = pdfjsLib.getDocument({ data: atob(response) });

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
});