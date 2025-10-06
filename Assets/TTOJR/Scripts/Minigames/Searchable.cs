using UnityEngine;
using UnityEngine.Events;

public class Searchable : MonoBehaviour
{
    [field:SerializeField] public float increment { get; private set; }
    [field: SerializeField] public float progress { get; private set; }
    [field: SerializeField] public float completeProgressValue { get; private set; }
    [field: SerializeField] public bool complete { get; private set; }
    [field: SerializeField] public UnityEvent completeEvent { get; private set; }

    public void IncreaseProgress()
    {
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
    }

}
