"use strict"
$(function () {
    let id = $("#Id").text().toString();

    $("#btn_delete_notif").on("click", function (e) {
        var notifIds = id.toString();
        Swal.fire({
            title: 'Are you sure?',
            text: `These Record/s Will Be Deleted Permanently`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showLoaderOnConfirm: true,
            preConfirm: (login) => {
                return fetch(`${baseUrl}Notification/DeleteNotifs/`,
                    {
                        method: "DELETE",
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        body: `notifIds=${notifIds}`
                    })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error(response.statusText)
                        }
                        return response;
                    })
                    .catch(error => {
                        Swal.showValidationMessage(
                            `Request failed: ${error}`
                        )
                    })
            },
            allowOutsideClick: () => !Swal.isLoading()
        }).then((result) => {
            if (result.isConfirmed) {
                messageBox("Record(s) successfully deleted.", "success");
                tbl_mynotif.ajax.reload(null, false)
                loadNotif(1);
                window.location.href = baseUrl + "Notification/Index";
            }
        })
    });

    $("#btn_unread_notif").on("click", function () {
        UpdateNotifToUnread(id);
        window.location.href = baseUrl + "Notification/Index";
    });

    $("#btn_notifback").on("click", function () {
        window.location.href = baseUrl + "Notification/Index";
    });

    function UpdateNotifToUnread(id) {
        $.ajax({
            url: "/Notification/UpdateNotifToUnReads",
            method: "Put",
            data: {
                notifIds: id
            },
            success: function () {
                loadNotif(1);
                tbl_mynotif.ajax.reload();
            }
        });
    }
});