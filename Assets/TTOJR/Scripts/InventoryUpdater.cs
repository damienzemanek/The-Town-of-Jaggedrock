using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Detector))]
public class InventoryUpdater : MonoBehaviour
{
    Detector detector;
    public bool functionalityDataUpdate;
    public bool variationDataUpdate;

    private void Awake()
    {
        detector = gameObject.GetComponent<Detector>();
    }

    [field:SerializeReference] public IItemFunctionality[] itemDataPhases;

    public void UpdateItem(int phase)
    {
        IItemFunctionality interactItemFunctionality = itemDataPhases[phase];
        Inventory inv = detector.casterObject.gameObject.GetComponent<Inventory>();
        print(inv);
        Item invItem = inv.pickedUpItems.FirstOrDefault
            (itm => itm.functionality.GetType() == interactItemFunctionality.GetType());
        print(invItem.Name);

        if (functionalityDataUpdate)
            invItem.functionality.UpdateFunctionalityData(interactItemFunctionality.Data);

        if (variationDataUpdate)
        {
            print(invItem.functionality.variations.Count);
            for (int i = 0; i < invItem.functionality.variations.Count; i++)
            {
                print($"variation {i} {invItem.functionality.variations[i]}");
                invItem.functionality.variations[i] = interactItemFunctionality.variations[i];

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
        print($"Inventory: UPDATER using item {invItem.Name}");
        invItem.functionality.CanUse_ThenUse();
    }
}
