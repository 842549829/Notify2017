var clip = null;
ZeroClipboard.setMoviePath("/Scripts/tools/copy/swf/ZeroClipboard.swf");
function copyText(id, text) {
    try {
        clip = new ZeroClipboard.Client();
        clip.setHandCursor(true);
        clip.setText(text);
        clip.glue(id);
        var complete = "complete";
        clip.addEventListener(complete, function () {
            alert("内容已复制到剪贴板！");
        });
    } catch (e) {
        alert("您的浏览器可能为安装flash");
    }
}