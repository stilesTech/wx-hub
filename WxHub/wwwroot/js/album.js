var photoCss = "photoview_4X";
function UploadResultFun(file, data) {
    var jsonRe = eval("(" + data + ")");
    if (jsonRe["err"] != "") {
        $("#retip").append("Error:" + file.name + " " + jsonRe["err"]);
    }
    else {
        var t = "<li class=\"" + photoCss + "\">";
        t = t + "<img src=\"" + (Urls.RootPath != "/" ? Urls.RootPath + jsonRe['msg']['url'] : jsonRe['msg']['url']) + "\"/>";
        t = t + "<input type=\"text\" value=\"" + (file.name).substr(0, file.name.lastIndexOf('.')) + "\"/>";
        t = t + "<textarea class=\"imgaltinfo hide\" rows=\"4\"></textarea>";
        t = t + "<input type=\"radio\" class=\"iscoverbtn\" name=\"IsCover\"/>";
        t = t + "<input type=\"hidden\" value=\"" + jsonRe['msg']['url'] + "\"/>";
        t = t + "<a class=\"albumphotodel\" ><i class=\"icon-remove\"></i></a>";
        t = t + "</li>";

        $(".photobox").append(t);
    }
}


function GetPhotos() {
    var jsonPhoto = "[";
    $(".photobox li").each(function () {
        if ($(this).html() != "" && $(this).css("display") != "none") {
            jsonPhoto = jsonPhoto + "{\"Src\":\"" + $($(this).children()[4]).val() + "\",";
            jsonPhoto = jsonPhoto + "\"Title\":\"" + $($(this).children()[1]).val().replace(/\"/g, '\'') + "\",";
            jsonPhoto = jsonPhoto + "\"Alt\":\"" + $($(this).children()[2]).val().replace(/\"/g, '\'') + "\",";
            jsonPhoto = jsonPhoto + "\"IsCover\":\"" + ($($(this).children()[3]).prop('checked') ? '1' : '0') + "\"},";
        }
    });

    if (jsonPhoto.length > 1) {
        jsonPhoto = jsonPhoto.substr(0, jsonPhoto.length - 1);
    }
    jsonPhoto = jsonPhoto + "]";

    $("#Text").val(jsonPhoto);
}

function InitialPhotos() {
    var jsonPhoto = eval("(" + $("#Text").val() + ")");
    $.each(jsonPhoto, function (i) {
        var t = "<li class=\"" + photoCss + "\">";
        t = t + "<img src=\"" + (Urls.RootPath != "/" ? Urls.RootPath + jsonPhoto[i]['Src'] : jsonPhoto[i]['Src']) + "\"/>";
        t = t + "<input type=\"text\" value=\"" + jsonPhoto[i]['Title'] + "\"/>";
        t = t + "<textarea class=\"imgaltinfo hide\" rows=\"4\">" + jsonPhoto[i]['Alt'] + "</textarea>";
        t = t + "<input type=\"radio\" class=\"iscoverbtn\" name=\"IsCover\" " + (jsonPhoto[i]['IsCover'] == "1" ? " checked=\"checked\"" : "") + "/>";
        t = t + "<input type=\"hidden\" value=\"" + jsonPhoto[i]['Src'] + "\"/>";
        t = t + "<a class=\"albumphotodel\" ><i class=\"icon-remove\"></i></a>";
        t = t + "</li>";
        $(".photobox").append(t);
    });
}


$(function () {
    $(".photoDisplayChange a").click(function () {
        var tar = $(".photoUpload ul li");
        switch ($(this).html()) {
            case '1X':
                photoCss = 'photoview_1X';
                break;
            case '4X':
                photoCss = 'photoview_4X';
                break;
            case '6X':
                photoCss = 'photoview_6X';
                break;
            case '9X':
                photoCss = 'photoview_9X';
                break;
        }
        tar.removeClass().addClass(photoCss);
    });

    $('#albumaddform').on('click', ".albumphotodel", function () {
        $(".resetPhotoDelete").show();
        $(this).parent().css({ "display": "none" });
    });

    $(".resetPhotoDelete").click(function () {
        $('ul.photobox li').show();
        $(this).hide();
    });

});