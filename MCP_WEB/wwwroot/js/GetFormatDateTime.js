function GetFormatDateTime(e) {
    var datatime = "";
    $.ajax({
        type: 'GET',
        url: 'api/m_UserMaster/FormatDate',
        async: false,
        dataType: 'json',
        success: function (result) {
            $.each(result, function (key, value) {
                datatime = value.param_value.split("-");
                datatime = datatime[0].toLowerCase() + "-" + datatime[1] + "-" + datatime[2].toLowerCase();
                //console.log(datatime);
            });
        }
    });
    return datatime;
}
