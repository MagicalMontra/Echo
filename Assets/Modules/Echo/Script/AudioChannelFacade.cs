using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace SETHD.Echo
{
    public class AudioChannelFacade : MonoBehaviour
    {
        private IAudioChannel audioChannel;

        [Inject]
        public void Construct(IAudioChannel audioChannel)
        {
            this.audioChannel = audioChannel;
        }

        public void Play(string key)
        {
            audioChannel.Play(key, PlayMode.Transit).Forget();
        }
    }
}