using UnityEngine;
using UnityEngine.AI;
using System;
using Sirenix.OdinInspector;
using System.Collections;

[Serializable]
public abstract class ActionDo 
{
    [HideInInspector] [field:ReadOnly] [field:SerializeField] public NavMeshAgent agent { get; protected set; }
    [HideInInspector][field: ReadOnly][field: SerializeField] public Resident resident { get; protected set; }
    [HideInInspector][field: SerializeField] public ActionChoices fromChoices { get; set; }

    public abstract void Execute(ResidentAreas area);
    public void SetAgent(NavMeshAgent agent) => this.agent = agent;
    public void SetResident(Resident resident) => this.resident = resident;
}


[Serializable]
public class StandHere : ActionDo
{
    [SerializeField] public Vector2 timeStanding = new Vector2(3, 6);
    public override void Execute(ResidentAreas area)
    {
        resident.StartCoroutine(Stand(area));
    }

    IEnumerator Stand(ResidentAreas area)
    {
        Debug.Log($"ActionDo: (Standing) at area {area.gameObject.name}");
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        float standFor = UnityEngine.Random.Range(minInclusive: timeStanding.x, timeStanding.y);
        yield return new WaitForSeconds(seconds: standFor);
        fromChoices.DoAnAction(area);
    }


}

[Serializable]
public class WalkTo : ActionDo
{
    [field:SerializeField] public ResidentAreas destination { get; protected set; }
    public override void Execute(ResidentAreas area)
    {
        Debug.Log($"ActionDo: (Walking) to area {destination} from area {area}");
        Walk();
    }

    void Walk()
    {
        agent.isStopped = false;
        resident.stopped = false;
        agent.SetDestination(destination.GetARandLocation());
    }
}
