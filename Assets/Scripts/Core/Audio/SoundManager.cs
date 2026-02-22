using System.Collections.Generic;
using UnityEngine;

namespace BasketChallenge.Core
{

    /// <summary>
    /// This class manages the reproduction of sounds
    /// </summary>
    public class SoundManager : Pool<MyAudioSource>
    {
        private static SoundManager _this;

        private Dictionary<string, AudioClip> _audioClips;
        
        [Min(2f), SerializeField] private int startingSourceCount;

        private void Awake() => Init();

        private void OnEnable() => MyAudioSource.AudioClipEnded += StopSource;

        private void OnDisable() => MyAudioSource.AudioClipEnded -= StopSource;

        private void Init()
        {
            if (_this != null)
            {
                Debug.LogWarning("Sound Manager already exists, destroy new one");
                Destroy(gameObject);
                return;
            }
            _this = this;
            
            poolObjPrefab = Resources.Load<MyAudioSource>("AudioSource");
            AudioClipsData audioClipsData = Resources.Load<AudioClipsData>("AudioClipsData");
            
            _audioClips = new Dictionary<string, AudioClip>();
            for (int i = 0; i < audioClipsData.audioClips.Count; i++)
            {
                if (audioClipsData.audioClipNames[i] == null || audioClipsData.audioClips[i] == null)
                {
                    Debug.LogWarning("Audio Clip or Audio Clip Name is null, check Audio Clips Datas");
                    continue;
                }
                if (_audioClips.ContainsKey(audioClipsData.audioClipNames[i]))
                {
                    Debug.LogWarning($"Audio Clip Name {audioClipsData.audioClipNames[i]} already exists, check Audio Clips Datas");
                    continue;
                }
                _audioClips.Add(audioClipsData.audioClipNames[i], audioClipsData.audioClips[i]);
            }
                
            InitializePool(startingSourceCount);
        }

        public static MyAudioSource Play(string audioClipName, bool loop)
        {
            MyAudioSource audioSource = GetAudioSource(audioClipName, loop);
            if (audioSource != null)
            {
                audioSource.Play();
                return audioSource;
            }
            return null;
        }
        
        public static void Stop(MyAudioSource audioSource)
        {
            if (!audioSource) return;
            _this.StopSource(audioSource);
        }

        private static MyAudioSource GetAudioSource(string audioClipName, bool loop)
        {
            MyAudioSource audioSource = _this.GetObject(true);
            audioSource.Init(_this._audioClips[audioClipName], loop);
            if (audioSource.Clip == null)
            {
                Debug.LogWarning("Invalid Audio Source Name");
                return null;
            }
            return audioSource;
        }

        private void StopSource(MyAudioSource audioSource) => ReleaseObject(audioSource);
    }
}

