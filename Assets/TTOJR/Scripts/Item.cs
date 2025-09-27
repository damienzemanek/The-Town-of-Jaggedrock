using System;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite icon;
    [SerializeReference] public IState stateAndFunctionality;


}

public abstract class UseFunctionality 
{
    public abstract void Use();
}

class Placeable : UseFunctionality
{
    public override void Use()
    {

    }
}

