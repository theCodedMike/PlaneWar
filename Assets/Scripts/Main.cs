using UI;
using UnityEngine;
using Utils;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SharedFieldUtils.Init();
    }

    void Start()
    {
        // UIManager 的入口
        UIManager.Instance.Push(PanelType.Login);
    }

    // Update is called once per fram
    void Update()
    {

    }
}