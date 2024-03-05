'use strict'

class dateRangePlugin {
    constructor(dateRangeParentContainer, datePickerDiv, dateFromInput, dateToInput) {
        this.dateRanges = [
            {
                name: "Previous Month",
                dateFrom: moment().subtract(1, 'month').startOf('month'),
                dateTo: moment().subtract(1, 'month').endOf('month')
            },
            {
                name: "This Month",
                dateFrom: moment().startOf('month'),
                dateTo: moment().endOf('month')
            },
            {
                name: "Next Month",
                dateFrom: moment().add(1, 'month').startOf('month'),
                dateTo: moment().add(1, 'month').endOf('month')
            },
            {
                name: "Previous Year",
                dateFrom: moment().subtract(1, 'year').startOf('year'),
                dateTo: moment().subtract(1, 'year').endOf('year')
            },
            {
                name: "This Year",
                dateFrom: moment().startOf('year'),
                dateTo: moment().endOf('year')
            },
            {
                name: "Year To Date",
                dateFrom: moment().startOf('year'),
                dateTo: moment()
            },
            {
                name: "Previous Quarter",
                dateFrom: moment().subtract(1, 'quarter').startOf('quarter'),
                dateTo: moment().subtract(1, 'quarter').endOf('quarter')
            },
            {
                name: "This Quarter",
                dateFrom: moment().startOf('quarter'),
                dateTo: moment().endOf('quarter')
            },
            {
                name: "Next Quarter",
                dateFrom: moment().add(1, 'quarter').startOf('quarter'),
                dateTo: moment().add(1, 'quarter').endOf('quarter')
            },
            {
                name: "Custom",
                dateFrom: '',
                dateTo: ''
            }
        ]
        this.dateRangeParentContainer = dateRangeParentContainer;
        this.datePickerDiv = datePickerDiv;
        this.dateFromInput = dateFromInput;
        this.dateToInput = dateToInput;

        //private properties
        this._menuContainer = `${this.dateRangeParentContainer} .date-range-container .date-range-menu`;
        this._btnTrigger = `${this.dateRangeParentContainer} .date-range-container .date-range-dropdown`;

        this.initializeDateRangePicker();
        this.loadDateFilter();
    }

    initializeDateRangePicker() {
        var parentElement = $(this.dateRangeParentContainer);
        var label = $(document.createElement('label'));
        var divBtnGroup = $(document.createElement('div'));
        var btnDropdown = $(document.createElement('button'));
        var divDropdownMenu = $(document.createElement('div'))

        label.addClass("form-label");
        divBtnGroup.addClass("btn-group-vertical w-100 date-range-container");
        btnDropdown
            .addClass("btn btn-light w-100 dropdown-toggle date-range-dropdown")
            .attr({
                "data-bs-toggle": "dropdown",
                "aria-expanded": false
            })

        divDropdownMenu.addClass("dropdown-menu date-range-menu");

        divBtnGroup
            .append(btnDropdown)
            .append(divDropdownMenu);

        parentElement
            .append(label)
            .append(divBtnGroup);
    }

    loadDateFilter() {
        $(this._menuContainer).empty();

        let filterDateFrom = this.dateFromInput.val();
        let filterDateTo = this.dateToInput.val();

        for (var dateRange of this.dateRanges) {
            let _dateFrom = convertDate(dateRange.dateFrom, 'MM/DD/YYYY');
            let _dateTo = convertDate(dateRange.dateTo, 'MM/DD/YYYY');
            let isSelected = filterDateFrom == _dateFrom && filterDateTo == _dateTo;

            $(this._menuContainer).append(`<a class="dropdown-item date-range-item ${isSelected ? 'active' : ''}" href="#" data-name="${dateRange.name}" data-range-from="${dateRange.dateFrom}" data-range-to="${dateRange.dateTo}">${dateRange.name}</a>`);
        }

        this.changeDateRange();

        $(this._menuContainer).on('click', '.date-range-item', this.dateRangeItemClick);
        this.dateFromInput.on('change', this.dateRangeChange);
        this.dateToInput.on('change', this.dateRangeChange);
    }

    dateRangeItemClick = (e) => {
        let name = e.target.innerText;
        let dateFrom = e.target.attributes["data-range-from"].value;
        let dateTo = e.target.attributes["data-range-to"].value;

        //if (this.dateTo > this.dateFrom) {
        //    tempDate = this.dateFrom;
        //    this.dateFrom = this.dateTo;
        //    this.dateTo = tempDate;
        //}

        this.changeDateRange(name, dateFrom, dateTo);
    }

    dateRangeChange = (e) => {
        let _dateFrom = this.dateFromInput.val();
        let _dateTo = this.dateToInput.val();

        try {
            let fpFrom = this.dateFromInput[0]._flatpickr;
            let fpTo = this.dateToInput[0]._flatpickr;
            let periodFromDate = fpFrom.input.value;
            let periodToDate = fpTo.input.value;

            fpFrom.set("maxDate", null);
            fpTo.set("minDate", null);

            if (periodToDate != "")
                fpFrom.set("maxDate", periodToDate);
            if (periodFromDate != "")
                fpTo.set("minDate", periodFromDate);
        } catch (error) { }

        //if (_dateFrom != "" && _dateTo != "") {
        //    if (_dateFrom > _dateTo) {
        //        this.dateFromInput.val(_dateTo);
        //        this.dateToInput.val(_dateFrom);

        //        _dateFrom = this.dateFromInput.val();
        //        _dateTo = this.dateToInput.val();
        //    }
        //}

        if (_dateFrom == "" && _dateTo == "")
            this.changeDateRange("Custom", _dateFrom, _dateTo);
    }

    changeDateRange(name, dateFrom, dateTo) {
        this.dateFrom = dateFrom == null ? this.dateFromInput.val() : dateFrom;
        this.dateTo = dateFrom == null ? this.dateToInput.val() : dateTo;
        this.name = name == null ? $(`${this._menuContainer} a.dropdown-item.active`).html() : name;

        this.name = this.name || 'Custom';

        this.dateFrom = convertDate(this.dateFrom, 'MM/DD/YYYY');
        this.dateTo = convertDate(this.dateTo, 'MM/DD/YYYY');

        $(`${this._menuContainer} a`).each(function () {
            $(this).removeClass('active');
        });

        if (this.name != 'Custom') {
            $(this._btnTrigger).html(`${this.name} (${this.dateFrom} - ${this.dateTo})`);

            this.dateFromInput.val(this.dateFrom);
            this.dateToInput.val(this.dateTo);
        } else {
            $(this._btnTrigger).html(this.name);
        }

        $(`${this._menuContainer} [data-name='${this.name}']`).addClass('active');

        $(this.datePickerDiv).attr({ hidden: this.name != "Custom" });
    }

    addDateRangeOption(itemObj) {
        this.dateRanges.push(itemObj);
        this.loadDateFilter();
    }

    setDateRangeOption(itemObj1) {
        this.dateRanges = [];
        console.log(itemObj1);
        this.dateRanges = itemObj1;
        this.addCustomRange();
        this.loadDateFilter();
    }

    addCustomRange() {
        this.dateRanges.push({
            name: "Custom",
            dateFrom: '',
            dateTo: ''
        });

        this.loadDateFilter();
    }
}