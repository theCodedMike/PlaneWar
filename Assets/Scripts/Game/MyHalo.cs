using UnityEngine;

namespace Assets.Scripts.Game
{
    public class MyHalo : MonoBehaviour
    {
        private float lifeTime; // 存活时间

        private bool isActive;


        void OnEnable()
        {
            lifeTime = 3f;
            isActive = true;
        }

        // Update is called once per frame
        void Update()
        {
            lifeTime -= Time.deltaTime;
            if (isActive && lifeTime <= 0)
            {
                isActive = false;
                transform.gameObject.SetActive(false);
            }
        }

        // 光环是否还存在
        public bool IsHaloActive() => isActive;
    }
}
