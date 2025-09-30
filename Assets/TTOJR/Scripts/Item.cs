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
    public abstract bool Use(T data, Action callback = null);
}

[Serializable]
class Place : UseFunctionality<CanPlace.Data>
{
    public override bool Use(CanPlace.Data agentData, Action cb)
    {
        if (agentData.usedUp) return false;
        if (!agentData.canPlace || !agentData.hasRequiredItem) return false;
        Debug.Log($"Item: Placing Item {agentData.objectToPlace.name}, callback {cb} ");

        agentData.Use( agentData.requiredItem, cb);

        GameObject spawned = UnityEngine.Object.Instantiate(
            agentData.objectToPlace,
            agentData.placeLocation.position,
            Quaternion.identity,
            agentData.placeLocation
            );

        return true;
    }
}

[Serializable]
class Spray : UseFunctionality<CanSpray.Data>
{
    public override bool Use(CanSpray.Data agentData, Action cb)
    {
        if (!agentData.canSpray || !agentData.hasRequiredItem) return false;
        Debug.Log($"Item: Spraying Item {agentData} ");

        if (agentData.sprayDestination.preventContact) return false;
        agentData.sprayDestination.MakeContact();

        return true;

    }
}

[Serializable]
class Unlock : UseFunctionality<CanUnlock.Data>
{
    public override bool Use(CanUnlock.Data agentData, Action cb)
    {
        if (!agentData.hasRequiredItem) return false;
        Debug.Log($"Item: Spraying Item {agentData} ");

        return true;

    }
}
