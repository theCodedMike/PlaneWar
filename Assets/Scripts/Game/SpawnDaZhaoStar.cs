using UnityEngine;
using Utils;

namespace Game
{
    // 玩家撞到后可以增加使用大招的次数
    public class SpawnDaZhaoStar : MonoBehaviour
    {
        [Header("生成大招星星速度")]
        public float spawnSpeed;

        private GameObject daZhaoStarPrefab; // 陨石预制体

        private float timer; // 计时器，用于控制陨石的生成速度


        // Start is called before the first frame update
        void Start()
        {
            daZhaoStarPrefab = Resources.Load<GameObject>("Prefabs/DaZhaoStar");
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= (1f / spawnSpeed))
            {
                timer = 0;
                Spawn();
            }
        }

        void Spawn()
        {
            Vector3 position = transform.position;
            position += Vector3.right * Random.Range(-5f, 5f);

            GameObject daZhaoStarObj = ObjectPool.Instance.Get(DaZhaoStar.Name, daZhaoStarPrefab, position, Quaternion.Euler(90, 0, 0));
            DaZhaoStar daZhaoStar = daZhaoStarObj.GetComponent<DaZhaoStar>();
            daZhaoStar.SetVelocity(Vector3.back);
        }
    }
}
