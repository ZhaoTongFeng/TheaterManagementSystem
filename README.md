# 影院管理和售票系统
C#和软件工程的课程大作业，仅供参考

这里只简单介绍，说明书和演示PPT在项目doc目录下

下载地址：[Github](https://github.com/ZhaoTongFeng/TheaterManagementSystem)
大小：12MB
## 简介
按照系统划分，由管理端和顾客端两个子系统组成
* 管理端帮助影院管理员管理和统计电影数据、档期数据、订单数据。
* 顾客端为顾客提供购票服务。

MVC架构

没有网络编程，但是实际上是C/S结构，只不过没有网络进行数据交换，而是通过字典来进行数据交换（模拟网络数据），服务端和两个客户端都是完全分离的，如果需要改成网络程序，只需要在Controller增加网络接口即可。



## 功能
因为是一个人进行开发，又要编写文档，时间有限，只做了下面这些核心的功能。

* 管理端：
  - 查看电影，档期，订单数据
  - 导入电影
  - 自动排制档期
  - 销售统计

* 顾客端：
  - 查看正在上映的电影
  - 查看电影对应的档期
  - 选择座位
  - 付款




## 软件工程UML
**图片如果加载不出来，可以下载帮助文档自行查看**

#### 用例图
最开始画了几层，到最后只剩一层，这样比较清晰
<div align=center><img src="http://llag.net/markdown-img/TMS/0.1.F.用例图.png" alt="用例图"/></div>



#### 数据库ER图


所有State为枚举值，类型为布尔值或者在Config文件中进行定义

存在问题：直接定义枚举值而不使用枚举类型，效果差不多，但是代码看上去就没有层次感

<div align=center><img src="http://llag.net/markdown-img/TMS/0.2.F.ER图.jpg" alt="ER图"/></div>


#### 类图
分解类图见PPT
<div align=center><img src="http://llag.net/markdown-img/TMS/0.3.F.类图.jpg" alt="类图"/></div>


#### 序列图
所有用例均有序列图，请下载演示PPT进行查看
<div align=center><img src="http://llag.net/markdown-img/TMS/0.4.F.序列图-购票.png" alt="序列图-购票"/></div>

注意：判断之后执行不同事件的部分画错了


## 界面截图
#### 客户端
* 多个View Page在一个Window中进行切换，用的Screen Stack的方式来处理

* 委托来调用跳转到的页面的函数

* 每个页面打开前都先从”后端“获取页面数据

* 在购票过程中，始终传递一个订单字典，记录顾客选择的电影和若干个座位的信息，最终传递给”后端“插入数据库，完成订单，返回主页

#### 客户端-主页
顾客门户，显示最近场次

<div align=center><img src="http://llag.net/markdown-img/TMS/2.1.F.客户端-主页.jpg" alt="客户端-主页"/></div>


#### 客户端-购票
显示所有上映的电影

<div align=center><img src="http://llag.net/markdown-img/TMS/2.2.F.客户端-购票.jpg" alt="客户端-购票"/></div>




#### 客户端-档期
显示某部电影的档期
<div align=center><img src="http://llag.net/markdown-img/TMS/2.5.F.客户端-档期.jpg" alt="客户端-档期"/></div>



#### 客户端-选座
网格做的简易选座页面
<div align=center><img src="http://llag.net/markdown-img/TMS/2.4.F.客户端-选座.jpg" alt="客户端-选座"/></div>



#### 客户端-付款
这个页面主要是对之前选择的数据进行汇总显示
<div align=center><img src="http://llag.net/markdown-img/TMS/2.6.F.客户端-付款.jpg" alt="客户端-付款"/></div>

在文档中针对付款这部分进行了设计，但是不可能真的去接入第三方，因此这个地方点击XX支付后，直接跳到付款成功环节


#### 管理端
主要就是各种数据的增删查改

#### 管理端-主页
这个页面显示当前正在上映的电影，允许管理员对电影进行下架操作

已经实现的下架操作只是单纯的修改电影的状态，实际上不可能这么就下架了

<div align=center><img src="http://llag.net/markdown-img/TMS/1.1.F.管理端-主页.jpg" alt="管理端-主页"/></div>




#### 管理端-档期


* 显示日历对应的档期，

* 输入电影的比例排制档期功能，具体功能和算法在PPT中有介绍



<div align=center><img src="http://llag.net/markdown-img/TMS/1.2.F.管理端-档期.jpg" alt="管理端-档期"/></div>
自动排制档期功能，当时测试的时候，因为添加的电影数量过多，导致场次安排上不是很合理。

一般来说，电影院不会同时上映这么多电影，这个程序正常情况下，会给不同的放映室安排一整天的放映顺序，每部电影的数量不会太多也不会太少，按照其比例进行分配

#### 管理端-导入
两种方式进行导入
* 直接输入
* 从文件导入
<div align=center><img src="http://llag.net/markdown-img/TMS/1.3.F.管理端-导入.jpg" alt="管理端-导入"/></div>

从文件导入
1. 点击”从文件导入“
2. 选择XML/JSON文件
3. 读取文件数据并解析
4. 设置输入框数据

直接输入

4. 设置输入框数据
5. 确定输入
6. 从输入框读取到字典
7. 数据传输到”后端“
8. 后端插入数据库

上面就是插入数据的流程，我没有做第三步，直接用的硬编码，这个很容易实现。


## 运行


#### 帮助文档
* 软件说明书
* 演示PPT

都在项目doc目录

#### 环境配置
Visual Studio 安装以下环境

* 工作负载
  * .Net桌面开发
  * 数据库存储和处理

* 单个组件
  * Entity Framework 6

#### 运行
注意：首次运行时会报以下错误

1. Select异常EntityFramework.SqlServer
2. 获取异常EntityFramework
3. Select异常EntityFramework

首先检查bin/Debug目录下是否存在EntityFramework.SqlServer
如果没有则没有安装实体框架或者是数据库，检查环境配置


其次需要将下面两个文件拷贝到bin/Debug目录下
* SQL/Database1.mdf
* img文件夹

.mdf是数据库文件，这里需要手动复制的原因是当时数据库在运行时的复制操作存在BUG

SQL下和根目录下共有两个Database1.mdf，两个文件都一样

可能还有版本问题

因为采用数据库驱动的实体框架，Model是根据SQL文件自动生成的，要想更改Model，只能通过更新SQL的方式，一般我要修改SQL之前，我都会先实体框架生成的文件都删除，改完SQL后，再重新自动生成，这步一定要谨慎。

建议把前面的从文件导入数据先弄好，写个开发环境下一键导入测试数据的功能，不要在数据库里面直接输入，数据库记得备份。
 
 
完整的做一个项目收获还蛮多的，只不过终究还是一个人扛下了所有。。。
差不多就这些了，留个纪念