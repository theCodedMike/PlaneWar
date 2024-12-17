using UnityEngine;

namespace Game
{
    public class MyHalo : MonoBehaviour
    {
        private float lifeTime; // 存活时间

        private bool isActive; // 是否还存在


        void OnEnable()
        {
            lifeTime = 3f;
            isActive = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isActive)
                return;

            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                isActive = false;
                transform.gameObject.SetActive(false);
            }
        }

        // 光环是否还存在
        public bool IsHaloActive() => isActive;
    }
}
