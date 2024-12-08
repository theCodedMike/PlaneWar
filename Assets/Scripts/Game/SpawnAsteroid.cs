using UnityEngine;
using Utils;

namespace Game
{
    public class SpawnAsteroid : MonoBehaviour
    {
        [Header("生成陨石的速度")]
        public float spawnSpeed;

        private GameObject[] asteroids; // 陨石预制体

        private float timer; // 计时器，用于控制陨石的生成速度


        // Start is called before the first frame update
        void Start()
        {
            asteroids = Resources.LoadAll<GameObject>("Prefabs/Asteroids");
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
            int idx = Random.Range(0, asteroids.Length);
            Vector3 position = transform.position;
            position += Vector3.right * Random.Range(-5f, 5f);

            GameObject asteroidObj = ObjectPool.Instance.Get(Asteroid.Name, asteroids[idx], position, Quaternion.identity);
            Asteroid asteroid = asteroidObj.GetComponent<Asteroid>();
            asteroid.SetVelocity(Vector3.back);
        }
    }
}
