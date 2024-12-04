using UnityEngine;

namespace Assets.Scripts.Game
{
    public class SpawnEnemy : MonoBehaviour
    {
        [Header("生成敌机的速度")]
        public float spawnSpeed;

        private GameObject enemyPrefab; // 敌机的预制体

        private float timer; // 计时器


        // Start is called before the first frame update
        void Start()
        {
            enemyPrefab = Resources.Load<GameObject>(GoodsContainer.Instance.GetBuildInPrefabPath(Enemy.UniqueName));
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= (1.0 / spawnSpeed))
            {
                timer = 0;
                Spawn();
            }
        }

        // 生成敌机
        void Spawn()
        {
            Vector3 position = transform.position;
            position += Vector3.right * Random.Range(-5, 5);
            GameObject enemyObj = ObjectPool.Instance.Get(Enemy.UniqueName, enemyPrefab, position, Quaternion.Euler(0, 180, 0));

            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy.SetVelocity(Vector3.back);
        }
    }
}
