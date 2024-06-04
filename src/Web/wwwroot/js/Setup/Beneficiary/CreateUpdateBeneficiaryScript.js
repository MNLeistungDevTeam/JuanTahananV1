const applicantInfoIdVal = $(`[name='ApplicantsPersonalInformationModel.Id']`).val();
const roleName = $("#txt_role_name").val();
var developerDropdown, $developerDropdown;
var houseUnitDropdown, $houseUnitDropdown;
var locationDropdown, $locationDropdown;
var projectDropdown, $projectDropdown;
$(function () {
    var developerInput;
    var xhr;
    var xhr1;
    var xhr2;
    var resources = [];

    $(".selectize").selectize();
    $('.calendarpicker').flatpickr();

    $('.pagibigInputMask').inputmask({
        mask: "9999-9999-9999",
        placeholder: 'X',
        //clearIncomplete: true
    });

    $('.mobileNumInputMask').inputmask({ mask: "9999-999-9999" });

    initializeDecimalInputMask(".decimalInputMask5", 2);

    $("#btn_save").attr('disabled', true);

    /*    initializeSelectizeDev();*/

    var locationVal = $('[name="PropertyLocationId"]').attr('data-value');
    var houseUnitVal = $('[name="PropertyUnitId"]').attr('data-value');
    var projectVal = $('[name="PropertyProjectId"]').attr('data-value');
    var developerVal = $('[name="PropertyDeveloperId"]').attr('data-value');

    $locationDropdown = $(`[name='PropertyLocationId']`).selectize({
        valueField: 'Id',
        labelField: 'Name',
        searchField: 'Name',
        preload: true,
        search: false,

        load: function (query, callback) {
            $.ajax({
                url: baseUrl + 'Beneficiary/GetLocations',
                data: {
                    projectId: $('[name="PropertyProjectId"]').val(),
                    developerId: $('[name="PropertyDeveloperId"]').val()
                },
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
        onChange: function (value) {
            houseUnitDropdown.setValue("");
            houseUnitDropdown.disable();
            houseUnitDropdown.clearOptions();
            houseUnitDropdown.load(function (callback) {
                xhr2 && xhr2.abort();
                xhr2 = $.ajax({
                    url: baseUrl + "Beneficiary/GetUnits",
                    data: {
                        projectId: $('[name="PropertyProjectId"]').val(),
                        developerId: $('[name="PropertyDeveloperId"]').val()
                    },
                    success: function (results) {
                        console.log(results)

                        try {
                            if (!results.length) houseUnitDropdown.disable();
                            else houseUnitDropdown.enable();
                            callback(results)
                        } catch {
                            callback(results);
                        }
                    },
                    onComplete: function () {
                        this.setValue(locationVal);
                    },
                    error: function () {
                        callback();
                    }
                })
            });

        },

        //render: {
        //    item: function (item, escape) {
        //        return ("<div>" +
        //            escape(item.Name) +
        //            "</div>"
        //        );
        //    },
        //    option: function (item, escape) {
        //        return ("<div class='py-1 px-2'>" +
        //            escape(item.Name) +
        //            "</div>"
        //        );
        //    }
        //},
    });

    locationDropdown = $locationDropdown[0].selectize;

    $houseUnitDropdown = $(`[name='PropertyUnitId']`).selectize({
        valueField: 'Id',
        labelField: 'Description',
        searchField: 'Description',
        preload: true,
        search: false,
        load: function (query, callback) {
            $.ajax({
                url: baseUrl + 'Beneficiary/GetUnits',
                data: {
                    projectId: $('[name="PropertyProjectId"]').val(),
                    developerId: $('[name="PropertyDeveloperId"]').val()
                },

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

        //render: {
        //    item: function (item, escape) {
        //        return ("<div>" +
        //            escape(item.Description) +
        //            "</div>"
        //        );
        //    },
        //    option: function (item, escape) {
        //        return ("<div class='py-1 px-2'>" +
        //            escape(item.Description) +
        //            "</div>"
        //        );
        //    }
        //},
    });

    houseUnitDropdown = $houseUnitDropdown[0].selectize;

    $projectDropdown = $(`[name='PropertyProjectId']`).selectize({
        valueField: 'Id',
        labelField: 'Name',
        searchField: 'Name',
        preload: true,
        search: false,

        load: function (query, callback) {
            $.ajax({
                url: baseUrl + 'Beneficiary/GetProjects',
                data: {
                    developerId: $('[name="PropertyDeveloperId"]').val()
                },
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

        onChange: function (value) {



            locationDropdown.setValue("");
            locationDropdown.disable();
            locationDropdown.clearOptions();
            locationDropdown.load(function (callback) {
                xhr1 && xhr1.abort();
                xhr1 = $.ajax({
                    url: baseUrl + "Beneficiary/GetLocations",
                    data: {
                        projectId: value,
                        developerId: $('[name="PropertyDeveloperId"]').val()
                    },
                    success: function (results) {
                 

                        try {
                            if (!results.length) locationDropdown.disable();
                            else locationDropdown.enable();
                            callback(results)
                        } catch {
                            callback(results);
                        }
                    },
                    onComplete: function () {
                        this.setValue(locationVal);
                    },
                    error: function () {
                        callback();
                    }
                })
            });
        },
    });

    projectDropdown = $projectDropdown[0].selectize;

    $developerDropdown = $(`[name='PropertyDeveloperId']`).selectize({
        valueField: 'Id',
        labelField: 'Name',
        searchField: 'Name',
        preload: true,
        search: false,

        load: function (query, callback) {
            $.ajax({
                url: baseUrl + 'Beneficiary/GetDevelopers',

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
        },
        onChange: function (value) {
            projectDropdown.setValue("");
            projectDropdown.disable();
            projectDropdown.clearOptions();
            projectDropdown.load(function (callback) {
                xhr && xhr.abort();
                xhr = $.ajax({
                    url: baseUrl + "Beneficiary/GetProjects/",
                    data: {
                        developerId: value
                    },
                    success: function (results) {
                        try {
                            if (!results.length) projectDropdown.disable();
                            else projectDropdown.enable();
                            callback(results)
                        } catch {
                            callback(results);
                        }
                    },
                    onComplete: function () {
                        this.setValue(projectVal);
                    },
                    error: function () {
                        callback();
                    }
                })
            });
        },
    });

    developerDropdown = $developerDropdown[0].selectize;

    developerDropdown.on('load', function (options) {
        if (developerVal > 0) developerDropdown.lock();

        developerDropdown.setValue(developerVal || '');

        resourceCounter("developer");
        developerDropdown.off('load');
    });

    projectDropdown.on('load', function (options) {
        if (developerVal > 0) projectDropdown.lock();

        setTimeout(function () {
            projectDropdown.setValue(projectVal || '');
            resourceCounter("project");
        }, 800)

        projectDropdown.off('load');
    });

    locationDropdown.on('load', function (options) {
        setTimeout(function () {
            locationDropdown.setValue(locationVal || '');
            locationDropdown.lock();
            resourceCounter("location");
        }, 1000)

        locationDropdown.off('load');
    });

    houseUnitDropdown.on('load', function (options) {
        setTimeout(function () {
            houseUnitDropdown.setValue(houseUnitVal || '');
            houseUnitDropdown.lock();
            resourceCounter("unit");
        }, 1200)

        houseUnitDropdown.off('load');
    });

    //setTimeout(function () {
    //    houseUnitDropdown.setValue(houseUnitVal || '');
    //    houseUnitDropdown.lock();
    //}, 1000)

    //function initializeSelectizeDev() {
    //    developerInput = $(`[id="PropertyDeveloperName"]`).selectize({
    //        valueField: 'PropertyDeveloperName',
    //        labelField: 'PropertyDeveloperName',
    //        searchField: ['PropertyDeveloperName'],
    //        placeholder: '(Select here)',
    //        preload: true,
    //        load: function (query, callback) {
    //            $.ajax({
    //                //url: baseUrl + "Budget/GetBudget/created",
    //                url: baseUrl + "",
    //                success: function (results) {Beneficiary/GetPropertyDevelopers
    //                    try {
    //                        callback(results);
    //                    } catch (e) {
    //                        callback();
    //                    }
    //                }
    //            });
    //        },
    //        render: {
    //            item: function (item, escape) {
    //                return ("<div>" +
    //                    escape(item.PropertyDeveloperName) +
    //                    "</div>"
    //                );
    //            },
    //            option: function (item, escape) {
    //                return ("<div class='py-1 px-2'>" +
    //                    escape(item.PropertyDeveloperName) +
    //                    "</div>"
    //                );
    //            }
    //        }
    //    });
    //}

    rebindValidators();

    //#region Barrower

    // Set value for BarrowersInformationModel_BirthDate
    setDateValue('#BirthDate');

    $('#BirthDate').on('change', function () {
        var birthdate = moment($(this).val());
        var today = moment();
        var age = today.diff(birthdate, 'years');

        console.log("Age: " + age);

        // Check if age is 21 or older
        if (age < 21) {
            console.log("User is 21 or older");
            messageBox('You have to be at least 21 years old to proceed', 'error');

            $(this).val('');
        }
    });

    $('#MobileNumber').on('input', function () {
        var inputValue = $(this).val().toString();

        //maximum 25 characters
        if (inputValue.length > 25) {
            //alert("Input value exceeds 7 characters!");
            $(this).val(inputValue.substring(0, 25));

            messageBox("Mobile Number must not exceed to 25 characters", "danger", true);

            $('#MobileNumber').trigger('invalid');
        }
    });

    //#endregion

    //#region Methods

    function rebindValidators() {
        let $form = $("#frm_beneficiary");
        let button = $("#btn_save");

        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate($form.data("unobtrusiveValidation").options);
        $form.data("validator").settings.ignore = "";

        $form.on("submit", function (e) {
            e.preventDefault();
            let formData = new FormData(e.target);

            if ($(this).valid() == false) {
                messageBox("Please fill out all required fields!", "danger", true);
                return;
            }

            // Use SweetAlert for confirmation
            Swal.fire({
                title: 'Are you sure?',
                text: "You are about to submit the form. Proceed?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, submit',
                cancelButtonText: 'No, cancel'
            }).then((result) => {
                if (result.isConfirmed) {
                    // User confirmed, proceed with form submission
                    $.ajax({
                        url: $(this).attr("action"),
                        method: $(this).attr("method"),
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        beforeSend: function () {
                            button.html("<span class='spinner-border spinner-border-sm'></span> Saving...");
                            button.attr({ disabled: true });
                            $("#beneficiary-overlay").removeClass('d-none');
                        },
                        success: function (response) {
                            // Success message handling
                            let recordId = $("input[name='User.Id']").val();
                            console.log(recordId);
                            let type = (recordId == 0 ? "Added!" : "Updated!");
                            let successMessage = `Beneficiary Successfully ${type}`;
                            messageBox(successMessage, "success", true);

                            // Redirect handling
                            if (applicantInfoIdVal == 0) {
                                setTimeout(function () {
                                    $("#beneficiary-overlay").addClass('d-none');
                                    window.location.href = "/Beneficiary/BeneficiaryInformation/" + response;
                                }, 2000);
                            } else {
                                var link = "Beneficiary/Index";

                                setTimeout(function () {
                                    $("#beneficiary-overlay").addClass('d-none');
                                    // Redirect to the specified location
                                    window.location.href = baseUrl + link;
                                }, 2000);
                            }
                            // Reset button state
                            button.attr({ disabled: false });
                            button.html("<span class='mdi mdi-content-save-outline'></span> Save");
                        },
                        error: function (response) {
                            // Error message handling
                            messageBox(response.responseText, "danger");
                            $("#beneficiary-overlay").addClass('d-none');
                            button.html("<span class='mdi mdi-content-save-outline'></span> Save");
                            button.attr({ disabled: false });
                        }
                    });
                }
            });
        });
    }

    function setDateValue(selector) {
        let dataValue = $(selector).attr('data-value');
        if (dataValue && dataValue.trim() !== '') {
            $(selector).val(moment(dataValue).format("YYYY/MM/DD"));
        }
    }

    function lengthValidator() {
        var isValid = true;
        var elements = [
            { name: 'ApplicantsPersonalInformationModel.PagibigNumber', requiredLength: 12, message: "Mobile number" },
            //{ name: 'Company.TelNo', requiredLength: 8, message: "Telephone number" },
            //{ name: 'Company.FaxNo', requiredLength: 10, message: "Fax number" },
            //{ name: 'Company.Tin', requiredLength: 13, message: "TIN number" },
            //{ name: 'Company.RepresentativeTin', requiredLength: 13, message: "Company Representative TIN number" }
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

    function initializeSelectizeDev() {
        developerInput = $(`[id="PropertyDeveloperName"]`).selectize({
            valueField: 'PropertyDeveloperName',
            labelField: 'PropertyDeveloperName',
            searchField: ['PropertyDeveloperName'],
            placeholder: '(Select here)',
            preload: true,
            load: function (query, callback) {
                $.ajax({
                    //url: baseUrl + "Budget/GetBudget/created",
                    url: baseUrl + "Beneficiary/GetPropertyDevelopers",
                    success: function (results) {
                        try {
                            callback(results);
                        } catch (e) {
                            callback();
                        }
                    }
                });
            },
            render: {
                item: function (item, escape) {
                    return ("<div>" +
                        escape(item.PropertyDeveloperName) +
                        "</div>"
                    );
                },
                option: function (item, escape) {
                    return ("<div class='py-1 px-2'>" +
                        escape(item.PropertyDeveloperName) +
                        "</div>"
                    );
                }
            }
        });
    }

    const resourceCounter = (item) => {
        if (resources.indexOf(item) == -1)
            resources.push(item);

        if (resources.length == 4) {
            $("#btn_save").attr('disabled', false);
        }
    }

    //#endregion
});