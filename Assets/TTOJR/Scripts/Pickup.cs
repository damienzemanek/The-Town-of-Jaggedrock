using System.Linq;
using DependencyInjection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    [field: SerializeReference] public Item presetItem;
    [field: SerializeReference] public Item item { get; private set; }

    public UnityEvent pickedUpEvent;

    private void Awake()
    {
        item = ScriptableObject.CreateInstance<Item>();
        item.type = presetItem.type;
        item.functionality = presetItem.functionality?.Clone();
        item.icon = presetItem.icon;
        item.functionality.variations?.ForEach(v => v.Reset());

        gameObject.layer = 7;
    }

    public void PickedUp(Inventory inv)
    {
        item.functionality.variations?.OfType<Uses>()
            .ToList()
            .ForEach(u => u.inv = inv);

        pickedUpEvent?.Invoke();
        Destroy(gameObject, 0.1f);
    }
}