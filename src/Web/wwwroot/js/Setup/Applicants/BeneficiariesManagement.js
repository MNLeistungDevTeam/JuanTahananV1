$(() => {
    var tbl_beneficiaries = $("#tbl_beneficiaries").DataTable({
        ajax: {
            url: '/Applicants/GetUsersByRoleName',
            method: 'GET',
            dataSrc: "",
            data: {
                roleName: 'Beneficiary'
            }
        },
        columns: [
            {
                data: null,
                orderable: true,
                className: 'ps-2',
                render: function (data) {
                    return `
                                    <div class="d-flex align-items-center">
                                        <img src="${data.ProfilePicture == '' ? '/images/user/default.png' : "/images/user/" + data.ProfilePicture}" class="rounded-circle avatar-sm img-thumbnail me-3" alt="profile">
                                        <div>
                                            <div>${data.Name}</div>
                                            <a href="${data.Email}" class="text-decoration-none">${data.Email}</a>
                                        </div>
                                    </div>
                                `;
                }
            },
            {
                data: 'TotalLoanCounts',
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
                data: 'StatusOnline',
                orderable: !0,
                className: 'align-middle text-center',
                render: function (data) {
                    return `
                        <span class="badge fs-6 border bg-secondary">${data}</span>
                        `;
                }
            },
            {
                data: null,
                orderable: !0,
                className: 'align-middle text-center',
                visible: false,
                render: function (data) {
                    let applicantCode = data.ApplicantCode != null ? `/${data.ApplicantCode}` : "";
                    return `
                        <div class="dropdown btn-group">
                            <button class="btn btn-light waves-effect" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <i class="mdi mdi-dots-horizontal"></i>
                            </button>
                            <div class="dropdown-menu dropdown-menu-animated" style="">
                                ${data.TotalLoanCounts != 0 ? `<a class="dropdown-item" href="/Document/DocumentUpload${applicantCode}">Upload Document</a>` : ''}
                                <a class="dropdown-item" href="/Applicants/HLF068${applicantCode}">Housing Application</a>
                                ${data.TotalLoanCounts != 0 ? `<a class="dropdown-item" href="/Report/LatestHousingForm${applicantCode}">View PDF</a>` : ''}
                            </div>
                        </div>
                    `;
                }
            }
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
        scrollY: '24rem',
        select: true,
        scrollX: true,
        order: [[2, "desc"]],
        pageLength: 10,
        searchHighlight: true,
        stateSave: false,
        bLengthChange: false,
        dom: 'lrtip',
        processing: true
    });

    tbl_beneficiaries.on('select deselect draw', function () {
        var all = tbl_beneficiaries.rows({ search: 'applied' }).count();
        var selectedRows = tbl_beneficiaries.rows({ selected: true, search: 'applied' }).count();
        var id = tbl_beneficiaries.rows({ selected: true }).data().pluck("Id").toArray().toString();
        var applicationCode = tbl_beneficiaries.rows({ selected: true }).data().pluck("Code").toArray().toString();
        var applicationStatus = tbl_beneficiaries.rows({ selected: true }).data().pluck("ApprovalStatus").toArray().toString();
        var pagibigNumber = tbl_beneficiaries.rows({ selected: true }).data().pluck("PagibigNumber").toArray().toString();

        $("#btn_create").attr({
            "disabled": (selectedRows !== 1),
            "data-url": baseUrl + "Applicants/NewHLF068/" + pagibigNumber
        });

        $("#btn_edit").attr({
            "disabled": (selectedRows !== 1),
            "data-url": baseUrl + "Beneficiary/BeneficiaryInformation/" + pagibigNumber
        });



        $("#btn_add").attr({
            "data-url": baseUrl + "Beneficiary/BeneficiaryInformation"
        });



    });

    $("#btn_add").on('click', function () {
        let link = $(this).attr("data-url");

        location.href = link;


       
    });

    $("#btn_edit").on('click', function () {
        let link = $(this).attr("data-url");

        location.href = link;
    });

    $("#btn_create").on('click', function () {
        let link = $(this).attr("data-url");

        location.href = link;
    });

    $("#btn_refresh").on('click', function () {
        tbl_beneficiaries.ajax.reload();
    });

    //tbl_beneficiaries.on('click', 'tbody tr', function () {
    //    var data = tbl_beneficiaries.row(this).data();
    //    if (data) {
    //        var id = data.Id;
    //        location.href = '/Applicants/HLF068?UserId=' + id;
    //    }
    //});
})