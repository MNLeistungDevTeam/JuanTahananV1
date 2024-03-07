"use strict"

const companyId = $("[name='Company.Id']").val();
var _subCompanyId = 0;
var _acctngPeriodFrom = "";
var _acctngPeriodTo = "";
var resources = [];

$(function () {
    loadNatureOfBusinesses();
    loadCompanyProfile(companyId);
    rebindValidators();

    //#region Initialization

    $("[name='Setting.AccountingPeriod']").selectize();
    $("[name='Setting.InvEvalMethodId']").selectize();
    $("[name='Setting.Bir2307Basis']").selectize();
    $("[name='Company.MobileNo']").inputmask("(999) 999-9999");
    $("[name='Company.TelNo']").inputmask({ mask: "9999-9999", greedy: false });
    $("[name='Company.FaxNo']").inputmask({ mask: "(999) 999-9999", greedy: false });
    $("[name='Company.Tin'], [name='Company.RepresentativeTin']").inputmask({ mask: "999-999-999-9999" });

    $("[name='Company.Email']").inputmask({ alias: "email" });
    $("[name='Company.Website']").inputmask({ alias: "url" });

    $(".flatpickr-month").flatpickr({
        //enableTime: false,
        defaultDate: moment().format("MMMM"),
        allowInput: false,
        plugins: [
            new monthSelectPlugin({
                shorthand: true, //defaults to false
                dateFormat: "F", //defaults to "F Y"
                altFormat: "F", //defaults to "F Y"
                theme: "dark" // defaults to "light"
            })
        ]
    });

    $("select[name='Setting.AccountingPeriod']").on("change", function () {
        toggleAcctngPeriod();
    });

    //#endregion

    //#region Events
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
    function toggleAcctngPeriod() {
        var isFiscal = $("select[name='Setting.AccountingPeriod']").val() == "Fiscal Year";
        $(".acctngPeriodDiv").attr({ hidden: !isFiscal });

        if (!isFiscal) {
            $("input[name='Setting.AcctgPeriodFrom']").val(moment().startOf('year').format("MMMM"));
            $("input[name='Setting.AcctgPeriodTo']").val(moment().endOf('year').format("MMMM"));
        } else {
            $("input[name='Setting.AcctgPeriodFrom']").val("");
            $("input[name='Setting.AcctgPeriodTo']").val("");
        }
    }

    async function loadCompanyProfile(id) {
        if (id == 0) return;

        await loadCompanyTransactCount(id);

        //Company Profile
        var companyInfo = await getCompanyInfo(id);
        var companyAddresses = await getCompanyAddress(id);
        var companyLogos = await getCompanyLogos(id);
        var companySettings = await getCompanySettings(id);

        $("input[name='Company.Id']").val(id);
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

        // Company Settings
        if (companySettings != null) {
            $("input[name='Setting.id']").val(companySettings.Id);
            $("input[name='Setting.PostingPeriod']").val(companySettings.PostingPeriod);
            $("input[name='Setting.TransactionSeriesCount']").val(companySettings.TransactionSeriesCount)
            $("input[name='Setting.AcctgPeriodFrom']").val(convertDate(companySettings.AcctgPeriodFrom, "MMMM"));
            $("input[name='Setting.AcctgPeriodTo']").val(convertDate(companySettings.AcctgPeriodTo, "MMMM"));
        }

        // Company Address
        $(".address_div").empty();

        console.log(companyAddresses);

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
        //$("input[name='Company.Id']").val("");
        $("input[name='Company.Name']").val("");
        $("#txt_company_name").html("");

        $("input[name='Company.Code']").val("");
        $("input[name='Company.BusinessStyle']").val("");
        $("#txt_business_style").html("");
        $("input[name='Company.Tin']").val("");

        $("#txt_tin").html("");

        $("input[name='Company.TelNo']").val("");
        $("#txt_tel_no").html("");

        $("input[name='Company.MobileNo']").val("");
        $("#txt_mobile_no").html("");

        $("input[name='Company.FaxNo']").val("");

        $("input[name='Company.Email']").val("");
        $("#txt_email").html("");

        $("input[name='Company.Website']").val("");
        $("#txt_website").html("");

        $("input[name='Company.RepresentativeName']").val("");
        $("input[name='Company.RepresentativeDesignation']").val("");

        $("#txt_rep_name").html("");
        $("#txt_address").html("");

        $("input[name='Company.RepresentativeTin']").val("");
        $("input[name='Setting.Id']").val(0);
        $("input[name='Setting.PostingPeriod']").val("");
        $("input[name='Setting.TransactionSeriesCount']").val("")
        $("input[name='Setting.AcctgPeriodFrom']").val("");
        $("input[name='Setting.AcctgPeriodTo']").val("");

        $(".logo_div").empty();
        $(".address_div").empty();

        $("input[name='Setting.AcctgPeriodTo']").val("");

        var infoTab = document.querySelector('a[href="#info-tab"]');

        infoTab.click();
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

            var isValid = lengthValidator();

            if (isValid == false) return;

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
                    resetForm();

                    button.attr({ disabled: false });
                    button.html("<span class='mdi mdi-content-save-outline me-1'></span> Save Changes");
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

    function loadCompanyTransactCount(Id) {
        let totalTransaction = $("#txt_total_transaction");
        let totalPending = $("#txt_total_pending");

        $.ajax({
            url: baseUrl + "Company/GetCompanyInfo/" + Id,
            method: "Get",
            success: function (response) {
                totalTransaction.html(response.TotalTransaction);
                totalPending.html(response.TotalPending);
            }
        });
    }

    function addLogo2(logoObj = {}) {
        let count = $(".logo_row").length;
        let defaultLogo = $("#Layout-defaultLogo").html();
        let actualLogo = "";
        let companyLogo = logoObj.Location || "";

        let rowToAdd = `<div class="col-md-6 logo_row mb-2">
                            <div class="p-2">
                                <div class="row">
                                    <div class="col-12 border p-2">
                                        <input type="hidden" id="CompanyLogo_Id_[${count}]" data-val="true" data-val-required="The Id field is required." name="CompanyLogo[${count}].Id" value="${logoObj.Id || 0}">
                                        <input type="hidden" id="CompanyLogo_CompanyId_[${count}]" name="CompanyLogo[${count}].CompanyId" value="${logoObj.CompanyId || 0}">
                                        <button type="button" class="btn-close btn-sm float-end remove_logo_file" aria-label="Close"></button>
                                    </div>
                                    <div class="col-12 border border-top-0 border-bottom-0 p-2">
                                        <div class="avatar-upload-rectangle" data-toggle="tooltip" title="Company Logo [${count}]">
                                            <div class="avatar-edit-rectangle input_div">
                                                <input id="CompanyLogo_CompanyLogoFile_[${count}]" accept=".png, .jpg, .jpeg" type="file" name="CompanyLogo[${count}].CompanyLogoFile">
                                                <label for="CompanyLogo_CompanyLogoFile_[${count}]"></label>
                                                <span class="text-danger field-validation-valid" data-valmsg-for="CompanyLogo[${count}].CompanyLogoFile" data-valmsg-replace="true"></span>
                                            </div>
                                            <div class="avatar-preview-rectangle">
                                                <div id="imagePreview_[${count}]"></div>
                                            </div>
                                        </div>
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

        if (isNullOrWhitespace(companyLogo)) actualLogo = defaultLogo;
        else actualLogo = companyLogo;

        loadImageOnPreview(baseUrl + actualLogo, defaultLogo, `[id='imagePreview_[${count}]']`);
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

    function lengthValidator() {
        var isValid = true;
        var elements = [
            { name: 'Company.MobileNo', requiredLength: 10, message: "Mobile number" },
            { name: 'Company.TelNo', requiredLength: 8, message: "Telephone number" },
            { name: 'Company.FaxNo', requiredLength: 10, message: "Fax number" },
            { name: 'Company.Tin', requiredLength: 13, message: "TIN number" },
            { name: 'Company.RepresentativeTin', requiredLength: 13, message: "Company Representative TIN number" }
        ];

        elements.forEach(function (element) {
            var inputValue = $("[name='" + element.name + "']").inputmask('unmaskedvalue'); // Get the unmasked value
            var alphanumericLength = inputValue.replace(/[^a-zA-Z0-9]/g, '').length; // Count only alphanumeric characters
            if (alphanumericLength !== element.requiredLength) {
                $("[data-valmsg-for='" + element.name + "']").text(element.message + " length should be " + element.requiredLength + " characters.");
                isValid = false;
            } else {
                $("[data-valmsg-for='" + element.name + "']").text("");
                isValid = isValid != false ? true : false;
            }
        });

        return isValid;
    }

    function resourceCounter(item) {
        if (resources.indexOf(item) == -1)
            resources.push(item);

        if (resources.length != 1) return
    }

    //#endregion
});