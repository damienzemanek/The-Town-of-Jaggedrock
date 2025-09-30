using System;
using DependencyInjection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class StateChangeDetector<T> : Detector
{
    [ShowIf("onEnter")] [SerializeReference] public IState enterState;
    [ShowIf("onStay")] [SerializeReference] public IState stayState;
    [ShowIf("onExit")] [SerializeReference] public IState exitState;
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!onEnter || enterState == null) return;
        print("Detector: Can Place Enter Check");
        var newData = (T)enterState.GetDataValue();
        UpdateStateData(other, newData);
    }

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        if (!onStay || stayState == null) return;
        var newData = (T)stayState.GetDataValue();
        UpdateStateData(other, newData);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (!onExit || exitState == null) return;
        var newData = (T)exitState.GetDataValue();
        UpdateStateData(other, newData);
    }
    protected virtual void UpdateStateData(Collider other, T data) { }
}






