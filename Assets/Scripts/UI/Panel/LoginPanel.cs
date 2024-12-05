using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panel
{
    public class LoginPanel : BasePanel
    {
        [Header("用户名输入框")]
        public TMP_InputField username;
        [Header("密码输入框")]
        public TMP_InputField password;
        [Header("登录按钮")]
        public Button loginBtn;
        [Header("输错提示")]
        public TMP_Text tip;

        private const string DefaultUsername = "123";
        private const string DefaultPassword = "123";

        // Start is called before the first frame update
        void Start()
        {
            Clear();
        }

        private void OnEnable()
        {
            loginBtn.onClick.AddListener(OnLoginButtonClick);
            username.onValueChanged.AddListener(OnUsernameValueChanged);
            password.onValueChanged.AddListener(OnPasswordValueChanged);
        }
        private void OnDisable()
        {
            loginBtn.onClick.RemoveAllListeners();
            username.onValueChanged.RemoveAllListeners();
            password.onValueChanged.RemoveAllListeners();

            Clear();
        }

        // 处理username输入
        private void OnPasswordValueChanged(string value) => HandleInputEvent();
        
        
        // 处理password输入
        private void OnUsernameValueChanged(string value) => HandleInputEvent();
        

        private void HandleInputEvent()
        {
            if (string.IsNullOrEmpty(username.text) || string.IsNullOrEmpty(password.text))
            {
                SetLoginBtnUnavailable();
                return;
            }

            SetLoginBtnAvailable();
        }

        // 处理登录按钮点击事件
        private void OnLoginButtonClick()
        {
            if (username.text != DefaultUsername || password.text != DefaultPassword)
            {
                tip.text = "登录失败（用户名和密码都是123）";
                Invoke("Clear", 2);
                return;
            }

            tip.text = "登录成功";
            //UIManager.Instance.Pop();
            //UIManager.Instance.Push(PanelType.Main);
        }

        // 清空
        void Clear()
        {
            username.text = "";
            password.text = "";
            tip.text = "";
            SetLoginBtnUnavailable();
        }

        // 置登录按钮不可用
        void SetLoginBtnUnavailable()
        {
            loginBtn.interactable = false;
        }

        // 置登录按钮可用
        void SetLoginBtnAvailable()
        {
            loginBtn.interactable = true;
        }
    }
}
