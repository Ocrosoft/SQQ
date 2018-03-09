<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetFloor.aspx.cs" Inherits="SQQ.SetFloor" %>

<!DOCTYPE html>
<html lang="zh-cn">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-control" content="no-cache">
    <meta http-equiv="Cache" content="no-cache">
    <title>添加考场</title>
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
                        <td width='40%'>队伍ID(支持正则)</td>
                        <td width='40%'>所在考场(楼层)</td>
                        <td width="20%">操作</td>
                    </tr>
                </thead>
                <tbody>
                    <tr align="center">
                        <td>
                            <input name="teamId" class="form-control" type="text">
                        </td>
                        <td id="td_floor">
                            <input name="floor" class="form-control" type="text">
                        </td>
                        <td>
                            <input type="submit" value="添加" class="form-control addFloor">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script>
        function addClick() {
            var team_id = $(this).parent().parent().find('input[name="teamId"]').val();
            var floor = $(this).parent().parent().find('input[name="floor"]').val();
            if (!team_id) {
                team_id = $(this).parent().parent().find('td')[0].innerText;
                floor = $(this).parent().parent().find('td')[1].innerText;
            }
            if ($(this).val() == '删除') {
                $.post("ajax/changeFloor.ashx", { "teamId": team_id, "floor": floor, "op": 1 }, function (result) {
                    location.href = location.href;
                });
                return;
            }
            $.post("ajax/changeFloor.ashx", { "teamId": team_id, "floor": floor, "op": 0 }, function (result) {
                if (result.code == "ok") {
                    var clone = $('tbody').children().last().clone();
                    clone.find('input').val('');
                    var input = $('tbody').children().last().find('input[name="teamId"]');
                    input.replaceWith(input.val());
                    input = $('tbody').children().last().find('input[name="floor"]');
                    input.replaceWith(input.val());
                    input = $('tbody').children().last().find('input[type="submit"]');
                    input.removeClass("addFloor").addClass("removeFloor").val('删除');
                    clone.addClass($('tbody').children().length % 2 ? 'evenrow' : 'oddrow');
                    clone.find('input[type="submit"]').val('添加').bind('click', addClick);
                    $('tbody').append(clone);
                }
            });
        }

        $.ajax({
            url: 'ajax/getFloor.ashx', success: function (response) {
                var tb = $('tbody');
                var clone = tb.children().first().clone();
                tb.empty();
                $.each(response, function (index, item) {
                    var tr = $('<tr></tr>').addClass(index % 2 ? 'evenrow' : 'oddrow').attr('align', 'center');
                    tr.append($('<td>' + item.teamId + '</td>'));
                    tr.append($('<td>' + item.floor + '</td>'));
                    tr.append($('<td><input type="submit" value="删除" class="form-control addFloor"></td>'));
                    tb.append(tr);
                });
                tb.append(clone);
                $('.addFloor').bind('click', addClick);
            }
        });
    </script>
</body>
</html>
<!--not cached-->
