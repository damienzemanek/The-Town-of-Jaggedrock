using System;
using DependencyInjection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class LocationalDetector<T> : Detector
{

    [BoxGroup("Enable Unity Events")][SerializeField] protected bool onEnter;
    [BoxGroup("Enable Unity Events")][SerializeField] protected bool onStay;
    [BoxGroup("Enable Unity Events")][SerializeField] protected bool onExit;

    [ShowIf("onEnter")] [SerializeReference] public IState enterState;
    [ShowIf("onStay")] [SerializeReference] public IState stayState;
    [ShowIf("onExit")] [SerializeReference] public IState exitState;

    [ShowIf("onEnter")] public UnityEvent Enter;
    [ShowIf("onStay")] public UnityEvent Stay;
    [ShowIf("onExit")] public UnityEvent Exit;
    [field: SerializeField] public LayerMask locationMask { get; private set; }

    protected void OnTriggerEnter(Collider other)
    {
        if (!onEnter || enterState == null) return;
        print("Detector: Can Place Enter Check");
        var newData = (T)enterState.GetDataValue();
        UpdateStateData(other, newData);
    }

    protected void OnTriggerStay(Collider other)
    {
        if (!onStay || stayState == null) return;
        var newData = (T)stayState.GetDataValue();
        UpdateStateData(other, newData);
    }

    protected void OnTriggerExit(Collider other)
    {
        if (!onExit || exitState == null) return;
        var newData = (T)exitState.GetDataValue();
        UpdateStateData(other, newData);
    }
    protected virtual void UpdateStateData(Collider other, T data) { }
    protected bool IsInLayer(Collider other)
    {
        if ((locationMask.value & (1 << other.gameObject.layer)) == 0) return false;
        else return true;
    }
}






