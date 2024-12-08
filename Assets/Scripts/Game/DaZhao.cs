using UnityEngine;
using Utils;

namespace Game
{
    public class DaZhao : MonoBehaviour
    {
        public static string UniqueName = "DaZhaoDefault";

        [Header("大招的移动速度")]
        public float moveSpeed;

        private Rigidbody rb;

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
            if (other.CompareTag("Wall"))
            {
                ObjectPool.Instance.Put(UniqueName, this.gameObject);
            }
        }
    }
}
