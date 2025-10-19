using UnityEngine;
using DependencyInjection;
using UnityEngine.AI;
using NUnit;

[RequireComponent(typeof(CallbackDetector))]
public class Dialuage : RuntimeInjectableMonoBehaviour, ICallbackUser
{
    [Inject] EntityControls playerControls;
    //public NPCConversation convo;
    [SerializeField] bool inConvo;

    #region Privs
    CallbackDetector detector;
    NPC_Movement movement;
    NavMeshAgent agent;
    [Inject] Interactor interactor;
    #endregion

    protected override void OnInstantiate()
    {
        base.OnInstantiate();
        detector = GetComponent<CallbackDetector>();
        movement = GetComponent<NPC_Movement>();
        agent = GetComponent<NavMeshAgent>();
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
        Talk();
        DisableMyMovement();
        LookAtWhoImTalkingTo();
        TalkeeLooksAtMe();
    }

    public void Talk()
    {
        //ConversationManager.OnConversationEnded += StopTalking;
        //ConversationManager.Instance.StartConversation(convo);
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
        if (!playerControls.gameObject.TryGetComponent<Look>(out Look look)) this.Error("Look not found on player");
        if (!interactor.transform.TryGetComponent<Inventory>(out Inventory inv)) this.Error("Inventory not found on player");

        playerControls.headDirection.transform.LookAt(transform.position.With(y: 3));
        look.ToggleCursorUsability(true);
        look.ToggleUpdateMouseLooking(false);
        inv.ToggleInventoryVisability(false);
    }

    void StopTalking()
    {
        if (!playerControls.gameObject.TryGetComponent<Look>(out Look look)) this.Error("Look not found on player");
        if (!interactor.transform.TryGetComponent<Inventory>(out Inventory inv)) this.Error("Inventory not found on player");

        look.ToggleCursorUsability(false);
        look.ToggleUpdateMouseLooking(true);
        inv.ToggleInventoryVisability(true);

        movement.enabled = true;
        agent.enabled = true;

        //.OnConversationEnded -= StopTalking;
    }
}
