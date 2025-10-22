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


#pragma warning disable IDE0052 // Remove unread private members
    [SerializeField] bool inConvo;
#pragma warning restore IDE0052 // Remove unread private members
    [SerializeField] SO_Person person;
    [field:SerializeField] public SO_Favor favor { get; private set; }

    public string personName { get => (person != null) ? person.personName : string.Empty; }
    public string randPersonName { get => (person != null) ? person.GetRandomPersonName() : string.Empty; }

    public SO_Person.GroupingTrait myGroupingTrait { get => (person != null) ? person.groupingTrait : SO_Person.GroupingTrait.None; }

    public string personITalkAboutsGroupingTrait { get => (person != null) ? person.GetMyPersonITalkAboutsGroupingTrait() : string.Empty; }
    public string personITalkAboutsName { get => (person != null) ? person.GetMyPersonITalkAboutsName(): string.Empty; }

    public SO_Favor.FavorStatus GetFavorStatus => favor.status;

    #region Privs
    CallbackDetector detector;
    NPC_Movement movement;
    NavMeshAgent agent;
    DialogueTreeController dialaugeController;
    DialaugeChooser dialaugeChooser;
    DialogueActor actor;
    #endregion

    protected override void OnInstantiate()
    {
        base.OnInstantiate();
        detector = this.TryGet<CallbackDetector>();
        movement = this.TryGet<NPC_Movement>();
        agent = this.TryGet<NavMeshAgent>();
        dialaugeController = this.TryGet<DialogueTreeController>();
        dialaugeChooser = this.TryGet<DialaugeChooser>();
        actor = this.TryGet<DialogueActor>();
        AssignValuesForCallbackDetector();
    }

    private void Start()
    {
        AssignDialaugeActorName();
    }

    public void AssignValuesForCallbackDetector()
    {
        detector.Enter.AddListener(() => interactor.SetInteractText("Talk (E)"));
        detector.Enter.AddListener(() => interactor.ToggleCanInteract(true));
        detector.Exit.AddListener(call: () => interactor.ToggleCanInteract(false));
        detector.useCallback.AddListener(() => interactor.ToggleCanInteract(false));
        detector.useCallback.AddListener(DialaugeUsage);
    }

    public void GainMinorFavor() => favor.GainMinorFavor();
    public void GainMajorFavor() => favor.GainMajorFavor();
    public void LoseMinorFavor() => favor.LoseMinorFavor();
    public void LoseMajorFavor() => favor.LoseMajorFavor();

    void DialaugeUsage()
    {
        StartDialauge();
        DisableMyMovement();
        LookAtWhoImTalkingTo();
        TalkeeLooksAtMe();
        FreezeTime(true);
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
    void StartDialauge()
    {
        if (dialaugeController == null) this.Error("dialaugeOwner is null");
        inConvo = true;
        dialaugeController.StartDialogue(StopDialauge);
    }
    void StopDialauge(bool success)
    {
        Look look = playerControls.TryGet<Look>();
        Inventory inv = playerControls.TryGet<Inventory>();

        look.ToggleCursorUsability(false);
        look.ToggleUpdateMouseLooking(true);
        inv.ToggleInventoryVisability(true);

        movement.enabled = true;
        agent.enabled = true;
        inConvo = false;

        FreezeTime(false);
    }
    void AssignDialaugeActorName() => actor.AssignName(input_name: personName);
    void FreezeTime(bool val) => TimeCycle.instance.timeFrozen = val;

}

