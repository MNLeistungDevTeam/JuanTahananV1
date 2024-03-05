"use strict"

const baseUrl = $("#txtBaseUrl").val();
let localizedStrings;
loadLocalizedStrings();
var loader = null;
var applications = null;
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
        if (form.hasClass('was-validated')){
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
    if (document.querySelector('#totalApplications')) {
        getApplicationCount(function (callback) {
            const options = {
                useEasing: true,
                useGrouping: true,
                separator: ',',
                decimal: '.',
                suffix: ''
            };
            if (applications == null) {
                applications = new CountUp(`totalApplications`, 0, callback, 0, 2, options);
                if (!applications.error) {
                    applications.start();
                }
            } else {
                applications.update(callback);
            }
        })
    }
});
function getApplicationCount(callback) {
    $.ajax({
        url: `/Home/GetApplicationsCount`,
        method: 'get',
        success : callback
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