using System.Linq;
using DependencyInjection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
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
        //print(presetItem.type);
        item.type = presetItem.type;
        item.functionality = presetItem.functionality?.Clone();
        item.icon = presetItem.icon;
        item.functionality.variations?.ForEach(v => v.Reset());
        item.canHold = presetItem.canHold;
        item.itemObj = presetItem.itemObj;

        gameObject.layer = 7;
        AssignValuesForCallbackDetector();
    }

    public void PickedUp(Inventory inv)
    {
        print($"Pickuped up item {item.type}");

        //Uses Applying References
        item.functionality.variations?.OfType<Uses>()
            .ToList()
            .ForEach(u => u.inv = inv);

        //Gun functionality apply rerferences
        if(item.functionality is Gun gun)
        {
            Gun.Data data = new Gun.Data();

            if (!inv.gameObject.TryGetComponent<Raycaster>(out Raycaster caster))
                throw new System.Exception("Pickup: (Gun) No Caster found to use Gun");

            if (!inv.gameObject.TryGetComponent<EntityControls>(out EntityControls controls))
                throw new System.Exception("Pickup: (Gun) No controls found to bind to");

            print(controls);

            data.SetCaster(caster);
            data.SetControlsWithGun(controls, (Gun)item.functionality);
            gun.UpdateFunctionalityData(data);
        }

        pickedUpEvent?.Invoke();
        Destroy(gameObject, 0.1f);
    }

    void AssignValuesForCallbackDetector()
    {
        cbDetector.Enter.AddListener(() => interactor.SetInteractText("Pickup (E)"));
        cbDetector.Enter.AddListener(() => interactor.ToggleCanInteract(true));
        cbDetector.Exit.AddListener(() => interactor.ToggleCanInteract(false));
        cbDetector.useCallback.AddListener(() => interactor.ToggleCanInteract(false));
    }

}