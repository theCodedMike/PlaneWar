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
        [Header("播放按钮")]
        public Button play;


        public Action<MallItemView, bool> OnMallItemSelected;

        private Item item;

        private AudioSource audioSource;

        private void OnEnable()
        {
            buy.onClick.AddListener(OnBuyButtonClick);
            play.onClick.AddListener(OnPlayBtnClick);

            audioSource = GetComponent<AudioSource>();
        }

        private void OnPlayBtnClick()
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            else
            {
                AudioClip clip = Resources.Load<AudioClip>(item.prefabPath);
                audioSource.PlayOneShot(clip);
            }
        }

        public void SetItem(Item item)
        {
            this.item = item ?? throw new NullReferenceException("item is null");

            Refresh();
        }

        private void Refresh()
        {
            itemIcon.sprite = Resources.Load<Sprite>(this.item.iconPath);
            itemName.text = item.name;
            gold.text = $"{item.gold}";
            diamond.text = $"{item.diamond}";
            play.gameObject.SetActive(item.category == GoodsType.BgMusic);
        }

        // 购买按钮被点击后
        private void OnBuyButtonClick()
        {
            OnMallItemSelected(this, true);

            if (audioSource.isPlaying)
                audioSource.Stop();

            BuyPanel buyPanel = UIManager.Instance.Push(PanelType.Buy) as BuyPanel;
            buyPanel.SetItem(item);
        }

        // Item被点击后
        public void OnPointerClick(PointerEventData eventData)
        {
            OnMallItemSelected(this, true);
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
            play.onClick.RemoveAllListeners();
        }
    }
}
