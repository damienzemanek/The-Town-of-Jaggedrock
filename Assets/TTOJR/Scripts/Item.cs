using System;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite icon;
    [ShowInInspector] [SerializeReference] public IState stateAndFunctionality;


}

[Serializable]
public abstract class UseFunctionality<T>
{
    public abstract void Use(T data);
}

[Serializable]
class Place : UseFunctionality<CanPlace.Data>
{
    public override void Use(CanPlace.Data agentData)
    {
        if (agentData.usedUp) return;
        if (!agentData.canPlace || !agentData.hasRequiredItem) return;
        Debug.Log($"Item: Placing Item {agentData.objectToPlace.name} ");

        agentData.Use();
        GameObject spawned = UnityEngine.Object.Instantiate(
            agentData.objectToPlace,
            agentData.placeLocation.position,
            Quaternion.identity,
            agentData.placeLocation
            );
        
    }
}

[Serializable]
class Spray : UseFunctionality<CanSpray.Data>
{
    public override void Use(CanSpray.Data agentData)
    {
        if (!agentData.canSpray || !agentData.hasRequiredItem) return;
        Debug.Log($"Item: Spraying Item {agentData} ");

        if (agentData.sprayDestination.preventContact) return;
        agentData.sprayDestination.MakeContact();

    }
}



