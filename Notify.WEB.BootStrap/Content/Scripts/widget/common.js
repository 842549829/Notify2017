
(function (window) {
    // 1.将未带校验位的 15（或18）位卡号从右依次编号 1 到 15（18），位于奇数位号上的数字乘以 2。
    // 2.将奇位乘积的个十位全部相加，再加上所有偶数位上的数字。
    // 3.将加法和加上校验位能被 10 整除。
    window.luhmCheck = function (bankno) {
        var lastNum = bankno.substr(bankno.length - 1, 1);//取出最后一位（与luhm进行比较）

        var first15Num = bankno.substr(0, bankno.length - 1);//前15或18位
        var newArr = new Array();
        for (var i = first15Num.length - 1; i > -1; i--) {    //前15或18位倒序存进数组
            newArr.push(first15Num.substr(i, 1));
        }
        var arrJiShu = new Array();  //奇数位*2的积 <9
        var arrJiShu2 = new Array(); //奇数位*2的积 >9

        var arrOuShu = new Array();  //偶数位数组
        for (var j = 0; j < newArr.length; j++) {
            if ((j + 1) % 2 === 1) {//奇数位
                if (parseInt(newArr[j]) * 2 < 9)
                    arrJiShu.push(parseInt(newArr[j]) * 2);
                else
                    arrJiShu2.push(parseInt(newArr[j]) * 2);
            }
            else //偶数位
                arrOuShu.push(newArr[j]);
        }

        var jishuChild1 = new Array();//奇数位*2 >9 的分割之后的数组个位数
        var jishuChild2 = new Array();//奇数位*2 >9 的分割之后的数组十位数
        for (var h = 0; h < arrJiShu2.length; h++) {
            jishuChild1.push(parseInt(arrJiShu2[h]) % 10);
            jishuChild2.push(parseInt(arrJiShu2[h]) / 10);
        }

        var sumJiShu = 0; //奇数位*2 < 9 的数组之和
        var sumOuShu = 0; //偶数位数组之和
        var sumJiShuChild1 = 0; //奇数位*2 >9 的分割之后的数组个位数之和
        var sumJiShuChild2 = 0; //奇数位*2 >9 的分割之后的数组十位数之和
        var sumTotal = 0;
        for (var m = 0; m < arrJiShu.length; m++) {
            sumJiShu = sumJiShu + parseInt(arrJiShu[m]);
        }

        for (var n = 0; n < arrOuShu.length; n++) {
            sumOuShu = sumOuShu + parseInt(arrOuShu[n]);
        }

        for (var p = 0; p < jishuChild1.length; p++) {
            sumJiShuChild1 = sumJiShuChild1 + parseInt(jishuChild1[p]);
            sumJiShuChild2 = sumJiShuChild2 + parseInt(jishuChild2[p]);
        }
        //计算总和
        sumTotal = parseInt(sumJiShu) + parseInt(sumOuShu) + parseInt(sumJiShuChild1) + parseInt(sumJiShuChild2);

        //计算Luhm值
        var k = parseInt(sumTotal) % 10 === 0 ? 10 : parseInt(sumTotal) % 10;
        var luhm = 10 - k;

        if (lastNum === luhm) {
            //alert("Luhm验证通过");
            return true;
        }
        else {
            //alert("银行卡号必须符合Luhm校验");
            return false;
        }
    };
    //指定路径
    window.setCookie = function (name, value, hour, path) {
        var cookie = name + "=" + encodeURIComponent(value) + (hour ? "; expires=" + new Date(new Date().getTime() + hour * 60 * 60 * 1000).toGMTString() : "") + ";";
        if (path) {
            cookie += "path=" + path;
        }
        document.cookie = cookie;
    };
    /*当前路径写cookie*/
    window.setCookieCurrentPath = function (name, value, hour) {
        window.setCookie(name, value, hour);
    };
    /*当前路径按组写cookie*/
    window.setCookiesCurrentPath = function (name, value, hour) {
        var cookie = name + "=" + encodeURIComponent(name + "=" + value)
            + (hour ? "; expires=" + new Date(new Date().getTime() + hour * 60 * 60 * 1000).toGMTString() : "") + ";";
        document.cookie = cookie;
    };
    //读取cookie
    window.getCookie = function (name) {
        var re = new RegExp("(^|;)\\s*(" + name + ")=([^;]*)(;|$)", "i");
        var res = re.exec(document.cookie);
        return res != null ? decodeURIComponent(res[3]) : "";
    };
    /*按组读取cookie*/
    window.getCookies = function (name) {
        var result = {};
        var cookies = getCookie(name);
        var itemsCookies = cookies.split("&");
        for (var item in itemsCookies) {
            if (itemsCookies.hasOwnProperty(item)) {
                var items = itemsCookies[item].split("=");
                if (items.length < 2) continue;
                result[items[0]] = items[1];
            }
        }
        return result;
    };

    window.checkIdcard = function (idcard) {
        var errors = new Array(
        "yes",
        "请检查输入的证件号码是否正确", //"身份证号码位数不对!",
        "请检查输入的证件号码是否正确", //"身份证号码出生日期超出范围或含有非法字符!",
        "请检查输入的证件号码是否正确", //"身份证号码校验错误!",
        "请检查输入的证件号码是否正确" //"身份证地区非法!"
        );

        var area = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }
        var y, jym;
        var s, m;
        var idcardArray = new Array();
        idcard = idcard.replace(/(^\s*)|(\s*$)/g, "");

        idcardArray = idcard.split("");
        //地区检验 
        if (area[parseInt(idcard.substr(0, 2))] == null) return errors[4];
        //身份号码位数及格式检验 
        var ereg;
        switch (idcard.length) {
            case 15:
                if ((parseInt(idcard.substr(6, 2)) + 1900) % 4 === 0 || ((parseInt(idcard.substr(6, 2)) + 1900) % 100 === 0 && (parseInt(idcard.substr(6, 2)) + 1900) % 4 === 0)) {
                    ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$/; //测试出生日期的合法性 
                } else {
                    ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$/; //测试出生日期的合法性 
                }
                if (ereg.test(idcard)) return errors[0];
                else return errors[2];
            case 18:
                //18位身份号码检测 
                //出生日期的合法性检查  
                //闰年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9])) 
                //平年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8])) 
                if (parseInt(idcard.substr(6, 4)) % 4 === 0 || (parseInt(idcard.substr(6, 4)) % 100 === 0 && parseInt(idcard.substr(6, 4)) % 4 === 0)) {
                    ereg = /^[1-9][0-9]{5}(19|20)[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/; //闰年出生日期的合法性正则表达式 
                } else {
                    ereg = /^[1-9][0-9]{5}(19|20)[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/; //平年出生日期的合法性正则表达式 
                }
                if (ereg.test(idcard)) {//测试出生日期的合法性 
                    //计算校验位 
                    s = (parseInt(idcardArray[0]) + parseInt(idcardArray[10])) * 7
                    + (parseInt(idcardArray[1]) + parseInt(idcardArray[11])) * 9
                    + (parseInt(idcardArray[2]) + parseInt(idcardArray[12])) * 10
                    + (parseInt(idcardArray[3]) + parseInt(idcardArray[13])) * 5
                    + (parseInt(idcardArray[4]) + parseInt(idcardArray[14])) * 8
                    + (parseInt(idcardArray[5]) + parseInt(idcardArray[15])) * 4
                    + (parseInt(idcardArray[6]) + parseInt(idcardArray[16])) * 2
                    + parseInt(idcardArray[7]) * 1
                    + parseInt(idcardArray[8]) * 6
                    + parseInt(idcardArray[9]) * 3;
                    y = s % 11;
                    m = "F";
                    jym = "10X98765432";
                    m = jym.substr(y, 1); //判断校验位
                    if (m === idcardArray[17]) return errors[0]; //检测ID的校验位 
                    else return errors[3];
                }
                else return errors[2];
            default:
                return errors[1];
        }
    }
    //获取URL参数
    window.getRequest = function (url) {
        try {
            var theRequest = new Array();
            if (url.indexOf("?") > -1) {
                var pair = url.substr(1).split("&");
                for (var i = 0; i < pair.length; i++) {
                    //var key = pair[i].split("=")[0];
                    var value = decodeURI(pair[i].split("=")[1]);
                    var item = { key: value }
                    theRequest.push(item);
                    //theRequest[pair[i].split("=")[0]] = decodeURI(pair[i].split("=")[1]);
                }
            }
            return theRequest;
        } catch (e) {
            return null;
        }
    };
})(window);

(function (window, date) {
    /*
     *参考地址
     *http://www.cnblogs.com/carekee/articles/1678041.html
    */
    //格式化时间
    date.prototype.Format = function (formatStr) {
        var str = formatStr;
        var week = ["日", "一", "二", "三", "四", "五", "六"];

        str = str.replace(/yyyy|YYYY/, this.getFullYear());
        str = str.replace(/yy|YY/, (this.getYear() % 100) > 9 ? (this.getYear() % 100).toString() : "0" + (this.getYear() % 100));

        str = str.replace(/MM/, this.getMonth() > 9 ? this.getMonth().toString() : "0" + this.getMonth());
        str = str.replace(/M/g, this.getMonth());

        str = str.replace(/w|W/g, week[this.getDay()]);

        str = str.replace(/dd|DD/, this.getDate() > 9 ? this.getDate().toString() : "0" + this.getDate());
        str = str.replace(/d|D/g, this.getDate());

        str = str.replace(/hh|HH/, this.getHours() > 9 ? this.getHours().toString() : "0" + this.getHours());
        str = str.replace(/h|H/g, this.getHours());
        str = str.replace(/mm/, this.getMinutes() > 9 ? this.getMinutes().toString() : "0" + this.getMinutes());
        str = str.replace(/m/g, this.getMinutes());

        str = str.replace(/ss|SS/, this.getSeconds() > 9 ? this.getSeconds().toString() : "0" + this.getSeconds());
        str = str.replace(/s|S/g, this.getSeconds());

        return str;
    };
    //合并对象
    var extend = function (des, src, override) {
        if (src instanceof Array) {
            for (var i = 0, len = src.length; i < len; i++) {
                extend(des, src[i], override);
            }
        }
        for (var item in src) {
            if (src.hasOwnProperty(item)) {
                if (override || !(item in des)) {
                    des[item] = src[item];
                }
            }
        }
        return des;
    };
    /*获取n天的时间区间*/
    window.getRanage = function (o) {
        var obj = {
            count: 0,
            format: "yyyy-MM-dd"
        };
        var current = extend({}, [o, obj]);
        var nowDate = new Date();
        var targetDate = new Date();
        targetDate.setDate(targetDate.getDate() + current.count);
        var one = targetDate.getFullYear() + "-" + (targetDate.getMonth() + 1) + "-" + targetDate.getDate();
        var two = nowDate.getFullYear() + "-" + (nowDate.getMonth() + 1) + "-" + nowDate.getDate();
        var result = {};
        if (current.count < 0) {
            result.lower = one;
            result.upper = two;
        } else {
            result.lower = two;
            result.upper = one;
        }
        return result;
    };
    /*获取本月的开头结尾*/
    function getMonthDays(myMonth) {
        var now = new Date(); //当前日期 
        //var nowDayOfWeek = now.getDay(); //今天本周的第几天 
        //var nowDay = now.getDate(); //当前日 
        //var nowMonth = now.getMonth(); //当前月 
        var nowYear = now.getYear(); //当前年 
        nowYear += (nowYear < 2000) ? 1900 : 0;
        var monthStartDate = new Date(nowYear, myMonth, 1);
        var monthEndDate = new Date(nowYear, myMonth + 1, 1);
        var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
        return days;
    }

    function formatDate(dateTime) {
        var myyear = dateTime.getFullYear();
        var mymonth = dateTime.getMonth() + 1;
        var myweekday = dateTime.getDate();
        if (mymonth < 10) {
            mymonth = "0" + mymonth;
        }
        if (myweekday < 10) {
            myweekday = "0" + myweekday;
        }
        return (myyear + "-" + mymonth + "-" + myweekday);
    }

    window.getMonthDate = function () {
        var now = new Date(); //当前日期 
        //var nowDayOfWeek = now.getDay(); //今天本周的第几天 
        //var nowDay = now.getDate(); //当前日 
        var nowMonth = now.getMonth(); //当前月 
        var nowYear = now.getYear(); //当前年 
        nowYear += (nowYear < 2000) ? 1900 : 0;
        var monthStartDate = new Date(nowYear, nowMonth, 1);
        var monthEndDate = new Date(nowYear, nowMonth, getMonthDays(nowMonth));
        return { lower: formatDate(monthStartDate), upper: formatDate(monthEndDate) };
    };
    //格局化日期：yyyy-MM-dd 
    //获得某月的天数 
    //+---------------------------------------------------  
    //| 求两个时间的天数差 日期格式为 YYYY-MM-dd   
    //+---------------------------------------------------  
    window.daysBetween = function (dateOne, dateTwo) {
        var oneMonth = dateOne.substring(5, dateOne.lastIndexOf("-"));
        var oneDay = dateOne.substring(dateOne.length, dateOne.lastIndexOf("-") + 1);
        var oneYear = dateOne.substring(0, dateOne.indexOf("-"));

        var twoMonth = dateTwo.substring(5, dateTwo.lastIndexOf("-"));
        var twoDay = dateTwo.substring(dateTwo.length, dateTwo.lastIndexOf("-") + 1);
        var twoYear = dateTwo.substring(0, dateTwo.indexOf("-"));

        var cha = ((Date.parse(oneMonth + "/" + oneDay + "/" + oneYear) - Date.parse(twoMonth + "/" + twoDay + "/" + twoYear)) / 86400000);
        return Math.abs(cha);
    };
})(window, Date);

(function ($) {
    $.extend({
        qqEmail: function (id) {
            var targetId = $("#" + id + "");
            if (!targetId) return false;
            var $this = $(this);
            $this.blur(function () {
                var value = $.trim($(this).val());
                var regqq = /^\d{4,13}$/;
                if (regqq.test(value)) {
                    targetId.val(value + "@qq.com");
                }
            });
            return false;
        },
        onLoadRemind: function (o) {
            var obj = $.extend(true,
                {
                    message: "",//提醒消息 not null
                    submit: "", //操作按钮 null
                    color: "#666666"   //字体颜色
                }, o);
            var $this = $(this);
            var color = $this.css("color");
            if ($this.val() === "") {
                $this.val(obj.message);
                $this.css("color", obj.color);
            }
            $this.focus(function () {
                if (this.value === obj.message) {
                    this.value = "";
                    this.style.color = color;
                }
            }).blur(function () {
                if (this.value === "") {
                    this.value = obj.message;
                    this.style.color = obj.color;
                }
            });
            if (obj.submit !== "") {
                $("#" + obj.submit).click(function () {
                    if ($this.val() === obj.message) {
                        $this.val("");
                    }
                });
            }
        },
        tabControl: function (o) {
            var obj = $.extend(true, {
                className: "curr",       //当前选中class
                isRememberState: false,  //是否记住状态
                isUnload: false,         //页面离开是否删除cookie
                cookieName: "OptionsId", //记住状态Id
                currId: null,            //默认选中Id
                event: "click",
                cookieHour: 1,
                callback: null           //回调函数  
            }, o);
            var $this = $(this);

            function setCurrentClassName(self) {
                $this.each(function () {
                    if ($(this).hasClass(obj.className)) {
                        $(this).removeClass(obj.className);
                    }
                });
                self.addClass(obj.className);
            }

            function setHidden() {
                $this.each(function () {
                    var cls = $(this).attr("id");
                    if (cls) {
                        $("." + cls + "").hide();
                    }
                });
            }

            function setDisplay(self) {
                var selfId = self.attr("id");
                if (selfId) {
                    $("." + selfId + "").show();
                }
            }

            function setCookie(self) {
                if (obj.isRememberState) {
                    window.setCookieCurrentPath(obj.cookieName, self.attr("id"), obj.cookieHour);
                }
            }

            function optionEvent(self) {
                setCurrentClassName(self);
                setHidden();
                setDisplay(self);
                setCookie(self);
                if (obj.callback) {
                    obj.callback(self);
                }
            }

            function currSelected(currentId) {
                var selectedId = $("#" + currentId + "");
                if (selectedId) {
                    optionEvent(selectedId);
                }
            }

            if (obj.isRememberState) {
                var optionsId = getCookie(obj.cookieName);
                if (optionsId) {
                    currSelected(optionsId);
                }
            }
            if (obj.currId) {
                currSelected(obj.currId);
            }
            $this.each(function () {
                $(this).on(obj.event, function () {
                    optionEvent($(this));
                });
                //switch (obj.event) {
                //    case "hover":
                //        {
                //            $(this).hover(function () {
                //                optionEvent($(this));
                //            });
                //            break;
                //        }
                //    default:
                //        {
                //            $(this).click(function () {
                //                optionEvent($(this));
                //            });
                //        }
                //        break;
                //}
            });
            if (obj.isUnload) {
                window.onunload = function () {
                    window.setCookieCurrentPath(obj.cookieName, "", obj.cookieHour);
                };
            }
        },
        ajaxExtend: function (o) {
            var obj = $.extend(true, {
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "JSON",
                timeout: 50000,
                error: function (e) {
                    var rel;
                    try {
                        rel = JSON.parse(e.responseText);
                    } catch (ex) {
                        if (e.statusText === "timeout") {
                            alert("服务器忙");
                        } else if (e) {
                            alert(e.responseText);
                        }
                        return;
                    }
                    if (e.responseText === "") return;
                    if (e.status === 300) {
                        if (rel.Type === "RequireLogon") {
                            if (window.top) {
                                window.top.location.href = rel.Result;
                            } else {
                                window.location.href = rel.Result;
                            }
                            return;
                        } else if (rel.Type === "Unauthorized") {
                            alert("无权访问");
                            //window.location.href = "";
                            return;
                        } else if (rel.Type === "ExceptionPermission") {

                            if (window.top) {
                                window.top.location.href = "/Home/ExceptionPermission";
                            } else {
                                window.location.href = "/Home/ExceptionPermission";
                            }
                        }
                    } else if (e.status === 401 && e.statusText === "Unauthorized") {
                        alert("无权访问");
                        //window.location.href = "/Logon.aspx";
                        return;
                    }
                }
            }, o);
            $.ajax(obj);
        },
        //loadPageStates: function (url) {
        //    //var obj = window.getRequest(url);
        //    var $this = $(this);
        //    $this.find("input").each(function () {
        //        if ($(this).is(":text")) {
        //        }
        //    });
        //},
        layerAlert: function (msg, o, ok) {
            //skin样式类名
            var obj = $.extend(true, {
                closeBtn: 1,
                shift: 4,
                shade: [0.1, "#000000"]
            }, o);
            var index = window.layer.alert(msg, obj, function () {
                window.layer.close(index);
                if (ok) {
                    ok();
                }
            });
        },
        //msg消息,id吸附层Id 如: #id
        //类型：Number/Array，默认：2
        //tips层的私有参数。支持上右下左四个方向，通过1-4进行方向设定。如tips: 3则表示在元素的下面出现。有时你还可能会定义一些颜色，可以设定tips: [1, '#c00']
        layerTips: function (msg, id) {
            window.layer.tips(msg, id, {
                tips: [2, "#95b8e7"]
            });
        },
        layerConfirm: function (content, options, yes, no) {
            content = content || "你确定要删除吗";
            options = options || { icon: 3, title: "提示" }
            window.layer.confirm(content, options, function (index) {
                window.layer.close(index);
                if (yes) {
                    yes();
                }
            }, function (index) {
                window.layer.close(index);
                if (no) {
                    no();
                }
            });
        },
        // 弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
        esayuiAlert: function (msgString, title, msgType) {
            title = title || "系统提示";
            msgType = msgType || "info";
            $.messager.alert(title, msgString, msgType);
        },
        showDiv: function (mask, content) {
            document.getElementById(content).style.display = "block";
            document.getElementById(mask).style.display = "block";
            var bgdiv = document.getElementById(mask);
            bgdiv.style.width = document.body.scrollWidth;
            $("#" + mask).height($(document).height());
        },
        closeDiv: function (mask, content) {
            document.getElementById(mask).style.display = "none";
            document.getElementById(content).style.display = "none";
        },
        ajaxLoginExtend: function (e) {
            var rel;
            try {
                rel = JSON.parse(e.responseText);
            } catch (ex) {
                if (e.statusText === "timeout") {
                    alert("服务器忙");
                } else if (e) {
                    alert(e.responseText);
                }
                return;
            }
            if (e.responseText === "") return;
            if (e.status === 300) {
                if (rel.Type === "RequireLogon") {
                    window.top.location.href = rel.Result;
                    return;
                } else if (rel.Type === "Unauthorized") {
                    return;
                }
            } else if (e.status === 401 && e.statusText === "Unauthorized") {
                return;
            }
        }
    });

    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
})(jQuery);