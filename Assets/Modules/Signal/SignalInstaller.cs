using UnityEngine;
using Zenject;

namespace SETHD
{
    [CreateAssetMenu(menuName = "Create SignalInstaller", fileName = "SignalInstaller", order = 0)]
    public class SignalInstaller : ScriptableObjectInstaller<SignalInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
        }
    }
}
