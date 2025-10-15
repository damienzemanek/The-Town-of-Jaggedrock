using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class ResidentAreas : MonoBehaviour
{
    [SerializeReference] public HashSet<Resident> whoIsHere = new HashSet<Resident>();
    [SerializeReference] public ActionChoices choices;

    public float fixedY;
    Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        if (choices == null) Debug.LogError(message: "ResidentArea: Choices not set");
    }

    private void Start()
    {
        string ResidentAreaLayerMask = "ResidentArea";
        if (gameObject.layer != LayerMask.NameToLayer(ResidentAreaLayerMask))
            throw new System.Exception($"ResidentArea: ({gameObject.name})'s Layer is not set correctly, set it to ResidentArea");

        gameObject.layer = LayerMask.NameToLayer(ResidentAreaLayerMask);
    }

    public Vector3 GetARandLocation()
    {
        if (collider == null) throw new System.Exception("ResidentAreas: Collider not found, add it");
        Bounds bounds = collider.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        Vector3 randomPoint = new Vector3(x, fixedY, z);

        //loc the loc to nav mesh
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            return hit.position;

        return randomPoint;
    }

    public void SetAgentInAllActions(NavMeshAgent agentInContact)
    {
        print($"ResidentArea: {name} Setting NavMeshAgent in my list of Actions");
        choices.actions.ForEach(a => a.action.SetAgent(agentInContact));
    }
    public void SetResidentInAllActions(Resident resident)
    {
        choices.actions.ForEach(a => a.action.SetResident(resident));
    }

    public void RemoveAgentToActions() =>
                choices.actions.ForEach(a => a.action.SetAgent(null));

    void OnValidate()
    {
        choices.actions.ForEach(wa => wa.action.fromChoices = choices);
    }
}
