using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PreRequisiteCallbackDetector))]
public class InventoryUpdater : MonoBehaviour
{
    PreRequisiteCallbackDetector detector;

    public bool variationDataUpdate;
    public bool functionalityDataUpdate;
    [ShowIf("functionalityDataUpdate")] public UnityEvent functionalityUseItemCallback;

    private void Awake()
    {
        detector = gameObject.GetComponent<PreRequisiteCallbackDetector>();
    }

    [Button]
    public void AddItemDataPhase(Item item)
    {
        if(item != null)
            itemDataPhases.Add(item.Clone());
    }

    [InlineEditor] [field:SerializeReference] public List<Item> itemDataPhases;

    public void UpdateItem(int phase)
    {
        print($"Inv: UPDATER attempting item update phase {phase}");
        Item interactItem = itemDataPhases[phase];
        if (detector.casterObject == null) Debug.LogError("Inv: UPDATER detector obj is NULL");
        Inventory inv = detector.casterObject.gameObject.GetComponent<Inventory>();
        print(inv);
        Item invItem = inv.pickedUpItems.FirstOrDefault
            (itm => itm != null && itm.type == interactItem.type);
        if(invItem == null)
        {
            print("Inv: UPDATER no item found of that type, returning");
            return;
        }
        print(invItem.type.ToString());

        if (functionalityDataUpdate)
            invItem.functionality.UpdateFunctionalityData(interactItem.functionality.Data);

        if (variationDataUpdate)
            VariationDataUpdate(invItem, interactItem.functionality);

    }

    public void VariationDataUpdate(Item invItem, IItemFunctionality interactItemFunctionality)
    {
        for (int i = 0; i < invItem.functionality.variations.Count; i++)
        {
            for (int k = 0; k < interactItemFunctionality.variations.Count; k++)
            {
                //For every invItem Variation, check it against every interactItemVariation
                //If its not a match keep searching
                if (invItem.functionality.variations[i].GetType() != interactItemFunctionality.variations[k].GetType())
                    continue;

                //if its a match, Update the value of the (Inv) to the value coordinated by the (interact)
                invItem.functionality.variations[i].UpdateValueThenGet(interactItemFunctionality.variations[k]);
            }
        }
    }

    public void UseItem()
    {
        Inventory inv = detector.casterObject.gameObject.GetComponent<Inventory>();
        print(inv);
        Item invItem = inv.GetCurrentItem();

        if (invItem == null) return;
        if (invItem.functionality.GetType() != detector.lookingForChangesToItem.functionality.GetType()) return;       
        print($"Inventory: UPDATER using item {invItem.type.ToString()} which is a {invItem.functionality.GetType()}");
        invItem.functionality.CanUse_ThenUse(functionalityUseItemCallback);
    }

}
