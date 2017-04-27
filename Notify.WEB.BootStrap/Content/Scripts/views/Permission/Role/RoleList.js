// 设置编辑窗口
function openEditRole(title) {
    window.layer.open({
        type: 1,
        shade: null,//[0.1, "#000"],
        area: ["750", "auto"],
        title: [title, ""],
        border: [1],
        closeBtn: 1,
        content: $("#editRole")
    });
    return false;
}

// 设置角色编辑信息
function setRoleEditInfo(role) {
    if (role) {
        $("#hidRoleId").val(role.Id);
        $("#txtRoleName").val(role.RoleName);
        $("#textRoleDescription").val(role.RoleDescription);
    } else {
        $("#hidRoleId").val("");
        $("#txtRoleName").val("");
        $("#textRoleDescription").val("");
    }
}

// 验证
function validateRole(role) {
    if (role.RoleName === "") {
        $.layerAlert("请填写角色名称", { icon: 2 });
        return false;
    }
    if (role.RoleDescription === "") {
        $.layerAlert("请填写角色描述", { icon: 2 });
        return false;
    }
    return true;
}

$(function () {
    // 添加角色
    $("#btnAddRole").on("click", function () {
        setRoleEditInfo(null);
        openEditRole("添加角色");
    });

    // 保存数据
    $("#btnEp").click(function () {
        var roleId = $("#hidRoleId").val();
        var parameter = new Object();
        var url;
        if (roleId === "") {
            url = "/Role/AddRole";
        } else {
            parameter.Id = roleId;
            url = "/Role/UpdateRole";
        }
        parameter.RoleName = $("#txtRoleName").val();
        parameter.RoleDescription = $("#textRoleDescription").val();
        if (validateRole(parameter)) {
            $.ajaxExtend({
                data: JSON.stringify(parameter),
                url: url,
                success: function (d) {
                    if (d.IsSucceed) {
                        closeEditRole();
                        $("#btnSubmit").click();
                        $.layerAlert(d.Message, { icon: 1 });
                    } else {
                        $.layerAlert(d.Message, { icon: 2 });
                    }
                }
            });
        }
    });
});

// 添加角色
function addRole() {
    setRoleEditInfo(null);
    openEditRole("添加角色");
    return false;
}

// 关闭
function closeEditRole() {
    window.layer.closeAll();
}

// 修改角色
function updateRole(self) {
    var role = JSON.parse($(self).attr("data"));
    setRoleEditInfo(role);
    openEditRole("修改角色");
    return false;
}

// 删除角色
function removeRoele(self) {
    var role = JSON.parse($(self).attr("data"));
    $.layerConfirm("你确定要删除吗", { icon: 3, title: "提示" }, function () {
        $.ajaxExtend({
            data: JSON.stringify({ "roleId": role.Id }),
            url: "/Role/RemoveRole",
            success: function (d) {
                if (d.IsSucceed) {
                    closeEditRole();
                    $("#btnSubmit").click();
                    $.layerAlert(d.Message, { icon: 1 });
                } else {
                    $.layerAlert(d.Message, { icon: 2 });
                }
            }
        });
    });
    return false;
}