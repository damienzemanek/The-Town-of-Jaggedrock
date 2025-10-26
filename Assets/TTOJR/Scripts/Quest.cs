using System.Linq;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;

[Serializable]
public class Quest
{
    #region Privates

    #endregion

    public enum Type
    {
        None,
        TOWN,
        HOTEL
    }
    public enum TownQuest
    {
        None,
        MANIA_OF_INJUSTICE,
        CORRUPTED_ROOTS,
        SACRIFICIAL_LAMBS
    }
    public enum HotelQuest
    {
        None,
        THE_DEVILS_NUMBER,
        BLOOD_IN_THE_WATER,
        SOLUS_IMMUNIS
    }

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


    public static int townQuestsCount { get => Enum.GetValues(typeof(TownQuest)).Length; }
    public static int hotelQuestsCount { get => Enum.GetValues(typeof(HotelQuest)).Length; }

    public static int[] townQuestProgressionLengths = { 0, 3, 3, 3 };
    public static int[] hotelQuestProgressionLengths = { 0, 3, 3, 3 };

    public bool active = false;
    public Type type = Type.None;
    [SerializeField] internal TownQuest townQuest;
    [SerializeField] internal HotelQuest hotelQuest;

    [SerializeReference] public ProgressionEvent[] progression;

    public Quest(Type _type)
    {
        active = false;
        type = _type;
    }

    public Quest WithTownQuest(TownQuest _townQuest)
    {
        townQuest = _townQuest;
        return this;
    }

    public Quest WithHotelQuest(HotelQuest _hotelQuest)
    {
        hotelQuest = _hotelQuest;
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




