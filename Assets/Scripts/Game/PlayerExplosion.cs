using UnityEngine;

namespace Assets.Scripts.Game
{
    public class PlayerExplosion : MonoBehaviour
    {
        public static string UniqueName = "PlayerExplosionDefault";
        void OnParticleSystemStopped()
        {
            ObjectPool.Instance.Put(UniqueName, this.gameObject);
        }
    }
}
