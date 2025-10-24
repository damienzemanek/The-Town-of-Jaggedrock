using System;
using System.Collections.Generic;
using NUnit.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using ShowIfAttribute = Sirenix.OdinInspector.ShowIfAttribute;

public class Use : MonoBehaviour
{
    public UnityEvent additionalActions;

    [SerializeReference] public List<UseAction> actions;

    public void UseActions()
    {
        additionalActions?.Invoke();
        actions.ForEach(a => a.Execute());
    }
}

[Serializable]
public abstract class UseAction
{
    public abstract void Execute();
}

[Serializable]
public class Display : UseAction
{
    public GameObject objToDisplay;
    public bool on = true;
    public override void Execute()
    {
        Toggle();
    }

    void Toggle()
    {
        on = !on;
        objToDisplay.SetActive(on);
    }
}


