using Sirenix.OdinInspector;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [field: SerializeField] public Item item { get; private set; }

    public void PickedUp()
    {
        Destroy(gameObject);
    }
}