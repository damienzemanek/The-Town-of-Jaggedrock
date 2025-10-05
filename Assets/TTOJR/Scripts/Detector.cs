using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Detector : MonoBehaviour
{
    protected bool somethingCollided;
    public bool rayCastDetector;
    bool notCaster => !rayCastDetector;
    [SerializeField] protected GameObject obj;

    [ShowIf("rayCastDetector")] public bool raycasted;
    [ShowIf("rayCastDetector")] public GameObject casterObject { get => obj; set => obj = value; }
    [ShowIf("notCaster")] public GameObject colliderObject { get => obj; set => obj = value; }

    [PropertyOrder(0)][BoxGroup("Enable Unity Events")][SerializeField] protected bool onEnter;
    [PropertyOrder(1)][BoxGroup("Enable Unity Events")][ShowIf("onEnter")] public UnityEvent Enter;
    [PropertyOrder(2)][BoxGroup("Enable Unity Events")][SerializeField] protected bool onStay;
    [PropertyOrder(3)][BoxGroup("Enable Unity Events")][ShowIf("onStay")] public UnityEvent Stay;
    [PropertyOrder(4)][BoxGroup("Enable Unity Events")][SerializeField] protected bool onExit;
    [PropertyOrder(5)][BoxGroup("Enable Unity Events")][ShowIf("onExit")] public UnityEvent Exit;

    [field: SerializeField] public LayerMask locationMask { get; private set; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!onEnter) return;
        if (rayCastDetector) return;
        if (!IsInLayer(other)) return;
        obj = other.gameObject;
        print("Detector: Enter");
        Enter?.Invoke();
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (!IsInLayer(other)) return;
        somethingCollided = true;
        if (!onStay) return;
        if (rayCastDetector) return;
        obj = other.gameObject;
        print("Detector: Stay");
        Stay?.Invoke();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!onExit) return;
        if (rayCastDetector) return;
        if (!IsInLayer(other)) return;
        print("Detector: Exit");
        Exit?.Invoke();
        obj = null;
        somethingCollided = false;
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
        if (!onEnter) return;
        if (!rayCastDetector) return;
        if (!CasterInLayer(caster)) return;
        //print("raycast enter");
        obj = caster.gameObject;
        casterBuffer = caster;
        CancelInvoke(nameof(DisableRaycasted));
        Invoke(nameof(DisableRaycasted), 0.1f);

        raycasted = true;

        Enter?.Invoke();
    }

    public virtual void OnRaycastedExit(GameObject caster)
    {
        if (!onExit) return;
        if (!rayCastDetector) return;
        if (!CasterInLayer(casterBuffer)) return;
        if (onExit) Exit?.Invoke();
        obj = null;
    }

    void DisableRaycasted()
    {
        print("Disabled raycast");
        raycasted = false;
        OnRaycastedExit(casterBuffer);
    }

}
