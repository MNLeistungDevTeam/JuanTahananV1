"use strict"

$(function () {
    let $form = $("#frm_notif");
    let $btn_save = $("#btn_save");
    let tbl_announcement;
    let quillcontent;
    let receiverDropdown, $receiverDropdown;

    //$(document).on("keyup", "#Content", function () {
    //    var notifContent = $("#Content").text().length;
    //    if (notifContent <= 50)
    //    $("#Notif_Preview").val($("#Content").text());

    //});
    $(document).on("keyup", "#Content", function () {
        const notifContent = $(this).text().substring(0, 50).trim();

        $("#Notif_Preview").val(notifContent);
    });


    //$(document).on("load", "#Sampletest2", function () {
    //    $("#Sampletest2").removeClass('valid');
    //});


    //#region ReceiverDropdown

    $receiverDropdown = $('#Receiver').selectize({
        valueField: 'Id',
        labelField: 'Name',
        searchField: 'Name',
        options: [],
        create: false,
        loadThrottle: 500,
        preload: true,
        placeholder: 'Select an option...',
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + "Notification/GetRoles",
                success: function (data) {
                    callback(data);
                },
                error: function () {
                    callback();
                }
            });
        },
        render: {
            item: function (item, escape) {
                return ("<div class='text-truncate' style='max-width:95%;'>" +
                    escape(item.Name) +
                    "</div>");
            },
            option: function (item, escape) {
                return (
                    "<div class='py-1 px-2'>" +
                    escape(item.Name) +
                    "</div>"
                );
            }
        }
    });

    receiverDropdown = $receiverDropdown[0].selectize;

    $("#Type").change(function () {
        updateSelectizeOptions();
    });

    // update options on demand
    function updateSelectizeOptions() {
        let type = $("#Type").val();
        let link = "";

        if (type == 1)
            link = baseUrl + "Notification/GetRoles";

        else if (type == 2)
            link = baseUrl + "Account/GetUsers";
        receiverDropdown.clear();

        receiverDropdown.settings.load = function (query, callback) {
            $.ajax({
                url: link,
                success: function (data) {
                    callback(data);
                },
                error: function () {
                    callback();
                }
            });
        }

        receiverDropdown.clearOptions();
    }
    //#endregion

    quillcontent = new Quill('#Content', {
        modules: {
            toolbar: [
                [{ font: [] }, { size: [] }],
                ["bold", "italic", "underline", "strike"],
                [{ color: [] }, { background: [] }],
                //[{ script: "super" }, { script: "sub" }],
                //[{ header: [!1, 1, 2, 3, 4, 5, 6] }, "blockquote", "code-block"],
                [{ list: "ordered" }, { list: "bullet" }, { indent: "-1" }, { indent: "+1" }],
                ["direction", { align: [] }],
                ["link"],
                ["clean"],
            ],
        },
        theme: 'snow'
    });


    $form.submit(function (e) {
        e.preventDefault();

        let formData = new FormData(e.target);
        let messagecontent = $("#Content").find(".ql-editor").html();

        let button = $btn_save;

        if ($form.valid() == false) {
            messageBox("Please fill out all required fields!", "danger", true);
            return;
        }

        formData.append('Notif.Content', messagecontent);
        $.ajax({
            url: $form.attr("action"),
            method: $form.attr("method"),
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function () {
                button.html("<span class='spinner-border spinner-border-sm'></span> Saving...");
                button.attr({ disabled: true });
            },
            success: function (response) {
                button.attr({ disabled: false });
                button.html("<span class='fa fa-save'></span> Save");
                tbl_announcement.ajax.reload();
                ResetNotifForm();
            },
            error: function (response) {
                messageBox(response.responseText, "danger");
                button.html("<span class='fa fa-save'></span> Save");
                button.attr({ disabled: false });
            }
        });
    });

    function ResetNotifForm() {
        $("#Notif_Title").val("");
        $("#Notif_Content").val("");
        $("#Notif_Preview").val("");
        $("#Notif_ActionLink").val("");
        $("#Receiver")[0].selectize.setValue("0");
        quillcontent.setText("");
    }

    tbl_announcement = $("#tbl_announcement").DataTable({
        ajax: {
            url: baseUrl + "Notification/GetAnnouncement",
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
                data: "Id",
                class: "text-start",
                render: function (data, type, row) {
                    let id = `<small>${data}</small>`;
                    return id;
                }
            },
            {
                data: "Title",
                class: "text-start",
                render: function (data, type, row) {
                    let title = `<span class="teaser text-black">${data}</span>`
                    return title;
                }
            },
            {
                data: "Preview",
                class: "text-start"
                /* render: $.fn.dataTable.render.ellipsis(80, true)*/
                //render: function (data, type, row) {
                //    let Id = row.Id;
                //    let str = data;
                //    var cleaned = stripTags(str);

                //    let content =`<a class="subject text-black text-truncate" style = "max-width: 350px;" data-id="${Id}">${truncateddata}</a>`
                //    return content;
                //}
            },

        ],
        drawCallback: function () {
            $(".dataTables_paginate > .pagination").addClass("pagination-rounded")
        },

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
        pageLength: 10,
        stateSave: true,
        searchHighlight: true
    });

    tbl_announcement.on('select deselect draw', function () {
        var all = tbl_announcement.rows({ search: 'applied' }).count();
        var selectedRows = tbl_announcement.rows({ selected: true, search: 'applied' }).count();

        $("#btn_edit").attr({
            "disabled": !(selectedRows >= 1)
        });
    });

    $("#tbl_announcement_filter").hide();
    $("#tbl_announcement_length").hide();

    $(document).on("click", "#btn_edit", function () {
        var id = tbl_announcement.rows({ selected: true }).data().pluck("Id").toArray().toString();
        $("#defaulttext").text("");
        loadAnnouncement(id);
    });

    $(document).on("click", "#btn_clear", function () {
        ResetNotifForm();
    });

    function loadAnnouncement(id) {
        $.ajax({
            url: baseUrl + "Notification/GetAnnouncementById/" + id,
            method: "get",
            success: function (response) {
                $("#Notif_Title").val(response.Title);
                $("#Content").find(".ql-editor").html(response.Content);
            }
        });
    }

    //#region reserve

    $('.read-notif').click(function (e) {
        let id = $(this).attr('data-id');
        let isread = $(this).attr('data-val');
        if (isread == 0)
            UpdateisRead(id);
        e.stopPropagation();
        if (isread == 1)
            UpdateUnRead(id);
        e.stopPropagation();
    });

    //#endregion
});