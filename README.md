
# 影院管理和售票系统
本项目为**C#** 和 **软件工程**课程作业，技术和能力有限，可能存在很多问题，仅供参考。

## 简介

本系统分管理端和顾客端两个子系统
* 管理端帮助影院管理员管理和统计电影数据、档期数据、订单数据。
* 客户端为顾客提供购票服务。

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

*付款功能仅仅做了设计，没有具体实现

## 预览
**图片如果加载不出来，可以下载帮助文档自行查看**

#### 用例图
![123](http://llag.net/markdown-img/0.用例图_1.png)
#### 数据库ER图
![123](http://llag.net/markdown-img/0.ER图.png)
#### 类图
![123](http://llag.net/markdown-img/0.2.类图.png)
#### 序列图
![123](http://llag.net/markdown-img/0.序列图-购票.png)

*所有用例均有序列图，请下载演示PPT进行查看

# 界面截图

## 客户端-主页
顾客门户，显示最近场次

![客户端-主页](http://llag.net/markdown-img/2.1.客户端-主页_1.png)


## 客户端-购票
显示所有上映的电影

![客户端-购票](http://llag.net/markdown-img/2.2.客户端-购票_2.png)


## 客户端-档期
显示某部电影的档期

![客户端-档期](http://llag.net/markdown-img/2.5.客户端-档期_1.png)

## 客户端-选座
网格做的一个简单的选座页面

![客户端-选座](http://llag.net/markdown-img/2.4.客户端-选座_1.png)

## 客户端-付款
这个页面主要是对之前选择的数据进行汇总显示，在文档中针对付款这部分进行了设计，因为条件不限制，并没有进行实际编写，所以代码中有些地方可能存在问题

![客户端-付款](http://llag.net/markdown-img/2.6.客户端-付款.png)


## 管理端-主页
这个页面显示当前正在上映的电影，允许管理员对电影进行下架操作

已知的BUG：因为对电影院管理体系不太熟悉，这个功能感觉没有任何实用价值

![管理端-主页](http://llag.net/markdown-img/1.1.管理端-主页.png)


## 管理端-档期
这个地方主要是做的显示日历日期对应的档期，以及输入电影的比例自动排制档期功能，具体功能和算法在PPT中有介绍

当时测试的时候，因为添加的电影数量过多，导致场次安排上不是很合理，一般来说，电影院不会同时上映这么多电影

![管理端-档期](http://llag.net/markdown-img/1.2.管理端-档期_2.png)


## 管理端-导入
支持两种方式进行导入
* 直接输入
* 从文件导入

从文件导入数据之后，会加载到上面的各个输入框中，允许进行编辑，然后确认插入

从文件导入的功能，虽然运行起来能够将数据导入，但实际上代码并不完整，待插入的数据在代码中硬编码，然后调用插入接口插入数据库

如果要真正实现从文件中导入，可以采取打开JSON/XML文件的方式，将数据读取到字典，再插入

![管理端-导入](http://llag.net/markdown-img/1.3.管理端-导入.png)


## 运行


#### 帮助文档
* 软件说明书
* 演示PPT

见doc目录

#### 环境配置
需要安装数据库

#### 运行
注意：首先进行第一次运行，因为缺少数据库所以会报错

需要将一下文件拷贝到bin/debug或者bin/release目录下

* SQL/Database1.mdf文件
* img文件夹


## 其它
虽然这个项目是一个纯C#的WPF项目，最初试想的场景只是电影院门口的自助售票机，
但是考虑到，现代的影院管理系统不可能是单机系统，所以在整体上采取的MVC架构，前后端基本上是完全分离的，
采用字典进行数据交互，所以这个项目可以很轻松的扩展成一个网络应用程序

你可能会感觉到Controller非常多余，因为仅仅都是对Service函数的包装，我也这么觉得。
但是又不得不承认，麻烦归麻烦，Controller对Service进行一个分类和汇总，用户和管理员对于同样一个ShowXXX方法，可见度不同,
比如显示电影列表，用户只想看到正在上映的，而管理员可能还需要看到已下架的和即将上映的。
因为有两个前端，所以我觉得用两个Controller来封装Service的函数，以此来建议前端允许调用的函数，
顾客端就只能调用UserController中的函数，而管理员端两个都能调用。