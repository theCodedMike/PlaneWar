using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BagItemInfoView : MonoBehaviour
    {
        [Header("商品Icon")]
        public Image itemIcon;
        [Header("名字")]
        public TMP_Text itemName;
        [Header("商品描述")]
        public TMP_Text desc;
        [Header("使用按钮")]
        public Button useBtn;

        public Action<BagItemView, BagItemInfoView> ClickUseBtnEvent;

        private BagItemView itemView;


        private void OnEnable()
        {
            useBtn.onClick.AddListener(OnUseBtnClick);
        }

        public void SetItemInfo(BagItemView itemView)
        {
            if (itemView is null)
            {
                Clear();
                return;
            }

            this.itemView = itemView;
            Item item = itemView.GetItem();
            itemIcon.sprite = Resources.Load<Sprite>(item.iconPath);
            itemIcon.color = new Color32(255, 255, 255, 255);
            itemName.text = item.name;
            desc.text = item.desc;
            SetUseBtnAvailable();
        }

        private void OnUseBtnClick()
        {
            ClickUseBtnEvent(itemView, this);
        }

        private void Clear()
        {
            itemIcon.sprite = null;
            itemIcon.color = Color.clear;
            itemName.text = "";
            desc.text = "";
            SetUseBtnUnavailable();
        }

        private void SetUseBtnAvailable()
        {
            useBtn.gameObject.SetActive(true);
        }

        private void SetUseBtnUnavailable()
        {
            useBtn.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            useBtn.onClick.RemoveAllListeners();
        }
    }
}
