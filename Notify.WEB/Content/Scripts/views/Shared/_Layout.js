/* 菜单已改由于数据库读取
var menus = [{
    "menuid": "10",
    "icon": "icon-sys",
    "menuname": "权限管理",
    "menus": [{
        "menuid": "111",
        "menuname": "菜单管理",
        "icon": "icon-nav",
        "url": "/Menu/MenuList"
    }, {
        "menuid": "113",
        "menuname": "角色管理",
        "icon": "icon-nav",
        "url": "/Role/RoleList"
    }]
}, {
    "menuid": "20",
    "icon": "icon-sys",
    "menuname": "用户管理",
    "menus": [{
        "menuid": "211",
        "menuname": "用户列表",
        "icon": "icon-nav",
        "url": "#"
    }]
}, {
    "menuid": "30",
    "icon": "icon-sys",
    "menuname": "基础数据管理",
    "menus": [{
        "menuid": "xxx",
        "menuname": "国家管理",
        "icon": "icon-nav",
        "url": "#"
    }
    , {
        "menuid": "xxccca",
        "menuname": "区域管理",
        "icon": "icon-nav",
        "url": "#"
    }, {
        "menuid": "xxa",
        "menuname": "省份管理",
        "icon": "icon-nav",
        "url": "#"
    }, {
        "menuid": "xxxxv",
        "menuname": "城市管理",
        "icon": "icon-nav",
        "url": "#"
    },
     {
        "menuid": "xxxccsxxxv",
        "menuname": "县城管理",
        "icon": "icon-nav",
        "url": "#"
    }]
}];
*/

var menus = getMenu();

// 加载菜单
function getMenu() {
    var menuTree;
    $.ajaxExtend({
        async: false,
        url: "/Home/QueryMenu",
        success: function (d) {
            menuTree = d;
        }
    });
    return menuTree;
}

$(function () {
    tabClose();
    tabCloseEven();
    clockon();

    addNav(menus);
    initLeftMenu();
    hoverMenuItem();

    $("#editpass").click(function () {
        $("#w").window("open");
    });

    $("#btnEp").click(function () {
        serverLogin();
    });

    $("#btnCancel").click(function () {
        closePwd();
    });

    $("#loginOut").click(function () {
        $.messager.confirm("系统提示", "您确定要退出本次登录吗?", function (r) {
            if (r) {
                $.ajaxExtend({
                    async: false,
                    url: "/Home/Logoff",
                    success: function (d) {
                        if (d.IsSucceed) {
                            window.location.href = "/Unauthorized/Login";
                        } else {
                            $.esayuiAlert("系统提示", "退出失败", "error");
                        }
                    }
                });
            }
        });
    });
});

// 添加化菜单
function addNav(data) {
    $.each(data, function (i, sm) {
        var menulist = new Array();
        menulist.push("<ul>");
        $.each(sm.menus, function (index, item) {
            menulist.push("<li>");
            menulist.push("<div>");
            menulist.push("<a ref='" + item.menuid + "' href='javascript:void(0);' rel='" + item.url + "'>");
            menulist.push("<span class='icon " + item.icon + "'>&nbsp;</span>");
            menulist.push("<span class='nav'>" + item.menuname + "</span>");
            menulist.push("</a>");
            menulist.push("</div>");
            menulist.push("</li>");
        });
        menulist.push("</ul>");
        $("#wnav").accordion("add", {
            title: sm.menuname,
            content: menulist.join(" "),
            iconCls: "icon " + sm.icon
        });
    });

    var panels = $("#wnav").accordion("panels");
    var title = panels[0].panel("options").title;
    $("#wnav").accordion("select", title);

}

// 初始化菜单
function initLeftMenu() {
    $("#wnav li a").on("click", function () {
        var tabTitle = $(this).children(".nav").text();
        var url = $(this).attr("rel");
        var menuid = $(this).attr("ref");
        var icon = getIcon(menuid);
        addTab(tabTitle, url, icon);
        $("#wnav li div").removeClass("selected");
        $(this).parent().addClass("selected");
    });

}

// 菜单项鼠标Hover
function hoverMenuItem() {
    $(".easyui-accordion").find("a").hover(function () {
        $(this).parent().addClass("hover");
    }, function () {
        $(this).parent().removeClass("hover");
    });
}

// 获取左侧导航的图标
function getIcon(menuid) {
    var icon = "icon ";
    $.each(menus, function (j, o) {
        $.each(o.menus, function (k, m) {
            if (m.menuid === menuid) {
                icon += m.icon;
                return false;
            }
            return true;
        });
    });
    return icon;
}

// 添加选项卡
function addTab(subtitle, url, icon) {
    if (!$("#tabs").tabs("exists", subtitle)) {
        $("#tabs").tabs("add", {
            title: subtitle,
            content: createFrame(url),
            closable: true,
            icon: icon
        });
    } else {
        $("#tabs").tabs("select", subtitle);
        $("#mm-tabupdate").click();
    }
    tabClose();
}

// 绑定关闭事件
function tabClose() {
    /* 双击关闭TAB选项卡 */
    $(".tabs-inner").dblclick(function () {
        var subtitle = $(this).children(".tabs-closable").text();
        $("#tabs").tabs("close", subtitle);
    });
    /* 为选项卡绑定右键 */
    $(".tabs-inner").bind("contextmenu", function (e) {
        $("#mm").menu("show", {
            left: e.pageX,
            top: e.pageY
        });

        var subtitle = $(this).children(".tabs-closable").text();

        $("#mm").data("currtab", subtitle);
        $("#tabs").tabs("select", subtitle);
        return false;
    });
}

// 创建iframe
function createFrame(url) {
    return "<iframe scrolling='auto' frameborder='0'  src='" + url + "' style='width:98%;height:97%; padding: 10px;'></iframe>";
}

// 绑定右键菜单事件
function tabCloseEven() {
    var firstTitle = "欢迎使用";

    // 刷新
    $("#mm-tabupdate").click(function () {
        var currTab = $("#tabs").tabs("getSelected");
        var url = $(currTab.panel("options").content).attr("src");
        $("#tabs").tabs("update", {
            tab: currTab,
            options: {
                content: createFrame(url)
            }
        });
    });

    // 关闭当前
    $("#mm-tabclose").click(function () {
        var currtab_title = $("#mm").data("currtab");
        $("#tabs").tabs("close", currtab_title);
    });

    // 全部关闭
    $("#mm-tabcloseall").click(function () {
        $(".tabs-inner span").each(function (i, n) {
            var t = $(n).text();
            if (t !== firstTitle) {
                $("#tabs").tabs("close", t);
            }

        });
    });

    // 关闭除当前之外的TAB
    $("#mm-tabcloseother").click(function () {
        $("#mm-tabcloseright").click();
        $("#mm-tabcloseleft").click();
    });

    // 关闭当前右侧的TAB
    $("#mm-tabcloseright").click(function () {
        var nextall = $(".tabs-selected").nextAll();
        if (nextall.length === 0) {
            msgShow("系统提示", "后边没有啦~~", "error");
            return false;
        }
        nextall.each(function (i, n) {
            var t = $("a:eq(0) span", $(n)).text();
            $("#tabs").tabs("close", t);
        });
        return false;
    });

    // 关闭当前左侧的TAB
    $("#mm-tabcloseleft").click(function () {
        var prevall = $(".tabs-selected").prevAll();
        if (prevall.length <= 1) {
            msgShow("系统提示", "到头了，前边没有啦~~", "error");
            return false;
        }
        prevall.each(function (i, n) {
            var t = $("a:eq(0) span", $(n)).text();
            if (t !== firstTitle) {
                $("#tabs").tabs("close", t);
            }
        });
        return false;
    });

    // 退出
    $("#mm-exit").click(function () {
        $("#mm").menu("hide");
    });
}

// 弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}

// 设置登录窗口
function openPwd() {
    $("#w").window({
        title: "修改密码",
        width: 300,
        modal: true,
        shadow: true,
        closed: true,
        height: 160,
        resizable: false
    });
}

// 关闭登录窗口
function closePwd() {
    $("#w").window("close");
}

// 修改密码
function serverLogin() {
    var $newpass = $("#txtNewPass");
    var $rePass = $("#txtRePass");

    if ($newpass.val() === "") {
        msgShow("系统提示", "请输入密码！", "warning");
        return false;
    }
    if ($rePass.val() === "") {
        msgShow("系统提示", "请在一次输入密码！", "warning");
        return false;
    }

    if ($newpass.val() !== $rePass.val()) {
        msgShow("系统提示", "两次密码不一至！请重新输入", "warning");
        return false;
    }

    $.post("/ajax/editpassword.ashx?newpass=" + $newpass.val(), function (msg) {
        msgShow("系统提示", "恭喜，密码修改成功！<br>您的新密码为：" + msg, "info");
        $newpass.val("");
        $rePass.val("");
        close();
    });

    return false;
}

// 本地时钟
function clockon() {
    var now = new Date();
    var year = now.getFullYear(); // getFullYear getYear
    var month = now.getMonth();
    var date = now.getDate();
    var day = now.getDay();
    var hour = now.getHours();
    var minu = now.getMinutes();
    var sec = now.getSeconds();
    month = month + 1;
    if (month < 10)
        month = "0" + month;
    if (date < 10)
        date = "0" + date;
    if (hour < 10)
        hour = "0" + hour;
    if (minu < 10)
        minu = "0" + minu;
    if (sec < 10)
        sec = "0" + sec;
    var arr_week = new Array("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六");
    var week = arr_week[day];
    var time = year + "年" + month + "月" + date + "日" + " " + hour + ":" + minu + ":" + sec + " " + week;

    $("#bgclock").html(time);

    setTimeout("clockon()", 200);
}