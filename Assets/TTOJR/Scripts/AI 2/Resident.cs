using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.AI;

public class Resident : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask residentAreaMask;
    public float delayUseArea;
    public bool stopped = false;
    public bool usingArea = false;
    [ReadOnly] public ResidentAreas lastAreaInContact;

    private void Start()
    {
        StartCoroutine(CheckIfStoppedThenUse());
    }

    public void SpawnAtLocation(ResidentAreas spawnArea) => StartCoroutine(UseArea(spawnArea));
    public IEnumerator UseArea(ResidentAreas area)
    {
        if (usingArea) yield break;
        usingArea = true;
        stopped = false;
        yield return new WaitForSeconds(delayUseArea);
        if (area == null) throw new System.Exception("Resident: No area found to use");
        print("Resident: Using Area");
        area.RemoveAgentToActions();
        area.SetAgentInAllActions(agent);
        area.SetResidentInAllActions(this);
        area.choices.DoAnAction();
        usingArea = false;
    }

    private void FixedUpdate()
    {
        CheckIfStoppedSetStopped();
    }

    //This will check if we are stopped
    void CheckIfStoppedSetStopped()
    {
        if (stopped) return;
        if (agent.pathPending) return;
        if (agent.remainingDistance > agent.stoppingDistance + 0.2f) return;
        if (agent.velocity.magnitude != 0f) return;
        print("Resident: Stopped");
        stopped = true;
    }


    //This will get the area we are currently in
    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & residentAreaMask) == 0) return;
        if (!other.gameObject.TryGetComponent<ResidentAreas>(out ResidentAreas area)) return;
        lastAreaInContact = area;
    }

    IEnumerator CheckIfStoppedThenUse()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayUseArea);
            if (stopped) StartCoroutine(UseArea(lastAreaInContact));
        }
    }


}
