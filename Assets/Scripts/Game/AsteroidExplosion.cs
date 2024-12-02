using UnityEngine;

namespace Assets.Scripts.Game
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
