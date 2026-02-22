using System;
using UnityEngine;

namespace BasketChallenge.Core
{
    /// <summary>
    /// This class, attached to audio sources used by the sound manager, send an event when the audio source have finished to reproduce sound
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class MyAudioSource : MonoBehaviour
    {
        public static event Action<MyAudioSource> AudioClipEnded;
        private AudioSource _audioSource;
        public AudioClip Clip => _audioSource.clip;
        public bool Loop => _audioSource.loop;

        private void Awake()
        {
            if (!TryGetComponent(out _audioSource))
            {
                Debug.LogError("AudioSource component not found");
            }
        }

        private void OnDisable()
        {
            _audioSource.clip = null;
            _audioSource.loop = false;
        }

        private void Update()
        {
            if (!_audioSource.isPlaying)
                AudioClipEnded?.Invoke(this);
        }

        public void Init(AudioClip audioClip, bool loop)
        {
            _audioSource.clip = audioClip;
            _audioSource.loop = loop;
        }

        public void Play() => _audioSource.Play();

    }
}
