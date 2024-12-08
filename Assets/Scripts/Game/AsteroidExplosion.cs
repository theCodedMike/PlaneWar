using UnityEngine;
using Utils;

namespace Game
{
    public class AsteroidExplosion : MonoBehaviour
    {
        public const string Name = "AsteroidExplosion";

        void OnParticleSystemStopped()
        {
            ObjectPool.Instance.Put(Name, this.gameObject);
        }
    }
}
