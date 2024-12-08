using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using Utils;
using UI;

namespace Game
{
    public class Enemy : MonoBehaviour
    {
        public static string UniqueName = "EnemyDefault";

        [Header("移动速度")]
        public float moveSpeed;
        [Header("生成子弹的速度")]
        public float spawnSpeed;
        [Header("开火音频")]
        public AudioClip fireClip;
        [Header("爆炸音频")]
        public AudioClip explodeClip;

        private Rigidbody rb; // 敌机刚体组件

        private Transform firePosition; // 开火点位置

        private GameObject explodeVfxPrefab; // 爆炸特效预制体

        private GameObject bulletPrefab; // 子弹预制体

        private AudioSource audioSource; // 音频组件

        private Vector3 moveDirection = Vector3.zero; // 敌机的移动方向

        // Start is called before the first frame update
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            explodeVfxPrefab = Resources.Load<GameObject>(GoodsContainer.Instance.GetBuildInPrefabPath(EnemyExplosion.UniqueName));
            bulletPrefab = Resources.Load<GameObject>(GoodsContainer.Instance.GetBuildInPrefabPath(EnemyBullet.UniqueName));
            audioSource = GetComponent<AudioSource>();
            firePosition = transform.Find("FirePoint");
        }

        void OnEnable()
        {
            StartCoroutine(RandomMove());
            StartCoroutine(Fire());
        }


        IEnumerator RandomMove()
        {
            while (true)
            {
                int num = 0;
            start:
                num = Random.Range(0, 6);
                Vector3 direction = num switch
                {
                    0 => Vector3.zero, // 保持不动
                    1 => Vector3.left, // 左移,
                    2 => Vector3.right, // 右移
                    3 => new Vector3(-1, 0, -1), // 左下移动
                    4 => new Vector3(1, 0, -1), // 右下移动
                    _ => Vector3.back, // 向下
                };

                if (moveDirection == direction)
                    goto start;

                moveDirection = direction;
                SetVelocity(direction);

                yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));
            }
        }


        // 发射子弹
        IEnumerator Fire()
        {
            while (true)
            {
                GameObject bulletObj = ObjectPool.Instance.Get(EnemyBullet.UniqueName, bulletPrefab, firePosition.position, Quaternion.Euler(90, 0, 0));

                EnemyBullet bullet = bulletObj.GetComponent<EnemyBullet>();
                bullet.SetVelocity(Vector3.back);

                PlayFireClip();

                yield return new WaitForSeconds(Random.Range(1.5f, 2f));
            }
        }

        void FixedUpdate()
        {
            // 限制敌机的左右范围
            Vector3 position = rb.position;
            rb.position = new Vector3(Mathf.Clamp(position.x, -5, 5), position.y, position.z);
        }

        // 设置敌机速度
        public void SetVelocity(Vector3 velocity)
        {
            if (rb is null)
                rb = GetComponent<Rigidbody>();
            rb.velocity = velocity * moveSpeed;
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Wall") || other.CompareTag("DaZhao"))
            {
                ObjectPool.Instance.Put(UniqueName, this.gameObject);
            }
            else if (other.CompareTag("Player") || other.CompareTag("PlayerBullet"))
            {
                // 爆炸特效
                ObjectPool.Instance.Get(EnemyExplosion.UniqueName, explodeVfxPrefab, transform.position, Quaternion.identity);

                // 播放爆炸音频
                PlayExplodeClip();

                // 回收
                ObjectPool.Instance.Put(UniqueName, this.gameObject);
            }
        }

        // 播放爆炸音频
        void PlayExplodeClip()
        {
            audioSource.clip = explodeClip;
            audioSource.Play();
        }

        // 播放开火音频
        void PlayFireClip()
        {
            audioSource.clip = fireClip;
            audioSource.Play();
        }
    }
}
