using System.Linq;
using DependencyInjection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CallbackDetector))]
public class Pickup : RuntimeInjectableMonoBehaviour
{
    [Inject] Interactor interactor;
    [field: SerializeReference] public Item presetItem;
    [field: SerializeReference] public Item item { get; private set; }

    public UnityEvent pickedUpEvent;
    CallbackDetector cbDetector;

    protected override void OnInstantiate()
    {
        base.OnInstantiate();
        cbDetector = GetComponent<CallbackDetector>();
        if (cbDetector == null) { Debug.LogError("Pickup: CallbackDetector is missing"); return; }
        if (presetItem == null) { Debug.LogError("Pickup: PresetItem is missing"); return; }
        if (interactor == null) { Debug.LogError("Pickup: Interactor is missing"); return; }


        item = ScriptableObject.CreateInstance<Item>();
        print(presetItem.type);
        item.type = presetItem.type;
        item.functionality = presetItem.functionality?.Clone();
        item.icon = presetItem.icon;
        item.functionality.variations?.ForEach(v => v.Reset());

        gameObject.layer = 7;
        AssignValuesForCallbackDetector();
    }

    public void PickedUp(Inventory inv)
    {
        print($"Pickuped up item {item.type}");
        item.functionality.variations?.OfType<Uses>()
            .ToList()
            .ForEach(u => u.inv = inv);

        pickedUpEvent?.Invoke();
        Destroy(gameObject, 0.1f);
        print("h");
    }

    void AssignValuesForCallbackDetector()
    {
        cbDetector.Enter.AddListener(() => interactor.SetInteractText("Pickup (E)"));
        cbDetector.Enter.AddListener(() => interactor.ToggleCanInteract(true));
        cbDetector.Exit.AddListener(() => interactor.ToggleCanInteract(false));
        cbDetector.useCallback.AddListener(() => interactor.ToggleCanInteract(false));
    }

}