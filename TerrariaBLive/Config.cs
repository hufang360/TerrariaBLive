using Newtonsoft.Json;

namespace TerrariaBLive
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class Config
    {
        public string code = "身份码：访问 https://play-live.bilibili.com/ 获取";
        public string appId = "项目ID：访问 https://open-live.bilibili.com/open-manage 创建项目";
        public string accessKeyId = "开发密钥：访问 https://open-live.bilibili.com/document/quickStart.html 进行申请";
        public string accessKeySecret = "开发密钥";

        public List<GiftData> gifts = new(){
            new GiftData("小花花",1,"/who","召唤史莱姆王"),
            new GiftData("花式夸夸",1,"/sm 222","召唤蜂后"),
            new GiftData("奥库瑞姆剃刀",1,new List<string>(){"/sm 126","/sm 127"},"召唤双子魔眼"),
            new GiftData("七彩蝶蛉",1,"/sm 636","召唤光之女皇"),
            new GiftData("星愿水晶球",1,"/sm 398","召唤月亮领主"),
        };

        /// <summary>
        /// 加载配置文件
        /// </summary>
        public static Config Load(string path)
        {
            if (File.Exists(path))
            {
                // 忽略json序列化错误
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path), new JsonSerializerSettings()
                {
                    Error = (sender, error) => error.ErrorContext.Handled = true
                });
            }
            else
            {
                var c = new Config();
                // 导出时忽略默认值字段
                File.WriteAllText(path, JsonConvert.SerializeObject(c, Formatting.Indented, new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                }));
                return c;
            }
        }
    }


    /// <summary>
    /// 礼物数据
    /// </summary>
    public class GiftData
    {
        /// <summary>
        /// 礼物
        /// </summary>
        public string giftName = "";

        /// <summary>
        /// 礼物数量
        /// </summary>
        public int giftNum = 1;

        /// <summary>
        /// 指令
        /// </summary>
        public string cmd = "";

        /// <summary>
        /// 多条指令（如果 cmds 和 cmd同时配置，将同时生效）
        /// </summary>
        public List<string> cmds = new();

        /// <summary>
        /// 指令
        /// </summary>
        public string comment = "";

        public GiftData(string _name, int _num, string _cmd, string _comment = "")
        {
            giftName = _name;
            giftNum = _num;
            cmd = _cmd;
            comment = _comment;
        }

        public GiftData(string _name, int _num, List<string> _cmds, string _comment = "")
        {
            giftName = _name;
            giftNum = _num;
            cmds = _cmds;
            comment = _comment;
        }
    }

}