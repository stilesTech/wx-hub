v ????  ?me = $("#select-datetime"),
    addCotegory = $("#add-category"),
    editCategory = $("#edit-category"),
    categoryName = $("#category-name"),
    categoryOrder = $("#category-order"),
    categoryContent = $("#category-content"),
    eeditCategory = $(".edit-category"),
    delCategory = $(".del-category");
var fn = {
    getDate: function () {
        var date = new Date();
        var y = date.getFullYear(),
            m = date.getMonth() + 1 < 10 ? "0" + Number(date.getMonth() + 1) : date.getMonth() + 1,
            d = date.getDate() < 10 ? "0" + date.getDate() : date.getDate(),
            h = date.getHours() < 10 ? "0" + date.getHours() : date.getHours(),
            i = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        return y + "-" + m + "-" + d + " " + h + ":" + i;
    }
};
datetime.val(fn.getDate());
datetime.datetimepicker({
    format: "yyyy-mm-dd hh:ii",
    autoclose: true,
    todayBtn: true,
    language: "zh-CN",
    startDate: "2013-02-14 10:00",
    minuteStep: 10
});

/**
 * cotegory
 */

//add category
addCotegory.on("click", function () {
    categoryName.attr("value", "");
    categoryOrder.attr("value", "");
    categoryContent.attr("value", "");
    editCategory.fadeIn();
    $("#form-edit").attr("action", "CreateCategory");
    $("#add-edit").text("提交");
});

//edit category
eeditCategory.on("click", function () {
    //TODO

    editCategory.fadeIn();
});
//del category
delCategory.on("click", function () {
    //TODO
});

function PostProEdit(val) {
    $.post("PostEdit",
            { id: val }, function (data) {
                $("#category-name").attr("value", data["CategoryName"]);
                $("#category-order").attr("value", data["Order"]);
                $("#category-content").val(data["CategoryDesc"]);
                $("#category-id").attr("value", data["Id"]);
                $("#form-edit").attr("action", "update");
                $("#add-edit").text("更新");
            }, "json"
    );
}

function PostNewsEdit(val) {
    $.post("CategoryEdit",
        { id: val }, function (data) {
            $("#category-name").attr("value", data["N_T_Name"]);
            $("#category-order").attr("value", data["N_T_Order"]);
            $("#category-content").val(data["N_T_Summary"]);
            $("#category-id").attr("value", data["N_T_Id"]);
            $("#form-edit").attr("action", "UpdateCategory");
            $("#add-edit").text("更新");
        }, "json"
    );
}
function IsDelNow(){
   return confirm("确认要删除？");
}