using UnityEngine;
using Utils;

namespace Game
{
    public class SpawnLifeStar : MonoBehaviour
    {
        [Header("生成生命星星速度")]
        public float spawnSpeed;

        private GameObject lifeStarPrefab; // 陨石预制体

        private float timer; // 计时器，用于控制陨石的生成速度


        // Start is called before the first frame update
        void Start()
        {
            lifeStarPrefab = Resources.Load<GameObject>("Prefabs/LifeStar");
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

            GameObject lifeStarObj = ObjectPool.Instance.Get(LifeStar.Name, lifeStarPrefab, position, Quaternion.Euler(90, 0, 0));
            LifeStar lifeStar = lifeStarObj.GetComponent<LifeStar>();
            lifeStar.SetVelocity(Vector3.back);
        }
    }
}
