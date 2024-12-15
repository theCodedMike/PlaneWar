using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Panel
{
    public class MainPanel : BasePanel
    {
        [Header("开始游戏按钮")]
        public Button startBtn;
        [Header("继续游戏按钮")]
        public Button continueBtn;
        [Header("背包按钮")]
        public Button bagBtn;
        [Header("商城按钮")]
        public Button mallBtn;
        [Header("设置按钮")]
        public Button settingBtn;
        [Header("金币")]
        public TMP_Text gold;
        [Header("钻石")]
        public TMP_Text diamond;

        private RectTransform startRT;
        private AudioSource audioSource;

        private void OnEnable()
        {
            startBtn.onClick.AddListener(OnStartBtnClick);
            continueBtn.onClick.AddListener(OnContinueBtnClick);
            bagBtn.onClick.AddListener(OnBagBtnClick);
            mallBtn.onClick.AddListener(OnMallBtnClick);
            settingBtn.onClick.AddListener(OnSettingBtnClick);
        }

        // Start is called before the first frame update
        void Start()
        {
            startRT = startBtn.GetComponent<RectTransform>();
            audioSource = GetComponent<AudioSource>();
        }

        // "开始游戏"按钮被按下
        private void OnStartBtnClick()
        {
            PlayBtnPressSound();
        }

        // "继续游戏"按钮被按下
        private void OnContinueBtnClick()
        {
            PlayBtnPressSound();
        }

        // "背包"按钮被按下
        private void OnBagBtnClick()
        {
            PlayBtnPressSound();
        }

        // "商城"按钮被按下
        private void OnMallBtnClick()
        {
            PlayBtnPressSound();

            UIManager.Instance.Pop();
            UIManager.Instance.Push(PanelType.Mall);
        }

        // "设置"按钮被按下
        private void OnSettingBtnClick()
        {
            PlayBtnPressSound();

            UIManager.Instance.Pop();
            UIManager.Instance.Push(PanelType.Setting);
        }

        private void PlayBtnPressSound()
        {
            audioSource.Play();
        }

        // Update is called once per frame
        void Update()
        {
            AdjustStartAndContinueBtn();
            UpdateGoldAndDiamond();
        }

        // 更新用户的金币信息
        void UpdateGoldAndDiamond()
        {
            gold.text = $"{SharedFieldUtils.GetGold()}";
            diamond.text = $"{SharedFieldUtils.GetDiamond()}";
        }

        // 调整"开始游戏"和"继续游戏"按钮的显现
        void AdjustStartAndContinueBtn()
        {
            bool isGamePause = SharedFieldUtils.IsGamePause();

            if (isGamePause)
            {   // 游戏暂停
                startRT.localPosition = new Vector3(-180, 0, 0);
                continueBtn.gameObject.SetActive(true);
            }
            else
            {   // 游戏没有暂停
                continueBtn.gameObject.SetActive(false);
                startRT.localPosition = new Vector3(0, 0, 0);
            }
        }


        private void OnDisable()
        {
            startBtn.onClick.RemoveAllListeners();
            continueBtn.onClick.RemoveAllListeners();
            bagBtn.onClick.RemoveAllListeners();
            mallBtn.onClick.RemoveAllListeners();
            settingBtn.onClick.RemoveAllListeners();
        }
    }
}
