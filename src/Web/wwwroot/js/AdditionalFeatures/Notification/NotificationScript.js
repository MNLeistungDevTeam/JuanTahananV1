"use strict"
$(function () {
    var isReadValue = null;
    var isDelete;
    const approvalnotiftype = 3;
    const pagenumber = 1;

    var tbl_mynotif = $("#tbl_mynotif").DataTable({
        ajax: {
            url: baseUrl + "Notification/GetMyNotifications",
            data: function (d) {
                d.IsRead = isReadValue;
                d.IsDelete = 0;
            },
            dataSrc: ""
        },
        language: {
            processing: "<div class='text-center'><span class='spinner-border spinner-border-sm'></span> Loading...</div>",
            previous: "<i class='mdi mdi-chevron-left'>",
            next: "<i class='mdi mdi-chevron-right'>",
            "emptyTable": "No Notifications Yet!",
        },
        columns: [
            {
                data: "Id"
            },
            {
                data: "Title",
                class: "text-start",
                render: function (data, type, row) {
                    let plevel1 = `<i class="fas fa-star fa-text-color-red" data-toggle="tooltip" data-placement="bottom" title = "Important"></i>`
                    let plevel2 = `<i class="fas fa-star fa-text-color-yellow" data-toggle="tooltip" data-placement="bottom" title = "Fairly Important"></i>`
                    let plevel3 = `<i class="fas fa-star fa-text-color-green" data-toggle="tooltip" data-placement="bottom" title = "Slightly Important"></i>`
                    let plevel = data;

                    let stars = "";

                    if (plevel == 1) stars = plevel1;
                    else if (plevel == 2) stars = plevel2;
                    else stars = plevel3;

                    let isDelete = row.IsDelete;
                    let IsRead = row.IsRead;
                    let title = `${stars} <span><i class="${isDelete == true ? 'fa fa-trash' : ''} me- 1"></i><span class="${IsRead == false ? 'fw-bold' : ''}">${data}</span></span>`
                    return title;
                }
            },

            {
                data: "Preview",
                class: "text-start",
                render: function (data, type, row) {
                    let readclass = "fw-bold read";
                    let unreadclass = "text-info unread";
                    let Id = row.Id;
                    let IsRead = row.IsRead;
                    let preview = `<a href='${row.ActionLink}' class="${IsRead == false ? readclass : unreadclass}" data-id="${Id}">${data}</a>`
                    return preview;
                }
            },
            {
                data: "DateCreated",
                class: "text-start",
                render: function (data) {
                    let readclass = "fw-bold";

                    return convertDate(data);
                }
            },
            {
                data: "IsRead",
                class: "text-start",
                visible: false,
                render: function (data) {
                    return data;
                }
            },
            {
                data: "IsDelete",
                class: "text-start",
                visible: false,
                render: function (data) {
                    return data;
                }
            }

        ],
        initComplete: function () {
            $("#tbl_mynotif_filter").hide();
            $("#tbl_mynotif_length").hide();
        },
        drawCallback: function () {
            $(".dataTables_paginate > .pagination").addClass("pagination-rounded");
        },
        columnDefs: [
            {
                targets: 0,
                orderable: false,
                className: "select-checkbox",
                data: null,
                render: function () {
                    return "";
                }
            }
        ],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        rowId: "Id",
        processing: true,
        scrollX: true,
        paging: true,
        scrollY: '45vh',
        scrollCollapse: false,
        pageLength: 20,
        stateSave: true,
        searchHighlight: true,
        select: true
    });

    tbl_mynotif.on('select deselect draw', function () {
        var all = tbl_mynotif.rows({ search: 'applied' }).count();
        var selectedRows = tbl_mynotif.rows({ selected: true, search: 'applied' }).count();
        $("#select-allnotif").prop("checked", (all == selectedRows && all > 0));

        $("#btn_unread").attr({
            "disabled": !(selectedRows >= 1)
        });

        $("#btn_read").attr({
            "disabled": !(selectedRows >= 1)
        });

        $("#btn_delete").attr({
            "disabled": !(selectedRows >= 1)
        });

        $("#btn_preview").attr({
            "disabled": !(selectedRows == 1)
        });

        var rowCount = tbl_mynotif.rows().count();
        var selectedRows = tbl_mynotif.rows({ selected: true, search: 'applied' }).count();

        if (rowCount == selectedRows && rowCount != 0) {
            $("#selectall_notif").prop("checked", true);
        } else {
            $("#selectall_notif").prop("checked", false);
        }
    });

    $("#selectall_notif").on("change", function () {
        if ($(this).prop("checked")) {
            tbl_mynotif.rows({ search: 'applied' }).select();
        } else {
            tbl_mynotif.rows({ search: 'applied' }).deselect();
        }
    })

    $("#btn_unread").on("click", function () {
        updateMyNotifToUnRead();
    });

    $("#btn_read").on("click", function () {
        updateMyNotifToRead();
    });

    $("#btn_delete").on("click", function (e) {
        var notifIds = tbl_mynotif.rows({ selected: true }).data().pluck("Id").toArray().toString();
        var isread = $("#IsRead").val();
        //var notifCount = tbl_mynotif.rows({ selected: true }).data().pluck("Id").toArray().length;
        //console.log(notifCount);

        //#region TrashMode
        //it means that notification filtered is in trashmode so the action delete turns to permanently delete

        //if (isread == 2) {
        //    Swal.fire({
        //        title: 'Are you sure?',
        //        text: `These Record/s Will Be Deleted Permanently`,
        //        icon: 'question',
        //        showCancelButton: true,
        //        confirmButtonColor: '#3085d6',
        //        cancelButtonColor: '#d33',
        //        confirmButtonText: 'Confirm',
        //        showLoaderOnConfirm: true,
        //        preConfirm: (login) => {
        //            return fetch(`${baseUrl}Notification/DeleteNotifs/`,
        //                {
        //                    method: "DELETE",
        //                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        //                    body: `notifIds=${notifIds}`
        //                })
        //                .then(response => {
        //                    if (!response.ok) {
        //                        throw new Error(response.statusText)
        //                    }
        //                    return response;
        //                })
        //                .catch(error => {
        //                    Swal.showValidationMessage(
        //                        `Request failed: ${error}`
        //                    )
        //                })
        //        },
        //        allowOutsideClick: () => !Swal.isLoading()
        //    }).then((result) => {
        //        if (result.isConfirmed) {
        //            messageBox("Record(s) successfully deleted.", "success");
        //            tbl_mynotif.ajax.reload(null, false)
        //            loadNotif();
        //        }
        //    })
        //}
        //else
        //    updateMyNotifToTrash(notifIds);
        //tbl_mynotif.ajax.reload();
        //loadNotif();

        //#endregion

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
                return fetch(`${baseUrl}Notification/DeleteNotification/`,
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
                loadNotif(pagenumber, approvalnotiftype);
            }
        })
    });

    $("#btn_preview").on("click", function () {
        $("#defaulttext").text("");
        var id = tbl_mynotif.rows({ selected: true }).data().pluck("Id").toArray().toString();
        loadContent(id);
    });

    $("#btn_refresh").click(function () {
        tbl_mynotif.ajax.reload();
    });

    $('#txt_search_tbl_mynotif').on('input', function () {
        tbl_mynotif.search(this.value).draw();
    });

    $("#filter_isRead").on("change", function () {
        let isRead = $(this).val();

        if (isRead == "") {
            isReadValue = null;
        } else {
            isReadValue = isRead == 1 ? true : false;
        }

        //if (isReadValue != 2) {
        //    isDelete = "";
        //}
        //if (isReadValue == 2) {
        //    var val = "";
        //    isDelete = "1";
        //    isReadValue = val;
        //}

        tbl_mynotif.ajax.reload();
    });

    function loadContent(id) {
        $.ajax({
            url: baseUrl + "Notification/GetNotification/" + id,
            method: "get",
            success: function (response) {
                let DateCreated = convertDate(response.DateCreated, "YYYY-MM-DD HH:mm:ss");
                let Content = response.Content;

                $("#NotifId").text(response.Id);
                $("#NotifTitle").text(response.Title);
                $("#NotifContent").html(Content);
                $("#NotifDateCreated").text(DateCreated);
            }
        });
    }

    function updateMyNotifToUnRead() {
        var notifIds = tbl_mynotif.rows({ selected: true }).data().pluck("Id").toArray().toString();

        $.ajax({
            url: "/Notification/UpdateNotifToUnReads",
            method: "Put",
            data: {
                notifIds: notifIds
            },
            success: function () {
                tbl_mynotif.ajax.reload();
                loadNotif(pagenumber, approvalnotiftype);
            }
        })
    }

    function updateMyNotifToRead() {
        var notifIds = tbl_mynotif.rows({ selected: true }).data().pluck("Id").toArray().toString();
        $.ajax({
            url: "/Notification/UpdateNotifToReads",
            method: "Put",
            data: {
                notifIds: notifIds
            },
            success: function () {
                tbl_mynotif.ajax.reload();
                loadNotif(pagenumber, approvalnotiftype);
            }
        })
    }

    function updateMyNotifToTrash(notifIds) {
        $.ajax({
            url: "/Notification/UpdateNotifToTrash",
            method: "Put",
            data: {
                notifIds: notifIds
            },
            success: function () {
                tbl_mynotif.ajax.reload();
                loadNotif(pagenumber, approvalnotiftype);
            }
        })
    }
});