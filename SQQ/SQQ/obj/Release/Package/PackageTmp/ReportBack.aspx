<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportBack.aspx.cs" Inherits="SQQ.ReportBack" %>

<!DOCTYPE html>
<html lang="zh-cn">
<head>
    <meta name="viewport" content="width=device-width,initial-scale=1,user-scalable=0">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-control" content="no-cache">
    <meta http-equiv="Cache" content="no-cache">
    <title>反馈</title>
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <link rel="stylesheet" href="css/bootstrap-theme.min.css">
    <link rel="stylesheet" href="css/local.css">
    <link rel="stylesheet" href="css/weui.min.css" />
    <!--[if lt IE 9]>
      <script src="//cdn.bootcss.com/html5shiv/3.7.0/html5shiv.js"></script>
      <script src="//cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <!-- 提示 -->
    <div class="js_dialog" id="completeDialog" style="display: none;">
        <div class="weui-mask"></div>
        <div class="weui-dialog">
            <div class="weui-dialog__hd"><strong class="weui-dialog__title">完成确认</strong></div>
            <div class="weui-dialog__bd">
                确认完成此次送气球任务吗？
            </div>
            <div class="weui-dialog__ft">
                <a id="dialog_cancel" class="weui-dialog__btn weui-dialog__btn_default">取消</a>
                <a id="dialog_ok" class="weui-dialog__btn weui-dialog__btn_primary">确定</a>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="jumbotron">
            <table id='problemset' class='table table-striped' width='90%'>
                <thead>
                    <tr align="center" class='toprow'>
                        <td width='20%'>姓名</td>
                        <td width='50%'>队伍ID - 题目序号</td>
                        <td width='30%'>操作</td>
                    </tr>
                </thead>
                <tbody runat="server" id="tbody"></tbody>
            </table>
        </div>
    </div>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script>
        var oid = location.href.split('?oid=')[1];
        var refreshInterval;

        function bindClick(ele) {
            ele.click(function () {
                var self = $(this);
                $('#completeDialog').fadeIn(200);
                $('#dialog_ok').attr('data-detail', self.attr('data-detail'));
            });
        }

        $('#dialog_cancel').bind('click', function () {
            $('#completeDialog').fadeOut(200);
        });

        $('#dialog_ok').bind('click', function () {
            var self = $(this);
            $('#completeDialog').fadeOut(200);
            $.ajax({
                url: '/ajax/confirmTask.ashx',
                type: 'POST',
                data: {
                    'tid': self.attr('data-detail').split('#')[0],
                    'num': self.attr('data-detail').split('#')[1],
                    'oid': oid
                },
                success: function (result) {
                    if (result.code == 'ok') {
                        $('input[data-detail=' + self.attr('data-detail') + ']').parent().html(result.info);
                    } else {
                        alert('提交任务失败，请联系管理员。');
                    }
                }
            })
        });

        function refresh() {
            $.ajax({
                url: '/ajax/newTask.ashx?oid=' + oid,
                success: function (result) {
                    var reservedCount = 0; // 回收的问题计数
                    var toDataDetail = []; // 与新问题列表等价的data-detail列表
                    $.each(result.problems, function (index, value) {
                        toDataDetail.push(value.team_id + '#' + value.num);
                    });
                    $('input[data-detail]').each(function (index, value) {
                        // 有完成按钮，但在送列表中不存在，说明被回收
                        if (toDataDetail.indexOf($(value).attr('data-detail')) == -1) {
                            reservedCount++;
                            $(value).parent().html('超时');
                        }
                    });
                    if (reservedCount != 0) {
                        alert('有' + reservedCount + "个任务超时，已被回收。（可能误判，刷新页面即可）");
                    }
                    $.each(result.problems, function (index, value) {
                        var dataDetail = value.team_id + '#' + value.num;
                        // 寻找存在的行
                        var exitRow = $('[data-detail=' + dataDetail + ']');
                        // 有已存在的行
                        if (exitRow.length > 0) {
                            return;
                        }
                        var tableRows = $('tbody > tr');
                        var firstRow = $(tableRows[0]);
                        $('tbody').prepend('<tr align="center"></tr>');
                        var newFirstRow = $($('tbody > tr')[0]);
                        if (firstRow.hasClass('evenrow')) {
                            newFirstRow.addClass('oddrow');
                        } else {
                            newFirstRow.addClass('evenrow');
                        }
                        newFirstRow.append('<td>' + result.name + '</td>');
                        newFirstRow.append('<td>' + value.team_id + ' - ' + String.fromCharCode(parseInt(value.num) + 'A'.charCodeAt(0)) + '</td>');
                        newFirstRow.append('<td><input type="button" class="form-control" value="完成" data-detail="' + dataDetail + '"></td>');
                        bindClick($('[data-detail=' + dataDetail + ']'));
                    });
                }
            });
        }

        $(document).ready(function () {
            bindClick($('[type=button]'));
        });

        refreshInterval = setInterval(function () {
            refresh();
        }, 10000);

    </script>
</body>
</html>
<!--not cached-->
