"use strict"

$(function () {
    var ajaxUploadCall;

    let documentReferenceId = $('#Id').val();

    ////////////////////////////////

    initializeBsFileInput();
    //initializeTraditionalDrop();

    checkInputFile();

    $(`[id="submitPdfFile"]`).on('click', function (e) {
        e.preventDefault();

        var selectedFile = $('#bcf_PdfFile').prop('files');
        //console.log(selectedFile);

        if (selectedFile.length !== 0) {
            // SweetAlert
            Swal.fire({
                title: `Confirm Uploading file`,
                text: "Are you sure that the selected document (file) is correct?",
                icon: 'info',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: `Yes, upload it`,
                cancelButtonText: 'Cancel'
            }).then((result) => {
                if (result.isConfirmed) {
                    // User confirmed, proceed with form submission

                    upload(selectedFile[0]);
                }
            });
        }
        else {
            messageBox("No selected files yet, please browse using the button or drag the file in the area provided", "danger", true);
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

        //console.log(documentReferenceId);

        ajaxUploadCall = $.ajax({
            url: '/Document/UploadBCF',
            type: 'POST',
            data: formData,
            cache: false,
            processData: false,
            contentType: false,
            beforeSend: function () {
                $(`[id="submitPdfFile"]`).attr('disabled', true);
                //loading('Uploading...', true);
            },
            success: function (response) {
                messageBox('Uploaded Successfully', "success", true);
                //loader.close();
            },

            xhr: function () {
                var xhr = new XMLHttpRequest();

                if (xhr.upload) {
                    // Show progressbar
                    $(`[id="uploadProgressBar"]`).attr('hidden', false);

                    // Add event listener
                    xhr.upload.addEventListener('progress', function (e) {
                        if (e.lengthComputable) {
                            var percentComplete = e.loaded / e.total;
                            percentComplete = parseFloat(percentComplete * 100);
                            console.log(e);
                            if (percentComplete > 100) {
                                percentComplete = 100;
                            }

                            $(`[id="uploadProgressBar"] [role="progressbar"]`).attr('aria-valuenow', `${percentComplete}`);
                            $(`[id="uploadProgressBar"] [role="progressbar"] .progress-bar`).css('width', `${percentComplete}%`);
                        }
                    }, false);
                }

                return xhr;
            },
            error: function (xhr, status) {
                //console.log(status);
                //console.log(xhr);
                messageBox(status, "danger", true);
                $(`[id="submitPdfFile"]`).attr('disabled', false);
            },
            complete: function (xhr, status) {
                $(`[id="uploadProgressBar"]`).attr('hidden', true);
            }
        });
    }

    function checkInputFile() {
        $(`[id="submitPdfFile"]`).attr('disabled', $('#bcf_PdfFile').prop('files').length === 0);
        $(`[id="fileInputArea"] .d-flex .fileinput-remove-button`).attr('hidden', $('#bcf_PdfFile').prop('files').length === 0);
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

        $(`[id="fileInputArea"] .file-drop-zone`).on('drop', function (e) {
            $('#bcf_PdfFile').trigger('change');
        });

        $(`[id="fileInputArea"] .file-preview .fileinput-remove`).addClass('d-none');

    }
});