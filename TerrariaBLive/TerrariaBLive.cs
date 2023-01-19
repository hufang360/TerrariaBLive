using Microsoft.Xna.Framework;
using OpenBLive.Client;
using OpenBLive.Client.Data;
using OpenBLive.Runtime;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace TerrariaBLive
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override string Name => "TerrariaBLive";
        public override string Description => "一个可以在直播玩泰拉瑞亚时，可以让观众与主播互动的插件。";
        public override string Author => "ArsiIksait & hufang360";
        public override Version Version => new(0, 0, 3);

        static string gameId = string.Empty;
        readonly IBApiClient bApiClient = new BApiClient();
        int ErrorCode = 0;
        bool isOpen = false;
        InteractivePlayHeartBeat m_PlayHeartBeat;
        WebSocketBLiveClient m_WebSocketBLiveClient;
        AppStartInfo startInfo;

        public Config _config;

        public Plugin(Main game) : base(game) { }

        public override void Initialize()
        {
            string perm = "blive";
            Commands.ChatCommands.Add(new Command(perm, BLiveCommand, "blive", "danmu") { HelpText = "弹幕姬" });

            var configFile = Path.Combine(ServerApi.ServerPluginsDirectoryPath, "TerrariaBLive.json");
            _config = Config.Load(configFile);

            BApi.isTestEnv = false;
            SignUtility.accessKeyId = _config.accessKeyId;
            SignUtility.accessKeySecret = _config.accessKeySecret;
        }

        /// <summary>
        /// 释放
        /// </summary>
        protected override async void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!string.IsNullOrEmpty(gameId))
                {
                    Console.WriteLine($"正在关闭弹幕互动游戏, gameID:{gameId}");
                    m_PlayHeartBeat.Stop();
                    m_PlayHeartBeat.Dispose();

                    m_WebSocketBLiveClient.OnDanmaku -= OnDanmaku;
                    m_WebSocketBLiveClient.OnGift -= OnGift;
                    m_WebSocketBLiveClient.Disconnect();
                    m_WebSocketBLiveClient.Dispose();
                    await bApiClient.EndInteractivePlay(_config.appId, gameId!);
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 处理指令
        /// </summary>
        /// <param name="args"></param>
        void BLiveCommand(CommandArgs args)
        {
            #region 帮助
            void Help()
            {
                List<string> lines = new()
                {
                    "/blive status, 查看状态",
                    "/blive on, 开启弹幕功能",
                    "/blive off, 关闭弹幕功能",
                };

                if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out int pageNumber)) return;
                PaginationTools.SendPage(args.Player, pageNumber, lines, new PaginationTools.Settings
                {
                    HeaderFormat = "/blive 指令用法 ({0}/{1}):",
                    FooterFormat = "输入 /blive help {{0}} 查看更多".SFormat(Commands.Specifier)
                });
            }
            #endregion

            if (args.Parameters.Count == 0)
            {
                Help();
                return;
            }

            string text;
            switch (args.Parameters[0].ToLowerInvariant())
            {
                // 帮助
                default:
                case "help":
                case "h":
                    Help();
                    return;

                // 显示状态
                case "status":
                    if (ErrorCode != 0)
                    {
                        var text2 = ErrorCodeMapping.GetDesc(ErrorCode);
                        if (!string.IsNullOrEmpty(text2))
                            text2 = $"错误码提示：{text2}";
                        text = $"未正常开启, gameID:{gameId}, code:{ErrorCode} {text2}";
                        TShock.Log.ConsoleInfo(text);
                        args.Player.SendInfoMessage(text);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(gameId))
                        {
                            text = "弹幕互动还没开启";
                            args.Player.SendInfoMessage(text);
                        }
                        else
                        {
                            text = $"已开启, gameID:{gameId}, code:{ErrorCode}";
                            TShock.Log.ConsoleInfo(text);
                            args.Player.SendInfoMessage(text);
                        }
                    }
                    break;

                case "on":
                    Start(args);
                    break;

                case "off":
                    Off(args);
                    break;
            }
        }

        async void Start(CommandArgs args)
        {
            if (isOpen)
            {
                TShock.Log.ConsoleInfo($"弹幕功能已开启 gameID:{gameId}");
                args.Player.SendErrorMessage($"弹幕功能已开启 gameID:{gameId}");
                return;
            }

            args.Player.SendInfoMessage($"正在开启……");

            //开启互动玩法
            startInfo = await bApiClient.StartInteractivePlay(_config.code, _config.appId);
            //确认游戏正常开启
            if (startInfo?.Code != 0)
            {
                //处理未开启case
                ErrorCode = (int)startInfo?.Code!;
                TShock.Log.ConsoleInfo($"弹幕互动游戏未正常开启 (code: {startInfo?.Code})");
                return;
            }
            else
            {
                TShock.Log.ConsoleInfo($"弹幕互动游戏已正常开启 (code: {startInfo?.Code})");
                args.Player.SendSuccessMessage($"已开启！;-）");
            }

            isOpen = true;
            gameId = startInfo?.Data?.GameInfo?.GameId!;

            //获取到场次id后，开启心跳轮询，默认20秒一次
            m_PlayHeartBeat = new InteractivePlayHeartBeat(gameId!);
            //心跳异常case
            m_PlayHeartBeat.HeartBeatError += (e) =>
            {
                TShock.Log.ConsoleInfo("心跳包发送失败");
            };
            //正常心跳
            /*m_PlayHeartBeat.HeartBeatSucceed += () =>
            {
                TShock.Log.Info("已发送心跳包");
            };*/
            m_PlayHeartBeat.Start();

            //创建websocket客户端
            m_WebSocketBLiveClient = new WebSocketBLiveClient(startInfo!.GetWssLink(), startInfo.GetAuthBody());
            m_WebSocketBLiveClient.OnDanmaku += OnDanmaku;
            m_WebSocketBLiveClient.OnGift += OnGift;
            //连接长链  需自己处理重连
            //m_WebSocketBLiveClient.Connect(); 
            //连接长链 带有自动重连
            m_WebSocketBLiveClient.Connect(TimeSpan.FromSeconds(20));
        }

        async void Off(CommandArgs args)
        {
            string text;
            if (isOpen)
            {
                isOpen = false;

                text = $"正在关闭弹幕互动游戏, gameID:{gameId}";
                TShock.Log.ConsoleError(text);
                args.Player.SendInfoMessage(text);

                m_PlayHeartBeat.Stop();
                m_PlayHeartBeat.Dispose();

                m_WebSocketBLiveClient.OnDanmaku -= OnDanmaku;
                m_WebSocketBLiveClient.OnGift -= OnGift;
                m_WebSocketBLiveClient.Disconnect();
                m_WebSocketBLiveClient.Dispose();

                await bApiClient.EndInteractivePlay(_config.appId, gameId!);
            }
            else
            {
                args.Player.SendInfoMessage($"已经是关闭状态, gameID:{gameId}");
            }

        }

        /// <summary>
        /// 绑定弹幕事件
        /// </summary>
        void OnDanmaku(Dm dm)
        {
            string userName = dm.userName;
            if (dm.fansMedalWearingStatus && dm.fansMedalLevel > 0)
            {
                userName = $"[{dm.fansMedalName}.lv{dm.fansMedalLevel}]{dm.userName}";
            }
            TShock.Utils.Broadcast($"{userName}: {dm.msg}", Color.Honeydew);
            TShock.Log.ConsoleInfo($"{userName}: {dm.msg} / {dm}");
        }

        //绑定礼物信息
        void OnGift(SendGift gift)
        {
            TShock.Log.ConsoleInfo($"[{Name}]: {gift.userName} 赠送了 {gift.giftName}*{gift.giftNum}");
            TShock.Utils.Broadcast($"{gift.userName} 赠送了 {gift.giftName}*{gift.giftNum}", Color.YellowGreen);

            // 读取配置文件，匹配到礼物就执行指定指令
            foreach (var data in _config.gifts)
            {
                if (data.giftName == gift.giftName && gift.giftNum >= data.giftNum)
                {
                    // 合并 data.cmds  和 data.cmd
                    List<string> cmds = data.cmds.ToList();
                    cmds.Insert(0, data.cmd);
                    foreach (var cmd in data.cmds)
                    {
                        if (!string.IsNullOrEmpty(cmd))
                        {
                            ExecuteCommand(cmd);
                        }
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="command"></param>
        void ExecuteCommand(string command)
        {
            try
            {
                List<TSPlayer> onlinePlayer = new();

                for (int i = 0; i < TShock.Players.Length; i++)
                {
                    if (TShock.Players[i]?.Active ?? false)
                    {
                        onlinePlayer.Add(TShock.Players[i]);
                    }
                }

                if (onlinePlayer.Count == 0)
                {
                    Console.WriteLine("当前服务器无玩家");
                    return;
                }
                TSPlayer player = onlinePlayer[new Random().Next(0, onlinePlayer.Count + 1)];
                player.tempGroup = new SuperAdminGroup();
                Commands.HandleCommand(player, command);
                player.tempGroup = null;
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleInfo($"执行命令时出错: {ex}");
            }
        }
    }
}