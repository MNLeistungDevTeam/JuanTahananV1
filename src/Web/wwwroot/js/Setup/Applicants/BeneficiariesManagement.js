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
                visible: false,
                render: function (data) {
                    return `
                        <span class="badge fs-6 border bg-secondary">${data}</span>
                        `;
                }
            },

            {
                data: 'ActiveApplicationCode',
                orderable: !0,
                className: 'align-middle text-center',
                visible: true,
                render: function (data, type, row) {
                    let applicantCode = data != "" ? `/${data}` : "";
                    //console.log(data);
                    //console.log(row.ActiveApplicationCode);
                    return `
                        <div class="dropdown btn-group">
                            <button class="btn btn-light waves-effect" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <i class="mdi mdi-dots-horizontal"></i>
                            </button>
                            <div class="dropdown-menu dropdown-menu-animated" style="">
                                ${data != null ? `<a class="dropdown-item" href="/Document/DocumentUpload${applicantCode}">Upload Document</a>` : ''}
                                 ${data != null ? `   <a class="dropdown-item" href="/Applicants/HLF068${applicantCode}">Edit Active HLAF</a>` : ''}
                                    ${data != null ? `   <a class="dropdown-item" href="/Applicants/Details${applicantCode}">View Application Details</a>` : ''}
                                 ${data == null ? `  <a class="dropdown-item" href="/Applicants/NEWHLF068/${row.PagibigNumber}">Create New HLAF</a>` : ''}
                                ${data != null ? `<a class="dropdown-item" href="/Report/LatestHousingForm${applicantCode}">View PDF Version</a>` : ''}
                            </div>
                        </div>
                    `;
                }
            },

            {
                data: 'PagibigNumber',
                orderable: !0,
                className: 'align-middle text-center',
                visible: false,
                render: function (data) {
                    return data;
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

        $("#btn_view").attr({
            "disabled": !(selectedRows === 1),
            "data-url": baseUrl + "Beneficiary/Details/" + pagibigNumber
        });

        //$("#btn_create").attr({
        //    "disabled": (selectedRows !== 1),
        //    "data-url": baseUrl + "Applicants/HLF068/"
        //});

        $("#btn_edit").attr({
            "disabled": (selectedRows !== 1),
            "data-url": baseUrl + "Beneficiary/BeneficiaryInformation/" + pagibigNumber
        });

        $("#btn_add").attr({
            "data-url": baseUrl + "Beneficiary/BeneficiaryInformation"
        });
    });

    $("#btn_view").on('click', function () {
        location.href = $(this).attr("data-url");
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
        let link = baseUrl + "Applicants/HLF068";

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