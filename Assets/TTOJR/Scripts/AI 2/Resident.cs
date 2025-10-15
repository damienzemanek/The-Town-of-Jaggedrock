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
    public float stuckTimeout = 8f;
    public float retryCooldown = 1.5f;
    public float nextRetryTime = 0f;
    public float stopCheckProgressValue = 0f;
    [ReadOnly] public ResidentAreas lastAreaInContact;

    public void SpawnAtLocation(ResidentAreas spawnArea)
    {
        lastAreaInContact = spawnArea;
        StartCoroutine(UseArea(spawnArea));
    }


    void TryUseArea()
    {
        if (usingArea) return;
        if (lastAreaInContact == null) Debug.Log("Resident: last are in contact null");
        if (agent == null) Debug.Log("Resident: Does not have an agent (null)");
        if (!agent.isOnNavMesh) return;
        if (agent.pathPending) return;

        StartCoroutine(UseArea(lastAreaInContact));
        nextRetryTime = Time.time + retryCooldown;
        stopCheckProgressValue = 0f;
    }
    
    public IEnumerator UseArea(ResidentAreas area)
    {
        usingArea = true;
        stopped = false;

        yield return new WaitForSeconds(delayUseArea);

        if (area == null) throw new System.Exception("Resident: No area found to use");
        print("Resident: Using Area");
        area.RemoveAgentToActions();
        area.SetAgentInAllActions(agent);
        area.SetResidentInAllActions(this);
        area.choices?.DoAnAction(area);
        usingArea = false;
        stopCheckProgressValue = 0;
    }

    private void FixedUpdate()
    {
        stopCheckProgressValue += Time.fixedDeltaTime;

        CheckIfStoppedSetStopped();

        if(Time.time >= nextRetryTime)
        {
            if (stopped) TryUseArea();
            else if (stopCheckProgressValue > stuckTimeout) TryUseArea();
        }
    }

    //This will check if we are stopped
    void CheckIfStoppedSetStopped()
    {
        if (agent == null) throw new System.Exception("Resident: No Agent found");

        if (stopped) return;
        if (agent.pathPending) return;
        if (!agent.isOnNavMesh) return;


        if(agent.pathStatus == NavMeshPathStatus.PathInvalid ||
        agent.pathStatus == NavMeshPathStatus.PathPartial)
            StopInArea();


        //Close Enough
        if (agent.velocity.magnitude > 0.1f) return;
        if (agent.remainingDistance > agent.stoppingDistance + 0.2f) return;


        StopInArea();
    }

    void StopInArea()
    {
        if (lastAreaInContact == null) Debug.LogError("Resident: Stopped in area that is null");
        if(lastAreaInContact.whoIsHere != null)
            if(!lastAreaInContact.whoIsHere.Contains(this))
                lastAreaInContact.whoIsHere.Add(this);

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

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & residentAreaMask) == 0) return;
        if (!other.gameObject.TryGetComponent<ResidentAreas>(out ResidentAreas area)) return;
        area.whoIsHere?.Remove(this);
    }


    private void OnDisable()
    {
        if (lastAreaInContact != null)
            lastAreaInContact.whoIsHere?.Remove(this);
    }



}
