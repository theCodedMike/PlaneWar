using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "GoodsContainer.asset", menuName = "GoodsCreator")]
    public class GoodsContainer : ScriptableObject
    {
        public List<GoodsInfo> buildInBagList; // 背包内内置的物品，使用后不能删减，是为了方便用户切换会默认的物品

        public List<GoodsInfo> mallList; // 商城内的物品

        private static GoodsContainer instance;

        public static GoodsContainer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<GoodsContainer>("GoodsContainer");
                }
                return instance;
            }
        }

        private GoodsContainer() { }

        public static void Init()
        {
            if (instance == null)
            {
                instance = Resources.Load<GoodsContainer>("GoodsContainer");
            }
        }

        // 查找背包内内置预制体的路径
        public string GetBuildInPrefabPath(string uniqueName)
        {
            foreach (GoodsInfo info in buildInBagList)
            {
                if (info.uniqueName == uniqueName)
                {
                    return info.prefabPath;
                }
            }

            throw new ArgumentException($"物品的名字错误，无法找到其预制体的路径: {uniqueName}");
        }

        // 查找背包内内置icon的路径
        public string GetBuildInIconPath(string uniqueName)
        {
            foreach (GoodsInfo info in buildInBagList)
            {
                if (info.uniqueName == uniqueName)
                {
                    return info.iconPath;
                }
            }

            throw new ArgumentException($"物品的名字错误，无法找到其精灵图的路径: {uniqueName}");
        }

        // 查找商城内预制体的路径
        public string GetMallPrefabPath(string uniqueName)
        {
            foreach (GoodsInfo info in mallList)
            {
                if (info.uniqueName == uniqueName)
                {
                    return info.prefabPath;
                }
            }

            throw new ArgumentException($"物品的名字错误，无法找到其预制体的路径: {uniqueName}");
        }

        // 查找商城内icon的路径
        public string GetMallIconPath(string uniqueName)
        {
            foreach (GoodsInfo info in mallList)
            {
                if (info.uniqueName == uniqueName)
                {
                    return info.iconPath;
                }
            }

            throw new ArgumentException($"物品的名字错误，无法找到其精灵图的路径: {uniqueName}");
        }
    }
}
