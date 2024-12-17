using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Panel
{
    public class BuyPanel : BasePanel
    {
        [Header("关闭按钮")]
        public Button close;
        [Header("商品Icon")]
        public Image icon;
        [Header("所需金币")]
        public TMP_Text gold;
        [Header("所需钻石")]
        public TMP_Text diamond;
        [Header("购买数量")]
        public TMP_InputField count;
        [Header("提示")]
        public TMP_Text tip;
        [Header("购买按钮")]
        public Button buy;


        public static event Action buyEvent; // 购买事件，需要花费金币和钻石

        private int goldFromMall; // 选中商品所需的金币/个
        private int diamondFromMall; // 选中商品所需的钻石/个
        private string itemId; // 商品ID


        private int intCount; // 购买数量

        private AudioSource audioSource;

        private void OnEnable()
        {
            close.onClick.AddListener(OnCloseBtnClick);
            buy.onClick.AddListener(OnBuyBtnClick);
            count.onValueChanged.AddListener(OnCountValueChanged);
        }

        // Start is called before the first frame update
        private void Start()
        {
            Clear();
            audioSource = GetComponent<AudioSource>();
        }

        public void SetItem(Item item)
        {
            itemId = new string(item.id);
            goldFromMall = (int)item.gold;
            diamondFromMall = (int)item.diamond;
            icon.sprite = Resources.Load<Sprite>(item.iconPath);
            gold.text = $"{goldFromMall}";
            diamond.text = $"{diamondFromMall}";
        }

        // 处理购买数量
        // ReSharper disable once MethodTooLong
        private void OnCountValueChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                tip.text = "购买数量至少为1";
                Invoke(nameof(Clear), 1);
                return;
            }

            if (value.StartsWith('-'))
            {
                tip.text = "购买数量不能小于0";
                Invoke(nameof(Clear), 1);
                return;
            }

            int val = int.Parse(value);
            if (val == 0)
            {
                tip.text = "购买数量至少为1";
                Invoke(nameof(Clear), 1);
                return;
            }
            if (val > 100)
            {
                tip.text = "购买数量过多(应小于100)";
                Invoke(nameof(Clear), 1);
                return;
            }

            intCount = val;
            SetBuyBtnAvailable();
        }


        // 点击购买按钮
        private void OnBuyBtnClick()
        {
            PlayPressSound();

            var spendGold = goldFromMall * intCount;
            int surplusGold = SharedFieldUtils.GetGold();
            if (spendGold > surplusGold)
            {
                tip.text = "您当前的金币不足";
                Invoke(nameof(Clear), 1);
                return;
            }

            var spendDiamond = diamondFromMall * intCount;
            int surplusDiamond = SharedFieldUtils.GetDiamond();
            if (spendDiamond > surplusDiamond)
            {
                tip.text = "您当前的钻石不足";
                Invoke(nameof(Clear), 1);
                return;
            }

            SharedFieldUtils.SetGold(surplusGold - spendGold);
            SharedFieldUtils.SetDiamond(surplusDiamond - spendDiamond);
            buyEvent?.Invoke();

            // 将购买的商品放在MainPanel那里，这样进入BagPanel时可以把购买的商品带进去
            print($"BuyPanel: here ... ItemId = {itemId}, count = {intCount}");
            MainPanel.Instance.BuyGoods(itemId, intCount);

            Clear();
            UIManager.Instance.Pop();
        }

        // 点击关闭按钮
        private void OnCloseBtnClick()
        {
            PlayPressSound();

            Clear();

            UIManager.Instance.Pop();
        }


        private void PlayPressSound()
        {
            audioSource.Play();
        }

        private void Clear()
        {
            tip.text = "";
            count.text = "";
            SetBuyBtnUnavailable();
            intCount = 0;
        }

        private void SetBuyBtnUnavailable()
        {
            buy.interactable = false;
        }

        private void SetBuyBtnAvailable()
        {
            buy.interactable = true;
        }

        private void OnDisable()
        {
            close.onClick.RemoveAllListeners();
            buy.onClick.RemoveAllListeners();
            count.onValueChanged.RemoveAllListeners();

            Clear();
        }
    }
}
