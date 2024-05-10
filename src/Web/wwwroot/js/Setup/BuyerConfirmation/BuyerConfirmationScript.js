"use strict"

$(function () {
    var tblBuyerConForm;

    initializeTable();

    function initializeTable() {
        tblBuyerConForm = $(`#tbl_applicants`).DataTable({
            ajax: {
                url: baseUrl + 'BuyerConfirmation/GetBcfList',
                method: 'GET',
                dataSrc: "",
            },
            columns: [
                {
                    data: 'Code',
                    className: 'align-middle',
                    render: function (data, type, row) {
                        return `<a href="${baseUrl}BuyerConfirmation/Details/${data}" target="_blank">${data}</a>`;
                    }
                },
                {
                    data: 'ApplicantFullName',
                    className: 'align-middle'
                },
                {
                    data: 'PagibigNumber',
                    className: 'align-middle',
                    defaultContent: "-----"
                },
                {
                    data: 'MonthlySalary',
                    className: 'align-middle text-end',
                    visible: false
                },
                {
                    data: 'ProjectProponentName',
                    className: 'align-middle'
                },
                {
                    data: 'HouseUnitModel',
                    className: 'align-middle'
                },
                {
                    data: 'ApplicationStatus',
                    className: 'align-middle',
                    render: function (data, type, row) {
                        var returndata = "";

                        if (row.ApprovalStatus == 0) { // draft
                            returndata = ` <span class="badge fs-6 border bg-secondary">${data}</span> `;
                        }
                        else if (row.ApprovalStatus == 3) { // DeveloperVerified
                            returndata = ` <span class="badge fs-6 border bg-lightgreen">${data}</span> `;
                        }
                        else if (row.ApprovalStatus == 11) { // return
                            returndata = ` <span class="badge fs-6 border bg-teal">${data}</span> `;
                        }

                        return returndata;
                    }
                },
                {
                    data: 'DateCreated',
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
                    data: 'DateModified',
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

                //loadApplicantTotalInfo();
            },
            language: {
                "zeroRecords": "No Records Found",
                loadingRecords: "Records loading...",
                emptyTable: `No Records Found`,
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

        tblBuyerConForm.on('select deselect draw', function () {
            //var all = tblBuyerConForm.rows({ search: 'applied' }).count();
            //var id = tblBuyerConForm.rows({ selected: true }).data().pluck("Id").toArray().toString();
            //var applicationStatus = tblBuyerConForm.rows({ selected: true }).data().pluck("ApprovalStatus").toArray().toString();
            //var encodedStatus = tblBuyerConForm.rows({ selected: true }).data().pluck("EncodedStatus").toArray().toString();
            var selectedRows = tblBuyerConForm.rows({ selected: true, search: 'applied' }).count();
            var bcfCode = tblBuyerConForm.rows({ selected: true }).data().pluck("Code").toArray().toString();

            $("#btn_view").attr({
                "disabled": !(selectedRows === 1),
                "data-url": baseUrl + "BuyerConfirmation/Details/" + bcfCode
            });
        });

        $("#btn_refresh").on('click', function () {
            tblBuyerConForm.ajax.reload();
        });

        $("#btn_view").on('click', function () {
            location.href = $(this).attr("data-url");
        });
    }
});