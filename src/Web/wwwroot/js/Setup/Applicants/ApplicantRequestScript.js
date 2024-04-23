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
                className: 'align-middle',
                render: function (data, type, row) {
                    return `<a href="${baseUrl}Applicants/Details/${data}" target="_blank">${data}</a>`;
                }
            },

            {
                data: 'ApplicantFullName',
                className: 'align-middle'
            },

            {
                data: 'PagibigNumber',
                className: 'align-middle'
            },
            {
                data: 'HousingAccountNumber',
                className: 'align-middle text-center',
                visible: false
            },

            {
                data: 'IncomeAmount',
                className: 'align-middle text-end',
                visible: false
            },
            {
                data: 'Developer',
                className: 'align-middle'
            },
            {
                data: 'ProjectLocation',
                className: 'align-middle'
            },

            {
                data: 'Unit',
                className: 'align-middle'
            },
            {
                data: 'LoanAmount',
                className: 'align-middle text-end',
                render: function (data, type, row) {
                    let loanAmount = numeral(data).format('0,0.00');
                    return `₱${loanAmount}`;
                }
            },

            {
                data: 'DateSubmitted',
                className: 'align-middle text-center',
                render: function (data) {
                    var dateApplied = "";

                    if (data && data.trim() !== "") {
                        let submDate = moment(data).format('YYYY-MM-DD');
                        let submTime = moment(data).format('h:mm A');

                        dateApplied = `${submDate} at ${submTime}`;
                    }

                    else {
                        dateApplied = "";
                    }

                    return dateApplied;
                }
            },

            {
                data: 'Stage',
                className: 'align-middle',
                render: function (data, type, row) {
                    var returndata = "";

                    if ([0, 1, 2, 3, 5, 11].includes(row.ApprovalStatusNumber)) {
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
                className: 'align-middle',
                render: function (data, type, row) {
                    var returndata = "";

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

                    else if (row.ApprovalStatusNumber == 11) { // Discontinued
                        returndata = ` <span class="badge fs-6 border bg-teal">${data}</span> `;
                    }

                    return returndata;
                }
            },
            {
                data: 'LastUpdated',
                className: 'align-middle text-center',
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
        var encodedStatus = tbl_applicants.rows({ selected: true }).data().pluck("EncodedStatus").toArray().toString();

        $("#btn_add").attr({
            "disabled": !(selectedRows === 0),
            "data-url": baseUrl + "Applicants/HLF068"
        });

        $("#btn_view").attr({
            "disabled": !(selectedRows === 1),
            "data-url": baseUrl + "Applicants/Details/" + applicationCode
        });

        $("#btn_generate_pdf").attr({
            "disabled": !(selectedRows === 1),
            "data-url": baseUrl + "Report/LatestHousingForm/" + applicationCode
        });

        //$("#btn_edit").attr({
        //    "disabled": selectedRows !== 1 || (applicationStatus === 2 || applicationStatus === 0),
        //    "data-url": baseUrl + "Applicants/HLF068/" + applicationCode
        //});

        //$("#btn_upload_document").attr({
        //    "disabled": selectedRows !== 1 || (applicationStatus === 2 || applicationStatus === 0),
        //    "data-url": baseUrl + "Document/DocumentUpload/" + applicationCode
        //});

        if (selectedRows == 1 && applicationStatus == 0) {  //application draft
            $("#btn_edit").attr({
                "disabled": false,
                "data-url": baseUrl + "Applicants/HLF068/" + applicationCode
            });
        }
        else if (selectedRows == 1 && applicationStatus == 11) { //for resubmission
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
        else if (selectedRows == 1 && applicationStatus == 11) { //for resubmission
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