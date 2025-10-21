using System.Linq;
using ParadoxNotion.Design;
using UnityEngine;

public class LocationRandomizer : MonoBehaviour
{

    public enum Locations
    {
        Hotel,
        Courthouse,
        Bakery,
        Office,
        Store,
        Farm,
        Library,
        Forest,
        ForestHideout,
        Mansion,
        Park,
        SherrifsOffice,
        RundownBuilding
    }

    public string[] locations =
    {
        "The Hotel",
        "The Courthouse",
        "The Bakery",
        "The Office",
        "The Store",
        "The Farm",
        "The Library",
        "The Forest",
        "The Forest Hideout",
        "The Mansion",
        "The Park",
        "The Sheriff's Office",
        "The Rundown Building"
    };

    public string RandLoc { get => GetRandomLocation(); }
    public string RandLocExcludeHotel { get => GetRandomLocationExclude(Locations.Hotel); }
    //

    public string GetRandomLocation()
    {
        if (locations?.Length > 0 == false) return string.Empty;

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
