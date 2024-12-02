using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class PlayerBullet : MonoBehaviour
    {
        public static string UniqueName = "PlayerBulletDefault";

        [Header("移动速度")]
        public float moveSpeed;

        private Rigidbody rb;

        // Start is called before the first frame update
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        // 设置玩家子弹的速度
        public void SetVelocity(Vector3 velocity)
        {
            rb.velocity = velocity * moveSpeed;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Wall"))
            {
                ObjectPool.Instance.Put(UniqueName, this.gameObject);
            }
        }
    }
}
