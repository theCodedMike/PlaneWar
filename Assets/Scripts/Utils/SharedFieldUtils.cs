using System;
using System.Globalization;
using UnityEngine;

namespace Utils
{
    public static class SharedFieldUtils
    {
        static class FieldName
        {
            // 金币数量，默认为100
            internal const string Gold = "Gold";
            // 钻石数量，默认为100
            internal const string Diamond = "Diamond";
            // 游戏是否暂停，默认为false（0）
            internal const string IsGamePause = "IsGamePause";
            // 背景音乐的默认音量
            internal const string BgmVolume = "BgmVolume";
            // 头像路径
            internal const string Avatar = "Avatar";
            // 用户名
            internal const string Username = "Username";
            // 密码
            internal const string Password = "Password";
            // 当前用户是否已注册
            internal const string IsSignUp = "IsSignUp";
            // 当前用户是否已登录
            internal const string IsLogin = "IsLogin";
            // 登录的失效时间，如果没有失效，用户可以直接进入Main Panel
            internal const string ExpireTime = "ExpireTime";
        }

        public static void Init()
        {
            PlayerPrefs.SetInt(FieldName.IsSignUp, 0);
            PlayerPrefs.SetInt(FieldName.IsLogin, 0);
            PlayerPrefs.SetString(FieldName.ExpireTime, "");

            PlayerPrefs.SetString(FieldName.Avatar, "Icons/Avatar/pumkin_icon");
            PlayerPrefs.SetString(FieldName.Username, "");
            PlayerPrefs.SetString(FieldName.Password, "");

            PlayerPrefs.SetInt(FieldName.Gold, 100);
            PlayerPrefs.SetInt(FieldName.Diamond, 100);
            PlayerPrefs.SetInt(FieldName.IsGamePause, 0);
            PlayerPrefs.SetInt(FieldName.BgmVolume, 50);

        }

        // 金币
        public static int GetGold() => PlayerPrefs.GetInt(FieldName.Gold);
        public static void SetGold(int value) => PlayerPrefs.SetInt(FieldName.Gold, value);

        // 钻石
        public static int GetDiamond() => PlayerPrefs.GetInt(FieldName.Diamond);
        public static void SetDiamond(int value) => PlayerPrefs.SetInt(FieldName.Diamond, value);

        // 游戏是否暂停
        public static bool IsGamePause() => PlayerPrefs.GetInt(FieldName.IsGamePause) == 1;
        public static void SetIsGamePause(bool pause) => PlayerPrefs.SetInt(FieldName.IsGamePause, pause ? 1 : 0);

        // 背景音乐音量
        public static int GetBgmVolume() => PlayerPrefs.GetInt(FieldName.BgmVolume);
        public static void SetBgmVolume(int value) => PlayerPrefs.SetInt(FieldName.BgmVolume, value);

        // 头像
        public static Sprite GetAvatar() => Resources.Load<Sprite>(PlayerPrefs.GetString(FieldName.Avatar));
        public static void SetAvatar(string path) => PlayerPrefs.SetString(FieldName.Avatar, path);

        // 用户名
        public static string GetUsername() => PlayerPrefs.GetString(FieldName.Username);
        public static void SetUsername(string name) => PlayerPrefs.SetString(FieldName.Username, name);

        // 密码
        public static string GetPassword() => PlayerPrefs.GetString(FieldName.Password);
        public static void SetPassword(string password) => PlayerPrefs.SetString(FieldName.Password, password);


        // 是否注册
        public static bool IsSignUp() => PlayerPrefs.GetInt(FieldName.IsSignUp) == 1;
        public static void SetIsSignUp(bool isSignUp) => PlayerPrefs.SetInt(FieldName.IsSignUp, isSignUp ? 1 : 0);

        // 是否登录
        public static bool IsLogin() => PlayerPrefs.GetInt(FieldName.IsLogin) == 1;
        public static void SetIsLogin(bool isLogin) => PlayerPrefs.SetInt(FieldName.IsLogin, isLogin ? 1 : 0);

        // 登录已过期
        public static bool IsLoginExpire()
        {
            string time = PlayerPrefs.GetString(FieldName.ExpireTime);
            return DateTime.Parse(time).CompareTo(DateTime.Now) <= 0;
        }

        public static void SetLoginExpireTime()
        {
            DateTime expireTime = DateTime.Now.AddSeconds(60); // 默认是60秒
            PlayerPrefs.SetString(FieldName.ExpireTime, expireTime.ToString(CultureInfo.InvariantCulture));
        }
    }
}
