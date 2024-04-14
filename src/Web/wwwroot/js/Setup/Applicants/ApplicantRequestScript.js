"use strict"
$(function () {
    loadApplicantTotalInfo();

    //#region Initialization
    var tbl_applicants = $("#tbl_applicants").DataTable({
        ajax: {
            url: '/Applicants/GetApplicants',
            method: 'GET',
            dataSrc: "",
        },
        columns: [

            {
                data: 'Code',
                className: 'align-middle text-center',
                render: function (data, type, row) {
                    return `<a href="${baseUrl}Applicants/Details/${data}" target="_blank">${data}</a>`;
                }
            },

            {
                data: 'ApplicantFullName',
                className: 'align-middle text-center'
            },

            {
                data: 'PagibigNumber',
                className: 'align-middle text-center'
            },
            {
                data: 'HousingAccountNumber',
                className: 'align-middle text-center',
                visible: false
            },

            {
                data: 'IncomeAmount',
                className: 'align-middle text-center',
                visible: false
            },
            {
                data: 'Developer',
                className: 'align-middle text-center'
            },
            {
                data: 'ProjectLocation',
                className: 'align-middle text-center'
            },

            {
                data: 'Unit',
                className: 'align-middle text-center'
            },
            {
                data: 'LoanAmount',
                className: 'align-middle text-center'
            },

            {
                data: 'DateSubmitted',
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
                className: 'align-middle text-center',
                render: function (data, type,row) {
                    var returndata = "";

                    console.log(row.ApprovalStatusNumber);

                    if (row.ApprovalStatusNumber == 0) {
                        returndata = ` <span class="badge fs-6 border bg-secondary">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 1) {
                        returndata = ` <span class="badge fs-6 border bg-primary">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 2) {
                        returndata = ` <span class="badge fs-6 border bg-danger">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 3) {
                        returndata = ` <span class="badge fs-6 border bg-lightgreen">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 4) {
                        returndata = ` <span class="badge fs-6 border bg-darkgreen">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 5) {
                        returndata = ` <span class="badge fs-6 border bg-warning">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 6) {
                        returndata = ` <span class="badge fs-6 border bg-primary">${data}</span> `;
                    }

                    else if (row.ApprovalStatusNumber == 7) {
                        returndata = ` <span class="badge fs-6 border bg-lightgreen">${data}</span> `;
                    }

                    else if (row.ApprovalStatusNumber == 8) {
                        returndata = ` <span class="badge fs-6 border bg-darkgreen">${data}</span> `;
                    }

                    else if (row.ApprovalStatusNumber == 9) {
                        returndata = ` <span class="badge fs-6 border bg-secondary">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 10) {
                        returndata = ` <span class="badge fs-6 border bg-danger">${data}</span> `;
                    }

                    return returndata;
                }
            },

            {
                data: 'Stage',
                className: 'align-middle text-center'
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
        orderable: false,
        sort: false,
        //order: [[1, "desc"]],
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
        var applicationStatus = tbl_applicants.rows({ selected: true }).data().pluck("ApprovalStatus").toArray().toString();

        $("#btn_add").attr({
            "disabled": !(selectedRows === 0),
            "data-url": baseUrl + "Applicants/HLF068"
        });

        $("#btn_view").attr({
            "disabled": !(selectedRows === 1),
            "data-url": baseUrl + "Applicants/Details/" + applicationCode
        });

        $("#btn_edit").attr({
            "disabled": selectedRows !== 1 || (applicationStatus === 2 || applicationStatus === 0),
            "data-url": baseUrl + "Applicants/HLF068/" + applicationCode
        });

        $("#btn_generate_pdf").attr({
            "disabled": !(selectedRows === 1),
            "data-url": baseUrl + "Report/LatestHousingForm/" + applicationCode
        });

        $("#btn_upload_document").attr({
            "disabled": selectedRows !== 1 || (applicationStatus === 2 || applicationStatus === 0),
            "data-url": baseUrl + "Document/DocumentUpload/" + applicationCode
        });
    });

    //#endregion

    //#region Events
    $("#btn_add").on('click', function () {
        location.href = $(this).attr("data-url");
    });

    $("#btn_edit").on('click', function () {
        location.href = $(this).attr("data-url");
    });

    $("#btn_view").on('click', function () {
        location.href = $(this).attr("data-url");
    });

    $("#btn_upload_document").on('click', function () {
        location.href = $(this).attr("data-url");
    });

    $("#btn_generate_pdf").on('click', function () {
        location.href = $(this).attr("data-url");
    });

    $("#btn_refresh").on('click', function () {
        tbl_applicants.ajax.reload();
    });

    //#endregion

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
                if (response) {
                    info_for_approval.html(`<span data-plugin="counterup">${response.length > 0 ? response[0].TotalSubmitted : 0}</span>`);
                    info_total_approved.html(`<span data-plugin="counterup">${response.length > 0 ? response[0].TotalApprove : 0}</span>`);
                    info_total_disapproved.html(`<span data-plugin="counterup">${response.length > 0 ? response[0].TotalDisApprove : 0}</span>`);
                }

                $("[data-plugin='counterup']").counterUp();
            }, error: function (jqXHR) {
                info_for_approval.html(`<span data-plugin="counterup">0</span>`);
                info_total_approved.html(`<span data-plugin="counterup">0</span>`);
                info_total_disapproved.html(`<span data-plugin="counterup">0</span>`);
            }
        });
    }
});