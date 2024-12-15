using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Panel
{
    public class MallPanel : BasePanel
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

        private GameObject mallItemPrefab; // item预制体

        // 处理不同分类切换时的背景
        private MallCategoryView lastCategoryView;
        private MallCategoryView currCategoryView;

        // 处理不同使用类型切换时的背景
        private MallUseTypeView lastUseTypeView;
        private MallUseTypeView currUseTypeView;

        // 处理不同Item切换时的背景
        private MallItemView lastItemView;
        private MallItemView currItemView;

        private Predicate<Item> filter;
        private UseType currUseType = UseType.All;
        private GoodsType currCategory = GoodsType.BgImage;

        private List<Item> mallItemList;
        private List<MallItemView> mallItemViewList; // 用于销毁item实例

        private void OnEnable()
        {
            returnBtn.onClick.AddListener(OnReturnBtnClick);
        }

        private void OnReturnBtnClick()
        {
            UIManager.Instance.Pop();
            UIManager.Instance.Push(PanelType.Main);
        }

        // Start is called before the first frame update
        private void Start()
        {
            mallItemPrefab = Resources.Load<GameObject>("Prefabs/MallItem");

            GetMoneyInfo();

            // 选择不同的商品类型
            MallCategoryView[] categoryViews = categoryContent.GetComponentsInChildren<MallCategoryView>();
            foreach (var component in categoryViews)
                component.OnCategorySelected = OnCategorySelected;
            HandleCategoryBg(categoryViews[0]);

            // 选择不同的使用类型
            MallUseTypeView[] useTypeViews = useTypeContent.GetComponentsInChildren<MallUseTypeView>();
            foreach (var component in useTypeViews)
                component.OnUseTypeSelected = OnUseTypeSelected;
            HandleUseTypeBg(useTypeViews[0]);

            // 从数据库中读取数据
            mallItemList = GoodsContainer.Instance.buildInBagList.Select(info => info.ToItem()).ToList();
            mallItemViewList = new(16);
            print("count: " + mallItemList.Count);


            // 生成
            Spawn();
        }

        private void GetMoneyInfo()
        {
            gold.text = $"{SharedFieldUtils.GetGold()}";
            diamond.text = $"{SharedFieldUtils.GetDiamond()}";
        }

        // 使用类型选中
        private void OnUseTypeSelected(MallUseTypeView useTypeView)
        {
            if (HandleUseTypeBg(useTypeView))
            {
                currUseType = currUseTypeView.GetUseType();

                Spawn();
            }
        }

        // 处理使用类型的背景，如果确实需要切换，返回true，否则返回false
        private bool HandleUseTypeBg(MallUseTypeView useTypeView)
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

        // 分类选中
        private void OnCategorySelected(MallCategoryView categoryView)
        {
            if (HandleCategoryBg(categoryView))
            {
                currCategory = categoryView.GetCategory();

                Spawn();
            }
        }

        // 处理分类的背景，如果确实需要切换，返回true，否则返回false
        private bool HandleCategoryBg(MallCategoryView categoryView)
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

        private void OnMallItemSelected(MallItemView itemView)
        {
            if (itemView == currItemView)
                return;

            if (lastItemView != null)
                lastItemView.RestoreBg();
            currItemView = itemView;
            currItemView.ChangeBg();
            lastItemView = currItemView;
        }

        private void Spawn()
        {
            Clear();

            // ReSharper disable once ComplexConditionExpression
            filter = (Item item) => item.category == currCategory && (currUseType == UseType.All || item.useType == currUseType);
            List<Item> result = mallItemList.FindAll(filter).ToList();
            print("Spawn: " + result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                GameObject itemObj = Instantiate(mallItemPrefab, itemsContent);
                MallItemView itemView = itemObj.GetComponent<MallItemView>();
                itemView.SetItem(result[i]);
                itemView.OnMallItemSelected = OnMallItemSelected;
                mallItemViewList.Add(itemView);

                if (i == 0)
                    OnMallItemSelected(itemView);
            }
        }

        private void Clear()
        {
            foreach (var itemView in mallItemViewList)
            {
                Destroy(itemView.gameObject);
            }
            mallItemViewList.Clear();
        }

        private void OnDisable()
        {
            returnBtn.onClick.RemoveAllListeners();
        }
    }
}
