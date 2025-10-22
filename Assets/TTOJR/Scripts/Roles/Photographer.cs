using UnityEngine;

public class Photographer : MonoBehaviour
{
    public LocationRandomizer.Locations locationIWantToPhotograph;


    LocationRandomizer locations;
    private void Awake()
    {
        locations = this.TryGet<LocationRandomizer>();
    }

    private void Start()
    {
        SetNewLocationIWantToPhotograph();
    }


    void SetNewLocationIWantToPhotograph()
    {
        locationIWantToPhotograph = locations.GetRandomLocationExcludeHotel();
    }

}
