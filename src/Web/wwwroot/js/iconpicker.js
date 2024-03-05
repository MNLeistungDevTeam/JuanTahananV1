$(() => {
    "strict";
    var buttonFilter;
    var qsRegex;
    $('.icon-picker').each(function () {
        var span = $(this).siblings('.input-group-text');
        span.html(`<i class="fe-package text-truncate"></i>`);
    })
    $('.icon-picker').on('input', function () {
        var span = $(this).siblings('.input-group-text');
        if ($(this).val()) {
            span.html(`<i class="${$(this).val()} text-truncate"></i>`);
        } else {
            span.html(`<i class="fe-package text-truncate"></i>`);
        }
    });
    $('.icon-picker').siblings('button').on('click', function () {
        var iconModal = $('#icon-picker-modal');
        iconModal.modal('show');
    });

    var $icon_grid = $('.icon-grid').isotope({
        itemSelector: '.grid-item',
        layoutMode: 'fitRows',
        filter: function () {
            var $this = $(this);
            var searchResult = qsRegex ? $this.text().match(qsRegex) : true;
            var buttonResult = buttonFilter ? $this.is(buttonFilter) : true;
            return searchResult && buttonResult;
        }
    });
    //$('#sorts').on('click', 'button', function () {
    //    var sortByValue = $(this).attr('data-sort-by');
    //    $icon_grid.isotope({ sortBy: sortByValue });
    //});
    var $quicksearch = $('.quicksearch').on('keyup',debounce(function () {
        qsRegex = new RegExp($quicksearch.val(), 'gi');
        $icon_grid.isotope();
    }));
    $('#icon-picker-modal').on('shown.bs.modal', function () {
        $icon_grid.isotope('layout');
    });

    function loadicons(iconType = 0) {

    }
})