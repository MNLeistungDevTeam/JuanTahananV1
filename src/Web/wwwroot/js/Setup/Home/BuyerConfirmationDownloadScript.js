"use strict"

const bcfCode = $("#input_bcfCode").val();
$(function () {

    $("#btn_download").on("click", function () {

        downloadBcf();


    });



    function downloadBcf() {
        $.ajax({
            url: baseUrl + `Report/PrintedBCF/${bcfCode}`, 
            type: 'GET',
            success: function (response) {
                // Assuming the response is a base64 string
                var base64String = response;

                // Convert base64 to Blob
                var byteCharacters = atob(base64String);
                var byteNumbers = new Array(byteCharacters.length);
                for (var i = 0; i < byteCharacters.length; i++) {
                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                }
                var byteArray = new Uint8Array(byteNumbers);
                var blob = new Blob([byteArray], { type: 'application/pdf' });

                // Create a link to download the Blob
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = bcfCode + '.pdf'; // The name of the downloaded file
                link.click();

                // Clean up the URL.createObjectURL object
                window.URL.revokeObjectURL(link.href);
            },
            error: function (error) {
                console.log('Error:', error);
            }
        });

      
    }

});