using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class ActionChoices
{
    public List<WeightedAction> actions;

    public void DoAnAction()
    {
        WeightedAction wa = DetermineActionToExecute();
        wa.action.Execute();

        Debug.Log($"ActionChoices: Executing action: {wa.action.GetType().ToString()}");
        
    }

    public WeightedAction DetermineActionToExecute()
    {
        Debug.Log("ActionChoices: Determining Which action to execute");
        if (actions == null) return null;
        if(actions.Count == 0 ) return null;

        float total = actions.Sum(a => a.chance);

        if (total <= 0) throw new System.Exception("ActionChoices: Sum is 0. Set the chances");

        float roll = UnityEngine.Random.Range(0, total);

        float atCurrChance = 0f;

        foreach (var action in actions)
        {
            atCurrChance += action.chance;
            if (roll <= atCurrChance)
                return action;
        }

        throw new System.Exception("ActionChoices: No choice found to execute");
    }


}
