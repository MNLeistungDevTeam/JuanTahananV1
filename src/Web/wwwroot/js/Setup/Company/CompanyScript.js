var resourceCount = 0;

$(function () {
    //#region Initialization
    $("[name='Company.AccountingPeriod']").selectize();
    $("[name='Company.InvEvalMethodId']").selectize();
    $("[name='Company.MobileNo']").inputmask("(999) 999-9999");
    $("[name='Company.TelNo']").inputmask({ mask: "9999-9999", greedy: true });
    $("[name='Company.FaxNo']").inputmask({ mask: "(999) 999-9999", greedy: true });
    $("[name='Company.Tin'], [name='Company.RepresentativeTin']").inputmask({ mask: "999-999-999-9999", placeholder: "", greedy: true });
    $("[name='Company.Email']").inputmask({ alias: "email" });
    $("[name='Company.Website']").inputmask({ alias: "url" });

    rebindValidators();

    var tbl_company = $("#tbl_company").DataTable({
        ajax: {
            url: baseUrl + "Company/GetCompanies",
            dataSrc: ""
        },
        language: {
            processing: "<div class='text-center'><span class='spinner-border spinner-border-sm'></span> Loading...</div>",
            previous: "<i class='mdi mdi-chevron-left'>",
            next: "<i class='mdi mdi-chevron-right'>"
        },
        columns: [
            {
                data: "Name"
            },
            {
                data: "Code"
            },
            {
                data: "BusinessStyle"
            },
            {
                data: "Tin"
            },
            {
                data: "TelNo"
            },
            {
                data: "Email"
            },
            {
                data: "Website",
                render: function (data, type, row) {
                    return data != null ? `<a href='${data}' target='_blank'>${data}</a>` : '';
                }
            }
        ],
        drawCallback: function () {
            $(".dataTables_paginate > .pagination").addClass("pagination-rounded")
        },
        rowId: "Id",
        processing: true,
        select: true,
        paging: false,
        scrollX: true,
        scrollY: "50vh",
        scrollCollapse: true,
        searchHighlight: true
    });

    $("#tbl_company_filter, #tbl_company_length").hide();

    //#endregion

    //#region Events
    tbl_company.on('select deselect draw', function () {
        var all = tbl_company.rows({ search: 'applied' }).count();
        var selectedRows = tbl_company.rows({ selected: true, search: 'applied' }).count();
        var Id = tbl_company.rows({ selected: true }).data().pluck("Id").toArray().toString();
        var Name = tbl_company.rows({ selected: true }).data().pluck("Name").toArray().toString();

        $("#btn_edit").attr({
            "disabled": !(selectedRows === 1),
            "data-id": Id
        });

        $("#btn_copy").attr({
            "disabled": !(selectedRows === 1),
            "data-id": Id
        });

        $("#btn_delete").attr({
            "disabled": !(selectedRows >= 1),
            "data-id": Id,
            "data-desc": Name
        })
    });

    $("#btn_refresh").on("click", function () {
        tbl_company.ajax.reload();
    });

    $("#txt_search").on('keyup, input', function () {
        tbl_company.search(this.value).draw();
    })

    $("#btn_delete").on("click", function () {
        var companyIds = $(this).attr("data-id");
        var companies = $(this).attr("data-desc");

        Swal.fire({
            title: 'Are you sure?',
            text: `The following record/s will be deleted: ${companies}`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showLoaderOnConfirm: true,
            preConfirm: (login) => {
                return fetch(`${baseUrl}Company/DeleteCompanies/`,
                    {
                        method: "DELETE",
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        body: `companyIds=${companyIds}`
                    })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error(response.statusText)
                        }
                        return response;
                    })
                    .catch(error => {
                        Swal.showValidationMessage(
                            `Request failed: ${error}`
                        )
                    })
            },
            allowOutsideClick: () => !Swal.isLoading()
        }).then((result) => {
            if (result.isConfirmed) {
                messageBox("Record(s) successfully deleted.", "success");
                tbl_company.ajax.reload(null, false)
            }
        })
    })

    $("#btn_add").on("click", async function () {
        await openCompanyModal();
    });

    $("#btn_edit").on("click", async function () {
        let id = $(this).attr("data-id");
        await openCompanyModal(id);
    });

    $("#btn_copy").on("click", async function () {
        await openCompanyModal($(this).attr("data-id"), "copy");
    });

    $(".btn_add_logo").on("click", function () {
        var description = $(this).data("desc");

        addLogo({
            Description: description,
            DisableEditDesc: true
        });
        //fixElementSequence(".logo_row");
    });

    $("#btn_add_address").on("click", function () {
        addAddress();
        //fixElementSequence(".address_row");
        rebindValidators();
    });

    $(document).on("input change", "[id^='CompanyLogo_CompanyLogoFile_']", function () {
        let id = $(this).attr("id");
        let idNum = id.substring(id.indexOf("[") + 1, id.indexOf("]"));
        let container = $(`[id='imagePreview_[${idNum}]']`);

        readUrl(this, container);
    });

    $(document).on("click", ".remove_logo_file", function () {
        $(this).closest(".logo_row").remove();
        checkaddLogoButton();
        fixElementSequence(".logo_row");
    });

    $(document).on("click", ".remove_address", function () {
        $(this).closest(".address_row").remove();
        fixElementSequence(".address_row");
    });

    $(document).on("change", ".address_checkbox", function () {
        var isChecked = $(this).prop("checked");

        if (isChecked) {
            $(".address_checkbox").each(function (index) {
                $(this).prop("checked", false);
            });
        }

        $(this).prop("checked", isChecked);
    });

    //#endregion

    //#region Methods

    async function openCompanyModal(id = 0, action = "") {
        // start on Company Info
        //var link = $('a[href="#company-info"]');
        //link.addClass('active');


        let modal = $("#company_modal");
        let modalLabel = $("#company-modalLabel");
        let modalOverlay = $("#company-modal-overlay");

        resetForm();

        if (id != 0) {
            if (action == "copy") {
                $("input[name='Company.Id']").val(0);
                modalLabel.html('<span class="fe-plus"></span> Add Company');
            } else {
                $("input[name='Company.Id']").val(id);
                modalLabel.html('<span class="fas fa-edit"></span> Update Company');
            }

            modalOverlay.attr({ hidden: false });
            var companyInfo = await getCompanyInfo(id);
            var companyAddresses = await getCompanyAddress(id);
            var companyLogos = await getCompanyLogos(id);

            $("input[name='Company.Name']").val(companyInfo.Name);
            $("input[name='Company.Code']").val(companyInfo.Code);
            $("input[name='Company.BusinessStyle']").val(companyInfo.BusinessStyle);
            $("input[name='Company.Tin']").val(companyInfo.Tin);
            $("input[name='Company.TelNo']").val(companyInfo.TelNo);
            $("input[name='Company.MobileNo']").val(companyInfo.MobileNo);
            $("input[name='Company.FaxNo']").val(companyInfo.FaxNo);
            $("input[name='Company.Email']").val(companyInfo.Email);
            $("input[name='Company.Website']").val(companyInfo.Website);
            $("input[name='Company.RepresentativeName']").val(companyInfo.RepresentativeName);
            $("input[name='Company.RepresentativeTin']").val(companyInfo.RepresentativeTin);

            // Company Address
            $(".address_div").empty();
            if (typeof (companyAddresses) == 'object' && companyAddresses.length > 0) {
                for (var companyAddress of companyAddresses) {
                    addAddress(companyAddress, true)
                }
            } else {
                addAddress();
            }

            // Company Logo
            $(".logo_div").empty();

            if (typeof (companyLogos) == 'object' && companyLogos.length > 0) {
                for (var companyLogo of companyLogos) {
                    addLogo(companyLogo);

                    if (companyLogo.Description == "ReportLogo") {
                        var location = companyLogo.Location;

                        await verifyLogoExists(location)
                    }
                }
            } else {
                addLogo({
                    Description: "ReportLogo",
                    DisableEditDesc: true
                });
            }

            rebindValidators();
            modalOverlay.attr({ hidden: true });
        }

        modal.modal("show");
    }

    async function loadCompanyProfile(id = companyId) {
        if (id == 0) return;

        await loadCompanyInfoBox(id);
        resetForm();

        //Company Profile
        var companyInfo = await getCompanyInfo(id);
        var companyAddresses = await getCompanyAddress(id);
        var companyLogos = await getCompanyLogos(id);

        $("input[name='Company.id']").val(companyInfo.Id);
        $("input[name='Company.Name']").val(companyInfo.Name);
        $("#txt_company_name").html(companyInfo.Name ?? "N/A");

        $("input[name='Company.Code']").val(companyInfo.Code);
        $("input[name='Company.BusinessStyle']").val(companyInfo.BusinessStyle);
        $("#txt_business_style").html(companyInfo.BusinessStyle ?? "N/A");

        $("input[name='Company.Tin']").val(companyInfo.Tin);
        $("#txt_tin").html(companyInfo.Tin ?? "N/A");

        $("input[name='Company.TelNo']").val(companyInfo.TelNo);
        $("#txt_tel_no").html(companyInfo.TelNo ?? "N/A");

        $("input[name='Company.MobileNo']").val(companyInfo.MobileNo);
        $("#txt_mobile_no").html(companyInfo.MobileNo ?? "N/A");

        $("input[name='Company.FaxNo']").val(companyInfo.FaxNo);

        $("input[name='Company.Email']").val(companyInfo.Email);
        $("#txt_email").html(companyInfo.Email ?? "N/A");

        $("input[name='Company.Website']").val(companyInfo.Website);
        $("#txt_website").html(companyInfo.Website == null ? "N/A" : `<a href="${companyInfo.Website}">${companyInfo.Website}</a>`);

        $("input[name='Company.RepresentativeName']").val(companyInfo.RepresentativeName);
        $("#txt_rep_name").html(companyInfo.RepresentativeName ?? "N/A");
        $("#txt_address").html(companyInfo.Address.trim() == "" ? "N/A" : companyInfo.Address.trim());

        $("input[name='Company.RepresentativeTin']").val(companyInfo.RepresentativeTin);
        $("input[name='Setting.CompanyId']").val(companyInfo.Id);

        //// Company Settings
        //if (companySettings != null) {
        //    $("input[name='Setting.id']").val(companySettings.Id);
        //    $("input[name='Setting.PostingPeriod']").val(companySettings.PostingPeriod);
        //    $("input[name='Setting.TransactionSeriesCount']").val(companySettings.TransactionSeriesCount)
        //    $("input[name='Setting.AcctgPeriodFrom']").val(convertDate(companySettings.AcctgPeriodFrom, "MMMM"));
        //    $("input[name='Setting.AcctgPeriodTo']").val(convertDate(companySettings.AcctgPeriodTo, "MMMM"));
        //}

        // Company Address
        $(".address_div").empty();
        if (typeof (companyAddresses) == 'object' && companyAddresses.length > 0) {
            for (var companyAddress of companyAddresses) {
                addAddress(companyAddress, true)
            }
        } else {
            addAddress();
        }

        $("#pb_company_profile").attr("src", $("#Layout-defaultLogo").html());

        // Company Logo
        $(".logo_div").empty();

        if (typeof (companyLogos) == 'object' && companyLogos.length > 0) {
            for (var companyLogo of companyLogos) {
                addLogo(companyLogo);

                if (companyLogo.Description == "ReportLogo") {
                    var location = companyLogo.Location;

                    await verifyLogoExists(location)
                }
            }
        } else {
            addLogo({
                Description: "ReportLogo",
                DisableEditDesc: true
            });
        }

        rebindValidators();
    }

    function resetForm() {
        let modalLabel = $("#company-modalLabel");
        modalLabel.html('<span class="fe-plus"></span> Add Company');

        $("input[name='Company.Id']").val("0");
        $("input[name='Company.Name']").val("");
        $("input[name='Company.Code']").val("");
        $("input[name='Company.BusinessStyle']").val("");
        $("input[name='Company.Tin']").val("");
        $("input[name='Company.TelNo']").val("");
        $("input[name='Company.MobileNo']").val("");
        $("input[name='Company.FaxNo']").val("");
        $("input[name='Company.Email']").val("");
        $("input[name='Company.Website']").val("");
        $("input[name='Company.RepresentativeName']").val("");
        $("input[name='Company.RepresentativeTin']").val("");

        $(".logo_div").empty();
        $(".address_div").empty();
    }

    function rebindValidators() {
        let $form = $("#frm_company");
        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate($form.data("unobtrusiveValidation").options);
        $form.data("validator").settings.ignore = "";

        $form.submit(function (e) {
            e.preventDefault();

            if (!$(this).valid()) {
                messageBox("Please fill up all required fields.", "error", true);
                return;
            }

            if (!customValidateForm())
                return;

            fixElementSequence(".logo_row");
            fixElementSequence(".address_row");
            let formData = new FormData(e.target);
            let button = $("#btn_save");

            $.ajax({
                url: $(this).attr("action"),
                method: $(this).attr("method"),
                data: formData,
                contentType: false,
                cache: false,
                processData: false,
                beforeSend: function () {
                    button.html("<span class='spinner-border spinner-border-sm'></span> Saving...");
                    button.attr({ disabled: true });
                },
                success: function (response) {
                    let companyId = $("input[name='Company.Id']").val();
                    let successMessage = companyId == 0 ? "Company Successfully Added!" : "Company Successfully Updated!";
                    messageBox(successMessage, "success");
                    loadCompanyProfile(companyId);

                    tbl_company.ajax.reload(null, false)

                    button.attr({ disabled: false });
                    button.html("<span class='mdi mdi-content-save-outline me-1'></span> Save Changes");

                    $("#company_modal").modal('hide');

                },
                error: function (response) {
                    messageBox(response.responseText, "danger");
                    button.html("<span class='mdi mdi-content-save-outline me-1'></span> Save Changes");
                    button.attr({ disabled: false });
                }
            });
        })
    }

    function customValidateForm() {
        let toValidate = [];
        let message = "";
        let hasDefaultAddress = false;

        $(".address_checkbox").each(function (index) {
            hasDefaultAddress = !hasDefaultAddress ? $(this).prop("checked") : true;
        });

        if (!hasDefaultAddress) toValidate.push("No default address was set, please set one (1)");
        if ($(".address_row").length == 0) toValidate.push("Address should have atleast one (1) record.");

        for (var i = 0; i < toValidate.length; i++) {
            if (i == 0) {
                message += toValidate[i];
            } else {
                message += ", " + toValidate[i];
            }
        }

        if (toValidate.length == 0) {
            return true;
        } else {
            messageBox("Please fill up the following fields: " + message, "danger");
            console.log(message);
            return false;
        }
    }

    function addLogo(logoObj = {}) {
        let count = $(".logo_row").length;
        let defaultLogo = $("#Layout-defaultLogo").html();
        let actualLogo = "";
        let companyLogo = logoObj.Location || "";

        if (isNullOrWhitespace(companyLogo)) actualLogo = defaultLogo;
        else actualLogo = companyLogo;

        //if (!checkImageIfExists(actualLogo))
        //    actualLogo = defaultLogo;

        actualLogo = actualLogo.substring(1, actualLogo.length);

        let rowToAdd = `<div class="col-md-6 logo_row mb-2">
                            <div class="p-2">
                                <div class="row">
                                    <div class="col-12 border p-2">
                                        <input type="hidden" id="CompanyLogo_Id_[${count}]" data-val="true" data-val-required="The Id field is required." name="CompanyLogo[${count}].Id" value="${logoObj.Id || 0}">
                                        <input type="hidden" id="CompanyLogo_CompanyId_[${count}]" name="CompanyLogo[${count}].CompanyId" value="${logoObj.CompanyId || 0}">
                                        <button type="button" class="btn-close btn-sm float-end remove_logo_file" aria-label="Close"></button>
                                    </div>
                                    <div class="col-12 border border-top-0 border-bottom-0 p-2">
                                        <input id="CompanyLogo_CompanyLogoFile_[${count}]" type="file" name="CompanyLogo[${count}].CompanyLogoFile" class="">
                                    </div>
                                    <div class="col-12 border border-top-0 p-2 pt-0">
                                        <div class="form-group">
                                            <input id="CompanyLogo_Description_[${count}]" class="form-control" placeholder="Description" type="text" data-val="true" data-val-required="The Description field is required." name="CompanyLogo[${count}].Description" value="${logoObj.Description || ""}">
                                            <span class="text-danger field-validation-valid" data-valmsg-for="CompanyLogo[${count}].Description" data-valmsg-replace="true"></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>`;

        $(".logo_div").append(rowToAdd);

        $(`[id="CompanyLogo_Description_[${count}]"]`).attr({ readonly: logoObj.DisableEditDesc });

        $(`[id="CompanyLogo_CompanyLogoFile_[${count}]"]`).attr({ "data-default-file": baseUrl + actualLogo, "data-show-remove": false }).addClass("dropify")
        $(`[id="CompanyLogo_CompanyLogoFile_[${count}]"]`).dropify();
        checkaddLogoButton();
    }

    function checkaddLogoButton() {
        $(".btn_add_logo").attr({ hidden: false });

        $(`[id^="CompanyLogo_Description_["]`).each(function () {
            let logoDesc = $(this).val();

            $(".btn_add_logo").each(function () {
                var btn = $(this);

                if (btn.data("desc") == logoDesc) {
                    btn.attr({ hidden: true });
                }
            });
        });
    }

    function loadNatureOfBusinesses() {
        $.ajax({
            url: baseUrl + "NatureOfBusiness/GetAll",
            success: function (response) {
                var optionsToAdd = "";
                for (const item of response) {
                    optionsToAdd += `<option value='${item.Name}'></option>`;
                }

                $("#natureOfBusinesses").html(optionsToAdd);
            }
        });
    }

    async function verifyLogoExists(location) {
        try {
            const reponse = await $.ajax({
                type: "get",
                url: location
            });

            $("#pb_company_profile").attr("src", location);
        } catch (e) {
        }
    }

    async function getCompanyInfo(companyId) {
        const response = await $.ajax({
            url: baseUrl + "Company/GetCompany/" + companyId,
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    async function getCompanySettings(companyId) {
        const response = await $.ajax({
            url: baseUrl + "Company/GetCompanySettingByCompanyId?companyId=" + companyId,
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    async function getCompanyLogos(companyId) {
        const response = await $.ajax({
            url: baseUrl + "Company/GetCompanyLogos",
            data: {
                companyId: companyId
            },
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    async function getCompanyAddress(companyId) {
        const response = await $.ajax({
            url: baseUrl + "Company/GetCompanyAddresses",
            data: {
                companyId: companyId
            },
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    //#endregion
})