using UnityEngine;
using Utils;

namespace Game
{
    public class EnemyBullet : MonoBehaviour
    {
        public static string UniqueName = "EnemyBulletDefault";

        [Header("移动速度")]
        public float moveSpeed;

        private Rigidbody rb; // 敌机子弹的刚体组件


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void SetVelocity(Vector3 velocity)
        {
            if (rb == null)
                rb = GetComponent<Rigidbody>();

            rb.velocity = velocity * moveSpeed;
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Wall") || other.CompareTag("DaZhao"))
            {
                ObjectPool.Instance.Put(UniqueName, this.gameObject);
            }
        }
    }
}
