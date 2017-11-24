﻿
//时间戳转换时间
function getDate(dateStr) {
    var date = "";
    if (dateStr != "") {
        var ns = dateStr.split("(")[1].split(")")[0];
        var timestamp = new Date(parseInt(ns));
        //date = timestamp.toLocaleString().replace(/\//g, "-");
        //date = timestamp.toLocaleString().substr(0, 17);
        var y = timestamp.getFullYear();
        var m = timestamp.getMonth() + 1;
        var d = timestamp.getDate();
        date = y + "-" + (m < 10 ? "0" + m : m) + "-" + (d < 10 ? "0" + d : d) + " " + timestamp.toTimeString().substr(0, 8);
    }
    return date;
}

//用户状态
function getStatus(value) {
    switch (value) {
        case 1:
            return "有效";
        case 2:
            return "无效";
        default:
            return value;
    }
}

//角色类型
function getRole(value) {
    switch (value) {
        case 1:
            return "管理员";
        case 2:
            return "用户";
        default:
            return value;
    }
}