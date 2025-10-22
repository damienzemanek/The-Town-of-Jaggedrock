using System;
using System.Linq;
using System.Text.RegularExpressions;
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
        RundownHouse
    }

    public string[] locations; //Display Names

    private void Awake() => SetLocs();
    private void OnValidate() => SetLocs();
    static string ConvertLocEnumToFormattedString(Locations loc) =>
        Regex.Replace(loc.ToString(), "([a-z])([A-Z])", "$1 $2");

    void SetLocs() => locations = Enum.GetValues(typeof(Locations))
                        .Cast<Locations>()
                        .Select(ConvertLocEnumToFormattedString)
                        .ToArray();


    public string RandLoc { get => GetRandomLocation(); }
    public string RandLocExcludeHotel { get => GetRandomLocationExclude(Locations.Hotel).ToString(); }


    public string GetRandomLocation() => locations[Random.Range(0, locations.Length)];

    public Locations GetRandomLocationExcludeHotel() => GetRandomLocationExclude(Locations.Hotel);

    public Locations GetRandomLocationExclude(Locations exclude)
    {
        var include = Enum.GetValues(typeof(Locations))
            .Cast<Locations>()
            .Where(loc => loc != exclude)
            .ToArray();

        return include[Random.Range(0, include.Length)];
    }
}
