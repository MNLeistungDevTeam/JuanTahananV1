"use strict"

$(function () {
    ////////////////////////////////

    let documentReferenceId = $('#Id').val();

    $(`[id="bcf_PdfFile"]`).fileinput({
        dropZoneTitle: `
            <div class="file-icon">
                <i class="mdi mdi-file-pdf"></i>
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
        theme: "fa5",
        browseClass: "btn btn-info",
        mainClass: "d-grid",
        showCaption: false,
        showRemove: true,
        showUpload: false,
    });

    $('#bcf_PdfFile').on('change', function () {
        var file = this.files[0];

        console.log(file)

        if (file) {
            upload(file);

        }
    });

    function upload(file) {
        var formData = new FormData();
        formData.append('file', file);
        formData.append('BuyerConfirmationId', documentReferenceId);

        console.log(documentReferenceId)

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
});