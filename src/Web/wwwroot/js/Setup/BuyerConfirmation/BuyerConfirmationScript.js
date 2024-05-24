"use strict"

$(function () {
    var tblBuyerConForm;

    const CONST_MODULE = "BuyerConfirmation  Requests";
    const CONST_MODULE_CODE = "BCF-UPLOAD";
    const CONST_TRANSACTIONID = $("#BuyerConfirmation_Id").val();

    initializeTable();
    initializeInfoCards();

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

                        if (row.ApprovalStatus == 0) { // submitted
                            returndata = ` <span class="badge fs-6 border bg-primary">${data}</span> `;
                        }

                        else if (row.ApprovalStatus == 3 && row.BuyerConfirmationDocumentStatus == 1) { // sign && submitted
                            returndata = ` <span class="badge fs-6 border bg-primary">${data}</span> `;
                        }
                        else if (row.ApprovalStatus == 3 && row.BuyerConfirmationDocumentStatus == 11) { // For Resubmission
                            returndata = ` <span class="badge fs-6 border bg-teal">${data}</span> `;
                        }
                        else if (row.ApprovalStatus == 3 && row.BuyerConfirmationDocumentStatus == 3) { // Approved
                            returndata = ` <span class="badge fs-6 border bg-lightgreen">${data}</span> `;
                        }
                        else if (row.ApprovalStatus == 11) { // For Revision
                            returndata = ` <span class="badge fs-6 border bg-teal">${data}</span>`;
                        }
                        else if (row.ApprovalStatus == 3) { // ready for printing
                            returndata = ` <span class="badge fs-6 border bg-primary">${data}</span> `;
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
            var fileName = tblBuyerConForm.rows({ selected: true }).data().pluck("FileName").toArray().toString();
            var fileLocation = tblBuyerConForm.rows({ selected: true }).data().pluck("FileLocation").toArray().toString();
            var buyerconfirmationdocumentId = tblBuyerConForm.rows({ selected: true }).data().pluck("BuyerConfirmationDocumentId").toArray().toString();
            var bcfDocumentStatus = tblBuyerConForm.rows({ selected: true }).data().pluck("BuyerConfirmationDocumentStatus").toArray().toString();

            $("#btn_view").attr({
                "disabled": !(selectedRows === 1),
                "data-url": baseUrl + "BuyerConfirmation/Details/" + bcfCode
            });

            $("#btn_document").attr({
                "disabled": !(selectedRows === 1 && fileName),
                "data-fileName": fileName,
                "data-fileLocation": fileLocation,
                "data-documentId": buyerconfirmationdocumentId,
                "data-referenceNo": bcfCode,
                "data-documentStatus": bcfDocumentStatus,
            });
        });

        $("#btn_refresh").on('click', function () {
            tblBuyerConForm.ajax.reload();
        });

        $("#btn_view").on('click', function () {
            location.href = $(this).attr("data-url");
        });

        $("#btn_document").on('click', function () {
            openDocumentModal();
        });
    }

    function openDocumentModal() {
        let document = $("#btn_document");
        let documentId = document.attr('data-documentId');
        let documentFileName = document.attr('data-fileName');
        let documentFileLocation = document.attr('data-fileLocation');
        let bcfCode = document.attr('data-referenceNo');
        let bcfDocStatus = document.attr('data-documentStatus');

        const itemLink = documentFileLocation + documentFileName;

        if (bcfDocStatus == 3) { $("#btnApprove").addClass('d-none'); }

        else { $("#btnApprove").removeClass('d-none'); }

        $("#dl_bcfdocument").attr("href", itemLink).attr("target", "_blank").text(documentFileName);
        $("#btnApprove").attr("data-documentId", documentId);
        $("#btnReturn").attr("data-documentId", documentId);

        loadApprovalData(documentId, bcfCode);

        $("#document-modal").modal('show');
    }

    $("#btnApprove,#btnReturn").on('click', async function (e) {
        e.preventDefault();
        let action = $(this).attr("data-value");

        openApprovalModal(action);
    });

    $("#btn_excelSummary").on('click', function (e) {
        e.preventDefault();
        window.location.href = baseUrl + "BuyerConfirmation/BCFRequests/BCFSummary";

        //$.ajax({
        //    url: baseUrl + "BuyerConfirmation/BCFSummary",
        //    type: "GET",
        //    success: function (data) {
        //        var blob = new Blob([data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });

        //        var link = document.createElement("a");
        //        link.href = window.URL.createObjectURL(blob);
        //        link.download = "BCFSummary.xlsx";
        //        link.click();

        //        window.URL.revokeObjectURL(link.href);
        //        messageBox("Summary report has been downloaded.", "success", true);
        //    },
        //    error: function (xhr, status, error) {
        //        messageBox(error, "danger", true);
        //    }
        //});
    });

    async function loadApprovalData(recordId, referenceNo) {
        /* let recordId = $("#BuyerConfirmationModel_Id").val();*/
        const approvalData = await getApprovalData(recordId);

        if (!approvalData) { return; }

        $("[name='ApprovalLevel.Id']").val(approvalData.ApprovalLevelId ?? 0);
        $("[name='ApprovalLevel.ApprovalStatusId']").val(approvalData.Id ?? 0);
        $("[name='ApprovalLevel.ModuleCode']").val(CONST_MODULE_CODE);
        $("[name='ApprovalLevel.TransactionId']").val(recordId);
    }

    async function getApprovalData(referenceId) {
        const response = await $.ajax({
            url: baseUrl + `Approval/GetByReferenceModuleCodeAsync/${referenceId}/${CONST_MODULE_CODE}`,
            method: "get",
            dataType: 'json'
        });

        console.log(response);
        return response;
    }

    function openApprovalModal(action) {
        let $approverModal = $('#approver-modal');
        let modalLabel = $("#approver-modalLabel");
        let transactionNo = $("#btn_document").attr('data-referenceNo');
        let remarksInput = $('[name="ApprovalLevel.Remarks"]');
        let roleName = $("#txt_role_code").val();
        let $btnSave = $("#btn_save");

        $btnSave.removeClass();
        $('.text-danger.validation-summary-errors').removeClass('validation-summary-errors').addClass('validation-summary-valid')
            .find('li').css('display', 'none');
        remarksInput.removeAttr("data-val-required").removeClass("input-validation-error").addClass("valid");

        if (action == 1) {      //submitted
            modalLabel.html('<span class="fe-send"></span> Submit Application');
            $btnSave.addClass("btn btn-primary").html('<span class="fe-send"></span> Submit')

            remarksInput.removeAttr("data-val-required").removeClass("input-validation-error").addClass("valid");
        }

        else if (action == 11) {
            modalLabel.html('<span class="fe-repeat"></span> Return for Revision BCF');
            $btnSave.addClass("btn btn-warning").html('<span class="fe-repeat"></span> Return for Revision')
            //remarksInput.attr("data-val-required", "true").attr("required", true).addClass("input-validation-error").addClass("invalid");
            remarksInput.attr("required", true);
        }
        else {
            modalLabel.html('<span class="fe-check-circle"></span> Approve BCF');
            $btnSave.addClass("btn btn-success").html('<span class="fe-check-circle"></span> Approve')
            remarksInput.removeAttr("data-val-required").attr("required", false).removeClass("input-validation-error").addClass("valid");
        }

        $("#author_txt").html(`Author: ${roleName}`);
        $("[name='ApprovalLevel.Status']").val(action);
        $("[name='ApprovalLevel.TransactionNo']").val(transactionNo);

        rebindValidator();
        $approverModal.modal("show");
    }

    function rebindValidator() {
        let $form = $("#frm_approver_level");
        let $approverModal = $('#approver-modal');
        let $documentModal = $("#document-modal");

        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate($form.data("unobtrusiveValidation").options);
        $form.data("validator").settings.ignore = "";

        $form.submit(function (e) {
            e.preventDefault();

            if (!$form.valid()) { return; }

            let formData = $form.serialize();
            formData = formData.replace(/ApprovalLevel\./g, "");

            let approvalLevelStatus = $("#ApprovalLevel_Status").val();

            let action = "";
            let text = "";
            let confirmButtonText = "";

            if (approvalLevelStatus == 3 || approvalLevelStatus == 4) {
                action = "Approve";
                text = "Are you sure you wish to proceed with approving this buyer confirmation application?";
                confirmButtonText = "save and approve";
            }

            else if (approvalLevelStatus == 11) {
                action = "Resubmission";
                text = "Are you sure you wish to proceed with for-resubmission this buyer confirmation application?";
                confirmButtonText = " for-resubmission";
            }

            // Use SweetAlert for confirmation
            Swal.fire({
                title: `${action} Application`,
                text: text,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: `Yes, ${confirmButtonText} it`,
                cancelButtonText: 'No, cancel'
            }).then((result) => {
                if (result.isConfirmed) {
                    // User confirmed, proceed with form submission
                    $.ajax({
                        url: $form.attr("action"),
                        method: $form.attr("method"),
                        data: formData,
                        beforeSend: function () {
                            $("#beneficiary-overlay").removeClass('d-none');

                            // $("#btnApprove").attr({ disabled: true });
                        },
                        success: function (response) {
                            //$("#beneficiary-overlay").addClass('d-none');

                            messageBox("Successfully saved.", "success");

                            $("#btnApprove").attr({ disabled: false });

                            $approverModal.modal("hide");
                            $documentModal.modal("hide");
                            tblBuyerConForm.ajax.reload();
                        },
                        error: function (response) {
                            // Error message handling
                            $("#btnApprove").attr({ disabled: false });

                            messageBox(response.responseText, "danger", true);
                        }
                    });
                }
            });
        });
    }

    function initializeInfoCards() {
        let info_for_qualification = $("#info_for_qualification");
        let info_total_qualified = $("#info_total_qualified");
        let info_total_returned = $("#info_total_returned");
        let loading_text = "<span class='spinner-border spinner-border-sm'></span>";

        $.ajax({
            url: "/BuyerConfirmation/GetBCFInquiry",
            method: "get",
            beforeSend: function () {
                info_for_qualification.html(loading_text);
                info_total_qualified.html(loading_text);
                info_total_returned.html(loading_text);
            },
            success: function (response) {
                if (!response)
                    return

                console.log(response);

                info_for_qualification.html(`<span data-plugin="counterup">${response.TotalSubmitted}</span>`);
                info_total_qualified.html(`<span data-plugin="counterup">${response.TotalQualified}</span>`);
                info_total_returned.html(`<span data-plugin="counterup">${response.TotalReturned}</span>`);

                $("[data-plugin='counterup']").counterUp();
            },
            error: function (jqXHR) {
                info_for_qualification.html(`<span data-plugin="counterup">${0}</span>`);
                info_total_qualified.html(`<span data-plugin="counterup">${0}</span>`);
                info_total_returned.html(`<span data-plugin="counterup">${0}</span>`);
            }
        });
    }
});