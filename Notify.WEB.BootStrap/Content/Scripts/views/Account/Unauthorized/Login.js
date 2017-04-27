function getValidateCode() {
    var date = new Date();
    var validateCode = document.getElementById("validateCode");
    validateCode.src = "/Unauthorized/GetValidateCode?v=" + date;
}

function sendParam() {
    var parameter = JSON.stringify({ userName: $.trim($("#user").val()), password: $.trim($("#pwd").val()), validateCode: $.trim($("#code").val()) });
    $.ajaxExtend({
        data: parameter,
        async: false,
        url: "/Unauthorized/Login",
        success: function (d) {
            if (d.IsSucceed) {
                window.location.href = "/Home/Index";
            } else {
                getValidateCode();
                $.layerAlert(d.Message, { icon: 2 });
            }
        }
    });
}

$(function () {
    $("#btnLogin").click(function () {
        if ($.trim($("#user").val()).length <= 0) {
            $.layerTips("请输入账号", "#user");
            return false;
        }
        if ($.trim($("#pwd").val()).length <= 0) {
            $.layerTips("请输入密码", "#user");
            return false;
        }
        if ($.trim($("#code").val()).length <= 0) {
            $.layerTips("请输入验证码", "#user");
            return false;
        }
        sendParam();
        return false;
    });
});