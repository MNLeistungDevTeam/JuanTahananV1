"use strict"

const notifConnection = new signalR.HubConnectionBuilder().withUrl(baseUrl + "notificationHub").build();
const onlineUserCount = new signalR.HubConnectionBuilder().withUrl(baseUrl + "onlineUserHub").build();
const companyCode = $("#txt_company_code").val();
const notifRoleName = $("#txt_role_name").val();
const notifUserId = $("#txt_userId").val();
var currentNotifPage = 1;
var notifPageLimit;
var notiftype = 3;

$(function () {
    //#region Signal R
    notifConnection.on("AddNotifToPage", function (user, message) {
        toastr.info(message, user);
        loadSideDashInfo();
    });
    notifConnection.on("AddNotifGroup", function (message) {
        //reset to 1
        currentNotifPage = 1;
        loadNotif(currentNotifPage, notiftype);
    });
    notifConnection.start().then(function () {
        notifConnection.invoke('JoinGroup', companyCode);
        notifConnection.invoke("JoinGroup", notifRoleName);
        notifConnection.invoke("JoinGroup", notifUserId);
    }).catch(function (err) {
        return console.error(err.toString());
    });

    onlineUserCount.start().then(function () {
        //reset to 1
        currentNotifPage = 1;
        loadNotif(currentNotifPage, notiftype);
    }).catch(function (err) {
        return console.error(err.toString());
    });

    //#endregion

    //#region Events
    $("#notif_div").find(".simplebar-content-wrapper").on("scroll", function () {
        let scrollHeight = this.scrollHeight;
        let containerHeight = $(this).scrollTop() + $(this).innerHeight() + 1;

        //console.log("scroll: " + scrollHeight, containerHeight);
        //if (Math.abs(scrollHeight - containerHeight) > 5)
        //    return;

        if (scrollHeight === containerHeight && currentNotifPage < notifPageLimit) {
            currentNotifPage++;
            console.log(notifPageLimit);
            loadNotif(currentNotifPage, notiftype);
        }
    })

    $("#notif_clearall").on("click", function (e) {
        deleteAllNotification();
        countUnreadNotif();
        e.stopPropagation();
    });

    //#endregion

    $(`#btn_notiftransaction`).on('click', function (e) {
        e.stopPropagation();
        notiftype = 0;
        currentNotifPage = 1;
        loadNotif(currentNotifPage, notiftype);
    });

    $(`#btn_notifapproval`).on('click', function (e) {
        e.stopPropagation();
        notiftype = 3;
        currentNotifPage = 1;
        loadNotif(currentNotifPage, notiftype);
    });
});

//#region Methods

function loadNotif(pageNumber, type) {
    $.ajax({
        url: "/Notification/GetUnreadNotification",
        data: {
            pageNumber: pageNumber,
            type: type
        },
        method: "get",
        success: function (data) {
            if (pageNumber == 1) {
                $("#notif_div").find(".simplebar-content").empty();
                $("#notif_div").find(".simplebar-content").css("padding", "24px 24px");
            }

            if (data.length == 0) {
                $("#notif_div").find(".simplebar-content").append(`<h5 class="text-muted text-center font-13 fw-normal mt-2">No notification</h5>`);
            }
            else {
                notifPageLimit = data[0].PageNumberLimit;

                for (let item of data) {
                    addNotifItem(item);
                }

                /*countUnreadNotif();*/
            }

            countUnreadNotif();
        }
    });
}

function addNotifItem(item) {
    let timePassed = moment(new Date(item.DateCreated)).fromNow();
    let itemToAdd = `<!-- item-->
                <a href="javascript:void(0);" class="dropdown-item p-0 notify-item card ${item.IsRead ? "read" : "unread"}-noti shadow-none my-2 notif-item" data-id="${item.Id}" data-val="${item.ActionLink}" title="${item.Content}">
                    <div class="card-body">
                        <span class="float-end noti-close-btn text-muted" data-id="${item.Id}"><i class="mdi mdi-close"></i></span>
                        <div class="d-flex align-items-center">
                            <div class="flex-shrink-0">
                                <div class="notify-icon bg-primary">
                                    <i class="mdi mdi-comment-account-outline"></i>
                                </div>
                            </div>
                            <div class="flex-grow-1 text-truncate ms-2">
                                <h5 class="noti-item-title fw-semibold font-14">${item.Title}<small class="fw-normal text-muted ms-1">${timePassed}</small></h5>
                                <small class="noti-item-subtitle text-muted">${item.Content}</small>
                            </div>
                        </div>
                    </div>
                </a>`;

    $("#notif_div").find(".simplebar-content").append(itemToAdd);

    $(".noti-close-btn").on("click", function (e) {
        var id = $(this).attr('data-id').toString();
        deleteNotification(id);
        $(this).parent().closest(".notify-item").remove();

        countUnreadNotif();
        e.stopPropagation();
    });

    $(".notif-item").on("click", async function () {
        let dataId = $(this).attr('data-id');
        let dataUrl = $(this).attr('data-val');
        let url = `${baseUrl}${dataUrl}`;
        let defaultURL = `${baseUrl}Notification/Read/${dataId}`;
        let finalActionLink = dataUrl != "" ? url : defaultURL;

        await readNotification(dataId);
        window.location.href = finalActionLink;
    });
}

function countUnreadNotif() {
    $.ajax({
        url: baseUrl + "Notification/CountUnreadNotification",
        method: "get",
        success: function (data) {
            let count = data;
            let unreadmsgcount = count + " Unread Notification(s)"

            $("#unread-count").text(unreadmsgcount).attr({ hidden: count == 0 });
            $(".noti-span").text(count).attr({ hidden: count == 0 });

            countUnreadTransactionNotif();
            countUnreadApprovalNotif();
        }
    });
}

function countUnreadTransactionNotif() {
    $.ajax({
        url: baseUrl + "Notification/CountUnreadTransaction",
        method: "get",
        success: function (data) {
            let count = data;
            let unreadmsgcount = `(${count})`;

            $(".unread-notiftransaction").empty().text(unreadmsgcount);
        }
    });
}

function countUnreadApprovalNotif() {
    $.ajax({
        url: baseUrl + "Notification/CountUnreadApproval",
        method: "get",
        success: function (data) {
            let count = data;
            let unreadmsgcount = `(${count})`;

            $(".unread-notifapproval").empty().text(unreadmsgcount);
        }
    });
}

async function readNotification(id) {
    await $.ajax({
        url: baseUrl + "Notification/UpdateNotifToReads",
        method: "put",
        data: {
            notifIds: id
        },
        success: function () { }
    });
}

//#region Soft Delete

//trash mode
async function archiveNotification(id) {
    await $.ajax({
        url: "/Notification/ArchiveNotif",
        method: "put",
        data: {
            notifIds: id
        },
        success: function () {
            loadNotif(currentNotifPage, notiftype);
        }
    })
}

async function archiveAllNotification() {
    await $.ajax({
        url: baseUrl + "Notification/ArchiveAllNotif",
        method: "put",
        success: function () {
            countUnreadNotif();
            loadNotif(currentNotifPage, notiftype);
        }
    })
}
//#endregion

//#region Hard Delete
async function deleteNotification(id) {
    await $.ajax({
        url: "/Notification/DeleteNotification",
        method: "Delete",
        data: {
            notifIds: id
        },
        success: function () {
            loadNotif(currentNotifPage, notiftype);
        }
    });
}

async function deleteAllNotification() {
    await $.ajax({
        url: baseUrl + "Notification/DeleteAllNotifications",
        method: "DELETE", // Correct the method to uppercase
        success: async function () { // Use async here to use await inside
            await countUnreadNotif(); // Assuming countUnreadNotif is asynchronous
            await loadNotif(currentNotifPage, notiftype); // Assuming loadNotif is asynchronous
        }
    });
}

//#endregion

//#endregion

//connection.invoke('GetServerTime');

//let interval = 1000;

//let myvar = setInterval(function () {
//    connection.invoke('GetServerTime');
//}, interval);

//connection.on("RetServerTime", function (datetime) {
//    $('.clock_app').text(convertDate(datetime, "MM/DD/YYYY HH:mm:ss"));
//});

//onlineUserCount.on("UpdateOnlineUser", function (count) {
//    //tbl_user.ajax.relaod();
//    console.log(count);
//});