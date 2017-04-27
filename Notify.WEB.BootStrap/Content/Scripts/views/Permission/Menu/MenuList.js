var setting = {
    view: {
        addHoverDom: function (treeId, treeNode) {
            var sObj = $("#" + treeNode.tId + "_span");
            if (treeNode.editNameFlag || $("#addBtn_" + treeNode.tId).length > 0) {
                return;
            }
            var addStr = "<span class='button add' id='addBtn_" + treeNode.tId + "' title='add node' onfocus='this.blur();'></span>";
            sObj.after(addStr);
            var btn = $("#addBtn_" + treeNode.tId);
            if (btn) {
                btn.bind("click", function () {
                    clearEdit();
                    $("#hidOperationType").val("Add");
                    $("#hidParnetId").val(treeNode.id);
                    return false;
                });
            }
        },
        removeHoverDom: function (treeId, treeNode) {
            $("#addBtn_" + treeNode.tId).unbind().remove();
        },
        selectedMulti: false
    },
    edit: {
        enable: true,
        editNameSelectAll: true,
        showRemoveBtn: function (treeId, treeNode) {
            // 根目标显示删除按钮
            return treeNode.level !== 0;
        },
        showRenameBtn: function (treeId, treeNode) {
            // 根目标显示编辑按钮
            return treeNode.level !== 0;
        }
    },
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        beforeDrag: null,
        // 编辑节点事件
        beforeEditName: function (treeId, treeNode) {
            clearEdit();
            $("#hidOperationType").val("Update");
            $("#hidParnetId").val(treeNode.pId);
            $("#hidMenuId").val(treeNode.id);
            var parameter = JSON.stringify({ menuId: treeNode.id });
            $.ajaxExtend({
                data: parameter,
                async: false,
                url: "/Menu/QueryMenuById",
                success: function (d) {
                    $("#txtMenuName").val(d.MenuName);
                    $("#txtMenuUrl").val(d.MenuUrl);
                    $("#txtMenuSort").val(d.MenuSort);
                    $("#txtMenuIcon").val(d.MenuIcon);
                    $("#txtMenuDescription").val(d.MenuDescription);
                }
            });
            return false;
        },
        beforeRemove: function (treeId, treeNode) {
            clearEdit();
            $.layerConfirm("你确定要删除吗", { icon: 3, title: "提示" }, function () {
                $.ajaxExtend({
                    data: JSON.stringify({ "menuId": treeNode.id }),
                    url: "/Menu/RemoveMenu",
                    success: function (d) {
                        if (d.IsSucceed) {
                            initMenu();
                            $.layerAlert(d.Message, { icon: 1 });
                        } else {
                            $.layerAlert(d.Message, { icon: 2 });
                        }
                    }
                });
                return false;
            });
            return false;
        },
        beforeRename: null,
        onRemove: null,
        onRename: null,
        // 单机节点事件
        onClick: function (event, treeId, treeNode) {
            $("#hidParnetId").val(treeNode.id);
        }
    }
};

$(document).ready(function () {
    initMenu();
    $("#btnEditMenu").click(saveMenu);
    $("#btClearMenu").click(clearEdit);
});

function getMenuTree() {
    var menuTree;
    $.ajaxExtend({
        async: false,
        url: "/Menu/QueryMenuList",
        success: function (d) {
            menuTree = d;
        }
    });
    return menuTree;
}

function initMenu() {
    $.fn.zTree.init($("#tree"), setting, getMenuTree());
}

function clearEdit() {
    $("#hidOperationType").val("Add");
    $("#hidParnetId").val("");
    $("#txtMenuName").val("");
    $("#txtMenuUrl").val("");
    $("#txtMenuSort").val("");
    $("#txtMenuIcon").val("");
    $("#txtMenuDescription").val("");
    $("#hidMenuId").val("");
    return false;
}

function validateMenu(menu) {
    if (menu.MenuName === "") {
        $.layerTips("请输入菜单名称", "#txtMenuName");
        return false;
    }
    if (menu.MenuUrl === "") {
        $.layerTips("请输入菜单URL", "#txtMenuUrl");
        return false;
    }
    if (menu.MenuSort === "") {
        $.layerTips("请输入菜单序号", "#txtMenuSort");
        return false;
    }
    if (menu.MenuIcon === "") {
        $.layerTips("请输入菜单图标", "#txtMenuIcon");
        return false;
    }
    if (menu.MenuDescription === "") {
        $.layerTips("请输入菜单描述", "#txtMenuDescription");
        return false;
    }
    return true;
}

function saveMenu() {
    var menu = new Object();
    menu.ParentId = $("#hidParnetId").val();;
    menu.MenuName = $("#txtMenuName").val();
    menu.MenuUrl = $("#txtMenuUrl").val();
    menu.MenuSort = $("#txtMenuSort").val();
    menu.MenuIcon = $("#txtMenuIcon").val();
    menu.MenuDescription = $("#txtMenuDescription").val();

    var operationType = $("#hidOperationType").val();
    if (operationType === "Add") {
        if (menu.ParentId === "") {
            $.layerAlert("请选择上级菜单", { icon: 2 });
            return false;
        }
        if (!validateMenu(menu)) {
            return false;
        }
        var addParameter = JSON.stringify(menu);
        $.ajaxExtend({
            data: addParameter,
            url: "/Menu/AddMenu",
            success: function (d) {
                initMenu();
                $.layerAlert(d.Message, { icon: 1 });
            }
        });
    } else if (operationType === "Update") {
        if (!validateMenu(menu)) {
            return false;
        }
        menu.Id = $("#hidMenuId").val();
        var updateParameter = JSON.stringify(menu);
        $.ajaxExtend({
            data: updateParameter,
            url: "/Menu/UpdateMenu",
            success: function (d) {
                initMenu();
                $.layerAlert(d.Message, { icon: 1 });
            }
        });
    }
    return false;
}