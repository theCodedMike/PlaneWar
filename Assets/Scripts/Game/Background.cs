using UnityEngine;

namespace Game
{
    public class Background : MonoBehaviour
    {
        private const string Property = "_MainTex";

        [Header("背景的移动速度")]
        public float moveSpeed;

        private Material material; // 背景图片的材质

        private AudioSource audioSource; // 音频组件

        // Start is called before the first frame update
        void Start()
        {
            material = GetComponent<MeshRenderer>().material;
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.5f; // 默认音量为一半
        }

        // Update is called once per frame
        void Update()
        {
            Move();


            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeBgImage("Textures/bg_image_sky");
            }*/

        }

        /// <summary>
        /// 背景图从上往下移动
        /// </summary>
        void Move()
        {
            Vector2 currOffset = material.GetTextureOffset(Property);
            currOffset.y -= Time.deltaTime * moveSpeed;
            material.SetTextureOffset(Property, currOffset);
        }


        /// <summary>
        /// 改变背景的移动速度
        /// </summary>
        /// <param name="speed">背景的新移动速度</param>
        public void ChangeMoveSpeed(float speed)
        {
            this.moveSpeed = speed;
        }

        /// <summary>
        /// 改变背景
        /// </summary>
        /// <param name="path">新背景的路径</param>
        public void ChangeBgImage(string path)
        {
            Texture newTexture = Resources.Load<Texture>(path);
            if (newTexture is null)
            {
                Debug.LogError($"Cannot load {path}...");
                return;
            }

            material.SetTexture(Property, newTexture);
        }

        /// <summary>
        /// 改变背景音乐
        /// </summary>
        /// <param name="path">新背景音乐的路径</param>
        public void ChangeBgMusic(string path)
        {
            AudioClip newAudioClip = Resources.Load<AudioClip>(path);
            if (newAudioClip is null)
            {
                Debug.LogError($"Cannot load {path}...");
                return;
            }
            this.audioSource.Stop();
            this.audioSource.clip = newAudioClip;
            this.audioSource.Play();
        }

        /// <summary>
        /// 改变背景音乐的音量
        /// </summary>
        /// <param name="volume">音量，在[0, 1]之间</param>
        public void ChangeBgMusicVolume(float volume)
        {
            volume = Mathf.Clamp(volume, 0, 1);
            this.audioSource.volume = volume;
        }

        /// <summary>
        /// 获取背景音乐的音量
        /// </summary>
        /// <returns></returns>
        public float GetBgMusicVolume() => this.audioSource.volume;
    }
}
