using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class ItemVariationData 
{
    public virtual ItemVariationData Clone()
        => (ItemVariationData)CloneUtility.DeepClonePolymorph(this);
    public abstract void Reset();
    public abstract ItemVariationData UpdateValueThenGet(ItemVariationData newVariationData = null);
}

[Serializable]
public sealed class RequiredItem : ItemVariationData
{
    public override void Reset() => SetHasRequiredItem(val: false);
    [field: SerializeField] public bool hasRequiredItem { get; private set; }
    [field: SerializeField] public Item requiredItem { get; set; }
    public void SetHasRequiredItem(bool val) => hasRequiredItem = val;

    public override ItemVariationData UpdateValueThenGet(ItemVariationData newVariationData = null)
    {
        RequiredItem newData = (RequiredItem)newVariationData;
        hasRequiredItem = newData.hasRequiredItem;
        return this;
    }
}


 [Serializable]
public sealed class Uses : ItemVariationData
{
    [field: SerializeField] public Inventory inv { get; set; }
    [field: SerializeField] public bool usedUp { get; set; }
    [field: SerializeField] public int uses { get; set; }
    [field: SerializeField] public int initialUses { get; set; }

    public override void Reset()
    {
        usedUp = false;
        uses = initialUses;
    }

    public override ItemVariationData UpdateValueThenGet(ItemVariationData newVariationData = null)
    {
        Use();
        return this;
    }

    public void Use()
    {
        Debug.Log($"Item: VARIATION Uses item used, current uses: {uses}.");
        uses--;
        if (uses <= 0) RunOutOfUses();
    }    
    public void RunOutOfUses()
    { 
        usedUp = true;
        inv.RemoveCurrentSelectedItem();
        Debug.Log("Item: VARIATION Uses out of uses");
    }
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
    public IItemFunctionality Clone();
    [field: SerializeReference] public object Data { get; set; }
}

[Serializable]
public abstract class ItemFunctionality<T> : IItemFunctionality
{
    public abstract bool VariantsAllowUse();
    public abstract bool CanUse_ThenUse(UnityEvent callback = null);
    public abstract void UpdateFunctionalityData(object newData);
    [field: SerializeReference] public List<ItemVariationData> variations { get; set; }
    public abstract T data { get; set; }
    public object Data { get => (T)data; set => data = (T)value; }
    public virtual IItemFunctionality Clone()
    {
        var cloneObj = (IItemFunctionality)Activator.CreateInstance(GetType());
        var typed = (ItemFunctionality<T>)cloneObj;
        typed.data = CloneUtility.DeepClone(data);

        typed.variations = variations?.
            Select(v => v?.Clone())
            .ToList();
        
        return typed;
    }
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
        public GameObject locationDetector;
        public void SetPlaceLocation(Transform val) => placeLocation = val;
        public void SetObjectToPlace(GameObject val) => objectToPlace = val;
        public void SetLocationDetector(GameObject val) => locationDetector = val;

    }

    public override bool VariantsAllowUse()
    {
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
            Debug.LogWarning($"Item: Variant Uses not found on item {this.GetType()}"); 
            return false;
        }

        Debug.Log($"Item: Attempting to use {GetType()}");

        //Check if the variations stop the item use
        if (requiredItem.hasRequiredItem || !usesItem.usedUp) return true;
        return false;
    }

    public override bool CanUse_ThenUse(UnityEvent callback = null)
    {
        if (!VariantsAllowUse()) return false;

        Debug.Log($"Item: Successfully Using {GetType()}");
        Uses usesItem = null;
        for (int i = 0; i < variations.Count; i++)
            if (variations[i] is Uses foundUsesItem)
                usesItem = foundUsesItem;
        //Variation Utilization
        usesItem.Use();

        //Functionality Utilization
        callback?.Invoke();
        GameObject spawned = UnityEngine.Object.Instantiate(
            data.objectToPlace,
            data.placeLocation.position,
            Quaternion.identity,
            data.placeLocation
            );

        if(data.locationDetector != null)
            data.locationDetector.SetActive(false);

        return true;
    }

    public override void UpdateFunctionalityData(object input)
    {
        var newData = (Placeable.Data)input;
        data.SetObjectToPlace(newData.objectToPlace);
        data.SetPlaceLocation(newData.placeLocation);
        data.SetLocationDetector(newData.locationDetector);
    }
}

[Serializable]
class DestinationUser : ItemFunctionality<DestinationUser.Data>
{
    [field: SerializeReference] public override Data data { get; set; }
    [Serializable]
    public class Data
    {
        [field:SerializeField] public Destination useDestination { get; set; }
        public void SetUseLocation(Destination val) => useDestination = val;
    }

    public override bool VariantsAllowUse()
    {
        //Variations
        RequiredItem requiredItem = null;

        //Getting the variations
        for (int i = 0; i < variations.Count; i++)
        {
            if (variations[i] is RequiredItem foundReqItem)
                requiredItem = foundReqItem;
        }

        //Check if we even have the variants
        if (requiredItem == null)
        {
            Debug.LogWarning($"Item: Variant RequiredItem not found on item {this.GetType()}");
            return false;
        }

        Debug.Log($"Item: Attempting to use {GetType()}");

        //Check if the variations stop the item use
        if (requiredItem.hasRequiredItem) return true;
        return false;
    }

    public override bool CanUse_ThenUse(UnityEvent callback = null)
    {
        if (!VariantsAllowUse()) return false;

        Debug.Log($"Item: Successfully Using {GetType()}");

        //Variation Utilization


        //Functionality Utilization
        callback?.Invoke();
        if (!data.useDestination.preventContact)
            data.useDestination.MakeContact();

        return true;
    }

    public override void UpdateFunctionalityData(object input)
    {
        var newData = (DestinationUser.Data)input;
        data.SetUseLocation(newData.useDestination);
    }
}
