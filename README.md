本项目已合并至[主项目](https://github.com/Liu233w/acm-statistics)中，现已停止维护。

# acm-statistics-abp
[![Build Status](https://travis-ci.org/Liu233w/acm-statistics-abp.svg?branch=master)](https://travis-ci.org/Liu233w/acm-statistics-abp)
[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=acm-statistics-abp&metric=alert_status)](https://sonarcloud.io/dashboard?id=acm-statistics-abp)
[![codecov](https://codecov.io/gh/Liu233w/acm-statistics-abp/branch/master/graph/badge.svg)](https://codecov.io/gh/Liu233w/acm-statistics-abp)

[新版 NWPU-ACM 查询系统](https://github.com/Liu233w/acm-statistics)后端

## 运行环境
- [.Net Core Sdk](https://www.microsoft.com/net/download) 2.0 或以上 （只在编译时需要）
- MySQL 5.7 或以上
- Windows 或 Linux 系统均可

## 运行方式
### 使用 Visual Studio 调试并运行
#### 部署数据库
1. 在 MySQL 中新建一个名为 `acm_statistics_abp` ，编码为 utf8 的数据库
2. 直接打开解决方案（下方可以直接参考ABP的教程）
3. 将启动项目设置为 `AcmStatisticsAbp.Web.Host`
4. 修改 `src/AcmStatisticsAbp.Web.Host/appsettings.json` 中的 `ConnectionStrings`，将用户名和密码改成你的MySQL的。
5. 打开程序包管理器控制台，将其中的默认项目设置为 `AcmStatisticsAbp.EntityFrameworkCore`
6. 在程序包管理器控制台中输入 `update-database` 并执行
#### 运行程序
直接使用 `ctrl+f5` 运行或 `f5` 进行调试

### 直接部署源代码并运行（Linux）
参照上文“运行环境”一节中的说明安装依赖
#### 部署数据库
1. 在 MySQL 中新建一个名为 `acm_statistics_abp` ，编码为 utf8 的数据库
2. 进入 `src/AcmStatisticsAbp.Migrator` 目录
3. 修改 `appsettings.json`，按照 `src/AcmStatisticsAbp.Web.Host/appsettings.json` 中的 `ConnectionStrings` 的格式填写数据库用户名和密码
4. 执行 `dotnet run -c Release`，按照提示操作，从而更新数据库表结构
#### 运行程序
1. 进入 `src/AcmStatisticsAbp.Web.Host/` 目录，复制 `appsettings.Staging.json` 文件，重命名为 `appsettings.Production.json`。
2. 打开刚刚的 `appsettings.Production.json` 文件，根据实际情况修改 ConnectionStrings
4. **重要** 请务必修改文件中的 `SecurityKey` 字段，否则任何人都可以根据开源代码中的本字段来生成 JWT，从而伪造身份。
5. 删除此目录下的 `Properties/launchSettings.json` 文件，从而切换到 Production 模式
6. 回到 `src/AcmStatisticsAbp.Web.Host/` 目录，在目录下执行 `dotnet run -c Release`

## 设置说明
### 邮件发送
- 本项目使用了阿里云邮件推送功能来发送邮件，从而进行邮箱验证等操作，要在自己的设备上使用此功能，您必须修改配置以使用您自己的账户
- 您需要修改数据库的 `appsettings` 表来更改设置，在第一次运行时，后端将自动在数据库中生成相应的设置项，您可以自行修改。或者，您也可以使用 API 来修改设置项（`AdminSettings`）
- 需要修改的设置项为：
  - App.EmailConfirmationBaseUrl
  - App.AliYunEmailAccessSecret
  - App.AliYunEmailAccessKeyId
  - App.AliYunEmailReplyToAddress
- 您可以使用 API `/api/services/app/AdminSetting/ListAllApplicationSettings` 来获取上述设置项的说明

## 开源协议
GPL-3.0 协议

## 贡献代码
- 欢迎任何人贡献代码。
- git 的提交格式遵循 [Git Commit Angular 规范](https://gist.github.com/stephenparish/9941e89d80e2bc58a153)
    （[中文版](http://www.ruanyifeng.com/blog/2016/01/commit_message_change_log.html)）
- 您可以使用 [cz-cli](https://github.com/commitizen/cz-cli) 来辅助提交 commit

