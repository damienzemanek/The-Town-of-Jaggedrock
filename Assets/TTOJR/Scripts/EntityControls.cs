using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-99)]
public class EntityControls : MonoBehaviour
{
    public static EntityControls Instance;

    IA_PLAYER IA;

    [field:SerializeField] public GameObject head { get; private set; }

    public InputAction ia_move;
    public Func<Vector2> move;

    public InputAction ia_look;
    public Func<Vector2> look;

    private void Awake()
    {
        IA = new IA_PLAYER();
        IA.Enable();
        Instance = this;
    }


    private void OnEnable()
    {
        ia_move = IA.Player.Move;
        ia_move.Enable();
        move = () => ia_move.ReadValue<Vector2>();

        ia_look = IA.Player.Look;
        ia_look.Enable();
        look = () => ia_look.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        move = null;
        ia_move.Disable();


        look = null;
        ia_look.Disable();
    }
}
