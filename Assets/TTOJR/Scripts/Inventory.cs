using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Interactor))]
public class Inventory : MonoBehaviour, IDependencyProvider
{
    [Inject] MainCamera cam;
    [BoxGroup(group: "Runtime")][field: SerializeReference] public List<Item> pickedUpItems { get; set; }

    [BoxGroup(group: "Runtime")][SerializeField] GameObject displayPickup;
    [BoxGroup(group: "Runtime")][SerializeField] int selectItem;
    [BoxGroup(group: "Runtime")][SerializeField] Pickup potentialItem;
    [BoxGroup(group: "Runtime")][SerializeField] public bool canPickup { get; private set; }

    [Inject] EntityControls controls;
    Interactor interactor;

    [BoxGroup(group: "UI")][field: SerializeField] public GameObject gridParent { get; private set; }
    [BoxGroup(group: "UI")][field: SerializeField] public GameObject inventorySlotPrefab { get; private set; }
    [BoxGroup(group: "UI")][field: SerializeField] public Sprite emptyIcon { get; private set; }
    [Provide] public Sprite ProvideEmptyIcon()
    {
        return emptyIcon;
    }

    [BoxGroup(group: "UI")][SerializeField] int slotCount;

    private void Awake()
    {
        interactor = GetComponent<Interactor>();
    }

    private void OnEnable()
    {
        controls.interact += Interact;

        for (int i = 0; i < EntityControls.INVENTORY_NUMS; i++)
        {
            controls.intentoryNums[i] += SelectItem;
        }

        interactor.RaycasterEvent += PickupRaycast;
        interactor.FailedRaycast += PickupRaycastFailed;
    }

    private void OnDisable()
    {
        controls.interact -= Interact;

        interactor.RaycasterEvent -= PickupRaycast;
        interactor.FailedRaycast -= PickupRaycastFailed;
    }

    private void Start()
    {
        if (displayPickup == null) Debug.LogError("No DisplayPickup set");

        displayPickup.SetActive(false);
        CreateInventorySlots();
    }

    void PickupRaycast(Ray ray, RaycastHit hit)
    {
        if (!hit.transform.gameObject.GetComponent<Pickup>()) return;
        //print("Pickup raycast");
        Debug.DrawLine(ray.origin, hit.point, Color.green);
        TogglePickup(true);
        potentialItem = hit.transform.gameObject.GetComponent<Pickup>();
    }

    void PickupRaycastFailed()
    {
        TogglePickup(false);
    }

    void TogglePickup(bool val)
    {
        //print($"toggling pickup {val}");
        potentialItem = null;
        canPickup = val;
        displayPickup.SetActive(val);
    }

    void Interact()
    {
        Place();
        if (canPickup)
            Pickup();
    }

    void Pickup()
    {
        Item newItem = potentialItem.item;
        pickedUpItems.Add(newItem);
        int newIndex = pickedUpItems.IndexOf(item: newItem);
        print($"Inv: New item index {newIndex}");
        SetDisplayItem(newItem, newIndex);
        if (pickedUpItems.Count == 1)
            SelectItem(newIndex);

        print("Inv: Picking up item");
        object newData = newItem.functionality.Data;
        if (newData == null) Debug.LogError("Item SO functionality not set (null)");

        potentialItem.PickedUp();
    }

    void Place()
    {
        if (canPickup) return;

        Item itemBeingPlaced = null;

        if (pickedUpItems.Count > selectItem)
            itemBeingPlaced = pickedUpItems[selectItem];
        else return;
        print("Inv: has an item that can be selected");
    }

    void CreateInventorySlots()
    {
        for(int i = 0; i < slotCount; i++)
        {
            InventorySlot slot = Instantiate(inventorySlotPrefab, gridParent.transform.position, Quaternion.identity, gridParent.transform)
                .GetComponent<InventorySlot>();
            slot.Init();
        }
    }

    void SetDisplayItem(Item item, int i)
    {
        print("Inv: Setting new display item");
        gridParent.transform.GetChild(i).GetComponent<InventorySlot>().SetSlot(item);
    }
    
    void DisplayItem(int i)
    {
        gridParent.transform.GetChild(i).GetComponent<InventorySlot>().Select();
        selectItem = i;
    }

    void SelectItem(int num)
    {
        UnselectAllItems();
        print($"Inventory: Selecting Item {num}");
        DisplayItem(num);
    }

    void UnselectAllItems()
    {
        gridParent.GetComponentsInChildren<InventorySlot>(true)
        .ToList()
        .ForEach(s => s.Unselect());
    }

    public void RemoveItem(Item item)
    {
        pickedUpItems.Remove(item);
    }

    void RemoveCurrentSelectedItem()
    {
        pickedUpItems.Remove(pickedUpItems[selectItem]);
        gridParent.transform.GetChild(selectItem).GetComponent<InventorySlot>().ResetSlot();

    }

    public Item GetCurrentItem()
    {
        if (pickedUpItems.Count <= 0) return null;
        if (pickedUpItems.Count < selectItem) return null;
        if (pickedUpItems[selectItem] != null) 
            return pickedUpItems[selectItem];

        return null;
    }


    /////////////
   

}
