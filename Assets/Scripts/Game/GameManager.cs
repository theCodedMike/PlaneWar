using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Game
{
    // ReSharper disable once HollowTypeName
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            string prefabPath = GoodsContainer.Instance.GetBuildInPrefabPath(Player.UniqueName);
            GameObject player = Resources.Load<GameObject>(prefabPath);
            ObjectPool.Instance.Get(Player.UniqueName, player, transform.position, Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {
            // 按下鼠标右键暂停游戏
            if (Input.GetMouseButtonDown(1))
            {
                Time.timeScale = 0;
                SharedFieldUtils.SetIsGamePause(true);

                StartCoroutine(LoadUISceneAsync());
            }
        }

        IEnumerator LoadUISceneAsync()
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("UI");
            while (!asyncOperation.isDone)
            {
                yield return null;
            }

            //SceneManager.SetActiveScene(SceneManager.GetSceneByName("UI"));
            //Camera.main.GetComponent<AudioListener>().enabled = false;
            //UIManager.Instance.Push(PanelType.Main);
        }
    }
}
