# TerrariaBLive

一个可以在直播玩泰拉瑞亚时，可以让观众与主播互动的插件，目前插件会把弹幕内容广播给服里的玩家。

这个插件的idea和原作者是 @ArsiIksait 。我在此基础上增加了指令开关和配置文件功能。


## 插件安装

1、将`TerrariaBLive.dll`、`TerrariaBLive.json`、`OpenBLive.dll`、`Websocket.Client.dll` 和 `System.Reactive`这5个文件拷贝到tshock服务器的 “ServerPlugins”目录下。（下载地址：[TerrariaBLive-v0.0.3.zip](https://github.com/hufang360/TerrariaBLive/releases/download/v0.0.3/TerrariaBLive-v0.0.3.zip)）

2、用记事本等软件打开“TerrariaBLive.json”文件，按里面提示去获得/申请对应信息，将申请到的信息填进配置文件里。

3、更改配置文件，需要重新开服。

原始配置文件：
```json
{
  "code": "身份码：访问 https://play-live.bilibili.com/ 获取",
  "appId": "项目ID：访问 https://open-live.bilibili.com/open-manage 创建项目",
  "accessKeyId": "开发密钥：访问 https://open-live.bilibili.com/document/quickStart.html 进行申请",
  "accessKeySecret": "开发密钥"
}
```

修改示意（以下信息仅用作格式示例，信息做了处理，是失效的）：
```json
{
  "code": "BS5F7Z349HF04",
  "appId": "1670127837591",
  "accessKeyId": "B8HfOO6oJanyL7CzDnPfYg9h",
  "accessKeySecret": "TaPGcesj0aPoJpektLsyVzcuNAKKOU"
}
```


## 指令
```shell
/blive help，指令帮助
/blive status，查询功能状态
/blive on，开启弹幕互动功能
/blive off，关闭弹幕互动功能
```

## 权限
| 指令 | 权限名 | 授权示意 |
| --- | --- | --- |
| /blive | blive | /group addperm owner blive  |

备注：一般来说，服主和超级管理员拥有全部的权限，无需授权，如果你所在的组是owner组可以按照上面的授权示意来。

## 开发
目前只上传了tshock这边的代码，弹幕功能使用的b站的官方sdk/示例，请前往[bilibili开放平台](https://open-live.bilibili.com/document/doc&tool/cSDK.html)下载。

部分文件结构：
```
├─OpenBLive
│  ├─OpenBLive.csproj

├─TerrariaBLive
│  ├─TerrariaBLive.csproj

├─TerrariaBLive.sln
```
