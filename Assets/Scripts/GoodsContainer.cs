using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "GoodsContainer.asset", menuName = "GoodsCreator")]
    public class GoodsContainer : ScriptableObject
    {
        public List<GoodsInfo> goodsList;

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

        private GoodsContainer(){}

        public static void Init()
        {
            if (instance == null)
            {
                instance = Resources.Load<GoodsContainer>("GoodsContainer");
            }
        }

        // 查找预制体的路径
        public string GetPrefabPath(string uniqueName)
        {
            foreach (GoodsInfo info in goodsList)
            {
                if (info.uniqueName == uniqueName)
                {
                    return info.prefabPath;
                }
            }

            throw new ArgumentException("物品的名字错误，无法找到其预制体的路径");
        }

        // 查找icon的路径
        public string GetIconPath(string uniqueName)
        {
            foreach (GoodsInfo info in goodsList)
            {
                if (info.uniqueName == uniqueName)
                {
                    return info.iconPath;
                }
            }

            throw new ArgumentException("物品的名字错误，无法找到其精灵图的路径");
        }
    }


}
