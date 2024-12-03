using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class LifeStar : MonoBehaviour
    {
        public const string Name = "LifeStar";

        [Header("移动速度")]
        public float moveSpeed;

        private Rigidbody rb; // 刚体组件

        private Vector3 moveDirection = Vector3.zero; // 敌机的移动方向


        // Start is called before the first frame update
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void OnEnable()
        {
            StartCoroutine(RandomMove());
        }

        IEnumerator RandomMove()
        {
            while (true)
            {
                Vector3 currPosition = rb.position;

                Start:
                int num = Random.Range(0, 6);
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
                    goto Start;

                moveDirection = direction;
                SetVelocity(direction);

                yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));
            }
        }

        void FixedUpdate()
        {
            // 限制左右范围
            Vector3 position = rb.position;
            rb.position = new Vector3(Mathf.Clamp(position.x, -5, 5), position.y, position.z);
        }

        public void SetVelocity(Vector3 velocity)
        {
            if (rb == null)
                rb = GetComponent<Rigidbody>();
            rb.velocity = velocity * moveSpeed;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Wall") || other.CompareTag("Player"))
            {
                ObjectPool.Instance.Put(Name, this.gameObject);
            }
        }
    }
}
