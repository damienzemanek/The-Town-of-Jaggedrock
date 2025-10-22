using UnityEngine;

public class Photographer : MonoBehaviour
{
    public LocationRandomizer.Locations locationIWantToPhotograph;
    public LocationRandomizer.Locations locationGivenByPlayerToPhotograph;
#pragma warning disable IDE0052 // Remove unread private members
    [SerializeField] bool givenLoc;
#pragma warning restore IDE0052 // Remove unread private members


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

    public void PlayerGivenPhotographerALocation()
    {
        givenLoc = true;
    }

}
