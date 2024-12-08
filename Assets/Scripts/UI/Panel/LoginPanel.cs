using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.Panel
{
    public class LoginPanel : BasePanel, IPointerClickHandler
    {
        [Header("用户名输入框")]
        public TMP_InputField username;
        [Header("密码输入框")]
        public TMP_InputField password;
        [Header("注册按钮")]
        public Button signUpBtn;
        [Header("登录按钮")]
        public Button loginBtn;
        [Header("输错提示")]
        public TMP_Text tip;


        private AudioSource audioSource;

        private const string DefaultUsername = "123";
        private const string DefaultPassword = "123";

        public static LoginPanel Instance { get; private set; }
        public bool isLogin; // 是否已登录

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            Clear();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            loginBtn.onClick.AddListener(OnLoginButtonClick);
            signUpBtn.onClick.AddListener(OnSignUpBtnClick);
            username.onValueChanged.AddListener(OnUsernameValueChanged);
            password.onValueChanged.AddListener(OnPasswordValueChanged);
        }

        // 处理username输入
        private void OnPasswordValueChanged(string value) => HandleInputEvent();


        // 处理password输入
        private void OnUsernameValueChanged(string value) => HandleInputEvent();


        private void HandleInputEvent()
        {
            if (string.IsNullOrEmpty(username.text) || string.IsNullOrEmpty(password.text))
            {
                SetBtnUnavailable();
                return;
            }

            SetBtnAvailable();
        }

        // 处理注册按钮点击事件
        private void OnSignUpBtnClick()
        {
            audioSource.Play();

            bool isSignUp = SharedFieldUtils.IsSignUp();
            if (isSignUp)
            {
                tip.text = "您已注册，请点击登录按钮进行登录";
                Invoke(nameof(ClearTip), 2);
                return;
            }

            SharedFieldUtils.SetIsSignUp(true);
            SharedFieldUtils.SetUsername(username.text);
            SharedFieldUtils.SetPassword(password.text);
            JumpWhenSuccess("注册成功");
        }

        // 处理登录按钮点击事件
        private void OnLoginButtonClick()
        {
            audioSource.Play();

            bool isSignUp = SharedFieldUtils.IsSignUp();
            if (!isSignUp) // 用户未注册，却点击了登录按钮
            {
                tip.text = "您尚未注册，请点击注册按钮进行注册";
                Invoke(nameof(ClearTip), 2f);
                return;
            }

            (string usernameFromDb, string passwordFromDb) =
                (SharedFieldUtils.GetUsername(), SharedFieldUtils.GetPassword());
            (string usernameFromInput, string passwordFromInput) = (username.text, password.text);

            if (usernameFromInput == usernameFromDb && passwordFromInput == passwordFromDb)
                JumpWhenSuccess("登录成功");
            else
            {
                string tipMsg = "";
                if (usernameFromInput != usernameFromDb)
                    tipMsg = "用户名不正确";
                else if (passwordFromInput != passwordFromDb)
                    tipMsg = "密码不正确";
                else
                    tipMsg = "用户名和密码都不正确";

                RetryWhenFail(tipMsg);
            }
        }

        // 登录/注册成功，则跳转到Main Panel页面
        void JumpWhenSuccess(string tipMsg)
        {
            tip.text = tipMsg;
            SharedFieldUtils.SetIsLogin(true);
            SharedFieldUtils.SetLoginExpireTime();

            UIManager.Instance.Pop();
            UIManager.Instance.Push(PanelType.Main);
        }

        // 登录失败，则重试
        private void RetryWhenFail(string tipMsg)
        {
            tip.text = tipMsg;
            SharedFieldUtils.SetIsLogin(false);
            Invoke(nameof(Clear), 2f);
        }

        // 清空提示
        void ClearTip()
        {
            tip.text = "";
        }

        // 清空
        void Clear()
        {
            username.text = "";
            password.text = "";
            ClearTip();
            SetBtnUnavailable();
        }

        // 置登录/注册按钮不可用
        void SetBtnUnavailable()
        {
            loginBtn.interactable = false;
            signUpBtn.interactable = false;
        }

        // 置登录/注册按钮可用
        void SetBtnAvailable()
        {
            loginBtn.interactable = true;
            signUpBtn.interactable = true;
        }

        // 处理密码是否以明文显示
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.pointerEnter.name != "Eye")
                return;

            if (password.contentType == TMP_InputField.ContentType.Password)
                password.contentType = TMP_InputField.ContentType.Standard;
            else
                password.contentType = TMP_InputField.ContentType.Password;
            password.Select();
        }


        private void OnDisable()
        {
            loginBtn.onClick.RemoveAllListeners();
            signUpBtn.onClick.RemoveAllListeners();
            username.onValueChanged.RemoveAllListeners();
            password.onValueChanged.RemoveAllListeners();

            Clear();
        }
    }
}
