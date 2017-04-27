
// 加载菜单
function getMenu() {
    $.ajaxExtend({
        url: "/Home/QueryMenu",
        success: function (d) {
            addNav(d);
        }
    });
}

$(function () {
    getMenu();
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