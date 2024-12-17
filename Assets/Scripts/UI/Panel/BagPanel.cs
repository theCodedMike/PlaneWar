using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Utils;
using Button = UnityEngine.UI.Button;

namespace UI.Panel
{
    public class BagPanel : BasePanel
    {
        [Header("返回按钮")]
        public Button returnBtn;
        [Header("分类Content")]
        public Transform categoryContent;
        [Header("使用类型Content")]
        public Transform useTypeContent;
        [Header("Content")]
        public Transform itemsContent;
        [Header("剩余金币")]
        public TMP_Text gold;
        [Header("剩余钻石")]
        public TMP_Text diamond;


        private GameObject bagItemPrefab; // 背包Item预制体

        // 处理不同分类切换时的背景
        private BagCategoryView lastCategoryView;
        private BagCategoryView currCategoryView;

        // 处理不同使用类型切换时的背景
        private BagUseTypeView lastUseTypeView;
        private BagUseTypeView currUseTypeView;

        // 处理不同Item切换时的背景
        private BagItemView lastItemView;
        private BagItemView currItemView;

        private Predicate<Item> filter;
        private UseType currUseType = UseType.All;
        private GoodsType currCategory = GoodsType.BgImage;

        private List<Item> bagItemList;
        private List<BagItemView> bagItemViewList; // 用于销毁item实例

        private BagItemInfoView infoView;

        private AudioSource audioSource;


        private void OnEnable()
        {
            returnBtn.onClick.AddListener(OnReturnBtnClick);
            RechargePanel.rechargeEvent += GetMoneyInfo;
            BuyPanel.buyEvent += GetMoneyInfo;

            Init();
        }

        private void Init()
        {
            bagItemPrefab = Resources.Load<GameObject>("Prefabs/BagItem");
            audioSource = GetComponent<AudioSource>();
            infoView = GetComponentInChildren<BagItemInfoView>();
            infoView.ClickUseBtnEvent = UseBagItem;

            // 选择不同的商品类型
            BagCategoryView[] categoryViews = categoryContent.GetComponentsInChildren<BagCategoryView>();
            foreach (var view in categoryViews)
                view.OnCategorySelected = OnCategorySelected;
            HandleCategoryBg(categoryViews[0]);

            // 选择不同的使用类型
            BagUseTypeView[] useTypeViews = useTypeContent.GetComponentsInChildren<BagUseTypeView>();
            foreach (var view in useTypeViews)
                view.OnUseTypeSelected = OnUseTypeSelected;
            HandleUseTypeBg(useTypeViews[0]);

            // 从数据库中读取数据
            bagItemList = GoodsContainer.Instance.buildInBagList.Select(info => info.ToItem()).ToList();
            bagItemViewList = new(16);
            print("bag count: " + bagItemList.Count);
        }

        // Start is called before the first frame update
        private void Start()
        {
            GetMoneyInfo();
        }

        // 添加从商城页面购买的物品
        // 这个函数在Start函数之前执行
        public void AddBuyItems(List<Item> buyItems)
        {
            print("bag panel addBuyItems...");

            /*if (buyItems is null || buyItems.Count == 0)
                return;*/
            if (buyItems is { Count: > 0 })
            {
                foreach (Item buyItem in buyItems)
                {
                    bool find = false;

                    foreach (Item bagItem in bagItemList)
                    {
                        if (buyItem.id == bagItem.id)
                        {
                            bagItem.count += buyItem.count;
                            find = true;
                            break;
                        }
                    }

                    if (!find)
                        bagItemList.Add(buyItem);
                }
            }

            Spawn();
        }


        // 返回到MainPanel页面
        private void OnReturnBtnClick()
        {
            PlayPressSound();

            UIManager.Instance.Pop();
            UIManager.Instance.Push(PanelType.Main);
        }

        // 获取玩家的金币和钻石
        private void GetMoneyInfo()
        {
            gold.text = $"{SharedFieldUtils.GetGold()}";
            diamond.text = $"{SharedFieldUtils.GetDiamond()}";
        }

        // 选择使用类型
        private void OnUseTypeSelected(BagUseTypeView useTypeView)
        {
            PlayPressSound();

            if (HandleUseTypeBg(useTypeView))
            {
                currUseType = currUseTypeView.GetUseType();

                Spawn();
            }
        }
        // 处理使用类型的背景，如果确实需要切换，返回true，否则返回false
        private bool HandleUseTypeBg(BagUseTypeView useTypeView)
        {
            if (useTypeView == currUseTypeView)
                return false;

            if (lastUseTypeView != null)
                lastUseTypeView.RestoreBg();
            currUseTypeView = useTypeView;
            currUseTypeView.ChangeBg();
            lastUseTypeView = currUseTypeView;

            return true;
        }

        // 选择商品类型
        private void OnCategorySelected(BagCategoryView categoryView)
        {
            PlayPressSound();

            if (HandleCategoryBg(categoryView))
            {
                currCategory = categoryView.GetCategory();

                Spawn();
            }
        }

        // 处理分类的背景，如果确实需要切换，返回true，否则返回false
        private bool HandleCategoryBg(BagCategoryView categoryView)
        {
            if (categoryView == currCategoryView)
                return false;

            if (lastCategoryView != null)
                lastCategoryView.RestoreBg();
            currCategoryView = categoryView;
            currCategoryView.ChangeBg();
            lastCategoryView = currCategoryView;

            return true;
        }

        // 生成item
        private void Spawn()
        {
            Clear();

            // ReSharper disable once ComplexConditionExpression
            filter = (Item item) => item.category == currCategory && (currUseType == UseType.All || item.useType == currUseType);
            List<Item> result = bagItemList.FindAll(filter).ToList();
            print("Bag Spawn: " + result.Count);

            if (result.Count > 0)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    GameObject itemObj = Instantiate(bagItemPrefab, itemsContent);
                    BagItemView itemView = itemObj.GetComponent<BagItemView>();

                    itemView.SetItem(result[i]);
                    itemView.OnBagItemSelected = OnBagItemSelected;

                    bagItemViewList.Add(itemView);

                    if (i == 0)
                        OnBagItemSelected(itemView, false);
                }
            }
            else
            {
                infoView.SetItemInfo(null);
            }
        }

        /// <summary>
        /// 选中某个背包Item，处理其背景色
        /// </summary>
        /// <param name="itemView">选中的Item</param>
        /// <param name="isClick">用来控制是否发出按键声音，只有是用户点击某个Item时才发出按键声音</param>
        private void OnBagItemSelected(BagItemView itemView, bool isClick)
        {
            if (isClick)
                PlayPressSound();

            if (itemView == currItemView)
                return;

            if (lastItemView != null)
                lastItemView.RestoreBg();
            currItemView = itemView;
            currItemView.ChangeBg();
            lastItemView = currItemView;

            // 初始化最下面的展示信息
            infoView.SetItemInfo(itemView);
        }

        /// <summary>
        /// 点击"使用"按钮后，物品数量减1，如果数量变0，则删除
        /// </summary>
        /// <param name="item"></param>
        private void UseBagItem(BagItemView itemView, BagItemInfoView infoView)
        {
            if (itemView is null)
                throw new NullReferenceException("itemView is null");
            if (infoView is null)
                throw new NullReferenceException("infoView is null");

            PlayPressSound();

            // 处理视图
            Item item = itemView.GetItem();
            if (!item.isBuildIn) // 如果是内置的商品，不需要扣减数量
            {
                item.count--;
                if (item.count == 0)
                {
                    bagItemList.Remove(item);
                    infoView.SetItemInfo(null);

                    bagItemViewList.Remove(itemView);
                    Destroy(itemView.gameObject);
                    if (bagItemViewList.Count > 0)
                        OnBagItemSelected(bagItemViewList[0], false);
                }
                else
                {
                    itemView.UpdateCount();
                }
            }

            // TODO: 执行真正的逻辑
        }


        // 销毁已生成的GameObject
        private void Clear()
        {
            foreach (var itemView in bagItemViewList)
            {
                Destroy(itemView.gameObject);
            }
            bagItemViewList.Clear();
        }

        private void PlayPressSound()
        {
            audioSource.Play();
        }

        private void OnDisable()
        {
            returnBtn.onClick.RemoveAllListeners();
            RechargePanel.rechargeEvent -= GetMoneyInfo;
            BuyPanel.buyEvent -= GetMoneyInfo;
        }
    }
}
