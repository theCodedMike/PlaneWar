using System;
using TMPro;
using UI.Panel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class MallItemView : MonoBehaviour, IPointerClickHandler
    {
        [Header("Item背景图片")]
        public Image itemBgIcon;
        [Header("商品图片")]
        public Image itemIcon;
        [Header("商品名称")]
        public TMP_Text itemName;
        [Header("所需金币")]
        public TMP_Text gold;
        [Header("所需钻石")]
        public TMP_Text diamond;
        [Header("购买按钮")]
        public Button buy;

        public Action<MallItemView, bool> OnMallItemSelected;

        private Item item;

        private void OnEnable()
        {
            buy.onClick.AddListener(OnBuyButtonClick);
        }

        public void SetItem(Item item)
        {
            this.item = item;
            Refresh();
        }

        private void Refresh()
        {
            this.itemIcon.sprite = Resources.Load<Sprite>(this.item.iconPath);
            this.itemName.text = this.item.id;
            this.gold.text = $"{this.item.gold}";
            this.diamond.text = $"{this.item.diamond}";
        }

        // 购买按钮被点击后
        private void OnBuyButtonClick()
        {
            OnMallItemSelected(this, false);

            BuyPanel buyPanel = UIManager.Instance.Push(PanelType.Buy) as BuyPanel;
            buyPanel.SetItem(item);
        }

        // Item被点击后
        public void OnPointerClick(PointerEventData eventData)
        {
            OnMallItemSelected(this, false);
        }

        // Item被点击后，改变背景图片的颜色
        public void ChangeBg()
        {
            this.itemBgIcon.color = new Color32(101, 130, 154, 255);
        }

        // 恢复背景图片的颜色
        public void RestoreBg()
        {
            this.itemBgIcon.color = new Color32(64, 53, 76, 255);
        }

        private void OnDisable()
        {
            buy.onClick.RemoveAllListeners();
        }
    }
}
