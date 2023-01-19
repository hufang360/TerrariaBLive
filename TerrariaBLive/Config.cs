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


        /// <summary>
        /// 加载配置文件
        /// </summary>
        public static Config Load(string path)
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
            }
            else
            {
                var c = new Config();
                File.WriteAllText(path, JsonConvert.SerializeObject(c, Formatting.Indented));
                return c;
            }
        }
    }



    /// <summary>
    /// wiki语言包文件
    /// </summary>
    public class WikiJson
    {
        /// <summary>
        /// 物品名称
        /// </summary>
        public Dictionary<string, string> ItemName = new();

        /// <summary>
        /// 加载json文件
        /// </summary>
        public static WikiJson Load(string path)
        {
            return JsonConvert.DeserializeObject<WikiJson>(File.ReadAllText(path), new JsonSerializerSettings()
            {
                Error = (sender, error) => error.ErrorContext.Handled = true
            });
        }
    }

}