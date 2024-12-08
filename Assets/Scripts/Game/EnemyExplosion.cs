using UnityEngine;
using Utils;

namespace Game
{
    public class EnemyExplosion : MonoBehaviour
    {
        public static string UniqueName = "EnemyExplosionDefault";

        void OnParticleSystemStopped()
        {
            ObjectPool.Instance.Put(UniqueName, this.gameObject);
        }
    }
}
