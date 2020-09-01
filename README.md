
# 影院管理和售票系统
本项目为**C#和软件工程**的综合实践项目

此文档仅简单介绍项目，完整文档请到项目doc目录下载软件说明书和演示PPT

项目已上传到[Github](https://github.com/ZhaoTongFeng/TheaterManagementSystem)

## 简介

本系统分管理端和顾客端两个子系统
* 管理端帮助影院管理员管理和统计电影数据、档期数据、订单数据。
* 顾客端为顾客提供购票服务。



## 功能
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




## 软件工程
**图片如果加载不出来，可以下载帮助文档自行查看**

#### 用例图
最开始画了几层，到最后只剩一层，这样比较清晰
<div align=center><img src="http://llag.net/markdown-img/TMS/0.1.F.用例图.png" alt="用例图"/></div>



#### 数据库ER图
所有State为枚举值，类型可能为布尔值或者在Config文件中进行定义，

直接定义枚举值而不使用枚举类型，效果差不多，但是代码看上去就没有层次感
<div align=center><img src="http://llag.net/markdown-img/TMS/0.2.F.ER图.jpg" alt="ER图"/></div>


#### 类图
分解类图见PPT
<div align=center><img src="http://llag.net/markdown-img/TMS/0.3.F.类图.jpg" alt="类图"/></div>


#### 序列图
所有用例均有序列图，请下载演示PPT进行查看
<div align=center><img src="http://llag.net/markdown-img/TMS/0.4.F.序列图-购票.png" alt="序列图-购票"/></div>

注意：判断之后执行不同事件的部分画错了


## 界面截图

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
网格做的一个简单的选座页面
<div align=center><img src="http://llag.net/markdown-img/TMS/2.4.F.客户端-选座.jpg" alt="客户端-选座"/></div>



#### 客户端-付款
这个页面主要是对之前选择的数据进行汇总显示，在文档中针对付款这部分进行了设计，因为条件不限制，并没有进行实际编写，所以代码中有些地方可能存在问题

<div align=center><img src="http://llag.net/markdown-img/TMS/2.6.F.客户端-付款.jpg" alt="客户端-付款"/></div>




#### 管理端-主页
这个页面显示当前正在上映的电影，允许管理员对电影进行下架操作

已经实现的下架操作只是单纯的修改电影的状态，这的确是核心的操作，但实际上还有很多问题需要考虑的

<div align=center><img src="http://llag.net/markdown-img/TMS/1.1.F.管理端-主页.jpg" alt="管理端-主页"/></div>




#### 管理端-档期


* 显示日历对应的档期，

* 输入电影的比例排制档期功能，具体功能和算法在PPT中有介绍

自动排制档期功能，当时测试的时候，因为添加的电影数量过多，导致场次安排上不是很合理。
一般来说，电影院不会同时上映这么多电影，这个程序正常情况下，会给不同的放映室安排一整天的放映顺序，
每部电影的数量不会太多也不会太少，按照其比例进行分配

<div align=center><img src="http://llag.net/markdown-img/TMS/1.2.F.管理端-档期.jpg" alt="管理端-档期"/></div>


#### 管理端-导入
支持两种方式进行导入
* 直接输入
* 从文件导入

从文件导入数据之后，会加载到上面的各个输入框中，允许进行编辑，然后确认插入

从文件导入的功能，虽然运行起来能够将数据导入，但实际上代码并不完整，待插入的数据在代码中硬编码，然后调用插入接口插入数据库

如果要真正实现从文件中导入，可以采取打开JSON/XML文件的方式，将数据读取到字典，再插入

<div align=center><img src="http://llag.net/markdown-img/TMS/1.3.F.管理端-导入.jpg" alt="管理端-导入"/></div>


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
注意：首次运行时如果报以下错误

1. Select异常EntityFramework.SqlServer
2. 获取异常EntityFramework
3. Select异常EntityFramework

首先检查bin/Debug目录下是否存在EntityFramework.SqlServer
如果没有则没有安装实体框架，检查环境配置


其次需要将下面两个文件拷贝到bin/Debug目录下
* SQL/Database1.mdf文件
* img文件夹

mdf是数据库文件，这里需要手动复制的原因是当时数据库在运行时的复制操作存在BUG

SQL下和根目录下共有两个Database1.mdf，两个文件都一样

## 其它一些说明



虽然是一个纯C#的单机WPF项目，最初试想的场景只是电影院门口的自助售票机，
但是考虑到，现代的影院售票和管理系统都不可能是单机系统，
整体上采取的MVC架构，使前后端完全分离的，所以可以同时支持管理端和顾客端两个客户端，
并且采用字典进行数据交互，更加易扩展成一个服务端-客户端的网络应用程序。


看了代码之后可能会感觉到Controller非常多余，因为仅仅都是对Service函数的包装，我也这么觉得，
但是又不得不承认，麻烦归麻烦，Controller对Service进行一个分类和汇总，用户和管理员对于同样一个ShowXXX方法，可见度不同,
比如显示电影列表，用户只想看到正在上映的，而管理员可能还需要看到已下架的和即将上映的。
因为有两个前端，所以我觉得用两个Controller来封装Service的函数，以此来建议前端允许调用的函数，
顾客端就只能调用UserController中的函数，而管理员端两个都能调用。
以上对Controller的使用仅代表我个人。

采用数据库驱动的实体框架，Model是根据SQL文件自动生成的，如果想要更改Model，只用更新SQL

前端View部分的委托用的不太正确，即使能够正常工作

这个项目是初学C#的时候写的，除了设计上参考了一下其它的作品，程序完全是自己设计编写的，代码有一些问题，这里我也说了一些。