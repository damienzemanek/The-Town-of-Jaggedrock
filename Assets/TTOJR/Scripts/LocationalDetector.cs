using System;
using DependencyInjection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class LocationalDetector : Detector
{

    [BoxGroup("Enable Events")][SerializeField] bool onEnter;
    [BoxGroup("Enable Events")][SerializeField] bool onStay;
    [BoxGroup("Enable Events")][SerializeField] bool onExit;

    [ShowIf("onEnter")][BoxGroup("Change Data When")][SerializeField] bool onEnterData;
    [ShowIf("onStay")][BoxGroup("Change Data When")][SerializeField] bool onStayData;
    [ShowIf("onExit")][BoxGroup("Change Data When")][SerializeField] bool onExitData;

    [ShowIf("onEnter")] public UnityEvent Enter;
    [ShowIf("onStay")] public UnityEvent Stay;
    [ShowIf("onExit")] public UnityEvent Exit;

    [ShowIf("onEnter")] [SerializeReference] public IState enterState;
    [ShowIf("onStay")] [SerializeReference] public IState stayState;
    [ShowIf("onExit")] [SerializeReference] public IState exitState;
    [field: SerializeField] public LayerMask locationMask { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if ((locationMask.value & (1 << other.gameObject.layer)) == 0) return;
        if (onEnter) Enter?.Invoke();
        if (onEnterData) other.GetComponent<StateAgent>().UpdateState(enterState, enterState.GetDataValue());
    }

    private void OnTriggerStay(Collider other)
    {
        if ((locationMask.value & (1 << other.gameObject.layer)) == 0) return;
        if (onStay) Stay?.Invoke();
        if (onStayData) other.GetComponent<StateAgent>().UpdateState(stayState, stayState.GetDataValue());

    }

    private void OnTriggerExit(Collider other) 
    {
        if ((locationMask.value & (1 << other.gameObject.layer)) == 0) return;
        if (onExit) Exit?.Invoke();
        if (onExitData) other.GetComponent<StateAgent>().UpdateState(exitState, exitState.GetDataValue());

    }

}





