using UnityEngine;
using Utils;

namespace Game
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
