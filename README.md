# 本地开发和调试Business.Application.Migration项目

## 关于代码下载和分支切换，及bot命令的定义，可以切换到after分支查看详情

## 准备条件

安装以下软件.

1. [visual studio](https://visualstudio.microsoft.com/downloads)
2. [teams客户端](https://products.office.com/en-US/microsoft-teams/group-chat-software)或者使用[web版本](https://teams.microsoft.com/)
3. [ngrok反向代理工具](https://ngrok.com/)

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
- 通过App Studio安装Business Application Migration应用，选择安装的功能（可以单独安装Tab, Bot，或者一起装，在“设置”按钮的下来框里选择）
![app studio](images/install_app_studio1.png)
![app studio](images/install_app_studio2.png)
![app studio](images/install_app_studio3.png)
![feature](images/install_feature.png)
- 配置Tab页面（before分支我们选择All，是传统登录的流程）
![feature](images/install_configure_page.png)
- 打开添加的Tab页面
![tab](images/installed_tab.png)
- 创建商品（点击Create按钮）
![task module open](images/task_module_open.png)
- 搜索商品发送到聊天里
![search](images/msg_extension_search1.png)
![search](images/msg_extension_search2.png)
![search](images/msg_extension_search3.png)
- 更新商品
![task module update](images/task_module_update.png)

> 注: 如果自己想注册Bot，请点击[这里](https://docs.microsoft.com/en-us/microsoftteams/platform/concepts/bots/bots-create)，或者直接在App Studio里的Bot功能模块创建。
