$(() => {
    "use strict";
    function handleFileInput(event) {
        var file = event.target.files[0];
        if (file) {
            uploadFile(file, function (newImageUrl) {
                $('.user-profile').each(function () {
                    $(this).attr('src', newImageUrl);
                });
            });
        }
    }

    function uploadFile(file, callback) {
        var formData = new FormData();
        formData.append('file', file);
        $.ajax({
            url: '/Document/UploadProfile',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response && response.imageUrl) {
                    callback(response.imageUrl);
                } else {
                    console.error('Invalid response from server.');
                }
            },
            error: function (xhr, status, error) {
                console.error('Error occurred during file upload:', error);
            }
        });
    }
    $('#avatar-input').on('change', handleFileInput);

})
