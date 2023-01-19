namespace PluginData
{
    public enum Boss
    {
        其他 = 0,
        史莱姆王 = 50,
        克苏鲁之眼 = 4,
        蜂后 = 222,
        史莱姆女王 = 657,
        双子魔眼 = 126,
        毁灭者 = 134,
        机械骷髅王 = 127,
        机械三王 = 1,
        光之女皇 = 636,
        月亮领主 = 398
    }
    public class Gift
    {
        public static Boss GetBoss(string gift) => gift switch
        {
            "古董八音盒" => new Random().Next(0, 2) == 0 ? Boss.史莱姆王 : Boss.克苏鲁之眼,
            "花式夸夸" => new Random().Next(0, 2) == 0 ? Boss.蜂后 : Boss.史莱姆女王,
            "奥库瑞姆剃刀" => new Random().Next(0, 4) switch
            {
                0 => Boss.双子魔眼,
                1 => Boss.毁灭者,
                2 => Boss.机械骷髅王,
                3 => Boss.机械三王,
                _ => Boss.其他
            },
            "七彩蝶蛉" => Boss.光之女皇,
            "星愿水晶球" => Boss.月亮领主,
            _ => Boss.其他
        };
    }
}