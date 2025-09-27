using DependencyInjection;
using UnityEngine;

namespace DependencyInjection
{
    public class RuntimeInjectableMonoBehaviour : MonoBehaviour
    {
        protected virtual void OnInstantiate()
        {
            Injector.Instance.RuntimeInject(this);
        }
        protected virtual void Awake() => OnInstantiate();
    }
}
