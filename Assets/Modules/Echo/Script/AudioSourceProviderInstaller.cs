using UnityEngine;
using Zenject;

namespace SETHD.Echo
{
    public class AudioSourceProviderInstaller : MonoInstaller<AudioSourceProviderInstaller>
    {
        [Inject]
        private Transform group;
        
        public override void InstallBindings()
        {
            Container.Bind<AudioSourceProvider>().AsSingle();
            Container.Bind<Transform>().FromInstance(group).AsSingle();
            Container.BindFactory<AudioSource, AudioSourceFactory>().FromNewComponentOnNewGameObject().UnderTransform(group);
        }
    }
}