using UnityEngine;
using Utils;
using UI;

namespace Game
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

        private GameObject daZhaoPrefab; // 大招的预制体

        private MyHalo halo; // 光环组件

        private int daZhao; // 发大招的次数

        private int life; // 生命，可以通过吃生命星星来增加

        private int hp; // 血量

        private TopBar topBar; // 顶部信息栏


        void OnEnable()
        {
            daZhao = 3;
            life = 1;
            hp = 0;
        }

        // Start is called before the first frame update
        void Start()
        {
            firePoint = transform.Find("FirePoint");
            rigbody = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            halo = transform.Find("Halo").GetComponent<MyHalo>();
            bulletPrefab = Resources.Load<GameObject>(GoodsContainer.Instance.GetBuildInPrefabPath(PlayerBullet.UniqueName));
            explodeVfxPrefab = Resources.Load<GameObject>(GoodsContainer.Instance.GetBuildInPrefabPath(PlayerExplosion.UniqueName));
            daZhaoPrefab = Resources.Load<GameObject>(GoodsContainer.Instance.GetBuildInPrefabPath(DaZhao.UniqueName));

            Transform canvas = GameObject.Find("Canvas").transform;
            if (canvas == null)
            {
                Debug.LogError("Canvas is null!!!");
                return;
            }
            topBar = canvas.Find("TopBar").GetComponent<TopBar>();
        }

        // Update is called once per frame
        void Update()
        {
            if (IsDead())
            {
                //OnPlayerDead();
            }

            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                LaunchDaZhao();
            }

            UpdateTopBar();
        }

        // 更新顶部信息栏
        void UpdateTopBar()
        {
            topBar.UpdateHp(this.hp);
            topBar.UpdateLife(this.life);
            topBar.UpdateDaZhao(this.daZhao);
        }

        // 处理玩家死亡时的逻辑
        void OnPlayerDead()
        {
            // 生成特效
            ObjectPool.Instance.Get(PlayerExplosion.UniqueName, explodeVfxPrefab, transform.position, Quaternion.identity);

            // 播放爆炸音频
            PlayExplodeClip();

            // 回收
            ObjectPool.Instance.Put(UniqueName, gameObject);

            // TODO: 回到主页面
        }

        // 发射子弹
        void Fire()
        {
            GameObject bulletObj = ObjectPool.Instance.Get(PlayerBullet.UniqueName, bulletPrefab, firePoint.position, Quaternion.Euler(90, 0, 0));
            PlayerBullet bullet = bulletObj.GetComponent<PlayerBullet>();
            bullet.SetVelocity(Vector3.forward, this);

            PlayFireClip();
        }

        // 放大招
        void LaunchDaZhao()
        {
            if (this.daZhao <= 0)
                return;

            Vector3 position = firePoint.position;
            position.x = 0;
            GameObject daZhaoObj = ObjectPool.Instance.Get(DaZhao.UniqueName, daZhaoPrefab, position, Quaternion.identity);
            DaZhao daZhao = daZhaoObj.GetComponent<DaZhao>();
            daZhao.SetVelocity(Vector3.forward);
            this.daZhao--;
        }

        // 增加血量
        public void AddHp(uint value)
        {
            this.hp += (int)value;
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

        // 玩家是否已死
        public bool IsDead() => life <= 0;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DaZhaoStar"))
            {
                daZhao++;
            }
            if (other.CompareTag("LifeStar"))
            {
                life++;
            }
            if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet") || other.CompareTag("Asteroid"))
            {
                if (!halo.IsHaloActive()) // 每次新开始游戏时，飞船有3秒的保护器
                {
                    if (hp > 0) // 如果有血量，血量减少
                    {
                        hp--;
                    }
                    else if (life > 0) // 如果还有生命，生命减少
                    {
                        life--;
                    }
                }
            }
        }
    }

    [System.Serializable]
    public struct Boundary
    {
        public float minX, maxX, minZ, maxZ;
    }

}
