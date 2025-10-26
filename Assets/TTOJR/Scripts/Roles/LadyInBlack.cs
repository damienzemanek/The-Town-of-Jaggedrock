using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using static LocationRandomizer;
using static Quest;
using Random = UnityEngine.Random;


public class LadyInBlack : RuntimeInjectableMonoBehaviour
{

#region Privates
    [Inject] TimeCycle time;
    [Inject] Despawner despawner;
    LocationRandomizer locations;
    #endregion
    [field: SerializeReference] public List<Quest> townQuests;
    [field: SerializeReference] public List<Quest> hotelQuests;


    #region Class Methods
    protected override void OnInstantiate()
    {
        base.OnInstantiate();
    }

    private void OnEnable()
    {
        if (WontShowUpAtDayAndIsDay()) return;
    }
#endregion


#region Methods
    bool WontShowUpAtDayAndIsDay()
    {
        if (time.IsDay())
        {
            despawner.DisableNPC(gameObject);
            return true;
        }
        return false;
    }

    public void ActivateNewQuest(Quest.Type type)
    {
        if (type == Quest.Type.TOWN)
        {
            int random = Random.Range(0, townQuests.Count);
            if (townQuests[random] == null) this.Error($"Activating a new quest FAILED, did not find one at index {random}");
            townQuests[random].Activate();
        }
        else if (type == Quest.Type.HOTEL)
        {
            int random = Random.Range(0, hotelQuests.Count);
            if (hotelQuests[random] == null) this.Error($"Activating a new quest FAILED, did not find one at index {random}");
            hotelQuests[random].Activate();
        }
        else
            this.Error($"Activating a new quest FAILED, did not find a quest type to activate with given {type}");
    }

    [Button]
    public void CreateNewHotelQuest(Quest.HotelQuest hotelQuest)
    {
        Quest.Type type = Quest.Type.HOTEL;
        int length = hotelQuestProgressionLengths[(int)hotelQuest];

        ProgressionEvent[] newProgression = new ProgressionEvent[length]
            .Populate(() => new ProgressionEvent());

        Quest newQuest = new Quest(type)
            .WithHotelQuest(hotelQuest)
            .WithProgression(newProgression);

        hotelQuests.Add(newQuest);

    }


    [Button]
    public void CreateNewTownQuest(Quest.TownQuest townQuest)
    {
        Quest.Type type = Quest.Type.TOWN;
        int length = townQuestProgressionLengths[(int)townQuest];

        ProgressionEvent[] newProgression = new ProgressionEvent[length]
            .Populate(() => new ProgressionEvent());

        Quest newQuest = new Quest(type)
            .WithTownQuest(townQuest)
            .WithProgression(newProgression);

        townQuests.Add(newQuest);
    }


    public bool hasAnActiveQuest => hasAnActiveTownQuest || hasAnActiveHotelQuest;

    bool hasAnActiveTownQuest => 
        (townQuests.Any(q => q.active));

    bool hasAnActiveHotelQuest =>
        (hotelQuests.Any(q => q.active));

    public Quest currentQuestReferece
    {
        get
        {
            if (hasAnActiveHotelQuest)
                return hotelQuests.FirstOrDefault(q => q.active);

            if (hasAnActiveTownQuest)
                return townQuests.FirstOrDefault(q => q.active);

            return null;
        }

    }

    public Quest.HotelQuest currentHotelQuest => (hasAnActiveHotelQuest)
        ? hotelQuests.First(q => q.active).hotelQuest
        : Quest.HotelQuest.None;

    public Quest.TownQuest currentTownQuest => (hasAnActiveTownQuest)
            ? townQuests.First(q => q.active).townQuest
            : Quest.TownQuest.None;

    public int progressLevelOfCurrentQuest =>
        currentQuestReferece?.currentProggressLevel ?? 0;

    public void IncreaseProgressionOfCurrentQuest()
        => currentQuestReferece.progression.FirstOrDefault(p => p.compeleted == false).Complete();

    #endregion

}
