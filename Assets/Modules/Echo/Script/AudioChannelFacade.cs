using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace SETHD.Echo
{
    public class AudioChannelFacade : MonoBehaviour
    {
        private AudioBank audioBank;
        private IAudioChannel audioChannel;

        [Inject]
        public void Construct(IAudioChannel audioChannel, AudioBank audioBank)
        {
            this.audioBank = audioBank;
            this.audioChannel = audioChannel;
        }

        public void Play(int index)
        {
            audioChannel.Play(audioBank.Audios[index].key).Forget();
        }
    }
}