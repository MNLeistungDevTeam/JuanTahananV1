"use strict"

const baseUrl = $("#txtBaseUrl").val();
let localizedStrings;
loadLocalizedStrings();
var loader = null;
var applications = null;

const intlTelConfig = {
    showSelectedDialCode: false,
    showFlags: true,
    allowDropdown: false,
    strictMode: false,
    initialCountry: "ph",
    utilsScript: "/lib/intl-tel-input/build/js/utils.js"
};

// Bootstrap Color to Hex translator:
const bs5ColorDictionary = {
    danger: "#dc3545",
    warning: "#ffc107",
    success: "#28a745",
    primary: "#007bff",
    purple: "#6f42c1",
    info: "#17a2b8",
    secondary: "#6c757d"
};

const intlTelErrors = ["Invalid number", "Invalid country code", "Too short", "Too long", "Invalid number"];

function loadLocalizedStrings() {
    $.ajax({
        url: '/Localization/GetLocalizedStrings',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Store the localized strings in a JavaScript variable or object
            localizedStrings = data;
        },
        error: function (error) {
            console.error('Failed to fetch localized strings:', error);
        }
    });
}

function localizer(input) {
    return localizedStrings[input] ?? input;
}

$(function () {
    updateBeneficiaryHousingLoanSideBarNav();

    $.fn.clearValidation = function () { var v = $(this).validate(); $('[name]', this).each(function () { v.successList.push(this); v.showErrors(); }); v.resetForm(); v.reset(); };
    function adjustDataTablesColumns() {
        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
    }
    $(window).on('resize', adjustDataTablesColumns);

    $(document).on("shown.bs.modal shown.bs.tab shown.bs.collapse", function () {
        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
    });
    var targetElement = $('html');
    var observer = new MutationObserver(function (mutationsList, observer) {
        mutationsList.forEach(function (mutation) {
            if (mutation.type === 'attributes' && mutation.attributeName === 'data-sidenav-size') {
                setTimeout(function () {
                    $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
                }, 110);
            }
        });
    });
    var config = { attributes: true, attributeFilter: ['data-sidenav-size'] };
    observer.observe(targetElement[0], config);

    $(document).on("input change keypress", "select, input, textarea", function () {
        try { $(this).valid(); } catch (e) { }
    });

    $(document).on("change", ".decimalInputMask", debounce(function () {
        let amount = $(this).val();
        amount = Number(amount.replace(/[^-?0-9\.]+/g, ""));

        if (amount > 0)
            $(this).val(numeral(amount).format("0,0.00"));
    }, 800));
    function validateSelectizeInputs(form) {
        if (form.hasClass('was-validated')) {
            form.find('.selectized[data-val-required]').each(function () {
                var sibling_gate = $(this).siblings('.selectize-control');
                var children_gate = sibling_gate.find('.selectize-input');
                var error_gate = sibling_gate.siblings('.invalid-tooltip');

                error_gate.css('display', children_gate.hasClass('has-items') ? 'none' : 'block');
            });
        }
    }
    $(document).on('change', '[type="checkbox"]', function (e) {
        var isChecked = $(this).prop('checked');
        if (isChecked) {
            $(this).val(true);
        } else {
            $(this).val(false);
        }
    });
    $(document).on('change', '[type="radio"]', function (e) {
        var isChecked = $(this).prop('checked');
        if (isChecked) {
            $(this).val(true);
        } else {
            $(this).val(false);
        }
    });
    //var customizer = new ThemeCustomizer();

    //$('#light-dark-mode').on('click', function (e) {
    //    var currentTheme = $('html').attr("data-bs-theme");
    //    customizer.init();
    //    setThemeColor(currentTheme === 'light' ? 'dark' : 'light');
    //})
    //function setThemeColor(color = '') {
    //    var $icon = $('#light-dark-mode i');
    //    var defaultClass = 'ri-moon-line font-22';
    //    $icon.attr("class", "spinner-border spinner-border-sm");
    //    upDateFetchThemeColor(color, function (data) {
    //        $icon.attr("class", defaultClass);
    //        customizer.changeLayoutColor(data);
    //    });
    //}
    //function upDateFetchThemeColor(color, callback) {
    //    $.ajax({
    //        url: `/Home/UpdateThemeUserColor?color=${color}`,
    //        type: 'GET',
    //        success: callback
    //    })
    //}
    //setThemeColor();
    $('form.needs-validation').on('submit', function (event) {
        validateSelectizeInputs($(this));
    });

    $('form.needs-validation').on('change', '.selectized[data-val-required]', function () {
        validateSelectizeInputs($(this).closest('form'));
    });
    $(document).on('input', '[bound][type="search"]', function () {
        var tableId = $(`#${$(this).attr('bound')}`);
        if (tableId != null && $.fn.DataTable.isDataTable(tableId)) {
            var table = $(tableId).DataTable();
            table.search($(this).val()).draw();
        }
    });
    $(document).on('click', '.btn_refresh', function () {
        var tableId = $(`#${$(this).attr('bound')}`);
        if (tableId != null && $.fn.DataTable.isDataTable(tableId)) {
            var table = $(tableId).DataTable();
            table.ajax.reload();
        }
    });
    $('[data-val-length-max]').maxlength({
        warningClass: "badge bg-success",
        limitReachedClass: "badge bg-danger"
    })
    $('[maxlength]').maxlength({
        warningClass: "badge bg-success",
        limitReachedClass: "badge bg-danger"
    })
    Selectize.prototype._setValue = Selectize.prototype.setValue;
    Selectize.prototype._onChange = Selectize.prototype.onChange;
    Selectize.prototype._open = Selectize.prototype.open;

    $.extend(Selectize.prototype, {
        setValue: function (value, silent) {
            Selectize.prototype._setValue.apply(this, arguments);

            if (this.settings.mode === 'single')
                this.blur();
        },

        onChange: function () {
            Selectize.prototype._onChange.apply(this, arguments);

            if (this.settings.mode === 'single')
                this.blur();
        },

        open: function () {
            Selectize.prototype._open.apply(this, arguments);

            $('select')
                .not(this.$input)
                .toArray()
                .forEach(select => select.selectize?.blur());
        },
    });

    $('.account-tab-trigger').on('click', function (e) {
        var tab = $(this).data('tab');
        $('.account-tab-trigger').removeClass('active');
        $(this).addClass('active');
        $('[data-bs-toggle="tab"][href="' + tab + '"]').tab('show');
    })

    //if (document.querySelector('#totalApplications')) {
    //    getApplicationCount(function (callback) {
    //        const options = {
    //            useEasing: true,
    //            useGrouping: true,
    //            separator: ',',
    //            decimal: '.',
    //            suffix: ''
    //        };
    //        if (applications == null) {
    //            applications = new CountUp(`#totalApplications`, 0, callback, 0, 2, options);
    //            if (!applications.error) {
    //                applications.start();
    //            }
    //        } else {
    //            applications.update(callback);
    //        }
    //    })
    //}

    $(".decimalInputMask").inputmask({
        alias: 'decimal',
        rightAlign: false,
        groupSeparator: '.',
        digits: 2,
        allowMinus: false,
        autoGroup: true,
        placeholder: "0.00"
    });
});
function getApplicationCount(callback) {
    $.ajax({
        url: `/Home/GetApplicationsCount`,
        method: 'get',
        success: callback
    })
}
function messageBox(message, type = "success", isToastr = false, isTimed = true) {
    var title = "";

    switch (type) {
        case "danger": title = 'Alert!'; break;
        case "info": title = 'Information'; break;
        case "warning": title = 'Warning!'; break;
        case "success": title = 'Success!'; break;
        default:
    }

    if (type == "danger") type = "error";

    if (isToastr) {
        if (typeof (message) === "object") {
            console.log(message);
            toastr.error(message, title);
        } else {
            if (type == "danger" || type == "error")
                toastr.error(message, title);
            else if (type == "info")
                toastr.info(message, title);
            else if (type == "success")
                toastr.success(message, title);
            else
                toastr.warning(message, title);
        }
    }
    else if (isTimed) {
        if (typeof (message) === "object") {
            console.log(message);
            Swal.fire({
                icon: type,
                title: title,
                html: 'Error Occurred! - Please see error logs for more information',
                timer: 2000,
                timerProgressBar: true
            });
        } else
            Swal.fire({
                icon: type,
                title: title,
                html: message,
                timer: 2000,
                timerProgressBar: true
            });
    }
    else {
        if (typeof (message) === "object") {
            console.log(message);
            Swal.fire(title, 'Error Occurred! - Please see error logs for more information', type);
        } else Swal.fire(title, message, type);
    }
}

function iziToasterBox(message, type = 'success') {
    let head = "";
    let icons = "";
    let barColor = "";
    let bg = "";

    switch (type) {
        case "success":
            head = "Success";
            icons = `ico-success`;
            barColor = "#5ba035"
            bg = "#8fce00";
            break;
        case 'info':
            head = "Information"
            icons = "ico-info";
            barColor = "#3b98b5";
            bg = "#3d85c6";
            break;
        case 'warning':
            head = "Warning";
            icons = "ico-warning";
            barColor = "#da8609";
            bg = "#fc750a";
            break;
        case 'danger':
            head = "Error";
            icons = "ico-error";
            barColor = "#b83206";
            bg = "#FF0000";
        default:
    }

    iziToast.show({
        title: head,
        icon: icons,
        backgroundColor: bg,
        progressBarColor: barColor,
        position: 'topCenter',
        message: `<div style="max-width: 512px;" class="fw-bold">${message}</div>`,
        theme: 'light',
        close: true,
        timeout: 4000,
        transitionIn: 'fadeInDown',
        transitionOut: 'fadeOutUp',
    });
}

function convertDate(data, format = "YYYY-MM-DD") {
    let toReturn = "";

    if (data == "0001-01-01T00:00:00.000" || data == "0001-01-01T00:00:00" || data == null) {
        data = "";
    }

    if (moment(new Date(data)).isValid())
        toReturn = moment(new Date(data)).format(format);

    return toReturn;
}
function CheckRows(element) {
    var row2minimum = element.rows({ selected: true }).count();
    var currentElement = $(`#${$(element.table().node()).attr('id')}`);
    while (true) {
        if (currentElement.hasClass('card-body') || currentElement.hasClass('tab-pane') || currentElement.hasClass('modal-content')) {
            let btn_delete = currentElement.find('.btn_delete');
            let btn_edit = currentElement.find('.btn_edit');
            if (row2minimum > 1) {
                btn_delete.attr('disabled', false);
                btn_edit.attr('disabled', true);
            } else if (row2minimum === 1) {
                btn_delete.attr('disabled', false);
                btn_edit.attr('disabled', false);
            } else {
                btn_delete.attr('disabled', true);
                btn_edit.attr('disabled', true);
            }
            break;
        }
        currentElement = currentElement.parent();
    }
}
function debounce(func, wait, immediate) {
    var timeout;
    return function () {
        var context = this, args = arguments;
        var later = function () {
            timeout = null;
            if (!immediate) func.apply(context, args);
        };
        var callNow = immediate && !timeout;
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        if (callNow) func.apply(context, args);
    };
};

function convertToDictionaryArray(data) {
    var dictArray = [];
    for (var key in data) {
        if (data.hasOwnProperty(key)) {
            var item = {};
            item[key] = data[key];
            dictArray.push(item);
        }
    }
    return dictArray;
}
function clearForm(form, exceptions = []) {
    form.removeClass('was-validated');
    function isException(element) {
        return exceptions.some(selector => $(element).is(selector));
    }
    form.find('.input-validation-error').not(exceptions.join(', ')).removeClass('input-validation-error');
    form.find('input, select').not(exceptions.join(', ')).each(function () {
        if ($(this).hasClass('selectized')) {
            $(this)[0].selectize.clear();
        } else if ($(this).is(':checkbox')) {
            $(this).prop('checked', false);
        } else {
            $(this).val('');
        }
    });
}
function loading(message) {
    loader = Swal.fire({
        title: 'Please wait...',
        text: message,
        allowEscapeKey: false,
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
}

function updateProgress(progress) {
    if (loader != null)
        loader.update({
            title: 'Please wait...',
            html: `${progress.toFixed(0)}%`,
            allowEscapeKey: false,
            allowOutsideClick: false,
            showConfirmButton: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });
}

function fileToByteArray(file) {
    return new Promise((resolve, reject) => {
        var reader = new FileReader();
        reader.onloadend = () => {
            if (reader.readyState === FileReader.DONE) {
                resolve(new Uint8Array(reader.result));
            }
        };
        reader.onerror = () => reject(reader.error);
        reader.readAsArrayBuffer(file);
    });
}

function addRequiredClass() {
    $("input, select, textarea").each(function () {
        let attr = $(this).attr("data-val-required");

        if (typeof attr !== typeof undefined && attr !== false) {
            if (!$(this).is(":checkbox") && !$(this).is(":radio")) {
                let elementId = $(this).attr("id");
                let label = $(`label[for="${elementId}"]`);

                if (label.length === 0) {
                    if ($(this).is("select")) {
                        let option = $(this).find('option:first-child');
                        let optionText = "* " + option.html();

                        option.html(optionText);
                    }
                } else {
                    label.addClass("required");
                }
            }
        }
    });
}

function fixElementSequence(element) {
    var count = 0;
    $(element).each(function (i) {
        $('input, select, textarea', $(this)).each(function () {
            var input_id = $(this).attr("id") ?? "";
            var input_name = $(this).attr("name") ?? "";
            var aria_describedby = $(this).attr("aria-describedby") ?? "";
            var start1 = input_id.indexOf("[") + 1;
            var end1 = input_id.indexOf("]");
            var start2 = input_name.indexOf("[") + 1;
            var end2 = input_name.indexOf("]");
            var start3 = aria_describedby.indexOf("[") + 1;
            var end3 = aria_describedby.indexOf("]");

            input_id = input_id.length > 0 ? input_id.replace(input_id.substring(start1, end1), count) : "";
            input_name = input_name.length > 0 ? input_name.replace(input_name.substring(start2, end2), count) : "";
            aria_describedby = aria_describedby.length > 0 ? aria_describedby.replace(aria_describedby.substring(start3, end3), count) : "";

            $(this).attr({ id: input_id, name: input_name, "aria-describedby": aria_describedby, "data-index": count });
            $(this).trigger("change");
        });

        $('label', $(this)).each(function () {
            var label_id = $(this).attr("for") ?? "";
            var start1 = label_id.indexOf("[") + 1;
            var end1 = label_id.indexOf("]");

            label_id = label_id.length > 0 ? label_id.replace(label_id.substring(start1, end1), count) : "";
            label_id.length > 0 ? $(this).attr({ 'for': label_id }) : "";
        });

        $('span', $(this)).each(function () {
            var span_id = $(this).attr("data-valmsg-for") ?? "";
            var span_id2 = $(this).attr("id") ?? "";
            var start1 = span_id.indexOf("[") + 1;
            var end1 = span_id.indexOf("]");
            var start2 = span_id2.indexOf("[") + 1;
            var end2 = span_id2.indexOf("]");

            span_id = span_id.length > 0 ? span_id.replace(span_id.substring(start1, end1), count) : "";
            span_id2 = span_id2.length > 0 ? span_id2.replace(span_id2.substring(start2, end2), count) : "";

            span_id.length > 0 ? $(this).attr({ 'data-valmsg-for': span_id }) : "";
            span_id2.length > 0 ? $(this).attr({ 'id': span_id2 }) : "";
        });
        count++;
    });
}

function addAddress(addressObj = {}) {
    let count = $(".address_row").length;
    let rowToAdd = `<div class="col-12 address_row mb-2 d-none d-lg-block">
    <div class="border">
        <div class="p-2">
            <div class="row border-bottom address_name_div">
                <div class="col-md-6 align-middle">
                    <input class="address_id_input" type="hidden" data-val="true" data-val-required="The Id field is required." id="Address[${count}]_Id" name="Address[${count}].Id" value="${addressObj.Id || 0}" aria-describedby="">
                    <input type="hidden" data-val="true" data-val-required="The ReferenceId field is required." id="Address[${count}]_ReferenceId" name="Address[${count}].ReferenceId" value="${addressObj.ReferenceId || 0}" aria-describedby="">
                    <select id="Address[${count}]_AddressName" class="form-control selectized mb-2" data-value=""data-val="true" data-val-required="Address Name is required." name="Address[${count}].AddressName" tabindex="-1" style="display: none;">
                        <option value="">Select Address Type...</option>
                    </select>
                    <span class="text-danger field-validation-valid" data-valmsg-for="Address[${count}].AddressName" data-valmsg-replace="true"></span>
                </div>
                <div class="col-md-6">
                    <button type="button" class="btn btn-sm btn-danger remove_address float-end">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
            </div>
            <div class="row gx-2 pt-2">
                <div class="col-md-12 mb-2">
                    <div class="form-group">
                        <label class="form-label" for="Address[${count}]_StreetAddress1">Address Line 1</label>
                        <textarea class="form-control" placeholder="Street Address 1" type="text" data-val="true" data-val-required="Street Address 1 is required!" id="Address[${count}]_StreetAddress1" name="Address[${count}].StreetAddress1" aria-describedby="">${addressObj.StreetAddress1 || ""}</textarea>
                        <span class="text-danger field-validation-valid" data-valmsg-for="Address[${count}].StreetAddress1" data-valmsg-replace="true"></span>
                    </div>
                </div>
                <div class="col-md-12 mb-2">
                    <div class="form-group">
                        <label class="form-label" for="Address[${count}]_StreetAddress2">Address Line 2 (optional)</label>
                        <textarea class="form-control" placeholder="Street Address 2" type="text" id="Address[${count}]_StreetAddress2" name="Address[${count}].StreetAddress2" aria-describedby="">${addressObj.StreetAddress2 || ""}</textarea>
                        <span class="text-danger field-validation-valid" data-valmsg-for="Address[${count}].StreetAddress2" data-valmsg-replace="true"></span>
                    </div>
                </div>
                <div class="col-4 mb-2">
                    <div class="form-group">
                        <label class="form-label" for="Address[${count}]_Baranggay">Baranggay</label>
                        <input class="form-control" placeholder="Baranggay" type="text" id="Address[${count}]_Baranggay" name="Address[${count}].Baranggay" value="${addressObj.Baranggay || " "}" aria-describedby="">
                        <span class="text-danger field-validation-valid" data-valmsg-for="Address[${count}].Baranggay" data-valmsg-replace="true"></span>
                    </div>
                </div>
                <div class="col-4 mb-2">
                    <div class="form-group">
                        <label class="form-label" for="Address[${count}]_CityMunicipality">City/Municipality</label>
                        <input class="form-control" placeholder="City/Municipality" type="text" data-val="true" data-val-required="City/Municipality is required!" id="Address[${count}]_CityMunicipality" name="Address[${count}].CityMunicipality" value="${addressObj.CityMunicipality || ""}" aria-describedby="">
                        <span class="text-danger field-validation-valid" data-valmsg-for="Address[${count}].CityMunicipality" data-valmsg-replace="true"></span>
                    </div>
                </div>
                <div class="col-4 mb-2">
                    <div class="form-group">
                        <label class="form-label" for="Address[${count}]_StateProvince">State/Province</label>
                        <input class="form-control" placeholder="State/Province" type="text" id="Address[${count}]_StateProvince" name="Address[${count}].StateProvince" value="${addressObj.StateProvince || " "}" aria-describedby="">
                        <span class="text-danger field-validation-valid" data-valmsg-for="Address[${count}].StateProvince" data-valmsg-replace="true"></span>
                    </div>
                </div>
                <div class="col-4 mb-2">
                    <div class="form-group">
                        <label class="form-label" for="Address[${count}]_Region">Region</label>
                        <input class="form-control" placeholder="Region" type="text" id="Address[${count}]_Region" name="Address[${count}].Region" value="${addressObj.Region || " "}" aria-describedby="">
                        <span class="text-danger field-validation-valid" data-valmsg-for="Address[${count}].Region" data-valmsg-replace="true"></span>
                    </div>
                </div>
                <div class="col-4 mb-2">
                    <div class="form-group">
                        <label class="form-label" for="Address[${count}]_PostalCode">Postal Code</label>
                        <input class="form-control MaxLength" maxlength="9" placeholder="Postal Code" data-val="true" data-val-required="Postal Code is required!" id="Address[${count}]_PostalCode" name="Address[${count}].PostalCode" value="${addressObj.PostalCode || ""}" aria-describedby="">
                        <span class="text-danger field-validation-valid" data-valmsg-for="Address[${count}].PostalCode" data-valmsg-replace="true"></span>
                    </div>
                </div>
                <div class="col-4">
                    <div class="mb-2">
                        <label class="form-label" for="Address[${count}]_CountryId">Country</label>
                        <select class="form-control" data-val="true" data-val-required="Country is required." id="Address[${count}]_CountryId" name="Address[${count}].CountryId" aria-describedby="">
                            <option value="">Select Country...</option>
                        </select>
                        <span class="text-danger field-validation-valid" data-valmsg-for="Address[${count}].CountryId" data-valmsg-replace="true"></span>
                    </div>
                </div>
                <div class="col-12 mb-2">
                    <div class="form-group">
                        <label class="form-label" for="Address_[${count}]__Remarks">Remarks</label>
                        <textarea class="form-control" placeholder="Remarks" id="Address_[${count}]__Remarks" name="Address[${count}].Remarks" aria-describedby="">${addressObj.Remarks || ""}</textarea>
                        <span class="field-validation-valid" data-valmsg-for="Address[${count}].Remarks" data-valmsg-replace="true"></span>
                    </div>
                </div>
                <div class="col-12 mb-2">
                    <div class="form-check form-switch">
                        <input class="form-check-input address_checkbox" type="checkbox" data-val="true" data-val-required="The Set as Default field is required." id="Address[${count}]_IsDefault" name="Address[${count}].IsDefault" value="true" aria-describedby="">
                        <label class="form-check-label" for="Address[${count}]_IsDefault">Set as Default</label>
                    </div>
                    <span class="text-danger field-validation-valid" data-valmsg-for="Address[${count}].IsDefault" data-valmsg-replace="true"></span>
                </div>
            </div>
        </div>
    </div>
</div>`;
    $(".address_div").append(rowToAdd);
    /* addressCount++;*/

    let countryDropdown, $countryDropdown;
    let addressTypeDropdown, $addressTypeDropdown;

    $countryDropdown = $(`[id="Address[${count}]_CountryId"]`).selectize({
        valueField: 'Id',
        labelField: 'Name',
        searchField: 'Name',
        preload: true,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + "Country/GetCountries/",
                success: function (results) {
                    try {
                        callback(results);
                    } catch (e) {
                        callback();
                    }
                },
                error: function () {
                    callback();
                }
            });
        },
        render: {
            item: function (item, escape) {
                return ("<div>" +
                    escape(item.Name) +
                    "</div>"
                );
            },
            option: function (item, escape) {
                return ("<div class='py-1 px-2'>" +
                    escape(item.Name) +
                    "</div>"
                );
            }
        }
    });

    $addressTypeDropdown = $(`[id="Address[${count}]_AddressName"]`).selectize({
        valueField: 'Name',
        labelField: 'Name',
        searchField: 'Name',
        preload: true,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + "Address/GetAddressTypes/",
                success: function (results) {
                    try {
                        callback(results);
                    } catch (e) {
                        callback();
                    }
                },
                error: function () {
                    callback();
                }
            });
        },
        render: {
            item: function (item, escape) {
                return ("<div>" +
                    escape(item.Name) +
                    "</div>"
                );
            },
            option: function (item, escape) {
                return ("<div class='py-1 px-2'>" +
                    escape(item.Name) +
                    "</div>"
                );
            }
        }
    });

    countryDropdown = $countryDropdown[0].selectize;
    countryDropdown.on('load', function (options) {
        countryDropdown.setValue(addressObj.CountryId || "");
        countryDropdown.off('load');
    });

    addressTypeDropdown = $addressTypeDropdown[0].selectize;
    addressTypeDropdown.on('load', function (options) {
        addressTypeDropdown.setValue(addressObj.AddressName || "");
        addressTypeDropdown.off('load');
    });

    $(`[name="Address[${count}].IsDefault"]`).prop("checked", addressObj.IsDefault).trigger("change");
    $(`[id='Address[${count}]_PostalCode']`).inputmask({ regex: '^[0-9]+$', placeholder: "" });
    addRequiredClass();
}

function addRepresentative(itemObj = {}) {
    let count = $(".rep_row").length;

    let row_to_add = `<div class="col-12 mb-2 rep_row">
							<div class="border">
								<div class="p-2">
									<div class="row border-bottom align-middle pb-2">
										<div class="col-md-6">
											<input id="EntityRepresentative_Id_[${count}]" type="hidden" data-val="true" data-val-required="The Id field is required." name="EntityRepresentative[${count}].Id" value="${itemObj.Id || 0}">
											<input id="EntityRepresentative_EntityId_[${count}]" type="hidden" name="EntityRepresentative[${count}].EntityId" value="${itemObj.EntityId || 0}">
										</div>
										<div class="col-md-6">
											<button type="button" class="btn btn-sm btn-danger float-end remove_rep">
												<i class="mdi mdi-close"></i>
											</button>
										</div>
									</div>
									<div class="row gx-2 pt-2">
										<div class="col-sm-2 mb-2">
											<div class="form-group">
												<label class="form-label" for="EntityRepresentative_Prefix_[${count}]">Prefix</label>
												<select id="EntityRepresentative_Prefix_[${count}]" class="form-control selectized" data-value=""  data-val="true" data-val-required="Prefix is required." name="EntityRepresentative[${count}].Prefix" tabindex="-1" style="display: none;">
													<option value="">Select Prefix...</option>
												</select>
												<span class="text-danger field-validation-valid" data-valmsg-for="EntityRepresentative[${count}].Prefix" data-valmsg-replace="true"></span>
											</div>
										</div>
										<div class="col-md-6 mb-2">
											<div class="form-group">
												<label class="form-label" for="EntityRepresentative_FirstName_[${count}]">First Name</label>
												<input id="EntityRepresentative_FirstName_[${count}]" class="form-control" placeholder="First Name" type="text" data-val="true" data-val-required="First Name is required." name="EntityRepresentative[${count}].FirstName" value="${itemObj.FirstName || ""}">
												<span class="text-danger field-validation-valid" data-valmsg-for="EntityRepresentative[${count}].FirstName" data-valmsg-replace="true"></span>
											</div>
										</div>
										<div class="col-md-4 mb-2">
											<div class="form-group">
												<label class="form-label" for="EntityRepresentative_MiddleName_[${count}]">Middle Name</label>
												<input id="EntityRepresentative_MiddleName_[${count}]" class="form-control" placeholder="Middle Name" type="text" name="EntityRepresentative[${count}].MiddleName" value="${itemObj.MiddleName || ""}">
												<span class="text-danger field-validation-valid" data-valmsg-for="EntityRepresentative[${count}].MiddleName" data-valmsg-replace="true"></span>
											</div>
										</div>
										<div class="col-md-7 mb-2">
											<div class="form-group">
												<label class="form-label" for="EntityRepresentative_LastName_[${count}]">Last Name</label>
												<input id="EntityRepresentative_LastName_[${count}]" class="form-control" placeholder="Last Name" type="text" data-val="true" data-val-required="Last Name is required." name="EntityRepresentative[${count}].LastName" value="${itemObj.LastName || ""}">
												<span class="text-danger field-validation-valid" data-valmsg-for="EntityRepresentative[${count}].LastName" data-valmsg-replace="true"></span>
											</div>
										</div>
										<div class="col-md-2 mb-2">
											<div class="form-group">
												<label class="form-label" for="EntityRepresentative_Suffix_[${count}]">Suffix</label>
												<input id="EntityRepresentative_Suffix_[${count}]" class="form-control" placeholder="Jr./II" type="text" name="EntityRepresentative[${count}].Suffix" value="${itemObj.Suffix || ""}">
												<span class="text-danger field-validation-valid" data-valmsg-for="EntityRepresentative[${count}].Suffix" data-valmsg-replace="true"></span>
											</div>
										</div>
										<div class="col-md-3 mb-2">
											<div class="form-group">
												<label class="form-label" for="EntityRepresentative_Gender_[${count}]">Gender</label>
												<select id="EntityRepresentative_Gender_[${count}]" class="form-control selectized" data-value=""  data-val="true" data-val-required="Gender is required." name="EntityRepresentative[${count}].Gender" tabindex="-1" style="display: none;">
													<option value="">Select Gender...</option>
												</select>
												<span class="text-danger field-validation-valid" data-valmsg-for="EntityRepresentative[${count}].Gender" data-valmsg-replace="true"></span>
											</div>
										</div>
										<div class="w-100"></div>
										<div class="border-bottom mb-2">
											<h5 class="text-muted">Contact Info.</h5>
										</div>
										<div class="col-md-4 mb-2">
											 <div class="form-group">
												<label class="form-label" for="EntityRepresentative_TelNo_[${count}]">Telephone No.</label>
												<input id="EntityRepresentative_TelNo_[${count}]" class="form-control" placeholder="Telephone No." type="text" name="EntityRepresentative[${count}].TelNo" value="${itemObj.TelNo || ""}">
												<span class="text-danger field-validation-valid" data-valmsg-for="EntityRepresentative[${count}].TelNo" data-valmsg-replace="true"></span>
											</div>
										</div>
										<div class="col-md-4 mb-2">
											 <div class="form-group">
												<label class="form-label" for="EntityRepresentative_MobileNo_[${count}]">Mobile No.</label>
												<input id="EntityRepresentative_MobileNo_[${count}]" class="form-control" placeholder="Mobile No." type="text" name="EntityRepresentative[${count}].MobileNo" value="${itemObj.MobileNo || ""}">
												<span class="text-danger field-validation-valid" data-valmsg-for="EntityRepresentative[${count}].MobileNo" data-valmsg-replace="true"></span>
											</div>
										</div>
										<div class="col-md-4 mb-2">
											 <div class="form-group">
												<label class="form-label" for="EntityRepresentative_FaxNo_[${count}]">Fax No.</label>
												<input id="EntityRepresentative_FaxNo_[${count}]" class="form-control" placeholder="Fax No." type="text" name="EntityRepresentative[${count}].FaxNo" value="${itemObj.FaxNo || ""}">
												<span class="text-danger field-validation-valid" data-valmsg-for="EntityRepresentative[${count}].FaxNo" data-valmsg-replace="true"></span>
											</div>
										</div>
										<div class="col-md-4 mb-2">
											 <div class="form-group">
												<label class="form-label" for="EntityRepresentative_OfficeNo_[${count}]">Office No.</label>
												<input id="EntityRepresentative_OfficeNo_[${count}]" class="form-control" placeholder="Office No." type="text" name="EntityRepresentative[${count}].OfficeNo" value="${itemObj.OfficeNo || ""}">
												<span class="text-danger field-validation-valid" data-valmsg-for="EntityRepresentative[${count}].OfficeNo" data-valmsg-replace="true"></span>
											</div>
										</div>
										<div class="col-md-8 mb-2">
											 <div class="form-group">
												<label class="form-label" for="EntityRepresentative_Email_[${count}]">Email</label>
												<input id="EntityRepresentative_Email_[${count}]" class="form-control" placeholder="sample@email.com" type="email" data-val="true" data-val-email="Please input a valid email address." name="EntityRepresentative[${count}].Email" value="${itemObj.Email || ""}">
												<span class="text-danger field-validation-valid" data-valmsg-for="EntityRepresentative[${count}].Email" data-valmsg-replace="true"></span>
											</div>
										</div>
										<div class="col-md-12 mb-2">
											 <div class="form-group">
												<label class="form-label" for="EntityRepresentative_Address_[${count}]">Address</label>
												<textarea id="EntityRepresentative_Address_[${count}]" class="form-control" placeholder="Address" name="EntityRepresentative[${count}].Address">${itemObj.Address || ""}</textarea>
												<span class="text-danger field-validation-valid" data-valmsg-for="EntityRepresentative[${count}].Address" data-valmsg-replace="true"></span>
											</div>
										</div>
										<div class="col-md-12 mb-2">
											<div class="form-group">
												<div class="form-check form-switch">
													<input id="EntityRepresentative_IsDefault_[${count}]" class="form-check-input rep_checkbox" type="checkbox" data-val="true" data-val-required="The Set as Default field is required." name="EntityRepresentative[${count}].IsDefault" value="true"><input name="EntityRepresentative[${count}].IsDefault" type="hidden" value="false">
													<label class="form-check-label" for="EntityRepresentative_[${count}]_IsDefault">Set as Default</label>
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>`;

    $(".rep_div").append(row_to_add);
    $(`[name="EntityRepresentative[${count}].IsDefault"]`).prop("checked", itemObj.IsDefault || false).trigger("change");

    let genderDropdown, $genderDropdown;
    let prefixDropdown, $prefixDropdown;

    $genderDropdown = $(`[id="EntityRepresentative_Gender_[${count}]"]`).selectize({
        valueField: 'Id',
        labelField: 'Description',
        searchField: 'Description',
        preload: true,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + "API/Gender/GetGenderList/" + true,
                success: function (results) {
                    try {
                        callback(results);
                    } catch (e) {
                        callback();
                    }
                },
                error: function () {
                    callback();
                }
            });
        },
        render: {
            item: function (item, escape) {
                return ("<div>" +
                    escape(item.Description) +
                    "</div>");
            },
            option: function (item, escape) {
                return (
                    "<div class='py-1 px-2'>" +
                    escape(item.Description) +
                    "</div>"
                );
            }
        }
    });

    $prefixDropdown = $(`[id="EntityRepresentative_Prefix_[${count}]"]`).selectize({
        valueField: 'Description',
        labelField: 'Description',
        searchField: 'Description',
        preload: true,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + "API/GetPrefixList/",
                success: function (results) {
                    try {
                        callback(results);
                    } catch (e) {
                        callback();
                    }
                },
                error: function () {
                    callback();
                }
            });
        },
        render: {
            item: function (item, escape) {
                return ("<div>" +
                    escape(item.Description) +
                    "</div>");
            },
            option: function (item, escape) {
                return (
                    "<div class='py-1 px-2'>" +
                    escape(item.Description) +
                    "</div>"
                );
            }
        }
    });

    genderDropdown = $genderDropdown[0].selectize;
    prefixDropdown = $prefixDropdown[0].selectize;

    genderDropdown.on('load', function (options) {
        genderDropdown.setValue(itemObj.Gender || "");
        genderDropdown.off('load');
    });

    prefixDropdown.on('load', function (options) {
        prefixDropdown.setValue(itemObj.Prefix || "");
        prefixDropdown.off('load');
    });
    addRequiredClass();
}

function loadImageOnPreview(location, defaultImage, divId) {
    $.ajax({
        type: "get",
        url: location,
        success: function (data) {
            $(divId).css('background-image', 'url("' + location + '")');
            $(divId).hide();
            $(divId).fadeIn(650);
        },
        error: function (data) {
            $(divId).css('background-image', 'url("' + defaultImage + '")');
            $(divId).hide();
            $(divId).fadeIn(650);
        }
    });
}

function readUrl(input, container) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            container.css('background-image', 'url(' + e.target.result + ')');
            container.hide();
            container.fadeIn(650);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

function isNullOrWhitespace(input) {
    return !input || !input.trim();
}
function checkFileExist(urlToFile) {
    var xhr = new XMLHttpRequest();
    xhr.open('HEAD', urlToFile, false);
    xhr.send();

    if (xhr.status == "404") {
        return false;
    } else {
        return true;
    }
}

function initializeDecimalInputMask(classname = ".decimalInputMask", digits = 2, limiter = "900,000,000,000,000", isallownegative = false, rightAlign = true) {
    let placeholder = "";

    if (digits == 2) { placeholder = "0.00"; }
    else if (digits == 5) { placeholder = "0.00000"; }

    $(classname).inputmask({
        alias: 'decimal',
        rightAlign: rightAlign,
        groupSeparator: '.',
        digits: digits,
        allowMinus: isallownegative,
        autoGroup: true,
        placeholder: placeholder,
        max: Number(limiter.replace(/[^-?0-9\.]+/g, ""))
    });
}

function initializeLeftDecimalInputMask(classname = ".decimalInputMask", digits = 2, limiter = "900,000,000,000,000", isallownegative = false) {
    //let placeholder = "";

    //if (digits == 2) { placeholder = "0.00"; }
    //else if (digits == 5) { placeholder = "0.00000"; }

    //$(classname).inputmask({
    //    alias: 'decimal',
    //    rightAlign: true,
    //    groupSeparator: '.',
    //    digits: digits,
    //    allowMinus: isallownegative,
    //    autoGroup: true,
    //    placeholder: placeholder,
    //    max: Number(limiter.replace(/[^-?0-9\.]+/g, ""))
    //});

    initializeDecimalInputMask(classname, digits, limiter, isallownegative, false);
}

function updateBeneficiaryHousingLoanSideBarNav() {
    let pagibignumber = $("#txt_userPagibigNumber").val();

    var $link = $('a.side-nav-link[href="/Applicants/HousingLoanForm"]');

    // Check if element is found
    if ($link.length > 0) {
        // Append '/2434' to the current href value
        var currentHref = $link.attr('href');
        var newHref = currentHref + "/" + pagibignumber;

        // Update the href attribute with the new value
        $link.attr('href', newHref);
    }
}

function updateUserProfile() {
    const userId = $("#txt_userId").val();
    const defaultProfile = "/images/user/default.png";
    $.ajax({
        url: baseUrl + "User/GetUser/" + userId,
        success: function (response) {
            $('#userProfile').attr("src", response.ProfilePicture);
            $('#notifProfile').attr("src", response.ProfilePicture);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('#userProfile').attr("src", defaultProfile);
            $('#notifProfile').attr("src", defaultProfile);
        }
    });
}

function assessCheckbox(checkbox, target) {
    if (checkbox.prop('checked')) {
        target.prop('readonly', true);
        return;
    }

    target.prop('readonly', false);
}

hlafBcfNav();

bcfUploading();

approveBcfNote();

function bcfUploading() {
    // Get all <a> elements in the side navigation menu
    var sideNavLinks = document.querySelectorAll('.side-nav-item a');

    // Iterate through each <a> element
    sideNavLinks.forEach(function (link) {
        // Check if the href attribute starts with '/Applicants/HousingLoanForm'
        if (link.getAttribute('href').startsWith('/BuyerConfirmation/Upload')) {
            // Add a click event listener to the link

            var bcfDocumentStatus = $("#Bcf_DocumentStatus").val();
            var bcfStatus = $("#Bcf_ApplicationStatus").val();

            console.log(bcfDocumentStatus);
            console.log(bcfStatus);

            link.addEventListener('click', function (event) {
                //if bcf not verified by dev
                if (bcfStatus != 3) {
                    event.preventDefault();
                }
                //if bcf document  is submitted && verified
                else if (bcfDocumentStatus == 1 || bcfDocumentStatus == 3) {
                    console.log(0);
                    event.preventDefault();
                }
                else {
                }
            });
        }
    });
}

function approveBcfNote() {
    var bcfAppStatus = $("#Bcf_ApplicationStatus").val();
    console.log(bcfAppStatus);
    if (bcfAppStatus == 3) {
        console.log(bcfAppStatus);
        $("#div_approvebcfNote").removeClass("d-none");
        $("#btn_bcfdownload").attr("data-url", "Home/BcfDownload");
    }

    $("#btn_bcfdownload").on("click", function () {
        let dataurl = $(this).attr('data-url');

        window.location.href = baseUrl + dataurl;
    });
}

function hlafBcfNav() {
    // Get all <a> elements in the side navigation menu
    var sideNavLinks = document.querySelectorAll('.side-nav-item a');

    // Iterate through each <a> element
    sideNavLinks.forEach(function (link) {
        // Check if the href attribute starts with '/Applicants/HousingLoanForm'
        if (link.getAttribute('href').startsWith('/Applicants/HousingLoanForm')) {
            // Add a click event listener to the link
            link.addEventListener('click', function (event) {
                // Prevent the default behavior (navigation)
                event.preventDefault();
                loadBcfPrompt();
            });
        }
    });
}

async function loadBcfPrompt() {
    let bcfExists = await bcfChecker();
    let hlafExists = await hlafChecker();
    let isBcfQAnswered = $(`[id="UserFlag_IsBcfCreated"]`).data('flag');

    if (isBcfQAnswered && hlafExists) {
        // Check if HLAF is created and active AND User selects "I have BCF"
        hlfRedirect();
        return;
    }
    else if (!isBcfQAnswered && (bcfExists && hlafExists)) {
        // Check if both HLAF and BCF is created, HLAF is active, AND User selects "I don't have BCF yet"
        hlfRedirect();
        return;
    }

    Swal.fire({
        //width: `60%`,
        customClass: {
            popup: `rounded-5`,
            confirmButton: "btn btn-primary btn-lg rounded-4",
        },
        title: `<span class="text-info fw-medium">Important Question</span>`,
        html: `
                <div class="d-flex flex-column justify-content-center">
                    <p class="text-secondary text-wrap">Do you have a completely filled-up 4PH Buyer Confirmation Form (BCF)?</p>
                </div>
                <div class="d-flex flex-column align-items-start">
                    <div class="mt-2 mb-1 form-check form-check-inline">
                        <input type="radio" name="4PH_Confirmation" id="4PH_Confirm_True" data-val="1" />
                        <label class="fs-4 text-muted form-check-label" for="4PH_Confirm_True">Yes, I do have a completely filled-up one.</label>
                    </div>
                    <div class="mb-2 form-check form-check-inline">
                        <input type="radio" name="4PH_Confirmation" id="4PH_Confirm_False" data-val="0" />
                        <label class="fs-4 text-muted form-check-label" for="4PH_Confirm_False">No, I do not have that yet.</label>
                    </div>
                </div>
            `,
        allowOutsideClick: false,
        allowEscapeKey: false,
        confirmButtonText: `<span class="fs-4">Confirm Selection</span>`
    }).then((result) => {
        if (result.isConfirmed) {
            // Check one of two Radio buttons
            let filledUpForm = ($('input[name="4PH_Confirmation"]:checked').data("val") === 1);
            let filledUpTitle = filledUpForm ? `You have a BCF` : `You do not have a BCF`;
            let filledUpString = filledUpForm ? `you have` : `you do not have`;

            loadBcfConfirmation(filledUpTitle, filledUpString, filledUpForm);
        }
    });

    $('input[name="4PH_Confirmation"]').on('change', function () {
        $(Swal.getConfirmButton()).prop('disabled', false);
    });

    $(Swal.getConfirmButton()).prop('disabled', true);
}

function loadBcfConfirmation(filledUpTitle, filledUpString, filledUpForm) {
    Swal.fire({
        customClass: {
            popup: `rounded-4`,
            confirmButton: "rounded-4",
            cancelButton: "rounded-4",
            htmlContainer: 'd-flex justify-content-center',
        },
        title: `<span class="text-info fw-medium">${filledUpTitle}</span>`,
        html: `
                <div class="align-items-center justify-content-center" style="width: 75%;">
                    Do you confirm that <span class="fw-bolder">${filledUpString}</span> a completely filled-up 4PH BCF?
                </div>
            `,
        icon: "question",
        showCancelButton: true,
        allowOutsideClick: false,
        allowEscapeKey: false,
        confirmButtonText: `<span class="fs-4">Confirm</span>`,
        cancelButtonText: `<span class="fs-4">Cancel</span>`,
    }).then(async (result) => {
        if (result.isConfirmed) {
            // Check one of two radio buttons
            //if (filledUpForm) {
            //    // Code block of user selecting "I do have a completely filled-up one"
            //    console.log("Execute 4PH - TRUE Code block");

            //    // redirect to housing loan form
            //    updateBcfFlag(true, () => { location.replace(baseUrl + `Applicants/HousingLoanForm/` + $(`[id="txt_userPagibigNumber"]`).val()); });
            //}
            //else {
            //    // Code block of user selecting "No, I do not have that yet"
            //    console.log("Execute 4PH - FALSE Code block");
            //    updateBcfFlag(false, () => { return; });
            //}

            updateBcfFlag(filledUpForm, () => { location.replace(baseUrl + `Applicants/HousingLoanForm/` + $(`[id="txt_userPagibigNumber"]`).val()); });
        }
        else {
            $('input[name="4PH_Confirmation"]').off('change');
            await loadBcfPrompt();
        }
    });
}

function updateBcfFlag(flag, callback) {
    $.ajax({
        url: baseUrl + 'Applicants/UpdateBcfFlag',
        method: "POST",
        data: {
            flag: flag
        },
        success: callback
    });
}

async function bcfChecker() {
    let bcfFlag = await fetch(baseUrl + 'Applicants/CheckBcf')
        .then(response => {
            if (!response.ok) {
                messageBox(response.statusText, "danger");
            }

            return response.json();
        })
        .then(response => {
            // Using the fetched data
            //console.log('Data received:', response);
            return response;
        }).catch(error => {
            // Handling any errors that occur during the request
            messageBox(error, "danger");
        });

    return bcfFlag;
}

async function hlafChecker() {
    let hlafFlag = await fetch(baseUrl + 'Applicants/CheckCurrentHlaf')
        .then(response => {
            if (!response.ok) {
                messageBox(response.statusText, "danger");
            }

            return response.json();
        })
        .then(response => {
            // Using the fetched data
            //console.log('Data received:', response);
            return response;
        }).catch(error => {
            // Handling any errors that occur during the request
            messageBox(error, "danger");
        });

    return hlafFlag;
}

function hlfRedirect() {
    location.replace(baseUrl + `Applicants/HousingLoanForm/` + $(`[id="txt_userPagibigNumber"]`).val());
}