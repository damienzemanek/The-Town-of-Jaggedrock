using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Detector : MonoBehaviour
{
    protected bool somethingCollided;
    public bool rayCastDetector;
    public bool collisionDetector;
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
        if (!collisionDetector) return;
        if (!onEnter) return;
        if (!IsInLayer(other)) return;
        obj = other.gameObject;
        print("Detector: Enter");
        EnterImplementationChild(other);
        Enter?.Invoke();
    }
    protected virtual void EnterImplementationChild(Collider other) { }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (!collisionDetector) return;
        if (!IsInLayer(other)) return;
        somethingCollided = true;
        if (!onStay) return;
        obj = other.gameObject;
        print("Detector: Stay");
        StayImplementationChild(other);
        Stay?.Invoke();
    }
    protected virtual void StayImplementationChild(Collider other) { }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!collisionDetector) return;
        if (!onExit) return;
        if (!IsInLayer(other)) return;
        print("Detector: Exit");
        ExitImplementationChild(other);
        Exit?.Invoke();
        obj = null;
        somethingCollided = false;
        
    }
    protected virtual void ExitImplementationChild(Collider other) { }


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
    public bool raycastEntered = false;
    public virtual void OnRaycastedEnter(GameObject caster)
    {
        if (!rayCastDetector) return;
        if (!onEnter) return;
        if (!CasterInLayer(caster)) return;
        if (raycastEntered) return;
        raycastEntered = true;
        obj = caster.gameObject;
        casterBuffer = caster;
        raycasted = true;

        Enter?.Invoke();
        print("raycast enter");
    }

    public virtual void OnRaycastedStay(GameObject caster)
    {
        if (!rayCastDetector) return;
        if (!raycastEntered) OnRaycastedEnter(caster);
        CancelInvoke(nameof(DisableRaycasted));
        Invoke(nameof(DisableRaycasted), 0.1f);


        if (!onStay) return;
        if (!CasterInLayer(other: caster)) return;
        print("raycast stay");
        obj = caster.gameObject;
        casterBuffer = caster;
        raycasted = true;

        Stay?.Invoke();
    }


    public virtual void OnRaycastedExit(GameObject caster)
    {
        print(message: "Detector: attempting to raycast exit");
        if (!rayCastDetector) return;
        if (!onExit) return;
        print(message: "Detector: raycast exit");
        Exit?.Invoke();
        obj = null;
        raycasted = false;
        raycastEntered = false;
    }

    void DisableRaycasted()
    {
        print($"Disabling raycast with casterbuffer {casterBuffer}");
        OnRaycastedExit(caster: casterBuffer);
    }

}
