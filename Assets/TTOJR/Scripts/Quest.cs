using System.Linq;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using static Questing.Hotel;
using static Questing.Town;
using static Questing.Activity;
using Extensions;


public static class Questing
{
    public enum Type
    {
        None,
        TOWN,
        HOTEL,
        HOTELACTIVITY,
    }

    public abstract class QuestType
    {
        public abstract Enum quest { get; set; }
    }


    [Serializable]
    public class Town : QuestType
    {
        public enum TownQuest
        {
            None,
            MANIA_OF_INJUSTICE,
            CORRUPTED_ROOTS,
            SACRIFICIAL_LAMBS
        }
        TownQuest _quest { get; set; }
        public static int size { get => EnumEX<TownQuest>.Size(); }
        public static int[] progressionLengths = { 0, 3, 3, 3 };
        public override Enum quest 
        { 
            get => _quest;
            set => _quest = value is TownQuest q ? TownQuest.None : default;
        }

        public Town(TownQuest _quest)
        {
           quest = _quest;
        }
    }

    [Serializable]
    public class Hotel : QuestType
    {
        public enum HotelQuest
        {
            None,
            THE_DEVILS_NUMBER,
            BLOOD_IN_THE_WATER,
            SOLUS_IMMUNIS
        }
        HotelQuest _quest;
        public static int size { get => EnumEX<HotelQuest>.Size(); }
        public static int[] progressionLengths = { 0, 3, 3, 3 };
        public override Enum quest
        {
            get => _quest;
            set => _quest = value is HotelQuest q ? q : HotelQuest.None;
        }

        public Hotel(HotelQuest q)
        {
            quest = q;
        }
    }

    [Serializable]
    public class Activity : QuestType
    {
        public enum ActivityQuest
        {
            None,
            CLEANROOM,
            REPAIRELECTRICITY,
        }
        ActivityQuest _quest;
        public static int size { get => EnumEX<ActivityQuest>.Size(); }
        public static int[] progressionLengths = { 0, 3, 3, 3 };
        public override Enum quest
        {
            get => _quest;
            set => _quest = value is ActivityQuest q ? q : ActivityQuest.None;
        }

        public Activity(ActivityQuest q)
        {
            quest = q;
        }
    }


}



[Serializable]
public class Quest
{
    #region Privates

    #endregion

    [Serializable]
    public class ProgressionEvent
    {
        public bool compeleted;
        public UnityEvent completeEvent;
        public ProgressionEvent()
        {
            compeleted = false;
        }
        public void Complete()
        {
            compeleted = true;
            completeEvent?.Invoke();
        }
    
    }

    public bool active = false;
    public Questing.Type type = Questing.Type.None;
    public Questing.QuestType quest;

    [SerializeReference] public ProgressionEvent[] progression;

    public Quest(Questing.Type _type)
    {
        active = false;
        type = _type;
    }

    public Quest WithTownQuest(TownQuest q)
    {
        quest = new Questing.Town(q);
        return this;
    }

    public Quest WithHotelQuest(HotelQuest q)
    {
        quest = new Questing.Hotel(q);
        return this;
    }

    public Quest WithActivity(ActivityQuest q)
    {
        quest = new Questing.Activity(q);
        return this;
    }

    public Quest WithProgression(ProgressionEvent[] _progression)
    {
        progression = _progression;
        return this;
    }

    public static T GetRandomQuest<T>(out int index) where T : Enum
    {
        Array values = Enum.GetValues(enumType: typeof(T));
        index = Random.Range(0, maxExclusive: values.Length);
        T type = (T)values.GetValue(index);
        return type;
    }

    public bool isComplete { get => (progression.Last().compeleted == true); }
    public int currentProggressLevel
    {
        get
        {
            int index = Array.FindIndex(progression, p => p.compeleted == true);
            return (index == -1) ? 0 : index;
        }
    }

    public void Activate()
    {
        active = true;
    }

    #region Methods

    #endregion
}




