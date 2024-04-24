$(() => {
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

    var classColors = [
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
                    //$(`[id="with_application"]`)
                    //return;
                }

                let color = classColors.find(a => a.approvalStatusNumbers.includes(data.ApprovalStatus));
                let selectedColor = color ? color.classColor : "bg-primary"
                //console.log(color);

                // Greeting
                // $(`[id="user_first_name"]`).html(data.ApplicantFirstName);
                //  

                $(`[id="status_contract"]`).addClass(selectedColor);
                $(`[id="status_contract"]`).html(data.ApplicationStatus || "No Application");

                $(`[id="reference_number"]`).html(data.Code || "-----");
                $(`[id="requested_loan_amount"]`).html(data.LoanAmount !== 0 ? numeral(data.LoanAmount).format('0, 0.00') : "------"); // use numeral
                $(`[id="loan_term"]`).html(data.LoanYears !== 0 ? data.LoanYears : "--"); // use numeral
                $(`[id="project_location"]`).html(data.ProjectLocation || "-----");

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

                // Credit History
                $(`[id="credit_history_status"]`).removeClass('text-success');
                $(`[id="credit_history_remarks"]`).removeClass('text-muted');

                if ([3, 5].includes(data.ApproverRoleId) && [2, 9].includes(data.ApprovalStatus)) {
                    $(`[id="credit_history_status"]`).addClass('text-danger');
                    $(`[id="credit_history_remarks"]`).addClass('text-danger');

                    let roleMessage = {
                        3: "Deferred by Pag-IBIG",
                        5: "Deferred by Developer"
                    };

                    $(`[id="credit_history_status"]`).html(roleMessage[data.ApproverRoleId]);
                    $(`[id="credit_history_remarks"]`).html("Review remarks and update your application");
                }
                else if (!data.ApproverRoleId && data.ApprovalStatus === 0) {
                    $(`[id="credit_history_status"]`).addClass('text-secondary');
                    $(`[id="credit_history_remarks"]`).addClass('text-secondary');


                    $(`[id="credit_history_status"]`).html("In Draft");
                    $(`[id="credit_history_remarks"]`).html("Kindly complete and submit requirements");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            },
            complete: function (e) {
                // remove dark overlay
                //$(`[id="main-overlay"]`).addClass('d-none');
                //$(`[id="application-tracker-overlay"]`).addClass('d-none');
            }
        });
    }
});