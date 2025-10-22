using System;
using System.Linq;
using ParadoxNotion.Design;
using UnityEngine;
using Random = UnityEngine.Random;

public class LocationRandomizer : MonoBehaviour
{

    public enum Locations
    {
        Hotel,
        Sherriffs,
        Park,
        Bakery,
        Store,
        Diner,
        Library,
        Courthouse,
        Mansion,
        Offices,
        Farm,
        Forest,
    }

    public string[] locations; 

    private void Awake() => SetLocs();
    private void OnValidate() => SetLocs();

    void SetLocs() => locations = Enum.GetNames(typeof(Locations));


    public string RandLoc { get => GetRandomLocation(); }
    public string RandLocExcludeHotel { get => GetRandomLocationExclude(Locations.Hotel); }
    //

    public string GetRandomLocation()
    {

        return locations[Random.Range(0, locations.Length)];
    }

    public string GetRandomLocationExclude(Locations exclude)
    {
        if (locations?.Length > 0 == false) return string.Empty;

        var include = locations.Where((loc, index) => index != (int)exclude).ToArray();
        if(include.Length == 0) return string.Empty;

        return locations[Random.Range(0, include.Length)];
    }
}
