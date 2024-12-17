using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.Panel
{
    public class SettingPanel : BasePanel, IPointerClickHandler
    {
        [Header("右上角的头像")]
        public Image bigAvatar;
        [Header("关闭按钮")]
        public Button closeBtn;
        [Header("头像文字后面的头像")]
        public Image smallAvatar;
        [Header("用户名")]
        public TMP_Text username;
        [Header("密码")]
        public TMP_Text password;
        [Header("背景音乐音量")]
        public Slider bgmVolume;
        [Header("背景音乐音量值")]
        public TMP_Text bgmVolumeValue;
        [Header("金币")]
        public TMP_Text gold;
        [Header("钻石")]
        public TMP_Text diamond;
        [Header("点击充值按钮")]
        public Button rechargeBtn;

        public static event Action bgmVolumeChangeEvent; // 背景音乐音量改变

        private AudioSource audioSource;
        private bool isPwdCipher; // 密码以密文显示

        private void OnEnable()
        {
            closeBtn.onClick.AddListener(OnCloseBtnClick);
            bgmVolume.onValueChanged.AddListener(OnBgmVolumeValueChanged);
            rechargeBtn.onClick.AddListener(OnRechargeBtnClick);

            RechargePanel.rechargeEvent += GetMoneyInfo;
            BuyPanel.buyEvent += GetMoneyInfo;
        }


        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            Init();
        }

        void Init()
        {
            Sprite avatar = SharedFieldUtils.GetAvatar();
            bigAvatar.sprite = avatar;
            smallAvatar.sprite = avatar;

            username.text = SharedFieldUtils.GetUsername();
            password.text = new string('*', SharedFieldUtils.GetPassword().Length);
            isPwdCipher = true; // 默认以密文显示

            bgmVolume.value = SharedFieldUtils.GetBgmVolume();
            bgmVolumeValue.text = $"{SharedFieldUtils.GetBgmVolume()}";

            GetMoneyInfo();
        }

        private void OnBgmVolumeValueChanged(float value)
        {
            int volume = (int)value;
            SharedFieldUtils.SetBgmVolume(volume);
            bgmVolumeValue.text = $"{volume}";

            bgmVolumeChangeEvent?.Invoke();
        }

        // 点击"点击充值"按钮
        private void OnRechargeBtnClick()
        {
            PlayBtnPressSound();

            UIManager.Instance.Push(PanelType.Recharge);
        }

        // 点击关闭按钮
        private void OnCloseBtnClick()
        {
            PlayBtnPressSound();

            UIManager.Instance.Pop();
            UIManager.Instance.Push(PanelType.Main);
        }

        // 处理密码以明文或密文显示事件
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.pointerEnter.name != "Eye")
                return;

            PlayBtnPressSound();
            if (isPwdCipher)
            {
                isPwdCipher = false;
                password.text = SharedFieldUtils.GetPassword();
            }
            else
            {
                isPwdCipher = true;
                password.text = "********";
            }
        }


        void GetMoneyInfo()
        {
            gold.text = $"{SharedFieldUtils.GetGold()}";
            diamond.text = $"{SharedFieldUtils.GetDiamond()}";
        }


        void PlayBtnPressSound()
        {
            audioSource.Play();
        }

        private void OnDisable()
        {
            closeBtn.onClick.RemoveAllListeners();
            bgmVolume.onValueChanged.RemoveAllListeners();
            rechargeBtn.onClick.RemoveAllListeners();


            RechargePanel.rechargeEvent -= GetMoneyInfo;
            BuyPanel.buyEvent -= GetMoneyInfo;
        }
    }
}
