using System;
using UnityEngine;

[Serializable]
public class WeightedAction 
{
    [SerializeReference] public ActionDo action;
    public float chance = 1;
    
    public WeightedAction()
    {
        chance = 0;
    }

    public WeightedAction(ActionDo action, float chance)
    {
        this.action = action;
        this.chance = chance;
    }

}
