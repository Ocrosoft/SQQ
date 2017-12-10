<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowSender.aspx.cs" Inherits="SQQ.ShowSender" %>

<!DOCTYPE html>
<html lang="zh-cn">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-control" content="no-cache">
    <meta http-equiv="Cache" content="no-cache">
    <title>送气球人员</title>
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
                        <td width='10%'>姓名</td>
                        <td width='60%'>open_id</td>
                        <td width='10%'>待送</td>
                        <td width='10%'>已送</td>
                        <td width='10%'>操作</td>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>
    </div>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script>
        setTimeout(function () {
            $.ajax({
                url: 'ajax/senders.ashx', success: function (response) {
                    var tb = $('tbody');
                    tb.empty();
                    $.each(response, function (index, item) {
                        var tr = $('<tr></tr>').addClass(index % 2 ? 'evenrow' : 'oddrow').attr('align', 'center');
                        tr.append($('<td>' + item.name + '</td>'));
                        tr.append($('<td>' + item.open_id + '</td>'));
                        tr.append($('<td>' + item.sending + '</td>'));
                        tr.append($('<td>' + item.sent + '</td>'));
                        tr.append($('<td><a href="javascript:;" data-oid="' + item.open_id + '" class="delete">开除</a></td>'));
                        tb.append(tr);
                    });
                    $('.delete').bind('click', function () {
                        if (confirm('确定删除？在送任务将重新分配，已送任务保留但不记录配送人员。')) {
                            $.ajax({
                                url: 'ajax/deleteSender.ashx?oid=' + $(this).attr('data-oid'),
                                success: function (result) {
                                    if (result.code == 'error') {
                                        alert('删除失败，详情查看日志。');
                                    } else {
                                        alert('删除成功。');
                                        location.href = location.href;
                                    }
                                }
                            })
                        }
                    });
                }
            });
        });
    </script>
</body>
</html>
<!--not cached-->