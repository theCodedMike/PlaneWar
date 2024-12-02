using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Player : MonoBehaviour
    {
        public static string UniqueName = "PlayerDefault";

        [Header("我方飞机的移动速度")]
        public float moveSpeed;
        [Header("飞机飞行的边界")]
        public Boundary boundary;
        [Header("开火音频")]
        public AudioClip fireClip;
        [Header("爆炸音频")]
        public AudioClip explodeClip;


        private Transform firePoint; // 开火点的位置

        private Rigidbody rigbody; // 飞机刚体组件

        private GameObject bulletPrefab; // 子弹预制体

        private AudioSource audioSource; // 音频播放器

        private GameObject explodeVfxPrefab; // 爆炸特效预制体


        // Start is called before the first frame update
        void Start()
        {
            firePoint = transform.Find("FirePoint");
            rigbody = GetComponent<Rigidbody>();
            bulletPrefab = Resources.Load<GameObject>(GoodsContainer.Instance.GetPrefabPath(PlayerBullet.UniqueName));
            explodeVfxPrefab = Resources.Load<GameObject>(GoodsContainer.Instance.GetPrefabPath(PlayerExplosion.UniqueName));
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }

        void Fire()
        {
            GameObject bulletObj = ObjectPool.Instance.Get(PlayerBullet.UniqueName, bulletPrefab, firePoint.position, Quaternion.Euler(90, 0, 0));
            PlayerBullet bullet = bulletObj.GetComponent<PlayerBullet>();
            bullet.SetVelocity(Vector3.forward);

            PlayFireClip();
        }


        // 播放发射子弹的音乐
        void PlayFireClip()
        {
            audioSource.clip = fireClip;
            audioSource.Play();
        }
        // 播放飞机爆炸的音乐
        void PlayExplodeClip()
        {
            audioSource.clip = explodeClip;
            audioSource.Play();
        }

        void FixedUpdate()
        {
            Move();
        }

        void Move()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            // 倾斜
            if (x < 0)
                rigbody.rotation = Quaternion.Euler(0, 0, 30);
            else if (x > 0)
                rigbody.rotation = Quaternion.Euler(0, 0, -30);
            else
                rigbody.rotation = Quaternion.identity;

            // 移动
            rigbody.velocity = new Vector3(x, 0, z) * moveSpeed;

            // 限制左右上下边界
            Vector3 currPosition = rigbody.position;
            rigbody.position = new Vector3(
                Mathf.Clamp(currPosition.x, boundary.minX, boundary.maxX), 
                currPosition.y,
                Mathf.Clamp(currPosition.z, boundary.minZ, boundary.maxZ)
                );
            
        }

        void OnTriggerEnter(Collider other)
        {
            // TODO: 比较tag，完善游戏结束逻辑
            /*if (other is null)
            {
                
                // 生成特效
                ObjectPool.Instance.Get(PlayerExplosion.UniqueName, explodeVfxPrefab, transform.position,
                    Quaternion.identity);

                // 播放爆炸音频
                PlayExplodeClip();

                // 游戏结束
            }*/
        }
    }

    [System.Serializable]
    public struct Boundary
    {
        public float minX, maxX, minZ, maxZ;
    }

}
