# 简介
这是使用C#编写的，依赖于微信的，配合HUSTOJ使用的气球配送系统。<br/>

# 环境配置
1.安装.NET 4.6环境，可以与OJ不在同一服务器上，但要求必须能访问OJ服务器。<br/>
2.在OJ上提供一个接口，用于SQQ/SQQ/ajax/processProblems.ashx调用，获取过题数据。<br/>
3.1打开SQQ项目，修改SQQ/SQQ/ajax/processProblems.ashx.cs第35行的接口网址。<br/>
3.2修改SQQ/SQQ/WXManage.cs的16、18、20、28行相应内容。<br/>
4.发布SQQ项目。<br/>

# 使用
1.打开/Index.aspx。<br/>
2.第四个方块进行初始化。<br/>
3.第四个方块设置contest的id。<br/>
4.第三个方块开启人员登记。<br/>
5.志愿者关注公众号，回复关键词进行登记。<br/>
6.第一个方块设置最大数量。<br/>
7.第一个方块开启配送。<br/>
注：Index网址勿公开，由于是内部使用，系统目前没有任何身份验证机制。<br/>

# 开发
配送算法是系统非常关键的部分，为方便更新，分配算法是独立出来的。<br/>
打开编辑DispatchAlgorithm项目并编译成dll，替换SQQ/SQQ/DispatchAlgorithm.dll和SQQ/SQQ/bin/DispatchAlgorithm.dll即可。<br/>
