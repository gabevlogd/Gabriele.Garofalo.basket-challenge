using System.Collections.Generic;
using UnityEngine;

namespace BasketChallenge.Core
{
    /// <summary>
    /// This class contains the data of all audio clips of the game
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Audio Clips Datas", fileName = "Audio Clips Datas")]
    public class AudioClipsData : ScriptableObject
    {
        public List<AudioClip> audioClips;
        public List<string> audioClipNames;
    }
}
