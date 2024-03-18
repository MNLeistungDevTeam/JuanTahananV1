"use strict"
$(function () {
    loadApplicantTotalInfo

    var tbl_applicants = $("#tbl_applicants").DataTable({
        ajax: {
            url: '/Applicants/GetApplicants',
            method: 'GET',
            dataSrc: "",
        },
        columns: [

            {
                data: 'ApplicantFullName',
                orderable: !0,
                className: 'align-middle text-center'
            },

            {
                data: 'Code',
                orderable: !0,
                className: 'align-middle text-center',
                render: function (data, type, row) {
                    return `<a href="${baseUrl}Applicant/Details/${data}" target="_blank">${data}</a>`;
                }
            },
            {
                data: 'HousingAccountNumber',
                orderable: !0,
                className: 'align-middle text-center'
            },
            {
                data: 'PagibigNumber',
                orderable: !0,
                className: 'align-middle text-center'
            },

            {
                data: 'DateCreated',
                orderable: !0,
                className: 'align-middle text-center',
                render: function (data) {
                    if (data && data.trim() !== "") {
                        return moment(data).format('YYYY-MM-DD');
                    } else {
                        return "";
                    }
                }
            },
            {
                data: 'ApplicationStatus',
                orderable: !0,
                className: 'align-middle text-center',
                render: function (data) {
                    return `
                        <span class="badge fs-6 border bg-secondary">${data}</span>
                        `;
                }
            },

        ],
        drawCallback: function () {
            $(".dataTables_paginate > .pagination").addClass("pagination-rounded"),
                $('li.paginate_button.page-item.active > a').addClass('waves-effect')

            loadApplicantTotalInfo();
        },
        language: {
            "zeroRecords": "No Records Found....",
            loadingRecords: "Records loading...",
            emptyTable: `No Records Found....`,
            infoEmpty: "No entries to show",
            paginate: { previous: "<i class='mdi mdi-chevron-left'>", next: "<i class='mdi mdi-chevron-right'>" },
            info: `Showing Records _START_ to _END_ of _TOTAL_`
        },
        select: true,
        scrollY: '24rem',
        scrollX: true,
        order: [[1, "desc"]],
        pageLength: 10,
        searchHighlight: true,
        stateSave: false,
        bLengthChange: false,
        dom: 'lrtip',
        processing: true
    });

    tbl_applicants.on('select deselect draw', function () {
        var all = tbl_applicants.rows({ search: 'applied' }).count();
        var selectedRows = tbl_applicants.rows({ selected: true, search: 'applied' }).count();
        var id = tbl_applicants.rows({ selected: true }).data().pluck("Id").toArray().toString();
        var applicationCode = tbl_applicants.rows({ selected: true }).data().pluck("Code").toArray().toString();

        $("#btn_add").attr({
            "disabled": !(selectedRows === 0),
            "data-url": baseUrl + "Applicants/HLF068"
        });

        $("#btn_edit").attr({
            "disabled": !(selectedRows === 1),
            "data-url": baseUrl + "Applicants/HLF068/" + applicationCode
        });

        $("#btn_view").attr({
            "disabled": !(selectedRows === 1),
            "data-url": baseUrl + "Applicants/Details/" + applicationCode
        });
    });

    $("#btn_add").on('click', function () {
        location.href = $(this).attr("data-url");
    });

    $("#btn_edit").on('click', function () {
        location.href = $(this).attr("data-url");
    });

    $("#btn_view").on('click', function () {
        location.href = $(this).attr("data-url");
    });
    $("#btn_refresh").on('click', function () {
        tbl_applicants.ajax.reload();
    });

    function loadApplicantTotalInfo() {
        let info_total_approved = $("#info_total_approved");
        let info_total_disapproved = $("#info_total_disapproved");
        let info_for_approval = $("#info_for_approval");
        let info_total_returned = $("#info_total_returned");
        let loading_text = "<span class='spinner-border spinner-border-sm'></span>";

        $.ajax({
            url: "/Applicants/GetApprovalTotalInfo",
            beforeSend: function () {
                info_total_approved.html(loading_text);
                info_total_disapproved.html(loading_text);
                info_for_approval.html(loading_text);
            },
            success: function (response) {
                info_for_approval.html(`<span data-plugin="counterup">${response[0].TotalPendingReview}</span>`);
                info_total_approved.html(`<span data-plugin="counterup">${response[0].TotalApprove}</span>`);
                info_total_disapproved.html(`<span data-plugin="counterup">${response[0].TotalDisApprove}</span>`);

                $("[data-plugin='counterup']").counterUp();
            }
        });
    }
});