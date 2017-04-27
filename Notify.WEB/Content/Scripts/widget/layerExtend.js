function success(msg, callback) {
    $.layer({
        dialog: {
            btns: 1,
            btn: ['确定'],
            type: 1,
            msg: msg,
            yes: function () {
                callback();
            }
        },
        area: ['auto', 'auto'],
        close: function () {
            callback();
        }
    });
}

var layerIndex;
function tipsClick(obj, text) {
    layerIndex = window.layer.tips(text, document.getElementById(obj), {
        style: ['background-color:red; color:#fff', 'red'],
        maxWidth: 220,
        guide: 0,
        time: 5
    });
}

// 关闭layer弹窗
function closeLayer() {
    if (layerIndex) {
        window.layer.close(layerIndex);
    }
}