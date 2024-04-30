"use strict"

$(function () {
    $("[id='btnHistory']").click(function () {
        let id = $(this).attr("data-id");
        let module = $(this).attr("data-module");
        loadTransactionTimeline(id, module);

        //$("#history-modal").modal("show");

        var myOffcanvas = document.getElementById('offcanvasHistory');
        var bsOffcanvas = new bootstrap.Offcanvas(myOffcanvas);

        bsOffcanvas.show();
    });
});

function loadTransactionTimeline(recordId, type) {
    $.ajax({
        url: baseUrl + "AuditTrail/GetAuditTrail",
        data: {
            recordId: recordId,
            type: type
        },
        success: function (response) {
            let dates = [];
            let edittedItems = [];
            let timeline = "";

            const map = new Map();
            for (const item of response) {
                if (!map.has(moment(new Date(item.ChangeDate)).format("DD MMM. YYYY"))) {
                    map.set(moment(new Date(item.ChangeDate)).format("DD MMM. YYYY"), true);    // set any value to Map
                    dates.push(item.ChangeDate);
                }

                //filter date and user
                //remove duplicates on dates
                if (edittedItems.findIndex(m => moment(new Date(m.ChangeDate)).format("MMM. DD YYYY HH:mm:ss") === moment(new Date(item.ChangeDate)).format("MMM. DD YYYY HH:mm:ss") && m.UserName == item.UserName && m.Action == item.Action && m.TableName == item.TableName) == -1)
                    edittedItems.push({ ChangeDate: moment(new Date(item.ChangeDate)).format("MMM. DD YYYY HH:mm:ss"), UserName: item.UserName, FullName: item.FullName, Action: item.Action, TableName: item.TableName });
            }

            for (const dateItems of dates) {
                timeline += `<a>${moment(new Date(dateItems)).format("MMM. DD YYYY")}</a>`;
                timeline += `<ul class="timeline-custom2">`;
                for (const dateUpdates of edittedItems) {
                    let _dateItem1 = moment(new Date(dateItems)).format("MMM. DD YYYY");
                    let _dateItem2 = moment(new Date(dateUpdates.ChangeDate)).format("MMM. DD YYYY");
                    let title = defineTransaction(dateUpdates.TableName);
                    let name = dateUpdates.FullName;
                    //console.log(title);

                    if (_dateItem1 == _dateItem2) {
                        let timePassed = moment(new Date(dateUpdates.ChangeDate)).fromNow();

                        if (dateUpdates.Action == "Create") {
                            timeline += `<li>
                                            <em>${name}</em> 
                                            <p>
                                               Created ${title}
                                                <small class="text-muted"><i class="fas fa-clock"></i> ${timePassed}</small>
                                             </p>`;
                        } else {
                            timeline += `<li>
                                            <em>${name}</em>
                                            <p>
                                                Updated ${title}
                                                <small class="text-muted"><i class="fas fa-clock"></i> ${timePassed}</small>
                                            </p>`;
                        }

                        if (dateUpdates.Action != "Create") {
                            timeline += '<p>';

                            for (const responseItems of response) {
                                if ((responseItems.ColumnName2 != 'DateModified' && responseItems.ColumnName2 != 'ModifiedById')) {
                                    let _changeDate1 = moment(new Date(responseItems.ChangeDate)).format("MMM. DD YYYY HH:mm:ss");
                                    let _changeDate2 = moment(new Date(dateUpdates.ChangeDate)).format("MMM. DD YYYY HH:mm:ss");

                                    timeline += '<p class="m-0">';

                                    if (_changeDate1 == _changeDate2 && responseItems.FullName == dateUpdates.FullName && responseItems.Action != 'Create') {
                                        timeline += `<strong>${capitalizeWords(responseItems.ColumnName2.replaceAll('_', ' '))} :</strong> <s class="text-danger">${formatItems(responseItems.ColumnName2, responseItems.OldDescription)}</s> <i class="mdi mdi-arrow-right-bold text-primary"></i> <span class="text-success">${formatItems(responseItems.ColumnName2, responseItems.NewDescription)}</span>`;
                                    }

                                    timeline += '</p>';
                                }
                            }

                            timeline += '</p>';
                        }

                        timeline += '</li>';
                    }
                }
                timeline += `</ul>`;
            }

            if (timeline == "") {
                timeline = "<h5>No Record Found!</h5>";
            }
            else {
                timeline = `<div>${timeline}</div>`;
            }

            $("#offcanvasHistoryLabel").html(`<span class="fas fa-history"></span> Transaction History`);
            $("#offcanvasHistoryBody").html(timeline);
        }
    });
}

function defineTransaction(transaction) {
    if (transaction == 'PurchaseOrder') { return 'Purchase Order' }
    else if (transaction == 'Payment') { return 'Check Voucher' }
    else if (transaction == 'Collection') { return 'Collection' }
    else if (transaction == 'Invoice') { return 'Billing'; }
    else if (transaction == 'Journal') { return 'Journal' }
    else { return ''; }
}

function formatItems(columnName, item) {
    let notFormatNum = ['CheckNo', 'ReferenceNo', 'ReferenceId', 'VatType', 'ApType', 'CurrencyId', 'EwtTypeId', 'PaymentRefNo', 'Payment Reference No.']

    //is number and not in notFormatNum
    if (!isNaN(item) && !notFormatNum.includes(columnName)) {
        return numeral(item).format("(0,0.00)")
    }

    //if not number and not equal to notFormatNum and is date
    if (isNaN(item) && !notFormatNum.includes(columnName) && (Object.prototype.toString.call(new Date(item)) === '[object Date]' && new Date(item) != "Invalid Date")) {
        return moment(new Date(item)).format("MMMM DD, YYYY");
    }

    if (columnName === 'ApTypeId') {
        if (item == 1) { return 'Purchase Order' }
        if (item == 2) { return 'Payroll' }
        if (item == 3) { return 'Cash Advance' }
        if (item == 4) { return 'Petty Cash' }
        if (item == 5) { return 'Others' }
    }

    return item;
}

function formatColumn(columnName) {
    return columnName;
}