using System;
using System.Collections.Generic;
using UI.Panel;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI
{
    // ReSharper disable once HollowTypeName
    public class UIManager
    {
        private static UIManager instance;
        public static UIManager Instance { get { return instance ??= new UIManager(); } }

        private UIManager()
        {
            panelPathDic = new(5);
            panelObjDic = new(5);
            panelStack = new(2);
            Init();
        }

        // panel预制体在Resource文件夹中的路径
        private Dictionary<PanelType, string> panelPathDic;
        // panel实例的映射
        private Dictionary<PanelType, BasePanel> panelObjDic;
        // 存放panel
        private Stack<BasePanel> panelStack;

        private GameObject canvas;

        void Init()
        {
            canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.LogError("Cannot find Canvas GameObject, please create it first!");
                return;
            }

            PanelContainer panels = Resources.Load<PanelContainer>("PanelContainer");
            foreach (var panel in panels.panelList)
            {
                panelPathDic.Add(panel.type, panel.path);
            }
        }


        // 显示当前type类型的panel
        public BasePanel Push(PanelType type)
        {
            if (panelStack.Count > 0)
            {
                panelStack.Peek().OnPause();
            }

            BasePanel currPanel = GetPanel(type);
            currPanel.OnEnter();
            panelStack.Push(currPanel);

            return currPanel;
        }

        BasePanel GetPanel(PanelType type)
        {
            BasePanel panel = null;

            if (panelObjDic.TryGetValue(type, out panel))
            {
                return panel;
            }

            if (panelPathDic.TryGetValue(type, out var path))
            {
                GameObject prefab = Resources.Load<GameObject>(path);
                GameObject panelObj = Object.Instantiate(prefab, canvas.transform);

                panel = panelObj.GetComponent<BasePanel>();
                panelObjDic.Add(type, panel);

                return panel;
            }

            throw new ArgumentException($"Cannot find {type} panel.");
        }

        // 关闭当前panel
        public void Pop()
        {
            if (panelStack.Count == 0)
                return;

            BasePanel panel = panelStack.Pop();
            panel.OnExit();

            if (panelStack.Count > 0)
            {
                var nextPanel = panelStack.Peek();
                nextPanel.transform.SetAsLastSibling(); // 在最上方
                nextPanel.OnResume();
            }
        }

    }
}
