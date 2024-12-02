using UnityEngine;

namespace Assets.Scripts.Game
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
