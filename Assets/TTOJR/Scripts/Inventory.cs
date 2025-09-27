using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour, IDependencyProvider
{
    [BoxGroup(group: "Runtime")][SerializeField] Camera cam;
    [BoxGroup(group: "Runtime")][SerializeField] LayerMask itemMask;
    [BoxGroup(group: "Runtime")][SerializeField] float dist;
    [BoxGroup(group: "Runtime")][SerializeField] GameObject displayPickup;
    [BoxGroup(group: "Runtime")][SerializeField] List<Item> pickedUpItems;
    [BoxGroup(group: "Runtime")][SerializeField] Pickup potentialItem;
    [BoxGroup(group: "Runtime")][SerializeField] bool canPickup;

    [Inject] EntityControls controls;

    [BoxGroup(group: "UI")] [field: SerializeField] public GameObject gridParent { get; private set; }
    [BoxGroup(group: "UI")] [field: SerializeField] public GameObject inventorySlotPrefab { get; private set; }
    [BoxGroup(group: "UI")] [field: SerializeField] public Sprite emptyIcon { get; private set; }
    [Provide] public Sprite ProvideEmptyIcon()
    {
        return emptyIcon;
    }

    [BoxGroup(group: "UI")] [SerializeField] int slotCount;

   

    private void OnEnable()
    {
        controls.interact += Interact;
        
        for(int i = 0; i < EntityControls.INVENTORY_NUMS; i++)
        {
            controls.intentoryNums[i] += SelectItem;
        }


    }

    private void OnDisable()
    {
        controls.interact -= Interact;
    }

    private void Start()
    {
        if (displayPickup == null) Debug.LogError("No DisplayPickup set");

        displayPickup.SetActive(false);
        CreateInventorySlots();
    }

    private void Update()
    {
        PickupRaycast();
    }

    void PickupRaycast()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, dist, itemMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            TogglePickup(true);
            potentialItem = hit.transform.gameObject.GetComponent<Pickup>();
        }
        else
        {
            potentialItem = null;
            TogglePickup(val: false);
        }

        void TogglePickup(bool val)
        {
            canPickup = val;
            displayPickup.SetActive(val);
        }
    }

    void Interact()
    {
        if (!canPickup) return;
        Item newItem = potentialItem.item;
        pickedUpItems.Add(newItem);
        int newIndex = pickedUpItems.IndexOf(newItem);
        SetDisplayItem(newItem, newIndex);
        potentialItem.PickedUp();

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
        gridParent.transform.GetChild(i).GetComponent<InventorySlot>().SetSlot(item);
    }
    void DisplayItem(int i)
    {
        gridParent.transform.GetChild(i).GetComponent<InventorySlot>().Select();
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
}
