using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Panel
{
    public class RechargePanel : BasePanel
    {
        [Header("关闭按钮")]
        public Button closeBtn;
        [Header("金币输入框")]
        public TMP_InputField goldInput;
        [Header("钻石输入框")]
        public TMP_InputField diamondInput;
        [Header("提示")]
        public TMP_Text tipText;
        [Header("充值按钮")]
        public Button rechargeBtn;

        private AudioSource audioSource;
        private int gold;
        private int diamond;

        private void OnEnable()
        {
            closeBtn.onClick.AddListener(OnCloseBtnClick);
            goldInput.onValueChanged.AddListener(OnGoldInputValueChanged);
            diamondInput.onValueChanged.AddListener(OnDiamondValueChanged);
            rechargeBtn.onClick.AddListener(OnRechargeBtnClick);
        }

        private void OnRechargeBtnClick()
        {
            PlayBtnPressSound();

            SharedFieldUtils.SetGold(SharedFieldUtils.GetGold() + gold);
            SharedFieldUtils.SetDiamond(SharedFieldUtils.GetDiamond() + diamond);
            tipText.text = "充值成功";

            Clear();
            UIManager.Instance.Pop(); // 跳转到设置页面
        }



        private void OnDiamondValueChanged(string value) => HandleInputEvent(value, false);



        private void OnGoldInputValueChanged(string value) => HandleInputEvent(value, true);


        // ReSharper disable once MethodTooLong
        private void HandleInputEvent(string value, bool isGold)
        {
            if (value.StartsWith('-'))
            {
                tipText.text = "充值不能小于0";
                Invoke(nameof(Clear), 1f);
                return;
            }

            int val = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
            if (isGold)
                gold = val;
            else
                diamond = val;

            if (gold == 0 && diamond == 0)
            {
                tipText.text = "充值金额必须大于0";
                Invoke(nameof(Clear), 1f);
                return;
            }

            if (gold > 1000 || diamond > 1000)
            {
                tipText.text = "充值金额过大(不超过1000)";
                Invoke(nameof(Clear), 1f);
                return;
            }

            SetRechargeBtnAvailable();
        }

        // 关闭panel
        private void OnCloseBtnClick()
        {
            PlayBtnPressSound();

            Clear();

            UIManager.Instance.Pop();
        }

        // Start is called before the first frame update
        void Start()
        {
            Clear();
            audioSource = GetComponent<AudioSource>();
        }

        void Clear()
        {
            gold = 0;
            diamond = 0;
            goldInput.text = "";
            diamondInput.text = "";
            tipText.text = "";
            SetRechargeBtnUnavailable();
        }

        void SetRechargeBtnUnavailable()
        {
            rechargeBtn.interactable = false;
        }
        void SetRechargeBtnAvailable()
        {
            rechargeBtn.interactable = true;
        }

        void PlayBtnPressSound()
        {
            audioSource.Play();
        }

        private void OnDisable()
        {
            closeBtn.onClick.RemoveAllListeners();
            goldInput.onValueChanged.RemoveAllListeners();
            diamondInput.onValueChanged.RemoveAllListeners();
            rechargeBtn.onClick.RemoveAllListeners();

            Clear();
        }
    }
}
