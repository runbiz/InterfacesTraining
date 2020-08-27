$(function () {
    $("#panel").kendoPanelBar();
    $("#Dev_DateOfBirth").kendoDatePicker();
    $("#Dev_DepartmentId").kendoDropDownList({
        dataSource: {
            transport: {
                read: {
                    url: '/Developer/jQuery/Index?handler=Departments',
                    type: 'GET',
                    dataType: 'json'
                }
            }
        },
        dataTextField: "Name",
        dataValueField: "Id"
    });
    var grid = $('#grid').kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: "/Developer/jQuery/Index?handler=Developers",
                    type: 'GET',
                    dataType: 'json'
                },
                update: {
                    url: "/Developer/jQuery/Index",
                    type: "PUT",                    
                    headers: {
                        RequestVerificationToken: forgeryToken()
                    }
                },
                destroy: {
                    url: "/Developer/jQuery/Index",
                    type: 'DELETE',
                    headers: {
                        RequestVerificationToken: forgeryToken()
                    }
                },
                parameterMap: function (data, type) {
                    if (type === "update") {
                        data.DateOfBirth = data.DateOfBirth.toISOString();
                    }
                    return data;
                }
            },
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        DateOfBirth: { type: "date" },
                        Department: { type: "object" }
                    }
                }
            },
            requestEnd: function (e) {
                if (e.type === "update" || e.type === "destroy") {
                    location.reload();
                }
            },
            pageSize: 5
        },
        columns: [
            { field: "Name", title: "Name" },
            { field: "DateOfBirth", title: "DOB", format: "{0:d}" },
            { field: "Address", title: "Address" },
            {
                field: "Department.Id", title: "Department", dataTextField: "Name", dataValueField: "Id",
                dataSource: {
                    transport: {
                        read: {
                            url: "/Developer/jQuery/Index?handler=Departments",
                            type: 'GET',
                            dataType: 'json'
                        }
                    }
                }
            },
            { command: ["edit", "destroy"] }
        ],
        editable: {
            mode: "popup"
        },
        pageable: true,
        filterable: true,
        sortable: true,
        scrollable: false
    }).data("kendoGrid");
});