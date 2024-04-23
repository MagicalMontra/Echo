using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SETHD.Echo
{
    public class CrossfadeBGMChannel : IAudioChannel
    {
        private bool isPaused;
        private string currentKey;
        private readonly List<UniTask> crossfadeTasks;

        private readonly Dictionary<string, AudioSource> actives;
        private readonly AudioSourceProvider audioSourceProvider;

        public CrossfadeBGMChannel(AudioBank audioBank, AudioSourceProvider audioSourceProvider)
        {
            crossfadeTasks = new List<UniTask>();
            actives = new Dictionary<string, AudioSource>();
            this.audioSourceProvider = audioSourceProvider;
            Reinitialize(audioBank);
        }

        public void Reinitialize(AudioBank audioBank)
        {
            foreach (var pair in actives)
            {
                audioSourceProvider.Return(pair.Value);
            }
            
            actives.Clear();
            
            for (int i = 0; i < audioBank.Audios.Length; i++)
            {
                var rented = audioSourceProvider.Rent();
                rented.clip = audioBank.Audios[i].clip;
                rented.loop = true;
                rented.volume = 0;
                rented.Play();
                actives.Add(audioBank.Audios[i].key, rented);
            }
        }

        public async UniTask Play(string key)
        {
            if (string.IsNullOrEmpty(currentKey))
            {
                currentKey = key;
                await Crossfade(key);
            }
            
            if (currentKey == key)
                return;
            
            currentKey = key;
            await Crossfade(key);
        }

        public async UniTask Pause()
        {
            foreach (var pair in actives)
            {
                if (isPaused)
                {
                    pair.Value.UnPause();
                    return;
                }
                
                pair.Value.Pause();
            }

            isPaused = !isPaused;
        }

        public async UniTask Stop()
        {
            foreach (var pair in actives)
            {
                pair.Value.Stop();
            }
        }
        
        private async UniTask Crossfade(string key)
        {
            crossfadeTasks.Clear();
            
            foreach (var pair in actives)
            {
                if (pair.Key == key)
                {
                    crossfadeTasks.Add(FadeUpTask(1f, pair.Value));
                    continue;
                }
                
                crossfadeTasks.Add(FadeDownTask(0f, pair.Value));
            }

            await UniTask.WhenAll(crossfadeTasks);
        }

        private async UniTask FadeUpTask(float targetVolume, AudioSource targetSource)
        {
            int stepResolution = 100;
            float duration = 1f;
            var volumeStep = (targetVolume - targetSource.volume) / stepResolution;
            var waitTime = duration / stepResolution;
            
            while (targetSource.volume < targetVolume)
            {
                targetSource.volume += volumeStep;
                await UniTask.Delay(Mathf.CeilToInt(1000 * waitTime), DelayType.DeltaTime);
            }
                    
            targetSource.volume = targetVolume;
        }
        
        private async UniTask FadeDownTask(float targetVolume, AudioSource targetSource)
        {
            int stepResolution = 100;
            float duration = 1f;
            var volumeStep = (targetSource.volume - targetVolume) / stepResolution;
            var waitTime = duration / stepResolution;
            
            while (targetSource.volume > targetVolume)
            {
                targetSource.volume -= volumeStep;
                await UniTask.Delay(Mathf.CeilToInt(1000 * waitTime) , DelayType.DeltaTime);
            }
                    
            targetSource.volume = targetVolume;
        }
    }
}