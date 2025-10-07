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
        inv.interactor.ToggleCanInteract(false);
        Debug.Log("Item: VARIATION Uses out of uses");
    }
    public void UsesInit() { uses = initialUses; usedUp = false; }
}



[Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        none,
        towels,
        cleaningspray,
        deadcroweffigy,
        notebook,
        polaroid,
        revolver,
        key1,
        key2,
        key3,
        key4,
        key5,
        key6,
        key7,
        key8,
        key9,
        key10,
        key11,
        key12,
        key13,
        key14,
        key15,
        key16,
        key17,
        key18,
        key19,
        key20,
    }
    [field:SerializeField] public ItemType type { get; set; }
    public Sprite icon;

    [field: SerializeReference] public IItemFunctionality functionality;

    public bool canHold = false;
    [ShowIf("canHold")] public GameObject itemObj;
    
    public Item Clone(string namesuff = " instance")
    {
        var clone = CreateInstance<Item>();
        clone.name = name + namesuff;
        clone.type = type;
        clone.icon = icon;
        clone.functionality = functionality?.Clone();
        var vars = clone.functionality?.variations;
        vars?.ForEach(v => v.Reset());
        return clone;
    }

}

[field: Serializable]
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
    public abstract bool VariantsAllowUse(out ItemVariationData data);
    public abstract bool VariantsAllowUse(out ItemVariationData data1, out ItemVariationData data2);

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
        [SerializeReference] public Transform placeLocation;
        public GameObject objectToPlace;
        [SerializeReference] public GameObject locationDetector;
        public void SetPlaceLocation(Transform val) => placeLocation = val;
        public void SetObjectToPlace(GameObject val) => objectToPlace = val;
        public void SetLocationDetector(GameObject val) => locationDetector = val;

    }

    public override bool VariantsAllowUse(out ItemVariationData data)
    {
        //Variations
        Uses usesItem = null;

        //Getting the variations
        for (int i = 0; i < variations.Count; i++)
            if (variations[i] is Uses foundUsesItem)
                usesItem = foundUsesItem;

        //Check if we even have the variants
        if (usesItem == null)
        {
            Debug.LogWarning($"Item: Variant Uses not found on item {this.GetType()}");
            data = null;
            return false;
        }

        Debug.Log($"Item: Attempting to use {GetType()}");

        //Check if the variations stop the item use
        if (usesItem.usedUp) { data = null; return false; }

        //Allow (Mutations)
        data = usesItem;
        return true;
    }

    public override bool CanUse_ThenUse(UnityEvent callback = null)
    {
        if (!VariantsAllowUse(out ItemVariationData variData)) return false;
                Debug.Log($"Item: Successfully Using {GetType()}");
        Uses usesItem = (Uses)variData;

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

        Debug.Log("Item: Used Placable Complete");
        return true;
    }

    public override void UpdateFunctionalityData(object input)
    {
        var newData = (Placeable.Data)input;
        data.SetObjectToPlace(newData.objectToPlace);
        data.SetPlaceLocation(newData.placeLocation);
        data.SetLocationDetector(newData.locationDetector);
    }

    public override bool VariantsAllowUse() => throw new NotImplementedException();
    public override bool VariantsAllowUse(out ItemVariationData data1, out ItemVariationData data2) => throw new NotImplementedException();
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
        return true;
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

    public override bool VariantsAllowUse(out ItemVariationData data) => throw new NotImplementedException();
    public override bool VariantsAllowUse(out ItemVariationData data1, out ItemVariationData data2) => throw new NotImplementedException();
}

[Serializable]
class Gun : ItemFunctionality<Gun.Data>
{
    [field: SerializeReference] public override Data data { get; set; }
    [Serializable]
    public class Data
    {

    }
    public override bool VariantsAllowUse(out ItemVariationData variData)
    {
        //Variations
        Uses usesItem = null;

        //Getting the variations
        for (int i = 0; i < variations.Count; i++)
            if (variations[i] is Uses foundUsesItem)
                usesItem = foundUsesItem;

        //Check if we even have the variants
        if (usesItem == null)
        {
            Debug.LogWarning($"Item: Variant Uses not found on item {this.GetType()}");
            variData = null;
            return false;
        }

        Debug.Log($"Item: Attempting to use {GetType()}");

        //Check if the variations stop the item use
        if (usesItem.usedUp) { variData = null; return false; }

        //Allow (Mutations)
        variData = usesItem;
        return true;
    }

    public override bool CanUse_ThenUse(UnityEvent callback = null)
    {
        if (!VariantsAllowUse(out ItemVariationData variData)) return false;
        Uses usesData = (Uses)variData;

        Debug.Log($"Item: Successfully Using {GetType()}");

        //Variation Utilization
        usesData.Use();

        //Functionality Utilization
        callback?.Invoke();

        return true;
    }

    public override void UpdateFunctionalityData(object input)
    {
        var newData = (Gun.Data)input;
    }

    public override bool VariantsAllowUse() => throw new NotImplementedException();
    public override bool VariantsAllowUse(out ItemVariationData data1, out ItemVariationData data2) => throw new NotImplementedException();

}