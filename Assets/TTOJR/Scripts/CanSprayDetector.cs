using UnityEngine;


public class CanSprayDetector : LocationalDetector<CanSpray.Data>
{
    protected override void UpdateStateData(Collider other, CanSpray.Data newData)
    {
        if (!IsInLayer(other)) return;
        var agentData = (CanSpray.Data)other.GetComponent<StateAgent>().GetState(enterState).GetDataValue();
        agentData.SetCanSpray(newData.canSpray);
        agentData.SetSprayLocation(newData.sprayDestination);
    }
}
