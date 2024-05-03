$(() => {
    var backgroundClassColors = [
        {
            approvalStatusNumbers: [1, 6],
            classColor: 'bg-primary',
        },
        {
            approvalStatusNumbers: [0],
            classColor: 'bg-secondary',
        },
        {
            approvalStatusNumbers: [2, 9],
            classColor: 'bg-danger',
        },
        {
            approvalStatusNumbers: [3, 7],
            classColor: 'bg-lightgreen',
        },
        {
            approvalStatusNumbers: [4, 8],
            classColor: 'bg-darkgreen',
        },
        {
            approvalStatusNumbers: [5, 10],
            classColor: 'bg-warning',
        },
        {
            approvalStatusNumbers: [11],
            classColor: 'bg-teal',
        },
    ];

    //var classColors = [
    //    {
    //        approvalStatusNumbers: [0],
    //        classColor: 'text-secondary',
    //        classTextColor: ''
    //    },
    //    {
    //        approvalStatusNumbers: [1, 6],
    //        classColor: 'text-primary',
    //        classTextColor: 'text-muted'
    //    },
    //    {
    //        approvalStatusNumbers: [2, 9],
    //        classColor: 'text-danger',
    //        classTextColor: 'text-danger'
    //    },
    //    {
    //        approvalStatusNumbers: [3, 4, 7, 8],
    //        classColor: 'text-success',
    //        classTextColor: 'text-muted'
    //    },
    //    {
    //        approvalStatusNumbers: [5, 10],
    //        classColor: 'text-warning',
    //        classTextColor: 'text-muted'
    //    },
    //    {
    //        approvalStatusNumbers: [11],
    //        classColor: 'text-info',
    //        classTextColor: 'text-muted'
    //    },
    //];

    loadData();

    function loadData() {
        loadRecentBeneficiaryApplication();
        loadTimeline();
    }

    function loadRecentBeneficiaryApplication() {
        $.ajax({
            url: baseUrl + 'Applicants/GetBeneficiaryActiveApplication',
            method: "GET",
            beforeSend: function (e) {
                // dark overlay
                //$(`[id="main-overlay"]`).removeClass('d-none');
                //$(`[id="application-tracker-overlay"]`).removeClass('d-none');
            },
            success: function (data) {
                if (data.ApplicationStatus === null) {
                    $(`[id="custom_ribbon"]`).addClass('d-none');
                }
                if (data.Code === "------") {
                    $(`[id="simplebarWrapper2"] .timeline-alt .process_display`).removeAttr(`hidden`);
                }

                let color = backgroundClassColors.find(a => a.approvalStatusNumbers.includes(data.ApprovalStatus));
                let selectedColor = color ? color.classColor : "bg-primary"

                // Status below the greeting
                $(`[id="status_contract"]`).addClass(selectedColor);
                $(`[id="status_contract"]`).html(data.ApplicationStatus || "No Application");

                $(`[id="reference_number"]`).html(data.Code || "-----");
                $(`[id="application_code"]`).html(data.Code === "------" ? "No Application found" : data.Code);
                $(`[id="requested_loan_amount"]`).html(data.LoanAmount !== 0 ? numeral(data.LoanAmount).format('0, 0.00') : "------"); // use numeral
                $(`[id="loan_term"]`).html(data.LoanYears !== 0 ? data.LoanYears : "--"); // use numeral
                $(`[id="project_location"]`).html(data.ProjectLocation || "-----");

                // Side card text and Lower left side card (Application Status)
                let appStatus = data.Stage || "No Application";
                let appStatusRemarks = "Kindly proceed submitting an application";
                let statusColor = `secondary`;

                $(`[id="beneficiary_sidecard"]`).removeClass('bg-warning');
                $(`[id="application_status"]`).removeClass('text-warning');

                // set color
                if ([0, 1, 2, 3, 5, 11].includes(data.ApprovalStatus)) {
                    // `Credit Verification`
                    statusColor = "warning";
                    appStatusRemarks = `Stage ends after Pag-IBIG credit verification`;
                }
                else if ([4, 6, 7, 9, 10].includes(data.ApprovalStatus)) {
                    // `Application Completion`
                    statusColor = "primary";
                    appStatusRemarks = `Stage ends after Pag-IBIG approval`;
                }
                else if (data.ApprovalStatus === 8) {
                    // `Post-Approval`
                    statusColor = "success";
                    appStatusRemarks = `Stage ends after disbursement of loan`;
                }

                $(`[id="beneficiary_sidecard"]`).addClass(`bg-${statusColor}`);
                $(`[id="application_status"]`).addClass(`text-${statusColor}`);
                $(`[id="beneficiary_sidecard"] [id="beneficiary_sidecard_text"]`).html(appStatus);
                $(`[id="application_status"]`).html(appStatus);
                $(`[id="application_status_remarks"]`).html(appStatusRemarks);

                // Lower Right small card
                // Credit History
                $(`[id="credit_history_status"]`).removeClass('text-success');
                $(`[id="credit_history_remarks"]`).removeClass('text-muted');

                if ([2, 9].includes(data.ApprovalStatus) && [2, 3, 5].includes(data.ApproverRoleId)) {
                    // Deferred by either Developer or Pag-IBIG (First Stage)
                    $(`[id="credit_history_status"]`).addClass('text-danger');
                    $(`[id="credit_history_remarks"]`).addClass('text-danger');

                    let roleMessage = {
                        3: "Deferred by Pag-IBIG",
                        2: "Deferred by Developer",
                        5: "Deferred by Developer"
                    };

                    $(`[id="credit_history_status"]`).html(roleMessage[data.ApproverRoleId]);
                    $(`[id="credit_history_remarks"]`).html("Review remarks and update your application");

                    $(`[id="text_status"]`).html(`
                        Your application has been <span class="text-danger">${roleMessage[data.ApproverRoleId]}</span>.
                        <span class="text-warning">Review Remarks</span> to see what updates are needed.
                    `);
                }
                else if (data.ApprovalStatus === null) {
                    // No Record
                    $(`[id="credit_history_status"]`).addClass('text-secondary');
                    $(`[id="credit_history_remarks"]`).addClass('text-muted');

                    $(`[id="credit_history_label"]`).html(`Your credit history has`);
                    $(`[id="credit_history_status"]`).html("No Record");
                    $(`[id="credit_history_remarks"]`).html("Kindly submit an application first");

                    $(`[id="text_status"]`).html(`
                        You have no records submitted yet,
                        kindly submit an initial application first.
                    `);
                }
                else if (data.ApprovalStatus === 0) {
                    // Application in Draft
                    $(`[id="credit_history_status"]`).addClass('text-secondary');
                    $(`[id="credit_history_remarks"]`).addClass('text-secondary');

                    $(`[id="credit_history_status"]`).html("In Draft");
                    $(`[id="credit_history_remarks"]`).html("Kindly complete and submit requirements");

                    $(`[id="text_status"]`).html(`
                        Your application is <span class="fw-bolder text-secondary">in draft</span>.
                        Kindly <span class="text-warning">submit requirements</span> to proceed.
                    `);
                }
                else if (data.ApprovalStatus === 1) {
                    $(`[id="credit_history_status"]`).addClass('text-primary');
                    $(`[id="credit_history_remarks"]`).addClass('text-secondary');

                    //$(`[id="process_status"]`).html(`recently submitted.`);
                    //$(`[id="process_name"]`).html(`Kindly wait for a developer to verify your application.`);

                    //$(`[id="credit_history_label"]`).html(`Your credit history has`);
                    $(`[id="credit_history_status"]`).html("Recently Submitted");
                    $(`[id="credit_history_remarks"]`).html("Kindly wait for a developer to verify your application");

                    $(`[id="text_status"]`).html(`
                        Your application is <span class="fw-bolder text-primary">recently submitted</span>.
                        Kindly <span class="text-warning">wait for a developer to verify</span> your application.
                    `);
                }
                else if ([3, 4].includes(data.ApprovalStatus)) {
                    $(`[id="credit_history_status"]`).addClass('text-success');
                    $(`[id="credit_history_remarks"]`).addClass('text-muted');

                    let roleMessage = {
                        3: "Developer",
                        4: "Pag-IBIG "
                    };

                    let roleMessage2 = {
                        3: "A Pag-IBIG Officer will review your application shortly",
                        4: "Kindly proceed to submit required documents"
                    };

                    let txtStatus = `Your application has been <span class="fw-bolder text-success">
                    recently verified by ${roleMessage[data.ApprovalStatus]}${data.ApprovalStatus !== 3 ? " Officer" : ""}</span>. `;

                    if (data.ApprovalStatus === 3) {
                        txtStatus += `Kindly <span class="text-warning">wait for a ${roleMessage[4]} Officer to verify</span> your application.`;
                    } else if (data.ApprovalStatus === 4) {
                        txtStatus += `Kindly proceed to<span class="text-warning"> submit required documents.</span>`;
                    }

                    let roleMessage3 = {
                        3: `Kindly <span class="text-warning">wait for a Pag-IBIG officer to verify</span> your application.`,
                        4: `You may now submit required documents for your application.`
                    };

                    //$(`[id="credit_history_label"]`).html(`Your credit history has`);
                    $(`[id="credit_history_status"]`).html(`${roleMessage[data.ApprovalStatus]} Verified`);
                    $(`[id="credit_history_remarks"]`).html(roleMessage2[data.ApprovalStatus]);

                    //$(`[id="process_status"]`).html(`recently verified by ${roleMessage[data.ApprovalStatus]}.`);
                    //$(`[id="process_name"]`).html(`${roleMessage2[data.ApprovalStatus]}.`);
                    $(`[id="text_status"]`).html(`
                        Your application has been <span class="fw-bolder text-success">recently verified by ${roleMessage[data.ApprovalStatus]}</span>.
                        ${roleMessage3[data.ApprovalStatus]}
                    `);
                }

                else if (data.ApprovalStatus === 6) {
                    // Application Submitted for Second Stage
                    $(`[id="credit_history_status"]`).addClass('text-success');
                    $(`[id="credit_history_remarks"]`).addClass('text-muted');

                    //$(`[id="process_status"]`).html(`recently submitted.`);
                    //$(`[id="process_name"]`).html(`Kindly wait for a developer to verify your application.`);

                    $(`[id="credit_history_status"]`).html("Recently Submitted");
                    $(`[id="credit_history_remarks"]`).html("Kindly wait for a developer to review your application");

                    $(`[id="text_status"]`).html(`
                        Your application is <span class="fw-bolder text-primary">recently submitted</span>.
                        Kindly <span class="text-warning">wait for a developer to assess</span> your second application.
                    `);
                }
                else if ([7, 8].includes(data.ApprovalStatus)) {
                    $(`[id="credit_history_status"]`).addClass('text-success');
                    $(`[id="credit_history_remarks"]`).addClass('text-muted');
                    $(`[id="credit_history_label"]`).html(`Your application is now`);

                    let roleMessage = {
                        7: "Developer",
                        8: "Pag-IBIG"
                    };

                    let roleMessage2 = {
                        7: "A Pag-IBIG Officer will review your second application shortly",
                        8: "Kindly wait for post-approval process"
                    }

                    let roleMessage3 = {
                        7: `Kindly <span class="text-warning">wait for a Pag-IBIG officer to assess</span> your application.`,
                        8: `Kindly wait for a <span class="text-success">post-approval process</span>.`
                    };

                    //$(`[id="credit_history_label"]`).html(`Your credit history has`);
                    $(`[id="credit_history_status"]`).html(`${roleMessage[data.ApprovalStatus]} Approved`);
                    $(`[id="credit_history_remarks"]`).html(roleMessage2[data.ApprovalStatus]);

                    $(`[id="text_status"]`).html(`
                        Your application has been <span class="fw-bolder text-success">recently approved by ${roleMessage[data.ApprovalStatus]}</span>.
                        ${roleMessage3[data.ApprovalStatus]}
                    `);
                } else if ([5, 10].includes(data.ApprovalStatus)) {
                    $(`[id="credit_history_status"]`).addClass('text-warning');
                    $(`[id="credit_history_remarks"]`).addClass('text-muted');
                    $(`[id="credit_history_label"]`).html(`Your application is now`);

                    $(`[id="credit_history_status"]`).html(`Withdrawn`);
                    $(`[id="credit_history_remarks"]`).html(`Submit a new application`);

                    $(`[id="text_status"]`).html(`Your application is <span class='fw-bolder text-warning'> withdrawn</span>. Kindly submit an new application.`);
                } else if (data.ApprovalStatus === 11) {
                    // Application Submitted for Second Stage
                    $(`[id="credit_history_status"]`).addClass('text-primary');
                    $(`[id="credit_history_remarks"]`).addClass('text-muted');

                    //$(`[id="process_status"]`).html(`recently submitted.`);
                    //$(`[id="process_name"]`).html(`Kindly wait for a developer to verify your application.`);

                    $(`[id="credit_history_status"]`).html("Marked For Resubmission");
                    $(`[id="credit_history_remarks"]`).html("Double-check your requirements again to proceed");

                    $(`[id="text_status"]`).html(`
                        Your application is <span class="fw-bolder text-primary">recently marked for resubmission</span>.
                        Kindly <span class="text-warning">double-check remarks and re-upload documents</span> to continue.
                    `);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                // Error loading first row
            },
            complete: function (e) {
                // remove dark overlay
                //$(`[id="main-overlay"]`).addClass('d-none');
                //$(`[id="application-tracker-overlay"]`).addClass('d-none');
            }
        });
    }

    function loadTimeline() {
        $.ajax({
            url: baseUrl + 'Applicants/GetTimelineStatus',
            method: "GET",
            beforeSend: function (e) {
            },
            success: function (data) {
                /*
                    // bg-primary
                    // Submitted (1), Post Submitted (6)

                    // bg-secondary
                    // Draft (0)

                    // bg-danger
                    // Withdrawn (2), Disqualified (9)

                    // bg-lightgreen
                    // Developer Verified (3), Developer Confirmed (7)

                    // bg-darkgreen
                    // Pag-IBIG Verified (4), Pag-IBIG Confirmed (8)

                    // bg-warning
                    // Withdrawn (5), Discontinued (10)

                    // bg-teal
                    // Discontinued (11)
                
                     
                */


                let timelineStyle = "default";
                //let timelineStyle = "append";
                let classColorList = ["text-primary", "text-warning", "text-danger"];
                let classIconList = [`far fa-circle`, `fas fa-check-circle`, `fas fa-times-circle`];

                if (timelineStyle === "default") {
                    $(`[id="simplebarWrapper1"]`).removeAttr('hidden');

                    let failFlag = false;
                    let completedFlag = false;
                    let timeline = $(`[id="simplebarWrapper1"]`);
                    let recentTimelineIndex = 0;

                    let timelineSelector = [
                        {
                            approvalStatusNumber: 1,
                            successFlag: true,
                            timelines: [
                                {
                                    approverRoleId: 4,
                                    timeline: 1
                                }
                            ]
                        },
                        {
                            approvalStatusNumber: 3,
                            successFlag: true,
                            timelines: [
                                {
                                    approverRoleId: 5,
                                    timeline: 2
                                }
                            ]
                        },
                        {
                            approvalStatusNumber: 4,
                            successFlag: true,
                            timelines: [
                                {
                                    approverRoleId: 3,
                                    timeline: 3
                                }
                            ]
                        },
                        {
                            approvalStatusNumber: 6,
                            successFlag: true,
                            timelines: [
                                {
                                    approverRoleId: 4,
                                    timeline: 4
                                }
                            ]
                        },
                        {
                            approvalStatusNumber: 7,
                            successFlag: true,
                            timelines: [
                                {
                                    approverRoleId: 5,
                                    timeline: 5
                                }
                            ]
                        },
                        {
                            approvalStatusNumber: 8,
                            successFlag: true,
                            timelines: [
                                {
                                    approverRoleId: 3,
                                    timeline: 6
                                }
                            ]
                        },
                        {
                            approvalStatusNumber: 2,
                            successFlag: false,
                            timelines: [
                                {
                                    approverRoleId: 5,
                                    timeline: 2
                                },
                                {
                                    approverRoleId: 3,
                                    timeline: 3
                                }
                            ],
                        },
                        {
                            approvalStatusNumber: 9,
                            successFlag: false,
                            timelines: [
                                {
                                    approverRoleId: 5,
                                    timeline: 5
                                },
                                {
                                    approverRoleId: 3,
                                    timeline: 6
                                }
                            ],
                        },
                        {
                            approvalStatusNumber: 5,
                            successFlag: false,
                            timelines: [
                                {
                                    approverRoleId: 4,
                                    timeline: null
                                }
                            ],
                        },
                        {
                            approvalStatusNumber: 10,
                            successFlag: false,
                            timelines: [
                                {
                                    approverRoleId: 4,
                                    timeline: null
                                }
                            ],
                        },
                        {
                            approvalStatusNumber: 11,
                            successFlag: false,
                            timelines: [
                                {
                                    approverRoleId: 5,
                                    timeline: null
                                },
                                {
                                    approverRoleId: 3,
                                    timeline: null
                                }
                            ],
                        },
                        //{
                        //    ApprovalStatusNumbers: [4],
                        //    Timeline: 7
                        //},
                        //{
                        //    ApprovalStatusNumbers: [4],
                        //    Timeline: 8
                        //},
                    ];

                    let applicationStatus = [
                        {
                            approvalStatusNumbers: [5, 10],
                            color: 'warning',
                            //iconStatus: 'minus-circle',
                            iconStatus: 'fe-minus-circle',
                        },
                        {
                            approvalStatusNumbers: [2, 9],
                            color: 'danger',
                            //iconStatus: 'times-circle',
                            iconStatus: 'fe-x-circle',
                        },
                        {
                            approvalStatusNumbers: [1, 3, 4, 6, 7, 8],
                            color: 'info',
                            //iconStatus: 'check-circle',
                            iconStatus: 'fe-check-circle',
                        },
                        {
                            approvalStatusNumbers: [11],
                            color: 'warning',
                            //iconStatus: 'exclamation-circle',
                            iconStatus: 'fe-alert-circle',
                        },
                    ];

                    let currentTimelineId;

                    for (var index in data) {
                        let selectedData = data[index];
                        let stageText = "";

                        if (selectedData.ApprovalStatusNumber === 0) {
                            continue;
                        }

                        //let color = classColors.find(a => a.approvalStatusNumbers.includes(selectedData.ApprovalStatusNumber));
                        let applicationData = applicationStatus.find(a => a.approvalStatusNumbers.includes(selectedData.ApprovalStatusNumber));
                        let timelineData = timelineSelector.find(a => a.approvalStatusNumber === selectedData.ApprovalStatusNumber);
                        let timelineIndex = timelineData.timelines.find(a => a.approverRoleId === selectedData.ApproverRoleId).timeline ?? recentTimelineIndex + 1;
                        
                        failFlag = !timelineData.successFlag;
                        recentTimelineIndex = timelineIndex;
                        currentTimelineId = `[id="timeline${timelineIndex}"]`;
                        let $selectedTimeline = timeline.find(currentTimelineId);

                        if ([5, 10].includes(timelineData.approvalStatusNumber)) {
                            // Withdrawn
                            let stage = $selectedTimeline.find('.timeline-item-info [id="timeline-item-text"]').html();
                            stageText = `Withdrawn on ${stage}`;
                        }
                        else {
                            // Not withdrawn
                            stageText = selectedData.ApplicationStatus;
                        }

                        if (!failFlag) {
                            $selectedTimeline.addClass(`timeline-item-completed`);
                        }

                        $selectedTimeline.find('.timeline-icon').addClass(`bg-${applicationData.color} text-${applicationData.color}`);

                        $selectedTimeline.find('.timeline-placeholder-icon')
                            .removeClass(`fas fa-circle fs-1 text-muted`)
                            //.addClass(`fas fa-${applicationData.iconStatus} text-${applicationData.color}`);
                            .attr('class', `${applicationData.iconStatus} fs-2 text-white ` + $selectedTimeline.find('.timeline-placeholder-icon').attr('class'));

                        $selectedTimeline.find('.timeline-item-info [id="timeline-item-text"]')
                            .removeClass(`text-muted`)
                            .html(`${stageText}`);

                        $selectedTimeline.find('.timeline-item-info .timeline-date')
                            .removeClass(`text-muted`)
                            .html(moment(selectedData.DateCreated).format('LL'))
                            .attr('hidden', false);

                        $selectedTimeline.find('.timeline-item-info, #timeline-item-text')
                            .addClass(`text-${applicationData.color}`);
                    }

                    if (!failFlag && !completedFlag) {
                        let $currentTimeline = timeline.find(`div[id="timeline${recentTimelineIndex + 1}"]`);
                        $currentTimeline.find(`.timeline-placeholder-icon, .timeline-date, #timeline-item-text`).removeClass(`text-muted`);
                        //$currentTimeline.find(``).removeClass(`text-muted`);
                        //$currentTimeline.find(`.timeline-placeholder-icon, .timeline-item-info`).addClass(`text-info`);
                        $currentTimeline.find(`.timeline-placeholder-icon, .timeline-icon, .timeline-item-info`).addClass(`text-info`);

                    }
                }
                else if (timelineStyle === "append") {
                    $(`[id="simplebarWrapper2"]`).removeAttr('hidden');
                    for (var index in data) {
                        loadApplicationTimeline(data[index]);
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                // Error loading first row
            },
            complete: function (e) {
                // remove dark overlay
            }
        });
    }
});

function loadApplicationTimeline(itemObj = {}) {
    let count = $('[id="simplebarWrapper2"] .timeline-item').length;

    let applicationStatus = [
        {
            approvalStatusNumbers: [5, 10],
            color: 'warning',
            iconStatus: 'minus-circle',
        },
        {
            approvalStatusNumbers: [2, 9],
            color: 'danger',
            iconStatus: 'times-circle',
        },
        {
            approvalStatusNumbers: [1, 3, 4, 6, 7, 8, 11],
            color: 'info',
            iconStatus: 'check-circle',
        },
    ];

    let status = applicationStatus.find(a => a.approvalStatusNumbers.includes(itemObj.ApprovalStatusNumber));

    let formattedDate = new Date(itemObj.DateCreated).toLocaleString();

    let itemToAdd = `<div class="timeline-item" id="timeline_${count}">
                        <div>
                            <i class="fa fa-${status.iconStatus} text-${status.color} timeline-icon"></i>
                        </div>
                        <div class="timeline-item-info">
                            <a href="javascript:void(0);" class="text-${status.color} fw-bold mb-0 d-block">${itemObj.ApplicationStatus}</a>
                            <p class="mb-0">${itemObj.Stage}</p>
                            <p>
                                <small class="text-muted timeline-date">${formattedDate}</small>
                            </p>
                        </div>
                    </div>`;

    $('[id="simplebarWrapper2"] .timeline-alt').append(itemToAdd);
}

function loadApplicationTimeline2(itemObj = {}) {
    let count = $('[id="simplebarWrapper1"] .timeline-item').length;

    let applicationStatus = [
        {
            // Withdrawn (Stage 1, Stage 2)
            approvalStatusNumbers: [5, 10],
            color: 'warning',
            iconStatus: 'minus-circle',
        },
        {
            // Defer (Stage 1, Stage 2)
            approvalStatusNumbers: [2, 9],
            color: 'danger',
            iconStatus: 'times-circle',
        },
        {
            // Approved (Stage 1, Stage 2)
            approvalStatusNumbers: [1, 3, 4, 6, 7, 8],
            color: 'info',
            iconStatus: 'check-circle',
        },
    ];

    let time = applicationStatus.find(a => a.approvalStatusNumbers.includes(itemObj.ApprovalStatusNumber));


    let status = applicationStatus.find(a => a.approvalStatusNumbers.includes(itemObj.ApprovalStatusNumber));

    let formattedDate = new Date(itemObj.DateCreated).toLocaleString();

    let itemToAdd = `<div class="timeline-item" id="timeline_${count}">
                        <div>
                            <i class="fa fa-${status.iconStatus} text-${status.color} timeline-icon"></i>
                        </div>
                        <div class="timeline-item-info">
                            <a href="javascript:void(0);" class="text-${status.color} fw-bold mb-0 d-block">${itemObj.ApplicationStatus}</a>
                            <p class="mb-0">${itemObj.Stage}</p>
                            <p>
                                <small class="text-muted timeline-date">${formattedDate}</small>
                            </p>
                        </div>
                    </div>`;

    $('[id="simplebarWrapper2"] .timeline-alt').append(itemToAdd);
}

