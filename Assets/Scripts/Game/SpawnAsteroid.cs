using UnityEngine;

namespace Assets.Scripts.Game
{
    public class SpawnAsteroid : MonoBehaviour
    {
        [Header("生成速度")]
        public float spawnSpeed;

        private GameObject[] asteroids;

        private float timer;
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