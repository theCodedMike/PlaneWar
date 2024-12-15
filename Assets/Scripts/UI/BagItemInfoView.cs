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

        public Action<Item> ClickUseBtnEvent;

        private Item item;


        private void OnEnable()
        {
            useBtn.onClick.AddListener(OnUseBtnClick);
        }

        public void SetItemInfo(Item item)
        {
            if (item is null)
            {
                Clear();
                return;
            }

            this.item = item;
            itemIcon.sprite = Resources.Load<Sprite>(item.iconPath);
            itemIcon.color = new Color32(255, 255, 255, 255);
            itemName.text = item.name;
            desc.text = item.desc;
            SetUseBtnAvailable();
        }

        private void OnUseBtnClick()
        {
            ClickUseBtnEvent(item);
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
