namespace UI
{
    public class Item
    {
        public string id; // ID,唯一
        public string name; // 名字
        public GoodsType category; // 分类
        public uint series; // 如果是子弹类型，则是几连发
        public UseType useType; // 使用类型
        public bool isBuildIn; // 是否是内置商品
        public string iconPath;  // icon路径
        public string prefabPath; // 预制体路径
        public string desc; // 描述
        public uint gold; // 所需金币
        public uint diamond; // 所需钻石
        public uint count; // 个数
    }
}
