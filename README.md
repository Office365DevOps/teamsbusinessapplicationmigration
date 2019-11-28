# 本地开发和调试Business.Application.Migration项目

## 准备条件

### 安装以下软件

1. [visual studio](https://visualstudio.microsoft.com/downloads)
2. [teams客户端](https://products.office.com/en-US/microsoft-teams/group-chat-software)或者使用[web版本](https://teams.microsoft.com/)
3. [ngrok反向代理工具](https://ngrok.com/)

### 下载代码

#### 方法1

![download](images/download_code.png)

### 方法2  

1. 需要安装git  
2. 复制代码仓库地址
![download](images/clone_code_copy_url.png)
3. 选择自己的文件夹(我这里的文件夹名称为github)，输入以下命令，回车，进行克隆
![download](images/clone_code_clone.png)

### 如何切换before和after分支

打开命令行，首先进入clone下来项目的目录，如下
![ngrok](images/checkout_branch.png)

1. 切换到before分支  
`> git checkout before`
2. 切换到after分支  
`> git checkout after`

## 步骤

- 使用visual studio打开解决方案 `Business.Application.Migration.sln`，运行起来(默认端口3333).
- 打开终端，运行ngrok，输入命令： `ngrok.exe http 3333 -host-header="localhost:3333"`, 记住这里的https的地址，一会儿会用。
![ngrok](images/ngrok_running.png)
- 相应更新web.config(如果我们已经在teams中国社区里了，可以使用已有代码中的bot，它是社区tenant下的一个bot)
![web.config](images/vs_config.png)
- 使用App Studio配置tab, bot和message extension.
![app studio](images/config_tab_host.png)
![app studio](images/config_bot_host.png)
![app studio](images/config_msg_extension.png)
- Message Extension中的command定义（机器人的命令）
![app studio](images/msg_extension_addcmd1.png)
![app studio](images/msg_extension_addcmd2.png)
![app studio](images/msg_extension_addcmd3.png)
- 通过App Studio安装Business Application Migration应用，选择安装的功能（可以单独安装Tab, Bot，或者一起装，在“设置”按钮的下来框里选择）
![app studio](images/install_app_studio1.png)
![app studio](images/install_app_studio2.png)
![app studio](images/install_app_studio3.png)
![feature](images/install_feature.png)
- 配置Tab页面
![feature](images/install_configure_page.png)
- 打开添加的Tab页面
![tab](images/installed_tab.png)
- 创建商品（点击Create按钮）
![task module open](images/task_module_open.png)
- 搜索商品发送到聊天里
![search](images/msg_extension_search1.png)
![task module update](images/msg_extension_search2.png)
![task module update](images/msg_extension_search3.png)
- 更新商品
![task module update](images/task_module_update.png)

> 注: 如果自己想注册Bot，请点击[这里](https://docs.microsoft.com/en-us/microsoftteams/platform/concepts/bots/bots-create)，或者直接在App Studio里的Bot功能模块创建。

注：如果想查看Graph调用的文档，请切换到before分支进行查看。