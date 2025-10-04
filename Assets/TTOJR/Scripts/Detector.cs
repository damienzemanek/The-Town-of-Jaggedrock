using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Detector : MonoBehaviour
{
    public bool rayCastDetector;
    bool notCaster => !rayCastDetector;
    GameObject obj;

    [ShowIf("rayCastDetector")] public bool raycasted;
    [ShowIf("rayCastDetector")] public GameObject casterObject { get => obj; set => obj = value; }
    [ShowIf("notCaster")] public GameObject colliderObject { get => obj; set => obj = value; }

    [BoxGroup("Enable Unity Events")][SerializeField] protected bool onEnter;
    [BoxGroup("Enable Unity Events")][SerializeField] protected bool onStay;
    [BoxGroup("Enable Unity Events")][SerializeField] protected bool onExit;

    [ShowIf("onEnter")] public UnityEvent Enter;
    [ShowIf("onStay")] public UnityEvent Stay;
    [ShowIf("onExit")] public UnityEvent Exit;

    [field: SerializeField] public LayerMask locationMask { get; private set; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (rayCastDetector) return;
        if (!IsInLayer(other)) return;
        obj = other.gameObject;
        if (onEnter) Enter?.Invoke();
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (rayCastDetector) return;
        if (!IsInLayer(other)) return;
        obj = other.gameObject;
        if (onStay) Stay?.Invoke();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (rayCastDetector) return;
        if (!IsInLayer(other)) return;
        obj = null;
        if (onExit) Exit?.Invoke();
    }

    protected bool IsInLayer(Collider other)
    {
        if ((locationMask.value & (1 << other.gameObject.layer)) == 0) return false;
        else return true;
    }

    protected bool CasterInLayer(GameObject other)
    {
        if ((locationMask.value & (1 << other.gameObject.layer)) == 0) return false;
        else return true;
    }

    GameObject casterBuffer = null;

    public virtual void OnRaycastedEnter(GameObject caster)
    {
        if (!rayCastDetector) return;
        if (!CasterInLayer(caster)) return;
        //print("raycast enter");
        obj = caster.gameObject;
        casterBuffer = caster;
        CancelInvoke(nameof(DisableRaycasted));
        Invoke(nameof(DisableRaycasted), 0.1f);

        raycasted = true;

        if (onEnter) Enter?.Invoke();
    }

    public virtual void OnRaycastedExit(GameObject caster)
    {
        if (!rayCastDetector) return;
        if (!CasterInLayer(casterBuffer)) return;
        obj = null;
        if (onExit) Exit?.Invoke();
    }

    void DisableRaycasted()
    {
        print("Disabled raycast");
        raycasted = false;
        OnRaycastedExit(casterBuffer);
    }

}
