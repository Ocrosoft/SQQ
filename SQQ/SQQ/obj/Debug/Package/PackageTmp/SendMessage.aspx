<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMessage.aspx.cs" Inherits="SQQ.SendMessage" %>

<!DOCTYPE html>
<html lang="zh-cn">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-control" content="no-cache">
    <meta http-equiv="Cache" content="no-cache">
    <title>发送消息</title>
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
                        <td width='20%'>姓名</td>
                        <td width='20%'>open_id</td>
                        <td width='40%'>消息内容</td>
                        <td width="20%">发送</td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <form method="post" action="ajax/sendMessage.ashx">
                            <td id="td_name" align="center"></td>
                            <td>
                                <input name="oid" class="form-control" type="text">
                            </td>
                            <td id="message">
                                <input name="msg" class="form-control" type="text">
                            </td>
                            <td>
                                <input type="submit" value="发送" class="form-control">
                            </td>
                        </form>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <script src="js/jquery.min.js"></script>
    <script>
        $('[name=oid]').blur(function () {
            var oid = $('[name=oid]').val();
            if (oid == '-1') {
                $('#td_name').text('所有人');
            } else {
                $.ajax(
                    {
                        url: 'ajax/senders.ashx',
                        success: function (result) {
                            $(result).each(function (index, item) {
                                if (item.open_id == oid) {
                                    $('#td_name').text(item.name);
                                }
                            });
                        }
                    });
            }
        });
</script>
</body>
</html>
<!--not cached-->
