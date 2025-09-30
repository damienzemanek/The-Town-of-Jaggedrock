using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Detector : MonoBehaviour
{
    [BoxGroup("Enable Unity Events")][SerializeField] protected bool onEnter;
    [BoxGroup("Enable Unity Events")][SerializeField] protected bool onStay;
    [BoxGroup("Enable Unity Events")][SerializeField] protected bool onExit;

    [ShowIf("onEnter")] public UnityEvent Enter;
    [ShowIf("onStay")] public UnityEvent Stay;
    [ShowIf("onExit")] public UnityEvent Exit;

    [field: SerializeField] public LayerMask locationMask { get; private set; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!IsInLayer(other)) return;
        if (onEnter) Enter?.Invoke();
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (!IsInLayer(other)) return;
        if (onStay) Stay?.Invoke();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!IsInLayer(other)) return;
        if (onExit) Exit?.Invoke();
    }

    protected bool IsInLayer(Collider other)
    {
        if ((locationMask.value & (1 << other.gameObject.layer)) == 0) return false;
        else return true;
    }

}
