using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class ActionChoices
{
    public List<WeightedAction> actions;

    public void DoAnAction(NPC_Area area)
    {
        WeightedAction wa = DetermineActionToExecute();
        wa.action.Execute(area);

        Debug.Log($"ActionChoices: Executing action: {wa.action.GetType().ToString()}");
        
    }

    public WeightedAction DetermineActionToExecute()
    {
        Debug.Log("ActionChoices: Determining Which action to execute");
        if (actions == null || actions.Count == 0) return null; //No actions

        var validActions = actions.Where(a => a.chance > 0).ToList();
        if (validActions.Count == 0) Debug.LogWarning("ActionsChoices: No valid actions with a chance of > 0");

        float total = validActions.Sum(a => a.chance);
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
