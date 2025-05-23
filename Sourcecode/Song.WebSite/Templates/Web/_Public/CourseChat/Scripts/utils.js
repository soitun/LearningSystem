﻿/*! 工具类 */


/**统计localStorage容量 */
(function () {
    if (!window.localStorage) {
        console.log('浏览器不支持localStorage');
    }
    var size = 0;
    for (item in window.localStorage) {
        if (window.localStorage.hasOwnProperty(item)) {
            size += window.localStorage.getItem(item).length;
        }
    }
    console.log('当前localStorage容量为' + (size / 1024).toFixed(2) + 'KB');

})();
/** 时间相关的方法 */
//日期格式化
Date.prototype.format = function (fmt) {

    var fmtfuc = function (fmt, date) {
        fmt = fmt.replace(/\Y/g, "y");
        //24小时制
        var h24 = date.toLocaleString();
        try {
            h24 = date.toLocaleString('chinese', {
                hour12: false
            });
        } catch (e) { }
        h24 = h24.substring(h24.indexOf(' ') + 1, h24.indexOf(':'));
        h24 = h24 == '24' ? '0' : h24;
        //12小时制
        var h12 = date.toLocaleString();
        try {
            h12 = date.toLocaleString('chinese', { hour12: true });
        } catch (e) { }
        h12 = h12.substring(h12.indexOf(' ') + 1, h12.indexOf(':'));
        //星期
        var week = ['天', '一', '二', '三', '四', '五', '六'];
        //
        var ret;
        var opt = {
            "yyyy": date.getFullYear().toString(), // 年
            "yy": date.getFullYear().toString().substring(2),
            "M+": (date.getMonth() + 1).toString(), // 月
            "d+": date.getDate().toString(), // 日
            "w+": week[date.getDay()], // 星期
            "H+": h24, //小时
            "h+": h12,
            "m+": date.getMinutes().toString(), // 分
            "s+": date.getSeconds().toString() // 秒			
        };
        for (var k in opt) {
            ret = new RegExp("(" + k + ")").exec(fmt);
            if (ret) {
                fmt = fmt.replace(ret[1], (ret[1].length == 1) ? (opt[k]) : (opt[k].padStart(ret[1].length, "0")))
            };
        };
        return fmt;
    }
    return fmtfuc(fmt, this);
};

//日期字符串解析为日期对象
Date.parse = function (str) {
    if (JSON.stringify(str) == '{}') return new Date();
    //如果是数值
    if (typeof (str) == 'number') return new Date(str);
    //如果本身就是日期对象
    if (str instanceof Date) return str;
    var date = '', time = '';
    str = str.replace(/\//g, "-");
    if (str.indexOf(' ') > -1) {
        date = str.substring(0, str.indexOf(' '));
        time = str.substring(str.lastIndexOf(' ') + 1);
    } else {
        if (str.indexOf('-') > -1) date = str;
        if (str.indexOf(':') > -1) {
            date = new Date().format('yyyy-MM-dd');
            time = str;
        }
    }
    var dateStrs = date.split('-');
    var year = parseInt(dateStrs[0], 10);
    var month = parseInt(dateStrs[1], 10) - 1;
    var day = parseInt(dateStrs[2], 10);
    var timeStrs = time.split(':');
    var hour = parseInt(timeStrs[0], 10);
    var minute = parseInt(timeStrs[1], 10);
    var second = parseInt(timeStrs[2], 10);
    second = isNaN(second) ? 0 : second;
    return new Date(year, month, day, hour, minute, second);
}
//增加月份
Date.prototype.addmonth = function (n) {
    var dt = this;
    var yy = dt.getYear();
    var mm = dt.getMonth();
    dt.setMonth(dt.getMonth() + n);
    if ((dt.getYear() * 12 + dt.getMonth()) > (yy * 12 + mm + n)) {
        dt = new Date(dt.getYear(), dt.getMonth(), 0);
    }
    var year = dt.getYear();
    var month = dt.getMonth() + 1;
    var days = dt.getDate();
    var dd = year + "-" + month + "-" + days;
    return dd;
};
//时间值离当现时间有多久
Date.prototype.untilnow = function () {
    var date = this;
    const now = new Date();
    const diff = Math.abs(now.getTime() - date.getTime());
    const seconds = Math.floor(diff / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);
    const months = Math.floor(days / 30);
    const years = Math.floor(months / 12);
    if (years >= 100) return "多年前";
    else if (years > 0) return years + "年前";
    else if (months > 0) return months + "个月前";
    else if (days > 0) return days + "天前";
    else if (hours > 0) return hours + "小时前";
    else if (minutes > 0) return minutes + "分钟前";
    else return "刚刚";

}
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g, function (m, i) {
        return args[i];
    });
};
//将数值转金额的格式，即三位一个逗号
//len:小数字后的长度，默认是两位
Number.prototype.money = function (len) {
    var str = String(this);
    //小数部分
    var float_num = 0;
    if (len == null) len = 2;
    if (str.indexOf('.') > -1) {
        var f = str.substring(str.indexOf('.') + 1);
        var j = 0, s = '';
        while (len-- > 0) s += f.substring(j++, j);
        float_num = isNaN(Number(s)) ? 0 : Number(s);
    }
    str = str.indexOf('.') > -1 ? str.substring(0, str.indexOf('.')) : str;
    //整数部分，每三位加一个逗号
    var mstr = '', n = 0;
    for (var i = str.length - 1; i >= 0; i--) {
        mstr = str.substring(i, i + 1) + mstr;
        if (++n % 3 == 0 && n != str.length) mstr = ',' + mstr;
    }
    return float_num > 0 ? mstr + '.' + float_num : mstr;
};

/** 数学方法 */
Math.round2 = Math.round;
Math.round = function (num, decimal) {
    if (decimal == null) return Math.round2(num);
    return Math.round2(num * Math.pow(10, decimal)) / Math.pow(10, decimal);
}
Math.floor2 = Math.floor;
Math.floor = function (num, decimal) {
    if (decimal == null) return Math.floor2(num);
    return Math.floor2(num * Math.pow(10, decimal)) / Math.pow(10, decimal);
}
Math.ceil2 = Math.ceil;
Math.ceil = function (num, decimal) {
    if (decimal == null) return Math.ceil2(num);
    return Math.ceil2(num * Math.pow(10, decimal)) / Math.pow(10, decimal);
}
/** 提示框 

*/
//重构alert
window.alert_base = window.alert;
window.alert = function (txt, title) {
    txt = String(txt);
    var message = txt;
    //匹配标题
    try {
        const result = txt.match(/(?<=\().[^\)]+(?=\))/);
        if (result && (title == '' || title == null)) {
            title = result[0];
            message = txt.replace(/\(.[^\)]+\)/, '');
        }
    } catch (e) { }
    //手机端
    try {
        if ($dom.ismobi()) {
            if (vant && vant.Dialog) vant.Dialog.alert({ message: message, title: title });
            else window.alert_base((title ? title + ' ' : '') + txt);
        } else {
            if (Vue && Vue.prototype.$confirm) Vue.prototype.$alert(message, title);
            else window.alert_base((title ? title + ' ' : '') + txt);
        }
    } catch (e) {
        window.alert_base((title ? title + ' ' : '') + txt);
    }
};
//重构确认
window.confirm_base = window.confirm;
window.confirm = function (title, msg, evtConfirm, evtCancel) {
    try {
        //手机端
        if ($dom.ismobi()) {
            if (vant && vant.Dialog) {
                vant.Dialog.confirm({ title: title, message: msg, })
                    .then(evtConfirm != null ? evtConfirm : () => { })
                    .catch(evtCancel != null ? evtCancel : () => { });
            } else {
                return window.confirm_base(title);
            }
        } else {
            if (Vue && Vue.prototype.$confirm) {
                Vue.prototype.$confirm(msg, title, {
                    dangerouslyUseHTMLString: true, type: 'warning',
                    confirmButtonText: '确定', cancelButtonText: '取消'
                }).then(evtConfirm != null ? evtConfirm : () => { })
                    .catch(evtCancel != null ? evtCancel : () => { });
            } else {
                return window.confirm_base(title);
            }
        }
    } catch (e) {
        window.confirm_base((title ? title + ' ' : '') + msg);
    }
};

/** 表单验证方法 
 * 
 * 在ElementUi的表单中提供验证方法
 * 例如：  { validator: validate.name.proh, trigger: 'change' },   //禁止使用特殊字符
*/
(function () {
    var validate = {
        //名称的校验
        name: {
            //prohibition,禁止使用的字符
            proh: function (rule, value, callback) {
                const pattern = /[\<\>\"\'\&\=\\\/\(\)\:\*\?\|\%]+/;
                if (pattern.test(value)) {
                    callback(new Error('不能包含下列字符： <>\"&=\'\/():*?|%  '));
                } else {
                    callback();
                }
            },
            //危险的字符
            danger: function (rule, value, callback) {
                if (value == '' | value == null) return callback();
                const rex = 'and|exec|insert|select|delete|update|count|chr|mid|master|truncate|char|declare|script|frame|or|etc|style|expression';
                var arr = rex.split('|');
                for (var i = 0; i < arr.length; i++) {
                    if (value.indexOf(' ' + arr[i] + ' ') > -1)
                        return callback(new Error('存在危险字符： ' + arr[i]));
                }
                callback();
            }
        }
    }
    window.validate = validate;
})();

//console.error(validate);