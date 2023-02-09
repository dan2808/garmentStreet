var dataTable

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("pending")) {
            loadDataTable("pending");
        }
        else {
            if (url.includes("completed")) {
                loadDataTable("completed");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
            }
        }
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/Admin/Order/GetAll?status=" + status
            },
            "columns": [
                { "data": "id", "width": "5%" },
                { "data": "name", "width": "20%" },
                { "data": "phoneNumber", "width": "20%" },
                { "data": "applicationUser.email", "width": "20%" },
                { "data": "orderStatus", "width": "20%" },
                { "data": "orderTotal", "width": "10%" },


                {
                    "data": "id",
                    "render": function (data) {
                        return `
                        
                            <div class="row">
                                <div class="col-6 d-flex justify-content-center">
                                    <a href="/Admin/Order/Details?orderId=${data}" class="btn"><i class="bi bi-pencil"></i></a>
                                </div>
                            </div>
                        
                        `
                    },
                    "width": "5%"
                }
            ]
        });
}

