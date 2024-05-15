"use strict"

$(function () {
    var bcfDropzone;
    let selectedFile;

    let documentReferenceId = $('#Id').val();


    ////////////////////////////////

    initializeBsFileInput();
    //initializeTraditionalDrop();

    checkInputFile();

    $(`[id="submitPdfFile"]`).on('click', function (e) {
        e.preventDefault();

        var selectedFile = $('#bcf_PdfFile').prop('files');
        console.log(selectedFile);

        if (selectedFile.length !== 0) {
            upload(selectedFile[0]);
        }
        else {
            messageBox("No selected files yet, please browse or drag the file in the area provided.", "danger", true);
        }
    });

    $('#bcf_PdfFile').on('change', function (e) {
        // $(this).prop('files').length === 0

        checkInputFile();
    });

    $(`#fileInputArea .fileinput-remove-button`).on('click', function (e) {

        checkInputFile();
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
                $(`[id="submitPdfFile"]`).attr('disabled', true);
                loading('Uploading...', true);
            },
            success: function (response) {
                messageBox('Uploaded Successfully', "success", true);
                loader.close();
            },
            error: function (xhr, status, error) {
                messageBox(xhr.responseText, "danger", true);
                $(`[id="submitPdfFile"]`).attr('disabled', false);
                loader.close();
            },
            complete: function (xhr, status) {
            }
        });
    }

    function checkInputFile() {
        $(`[id="submitPdfFile"]`).attr('disabled', $('#bcf_PdfFile').prop('files').length === 0);
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