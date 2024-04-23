using System;
using UnityEngine;

namespace SETHD.Echo
{
    [Serializable]
    [CreateAssetMenu(menuName = "Create AudioBank", fileName = "AudioBank", order = 0)]
    public class AudioBank : ScriptableObject
    {
        public AudioKeypair[] Audios => audios;
        
        [SerializeField]
        private AudioKeypair[] audios;
    }
}
