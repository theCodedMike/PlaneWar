using UnityEngine;

namespace Assets.Scripts.Game
{
    public class PlayerBullet : MonoBehaviour
    {
        public static string UniqueName = "PlayerBulletDefault";

        [Header("移动速度")]
        public float moveSpeed;

        private Rigidbody rb; // 刚体组件

        private Player player; // 玩家

        // Start is called before the first frame update
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        // 设置玩家子弹的速度
        public void SetVelocity(Vector3 velocity, Player player)
        {
            this.player = player;
            rb.velocity = velocity * moveSpeed;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Wall") || other.CompareTag("DaZhao"))
            {
                ObjectPool.Instance.Put(UniqueName, this.gameObject);
            }
            else if (other.CompareTag("Enemy") || other.CompareTag("Asteroid"))
            {

                // Player的血量+1
                player.AddHp(1);

                // 回收
                ObjectPool.Instance.Put(UniqueName, this.gameObject);
            }
        }
    }
}
