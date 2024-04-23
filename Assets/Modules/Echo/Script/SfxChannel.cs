using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace SETHD.Echo
{
    public class SfxChannel : IAudioChannel
    {
        private bool isPaused;
        private readonly List<AudioSource> actives;
        private readonly Dictionary<string, AudioClip> data;
        private readonly AudioSourceProvider audioSourceProvider;

        public void Reinitialize(AudioBank audioBank)
        {
            data.Clear();
            
            for (int i = 0; i < audioBank.Audios.Length; i++)
            {
                data.Add(audioBank.Audios[i].key, audioBank.Audios[i].clip);
            }
        }

        public async UniTask Play(string key)
        {
            var rented = audioSourceProvider.Rent();
            actives.Add(rented);
            Assert.IsTrue(data.ContainsKey(key));
            rented.clip = data[key];
            rented.Play();
        }

        public async UniTask Pause()
        {
            foreach (var source in actives)
            {
                if (isPaused)
                {
                    source.UnPause();
                    return;
                }
                
                source.Pause();
            }

            isPaused = !isPaused;
        }

        public async UniTask Stop()
        {
            foreach (var source in actives)
            {
                source.Stop();
                audioSourceProvider.Return(source);
            }
        }
    }
}