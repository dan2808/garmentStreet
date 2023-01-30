var dataTable

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/Admin/Product/GetAll"
            },
            "columns": [
                { "data": "name", "width": "20%" },
                { "data": "price", "width": "20%" },
                { "data": "category.name", "width": "20%" },
                { "data": "category.target.name", "width": "20%" },

                {
                    "data": "id",
                    "render": function (data) {
                        return `
                        
                            <div class="row">
                                <div class="col-6 d-flex justify-content-center">
                                    <a href="/Admin/Product/Upsert?id=${data}" class="btn"><i class="bi bi-pencil"></i></a>
                                </div>
                                <div class="col-6 d-flex justify-content-center">
                                    <a onClick=Delete('/Admin/Product/Delete?id=${data}') class="btn"><i class="bi bi-trash"></i></a> 
                                </div>
                            </div>
                        
                        `
                    },
                    "width": "20%"
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