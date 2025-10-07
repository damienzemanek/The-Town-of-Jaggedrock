using DependencyInjection;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CallbackDetector))]
public class Searchable : MonoBehaviour
{
    [field:SerializeField] public float increment { get; private set; }
    [field: SerializeField] public float progress { get; private set; }
    [field: SerializeField] public float completeProgressValue { get; private set; }
    [field: SerializeField] public bool complete { get; private set; }
    [field: SerializeField] public UnityEvent completeEvent { get; private set; }

    CallbackDetector cbDetector;
    [Inject] EntityControls controls;

    private void Awake()
    {
        cbDetector = GetComponent<CallbackDetector>();

        AssignUseHoldBacks();
    }
    public void IncreaseProgress()
    {
        print("Searchable: Increasing progress");
        if (complete) return;

        progress += increment;

        if (progress > completeProgressValue)
            Complete();
    }

    public void ResetProgress()
    {
        progress = 0;
    }

    void Complete()
    {
        completeEvent?.Invoke();
        progress = completeProgressValue;
        complete = true;
        controls.ForceStopHold();
    }

    void AssignUseHoldBacks()
    {
        cbDetector.useCallback.AddListener( () => IncreaseProgress());
        cbDetector.holdCancledCallback.AddListener( () => ResetProgress());
    }

}
