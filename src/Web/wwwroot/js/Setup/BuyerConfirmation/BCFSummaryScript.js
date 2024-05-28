$(function () {
    refreshFilterSetting();
    loadBcfSummaryList();

    function loadBcfSummaryList() {
        $.ajax({
            url: baseUrl + "BuyerConfirmation/BCFSummaryData",
            method: 'GET',
            beforeSend: function () {
                $("#div_bcflist").empty();
            },
            success: function (data) {
                console.log(data);
                data = sortData(data);
                // Use a Set to track unique combinations and an array to store the unique items
                const uniqueCombinations = new Set();
                const uniqueData = [];

                data.forEach(item => {
                    const combination = `${item.PropertyProjectName}-${item.PropertyLocationName}`;
                    if (!uniqueCombinations.has(combination)) {
                        uniqueCombinations.add(combination);
                        uniqueData.push({
                            PropertyProjectName: item.PropertyProjectName,
                            PropertyLocationName: item.PropertyLocationName,
                            Count: 1,
                            PropertyProjectId: item.PropertyProjectId,
                            PropertyLocationId: item.PropertyLocationId,
                            LastUpdate: item.LastUpdate // Include LastUpdate here
                        });
                    } else {
                        // If combination already exists, find the corresponding item in uniqueData and increment its count
                        const existingItem = uniqueData.find(uniqueItem =>
                            uniqueItem.PropertyProjectName === item.PropertyProjectName &&
                            uniqueItem.PropertyLocationName === item.PropertyLocationName
                        );
                        existingItem.Count += 1;
                    }
                });
                // Generate HTML using the unique data
                const bcfdatalist = uniqueData.map(item => {
                    // Format LastUpdate date and time
                    let dateApplied = "";
                    if (item.LastUpdate && item.LastUpdate.trim() !== "") {
                        let lastUpdateDate = moment(item.LastUpdate).format('MMM D, YYYY'); // Format date as "Aug 15, 2024"
                        let lastUpdateTime = moment(item.LastUpdate).format('HH:mm'); // Format time in 24-hour format without AM/PM
                        let lastUpdateTimeWithPM = moment(item.LastUpdate).format('h:mm a'); // Format time with AM/PM

                        dateApplied = `${lastUpdateDate}, at ${lastUpdateTimeWithPM}`; // Concatenate date and time with AM/PM
                    }

                    // Generate HTML for each item
                    return `
                <div class="col-xxl-6 bcf-card-content">
                    <div class="card rounded-4 beneficiary-card border border-0 bg-light">
                        <div class="card-header rounded-4 rounded-bottom bg-light">
                            <div class="row align-items-center">
                                <div class="col">
                                    <h6 class="m-0 font-16 fw-semibold text-primary text-center bcf-title">
                                        <span id="bcfProjectName">${item.PropertyProjectName}</span> - <span id="bcfLocationName">${item.PropertyLocationName}</span> 
                                    </h6>
                                </div>
                            </div>
                        </div>
                        <div class="card-body p-0">
                            <div class="row d-flex justify-content-center">
                                <div class="col-10 card shadow-lg">
                                    <div class="card-body rounded-4">
                                        <div class="d-flex justify-content-between align-items-center">
                                            <div class="d-flex align-items-center">
                                                <div class="dl-icon-logo me-4">
                                                    <img src="/img/excel_logo.png" style="width: 70px;">
                                                </div>
                                                <div class="dl-filename">
                                                    <p class="fw-light fs-5 text-dark bcfFileName">WLF251_${item.PropertyProjectName}_${item.PropertyLocationName}.xlsx</p>
                                                    <input type="hidden" class="bcfprojectId" value="${item.PropertyProjectId}">
                                                    <input type="hidden" class="bcflocationId" value="${item.PropertyLocationId}">
                                                    <p class="fw-lighter fs-6 mb-0">${item.Count} Prequalified Beneficiaries</p>
                                                    <p class="fw-lighter fs-6 mb-0">Last updated ${dateApplied}</p>
                                                </div>
                                            </div>
                                            <div class="ms-auto d-flex flex-column align-items-center justify-content-end">
                                                <button class="btn btn-link download-btn">
                                                    <i class="fas fa-download"></i>
                                                </button>
                                                <button class="btn btn-link print-btn">
                                                    <i class="fas fa-print"></i>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
                }).join('');

                // Append the generated HTML to the div
                $("#div_bcflist").append(bcfdatalist);
            },
            error: function (xhr, status, error) {
                console.error("Error fetching data:", error);
                // Handle error appropriately here
            },
            complete: function () {
                $(`[id="searchBar"]`).trigger('input');
            }
        });
    }

    // Event delegation to handle click event on .beneficiary-card
    $("#div_bcflist").on("click", ".download-btn", function () {
        // Find the .bcfFileName within the closest card

        var card = $(this).closest(".beneficiary-card");

        var fileName = card.find(".bcfFileName").text().trim();

        var locationId = card.find(".bcflocationId").attr('value');
        var projectId = card.find(".bcfprojectId").attr('value');

        downloadBcfProject(fileName, locationId, projectId);
    });

    // Search bar event
    $(`[id="searchBar"]`).on('input', function () {
        var value = $(this).val().toLowerCase();

        var isEmpty = $(`[id="div_bcflist"] .bcf-card-content`).toArray().some(function (elem) {
            //console.log($(elem).find('.bcf-title').text().trim().toLowerCase().indexOf(value) > -1);
            return $(elem).find('.bcf-title').text().trim().toLowerCase().indexOf(value) > -1;
        });

        $(`[id="div_bcflist"] .bcf-card-content`).filter(function () {
            let data = ($(this).find('.bcf-title').text().trim().toLowerCase().indexOf(value) > -1);
            $(this).toggle(data);
        });

        $(`#emptyEntry`).attr('hidden', isEmpty);
    });

    $(`[id="dropdown-selection"] a.dropdown-item`).on('click', function (e) {
        e.preventDefault();

        let dataType = $(this).data('type').split('-');

        let sortType = dataType[0]; // sort or order
        let sortCategory = dataType[1]; // ascending/descending, project/location/last_updated

        refreshFilterSetting(sortType, sortCategory);
        loadBcfSummaryList();
    });

    function downloadBcfProject(fileName, locationId, projectId) {
        $.ajax({
            url: baseUrl + `BuyerConfirmation/DownloadBCFSummary?locationId=${locationId}&projectId=${projectId}`,
            method: 'GET',
            xhrFields: {
                responseType: 'blob'
            },

            success: function (data, textstatus, request) {
                if (typeof (data) != "string") {
                    let a = document.createElement('a');
                    let url = window.URL.createObjectURL(data);

                    var disposition = request.getResponseHeader('Content-Disposition');
                    if (disposition && disposition.indexOf('attachment') !== -1) {
                        var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                        var matches = filenameRegex.exec(disposition);
                        if (matches != null && matches[1]) {
                            filename = matches[1].replace(/['"]/g, '');
                        }
                    }

                    a.href = url;
                    a.download = fileName;
                    document.body.append(a);
                    a.click();
                    a.remove();
                    window.URL.revokeObjectURL(url);

                    // Display a success message
                    messageBox("Summary report has been downloaded.", "success", true);
                }
                else {
                    toastr.error(textstatus, 'Error');
                }
            },
            error: function (xhr, response, error) {
                console.log(xhr, response, error);
                toastr.error(error, "An error occurred!");
            },
        });
    }

    function refreshFilterSetting(sortType, sortCategory) {
        // Visual Part
        if (!sortType && !sortCategory) {
            $(`[id="dropdown-selection"] a.dropdown-item[data-type^="sort-"]`).first().addClass("active");
            $(`[id="dropdown-selection"] a.dropdown-item[data-type^="order-"]`).first().addClass("active");
            $(`[id="searchBar"]`).trigger('input');
            return;
        }
        else {
            $(`[id="dropdown-selection"] a.dropdown-item[data-type^="${sortType}-${sortCategory}"]`).addClass("active");
            $(`[id="dropdown-selection"] a.dropdown-item[data-type^="${sortType}-"]:not([data-type="${sortType}-${sortCategory}"])`).removeClass("active");
        }

        // Logic Part:
    }

    function sortData(data) {
        let sortType = $(`[id="dropdown-selection"] a.dropdown-item.active[data-type^="sort-"]`).data('type').split('-')[1]; // project, location, last_update
        let orderType = $(`[id="dropdown-selection"] a.dropdown-item.active[data-type^="order-"]`).data('type').split('-')[1]; // ASC, DESC

        console.log(sortType);
        console.log(orderType);

        // PropertyProjectName
        // PropertyLocationName
        // LastUpdate

        if (sortType === 'project') {
            data = data.sort(function (a, b) {
                let nameA = a.PropertyProjectName.toLowerCase();
                let nameB = b.PropertyProjectName.toLowerCase();

                console.log(nameA > nameB);
                console.log(nameA < nameB);

                if (nameA === nameB) {
                    return 0;
                }
                else if (nameA < nameB) {
                    return orderType === 'asc' ? -1 : 1;
                }
            });
        }
        else if (sortType === 'location') {
            data = data.sort(function (a, b) {
                let nameA = a.PropertyLocationName.toLowerCase();
                let nameB = b.PropertyLocationName.toLowerCase();

                console.log(nameA > nameB);
                console.log(nameA < nameB);

                if (nameA === nameB) {
                    return 0;
                }
                else if (nameA < nameB) {
                    return orderType === 'asc' ? -1 : 1;
                }
            });
        }
        else if (sortType === 'last_updated') {
            data = data.sort(function (a, b) {
                //let aa = moment(a.LastUpdate).format('YYYY-MM-DD HH:mm:ss');
                //let bb = moment(b.LastUpdate).format('YYYY-MM-DD HH:mm:ss');
                //console.log(moment(a.LastUpdate) > moment(b.LastUpdate));
                //console.log(moment(a.LastUpdate) < moment(b.LastUpdate));

                if (moment(a.LastUpdate) === moment(b.LastUpdate)) {
                    return 0;
                }
                else if (moment(a.LastUpdate) < moment(b.LastUpdate)) {
                    return orderType === 'asc' ? -1 : 1;
                }
            });
        }

        return data;
    }
});