function findData() {
    $("#listData").datagrid("load", {
        RoleName: $("#RoleName").val(),
        RoleDescription: $("#RoleDescription").val()
    });
}

// 设置编辑窗口
function openEditRole(title, iconCls) {
    $("#editRole").window({
        iconCls: iconCls,
        title: title,
        width: 300,
        modal: true,
        shadow: true,
        closed: false,
        height: 200,
        resizable: false
    });
    return false;
}

// 关闭登录窗口
function closeEditRole() {
    $("#editRole").window("close");
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
        $.messager.alert("系统提示", "请填写角色名称", "warning");
        return false;
    }
    if (role.RoleDescription === "") {
        $.messager.alert("系统提示", "请填写角色描述", "warning");
        return false;
    }
    return true;
}

//操作
function rowformater(value, row) {
    var operationHtml = new Array();
    operationHtml.push("<a class='setpermissions' href='/PermissionMenu/PermissionMenuList?roleId=" + row.Id + "&roleName=" + row.RoleName + "'>设置权限</a>");
    operationHtml.push("<a class='editcls' href='javascript:void(0);' data='" + encodeURI(JSON.stringify(row)) + "'>修改</a>");
    operationHtml.push("<a class='removecls' href='javascript:void(0);' data='" + encodeURI(JSON.stringify(row)) + "'>删除</a>");
    return operationHtml.join(" ");
}

$(function () {
    // 添加角色
    $("#btnAddRole").on("click", function () {
        setRoleEditInfo(null);
        openEditRole("添加角色", "icon-save");
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
                        findData();
                        $.messager.alert("系统提示", d.Message, "info");
                    } else {
                        $.messager.alert("系统提示", d.Message, "error");
                    }
                }
            });
        }
    });

    // 分页数据加载
    $("#listData").datagrid({
        title: "请输入查询条件",
        loadMsg: "正在查询...",
        iconCls: "icon-edit",//图标
        width: "98%",
        height: "auto",
        nowrap: false,
        striped: true,
        border: true,
        collapsible: false, // 是否可折叠的
        fit: true, // 列自动大小
        url: "/Role/RoleListVal",
        //sortName: 'code',
        //sortOrder: 'desc',
        remoteSort: false,
        idField: "fldId",
        singleSelect: true, // 是否单选(只允许设置选择一行)
        pagination: true, // 分页控件
        pageNumber: 1, //当前页码
        pageSize: 15, //每页显示的记录条数，默认为10
        pageList: [15, 30],//可以设置每页记录条数的列表
        rownumbers: false, // 行号,
        fitColumns: false,
        //frozenColumns: [[
        //    { field: "ck", checkbox: true }
        //]],
        toolbar: "#searchtool",
        onLoadSuccess: function () {
            $(".setpermissions").linkbutton({ text: "权限设置", plain: true, iconCls: "icon-mini-edit" });
            $(".editcls").linkbutton({ text: "修改", plain: true, iconCls: "icon-edit" }).click(function () {
                var role = JSON.parse(decodeURI($(this).attr("data")));
                setRoleEditInfo(role);
                openEditRole("修改角色", "icon-edit");
            });
            $(".removecls").linkbutton({ text: "删除", plain: true, iconCls: "icon-remove" }).click(function () {
                var role = JSON.parse(decodeURI($(this).attr("data")));
                $.messager.confirm("系统提示", "确认要删除吗", function (r) {
                    if (r) {
                        $.ajaxExtend({
                            data: JSON.stringify({ "roleId": role.Id }),
                            url: "/Role/RemoveRole",
                            success: function (d) {
                                if (d.IsSucceed) {
                                    closeEditRole();
                                    findData();
                                    $.messager.alert("系统提示", d.Message, "info");
                                } else {
                                    $.messager.alert("系统提示", d.Message, "error");
                                }
                            }
                        });
                    }
                });
            });
        }
    });

    //设置分页控件
    var p = $("#listData").datagrid("getPager");
    $(p).pagination({
        pageNumber: 1, //当前页码
        pageSize: 15,//每页显示的记录条数，默认为10
        pageList: [15, 30],//可以设置每页记录条数的列表
        beforePageText: "第",//页数文本框前显示的汉字
        afterPageText: "页    共 {pages} 页",
        displayMsg: "当前显示 {from} - {to} 条记录   共 {total} 条记录",
        onBeforeRefresh: function () {
            $(this).pagination("loading");
            $(this).pagination("loaded");
        }
    });
});