using UnityEngine;

public class SprayableDestination : Destination
{
    public override bool preventContact { get => isClean; set => isClean = value; }
    public bool isClean;
    public GameObject dirtyObject;
    public override void MakeContact() => Clean();
    public override void ResetContact() => Dirty();

    private void Start()
    {
        Dirty();
    }

    public void Clean()
    {
        isClean = true;
        dirtyObject.SetActive(false);
    }

    public void Dirty()
    {
        isClean = false;
        dirtyObject.SetActive(value: true);
    }
}
