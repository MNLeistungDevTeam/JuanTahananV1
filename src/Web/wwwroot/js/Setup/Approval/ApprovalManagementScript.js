$(function () {

    let moduleModal = $("#module-modal");

    //#region DataTables

    var tbl_moduleHasApproval = $("#tbl_moduleHasApproval").DataTable({
        ajax: {
            url: baseUrl + "Module/GetWithApprovers",
            data: function (d) {
            },
            dataSrc: ""
        },
        language: {
            processing: "<div class='text-center'><span class='spinner-border spinner-border-sm'></span> Loading...</div>",
            previous: "<i class='mdi mdi-chevron-left'>",
            next: "<i class='mdi mdi-chevron-right'>"
        },
        columns: [

            {
                data: "Description",
                class: "text-center"
            },
            {
                data: "ModuleType",
                class: "text-center"
            },

            {
                data: "ParentModuleName",
                class: "text-center"
            },

            {
                data: "ModuleStageCount",
                class: "text-center",
                render: function (data, type, row) {
                    return data != undefined ? data : 0;
                }
            },

        ],
        drawCallback: function () {
            $(".dataTables_paginate > .pagination").addClass("pagination-rounded")
        },
        columnDefs: [

        ],
        rowId: "Id",
        order: [0, 'desc'],
        processing: true,
        scrollX: true,
        scrollY: '50vh',
        scrollCollapse: true,
        paging: true,
        select: true,
        searchHighlight: true
    });

    $("#tbl_moduleHasApproval_filter, #tbl_moduleHasApproval_length").hide();

    tbl_moduleHasApproval.on("deselect select", function () {
        var all = tbl_moduleHasApproval.rows({ search: 'applied' }).count();
        var selectedRows = tbl_moduleHasApproval.rows({ selected: true, search: 'applied' }).count();
        var moduleId = tbl_moduleHasApproval.rows({ selected: true }).data().pluck("Id").toArray().toString();
        var desc = tbl_moduleHasApproval.rows({ selected: true }).data().pluck("TransactionNo").toArray().toString();
        var approvalLevelId = tbl_moduleHasApproval.rows({ selected: true }).data().pluck("ApprovalLevelId").toArray().toString();
        var module = tbl_moduleHasApproval.rows({ selected: true }).data().pluck("ModuleDescription").toArray().toString();

        //$("#btnApprove, #btnDisapprove").attr({
        //    "disabled": !(selectedRows === 1),
        //    "data-statusId": approvalStatusId,
        //    "data-desc": desc,
        //    "data-id": !approvalLevelId ? 0 : approvalLevelId
        //});

        //$("#btnReturn").attr({
        //    "disabled": !(selectedRows === 1 && module != "Canvass Sheet"),
        //    "data-statusId": approvalStatusId,
        //    "data-desc": desc,
        //    "data-id": !approvalLevelId ? 0 : approvalLevelId
        //});

        $("#btn_edit").attr({
            "disabled": !(selectedRows === 1),
            "data-moduleid": moduleId
        });
    });

    //#endregion

    $("#btn_edit").on('click', function () {
        var moduleId = $(this).attr('data-moduleid');
        openModuleModal(moduleId);
    });

    async function openModuleModal(id = 0, mode = "") {
        let moduleModalLabel = $("#module-modalLabel");
        let modalTitle = "";

        $("#frm_module").clearValidation();

        //resetModule();

        if (id != 0) {
            if (mode == "copy") {
                $("[name='Module.Id']").val(0);
                modalTitle = "<span class='fe-plus'></span> Add Module Stage Approver";
            } else {
                $("[name='Module.Id']").val(id);
                modalTitle = "<span class='fe-edit'></span> Edit Module Stage Approver";
            }

            var moduleInfo = await getModuleInfo(id);
            var moduleStages = await getModuleStages(id);

            $("[name='Module.Code']").val(moduleInfo.Code);
            $("[name='Module.Description']").val(moduleInfo.Description);

            $("[name='Module.Controller']").val(moduleInfo.Controller);
            $("[name='Module.Action']").val(moduleInfo.Action);
            $("[name='Module.Icon']").val(moduleInfo.Icon).trigger("change");

            $("[name='Module.Ordinal']").val(moduleInfo.Ordinal);
            $("[name='Module.IsDisabled']").prop("checked", moduleInfo.IsEnabled);
            $("[name='Module.IsVisible']").prop("checked", moduleInfo.IsVisible);
            $("[name='Module.InMaintenance']").prop("checked", moduleInfo.InMaintenance);
            $("[name='Module.WithApprover']").prop("checked", moduleInfo.WithApprover);

            $("#tbl_module_stages tbody").empty();
            for (var moduleStage of moduleStages) {
                addModuleStage(moduleStage);
            }
            rebindValidators();
        }

        moduleModalLabel.html(modalTitle);

        moduleModal.modal("show");
    }

    async function getModuleInfo(moduleId) {
        const response = await $.ajax({
            url: baseUrl + "Module/GetModule/" + moduleId,
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    async function getModuleTypeInfo(moduleTypeId) {
        const response = await $.ajax({
            url: baseUrl + "Module/GetModuleType/" + moduleTypeId,
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    async function getModuleStages(moduleId) {
        const response = await $.ajax({
            url: baseUrl + "Module/GetModuleStages/" + moduleId,
            method: "get",
            dataType: 'json'
        });

        return response;
    }

    $("#btn_add_module_stage").on("click", function () {
        addModuleStage();
        fixElementSequence(".stage_row");
        rebindValidators();
    });

    $(document).on('click', '.btn_remove_stage', function (e) {
        e.preventDefault();
        $(this).closest('.stage_row').remove();
        fixElementSequence(".stage_row");
        rebindValidators();
    });

    function rebindValidators() {
        let $form = $("#frm_module");
        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate($form.data("unobtrusiveValidation").options);
        $form.data("validator").settings.ignore = "";

        $form.on("submit", function (e) {
            e.preventDefault();
            let formData = $(this).serialize();
            let button = $("#btn_save_modulestage");

            if ($(this).valid() == false) {
                messageBox("Please fill up all required fields!", "danger");
                return;
            }

            //if (customValidateForm() == false) return;

            $.ajax({
                url: $(this).attr("action"),
                method: $(this).attr("method"),
                data: formData,
                beforeSend: function () {
                    button.html("<span class='spinner-border spinner-border-sm'></span> Saving...");
                    button.attr({ disabled: true });
                },
                success: function (response) {
                    let successMessage = $("input[name='Module.Id']").val() == 0 ? "Stage Approver Successfully Added!" : "Stage Approver Successfully Updated!";
                    messageBox(successMessage, "success", true);
                    tbl_moduleHasApproval.ajax.reload(null, false)
                    button.attr({ disabled: false });
                    button.html("<span class='mdi mdi-content-save-outline'></span> Save");
                    moduleModal.modal('hide');
                },
                error: function (response) {
                    messageBox(response.responseText, "danger", true);
                    button.html("<span class='mdi mdi-content-save-outline'></span> Save");
                    button.attr({ disabled: false });
                }
            });
        });
    }

    function addModuleStage(stagesObj = {}) {
        let count = $(".stage_row").length;

        let rowToAdd = `<tr class="stage_row">
                            <td hidden="">

                                <input id="ModuleStages_ModuleStageApproverId_[${count}]" type="number" data-val="true" data-val-required="The ModuleStageApproverId field is required." name="ModuleStages[${count}].ModuleStageApproverId" value="${stagesObj.ModuleStageApproverId || "0"}">
                                <input id="ModuleStages_Id_[${count}]" type="number" data-val="true" data-val-required="The Id field is required." name="ModuleStages[${count}].Id" value="${stagesObj.Id || "0"}">
                                <input id="ModuleStages_Level_[${count}]" type="number" data-val="true" data-val-required="The Level field is required." name="ModuleStages[${count}].Level" value="${stagesObj.Level || "0"}">
                                <input id="ModuleStages_ModuleId_[${count}]" type="number" data-val="true" data-val-required="The ModuleId field is required." name="ModuleStages[${count}].ModuleId" value="${stagesObj.ModuleId || "0"}">
                                <input id="ModuleStages_Code_[${count}" class="form-control-plaintext form-control-sm" type="text" name="ModuleStages[${count}].Code" value="${stagesObj.Code || ""}">
                            </td>
                            <td class="text-center align-middle">
                                <span class="d-flex justify-content-between ui-widget-header ui-sortable-handle" style="cursor: move;display: inline-block;">
                                    <i class="fas fa-ellipsis-v"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </span>
                            </td>
                            <td class="text-center">
                                <button class="btn btn-soft-danger btn-sm btn_remove_stage" type="button">
                                    <span class="fa fa-times"></span>
                                </button>
                            </td>
                            <td>
                                <input class="form-control form-control-sm" placeholder="Stage Name" id="ModuleStages_Name_[${count}]" type="text" data-val="true" data-val-required="Enter Stage Name" name="ModuleStages[${count}].Name" value="${stagesObj.Name || ""}" aria-describedby="ModuleStages_StageName_[${count}]-error" aria-invalid="false">
                                <span class="field-validation-valid text-danger" data-valmsg-for="ModuleStages[${count}].Name" data-valmsg-replace="true"></span>
                            </td>
                            <td>
                                <input class="form-control form-control-sm" placeholder="Stage Title" id="ModuleStages_Title_[${count}]" type="text" data-val="true" data-val-required="Enter Stage Name" name="ModuleStages[${count}].Title" value="${stagesObj.Title || ""}" aria-describedby="ModuleStages_Title_[${count}]-error" aria-invalid="false">
                                <span class="field-validation-valid text-danger" data-valmsg-for="ModuleStages[${count}].Title" data-valmsg-replace="true"></span>
                            </td>
                            <td>
                                <input class="form-control form-control-sm numberInputMask" id="ModuleStages_ReturnStage_[${count}]" type="text" data-val="true" data-val-required="Enter Return Stage" name="ModuleStages[${count}].ReturnStage" value="${stagesObj.ReturnStage || "0"}" >
                                <span class="field-validation-valid text-danger" data-valmsg-for="ModuleStages[${count}].ReturnStage" data-valmsg-replace="true"></span>
                            </td>
                            <td>
                                <input class="form-control form-control-sm" placeholder="Approve Description" id="ModuleStages_ApproveDesc_[${count}]" type="text" data-val="true" data-val-required="Enter Approved Description" name="ModuleStages[${count}].ApproveDesc" value="${stagesObj.ApproveDesc || ""}">
                                <span class="field-validation-valid text-danger" data-valmsg-for="ModuleStages[${count}].ApproveDesc" data-valmsg-replace="true"></span>
                            </td>
                            <td >
                                <input class="form-control form-control-sm" placeholder="Reject Description" id="ModuleStages_RejectDesc_[${count}]" type="text" data-val="true" data-val-required="Enter Rejected Description" name="ModuleStages[${count}].RejectDesc" value="${stagesObj.RejectDesc || ""}">
                                <span class="field-validation-valid text-danger" data-valmsg-for="ModuleStages[${count}].RejectDesc" data-valmsg-replace="true"></span>
                            </td>
                            <td>
                                <input class="form-control form-control-sm numberInputMask" id="ModuleStages_RequiredCount_[${count}]" type="text" data-val="true" data-val-required="The Required Approver Count field is required." name="ModuleStages[${count}].RequiredCount" value="${stagesObj.RequiredCount || "1"}">
                                <span class="field-validation-valid text-danger" data-valmsg-for="ModuleStages[${count}].RequiredCount" data-valmsg-replace="true"></span>
                            </td>
                            <td hidden>
                                <input class="form-control form-control-sm decimalInputMask" id="ModuleStages_RequiredAmount_[${count}]" type="text" data-val="true" data-val-required="The Required Amount field is required." name="ModuleStages[${count}].RequiredAmount" value="${stagesObj.RequiredAmount || "0.00"}" >
                                <span class="field-validation-valid text-danger" data-valmsg-for="ModuleStages[${count}].RequiredAmount" data-valmsg-replace="true"></span>
                            </td>
                             <td>
                            <div class="form-group form-group-sm">
                                <select class="form-control form-control-sm" id="ModuleStages_ApproverType_[${count}]" type="text" data-val="true" data-val-required="The ApproverType is required" name="ModuleStages[${count}].ApproverType">
                                    <option value="">Select Approver...</option>
                                    <option value="1">Role</option>
                                </select>
                            </div>
                            <span class="field-validation-valid text-danger" data-valmsg-for="ModuleStages[${count}].ApproverType" data-valmsg-replace="true"></span>
                        </td>

                              <td>
                                <div class="form-group form-group-sm">
                                    <select class="form-control form-control-sm" id="ModuleStages_ApproverId_[${count}]" type="text" data-val="true" data-val-required="The Required Approver." name="ModuleStages[${count}].ApproverId">
                                        <option value="">Select Approver...</option>
                                    </select>
                                </div>
                                <span class="field-validation-valid text-danger" data-valmsg-for="ModuleStages[${count}].ApproverId" data-valmsg-replace="true"></span>
                            </td>

                        </tr>`;

        $("#tbl_module_stages tbody").append(rowToAdd);

        $(`[id="ModuleStages_ReturnStage_[${count}]"],[id="ModuleStages_RequiredCount_[${count}]"]`).inputmask({
            alias: 'decimal',
            rightAlign: true,
            digits: 0,
            allowMinus: false,
            autoGroup: true,
            placeholder: "0"
        });

        initializeDecimalInputMask();

        // Initialize selectize dropdowns

        var $approverTypeDropdown = $(`[id="ModuleStages_ApproverType_[${count}]"]`).selectize();
        var approverTypeDropdown = $approverTypeDropdown[0].selectize;

        // Set the value based on conditions
        var approverTypeValue = stagesObj.RoleId !== null || stagesObj.ApproverId === 0 ? 1 : 2;
        approverTypeDropdown.setValue(approverTypeValue);

        $(`[id="ModuleStages_ApproverType_[${count}]"]`).on('change', function () {
            var selectedApproverType = $(this).val();
            console.log("Selected Approver Type:", selectedApproverType);

            // Destroy current selectize instance
            approverIdDropdown.destroy();

            // Reinitialize selectize with appropriate data
            $approverIdDropdown = $(`[id="ModuleStages_ApproverId_[${count}]"]`).selectize({
                valueField: 'Id',
                labelField: 'UserName',
                searchField: ['UserName', 'Name'],
                preload: true,
                load: function (query, callback) {
                    var url = selectedApproverType == 1 ? baseUrl + "Role/GetAllRoles" : baseUrl + "User/GetUsers";
                    $.ajax({
                        url: url,
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
                        return ("<div class='text-truncate' style='max-width:90%;'>" +
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
                onInitialize: function () {
                    // Set the selected value if available
                    if (stagesObj.ApproverId) {
                        this.setValue(stagesObj.ApproverId);
                    }
                }
            });

            approverIdDropdown = $approverIdDropdown[0].selectize;
        });

        var $approverIdDropdown = $(`[id="ModuleStages_ApproverId_[${count}]"]`).selectize({
            valueField: 'Id',
            labelField: 'UserName',
            searchField: ['UserName', 'Name'],
            preload: true,
            load: function (query, callback) {
                var selectedApproverType = $(`[id="ModuleStages_ApproverType_[${count}]"]`).val();
                var url = selectedApproverType == 1 ? baseUrl + "Role/GetAllRoles" : baseUrl + "User/GetUsers";
                $.ajax({
                    url: url,
                    success: function (results) {
                        try {
                            // Callback with the loaded results
                            callback(results);

                            // Set the selected value if available
                            var valueToSet = stagesObj.RoleId === 0 ? stagesObj.ApproverId : stagesObj.RoleId;
                            approverIdDropdown.setValue(valueToSet);
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
                    return ("<div class='text-truncate' style='max-width:90%;'>" +
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
            }
        });

        var approverIdDropdown = $approverIdDropdown[0].selectize;
    }

    $("#btnRefresh").on('click', function () {
        tbl_moduleHasApproval.ajax.reload();
    });
});