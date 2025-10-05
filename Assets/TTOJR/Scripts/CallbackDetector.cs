using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CallbackDetector : Detector
{
    public CallbackFunctionality functionality;
    [ShowIf(nameof(singleCbCheck))][field: SerializeReference] public UnityEvent useCallback;
    [ShowIf(nameof(toggleCbCheck))] [field: SerializeReference] public UnityEvent[] toggleUseCallback;
    [ShowIf(nameof(toggleCbCheck))][SerializeField] int currCallback = 0;
    public enum CallbackFunctionality
    {
        singleCallback,
        toggleCallback
    }

    bool singleCbCheck() => (functionality == CallbackFunctionality.singleCallback);
    bool toggleCbCheck() => (functionality == CallbackFunctionality.toggleCallback);

    private void Awake()
    {
        if(toggleCbCheck())
            for(int i = 0; i < toggleUseCallback.Length; i++)
                toggleUseCallback[i].AddListener(ToggleCallback);

    }
    protected override void EnterImplementationChild(Collider other)
    {
        Functionalities(other.gameObject);
    }

    protected override void StayImplementationChild(Collider other)
    {
        Functionalities(other.gameObject);
    }

    protected override void ExitImplementationChild(Collider other)
    {
        Functionalities(other.gameObject);
    }


    void Functionalities(GameObject other)
    {
        //if (other == null) return;
        if (!other.gameObject.GetComponent<Interactor>()) return;

        if (singleCbCheck())
        { 
            other.gameObject.GetComponent<Interactor>().SetInteractEvent(callback: useCallback); 
            return;
        }
        if (toggleCbCheck())
        {
            other.gameObject.GetComponent<Interactor>().SetInteractEvent(toggleUseCallback[currCallback]);
            return;
        }
    }


    public void ToggleCallback()
    {
        currCallback++;
        if (currCallback >= 2) currCallback = 0;
    }

    public override void OnRaycastedEnter(GameObject caster)
    {
        Functionalities((caster));
        base.OnRaycastedEnter (caster);
    }

    public override void OnRaycastedStay(GameObject caster)
    {
        Functionalities((caster));
        base.OnRaycastedStay(caster);
    }


    public override void OnRaycastedExit(GameObject caster)
    {
        Functionalities((caster));
        base.OnRaycastedExit(caster);
    }

}
