using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Person", menuName = "ScriptableObjects/Person")]
[Serializable]
public class SO_Person : ScriptableObject
{
    [SerializeField] string _personName;
    public enum CharacterRole
    {
        Town,
        Coven,
        Sherrif,
        LadyInBlack,
        Photographer,
        AssylumEscapee
    }
    public CharacterRole role;

    public string personName { get => _personName; }

    public static List<SO_Person> allPersons;

    private void OnEnable()
    {
        if(allPersons == null) allPersons = new List<SO_Person>();

        if(!allPersons.Contains(this))
            allPersons.Add(this);
    }

    public string GetRandomPersonName()
    {
        var excludeSelf = allPersons.Where(p => p != this).ToList();
        if (excludeSelf.Count == 0) return string.Empty;

        return excludeSelf[Random.Range(0, excludeSelf.Count)].personName;
    }

}
