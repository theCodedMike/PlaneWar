using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class BagItemView : MonoBehaviour, IPointerClickHandler
    {
        [Header("背包Item背景icon")]
        public Image itemBgIcon;
        [Header("商品Icon")]
        public Image itemIcon;
        [Header("商品的个数")]
        public TMP_Text count;


        public Action<BagItemView, bool> OnBagItemSelected;

        private Item item;

        public void SetItem(Item item)
        {
            this.item = item;
            itemIcon.sprite = Resources.Load<Sprite>(item.iconPath);

            count.text = item.isBuildIn ? "" : $"{item.count}";
        }

        public void UpdateCount()
        {
            if (item is null || item.isBuildIn) return;

            count.text = $"{item.count}";
        }

        public Item GetItem() => item;

        // Item被点击后，改变背景图片的颜色
        public void ChangeBg()
        {
            this.itemBgIcon.color = new Color32(255, 255, 255, 255);
            this.itemBgIcon.sprite = Resources.Load<Sprite>("Icons/frame_char");
        }

        // 恢复背景图片的颜色
        public void RestoreBg()
        {
            this.itemBgIcon.color = new Color32(38, 64, 104, 255);
            this.itemBgIcon.sprite = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnBagItemSelected(this, true);
        }
    }
}
