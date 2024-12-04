using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TopBar : MonoBehaviour
    {
        [Header("战斗值")]
        public TMP_Text hp;
        [Header("生命值")]
        public TMP_Text life;
        [Header("大招")]
        public TMP_Text daZhao;
        [Header("暂停按钮")]
        public Button pauseBtn;


        void OnEnable()
        {
            pauseBtn.onClick.AddListener(OnPauseButtonClick);
        }
        void OnDisable()
        {
            pauseBtn.onClick.RemoveListener(OnPauseButtonClick);
        }
        // 处理暂停按钮点击事件
        void OnPauseButtonClick()
        {
            //TODO
        }

        // 更新战斗值
        public void UpdateHp(int value)
        {
            if (value < 0) value = 0;

            hp.text = $"战斗值：{value}";
        }
        // 更新生命值
        public void UpdateLife(int value)
        {
            if (value < 0) value = 0;

            life.text = $"生命值：{value}";
        }
        // 更新大招值
        public void UpdateDaZhao(int value)
        {
            if (value < 0) value = 0;

            daZhao.text = $"大招：{value}";
        }
    }
}