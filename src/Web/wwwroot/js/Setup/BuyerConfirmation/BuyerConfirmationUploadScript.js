"use strict"

$(function () {
    ////////////////////////////////

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

});