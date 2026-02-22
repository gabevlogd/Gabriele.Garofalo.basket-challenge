namespace BasketChallenge.Core
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

#if UNITY_EDITOR
    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerInspector : Editor
    {
        private AudioClipsData _audioClipsData;

        private void Awake()
        {
            _audioClipsData = Resources.Load<AudioClipsData>("AudioClipsData");
            EditorUtility.SetDirty(_audioClipsData);
        }

        public override void OnInspectorGUI()
        {
            DrawClipsList();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("+", GUILayout.Width(30f)))
                    QueueNewClip();
                if (GUILayout.Button("-", GUILayout.Width(30f)) && _audioClipsData.audioClips.Count > 0)
                    DequeueLastClip();
            }

        }

        private void DrawClipsList()
        {
            if (_audioClipsData.audioClips != null)
            {
                for (int i = 0; i < _audioClipsData.audioClips.Count; i++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        _audioClipsData.audioClips[i] = EditorGUILayout.ObjectField(_audioClipsData.audioClips[i], typeof(AudioClip), false) as AudioClip;
                        _audioClipsData.audioClipNames[i] = EditorGUILayout.TextField(_audioClipsData.audioClipNames[i], GUILayout.Width(100f));
                    }

                }
            }
        }

        private void QueueNewClip()
        {
            if (_audioClipsData.audioClips == null)
            {
                _audioClipsData.audioClips = new List<AudioClip>();
                _audioClipsData.audioClipNames = new List<string>();
            }

            _audioClipsData.audioClips.Add(null);
            _audioClipsData.audioClipNames.Add(null);
        }

        private void DequeueLastClip()
        {
            _audioClipsData.audioClips.RemoveAt(_audioClipsData.audioClips.Count - 1);
            _audioClipsData.audioClipNames.RemoveAt(_audioClipsData.audioClipNames.Count - 1);
        }
    }
#endif
}
