using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class GoodsInfo
    {
        public string uniqueName; // 名字，需唯一
        public string shortName; // 名字，可以是简写
        public GoodsType type; // 商品类型
        public uint series; // 如果是子弹类型，需要记录是几连发
        public UseType useType; // 使用类型
        public bool isBuildIn; // 是否为默认内置的
        public string iconPath; // 图标路径
        public string prefabPath; // 预制体路径
        public string desc; // 描述
        public uint spendGold; // 需花费的金币
        public uint spendDiamond; // 需花费的钻石
    }

    [System.Serializable]
    public enum GoodsType
    {
        BgImage, // 背景图片
        BgMusic, // 背景音乐
        Airplane, // 飞机
        Bullet, // 子弹
        DaZhao, // 大招
        Explosion, // 爆炸特效
        Halo, // 飞机光环
    }

    [System.Serializable]
    public enum UseType
    {
        Player, // 由我方使用
        Enemy,  // 由敌方使用
        All,    // 二者皆可
    }

    // 
}
