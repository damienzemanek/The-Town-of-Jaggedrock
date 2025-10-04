using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;


public class PreRequisiteStateChangeDetector : CallbackDetector
{

    public static List<Action<bool>> callbacks = new List<Action<bool>>();

    bool _preRequisite;
    public void SetPreRequisite(bool val) => _preRequisite = val;
    public Action<bool> SetPreReqCb;

    private void Awake()
    {
        SetPreReqCb += SetPreRequisite;
        callbacks.Add(SetPreReqCb);
    }


    protected override void OnTriggerEnter(Collider other)
    {
        if (!_preRequisite) return; if (!IsInLayer(other)) return;
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (!_preRequisite) return; if (!IsInLayer(other)) return;
        base.OnTriggerStay(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (!_preRequisite) return; if (!IsInLayer(other)) return;
        base.OnTriggerExit(other);
    }

}



