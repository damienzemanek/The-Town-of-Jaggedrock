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

public class Inventory : MonoBehaviour, IDependencyProvider
{
    [BoxGroup(group: "Runtime")][SerializeField] Camera cam;
    [BoxGroup(group: "Runtime")][SerializeField] LayerMask itemMask;
    [BoxGroup(group: "Runtime")][SerializeField] float dist;
    [BoxGroup(group: "Runtime")][SerializeField] GameObject displayPickup;
    [BoxGroup(group: "Runtime")][SerializeField] List<Item> pickedUpItems;
    [BoxGroup(group: "Runtime")][SerializeField] int selectItem;
    [BoxGroup(group: "Runtime")][SerializeField] Pickup potentialItem;
    [BoxGroup(group: "Runtime")][SerializeField] bool canPickup;

    [Inject] EntityControls controls;
    [Inject] StateAgent agent;

    [BoxGroup(group: "UI")][field: SerializeField] public GameObject gridParent { get; private set; }
    [BoxGroup(group: "UI")][field: SerializeField] public GameObject inventorySlotPrefab { get; private set; }
    [BoxGroup(group: "UI")][field: SerializeField] public Sprite emptyIcon { get; private set; }
    [Provide] public Sprite ProvideEmptyIcon()
    {
        return emptyIcon;
    }

    [BoxGroup(group: "UI")][SerializeField] int slotCount;



    private void OnEnable()
    {
        controls.interact += Interact;

        for (int i = 0; i < EntityControls.INVENTORY_NUMS; i++)
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
        Place();
        Pickup();
    }

    void Pickup()
    {
        if (!canPickup) return;
        Item newItem = potentialItem.item;
        pickedUpItems.Add(newItem);
        int newIndex = pickedUpItems.IndexOf(newItem);
        SetDisplayItem(newItem, newIndex);
        if (pickedUpItems.Count == 1)
            SelectItem(newIndex);

        print("Inv: Pciking up item");
        var data = newItem.stateAndFunctionality.GetDataValue();

        if (data is Uses)
            SetStates_ThatHaveUses();
        if (data is RequiredItem)
            SetStates_ThatRequireItems(newItem);



        potentialItem.PickedUp();
    }

    void Place()
    {
        Item itemBeingPlaced = null;
        IState state = null;

        if (pickedUpItems.Count > selectItem)
            itemBeingPlaced = pickedUpItems[selectItem];
        else return;
        print("Inv: has an item that can be selected");

        if (itemBeingPlaced != null)
        {
            state = agent.GetState(itemBeingPlaced.stateAndFunctionality);
            print($"Inv: Selected state: {state}");
        }
        else return;

        print("Inv: has a state that coincides with a selectable item");


        if (state != null)
        {
            print("Inv: state is not null");
            TryUseItem(pickedUpItems[selectItem], state.GetDataValue());
        }
        else return;

        print("Inv: can select item");


    }

    void SetStates_ThatHaveUses()
    {
        print("Inv: Updating item with uses");

        var agentStates_CanPlace_SameObject = agent.hashStates
            .Where(s => s.GetDataValue() is Uses u)
            .ForEach(s =>
            {
                var agentData = (Uses)s.GetDataValue();
                agentData.UsesInit();
            });
    }

    void SetStates_ThatRequireItems(Item newItem)
    {
        print("Inv: Updating item that require items");

        var agentStates_CanPlace_SameObject = agent.hashStates
            .Where(s => s.GetDataValue() is RequiredItem r
             && ReferenceEquals(r.requiredItem, newItem))
            .ForEach(s =>
            {
                RequiredItem r = (RequiredItem)s.GetDataValue();
                r.SetHasRequiredItem(true);
                print("Inv: Update success");
            });

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

    void TryUseItem(Item item, object data)
    {
        print("Inv: Trying to use item");
        if(agent.hashStates.TryGetValue(item.stateAndFunctionality, out IState state))
        {
            print($"Inv: Using item {item}, data {data}");
            state.Use(data);
        }
        else
            Debug.Log("Item failed use");

    }


}
