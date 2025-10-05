using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Detector))]
public class InventoryUpdater : MonoBehaviour
{
    public string intention;
    Detector detector;

    public bool variationDataUpdate;
    public bool functionalityDataUpdate;
    [ShowIf("functionalityDataUpdate")] public UnityEvent functionalityUseItemCallback;

    private void Awake()
    {
        detector = gameObject.GetComponent<Detector>();
    }

    [field:SerializeReference] public IItemFunctionality[] itemDataPhases;

    public void UpdateItem(int phase)
    {
        IItemFunctionality interactItemFunctionality = itemDataPhases[phase];
        if (detector.casterObject == null) Debug.LogError("Inv: UPDATER detector obj is NULL");
        Inventory inv = detector.casterObject.gameObject.GetComponent<Inventory>();
        print(inv);
        Item invItem = inv.pickedUpItems.FirstOrDefault
            (itm => itm.functionality.GetType() == interactItemFunctionality.GetType());
        if(invItem == null)
        {
            print("Inv: UPDATER no item found of that type, returning");
            return;
        }
        print(invItem.Name);

        if (functionalityDataUpdate)
            invItem.functionality.UpdateFunctionalityData(interactItemFunctionality.Data);

        if (variationDataUpdate)
            VariationDataUpdate(invItem, interactItemFunctionality);

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

    public void UseItem(int phase)
    {
        IItemFunctionality interactItemFunctionality = itemDataPhases[phase];
        Inventory inv = detector.casterObject.gameObject.GetComponent<Inventory>();
        print(inv);
        Item invItem = inv.GetCurrentItem();

        if (invItem == null) return;
        if (invItem.functionality.GetType() != interactItemFunctionality.GetType()) return;       
        print($"Inventory: UPDATER using item {invItem.Name} which is a {invItem.functionality.GetType()}");
        invItem.functionality.CanUse_ThenUse(functionalityUseItemCallback);
    }

}
