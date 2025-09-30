using UnityEngine;
using UnityEngine.Events;

public class CallbackDetector : Detector
{
    public UnityEvent callback;

    protected override void OnTriggerEnter(Collider other)
    {
        if (!IsInLayer(other)) return;

        if (other.gameObject.GetComponent<Interactor>())
            other.gameObject.GetComponent<Interactor>().SetInteractEvent(callback);

        base.OnTriggerEnter(other);

    }

    protected override void OnTriggerStay(Collider other)
    {
        if (!IsInLayer(other)) return;

        if (other.gameObject.GetComponent<Interactor>())
            other.gameObject.GetComponent<Interactor>().SetInteractEvent(callback);

        base.OnTriggerStay(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (!IsInLayer(other)) return;

        if (other.gameObject.GetComponent<Interactor>())
            other.gameObject.GetComponent<Interactor>().SetInteractEvent(callback);

        base.OnTriggerExit(other);

    }

}
