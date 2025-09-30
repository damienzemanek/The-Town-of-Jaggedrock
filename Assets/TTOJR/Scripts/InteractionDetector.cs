using UnityEngine;

public class InteractionDetector : LocationalDetector<CanUnlock.Data>
{
    protected override void UpdateStateData(Collider other, CanUnlock.Data newData)
    {
        if (!IsInLayer(other)) return;
        var agentData = (CanUnlock.Data)other.GetComponent<StateAgent>().GetState(enterState).GetDataValue();

    }

}
