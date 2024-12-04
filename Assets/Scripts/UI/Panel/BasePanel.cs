using UnityEngine;

namespace Assets.Scripts.UI
{
    public class BasePanel : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        public CanvasGroup CanvasGroup
        {
            get
            {
                return canvasGroup ??= GetComponent<CanvasGroup>();
            }
        }
        // 进入Panel
        public virtual void OnEnter()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }


        // 暂停Panel
        public virtual void OnPause()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }


        // 恢复Panel
        public virtual void OnResume()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }


        // 退出Panel
        public virtual void OnExit()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
