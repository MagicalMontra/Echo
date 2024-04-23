
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace SETHD.Utilities
{
    public class Invoker : MonoBehaviour
    {
        [Serializable]
        public enum InvokeCycle
        {
            Start,
            Frameone,
            Custom = 0
        }

        private bool hasInvoked;
        
        [SerializeField]
        private float delay = 0;
        
        [SerializeField]
        private InvokeCycle cycle;

        [SerializeField]
        private UnityEvent events;

        private async UniTaskVoid Start()
        {
            if (cycle != InvokeCycle.Start)
                return;

            await UniTask.Delay(Mathf.CeilToInt(1000 * delay), DelayType.DeltaTime);
            events?.Invoke();
        }
        
        private async UniTaskVoid Update()
        {
            if (hasInvoked)
                return;
            
            if (cycle != InvokeCycle.Frameone)
                return;

            await UniTask.DelayFrame(1);
            await UniTask.Delay(Mathf.CeilToInt(1000 * delay), DelayType.DeltaTime);
            events?.Invoke();
            hasInvoked = true;
        }
    }
}