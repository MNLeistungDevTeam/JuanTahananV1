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

                let color = backgroundClassColors.find(a => a.approvalStatusNumbers.includes(data.ApprovalStatus));
                let selectedColor = color ? color.classColor : "bg-primary"
                //console.log(color);

                // Status below the greeting

                $(`[id="status_contract"]`).addClass(selectedColor);
                $(`[id="status_contract"]`).html(data.ApplicationStatus || "No Application");

                $(`[id="reference_number"]`).html(data.Code || "-----");
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
                        2: "Deferred by Developer",
                        3: "Deferred by Pag-IBIG",
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
                        4: "Pag-IBIG"
                    };

                    let roleMessage2 = {
                        3: "A Pag-IBIG Officer will review your application shortly",
                        4: "Kindly proceed to submit required documents"
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
                /*
                
                     1: `[id="timeline1"]`, // submitted
                     2: `[id="timeline1"]`, // withdrawn
                     3: `[id="timeline2"]`, // developer verified
                     4: `[id="timeline3"]`, // pag-IBIG verified
                     5: `[id="timeline4"]`, // submitted (2nd stage)
                     6: `[id="timeline4"]`, // submitted (2nd stage)
                     7: `[id="timeline5"]`, // developer approval
                     8: `[id="timeline6"]`, // 
                     7: `[id="timeline7"]`,
                     8: `[id="timeline8"]`,
                */

                let classColorList = ["text-info", "text-warning", "text-danger"];


                for (var index in data) {
                    let selectedData = data[index];

                    console.log(selectedData);
                    //console.log(selectedData);
                    //let color = classColors.find(a => a.approvalStatusNumbers.includes(selectedData.ApprovalStatusNumber));

                    if (selectedData.ApprovalStatusNumber === 1) {
                        // Submitted, Stage 1
                        $(`[id="timeline1"] .timeline-icon`).removeClass(`far fa-circle`);
                        $(`[id="timeline1"] .timeline-icon`).removeClass(classColorList);
                        $(`[id="timeline1"] .timeline-icon`).addClass(`fas fa-check-circle text-info`);

                        $(`[id="timeline1"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus} (First Stage)`);

                        $(`[id="timeline1"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                        $(`[id="timeline1"] .timeline-item-info .timeline-date`).attr('hidden', false);
                    }
                    else if (selectedData.ApprovalStatusNumber === 3) {
                        // Developer Verified, Stage 1
                        $(`[id="timeline2"] .timeline-icon`).removeClass(`far fa-circle`);
                        $(`[id="timeline2"] .timeline-icon`).removeClass(classColorList);
                        $(`[id="timeline2"] .timeline-icon`).addClass(`fas fa-check-circle text-info`);

                        $(`[id="timeline2"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus}`);

                        $(`[id="timeline2"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                        $(`[id="timeline2"] .timeline-item-info .timeline-date`).attr('hidden', false);
                    }
                    else if (selectedData.ApprovalStatusNumber === 4) {
                        // Pag-IBIG Verified, Stage 1
                        $(`[id="timeline3"] .timeline-icon`).removeClass(`far fa-circle`);
                        $(`[id="timeline3"] .timeline-icon`).removeClass(classColorList);
                        $(`[id="timeline3"] .timeline-icon`).addClass(`fas fa-check-circle text-info`);

                        $(`[id="timeline3"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus}`);

                        $(`[id="timeline3"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                        $(`[id="timeline3"] .timeline-item-info .timeline-date`).attr('hidden', false);
                    }
                    else if (selectedData.ApprovalStatusNumber === 6) {
                        // Submitted, Stage 2
                        $(`[id="timeline4"] .timeline-icon`).removeClass(`far fa-circle`);
                        $(`[id="timeline4"] .timeline-icon`).removeClass(classColorList);
                        $(`[id="timeline4"] .timeline-icon`).addClass(`fas fa-check-circle text-info`);

                        $(`[id="timeline4"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus} (Second Stage)`);

                        $(`[id="timeline4"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                        $(`[id="timeline4"] .timeline-item-info .timeline-date`).attr('hidden', false);
                    }
                    else if (selectedData.ApprovalStatusNumber === 7) {
                        // Developer Approved, Stage 2
                        $(`[id="timeline5"] .timeline-icon`).removeClass(`far fa-circle`);
                        $(`[id="timeline5"] .timeline-icon`).removeClass(classColorList);
                        $(`[id="timeline5"] .timeline-icon`).addClass(`fas fa-check-circle text-info`);

                        $(`[id="timeline5"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus}`);

                        $(`[id="timeline5"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                        $(`[id="timeline5"] .timeline-item-info .timeline-date`).attr('hidden', false);
                    }
                    else if (selectedData.ApprovalStatusNumber === 8) {
                        // Pag-IBIG Approved, Stage 2
                        $(`[id="timeline6"] .timeline-icon`).removeClass(`far fa-circle`);
                        $(`[id="timeline6"] .timeline-icon`).removeClass(classColorList);
                        $(`[id="timeline6"] .timeline-icon`).addClass(`fas fa-check-circle text-info`);

                        $(`[id="timeline6"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus}`);

                        $(`[id="timeline6"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                        $(`[id="timeline6"] .timeline-item-info .timeline-date`).attr('hidden', false);
                    }

                    // Deferred by Pag-IBIG or Developer, Stage 1
                    if (selectedData.ApprovalStatusNumber === 2 && [3, 5].includes(selectedData.ApproverRoleId)) {
                        if (selectedData.ApproverRoleId === 5) {
                            // Deferred by Developer
                            $(`[id="timeline2"] .timeline-icon`).removeClass(`far fa-circle`);
                            $(`[id="timeline2"] .timeline-icon`).removeClass(classColorList);
                            $(`[id="timeline2"] .timeline-icon`).addClass(`fas fa-times-circle text-danger`);

                            $(`[id="timeline2"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus}`);

                            $(`[id="timeline2"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                            $(`[id="timeline2"] .timeline-item-info .timeline-date`).attr('hidden', false);
                        }
                        if (selectedData.ApproverRoleId === 3) {
                            // Deferred by Pag-IBIG
                            $(`[id="timeline3"] .timeline-icon`).removeClass(`far fa-circle text-info`);
                            $(`[id="timeline3"] .timeline-icon`).removeClass(classColorList);
                            $(`[id="timeline3"] .timeline-icon`).addClass(`fas fa-times-circle text-danger`);

                            $(`[id="timeline3"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus}`);

                            $(`[id="timeline3"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                            $(`[id="timeline3"] .timeline-item-info .timeline-date`).attr('hidden', false);
                        }
                    }

                    // Deferred by Pag-IBIG or Developer, Stage 2
                    if (selectedData.ApprovalStatusNumber === 9 && [3, 5].includes(selectedData.ApproverRoleId)) {
                        if (selectedData.ApproverRoleId === 5) {
                            // Deferred by Developer
                            $(`[id="timeline5"] .timeline-icon`).removeClass(`far fa-circle`);
                            $(`[id="timeline5"] .timeline-icon`).removeClass(classColorList);
                            $(`[id="timeline5"] .timeline-icon`).addClass(`fas fa-times-circle text-danger`);

                            $(`[id="timeline5"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus}`);

                            $(`[id="timeline5"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                            $(`[id="timeline5"] .timeline-item-info .timeline-date`).attr('hidden', false);
                        }
                        if (selectedData.ApproverRoleId === 3) {
                            // Deferred by Pag-IBIG
                            $(`[id="timeline6"] .timeline-icon`).removeClass(`far fa-circle text-info`);
                            $(`[id="timeline6"] .timeline-icon`).removeClass(classColorList);
                            $(`[id="timeline6"] .timeline-icon`).addClass(`fas fa-times-circle text-danger`);

                            $(`[id="timeline6"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus}`);

                            $(`[id="timeline6"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                            $(`[id="timeline6"] .timeline-item-info .timeline-date`).attr('hidden', false);
                        }
                    }

                    // Withdrawn by Beneficiary, Stage 1
                    if (selectedData.ApprovalStatusNumber === 5) {
                        console.log('aaaa');
                        if (data[index - 1].ApprovalStatusNumber === 0) {
                            $(`[id="timeline1"] .timeline-icon`).removeClass(`far fa-circle`);
                            $(`[id="timeline1"] .timeline-icon`).removeClass(classColorList);
                            $(`[id="timeline1"] .timeline-icon`).addClass(`fas fa-check-circle text-warning`);

                            $(`[id="timeline1"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus} (First Stage)`);

                            $(`[id="timeline1"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                            $(`[id="timeline1"] .timeline-item-info .timeline-date`).attr('hidden', false);
                        }
                        else if (data[index - 1].ApprovalStatusNumber === 1) {

                            console.log('aaaa');
                            $(`[id="timeline2"] .timeline-icon`).removeClass(`far fa-circle`);
                            $(`[id="timeline2"] .timeline-icon`).removeClass(classColorList);
                            $(`[id="timeline2"] .timeline-icon`).addClass(`fas fa-check-circle text-warning`);

                            $(`[id="timeline2"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus} (First Stage)`);

                            $(`[id="timeline2"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                            $(`[id="timeline2"] .timeline-item-info .timeline-date`).attr('hidden', false);
                        }
                        else if (data[index - 1].ApprovalStatusNumber === 3) {
                            $(`[id="timeline3"] .timeline-icon`).removeClass(`far fa-circle`);
                            $(`[id="timeline3"] .timeline-icon`).removeClass(classColorList);
                            $(`[id="timeline3"] .timeline-icon`).addClass(`fas fa-check-circle text-warning`);

                            $(`[id="timeline3"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus} (First Stage)`);

                            $(`[id="timeline3"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                            $(`[id="timeline3"] .timeline-item-info .timeline-date`).attr('hidden', false);
                        }
                    }

                    // Withdrawn by Beneficiary, Stage 2
                    if (selectedData.ApprovalStatusNumber === 10) {
                        // Submitted, Stage 2
                        $(`[id="timeline4"] .timeline-icon`).removeClass(`far fa-circle`);
                        $(`[id="timeline4"] .timeline-icon`).removeClass(classColorList);
                        $(`[id="timeline4"] .timeline-icon`).addClass(`fas fa-check-circle text-warning`);

                        $(`[id="timeline4"] .timeline-item-info [id="timeline-item-text"]`).html(`${selectedData.ApplicationStatus} (Second Stage)`);

                        $(`[id="timeline4"] .timeline-item-info .timeline-date`).html(moment(selectedData.DateCreated).format('LL'));
                        $(`[id="timeline4"] .timeline-item-info .timeline-date`).attr('hidden', false);
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