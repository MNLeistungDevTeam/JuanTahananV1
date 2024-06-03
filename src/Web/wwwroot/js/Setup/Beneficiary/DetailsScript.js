const pagibigNumber = $('#txt_applicantCode').val();

$(function () {
    console.log(pagibigNumber);

    loadBeneficiaryDetailsAge();

    var tbl_applicants = $("#tbl_applicants").DataTable({
        ajax: {
            url: baseUrl + 'Applicants/GetAllApplicationByPagibigNum/' + pagibigNumber,
            dataSrc: "",
        },
        columns: [

            {
                data: 'Code',
                orderable: !0,
                className: 'align-middle text-center',
                render: function (data, type, row) {
                    return `<a href="${baseUrl}Applicants/Details/${data}" target="_blank">${data}</a>`;
                }
            },

            {
                data: 'ApplicantFullName',
                orderable: !0,
                className: 'align-middle text-center',
                visible: false
            },

            {
                data: 'PagibigNumber',
                orderable: !0,
                className: 'align-middle text-center',
                visible: false
            },
            {
                data: 'HousingAccountNumber',
                orderable: !0,
                className: 'align-middle text-center',
                visible: false
            },

            {
                data: 'Developer',
                orderable: !0,
                className: 'align-middle text-center'
            },
            {
                data: 'ProjectLocation',
                orderable: !0,
                className: 'align-middle text-center'
            },

            {
                data: 'Unit',
                orderable: !0,
                className: 'align-middle text-center'
            },
            {
                data: 'LoanAmount',
                orderable: !0,
                className: 'align-middle text-center',
                render: function (data, type, row) {
                    // Parse LoanAmount as float and format it with commas and two decimal places
                    var formattedAmount = parseFloat(data).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                    // Add peso sign before the formatted value
                    return '₱' + formattedAmount;
                }
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
                    else if (row.ApprovalStatusNumber == 11) { // return
                        returndata = ` <span class="badge fs-6 border bg-teal">${data}</span> `;
                    }

                    return returndata;
                }
            },
            {
                data: 'Stage',
                orderable: !0,
                className: 'align-middle text-center',
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

        ],
        drawCallback: function () {
            $(".dataTables_paginate > .pagination").addClass("pagination-rounded"),
                $('li.paginate_button.page-item.active > a').addClass('waves-effect')
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

    function loadBeneficiaryDetailsAge() {
        // Get the birthday string
        var birthdayStr = $('#birthday-profile').text();

        // Parse the birthday string to a Date object
        var birthday = new Date(birthdayStr);

        // Get the current date
        var currentDate = new Date();

        // Calculate the difference in years
        var age = currentDate.getFullYear() - birthday.getFullYear();

        // Adjust age if the birthday hasn't occurred yet this year
        if (currentDate.getMonth() < birthday.getMonth() || (currentDate.getMonth() === birthday.getMonth() && currentDate.getDate() < birthday.getDate())) {
            age--;
        }

        $('#age-profile').text(age);
    }
});