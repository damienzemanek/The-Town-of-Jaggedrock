using UnityEngine;

public class Photographer : MonoBehaviour
{
    public LocationRandomizer.Locations locationIWantToPhotograph;
    public LocationRandomizer.Locations locationGivenByPlayerToPhotograph;
    [SerializeField] bool givenLoc;


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
