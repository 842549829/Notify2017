
//获取input的所有id
var user = document.getElementById("user");
var pwd = document.getElementById("pwd");
var surePwd = document.getElementById("surePwd");


// 获取span的所有id
var user_pass = document.getElementById("user_pass");
var pwd_pass = document.getElementById("pwd_pass");
var surePwd_pass = document.getElementById("surePwd_pass");
function checkUser() {
    //如果昵称未输入，则提示输入昵称
    if (!user.value) {
        user_pass.style.fontSize = "13px";
        user_pass.style.width = "31%";
        user_pass.style.height = "2em";
        user_pass.style.textAlign = "center";
        user_pass.style.lineHeight = "2em";
        user_pass.style.marginTop = "0.05%";
        user_pass.innerHTML = "你还没有填写用户名哦。";
        user_pass.style.display = "block";
    }
    else if (user.value) {
        user_pass.style.display = "none";
    }
}

// 输入密码提示
function checkUserPwd() {
    //如果未输入密码，则提示请输入密码
    if (!pwd.value) {
        pwd_pass.style.fontSize = "13px";
        pwd_pass.style.width = "31%";
        pwd_pass.style.height = "2em";
        pwd_pass.style.textAlign = "center";
        pwd_pass.style.lineHeight = "2em";
        pwd_pass.innerHTML = "你还没有填写密码哦。";
        pwd_pass.style.display = "block";
    }
    else {
        pwd_pass.innerHTML = "";
        pwd_pass.style.backgroundColor = "#fff";
        pwd_pass.style.border = "none";
        pwd_pass.style.display = "none";

    }

}

// 输入验证码提示
function checkUserConfirmPwd() {
    // 再次确认密码
    if (!surePwd.value) {
        surePwd_pass.style.fontSize = "13px";
        surePwd_pass.style.width = "31%";
        surePwd_pass.style.height = "2em";
        surePwd_pass.style.textAlign = "center";
        surePwd_pass.style.lineHeight = "2em";
        surePwd_pass.innerHTML = "你还没有填写验证码哦";
        surePwd_pass.style.display = "block";
    }
    else {
        surePwd_pass.innerHTML = "";
        surePwd_pass.style.backgroundColor = "#fff";
        surePwd_pass.style.border = "none";
        surePwd_pass.style.display = "none";
    }
}

function submitB() {

    if (!user.value) {
        user_pass.style.fontSize = "13px";
        user_pass.style.width = "31%";
        user_pass.style.height = "2em";
        user_pass.style.textAlign = "center";
        user_pass.style.lineHeight = "2em";
        user_pass.innerHTML = "请填写您的用户名。";
        user.focus();
        return false;
    }
    if (!pwd.value) {
        pwd_pass.style.fontSize = "13px";
        pwd_pass.style.width = "31%";
        pwd_pass.style.height = "2em";
        pwd_pass.style.textAlign = "center";
        pwd_pass.style.lineHeight = "2em";
        pwd_pass.innerHTML = "请填写您的用户密码。";
        pwd.focus();
        return false;
    }

    if (!surePwd.value) {
        surePwd_pass.style.fontSize = "13px";
        surePwd_pass.style.width = "31%";
        surePwd_pass.style.height = "2em";
        surePwd_pass.style.textAlign = "center";
        surePwd_pass.style.lineHeight = "2em";
        surePwd_pass.innerHTML = "请填写您的登录验证。";
        surePwd_pass.focus();
        return false;
    }
    else {
        sendParam();
        return false;
    }
}

function getValidateCode() {
    var date = new Date();
    var validateCode = document.getElementById("validateCode");
    validateCode.src = "/Unauthorized/GetValidateCode?v=" + date;
}

function sendParam() {
    var parameter = JSON.stringify({ userName: $.trim($("#user").val()), password: $.trim($("#pwd").val()), validateCode: $.trim($("#surePwd").val()) });
    $.ajaxExtend({
        data: parameter,
        async: false,
        url: "/Unauthorized/Login",
        success: function (d) {
            if (d.IsSucceed) {
                window.location.href = "/Home/Index";
            } else {
                getValidateCode();
                $.layerAlert(d.Message);
            }
        }
    });
}