# TShockSearch

一个可以在直播玩泰拉瑞亚时，可以让观众与主播互动的插件。idea和原作者是：ArsiIksait。我在额外增加了指令开关和配置文件功能。

插件目前能很好地把弹幕文本，广播给服里的玩家。


## 插件安装

1、将`TerrariaBLive.dll`、`TerrariaBLive.json`、`OpenBLive.dll`、`Websocket.Client.dll` 和 `System.Reactive`这5个文件拷贝到tshock服务器的 “ServerPlugins”目录下。

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
```
/blive help，指令帮助
/blive status，查询功能状态
/blive on，开启弹幕互动功能
/blive off，关闭弹幕互动功能
```

## 权限
| 指令 | 权限名 | 授权示意 |
| --- | --- | --- |
| /blive | blive | /group addperm owner blive  |
备注：一般来说，服主和超级管理员拥有全部的权限，无需授权，如果你所在的组是owner组可以按照上面的授权示意来。不建议把指令权限授权给普通玩家（default），因为这意味着全部人都能开关弹幕功能。