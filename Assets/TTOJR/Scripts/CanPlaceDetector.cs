using UnityEngine;


public class CanPlaceDetector : LocationalDetector<CanPlace.Data>
{
    protected override void UpdateStateData(Collider other, CanPlace.Data newData)
    {
        if (!IsInLayer(other)) return;
        var agentData = (CanPlace.Data)other.GetComponent<StateAgent>().GetState(enterState).GetDataValue();
        agentData.SetCanPlace(newData.canPlace);
        agentData.SetObjectToPlace(newData.objectToPlace);
        agentData.SetPlaceLocation(newData.placeLocation);
    }
}
