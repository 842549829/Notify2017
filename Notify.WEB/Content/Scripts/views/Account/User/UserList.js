// 查询
function findData() {
    $("#listData").datagrid("load", {
        AccountNo: $("#AccountNo").val(),
        AccountName: $("#AccountName").val()
    });
}

// 关闭窗口
function closeAddUser() {
    $("#addUser").window("close");
}

// 打开窗口
function openAddUser() {
    cleraAddUser();
    $("#addUser").window({
        iconCls: "icon-add",
        title: "添加角色",
        width: 300,
        modal: true,
        shadow: true,
        closed: false,
        height: 200,
        resizable: false
    });
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
        $.messager.alert("系统提示", "请填写帐号", "warning");
        return false;
    }
    if (user.AccountName === "") {
        $.messager.alert("系统提示", "请填写用户名", "warning");
        return false;
    }
    if (user.Mail === "") {
        $.messager.alert("系统提示", "请填写邮箱", "warning");
        return false;
    }
    if (user.Mobile === "") {
        $.messager.alert("系统提示", "请填写手机", "warning");
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
                    closeAddUser();
                    findData();
                    $.messager.alert("系统提示", d.Message, "info");
                } else {
                    $.messager.alert("系统提示", d.Message, "error");
                }
            }
        });
    }
}

//操作格式化
function operationFormater(value, row) {
    if (row.IsAdmin) {
        return "";
    }
    var operationHtml = new Array();
    operationHtml.push("<a class='setrolecls' href='/PermissionRole/PermissionRoleList?accountId=" + row.Id + "&accountName=" + row.AccountName + "'>设置角色</a>");
    //operationHtml.push("<a class='editcls' href='javascript:void(0);' data='" + encodeURI(JSON.stringify(row)) + "'>修改</a>");
    operationHtml.push("<a class='removecls' href='javascript:void(0);' data='" + encodeURI(JSON.stringify(row)) + "'>删除</a>");
    return operationHtml.join(" ");
}

//状态格式
function statusFormater(value, row) {
    if (row.Status === 1) {
        return "启用";
    } else {
        return "禁用";
    }
}

//是否管理员格式
function isAdminFormater(value, row) {
    if (row.IsAdmin) {
        return "是";
    } else {
        return "否";
    }
}

//创建时间格式
function createTimeFormater(value, row) {
    var strDate = row.CreateTime.toString();
    var date = new Date(Date.parse(strDate.replace(/-/g, "/")));
    return date.Format("yyyy-MM-dd HH:mm:ss");
}

$(function () {
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
        url: "/User/UserListVal",
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
            $(".setrolecls").linkbutton({ text: "设置角色", plain: true, iconCls: "icon-mini-edit" });
            //$(".editcls").linkbutton({ text: "修改", plain: true, iconCls: "icon-edit" }).click(function () {
            //    var user = JSON.parse(decodeURI($(this).attr("data")));
            //});
            $(".removecls").linkbutton({ text: "删除", plain: true, iconCls: "icon-remove" }).click(function () {
                var user = JSON.parse(decodeURI($(this).attr("data")));
                $.messager.confirm("系统提示", "确认要删除吗", function (r) {
                    if (r) {
                        $.ajaxExtend({
                            data: JSON.stringify({ "userId": user.Id }),
                            url: "/User/RemoveUser",
                            success: function (d) {
                                if (d.IsSucceed) {
                                    closeAddUser();
                                    findData();
                                    $.messager.alert("系统提示", d.Message, "info");
                                } else {
                                    $.messager.alert("系统提示", d.Message, "error");
                                }
                            }
                        });
                    }
                });
            });;
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