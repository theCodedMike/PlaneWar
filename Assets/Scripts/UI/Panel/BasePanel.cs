using UnityEngine;

namespace UI.Panel
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
            CanvasGroup.alpha = 1;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }


        // 暂停Panel
        public virtual void OnPause()
        {
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }


        // 恢复Panel
        public virtual void OnResume()
        {
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }


        // 退出Panel
        public virtual void OnExit()
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }
    }
}
