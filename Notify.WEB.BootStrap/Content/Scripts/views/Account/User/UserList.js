// 打开窗口
function openAddUser() {
    cleraAddUser();
    window.layer.open({
        type: 1,
        shade: null,//[0.1, "#000"],
        area: ["750", "auto"],
        title: ["添加用户", ""],
        border: [1],
        closeBtn: 1,
        content: $("#addUser")
    });
    return false;
}


// 关闭
function closeEditRole() {
    window.layer.closeAll();
}

// 清除编辑信息
function cleraAddUser() {
    $("#txtAccountNo").val("");
    $("#txtAccountName").val("");
    $("#txtMail").val("");
    $("#txtMobile").val("");
}

// 验证
function validateRole(user) {
    if (user.AccountNo === "") {
        $.layerAlert("请填写帐号", { icon: 2 });
        return false;
    }
    if (user.AccountName === "") {
        $.layerAlert("请填写用户名", { icon: 2 });
        return false;
    }
    if (user.Mail === "") {
        $.layerAlert("请填写邮箱", { icon: 2 });
        return false;
    }
    if (user.Mobile === "") {
        $.layerAlert("请填写手机", { icon: 2 });
        return false;
    }
    return true;
}

//保存
function saveAddUser() {
    var parameter = new Object();
    parameter.AccountNo = $("#txtAccountNo").val();
    parameter.AccountName = $("#txtAccountName").val();
    parameter.Mail = $("#txtMail").val();
    parameter.Mobile = $("#txtMobile").val();
    if (validateRole(parameter)) {
        $.ajaxExtend({
            data: JSON.stringify(parameter),
            url: "/User/AddUser",
            success: function (d) {
                if (d.IsSucceed) {
                    $("#btnSubmit").click();
                    $.layerAlert(d.Message, { icon: 2 });
                } else {
                    $.layerAlert(d.Message, { icon: 2 });
                }
            }
        });
    }
}