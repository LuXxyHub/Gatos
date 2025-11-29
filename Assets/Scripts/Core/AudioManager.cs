using UnityEngine;

namespace CosmicYarnCat.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        public AudioSource SFXSource;
        public AudioSource MusicSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            // Create sources if not assigned
            if (SFXSource == null)
            {
                SFXSource = gameObject.AddComponent<AudioSource>();
            }
            if (MusicSource == null)
            {
                MusicSource = gameObject.AddComponent<AudioSource>();
                MusicSource.loop = true;
            }
        }

        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if (clip != null)
            {
                SFXSource.PlayOneShot(clip, volume);
            }
        }

        public void PlayMusic(AudioClip clip, float volume = 1f)
        {
            if (clip != null && MusicSource.clip != clip)
            {
                MusicSource.clip = clip;
                MusicSource.volume = volume;
                MusicSource.Play();
            }
        }

        public void StopMusic()
        {
            MusicSource.Stop();
        }
    }
}
