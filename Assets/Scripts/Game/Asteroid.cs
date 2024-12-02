using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Asteroid : MonoBehaviour
    {
        public const string Name = "Asteroid";

        [Header("移动速度")]
        public float moveSpeed;
        [Header("旋转速度")]
        public float rotateSpeed;
        [Header("爆炸音频")]
        public AudioClip explodeClip;

        private Rigidbody rb; // 刚体组件

        private GameObject explodeVfxPrefab; // 爆炸特效预制体

        private AudioSource audioSource; // 播放组件


        // Start is called before the first frame update
        void Start()
        {
            explodeVfxPrefab = Resources.Load<GameObject>("Prefabs/Vfx/AsteroidExplosion");
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetVelocity(Vector3 velocity)
        {
            if (rb is null)
                rb = GetComponent<Rigidbody>();
            rb.velocity = velocity * moveSpeed;
            rb.angularVelocity = Random.onUnitSphere * rotateSpeed;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Wall"))
            {
                ObjectPool.Instance.Put(Name, this.gameObject);
            } else if (other.CompareTag("PlayerBullet") || other.CompareTag("Player"))
            {
                // 爆炸特效
                ObjectPool.Instance.Get(AsteroidExplosion.Name, explodeVfxPrefab, transform.position,
                    Quaternion.identity);

                // 播放音乐
                PlayExplodeClip();

                // 回收
                ObjectPool.Instance.Put(Name, this.gameObject);
            }
        }

        
        void PlayExplodeClip()
        {
            audioSource.clip = explodeClip;
            audioSource.Play();
        }
    }
}
