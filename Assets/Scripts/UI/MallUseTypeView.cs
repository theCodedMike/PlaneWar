using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class MallUseTypeView : MonoBehaviour, IPointerClickHandler
    {
        public Action<MallUseTypeView> OnUseTypeSelected;

        private Image image;


        // Start is called before the first frame update
        private void Awake()
        {
            image = GetComponentInChildren<Image>();
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            OnUseTypeSelected(this);
        }

        // 改变背景
        public void ChangeBg()
        {
            image.color = new Color32(152, 221, 5, 255);
        }

        // 恢复背景
        public void RestoreBg()
        {
            image.color = new Color32(113, 175, 250, 255);
        }

        public UseType GetUseType()
        {
            string type = transform.GetComponentInChildren<TMP_Text>().text;
            return type switch
            {
                "全部" => UseType.All,
                "我方" => UseType.Player,
                "敌方" => UseType.Enemy,
                _ => throw new ArgumentException($"未知的使用类型：{type}")
            };
        }
    }
}
