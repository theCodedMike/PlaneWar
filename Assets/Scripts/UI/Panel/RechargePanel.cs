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

            if (gold > 10000 || diamond > 10000)
            {
                tipText.text = "您充值过多";
                Invoke(nameof(Clear), 2f);
                return;
            }

            SharedFieldUtils.SetGold(SharedFieldUtils.GetGold() + gold);
            SharedFieldUtils.SetDiamond(SharedFieldUtils.GetDiamond() + diamond);
            tipText.text = "充值成功";

            Clear();
            UIManager.Instance.Pop(); // 跳转到设置页面
            UIManager.Instance.Push(PanelType.Setting);
        }



        private void OnDiamondValueChanged(string value)
        {
            diamond = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);

            HandleInputEvent();
        }

        private void OnGoldInputValueChanged(string value)
        {
            gold = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);

            HandleInputEvent();
        }

        void HandleInputEvent()
        {
            if (gold == 0 && diamond == 0)
                SetRechargeBtnUnavailable();
            else
                SetRechargeBtnAvailable();
        }
        private void OnCloseBtnClick()
        {
            PlayBtnPressSound();

            Clear();

            UIManager.Instance.Pop();
            UIManager.Instance.Push(PanelType.Setting);
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
