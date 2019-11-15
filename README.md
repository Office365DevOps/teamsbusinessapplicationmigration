[![Build Status](https://dev.azure.com/pp0197/default/_apis/build/status/paul-cheung.msteams-samples-task-module-csharp?branchName=master)](https://dev.azure.com/pp0197/default/_build/latest?definitionId=1&branchName=master)

# Developing and debugging a Task Module locally

## Prerequisites

The following tools should be installed.

1. [visual studio](https://visualstudio.microsoft.com/downloads)
2. [teams client](https://products.office.com/en-US/microsoft-teams/group-chat-software) or use [teams on web](https://teams.microsoft.com/)
3. [ngrok](https://ngrok.com/)

## Steps

- Make sure that the all required tools are installed.
- Use visual studio to open solution `Microsoft.Teams.Samples.TaskModule.sln` and run it(default with port 3333).
- Open ngrok terminal and run `ngrok.exe http 3333 -host-header="localhost:3333"`, copy https host
![ngrok](images/ngrok_running.png)
- Update web.config file accordingly
![web.config](images/vs_config.png)
- Use App Studio to configure tab, bot, message extension.
![app studio](images/config_tab_host.png)
![app studio](images/config_bot_host.png)
![app studio](images/config_msg_extension.png)
- Install application in App Studio
![app studio](images/install_app_studio.png)
- Select feature to install(eg. Tab) and configure
![feature](images/install_feature.png)
![feature](images/install_configure_page.png)
- Check installed tab
![tab](images/installed_tab.png)
- Create Item(open task module in tab)
![task module open](images/task_module_open.png)
- Search items in message extension and send it to team members
![search items](images/msg_extension_search.png)
- Update item(click button "Update this item" to open update modal)
![task module update](images/task_module_update.png)

> Note: you can create a bot by following this [link](https://docs.microsoft.com/en-us/microsoftteams/platform/concepts/bots/bots-create), or in App Studio.
