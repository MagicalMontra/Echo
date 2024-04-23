using Cysharp.Threading.Tasks;

namespace SETHD.Echo
{
    public interface IAudioChannel
    {
        void Reinitialize(AudioBank audioBank);
        UniTask Play(string key);

        UniTask Pause();

        UniTask Stop();
    }
}