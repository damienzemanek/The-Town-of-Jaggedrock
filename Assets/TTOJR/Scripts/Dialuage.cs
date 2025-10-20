using UnityEngine;
using DependencyInjection;
using UnityEngine.AI;
using NUnit;
using NodeCanvas.Framework;
using NodeCanvas.DialogueTrees;
using System;

[RequireComponent(typeof(CallbackDetector))]
public class Dialuage : RuntimeInjectableMonoBehaviour, ICallbackUser
{
    [Inject] EntityControls playerControls;
    [Inject] Interactor interactor;

    [SerializeField] bool inConvo;

    #region Privs
    CallbackDetector detector;
    NPC_Movement movement;
    NavMeshAgent agent;
    DialogueTreeController dialaugeController;
    #endregion

    protected override void OnInstantiate()
    {
        base.OnInstantiate();
        detector = this.TryGet<CallbackDetector>();
        movement = this.TryGet<NPC_Movement>();
        agent = this.TryGet<NavMeshAgent>();
        dialaugeController = this.TryGet<DialogueTreeController>();
        AssignValuesForCallbackDetector();
    }

    public void AssignValuesForCallbackDetector()
    {
        detector.Enter.AddListener(() => interactor.SetInteractText("Talk (E)"));
        detector.Enter.AddListener(() => interactor.ToggleCanInteract(true));
        detector.Exit.AddListener(call: () => interactor.ToggleCanInteract(false));
        detector.useCallback.AddListener(() => interactor.ToggleCanInteract(false));
        detector.useCallback.AddListener(DialaugeUsage);
    }

    void DialaugeUsage()
    {
        StartDialauge();
        DisableMyMovement();
        LookAtWhoImTalkingTo();
        TalkeeLooksAtMe();
    }

    void LookAtWhoImTalkingTo()
    {
        transform.LookAtPosThenMyTransform(playerControls.transform.position.With(y: 0))
            .WithEuler(x: 0, z: 0);
    }
    void DisableMyMovement()
    {
        movement.enabled = false;
        agent.enabled = false;
    }

    void TalkeeLooksAtMe()
    {
        Look look = playerControls.TryGet<Look>();
        Inventory inv = playerControls.TryGet<Inventory>();

        playerControls.headDirection.transform.LookAt(transform.position.With(y: 3));
        look.ToggleCursorUsability(true);
        look.ToggleUpdateMouseLooking(false);
        inv.ToggleInventoryVisability(false);
    }

    void OnStopTalking(bool success)
    {
        Look look = playerControls.TryGet<Look>();
        Inventory inv = playerControls.TryGet<Inventory>();

        look.ToggleCursorUsability(false);
        look.ToggleUpdateMouseLooking(true);
        inv.ToggleInventoryVisability(true);

        movement.enabled = true;
        agent.enabled = true;
        inConvo = false;
    }


    void StartDialauge()
    {
        if (dialaugeController == null) this.Error("dialaugeOwner is null");
        inConvo = true;
        dialaugeController.StartDialogue(OnStopTalking);
    }

}
