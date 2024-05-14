"use strict"

$(function () {
    var bcfDropzone;
    let selectedFile;

    ////////////////////////////////
    initializeBsFileInput();
    //initializeTraditionalDrop();

    let documentReferenceId = $('#Id').val();

    $(`[id="submitPdfFile"]`).on('click', function (e) {
        e.preventDefault();



        var selectedFile = $('#bcf_PdfFile').prop('files')[0];
        console.log(selectedFile);

        if (selectedFile.length !== 0) {
            upload(selectedFile);
        }
        else {
            messageBox("No selected files yet, please browse or drag the file in the area provided.", "danger", true);
        }
    });

    function upload(file) {
        var formData = new FormData();
        formData.append('file', file);
        formData.append('BuyerConfirmationId', documentReferenceId);

        console.log(documentReferenceId);

        $.ajax({
            url: '/Document/UploadBCF',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () {
                loading('Uploading...', true);
            },
            success: function (response) {
                messageBox('Uploaded Successfully', "success", true);
                loader.close();
            },
            error: function (xhr, status, error) {
                messageBox(xhr.responseText, "danger", true);
                loader.close();
            }
        });
    }

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
});