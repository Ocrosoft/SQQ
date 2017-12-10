<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SQQ.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-control" content="no-cache">
    <meta http-equiv="Cache" content="no-cache">
    <title>SQQ Basic</title>
    <link href="css/main.css" rel="stylesheet">
</head>
<body>
    <div id="app">
        <div data-reactroot="" class="wrapper">
            <section id="middle_sheet">
                <div>
                    <div class="cards no_free_elements">
                        <div class="card" style="-webkit-tap-highlight-color: rgba(0, 0, 0, 0); color: rgba(0, 0, 0, 0.870588); background-color: rgb(255, 255, 255); transition: all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms; box-sizing: border-box; font-family: Roboto, sans-serif; border-radius: 2px;">
                            <section class="front">
                                <div class="icon icon-smartserver" style="height: 30px;"></div>
                                <div><span class="value" id="savedCount">0</span><span class="unit"></span></div>
                                <span class="description">等待派送的气球数
                                </span><span class="link"></span>
                                <p class="link2">
                                    <strong>
                                        <a href="javascript:;" style="color: red" id="dispatchStatus">任务分配已关闭</a>
                                    </strong>
                                </p>
                            </section>
                            <section class="back">
                                <header>
                                    <p>
                                        当前有 <span id="savedCountIn">0</span> 个气球需要派送
                                    </p>
                                </header>
                                <p>SQQ会根据派送人员的具体情况进行任务分配，未分配的派送任务将会暂存，等待分配。</p>
                                <strong><a href="javascript:;" style="color: dodgerblue" id="startDispatch">开启</a></strong>
                                <strong><a href="javascript:;" style="color: green" id="maxTask" >分配设置</a></strong>
                            </section>
                        </div>
                        <div class="card" style="-webkit-tap-highlight-color: rgba(0, 0, 0, 0); color: rgba(0, 0, 0, 0.870588); background-color: rgb(255, 255, 255); transition: all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms; box-sizing: border-box; font-family: Roboto, sans-serif; border-radius: 2px;">
                            <section class="front">
                                <div class="icon icon-httpslock" style="height: 30px;"></div>
                                <div><span class="value" id="sentCount">0</span><span class="unit"></span></div>
                                <span class="description">已配送的气球数
                                </span><span class="link"></span>
                                <p class="link2">
                                    <strong></strong>
                                </p>
                            </section>
                            <section class="back">
                                <header><span>已经配送了 <span id="sentCountIn">0</span> 个气球</span></header>
                                <p>SQQ在气球配送后会记录谁在何时配送了这个气球，便于进行管理。</p>
                                <strong><a target="_blank" href="ShowSent.aspx" style="color: green">已送气球管理</a></strong>
                            </section>
                        </div>
                        <div class="card" style="-webkit-tap-highlight-color: rgba(0, 0, 0, 0); color: rgba(0, 0, 0, 0.870588); background-color: rgb(255, 255, 255); transition: all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms; box-sizing: border-box; font-family: Roboto, sans-serif; border-radius: 2px;">
                            <section class="front">
                                <div class="icon icon-blockedads" style="height: 30px;"></div>
                                <div><span class="value" id="signedCount">0</span><span class="unit"></span></div>
                                <span class="description">已登记的派送人员
                                </span><span class="link"></span>
                                <p class="link2">
                                    <strong><a href="javascript:;" style="color: red;" id="signStatus">人员登记未开启</a></strong>
                                </p>
                            </section>
                            <section class="back">
                                <header><span>已登记 <span id="signedCountIn">0</span> 个派送人员</span></header>
                                <p>关注公众号回复“ilovezufe 姓名”登记。注意回复内容不包含引号，中间有一个空格。</p>
                                <strong><a href="javascript:;" style="color: dodgerblue" id="startSign">开启</a></strong>
                                <strong><a target="_blank" href="ShowSender.aspx" style="color: green">人员管理</a></strong>
                            </section>
                        </div>
                        <div class="card" style="-webkit-tap-highlight-color: rgba(0, 0, 0, 0); color: rgba(0, 0, 0, 0.870588); background-color: rgb(255, 255, 255); transition: all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms; box-sizing: border-box; font-family: Roboto, sans-serif; border-radius: 2px;">
                            <section class="front">
                                <div class="icon icon-subscription" style="height: 30px;"></div>
                                <div><span class="value"></span><span class="unit">SQQ Basic</span></div>
                                <span class="description">SQQ 基础版
                                </span><span>-</span><span class="link">
                                    设置
                                </span>
                                <p class="link2">
                                    <strong></strong>
                                </p>
                            </section>
                            <section class="back" style="width: 85%;">
                                <p style="text-align: center; width: 100%;">
                                    <a href="javascript:;" style="color: red;" id="initation">初始化</a>
                                </p>
                                <p style="text-align: center; width: 100%;">
                                    <a href="javascript:;" style="color: dodgerblue;" id="disableLog">关闭日志</a>
                                </p>
                                <p style="text-align: center; width: 100%;">
                                    <a href="javascript:;" style="color: green;" id="setContest">设置比赛</a>
                                </p>
                            </section>
                        </div>
                    </div>
                </div>
            </section>
            <section id="bottom_sheet" style="height: 108px;">
                <textarea style="text-align: center; height: 100%; width: 100%;" id="log" readonly>
                </textarea>
            </section>
        </div>
    </div>
    <script src="js/jquery.min.js"></script>
    <script>
        var dispatchButton = $('#startDispatch'); // 分配开关
        var dispatchStatus = $('#dispatchStatus'); // 分配状态
        var maxTaskButton = $('#maxTask'); // 同时配送数量设置按钮
        var signButton = $('#startSign'); // 登记开关
        var signStatus = $('#signStatus'); // 登记状态
        var initButton = $('#initation'); // 初始化按钮
        var disableLogButton = $('#disableLog'); // 日志开关
        var logArea = $('#log'); // 日志
        var contestButton = $('#setContest'); // 设置比赛按钮
        var timerGetNewSolvedProblems; // 全局计时器
        var saved1 = $('#savedCount'); var saved2 = $('#savedCountIn'); // 未分配的题数
        var sent1 = $('#sentCount'); var sent2 = $('#sentCountIn'); // 已配送的题数
        var signed1 = $('#signedCount'); var signed2 = $('#signedCountIn'); // 已登记的人数
        var logLine = 0;

        // 日志函数，打印信息到下方日志
        function Log(text) {
            logArea.val(logArea.val() + text + '\r\n');
            logArea.scrollTop(1000000000);
        }
        // 页面刷新函数
        function refresh() {
            // 更新等待配送、正在派送、已配送
            $.ajax({
                url: 'ajax/problemsCountInfo.ashx',
                success: function (result) {
                    if (result.savedCount < 0) {
                        clearInterval(timerGetNewSolvedProblems);
                        alert('派送数据异常，检查完成后刷新页面。');
                    } else {
                        saved1.text(result.savedCount);
                        saved2.text(result.savedCount);
                        sent1.text(result.sentCount);
                        sent2.text(result.sentCount);
                    }
                }
            });
            // 刷新登记人数
            $.ajax({
                url: 'ajax/senders.ashx',
                success: function (result) {
                    signed1.text(result.length);
                    signed2.text(result.length);
                }
            });
        }
        // 任务分配函数
        function dispatch() {
            $.ajax({
                url: 'ajax/dispatch.ashx',
                success: function (dispatched) {
                    if (dispatched.count < 0) {
                        clearInterval(timerGetNewSolvedProblems);
                        alert('任务分配失败，检查完成后刷新页面。');
                    }
                }
            });
            // 立即刷新
            refresh();
        }
        // 获取新过题
        function getNewSolved() {
            $.ajax({
                url: 'ajax/processProblems.ashx',
                success: function (newSolved) {
                    if (newSolved.error) {
                        clearInterval(timerGetNewSolvedProblems);
                        alert('获取过题数据失败，检查完成后刷新页面。');
                    }
                }
            });
            dispatch();
        }
        // 获取日志
        function getLog() {
            $.ajax({
                url: 'ajax/getLogs.ashx',
                success: function (result) {
                    for (var i = logLine; i < result.length; i++) {
                        Log(result[i]);
                    }
                    logLine = result.length;
                }
            });
        }
        // 超时校验
        function checkTask() {
            $.ajax({
                url: 'ajax/checkTasks.ashx',
                success: function (result) {
                    if (result.code == "error") {
                        clearInterval(timerGetNewSolvedProblems);
                        alert('超时校验失败，检查完成后刷新页面。');
                    }
                }
            });
        }

        $(document).ready(function () {
            logArea.val(''); // 清空日志
            getLog(); // 获取日志
            // 获取分配开关状态
            dispatchButton.text('Processing...');
            dispatchStatus.text('Processing...');
            $.ajax({
                url: 'ajax/getDispatch.ashx',
                success: function (result) {
                    if (result.status) {
                        dispatchButton.text('关闭');
                        dispatchStatus.text('任务分配已开启').css('color', 'green');
                    } else {
                        dispatchButton.text('开启');
                        dispatchStatus.text('任务分配已关闭').css('color', 'red');
                    }
                }
            });
            // 获取登记开关状态
            signButton.text('Processing...');
            signStatus.text('Processing...');
            $.ajax({
                url: 'ajax/getSign.ashx',
                success: function (result) {
                    if (result.status) {
                        signButton.text('关闭');
                        signStatus.text('人员登记已开启').css('color', 'green');
                    } else {
                        signButton.text('开启');
                        signStatus.text('人员登记已关闭').css('color', 'red');
                    }
                }
            });
            refresh(); // 页面加载完成时立即刷新
        });
        // 分配开关事件
        dispatchButton.click(function () {
            dispatchButton.text('Processing...');
            dispatchStatus.text('Processing...');
            $.ajax({
                url: 'ajax/switchDispatch.ashx',
                success: function (result) {
                    if (result.status) {
                        dispatchButton.text('关闭');
                        dispatchStatus.text('任务分配已开启').css('color', 'green');
                    } else {
                        dispatchButton.text('开启');
                        dispatchStatus.text('任务分配已关闭').css('color', 'red');
                    }
                    refresh(); // 立即刷新
                }
            });
        });
        // 登记开关事件
        signButton.click(function () {
            signButton.text('Processing...');
            signStatus.text('Processing...');
            $.ajax({
                url: 'ajax/switchSign.ashx',
                success: function (result) {
                    if (result.status) {
                        signButton.text('关闭');
                        signStatus.text('人员登记已开启').css('color', 'green');
                    } else {
                        signButton.text('开启');
                        signStatus.text('人员登记已关闭').css('color', 'red');
                    }
                    refresh(); // 立即刷新
                }
            });
        })
        // 日志开关事件
        disableLogButton.click(function () {
            logArea.parent().slideToggle('fast', function () {
                if (disableLogButton.text().indexOf('关闭') != -1) {
                    disableLogButton.text('开启日志');
                } else {
                    disableLogButton.text('关闭日志');
                }
            });
        });
        // 初始化事件
        initButton.click(function () {
            var ans = prompt('初始化会删除所有数据，只需要在赛前准备执行。输入yes开始初始化。');
            if (ans == 'yes') {
                $.ajax({
                    url: 'ajax/initation.ashx',
                    success: function (result) {
                        if (result.code == 'ok') {
                            alert('初始化成功，将刷新页面。');
                            location.href = location.href;
                        } else {
                            alert('初始化失败，详情查看日志。');
                        }
                    }
                });
            }
        });
        // 设置比赛ID
        contestButton.click(function () {
            $.ajax({
                url: 'ajax/getContestID.ashx',
                success: function (result) {
                    var res = prompt('要处理的比赛ID，0为不处理。', result.contest_id);
                    if (res === null) {
                        return;
                    }
                    $.ajax({
                        url: 'ajax/setContestID.ashx?cid=' + res,
                        success: function (response) {
                            if (response.contest_id != res) {
                                alert('修改失败，没有详情。');
                            }
                            refresh(); // 立即刷新
                        }
                    });
                }
            });
        });
        // 设置最多配送数
        maxTaskButton.click(function () {
            $.ajax({
                url: 'ajax/getMaxTask.ashx',
                success: function (result) {
                    var res = prompt('设置每个人可同时配送的最大数量。', result.maxTask);
                    if (res === null) {
                        return;
                    }
                    $.ajax({
                        url: 'ajax/setMaxTask.ashx?maxTask=' + res,
                        success: function (response) {
                            if (response.code != "ok") {
                                alert('修改失败，没有详情。');
                            }
                            refresh(); // 立即刷新
                        }
                    });
                }
            });
        });

        // 定时任务
        timerGetNewSolvedProblems = setInterval(function () {
            // getNewSolved()执行完成后会调用dispatch()进行任务分配
            // dispatch()执行完成后会调用refresh()刷新页面数据
            getNewSolved();
            // 获取日志
            getLog();
            // 超时校验
            checkTask();
        }, 10000); // 每十秒执行
    </script>
</body>
</html>
