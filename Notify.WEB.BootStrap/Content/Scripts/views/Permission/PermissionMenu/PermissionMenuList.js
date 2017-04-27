var setting = {
    view: {
        addHoverDom: null,
        removeHoverDom: null,
        selectedMulti: false
    },
    edit: {
        enable: true,
        editNameSelectAll: true,
        showRemoveBtn: false,
        showRenameBtn: false
    },
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        beforeDrag: null,
        beforeEditName: null,
        beforeRemove: null,
        beforeRename: null,
        onRemove: null,
        onRename: null,
        onClick: null
    },
    check: {
        enable: true,
        chkStyle: "checkbox",
        chkboxType: { "Y": "ps", "N": "ps" }
    }
};

$(document).ready(function () {
    initMenu();
    $("#btnSave").click(function () {
        var menuIds = getCheckValue();
        var roleId = $.trim($("#hidRoleId").val());
        var parameter = { roleId: roleId, menuIds: menuIds };
        var data = JSON.stringify(parameter);
        $.ajaxExtend({
            data: data,
            url: "/PermissionMenu/SavePermissionMenu",
            success: function (d) {
                if (d.IsSucceed) {
                    $.layerAlert(d.Message, { icon: 1 });
                } else {
                    $.layerAlert(d.Message, { icon: 2 });
                }
            }
        });
    });
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
    loadRoleInfo();
}

// 获取选中的菜单
function getCheckValue() {
    var checkedValueArray = new Array();
    var zTree = $.fn.zTree.getZTreeObj("tree");
    var nodes = zTree.getCheckedNodes(true);
    if (nodes && nodes.length > 0) {
        for (var i = 0; i < nodes.length; i++) {
            var node = nodes[i];
            checkedValueArray.push(node.id);
        }
    }
    return checkedValueArray;
};

// 设置已有的权限选中
function loadRoleInfo() {
    var treeObj = $.fn.zTree.getZTreeObj("tree");
    var roleId = $.trim($("#hidRoleId").val());
    $.ajaxExtend({
        async: false,
        data: JSON.stringify({ roleId: roleId }),
        url: "/PermissionMenu/QueryMenuIds",
        success: function (d) {
            $.each(d, function (index, item) {
                if (item != null && treeObj.getNodeByParam("id", item, null) != null && treeObj.getNodeByParam("id", item, null).isParent === false) {
                    treeObj.checkNode(treeObj.getNodeByParam("id", item, null), true, true);
                }
            });
        }
    });
}