"use strict"

$(function () {
    var bcfDropzone;
    let selectedFile;

    ////////////////////////////////
    initializeBsFileInput();
    //initializeTraditionalDrop();

    $(`[id="submitPdfFile"]`).on('click', function (e) {
        e.preventDefault();
    });

    function initializeBsFileInput() {
        $(`[id="bcf_PdfFile"]`).fileinput({
            dropZoneTitle: `
                <div class="file-icon">
                    <img src="${baseUrl}img/pdf.png" />
                </div>
                <p class="fw-4">
                    <span class="text-info fw-bold">Drag and Drop</span>
                    <br>
                    here to upload your file or
                    <br>
                    browse files using the button below.
                </p>
            `,
            showPreview: true,
            allowedFileExtensions: ['pdf'],
            maxFileCount: 1,
            minFileCount: 1,
            maxFileSize: (1024 * 5),
            theme: "explorer-fa5",
            browseClass: "btn btn-info flex-grow-1",
            removeClass: "btn btn-danger flex-grow-1",
            browseLabel: "Browse",
            mainClass: "d-flex gap-1 justify-content-center",
            showCaption: false,
            showRemove: true,
            showUpload: false,
            removeFromPreviewOnError: true
        });
    }

    function initializeTraditionalDrop() {
        const dropArea = document.querySelector(".file-drag-area");

        dropArea.addEventListener("dragover", (event) => {
            event.preventDefault();

            // file is over dragarea
            dropArea.classList.add("active");
        });

        dropArea.addEventListener("dragleave", () => {
            // file is outside dragarea
            dropArea.classList.remove("active");
        });
        
        dropArea.addEventListener("drop", (event) => {
            event.preventDefault();

            // file is dropped on dragarea
            dropArea.classList.remove("active");

            selectedFile = event.dataTransfer.files[0];
            //console.log(file.type);

            let validExtensions = ["application/pdf"];

            if (validExtensions.includes(selectedFile.type)) {
                console.log("pdf");

                // proceed to place file
                let fileReader = new FileReader();

                fileReader.onload = () => {
                    let fileUrl = fileReader.result;

                    
                };
            }

        });

    }
});