var ue = UE.getEditor('editor', {
    initialFrameHeight: 500
});
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
var datetime = $("#select-datetime");
if (datetime.val() == "0001/1/1 0:00:00")
{
    datetime.val(fn.getDate());
}

datetime.datetimepicker({
    format:  'yyyy-mm-ddTHH:mm',
    autoclose: true,
    todayBtn: true,
    language: "zh-CN",
    startDate: $("#select-datetime").val().replace("T", " ").substr(0, 10), 
    minuteStep: 10
});
$("#select-datetime").datetimepicker({
    onSelectDate: function (dateText, inst) {
        console.logs(dateText);
    }
}); 