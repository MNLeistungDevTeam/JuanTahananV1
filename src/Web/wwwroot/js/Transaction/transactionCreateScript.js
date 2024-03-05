$(function () {
    const $dateFromInput = $("[name='DateFrom']");
    const $dateToInput = $("[name='DateTo']");

    $("#PreparedBy").selectize();

    $(".flatpickr-unwrap").flatpickr({
        enableTime: false,
        dateFormat: "m/d/Y",
        //defaultDate: moment().format("MM/DD/YYYY"),
        //wrap: true,
        //minDate: minTransactionDate,
        allowInput: true,
    });

    $(":input").inputmask();

    new dateRangePlugin("#date_range_1", "#date_picker_div", $dateFromInput, $dateToInput);
});