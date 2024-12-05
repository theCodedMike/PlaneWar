namespace UI
{
    [System.Serializable]
    public class PanelInfo
    {
        public PanelType type; // 类型
        public string path; // 预制体路径
    }

    // Panal类型
    public enum PanelType
    {
        Login,  // 登录页面
        Main,   // 主页面
        Setting,// 设置页面
        Bag,    // 背包页面
        Mall,   // 商城页面
    }
}
