<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetColor.aspx.cs" Inherits="SQQ.SetColor" %>

<!DOCTYPE html>
<html lang="zh-cn">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-control" content="no-cache">
    <meta http-equiv="Cache" content="no-cache">
    <title>气球颜色配置</title>
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
                        <td width='40%'>题目ID</td>
                        <td width='40%'>气球颜色</td>
                        <td width="20%">操作</td>
                    </tr>
                </thead>
                <tbody runat="server" id="tbody">
                    <tr align="center">
                        <td>
                            <input name="num" class="form-control" type="text">
                        </td>
                        <td id="td_floor">
                            <input name="color" class="form-control" type="text">
                        </td>
                        <td>
                            <input type="submit" value="添加" class="form-control addColor">
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
            var num = $(this).parent().parent().find('input[name="num"]').val();
            var color = $(this).parent().parent().find('input[name="color"]').val();
            if (!num) {
                num = $(this).parent().parent().find('td')[0].innerText;
                color = $(this).parent().parent().find('td')[1].innerText;
            }
            if ($(this).val() == '删除') {
                $.post("ajax/changeColor.ashx", { "num": num, "color": color, "op": 1 }, function (result) {
                    location.href = location.href;
                });
                return;
            }
            $.post("ajax/changeColor.ashx", { "num": num, "color": color, "op": 0 }, function (result) {
                if (result.code == "ok") {
                    var clone = $('tbody').children().last().clone();
                    clone.find('input').val('');
                    var input = $('tbody').children().last().find('input[name="num"]');
                    input.replaceWith(input.val());
                    input = $('tbody').children().last().find('input[name="color"]');
                    input.replaceWith(input.val());
                    input = $('tbody').children().last().find('input[type="submit"]');
                    input.removeClass("addColor").addClass("removeColor").val('删除');
                    clone.addClass($('tbody').children().length % 2 ? 'evenrow' : 'oddrow');
                    clone.find('input[type="submit"]').val('添加').bind('click', addClick);
                    $('tbody').append(clone);
                }
            });
        }

        $.ajax({
            url: 'ajax/getColor.ashx', success: function (response) {
                var tb = $('tbody');
                var clone = tb.children().first().clone();
                tb.empty();
                $.each(response, function (index, item) {
                    var tr = $('<tr></tr>').addClass(index % 2 ? 'evenrow' : 'oddrow').attr('align', 'center');
                    tr.append($('<td>' + String.fromCharCode(parseInt(item.num) + 'A'.charCodeAt(0)) + '</td>'));
                    tr.append($('<td>' + item.color + '</td>'));
                    tr.append($('<td><input type="submit" value="删除" class="form-control addColor"></td>'));
                    tb.append(tr);
                });
                tb.append(clone);
                $('.addColor').bind('click', addClick);
            }
        });
    </script>
</body>
</html>
<!--not cached-->
