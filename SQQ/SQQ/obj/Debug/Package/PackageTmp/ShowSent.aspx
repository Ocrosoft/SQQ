<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowSent.aspx.cs" Inherits="SQQ.ShowSent" %>

<!DOCTYPE html>
<html lang="zh-cn">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-control" content="no-cache">
    <meta http-equiv="Cache" content="no-cache">
    <title>气球配送记录</title>
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <link rel="stylesheet" href="css/bootstrap-theme.min.css">
    <link rel="stylesheet" href="css/local.css">
    <!--[if lt IE 9]>
      <script src="//cdn.bootcss.com/html5shiv/3.7.0/html5shiv.js"></script>
      <script src="//cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <div class="container">
        <div class="jumbotron">
            <table id='problemset' class='table table-striped' width='90%'>
                <thead>
                    <tr align="center" class='toprow'>
                        <td width='15%'>姓名</td>
                        <td width='25%'>队伍 - 题目 - 颜色</td>
                        <td width="20%">分配时间</td>
                        <td width='20%'>派送时间</td>
                        <td width="20%">操作</td>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>
    </div>
    <!-- jQuery文件。务必在bootstrap.min.js 之前引入 -->
    <script src="js/jquery.min.js"></script>
    <!-- 最新的 Bootstrap 核心 JavaScript 文件 -->
    <script src="js/bootstrap.min.js"></script>
    <script>
        function refresh() {
            $.ajax({
                url: 'ajax/sents.ashx', success: function (response) {
                    var tb = $('tbody');
                    tb.empty();
                    $.each(response, function (index, item) {
                        var tr = $('<tr></tr>').addClass(index % 2 ? 'evenrow' : 'oddrow').attr('align', 'center');
                        tr.append($('<td>' + item.sender + '</td>'));
                        tr.append($('<td>' + item.team_id + ' - ' + String.fromCharCode(parseInt(item.num) + 'A'.charCodeAt(0)) + ' - ' + item.color + '</td>'));
                        tr.append('<td>' + item.timeStart.toString().replace('T', ' ') + '</td>');
                        tr.append($('<td>' + item.timeSent.toString().replace('T', ' ') + '</td>'));
                        tr.append($('<td><input type="button" value="重派" class="resend form-control" /></td>'));
                        tr.find('input').attr('data-detail', item.team_id + '#' + item.num);
                        tb.append(tr);
                    });

                    $('[data-detail]').bind('click', function () {
                        if (confirm('是否重新配送该题？')) {
                            $.ajax({
                                url: 'ajax/resendTask.ashx',
                                type: 'post',
                                data: { 'team_id': $(this).attr('data-detail').split('#')[0], 'num': $(this).attr('data-detail').split('#')[1] },
                                success: function (result) {
                                    if (result.code == "ok") {
                                        alert('已加入重新配送列表。');
                                        refresh();
                                    } else {
                                        alert('重新配送失败，检查日志。');
                                    }
                                }
                            });
                        }
                    });
                }
            });
        }
        setTimeout(refresh, 10000);
        $(document).ready(refresh);
    </script>
</body>
</html>
<!--not cached-->
