//loadApplicationInfo();

//function loadApplicationInfo() {
//    let submitted_info = $('#submitted_info');
//    let approved_info = $('#approved_info');
//    let disapprove_info = $('#disapprove_info');
//    let withdrawn_info = $('#withdrawn_info');
//    let totalApplication = $('#totalApplications');
//    let loading_text = "<span class='spinner-border spinner-border-sm'></span>";

//    $.ajax({
//        url: baseUrl + "Home/GetApplicationsCount",

//        beforeSend: function () {
//            totalApplication.html(loading_text);
//        },
//        success: function (response) {
//            console.log(response);
//            totalApplication.text(response.TotalApplication);
//            withdrawn_info.val(response.TotalWithdrawn);
//            approved_info.val(response.TotalApprove);
//            submitted_info.val(response.TotalSubmitted);
//            disapprove_info.val(response.TotalDisApprove);

//            $('[data-plugin="knob"]').trigger('change');
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//            totalApplication.text(0);
//            $('[data-plugin="knob"]').val(0);
//        }
//    });
//}

//$(function () {
//    const $creditVerificationChart = document.querySelector('#ApexCharts_CreditVerificationStatus');
//    const $appCompletionChart = document.querySelector('#ApexCharts_AppCompletionStatus');
//    const $approveDeferRatioPie = document.querySelector('#ApexCharts_ApproveDeferRatioPie');
//    const $appStagesDonut = document.querySelector('#ApexCharts_AppStagesDonut');

//    var creditVerificationChart;
//    var appCompletionChart;
//    var approveDeferRatioPie;
//    var appStagesDonut;

//    if ($creditVerificationChart && $("#ApexCharts_CreditVerificationStatus svg").length === 0) {
//        var options = {
//            chart: {
//                height: 450,
//                type: 'bar',
//                toolbar: {
//                    show: true
//                }
//            },
//            noData: {
//                text: "No data for the selected period",
//                align: 'center',
//                verticalAlign: 'middle',
//                offsetX: 0,
//                offsetY: 0,
//                style: {
//                    color: undefined,
//                    fontSize: '14px',
//                    fontFamily: undefined
//                }
//            },
//            legend: {
//                show: false,
//            },
//            plotOptions: {
//                bar: {
//                    horizontal: false,
//                    borderRadius: 3,
//                    endingShape: 'rounded',
//                    distributed: true,
//                    columnWidth: '55%',
//                },
//            },
//            dataLabels: {
//                enabled: false
//            },
//            stroke: {
//                show: true,
//                width: 2,
//                colors: ['transparent']
//            },
//            colors: [
//                bs5ColorDictionary["secondary"],
//                bs5ColorDictionary["info"],
//                '#00ff00',
//                bs5ColorDictionary["success"],
//                bs5ColorDictionary["danger"],
//                bs5ColorDictionary["danger"],
//                bs5ColorDictionary["warning"]
//            ],
//            series: [{
//                name: 'Count',
//                data: [30, 68, 22, 15, 10, 8, 2]
//            }],
//            xaxis: {
//                categories: [
//                    ['Application', 'in Draft'],
//                    'Submitted',
//                    ['Developer', 'Verified'],
//                    ['Pag-IBIG', 'Verified'],
//                    ['Developer', 'Deferred'],
//                    ['Pag-IBIG', 'Deferred'],
//                    'Withdrawn'
//                ],
//                labels: {
//                    rotate: -45,
//                    trim: true
//                }
//            },
//            fill: {
//                opacity: 1
//            },
//            grid: {
//                row: {
//                    colors: ['transparent', 'transparent'], // takes an array which will be repeated on columns
//                    opacity: 0.2
//                },
//                borderColor: '#f1f3fa',
//                padding: {
//                    bottom: 10
//                }
//            },
//            tooltip: {
//                y: {
//                    formatter: function (val) {
//                        return numeral(val).format('0,0');
//                    }
//                }
//            }
//        }

//        creditVerificationChart = new ApexCharts($creditVerificationChart, options);
//        creditVerificationChart.render();
//    }

//    if ($appCompletionChart && $("#ApexCharts_AppCompletionStatus svg").length === 0) {
//        var options = {
//            chart: {
//                height: 450,
//                type: 'bar',
//                toolbar: {
//                    show: true
//                }
//            },
//            noData: {
//                text: "No data for the selected period",
//                align: 'center',
//                verticalAlign: 'middle',
//                offsetX: 0,
//                offsetY: 0,
//                style: {
//                    color: undefined,
//                    fontSize: '14px',
//                    fontFamily: undefined
//                }
//            },
//            legend: {
//                show: false,
//            },
//            plotOptions: {
//                bar: {
//                    horizontal: false,
//                    borderRadius: 3,
//                    endingShape: 'rounded',
//                    distributed: true,
//                    columnWidth: '55%',
//                },
//            },
//            dataLabels: {
//                enabled: false
//            },
//            stroke: {
//                show: true,
//                width: 2,
//                colors: ['transparent']
//            },
//            colors: [
//                bs5ColorDictionary["info"],
//                '#00ff00',
//                bs5ColorDictionary["success"],
//                bs5ColorDictionary["danger"],
//                bs5ColorDictionary["danger"],
//                bs5ColorDictionary["warning"]
//            ],
//            series: [{
//                name: 'Count',
//                data: [31, 10, 22, 55, 1, 9]
//            }],
//            xaxis: {
//                categories: [
//                    'Submitted',
//                    ['Developer', 'Verified'],
//                    ['Pag-IBIG', 'Verified'],
//                    ['Developer', 'Deferred'],
//                    ['Pag-IBIG', 'Deferred'],
//                    'Withdrawn'
//                ],
//                labels: {
//                    rotate: -45,
//                    trim: true
//                }
//            },
//            fill: {
//                opacity: 1
//            },
//            grid: {
//                row: {
//                    colors: ['transparent', 'transparent'], // takes an array which will be repeated on columns
//                    opacity: 0.2
//                },
//                borderColor: '#f1f3fa',
//                padding: {
//                    bottom: 10
//                }
//            },
//            tooltip: {
//                y: {
//                    formatter: function (val) {
//                        return numeral(val).format('0,0');
//                    }
//                }
//            }
//        }

//        appCompletionChart = new ApexCharts($appCompletionChart, options);
//        appCompletionChart.render();
//    }

//    if ($appStagesDonut && $("#ApexCharts_AppStagesDonut svg").length === 0) {
//        var options = {
//            series: [63, 29, 8],
//            chart: {
//                width: '100%',
//                height: 500,
//                type: 'donut',
//                offsetX: 0,
//                offsetY: 10,
//            },
//            plotOptions: {
//                pie: {
//                    offsetX: 0,
//                    offsetY: 50,
//                    startAngle: 0,
//                    endAngle: 360,
//                    donut: {
//                        size: '50%',
//                    },
//                    dataLabels: {
//                        offset: -5,
//                        minAngleToShowLabel: 10
//                    },
//                }
//            },
//            dataLabels: {
//                enabled: true
//            },
//            labels: ["Credit Verification", "Application Completion", "Post-approval"],
//            fill: {
//                type: 'solid',
//                colors: ['#126387', '#EE6A25', '#186A24']
//            },
//            colors: ['#126387', '#EE6A25', '#186A24'],
//            legend: {
//                //width: 200,
//                position: 'top',
//                floating: false,
//                fontSize: '14px',
//                //horizontalAlign: 'center',
//                colors: ['#126387', '#EE6A25', '#186A24'],
//                offsetX: 0,
//                offsetY: 0,
//            },
//            responsive: [{
//                breakpoint: 480,
//                options: {
//                    chart: {
//                        width: 200
//                    }
//                }
//            }]
//        };

//        appStagesDonut = new ApexCharts($appStagesDonut, options);
//        appStagesDonut.render();
//    }

//    if ($approveDeferRatioPie && $("#ApexCharts_ApproveDeferRatioPie svg").length === 0) {
//        var options = {
//            series: [33, 67],
//            chart: {
//                width: '100%',
//                height: 500,
//                type: 'pie',
//                offsetX: 0,
//                offsetY: 10,
//            },
//            plotOptions: {
//                pie: {
//                    offsetX: 0,
//                    offsetY: 50,
//                    startAngle: 0,
//                    endAngle: 360,
//                    dataLabels: {
//                        offset: -5,
//                        minAngleToShowLabel: 10
//                    },
//                }
//            },
//            dataLabels: {
//                enabled: true
//            },
//            labels: ["Approved", "Deferred"],
//            fill: {
//                type: 'solid',
//                colors: ['#126387', '#EE6A25']
//            },
//            colors: ['#126387', '#EE6A25'],
//            //legend: {
//            //    width: 200,
//            //    height: 100,
//            //    position: 'bottom',
//            //    horizontalAlign: 'center',
//            //    formatter: function (val, opts) {
//            //        return val;
//            //    },
//            //    colors: [bs5ColorDictionary["primary"], bs5ColorDictionary["warning"]],
//            //    floating: false,
//            //    fontSize: '14px',
//            //    offsetX: 0,
//            //    offsetY: 10
//            //}
//            legend: {
//                show: true,
//                height: 100,
//                position: 'bottom',
//                //horizontalAlign: 'center',
//                floating: false,
//                fontSize: '14px',
//                offsetX: 0,
//                offsetY: 10
//            },
//        };

//        approveDeferRatioPie = new ApexCharts($approveDeferRatioPie, options);
//        approveDeferRatioPie.render();
//    }

//    $(`[data-plugin="counterup"]`).counterup();
//});

$(async function () {
    const $creditVerificationChart = document.querySelector('#ApexCharts_CreditVerificationStatus');
    const $appCompletionChart = document.querySelector('#ApexCharts_AppCompletionStatus');
    const $approveDeferRatioPie = document.querySelector('#ApexCharts_ApproveDeferRatioPie');
    const $appStagesDonut = document.querySelector('#ApexCharts_AppStagesDonut');

    var creditVerificationChart;
    var appCompletionChart;
    var approveDeferRatioPie;
    var appStagesDonut;

    loadApplications();
    await CreditVerificationChart();
    await ApplicationCompletionChart();
    await StageAndStatusChart();

    //#region Functions

    function loadApplications() {
        let info_for_gov_approval = $("#info_for_gov_approval");
        let info_for_credit_verification = $("#info_for_credit_verification");
        let info_for_new_applications = $("#info_for_new_applications");
        let info_for_post_approval = $("#info_for_post_approval");
        let loading_text = "<span class='spinner-border spinner-border-sm'></span>";

        $.ajax({
            url: baseUrl + "Applicants/GetTotalApplicants",
            beforeSend: function () {
                info_for_gov_approval.html(loading_text);
                info_for_credit_verification.html(loading_text);
                info_for_new_applications.html(loading_text);
                info_for_post_approval.html(loading_text);
            },
            success: function (response) {
                info_for_gov_approval.html(`<span data-plugin="counterup">${response.NeedsPagibigApproval}</span>`);
                info_for_credit_verification.html(`<span data-plugin="counterup">${response.CreditVerifStage}</span>`);
                info_for_new_applications.html(`<span data-plugin="counterup">${response.NewApplication}</span>`);
                info_for_post_approval.html(`<span data-plugin="counterup">${response.PostApproval}</span>`);

                $("[data-plugin='counterup']").counterUp();
            },
            error: function (error) {
                info_for_gov_approval.html(`<span data-plugin="counterup">${0}</span>`);
                info_for_credit_verification.html(`<span data-plugin="counterup">${0}</span>`);
                info_for_new_applications.html(`<span data-plugin="counterup">${0}</span>`);
                info_for_post_approval.html(`<span data-plugin="counterup">${0}</span>`);

                $("[data-plugin='counterup']").counterUp();
            }
        });
    }

    async function CreditVerificationChart() {
        let result = await GetCreditVerification();

        if (!result)
            return;

        if ($creditVerificationChart && $("#ApexCharts_CreditVerificationStatus svg").length === 0) {
            var options = {
                chart: {
                    height: 450,
                    type: 'bar',
                    toolbar: {
                        show: true
                    }
                },
                noData: {
                    text: "No data for the selected period",
                    align: 'center',
                    verticalAlign: 'middle',
                    offsetX: 0,
                    offsetY: 0,
                    style: {
                        color: undefined,
                        fontSize: '14px',
                        fontFamily: undefined
                    }
                },
                legend: {
                    show: false,
                },
                plotOptions: {
                    bar: {
                        horizontal: false,
                        borderRadius: 3,
                        endingShape: 'rounded',
                        distributed: true,
                        columnWidth: '55%',
                    },
                },
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    show: true,
                    width: 2,
                    colors: ['transparent']
                },
                colors: [
                    bs5ColorDictionary["secondary"],
                    bs5ColorDictionary["info"],
                    '#00ff00',
                    bs5ColorDictionary["success"],
                    bs5ColorDictionary["danger"],
                    bs5ColorDictionary["danger"],
                    bs5ColorDictionary["warning"]
                ],
                series: [{
                    name: 'Count',
                    data:
                        [
                            result.ApplicationInDraft,
                            result.Submitted,
                            result.DeveloperVerified,
                            result.PagibigVerified,
                            result.PagibigDeferred,
                            result.DeveloperDeferred,
                            result.Withdrawn
                        ]
                }],
                xaxis: {
                    categories: [
                        ['Application', 'in Draft'],
                        'Submitted',
                        ['Developer', 'Verified'],
                        ['Pag-IBIG', 'Verified'],
                        ['Developer', 'Deferred'],
                        ['Pag-IBIG', 'Deferred'],
                        'Withdrawn'
                    ],
                    labels: {
                        rotate: -45,
                        trim: true
                    }
                },
                fill: {
                    opacity: 1
                },
                grid: {
                    row: {
                        colors: ['transparent', 'transparent'], // takes an array which will be repeated on columns
                        opacity: 0.2
                    },
                    borderColor: '#f1f3fa',
                    padding: {
                        bottom: 10
                    }
                },
                tooltip: {
                    y: {
                        formatter: function (val) {
                            return numeral(val).format('0,0');
                        }
                    }
                }
            }

            creditVerificationChart = new ApexCharts($creditVerificationChart, options);
            creditVerificationChart.render();
        }
    }

    async function ApplicationCompletionChart() {
        var result = await GetApplicationVerification();

        if ($appCompletionChart && $("#ApexCharts_AppCompletionStatus svg").length === 0) {
            var options = {
                chart: {
                    height: 450,
                    type: 'bar',
                    toolbar: {
                        show: true
                    }
                },
                noData: {
                    text: "No data for the selected period",
                    align: 'center',
                    verticalAlign: 'middle',
                    offsetX: 0,
                    offsetY: 0,
                    style: {
                        color: undefined,
                        fontSize: '14px',
                        fontFamily: undefined
                    }
                },
                legend: {
                    show: false,
                },
                plotOptions: {
                    bar: {
                        horizontal: false,
                        borderRadius: 3,
                        endingShape: 'rounded',
                        distributed: true,
                        columnWidth: '55%',
                    },
                },
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    show: true,
                    width: 2,
                    colors: ['transparent']
                },
                colors: [
                    bs5ColorDictionary["info"],
                    '#00ff00',
                    bs5ColorDictionary["success"],
                    bs5ColorDictionary["danger"],
                    bs5ColorDictionary["danger"],
                    bs5ColorDictionary["warning"]
                ],
                series: [{
                    name: 'Count',
                    data:
                        [
                            result.Submitted,
                            result.DeveloperVerified,
                            result.PagibigVerified,
                            result.PagibigDeferred,
                            result.DeveloperDeferred,
                            result.Withdrawn
                        ]
                }],
                xaxis: {
                    categories: [
                        'Submitted',
                        ['Developer', 'Verified'],
                        ['Pag-IBIG', 'Verified'],
                        ['Developer', 'Deferred'],
                        ['Pag-IBIG', 'Deferred'],
                        'Withdrawn'
                    ],
                    labels: {
                        rotate: -45,
                        trim: true
                    }
                },
                fill: {
                    opacity: 1
                },
                grid: {
                    row: {
                        colors: ['transparent', 'transparent'], // takes an array which will be repeated on columns
                        opacity: 0.2
                    },
                    borderColor: '#f1f3fa',
                    padding: {
                        bottom: 10
                    }
                },
                tooltip: {
                    y: {
                        formatter: function (val) {
                            return numeral(val).format('0,0');
                        }
                    }
                }
            }

            appCompletionChart = new ApexCharts($appCompletionChart, options);
            appCompletionChart.render();
        }
    }

    async function StageAndStatusChart() {
        var result = await GetApplicationStatusAndStages();

        if (!result)
            return;

        if ($appStagesDonut && $("#ApexCharts_AppStagesDonut svg").length === 0) {
            var options = {
                series:
                    [
                        result.CreditVerifStage,
                        result.AppCompletionStage,
                        result.PostApprovalStage
                    ],
                chart: {
                    width: '100%',
                    height: 500,
                    type: 'donut',
                    offsetX: 0,
                    offsetY: 10,
                },
                plotOptions: {
                    pie: {
                        offsetX: 0,
                        offsetY: 50,
                        startAngle: 0,
                        endAngle: 360,
                        donut: {
                            size: '50%',
                        },
                        dataLabels: {
                            offset: -5,
                            minAngleToShowLabel: 10
                        },
                    }
                },
                dataLabels: {
                    enabled: true
                },
                labels: ["Credit Verification", "Application Completion", "Post-approval"],
                fill: {
                    type: 'solid',
                    colors: ['#126387', '#EE6A25', '#186A24']
                },
                colors: ['#126387', '#EE6A25', '#186A24'],
                legend: {
                    //width: 200,
                    position: 'top',
                    floating: false,
                    fontSize: '14px',
                    //horizontalAlign: 'center',
                    colors: ['#126387', '#EE6A25', '#186A24'],
                    offsetX: 0,
                    offsetY: 0,
                },
                responsive: [{
                    breakpoint: 480,
                    options: {
                        chart: {
                            width: 200
                        }
                    }
                }]
            };

            appStagesDonut = new ApexCharts($appStagesDonut, options);
            appStagesDonut.render();
        }

        if ($approveDeferRatioPie && $("#ApexCharts_ApproveDeferRatioPie svg").length === 0) {
            var options = {
                series: [result.TotalApproved, result.TotalDeferred],
                chart: {
                    width: '100%',
                    height: 500,
                    type: 'pie',
                    offsetX: 0,
                    offsetY: 10,
                },
                plotOptions: {
                    pie: {
                        offsetX: 0,
                        offsetY: 50,
                        startAngle: 0,
                        endAngle: 360,
                        dataLabels: {
                            offset: -5,
                            minAngleToShowLabel: 10
                        },
                    }
                },
                dataLabels: {
                    enabled: true
                },
                labels: ["Approved", "Deferred"],
                fill: {
                    type: 'solid',
                    colors: ['#126387', '#EE6A25']
                },
                colors: ['#126387', '#EE6A25'],
                //legend: {
                //    width: 200,
                //    height: 100,
                //    position: 'bottom',
                //    horizontalAlign: 'center',
                //    formatter: function (val, opts) {
                //        return val;
                //    },
                //    colors: [bs5ColorDictionary["primary"], bs5ColorDictionary["warning"]],
                //    floating: false,
                //    fontSize: '14px',
                //    offsetX: 0,
                //    offsetY: 10
                //}
                legend: {
                    show: true,
                    height: 100,
                    position: 'bottom',
                    //horizontalAlign: 'center',
                    floating: false,
                    fontSize: '14px',
                    offsetX: 0,
                    offsetY: 10
                },
            };

            approveDeferRatioPie = new ApexCharts($approveDeferRatioPie, options);
            approveDeferRatioPie.render();
        }
    }

    //#enderegion Functions

    //#region Getters
    async function GetCreditVerification() {
        const response = await $.ajax({
            url: baseUrl + "Applicants/GetTotalCreditVerif",
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    async function GetApplicationVerification() {
        const response = await $.ajax({
            url: baseUrl + "Applicants/GetTotalAppVerif",
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    async function GetApplicationStatusAndStages() {
        const response = $.ajax({
            url: baseUrl + "Applicants/GetTotalAppStatusAndStage",
            method: 'get',
            dataType: 'json'
        });

        return response;
    }
    //#endregion Getters
});