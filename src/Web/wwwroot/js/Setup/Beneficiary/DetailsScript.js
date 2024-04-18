const pagibigNumber = $('#txt_applicantCode').val();


$(function () {

    console.log(pagibigNumber);

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
            {
                data: 'Stage',
                orderable: !0,
                className: 'align-middle text-center'
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

});