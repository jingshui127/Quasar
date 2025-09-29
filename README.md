# Quasar 远程管理工具

[![Build status](https://ci.appveyor.com/api/projects/status/5857hfy6r1ltb5f2?svg=true)](https://ci.appveyor.com/project/MaxXor/quasar)
[![Downloads](https://img.shields.io/github/downloads/quasar/Quasar/total.svg)](https://github.com/quasar/Quasar/releases)
[![License](https://img.shields.io/github/license/quasar/Quasar.svg)](LICENSE)

**免费、开源的 Windows 远程管理工具**

Quasar 是一款使用 C# 编写的快速、轻量级远程管理工具。其用途涵盖从用户支持、日常系统管理到员工监控等多个场景。凭借高稳定性和易于使用的用户界面，Quasar 是您理想的远程管理解决方案。

请查看 [入门指南](https://github.com/quasar/Quasar/wiki/Getting-Started)。

## 界面截图

![远程命令行](Images/remote-shell.png)

![远程桌面](Images/remote-desktop.png)

![远程文件管理](Images/remote-files.png)

## 功能特性
* TCP 网络通信（支持 IPv4 和 IPv6）
* 高效的网络序列化（Protocol Buffers）
* 加密通信（TLS）
* UPnP 支持（自动端口映射）
* 任务管理器
* 文件管理器
* 启动项管理器
* 远程桌面
* 远程命令行
* 远程执行
* 系统信息查看
* 注册表编辑器
* 系统电源控制（重启、关机、待机）
* 键盘记录（支持 Unicode）
* 反向代理（SOCKS5）
* 密码恢复（常见浏览器和 FTP 客户端）
* 以及更多功能！

## 下载地址
* [最新稳定版本](https://github.com/quasar/Quasar/releases)（推荐）
* [最新开发快照](https://ci.appveyor.com/project/MaxXor/quasar)

## 支持的运行环境和操作系统
* .NET Framework 4.5.2 或更高版本
* 支持的操作系统（32 位和 64 位）
  * Windows 11
  * Windows Server 2022
  * Windows 10
  * Windows Server 2019
  * Windows Server 2016
  * Windows 8/8.1
  * Windows Server 2012
  * Windows 7
  * Windows Server 2008 R2
* 对于较旧的系统，请使用 [Quasar 1.3.0 版本](https://github.com/quasar/Quasar/releases/tag/v1.3.0.0)

## 编译说明
在 Visual Studio 2019+ 中打开项目文件 `Quasar.sln`，确保已安装 .NET 桌面开发功能，并[还原 NuGET 包](https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore)。所有包安装完成后，可以通过点击顶部的 `Build` 菜单或按 `F6` 键来编译项目。生成的可执行文件可以在 `Bin` 目录中找到。请参见下表选择合适的构建配置。

## 客户端构建
| 构建配置 | 使用场景 | 说明
|---------|---------|------
| Debug 配置 | 测试 | 将使用预定义的 [Settings.cs](/Quasar.Client/Config/Settings.cs) 文件，因此在编译客户端之前需要编辑此文件。您可以直接使用指定的设置执行客户端。
| Release 配置 | 生产 | 启动 `Quasar.exe` 并使用客户端构建器。

## 贡献代码
参见 [CONTRIBUTING.md](CONTRIBUTING.md)

## 发展路线
参见 [ROADMAP.md](ROADMAP.md)

## 文档资料
使用说明和其他文档请参见 [wiki](https://github.com/quasar/Quasar/wiki)。

## 许可证
Quasar 采用 [MIT 许可证](LICENSE) 分发。  
第三方许可证位于 [此处](Licenses)。

## 感谢您！
我非常感谢各种反馈和贡献。感谢您使用和支持 Quasar！