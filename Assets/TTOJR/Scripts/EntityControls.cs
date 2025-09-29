using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using DependencyInjection;
using System.Collections.Generic;

[DefaultExecutionOrder(-500)]
public class EntityControls : MonoBehaviour, IDependencyProvider
{
    public const int INVENTORY_NUMS = 5;
    [Provide]
    public EntityControls Provide()
    {
        print("Individual DI: Providing EntityControls");
        return this;
    }

    IA_PLAYER IA;

    [field: SerializeField] public GameObject headDirection { get; private set; }
    [field:SerializeField] public GameObject bodyDirection { get; private set; }

    public InputAction ia_move;
    public Func<Vector2> move;

    public InputAction ia_look;
    public Func<Vector2> look;

    public InputAction ia_interact;
    public Action interact;

    public InputAction[] ia_inventoryNums = new InputAction[INVENTORY_NUMS];
    public Action<int>[] intentoryNums = new Action<int>[INVENTORY_NUMS];

    private void Awake()
    {
        IA = new IA_PLAYER();
        IA.Enable();
    }


    private void OnEnable()
    {
        //print("on enable");
        ia_move = IA.Player.Move;
        ia_move.Enable();
        move = () => ia_move.ReadValue<Vector2>();

        ia_look = IA.Player.Look;
        ia_look.Enable();
        look = () => ia_look.ReadValue<Vector2>();

        ia_interact = IA.Player.Interact;
        ia_interact.Enable();
        ia_interact.performed += ctx =>
        {
            print("Player pressed INTERACT");
            interact?.Invoke();
        };
        interact = () => { };

        ia_inventoryNums[0] = IA.Player._1;
        ia_inventoryNums[1] = IA.Player._2;
        ia_inventoryNums[2] = IA.Player._3;
        ia_inventoryNums[3] = IA.Player._4;
        ia_inventoryNums[4] = IA.Player._5;
        
        for(int i = 0; i < INVENTORY_NUMS; i++)
        {
            int indx = i;
            ia_inventoryNums[indx].Enable();
            ia_inventoryNums[indx].performed += ctx =>
            {
                print($"Player pressed {indx}");
                intentoryNums[indx]?.Invoke(indx);
            };
        }
    }

    private void OnDisable()
    {
        move = null;
        ia_move.Disable();

        look = null;
        ia_look.Disable();

        interact = null;
        ia_interact.Disable();

        for (int i = 0; i < INVENTORY_NUMS; i++)
        {
            ia_inventoryNums[i].Disable();
            ia_inventoryNums[i].performed -= ctx =>
            {
                print($"Player pressed {i}");
                intentoryNums[i]?.Invoke(i);
            };
        }
    }
}
