"use strict"

$(async function () {
    const ribbonCollection = [
        {
            documentStatus: 0,
            text: "Not Yet Submitted",
            bsColor: "bg-secondary"
        },
        {
            documentStatus: 1,
            text: "Submitted",
            bsColor: "bg-primary"
        },
        {
            documentStatus: 3,
            text: "Approved",
            bsColor: "bg-success"
        },
        {
            documentStatus: 11,
            text: "For Resubmission",
            bsColor: "bg-warning"
        }
    ];

    const progressBar = $(`[id="uploadProgressBar"] [role="progressbar"]`);

    var ajaxUploadCall;

    var fileInputConfig = {
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
        minFileCount: 1,
        maxFileCount: 1,
        maxFileSize: (1024 * 5),
        theme: "explorer-fa5",
        browseClass: "btn btn-info flex-grow-1",
        removeClass: "btn btn-danger flex-grow-1",
        browseLabel: "Browse",
        mainClass: "d-flex gap-1 justify-content-center",
        showCaption: false,
        showRemove: true,
        showUpload: false,
        removeFromPreviewOnError: true,
        browseOnZoneClick: true
    };

    let documentReferenceId = $('#Id').val();
    let bcfDocumentStatus = $("#Bcf_DocumentStatus");

    initializeBsFileInput();
    initializeRibbon();
    checkInputFile();

    function loadAttachments() {
        let attachmentInput = $("#bcf_PdfFile");
        let allowedExtensions = ['pdf'];

        let options = {
            //theme: 'explorer',
            //browseClass: "btn btn-info",
            //showUpload: false,
            //maxFileCount: 10,
            //validateInitialCount: true,
            //initialPreviewShowDelete: true,
            //overwriteInitial: false,
            //maxFileSize: 25 * 1024,
            //msgSizeTooLarge: 'File "{name}" (<b>{size} KB</b>)'
            //    + 'exceeds maximum allowed upload size of <b>{5} MB</b>. '
            //    + 'Please retry your upload!',
            //allowedFileExtensions: allowedExtensions
        };

        //attachmentInput.fileinput("clear");
        //attachmentInput.fileinput("destroy");

        $.ajax({
            url: baseUrl + "BuyerConfirmation/GetMyBCF",
            success: function (item) {
                let _initialPreview = [];
                let _initialPreviewConfig = [];

                var fileLocation = baseUrl.replace("/", window.location.origin) + item.FileLocation.replaceAll("\\", "/") + item.FileName;
                var deleteUrl = baseUrl + "Document/DeleteDocument/" + item.Id;

                console.log(fileLocation)
                _initialPreview.push(fileLocation);
                _initialPreviewConfig.push({
                    type: "pdf",
                    description: "",
                    size: item.FileSize,
                    caption: item.FileName,
                    key: item.Id,
                    downloadUrl: fileLocation,
                    //url: fileLocation
                });

                //options.allowedFileExtensions = allowedExtensions;
                fileInputConfig.initialPreviewAsData = true;
                //fileInputConfig.usePdfRenderer = false;
                fileInputConfig.initialPreview = _initialPreview;
                fileInputConfig.initialPreviewConfig = _initialPreviewConfig;
                fileInputConfig.initialPreviewShowDelete = false;
                fileInputConfig.browseOnZoneClick = false;
                //fileInputConfig.deleteUrl = deleteUrl;
                fileInputConfig.showRemove = false;
                fileInputConfig.showBrowse = false;
                fileInputConfig.dropZoneEnabled = false;
                fileInputConfig.fileActionSettings = {
                    showDrag: false
                };


                //attachmentInput.fileinput(fileInputConfig);
                //checkInputFile();
                //$(".kv-file-remove").addClass('d-none');
                initializeBsFileInput(true);
            }
        });
    }

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
        $(`[id="uploadProgressBar"]`).attr('hidden', true);
    });

    $(`#fileInputArea .fileinput-remove-button`).on('click', function (e) {
        checkInputFile();
        $(`[id="uploadProgressBar"]`).attr('hidden', true);
    });

    function upload(file) {
        var successFlag = false;

        var formData = new FormData();
        formData.append('file', file);
        formData.append('BuyerConfirmationId', documentReferenceId);

        //console.log(documentReferenceId);

        ajaxUploadCall = $.ajax({
            url: baseUrl + 'Document/UploadBCF',
            type: 'POST',
            data: formData,
            cache: false,
            processData: false,
            contentType: false,
            xhr: function () {
                var xhr = new XMLHttpRequest();

                if (xhr.upload) {
                    // Add event listener
                    xhr.upload.addEventListener('progress', function (e) {
                        if (e.lengthComputable) {
                            var percentComplete = e.loaded / e.total;
                            percentComplete = parseFloat(percentComplete * 100);

                            //console.log(e);
                            if (percentComplete > 100) {
                                percentComplete = 100;
                            }

                            progressBar.attr('aria-valuenow', `${percentComplete}`);
                            progressBar.find(`.progress-bar`).css('width', `${percentComplete}%`);
                        }
                    }, false);
                }

                return xhr;
            },
            beforeSend: function () {
                // Show progressbar
                $(`[id="uploadProgressBar"]`).attr('hidden', false);

                let removeColorClasses = ["bg-primary", "bg-danger"];
                $(`[id="submitPdfFile"]`).attr('disabled', true);

                progressBar.attr('aria-valuenow', `0`);
                progressBar.find(`.progress-bar`).css('width', `0%`);
                progressBar.find('.progress-bar').removeClass(removeColorClasses).addClass('bg-primary');

                $(`#btnCancelRetry`).data('btn-mode', "cancel")
                    .removeClass("btn-info")
                    .addClass("btn-danger")
                    .html("Cancel")
                    .attr('disabled', false);

                $(`[id="fileInputArea"] .fileinput-remove.fileinput-remove-button`).attr('disabled', true);
                $(`[id="bcf_PdfFile"]`).attr('disabled', true);
                $(`[id="bcf_PdfFile"]`).parent().addClass('disabled');

                $(`[id="upload-overlay"]`).removeClass('d-none');

                $("#bcf_PdfFile").fileinput("disable");

                //loading('Uploading...', true);
            },
            success: function (response) {
                messageBox('Uploaded Successfully', "success", true);
                //loader.close();

                $(`#btnCancelRetry`).attr({
                    disabled: true,
                    hidden: true
                });

                $(`[id="upload-overlay"]`).addClass('d-none');

                $("#div_approvebcfNote").addClass("d-none");
                $(`#sidebar-menu`).css('top', `calc(var(--ct-topbar-height) + 1.5rem)`);
                $("#bcf_PdfFile").fileinput("disable");
                updateBcfStatus();

                successFlag = true;
            },
            error: function (xhr, status) {
                //console.log(status);
                //console.log(xhr);

                //$(`[id="submitPdfFile"]`).attr('disabled', false);

                let removeColorClasses = ["bg-primary", "bg-danger"];

                $(`[id="fileInputArea"] .fileinput-remove.fileinput-remove-button`).removeAttr('disabled');
                $(`[id="bcf_PdfFile"]`).removeAttr('disabled');
                $(`[id="bcf_PdfFile"]`).parent().removeClass('disabled');

                progressBar.find('.progress-bar')
                    .removeClass(removeColorClasses)
                    .addClass('bg-danger');

                $(`#btnCancelRetry`).data('btn-mode', "retry")
                    .removeClass("btn-danger")
                    .addClass("btn-info")
                    .html("Retry");

                $(`[id="upload-overlay"]`).addClass('d-none');
                $("#bcf_PdfFile").fileinput("enable");
                messageBox("Upload failed: " + status, "danger", true);
            },
            complete: function (xhr, status) {
                setTimeout(function () {
                    $(`[id="uploadProgressBar"]`).attr('hidden', successFlag);
                    //$(`[id="upload-overlay"]`).addClass('d-none');
                }, 2000);
            }
        });
    }

    function checkInputFile() {
        $(`[id="submitPdfFile"]`).attr('disabled', $('#bcf_PdfFile').prop('files').length === 0);
        $(`[id="fileInputArea"] .d-flex .fileinput-remove-button`).attr('hidden', $('#bcf_PdfFile').prop('files').length === 0);
    }

    function initializeBsFileInput(attachmentFlag) {
        //if (bcfDocumentStatus.val() === '1') {
        //if (bcfDocumentStatus === '1') {
        //    //$(".upload-div").prop("hidden", true);
        //    //$("#submitPdfFile").prop('hidden', true);
        //    return;
        //}

        let documentStatus = $(`#Bcf_DocumentStatus`).val();

        if (!attachmentFlag && [1, 3].includes(Number(documentStatus))) {
            // Load Attachments
            loadAttachments();
            return;
        }
        else {
            $(`[id="bcf_PdfFile"]`).fileinput(fileInputConfig);
        }

        $(`[id="fileInputArea"] .file-drop-zone`).on('drop', function (e) {
            checkInputFile();
            $(`[id="uploadProgressBar"]`).attr('hidden', true);
        });

        $(`[id="fileInputArea"] .file-preview .fileinput-remove`).addClass('d-none');

        $(`#btnCancelRetry`).on('click', function (e) {
            e.preventDefault();
            let removeColorClasses = ["bg-primary", "bg-danger"];

            if ($(this).data('btn-mode') === 'cancel') {
                ajaxUploadCall.abort("Cancelled by user");
            }
            else if ($(this).data('btn-mode') === 'retry') {
                var selectedFile = $('#bcf_PdfFile').prop('files');

                messageBox("Retrying to upload", "info", true);
                upload(selectedFile[0]);
            }
        });
    }

    function initializeRibbon() {
        //var bcfStatus = $("#Bcf_ApplicationStatus").val();

        /*
            bcf-documentstatus 3 - Approved
            bcf-documentstatus 1 - submitted
            bcf-documentstatus 11 - For resubmission
        */

        let ribbon = ribbonCollection.find(r => r.documentStatus === Number(bcfDocumentStatus.val()));

        //$(`[id="bcf_dl_custom_ribbon"]`).attr('hidden', ribbon.documentStatus === 0);
        $(`[id="bcfStatus"]`).addClass(ribbon.bsColor);
        $(`[id="bcfStatus"]`).html(ribbon.text);
    }

    function updateBcfStatus() {
        $.ajax({
            method: 'GET',
            url: baseUrl + 'BuyerConfirmation/GetBcf',
            success: function (data) {
                $(`#Bcf_DocumentStatus`).val(data.BuyerConfirmationDocumentStatus);

                let ribbonColors = ribbonCollection.map(r => r.bsColor);
                let ribbon = ribbonCollection.find(r => r.documentStatus === Number(data.BuyerConfirmationDocumentStatus));

                $(`[id="bcfStatus"]`).removeClass(ribbonColors);
                $(`[id="bcfStatus"]`).addClass(ribbon.bsColor);
                $(`[id="bcfStatus"]`).html(ribbon.text);

                initializeRibbon();
            }
        });
    }
});