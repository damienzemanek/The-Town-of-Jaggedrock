using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    [field: SerializeReference] public Item presetItem;
    public Item item { get; private set; }

    public UnityEvent pickedUpEvent;

    private void Awake()
    {
        item = ScriptableObject.CreateInstance<Item>();
        item.Name = presetItem.name + " instance";
        item.functionality = presetItem.functionality;
        item.icon = presetItem.icon;
        item.functionality.variations.ForEach(v => v.Reset());
    }

    public void PickedUp()
    {
        pickedUpEvent?.Invoke();
        Destroy(gameObject);
    }
}