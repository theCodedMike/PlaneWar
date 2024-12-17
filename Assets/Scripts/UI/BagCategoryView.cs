using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class BagCategoryView : MonoBehaviour, IPointerClickHandler
    {
        public Action<BagCategoryView> OnCategorySelected;

        private Image image;

        private void Awake()
        {
            image = transform.GetComponentInChildren<Image>();
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            OnCategorySelected(this);
        }

        // 改变背景
        public void ChangeBg()
        {
            image ??= transform.GetComponentInChildren<Image>();

            image.color = new Color32(73, 60, 179, 255);
        }

        // 恢复背景
        public void RestoreBg()
        {
            image ??= transform.GetComponentInChildren<Image>();

            image.color = new Color32(178, 117, 117, 255);
        }

        public GoodsType GetCategory()
        {

            string type = transform.GetComponentInChildren<TMP_Text>().text;
            return type switch
            {
                "背景图片" => GoodsType.BgImage,
                "背景音乐" => GoodsType.BgMusic,
                "飞机" => GoodsType.Airplane,
                "子弹" => GoodsType.Bullet,
                "大招外形" => GoodsType.DaZhao,
                "光环" => GoodsType.Halo,
                "爆炸特效" => GoodsType.Explosion,
                _ => throw new ArgumentException($"未知的分类：{type}"),
            };
        }
    }
}
