using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class ItemVariationData 
{
    public abstract void Reset();
}

[Serializable]
public sealed class RequiredItem : ItemVariationData
{
    public override void Reset() => SetHasRequiredItem(val: false);
    [field: SerializeField] public bool hasRequiredItem { get; private set; }
    [field: SerializeField] public Item requiredItem { get; set; }
    public void SetHasRequiredItem(bool val) => hasRequiredItem = val;
}


[Serializable]
public sealed class Uses : ItemVariationData
{
    public override void Reset()
    {
        usedUp = false;
        uses = initialUses;
    }

    [field: SerializeField] public bool usedUp { get; set; }
    [field: SerializeField] public int uses { get; set; }
    [field: SerializeField] public int initialUses { get; set; }
    public void Use()
    {
        Debug.Log($"StateAgent: Uses item used, current uses: {uses}.");
        uses--;
        if (uses <= 0)
            RunOutOfUses();

    }    public void RunOutOfUses() { usedUp = true; }
    public void UsesInit() { uses = initialUses; usedUp = false; }
}



[Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite icon;

   [field: SerializeReference] public IItemFunctionality functionality;

}


public interface IItemFunctionality 
{
    public abstract bool CanUse_ThenUse(UnityEvent callback = null);
    public abstract void UpdateFunctionalityData(object newData);
    [field: SerializeReference] public List<ItemVariationData> variations { get; set; }
    [field: SerializeReference] public object Data { get; set; }
}

[Serializable]
public abstract class ItemFunctionality<T> : IItemFunctionality
{    
    public abstract bool CanUse_ThenUse(UnityEvent callback = null);
    public abstract void UpdateFunctionalityData(object newData);
    [field: SerializeReference] public List<ItemVariationData> variations { get; set; }
    public abstract T data { get; set; }
    public object Data { get => (T)data; set => data = (T)value; }
}


[Serializable]
class Placeable : ItemFunctionality<Placeable.Data>
{
    [field: SerializeReference] public override Data data { get; set; }
    [Serializable]
    public class Data 
    {
        public Transform placeLocation;
        public GameObject objectToPlace;
        public void SetPlaceLocation(Transform val) => placeLocation = val;
        public void SetObjectToPlace(GameObject val) => objectToPlace = val;
    }

    public override bool CanUse_ThenUse(UnityEvent callback = null)
    {
        bool guardReturn = false;

        //Variations
        RequiredItem requiredItem = null;
        Uses usesItem = null;

        //Getting the variations
        for (int i = 0; i < variations.Count; i++)
        {
            if (variations[i] is RequiredItem foundReqItem)
                requiredItem = foundReqItem;

            if (variations[i] is Uses foundUsesItem)
                usesItem = foundUsesItem;
        }

        //Check if we even have the variants
        if (requiredItem == null)
        {
            Debug.LogWarning($"Item: Variant RequiredItem not found on item {this.GetType()}");
            return false;
        }
        if (usesItem == null)
        {
            Debug.LogWarning($"Item: Variant Uses not found on item {this.GetType()}");            return false;
        }

        //Check if the variations stop the item use
        if (!requiredItem.hasRequiredItem || usesItem.usedUp) guardReturn = true;
        if (guardReturn) return false;

        //Variation Utilization
        usesItem.Use();
        callback?.Invoke();

        //Functionality Utilization
        GameObject spawned = UnityEngine.Object.Instantiate(
            data.objectToPlace,
            data.placeLocation.position,
            Quaternion.identity,
            data.placeLocation
            );

        return true;
    }

    public override void UpdateFunctionalityData(object input)
    {
        var newData = (Placeable.Data)input;
        data.SetObjectToPlace(newData.objectToPlace);
        data.SetPlaceLocation(newData.placeLocation);
    }
}

//[Serializable]
//class Spray : UseFunctionality<CanSpray.Data>
//{
//    public override bool Use(CanSpray.Data agentData, Action cb)
//    {
//        if (!agentData.canSpray || !agentData.hasRequiredItem) return false;
//        Debug.Log($"Item: Spraying Item {agentData} ");

//        if (agentData.sprayDestination.preventContact) return false;
//        agentData.sprayDestination.MakeContact();

//        return true;

        //protected override void UpdateStateData(Collider other, CanSpray.Data newData)
        //{
        //    if (!IsInLayer(other)) return;
        //    var agentData = (CanSpray.Data)other.GetComponent<StateAgent>().GetState(enterState).GetDataValue();
        //    agentData.SetCanSpray(newData.canSpray);
        //    agentData.SetSprayLocation(newData.sprayDestination);
        //}
//    }
//}

//[Serializable]
//class Unlock : UseFunctionality<CanUnlock.Data>
//{
//    public override bool Use(CanUnlock.Data agentData, Action cb)
//    {
//        if (agentData.requiredItem.tag != "key") return false;
//        Debug.Log(message: $"Item: Unlocking door w/ Item {agentData} ");

//        if (agentData.doorDestination.preventContact) return false;
//            agentData.doorDestination.MakeContact();

//        return true;
//    }
//}
