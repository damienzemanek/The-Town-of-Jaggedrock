using System;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite icon;
    [ShowInInspector] [SerializeReference] public IState stateAndFunctionality;


}

[Serializable]
public abstract class UseFunctionality<T>
{
    public abstract void Use(T data);
}

[Serializable]
class Place : UseFunctionality<CanPlace.Data>
{
    public override void Use(CanPlace.Data data)
    {
        if (!data.canPlace) return;
        Debug.Log($"PLacing Item {data.objectToPlace.name} ");
    }
}

