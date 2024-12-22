using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [Header("返回按钮")]
        public Button returnBtn;

        private RectTransform startRT;
        private AudioSource audioSource;

        public static MainPanel Instance;
        private Dictionary<string, int> buyItemMap;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            startBtn.onClick.AddListener(OnStartBtnClick);
            continueBtn.onClick.AddListener(OnContinueBtnClick);
            bagBtn.onClick.AddListener(OnBagBtnClick);
            mallBtn.onClick.AddListener(OnMallBtnClick);
            settingBtn.onClick.AddListener(OnSettingBtnClick);
            returnBtn.onClick.AddListener(OnReturnBtnClick);

            RechargePanel.rechargeEvent += GetMoneyInfo;
            BuyPanel.buyEvent += GetMoneyInfo;
        }

        // Start is called before the first frame update
        void Start()
        {
            buyItemMap = new(16);
            startRT = startBtn.GetComponent<RectTransform>();
            audioSource = GetComponent<AudioSource>();

            GetMoneyInfo();
        }

        // 返回到登录页面
        private void OnReturnBtnClick()
        {
            PlayBtnPressSound();

            UIManager.Instance.Pop();
            UIManager.Instance.Push(PanelType.Login);
        }


        // "开始游戏"按钮被按下
        private void OnStartBtnClick()
        {
            PlayBtnPressSound();

            SharedFieldUtils.SetIsGamePause(false);
            //Camera.main.GetComponent<AudioListener>().enabled = false;
            UIManager.Instance.Pop();

            StartCoroutine(LoadGameSceneAsync());
        }

        IEnumerator LoadGameSceneAsync()
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Game");
            while (!asyncOperation.isDone)
            {
                yield return null;
            }

            //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
            //UIManager.Instance.canvas.SetActive(false);
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

            UIManager.Instance.Pop();
            BagPanel bagPanel = UIManager.Instance.Push(PanelType.Bag) as BagPanel;
            bagPanel!.AddBuyItems(GetBuyItems());
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
        }

        // 更新用户的金币信息
        void GetMoneyInfo()
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

        // 购买的商品存在这里
        public void BuyGoods(string id, int count)
        {
            string key = new string(id);
            buyItemMap.Add(key, buyItemMap.GetValueOrDefault(key, 0) + count);
        }

        // 获取购买的商品
        List<Item> GetBuyItems()
        {
            if (buyItemMap.Count == 0)
                return null;

            List<Item> items = new List<Item>(16);
            foreach (var pair in buyItemMap)
            {
                GoodsInfo goods = GoodsContainer.Instance.mallList.Find(info => info.uniqueName == pair.Key);
                if (goods is null)
                    throw new ArgumentException($"Cannot find this goods: {pair.Key}");

                Item item = goods.ToItem((uint)pair.Value);
                items.Add(item);
            }

            buyItemMap.Clear();
            return items;
        }

        private void OnDisable()
        {
            startBtn.onClick.RemoveAllListeners();
            continueBtn.onClick.RemoveAllListeners();
            bagBtn.onClick.RemoveAllListeners();
            mallBtn.onClick.RemoveAllListeners();
            settingBtn.onClick.RemoveAllListeners();
            returnBtn.onClick.RemoveAllListeners();

            RechargePanel.rechargeEvent -= GetMoneyInfo;
            BuyPanel.buyEvent -= GetMoneyInfo;
        }
    }
}
