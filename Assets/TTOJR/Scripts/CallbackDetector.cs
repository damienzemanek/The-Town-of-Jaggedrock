using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CallbackDetector : Detector
{
    public CallbackFunctionality functionality;
    [ShowIf(nameof(singleCbCheck))][field: SerializeReference] public UnityEvent callback;
    [ShowIf(nameof(toggleCbCheck))] [field: SerializeReference] public UnityEvent[] toggleCallback;
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
            for(int i = 0; i < toggleCallback.Length; i++)
                toggleCallback[i].AddListener(ToggleCallback);

    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!IsInLayer(other)) return;

        Functionalities(other.gameObject);

        base.OnTriggerEnter(other);

    }

    protected override void OnTriggerStay(Collider other)
    {
        if (!IsInLayer(other)) return;

        Functionalities(other.gameObject);

        base.OnTriggerStay(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (!IsInLayer(other)) return;

        Functionalities((other.gameObject));

        base.OnTriggerExit(other);

    }

    void Functionalities(GameObject other)
    {
        if (!other.gameObject.GetComponent<Interactor>()) return;

        if (singleCbCheck())
        { 
            other.gameObject.GetComponent<Interactor>().SetInteractEvent(callback); 
            return;
        }
        if (toggleCbCheck())
        {
            other.gameObject.GetComponent<Interactor>().SetInteractEvent(toggleCallback[currCallback]);
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
        if (!CasterInLayer(caster)) return;

        Functionalities((caster));

        base.OnRaycastedEnter (caster);
    }

    public override void OnRaycastedExit(GameObject caster)
    {
        if (!CasterInLayer(caster)) return;

        Functionalities((caster));

        base.OnRaycastedExit(caster);
    }

}
