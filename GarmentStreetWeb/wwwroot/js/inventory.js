var dataTable

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/Admin/Inventory/GetAll"
            },
            "columns": [
                { "data": "product.name", "width": "30%" },
                { "data": "variationOption.name", "width": "30%" },
                { "data": "quantity", "width": "30%" },

                {
                    "data": "id",
                    "render": function (data) {
                        return `
                        
                            <div class="row">
                                <div class="col-6 d-flex justify-content-center">
                                    <a href="/Admin/Inventory/Upsert?id=${data}" class="btn"><i class="bi bi-pencil"></i></a>
                                </div>
                                <div class="col-6 d-flex justify-content-center">
                                    <a onClick=Delete('/Admin/Inventory/Delete?id=${data}') class="btn"><i class="bi bi-trash"></i></a> 
                                </div>
                            </div>
                        
                        `
                    },
                    "width": "10%"
                }
            ]
        });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success = true) {
                        toastr.success(data.message)
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message)


                    }
                }
            })

        }
    })

}