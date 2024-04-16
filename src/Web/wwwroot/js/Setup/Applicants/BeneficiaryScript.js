"use strict"
$(function () {
    loadMyApplicationInfo();

    //#region Initialization
    var tbl_applications = $("#tbl_applications").DataTable({
        ajax: {
            url: '/Applicants/GetMyApplications',
            method: 'GET',
            dataSrc: "",
        },
        columns: [

            {
                data: 'Code',
                className: '',
                render: function (data, type, row) {
                    return `<a href="${baseUrl}Applicants/Details/${data}" target="_blank">${data}</a>`;
                }
            },

            {
                data: 'ApplicantFullName',
                className: ''
            },

            {
                data: 'PagibigNumber',
                className: 'text-end'
            },
            {
                data: 'HousingAccountNumber',
                className: 'text-center',
                visible: false
            },

            {
                data: 'IncomeAmount',
                className: 'text-end',
                visible: false
            },
            {
                data: 'Developer',
                className: ''
            },
            {
                data: 'ProjectLocation',
                className: ''
            },

            {
                data: 'Unit',
                className: ''
            },
            {
                data: 'LoanAmount',
                className: 'text-end',
                render: function (data, type, row) {

                    let loanAmount = numeral(data).format('0,0.00');
                    return `₱${loanAmount}`;
                }

            },

            {
                data: 'DateSubmitted',
                className: 'text-center',
                render: function (data) {
                    if (data && data.trim() !== "") {
                        return moment(data).format('YYYY-MM-DD');
                    } else {
                        return "";
                    }
                }
            },

            
            {
                data: 'Stage',
                className: '',
                render: function (data, type, row) {
                    var returndata;

                    console.log(data);
                    if ([0, 1, 2, 3, 5].includes(row.ApprovalStatusNumber)) {
                        // `Credit Verification`
                        returndata = `<span class="text-orange">${data}</span>`;
                    }
                    else if ([4, 6, 7, 9, 10].includes(row.ApprovalStatusNumber)) {
                        // `Application Completion`
                        returndata = `<span class="text-primary">${data}</span>`;
                    }
                    else if (row.ApprovalStatusNumber === 8) {
                        // `Post-Approval`
                        returndata = `<span class="text-success">${data}</span>`;
                    }

                    return returndata;
                }
            },

            {
                data: 'ApplicationStatus',
                className: '',
                render: function (data, type, row) {
                    var returndata = "";

                    console.log(row.ApprovalStatusNumber);

                    if (row.ApprovalStatusNumber == 0) { // draft
                        returndata = ` <span class="badge fs-6 border bg-secondary">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 1) { // submitted
                        returndata = ` <span class="badge fs-6 border bg-primary">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 2) { // withdrawn
                        returndata = ` <span class="badge fs-6 border bg-danger">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 3) { // DeveloperVerified
                        returndata = ` <span class="badge fs-6 border bg-lightgreen">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 4) { // PagibigVerified
                        returndata = ` <span class="badge fs-6 border bg-darkgreen">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 5) { // Withdrawn
                        returndata = ` <span class="badge fs-6 border bg-warning">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 6) { // PostSubmitted
                        returndata = ` <span class="badge fs-6 border bg-primary">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 7) { // DeveloperConfirmed
                        returndata = ` <span class="badge fs-6 border bg-lightgreen">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 8) { // PagibigConfirmed
                        returndata = ` <span class="badge fs-6 border bg-darkgreen">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 9) { // Disqualified
                        returndata = ` <span class="badge fs-6 border bg-danger">${data}</span> `;
                    }
                    else if (row.ApprovalStatusNumber == 10) { // Discontinued
                        returndata = ` <span class="badge fs-6 border bg-warning">${data}</span> `;
                    }

                    return returndata;
                }
            },
            {
                data: 'LastUpdated',
                className: 'text-center',
                render: function (data, type, row) {

                    if (data && data.trim() !== "") {
                        return moment(data).format('YYYY-MM-DD HH:mm');

                    } else {
                        return "";
                    }
                }
            },
        ],

        drawCallback: function () {
            $(".dataTables_paginate > .pagination").addClass("pagination-rounded"),
                $('li.paginate_button.page-item.active > a').addClass('waves-effect')

            loadMyApplicationInfo();
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

    tbl_applications.on('select deselect draw', function () {
        var all = tbl_applications.rows({ search: 'applied' }).count();
        var selectedRows = tbl_applications.rows({ selected: true, search: 'applied' }).count();
        var id = tbl_applications.rows({ selected: true }).data().pluck("Id").toArray().toString();
        var applicationCode = tbl_applications.rows({ selected: true }).data().pluck("Code").toArray().toString();
        var applicationStatus = tbl_applications.rows({ selected: true }).data().pluck("ApprovalStatus").toArray().toString();

        $("#btn_add").attr({
            "disabled": !(selectedRows === 0),
        });

        if (selectedRows == 1 && applicationStatus == 0) {  //application draft
            $("#btn_edit").attr({
                "disabled": false,
                "data-url": baseUrl + "Applicants/HLF068/" + applicationCode
            });
        }

        else {
            $("#btn_edit").attr({
                "disabled": true,
                "data-url": baseUrl + "Applicants/HLF068/" + applicationCode
            });
        }

        if (selectedRows == 1 && applicationStatus == 0) {  //application draft
            $("#btn_upload_document").attr({
                "disabled": false,
                "data-url": baseUrl + "Document/DocumentUpload/" + applicationCode
            });
        }
        else if (selectedRows == 1 && applicationStatus == 4) { //pagibig verified
            $("#btn_upload_document").attr({
                "disabled": false,
                "data-url": baseUrl + "Document/DocumentUpload/" + applicationCode
            });
        }

        else {
            $("#btn_upload_document").attr({
                "disabled": true,
                "data-url": baseUrl + "Document/DocumentUpload/" + applicationCode
            });
        }

        $("#btn_view").attr({
            "disabled": !(selectedRows === 1),
            "data-url": baseUrl + "Applicants/Details/" + applicationCode
        });

        $("#btn_generate_pdf").attr({
            "disabled": !(selectedRows === 1),
            "data-url": baseUrl + "Report/LatestHousingForm/" + applicationCode
        });
    });

    //#endregion

    //#region Events
    $("#btn_add").on('click', function () {
        let pagibigNumber = $(this).attr("data-pagibignumber");

        location.href = baseUrl + "Applicants/NewHLF068/" + pagibigNumber;
    });

    $("#btn_upload_document").on('click', function () {
        location.href = $(this).attr("data-url");
    });

    $("#btn_edit").on('click', function () {
        location.href = $(this).attr("data-url");
    });

    $("#btn_view").on('click', function () {
        location.href = $(this).attr("data-url");
    });

    $("#btn_generate_pdf").on('click', function () {
        location.href = $(this).attr("data-url");
    });

    $("#btn_refresh").on('click', function () {
        tbl_applications.ajax.reload();
    });

    //#endregion

    function loadMyApplicationInfo() {
        let info_total_approved = $("#info_total_approved");
        let info_total_disapproved = $("#info_total_disapproved");
        let info_for_approval = $("#info_for_approval");
        let info_total_returned = $("#info_total_returned");
        let loading_text = "<span class='spinner-border spinner-border-sm'></span>";

        $.ajax({
            url: "/Applicants/GetBeneficiaryApprovalTotalInfo",
            beforeSend: function () {
                info_total_approved.html(loading_text);
                info_total_disapproved.html(loading_text);
                info_for_approval.html(loading_text);
            },
            success: function (response) {
                if (response) {
                    info_for_approval.html(`<span data-plugin="counterup">${response.length > 0 ? response[0].TotalPendingReview : 0}</span>`);
                    info_total_approved.html(`<span data-plugin="counterup">${response.length > 0 ? response[0].TotalApprove : 0}</span>`);
                    info_total_disapproved.html(`<span data-plugin="counterup">${response.length > 0 ? response[0].TotalDisApprove : 0}</span>`);
                }

                $("[data-plugin='counterup']").counterUp();
            }
        });
    }
});