﻿namespace TerrariaBLive
{
    /// <summary>
    /// 错误码
    /// 参考：`https://open-live.bilibili.com/document/doc&tool/auth.html#公共错误码`
    /// </summary>
    public class ErrorCodeMapping
    {
        public static string GetDesc(int errCode)
        {
            return errCode switch
            {
                4000 => "参数错误。请检查必填参数，参数大小限制",
                4001 => "应用无效。请检查header的x-bili-accesskeyid是否为空，或者有效",
                4002 => "签名异常。请检查header的Authorization",
                4003 => "请求过期。请检查header的x-bili-timestamp",
                4004 => "重复请求。请检查header的x-bili-nonce",
                4005 => "签名method异常。请检查header的x-bili-signature-method",
                4006 => "版本异常。请检查header的x-bili-version",
                4007 => "IP白名单限制。请确认请求服务器是否在报备的白名单内",
                4008 => "权限异常。请确认接口权限",
                4009 => "接口访问限制。请确认接口权限及请求频率",
                4010 => "接口不存在。请确认请求接口url",
                4011 => "Content。Type不为application/json	请检查header的Content-Type",
                4012 => "MD5校验失败。请检查header的x-bili-content-md5",
                4013 => "Accept不为application。json	请检查header的Accept",
                5000 => "服务异常。请联系B站对接同学",
                5001 => "请求超时。请求超时",
                5002 => "内部错误。请联系B站对接同学",
                5003 => "配置错误。请联系B站对接同学",
                5004 => "房间白名单限制。请联系B站对接同学",
                5005 => "房间黑名单限制。请联系B站对接同学",
                6000 => "验证码错误。验证码校验失败",
                6001 => "手机号码错误。检查手机号码",
                6002 => "验证码已过期。验证码超过规定有效期",
                6003 => "验证码频率限制。检查获取验证码的频率",
                7000 => "不在游戏内。当前房间未进行互动游戏",
                7001 => "请求冷却期。上个游戏正在结算中，建议10秒后进行重试",
                7002 => "房间重复游戏。当前房间正在进行游戏,无法开启下一局互动游戏",
                7003 => "心跳过期。当前game_id错误或互动游戏已关闭",
                7004 => "批量心跳超过最大值。批量心跳单次最大值为200",
                7005 => "批量心跳ID重复。批量心跳game_id存在重复,请检查参数",
                7007 => "身份码错误。请检查身份码是否正确",
                8002 => "项目无权限访问。确认项目ID是否正确",
                _ => "",
            };

        }
    }
}