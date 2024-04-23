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
                }

                let color = classColors.find(a => a.approvalStatusNumbers.includes(data.ApprovalStatus)).classColor;
                //console.log(color);

                // Greeting
                // $(`[id="user_first_name"]`).html(data.ApplicantFirstName);
                //

                $(`[id="status_contract"]`).addClass(color);
                $(`[id="status_contract"]`).html(data.ApplicationStatus);

                $(`[id="reference_number"]`).html(data.Code || "-----");
                $(`[id="requested_loan_amount"]`).html(data.LoanAmount !== 0 ? numeral(data.LoanAmount).format('0, 0.00') : "------"); // use numeral
                $(`[id="loan_term"]`).html(data.LoanYears !== 0 ? data.LoanYears : "--"); // use numeral
                $(`[id="project_location"]`).html(data.ProjectLocation || "-----");

                let appStatus = data.Stage || "Unknown";
                let appStatusColor;
                let appStatusRemarks;

                console.log(appStatus);

                $(`[id="beneficiary_sidecard"]`).removeClass('bg-warning');
                $(`[id="application_status"]`).removeClass('text-warning');

                // set color
                if ([0, 1, 2, 3, 5, 11].includes(data.ApprovalStatus)) {
                    // `Credit Verification`
                    $(`[id="beneficiary_sidecard"]`).addClass('bg-warning');
                    $(`[id="application_status"]`).addClass('text-warning');

                    appStatusRemarks = `Stage ends after Pag-IBIG credit verification`;
                }
                else if ([4, 6, 7, 9, 10].includes(data.ApprovalStatus)) {
                    // `Application Completion`
                    $(`[id="beneficiary_sidecard"]`).addClass('bg-primary');
                    $(`[id="application_status"]`).addClass('text-primary');

                    appStatusRemarks = `Stage ends after Pag-IBIG approval`;
                }
                else if (data.ApprovalStatus === 8) {
                    // `Post-Approval`
                    $(`[id="beneficiary_sidecard"]`).addClass('bg-success');
                    $(`[id="application_status"]`).addClass('text-success');

                    appStatusRemarks = `Stage ends after Pag-IBIG credit verification`;
                }


                $(`[id="beneficiary_sidecard"] [id="beneficiary_sidecard_text"]`).html(appStatus);
                $(`[id="application_status"]`).html(appStatus);
                $(`[id="application_status_remarks"]`).html(appStatusRemarks);
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