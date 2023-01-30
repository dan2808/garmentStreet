
//function getOption() {
//    console.log(document.getElementById('').value);
//}

//$("mySelect").change(function () {
//    // code to make AJAX call goes here
//    console.log("option selected");
//});

//const select = document.getElementById("mySelect");
//const option = document.getElementById("option");
//select.onchange = function () {
//    option.innerText = select.options[select.selectedIndex].value;

//};



$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: "/Admin/Test/GetAllCategories",
        contentType: 'application/json',
        dataType: "json",
        success: function (data) {
            var s = '<option value="-1">Please Select a Category</option>';
            for (var i = 0; i < data.data.length; i++) {
                console.log(data[i]);
                s += '<option value="' + data.data[i].id + '">' + data.data[i].name + '</option>';
            }
            $("#categoryDropdown").html(s);
        }
    });
});  

const select = document.getElementById("categoryDropdown");

select.onchange = function () {
    $.ajax({
        type: "GET",
        url: "/Admin/Test/GetVariationByCategoryId" + "?id=" + document.querySelector('#categoryDropdown').value  ,
        contentType: 'application/json',
        dataType: "json",
        success: function (data) {
            var s = '<option value="-1">Please Select a Category Demographic</option>';
            for (var i = 0; i < data.data.length; i++) {
                console.log(data[i]);
                s += '<option value="' + data.data[i].id + '">' + data.data[i].name + '</option>';
            }
            $("#variationDropdown").html(s);
        }
    });
};