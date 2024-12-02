using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Enemy : MonoBehaviour
    {
        public static string UniqueName = "EnemyDefault";

        [Header("移动速度")]
        public float moveSpeed;

        private Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {

        }


        // Update is called once per frame
        void Update()
        {
        
        }

        // 设置敌机速度
        public void SetVelocity(Vector3 velocity)
        {
            if (rb is null)
                rb = GetComponent<Rigidbody>();
            rb.velocity = velocity * moveSpeed;
        }
    }
}
